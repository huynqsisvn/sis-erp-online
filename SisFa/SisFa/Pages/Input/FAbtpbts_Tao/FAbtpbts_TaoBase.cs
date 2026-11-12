using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SisCom.Model;
using SisCom.Shared;
using SisData.Data;
using SisData.Service;
using SisLib.List;
using SisLib.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace SisFa.Pages.Input.FAbtpbts_Tao
{
    public class FAbtpbts_TaoBase : ListBaseComponent
    {
        // ===================================================
        // BIẾN FILTER
        // ===================================================
        protected DxPopup popFilter;
        protected bool PopupVisible   = false;
        protected int  FilterNam;
        protected int  FilterKy1      = 1;
        protected int  FilterKy2      = 12;
        protected string FilterMaDvcs = "";

        // fbrowseCt lấy từ VBrowse1 split '|' (giống WinForm strArray[1])
        protected string FbrowseCT = "";

        // ===================================================
        // BIẾN LƯỚI PH + CT
        // ===================================================
        protected DataTable TblPH     = null;
        protected DataTable TblCT     = null;
        protected DataTable TblCTFull = null;  // toàn bộ CT — lọc theo dòng PH đang chọn
        protected bool ShowLoaddingVoucher = false;
        protected FrmBrowse3Component oBrowsePH;
        protected FrmBrowseComponent oBrowseCT;
        // ===================================================
        // BIẾN BÁO CÁO IN (F7) — dùng ReportOptionComponent giống Zcdmslts
        // ===================================================
        protected bool IsShowReport = false;
        protected List<FAbtpbts_TaoReportModel> LSReportData;

        // ===================================================
        // BIẾN DIALOG TẠO BÚT TOÁN
        // ===================================================
        protected bool ShowDlgTaoBt       = false;
        protected TaoBtModel DlgTaoBt     = new();
        protected bool DlgLoading         = false;
        protected string DlgSoCt2         = "";   // Số CT đến (khi chọn nhiều dòng)

        // ===================================================
        // BIẾN CONFIRM XÓA
        // ===================================================
        protected bool ShowConfirmDelete  = false;

        // ===================================================
        // BIẾN CONFIRM GHI ĐÈ (khi đã có bút toán)
        // ===================================================
        protected bool ShowConfirmOverwrite = false;
        protected string OverwriteInfo      = "";

        // ===================================================
        // KHỞI TẠO
        // ===================================================
        protected override void OnInitialized()
        {
            mPage        = "fabtpbts_tao";
            sqlTableName = "ctpbtskc";
            sqlTableView = "ctpbtskc";
            SqlTableKey  = "stt_rec";
            SqlItemName  = "stt_rec";
            IsShowCopy   = false;
            IsShowImport = false;

            if (string.IsNullOrEmpty(Menu_id))
                Menu_id = "wf.ct.09";

            FilterNam = DateTime.Now.Year;
            FilterKy1 = DateTime.Now.Month;
            FilterKy2 = DateTime.Now.Month;

            base.OnInitialized();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Language();
                await PageLoad();

                string menuid = Menu_id.Replace("_", ".");
                if (curUser.Is_admin == 1 || curUser.CheckRight(menuid, "VIEW"))
                {
                    myStateMN.SetToolBar(new MyToolbar("toolbar", true));
                    myStateMN.SetToolBar(new MyToolbar("New",     true,  "F4"));
                    myStateMN.SetToolBar(new MyToolbar("Copy",    false));
                    myStateMN.SetToolBar(new MyToolbar("Import",  false));
                    myStateMN.SetToolBar(new MyToolbar("Edit",    false));
                    myStateMN.SetToolBar(new MyToolbar("Save",    false));
                    myStateMN.SetToolBar(new MyToolbar("Delete",  true,  "F8"));
                    myStateMN.SetToolBar(new MyToolbar("Change",  false));
                    myStateMN.SetToolBar(new MyToolbar("View",    false));
                    myStateMN.SetToolBar(new MyToolbar("Aproval", false));
                    myStateMN.SetToolBar(new MyToolbar("Print",   true,  "F7"));
                    myStateMN.SetToolBar(new MyToolbar("Export",  true));
                    myStateMN.SetToolBar(new MyToolbar("Search",  true,  "Ctrl+F"));
                    myStateMN.SetToolBar(new MyToolbar("Quit",    false));
                }
                else
                {
                    myStateMN.SetToolBar(new MyToolbar("toolbar", false));
                }

                myStateMN.OnActionTaskChanged += OnActionTaskchanged;
                myStateMN.OnLangguaChanged    += OnLangguachange;
                myStateMN.OnDvcsChanged       += OnDvcsChangedHandler;

                var dvcs = myStateMN.GetMa_Dvcs();
                FilterMaDvcs = (dvcs == "ALL" || string.IsNullOrEmpty(dvcs)) ? "" : dvcs.Trim();

                PopupVisible = true;
                StateHasChanged();
            }
            else
            {
            }
        }

        // ===================================================
        // PAGELOAD — fbrowse và FbrowseCT lấy từ DB
        // WinForm: M_LAN=="V" → VBrowse1, else → EBrowse1, split '|' → [0]=PH, [1]=CT
        // ===================================================
        public override async Task PageLoad()
        {
            try
            {
                if (myCommand == null) return;
                curCommand = await myCommand.GetSubMenuBymenuId(Menu_id.Replace("_", "."));
                if (curCommand != null)
                {
                    // WinForm: M_LAN=="V" ? VBrowse1 : EBrowse1
                    string browse = IsLangEn ? curCommand.Ebrowse1 : curCommand.Vbrowse1;
                    if (!string.IsNullOrWhiteSpace(browse))
                    {
                        var parts = browse.Split('|');
                        fbrowse   = parts.Length > 0 ? parts[0].Trim() : "";
                        FbrowseCT = parts.Length > 1 ? parts[1].Trim() : "";
                    }
                    string ps = myOption.GetOptionsValue("M_GRID_NROW");
                    Page_size = string.IsNullOrEmpty(ps) ? 50 : int.Parse(ps.Trim());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("FAbtpbts_Tao PageLoad error: " + ex.Message);
            }
        }

        public override async Task Loaddata()
        {
            ShowLoaddingVoucher = true;
            SelectedItem = null;
            TblPH = null;
            TblCT = null;
            TblCTFull = null;
            StateHasChanged();

            try
            {
                string spName  = "FAbtpbts_Tao";
                string sBrowse = IsLangEn ? "" : "";
                if (curCommand != null && !string.IsNullOrEmpty(curCommand.Store_proc))
                {
                    var parts = curCommand.Store_proc.Split(';');
                    if (parts.Length > 0 && !string.IsNullOrEmpty(parts[0].Trim()))
                        spName = parts[0].Trim();
                }

                string maDvcs  = FilterMaDvcs ?? "";
                string paramNames  = "@Nam;@ky;@ky2;@Ma_dvcs";
                string paramValues = $"{FilterNam};{FilterKy1};{FilterKy2};{maDvcs}";

                DataSet ds = await myDb.LoadStoreProcedure(spName, paramNames, paramValues);

                if (ds != null && ds.Tables.Count >= 2)
                {
                    // Winform: table2 = Tables[Count-2] (PH), table1 = Tables[Count-1] (CT)
                    DataTable tblPh = ds.Tables[ds.Tables.Count - 2];
                    DataTable tblCt = ds.Tables[ds.Tables.Count - 1];

                    // Đảm bảo cột tag tồn tại trong TblPH với kiểu bool
                    if (!tblPh.Columns.Contains("tag"))
                        tblPh.Columns.Add("tag", typeof(bool));
                    // Khởi tạo giá trị mặc định = false cho tất cả dòng
                    foreach (DataRow r in tblPh.Rows)
                        if (r["tag"] == DBNull.Value) r["tag"] = false;

                    TblPH     = tblPh;
                    TblCTFull = tblCt;
                    TblCT     = tblCt; // hiển thị toàn bộ CT ban đầu

                    if (TblPH != null && TblPH.DefaultView.Count > 0)
                    {
                        SelectedItem = TblPH.DefaultView[0];
                        FilterCtBySelectedPh();
                    }
                }
                else if (ds != null && ds.Tables.Count == 1)
                {
                    DataTable tblPh = ds.Tables[0];
                    if (!tblPh.Columns.Contains("tag"))
                        tblPh.Columns.Add("tag", typeof(bool));
                    foreach (DataRow r in tblPh.Rows)
                        if (r["tag"] == DBNull.Value) r["tag"] = false;
                    TblPH     = tblPh;
                    TblCTFull = new DataTable();
                    TblCT     = TblCTFull;
                }
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao("Lỗi tải dữ liệu: " + ex.Message);
                Console.WriteLine("FAbtpbts_Tao Loaddata error: " + ex.Message);
            }
            finally
            {
                ShowLoaddingVoucher = false;
                StateHasChanged();
            }
        }

        // ===================================================
        // LỌC CT THEO DÒNG PH ĐANG CHỌN (giống WinForm oBrowse_F7 logic)
        // Key filter: ky;ma_dvcs;tk_kh;tk_cp — đọc từ dòng PH đang chọn
        // ===================================================
        protected void FilterCtBySelectedPh()
        {
            try
            {
                if (SelectedItem == null || TblCTFull == null) return;
                var row   = (DataRowView)SelectedItem;
                var phTbl = row.Row.Table;

                if (!TblCTFull.Columns.Contains("ky") || !TblCTFull.Columns.Contains("ma_dvcs")) return;

                // Đọc từ dòng PH (không đọc từ TblCTFull)
                string ky     = phTbl.Columns.Contains("ky")      ? row["ky"]?.ToString()?.Trim()      ?? "" : "";
                string maDvcs = phTbl.Columns.Contains("ma_dvcs") ? row["ma_dvcs"]?.ToString()?.Trim() ?? "" : "";
                string tkKh   = phTbl.Columns.Contains("tk_kh")   ? row["tk_kh"]?.ToString()?.Trim()   ?? "" : "";
                string tkCp   = phTbl.Columns.Contains("tk_cp")   ? row["tk_cp"]?.ToString()?.Trim()   ?? "" : "";

                // Build filter cho TblCTFull
                var parts = new List<string>();
                if (!string.IsNullOrEmpty(ky))     parts.Add($"ky='{ky}'");
                if (!string.IsNullOrEmpty(maDvcs)) parts.Add($"ma_dvcs='{maDvcs.Replace("'", "''")}'");
                if (!string.IsNullOrEmpty(tkKh))   parts.Add($"tk_kh='{tkKh.Replace("'", "''")}'");
                if (!string.IsNullOrEmpty(tkCp))   parts.Add($"tk_cp='{tkCp.Replace("'", "''")}'");

                TblCTFull.DefaultView.RowFilter = parts.Count > 0 ? string.Join(" AND ", parts) : "";
            }
            catch (Exception ex)
            {
                Console.WriteLine("FilterCtBySelectedPh error: " + ex.Message);
            }
        }

        // ===================================================
        // CHỌN DÒNG PH — single-click chỉ set active row + lọc lưới CT
        // ===================================================
        public override void CurItem(object obj)
        {
            if (obj != null)
            {
                SelectedItem = obj;
                // Hàm lọc chi tiết cũ của bạn (chắc hẳn bạn đã viết sẵn hàm này trong code hiện tại)
                FilterCtBySelectedPh();
            }
            else
            {
                SelectedItem = null;
                if (TblCT != null)
                    TblCT.DefaultView.RowFilter = "1=0"; // Ẩn chi tiết nếu không chọn master
            }
            StateHasChanged();
        }

        // ===================================================
        // TOOLBAR ACTIONS
        // ===================================================
        public override void OnActionTaskchanged(ActionTask ac)
        {
            switch (ac)
            {
                case ActionTask.New:
                    // F4 — Tạo bút toán
                    V_TaoBt();
                    break;
                case ActionTask.Delete:
                    // F8 — Xóa bút toán
                    V_XoaBt();
                    break;
                case ActionTask.Print:
                    myStateMN.SetCurrentActionTask(ActionTask.Print);
                    PrintVoucherAsync(); // <--- ĐẢM BẢO PHẢI CÓ DÒNG NÀY ĐỂ GỌI HÀM
                    break;
                case ActionTask.Export:
                    ExportData(1);
                    break;
                case ActionTask.Search:
                    PopupVisible = true;
                    StateHasChanged();
                    break;
            }
        }

        // ===================================================
        // DVCS CHANGED
        // ===================================================
        private async void OnDvcsChangedHandler(string maDvcs)
        {
            await InvokeAsync(async () =>
            {
                // Cập nhật biến ĐVCS mới
                FilterMaDvcs = (maDvcs == "ALL" || string.IsNullOrEmpty(maDvcs)) ? "" : maDvcs.Trim();

                // Reset lưới chi tiết để tránh lỗi hiển thị chéo dữ liệu
                SelectedItem = null;
                if (TblCT != null)
                    TblCT.DefaultView.RowFilter = "1=0";

                // Chạy lại hàm Load danh sách
                await Loaddata();
                StateHasChanged();
            });
        }


        // ===================================================
        // VALIDATE FILTER
        // ===================================================
        protected bool ValidateFilter()
        {
            if (FilterNam <= 0)
            {
                myStateMN.SetNewThongbao("Năm không hợp lệ (phải > 0).");
                return false;
            }
            if (FilterKy1 < 1 || FilterKy1 > 12)
            {
                myStateMN.SetNewThongbao("Từ kỳ phải từ 1 đến 12.");
                return false;
            }
            if (FilterKy2 < 1 || FilterKy2 > 12)
            {
                myStateMN.SetNewThongbao("Đến kỳ phải từ 1 đến 12.");
                return false;
            }
            if (FilterKy1 > FilterKy2)
            {
                myStateMN.SetNewThongbao("Từ kỳ không được lớn hơn Đến kỳ.");
                return false;
            }
            return true;
        }

        // ===================================================
        // LẤY DANH SÁCH DÒNG ĐƯỢC ĐÁNH DẤU (tag = true)
        // Giống WinForm: dataSource.Cast<DataRowView>().Where(x => (bool)x["tag"]).ToArray()
        // ===================================================
        protected List<DataRowView> GetTaggedRows()
        {
            var result = new List<DataRowView>();
            try
            {
                if (TblPH == null) return result;
                foreach (DataRowView rv in TblPH.DefaultView)
                {
                    if (TblPH.Columns.Contains("tag") && rv["tag"] != DBNull.Value && (bool)rv["tag"])
                        result.Add(rv);
                }
            }
            catch { }
            return result;
        }

        // ===================================================
        // F4 — TẠO BÚT TOÁN (newButton_Click trong WinForm)
        // ===================================================
        protected async void V_TaoBt()
        {
            var taggedRows = GetTaggedRows();
            if (taggedRows.Count == 0)
            {
                myStateMN.SetNewThongbao("Phải đánh dấu trước khi thực hiện chức năng này!");
                return;
            }

            // Kiểm tra khóa sổ (giống WinForm newButton_Click)
            try
            {
                string strKhoaSo = myOption.GetOptionsValue("M_NGAY_DONG_SO");
                if (!string.IsNullOrEmpty(strKhoaSo) && DateTime.TryParse(strKhoaSo, out DateTime ngayDongSo))
                {
                    DateTime ngayBatDau = new DateTime(FilterNam, FilterKy1, 1);
                    if (ngayBatDau <= ngayDongSo)
                    {
                        myStateMN.SetNewThongbao("Số liệu đã khóa sổ không kết chuyển được!");
                        return;
                    }
                }
            }
            catch { }

            // Lấy thông tin mặc định từ các dòng đã tag (giống WinForm)
            string sttRecList = string.Join(",",
                taggedRows.Where(r => !string.IsNullOrWhiteSpace(r["stt_rec"]?.ToString()))
                          .Select(r => r["stt_rec"].ToString()));

            // Tìm dòng có so_ct nhỏ nhất (giống WinForm tìm dataRowView1)
            DataRowView firstRow = taggedRows[0];
            foreach (var r in taggedRows)
            {
                string sc = r["so_ct"]?.ToString()?.Trim() ?? "";
                if (sc != "" && sc.CompareTo(firstRow["so_ct"]?.ToString() ?? "") < 0)
                    firstRow = r;
            }

            string ma_qs_default   = firstRow["ma_qs"]?.ToString()?.Trim() ?? "";
            string so_ct_default   = firstRow["so_ct"]?.ToString()?.Trim() ?? "";
            string dien_giai_default = firstRow["dien_giai"]?.ToString()?.Trim() ?? "";

            // Fallback to active item nếu không có
            if (ma_qs_default == "" && SelectedItem != null)
                ma_qs_default = ((DataRowView)SelectedItem)["ma_qs"]?.ToString()?.Trim() ?? "";
            if (dien_giai_default == "" && SelectedItem != null)
                dien_giai_default = ((DataRowView)SelectedItem)["dien_giai"]?.ToString()?.Trim() ?? "";

            // Kiểm tra đã có bút toán chưa (giống WinForm)
            if (!string.IsNullOrEmpty(sttRecList))
            {
                try
                {
                    string sqlChk = $"select stt_rec from ct00 where ma_ct='PK5' AND dbo.InList(stt_rec,'{sttRecList.Replace("'", "''")}',',')=1";
                    DataSet dsChk = await myDb.LoadDataset(sqlChk);
                    if (dsChk?.Tables[0]?.Rows.Count > 0)
                    {
                        OverwriteInfo = sttRecList;
                        ShowConfirmOverwrite = true;
                        // Lưu context tạm để dùng sau khi confirm
                        _pendingMaQs       = ma_qs_default;
                        _pendingSoCt       = so_ct_default;
                        _pendingDienGiai   = dien_giai_default;
                        _pendingTaggedRows = taggedRows;
                        StateHasChanged();
                        return;
                    }
                }
                catch { }
            }

            await OpenDlgTaoBt(ma_qs_default, so_ct_default, dien_giai_default, taggedRows);
        }

        // Context tạm cho confirm overwrite
        private string _pendingMaQs     = "";
        private string _pendingSoCt     = "";
        private string _pendingDienGiai = "";
        protected List<DataRowView> _pendingTaggedRows = new();

        protected async void ConfirmOverwriteAndContinue()
        {
            ShowConfirmOverwrite = false;
            await OpenDlgTaoBt(_pendingMaQs, _pendingSoCt, _pendingDienGiai, _pendingTaggedRows);
        }

        protected void CancelOverwrite()
        {
            ShowConfirmOverwrite = false;
            StateHasChanged();
        }

        private async Task OpenDlgTaoBt(string maQs, string soCt, string dienGiai, List<DataRowView> rows)
        {
            DlgTaoBt = new TaoBtModel
            {
                Ma_qs      = maQs,
                So_ct      = soCt,
                Dien_giai  = dienGiai,
                IsOk       = false
            };
            DlgSoCt2     = "";
            DlgLoading   = false;

            // Lấy so_ct_length từ DB (giống WinForm GetDatabaseFieldLength)
            try
            {
                var lenDs = await myDb.LoadDataset("SELECT MAX(LEN(so_ct)) FROM ctpbtskc");
                if (lenDs?.Tables[0]?.Rows.Count > 0 && lenDs.Tables[0].Rows[0][0] != DBNull.Value)
                    DlgTaoBt.So_ct_length = Math.Max(Convert.ToInt32(lenDs.Tables[0].Rows[0][0]), 10);
            }
            catch { DlgTaoBt.So_ct_length = 10; }

            // Tự động lấy số CT tiếp theo nếu có ma_qs
            if (!string.IsNullOrEmpty(maQs))
                await LoadNewSoct(maQs, rows.Count);

            // Lưu tagged rows để dùng khi OK
            _pendingTaggedRows = rows;
            ShowDlgTaoBt = true;
            StateHasChanged();
        }

        // ===================================================
        // LẤY SỐ CT MỚI (GetNewSoct trong WinForm)
        // ===================================================
        protected async Task LoadNewSoct(string maQs, int rowCount = 1)
        {
            try
            {
                bool autoSoCt = false;
                string optAuto = myOption.GetOptionsValue("M_AUTO_SOCT");
                if (!string.IsNullOrEmpty(optAuto) && Convert.ToInt16(optAuto) == 1)
                    autoSoCt = true;

                string sql = autoSoCt
                    ? $"SELECT transform, so_ct + 1 as so_ct FROM dmqs WHERE ma_qs='{maQs.Trim().Replace("'", "''")}'"
                    : $"EXEC [GetNewSoct] '{maQs.Trim().Replace("'", "''")}'";

                DataSet ds = await myDb.LoadDataset(sql);
                if (ds?.Tables[0]?.Rows.Count > 0)
                {
                    DataRow r = ds.Tables[0].Rows[0];
                    if (r[1] != null && r[1] != DBNull.Value)
                    {
                        DlgTaoBt.So_ct_format = r[0]?.ToString() ?? "";
                        DlgTaoBt.So_ct_num    = Convert.ToDouble(r[1].ToString());
                        DlgTaoBt.So_ct        = string.Format(DlgTaoBt.So_ct_format, DlgTaoBt.So_ct_num)
                                                      .Trim().PadLeft(DlgTaoBt.So_ct_length);
                        if (rowCount > 1)
                            DlgSoCt2 = string.Format(DlgTaoBt.So_ct_format,
                                           DlgTaoBt.So_ct_num + rowCount - 1)
                                              .Trim().PadLeft(DlgTaoBt.So_ct_length);
                        else
                            DlgSoCt2 = "";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoadNewSoct error: " + ex.Message);
            }
            StateHasChanged();
        }

        // ===================================================
        // XỬ LÝ KHI THAY ĐỔI MÃ QS TRONG DIALOG
        // ===================================================
        protected async void OnDlgMaQsChanged(ExpandoObject value)
        {
            if (value == null) return;
            var dict = value as IDictionary<string, object>;
            DlgTaoBt.Ma_qs  = dict?.ContainsKey("ma_qs")  == true ? dict["ma_qs"]?.ToString() ?? "" : "";
            DlgTaoBt.Ten_qs = dict?.ContainsKey("ten_qs") == true ? dict["ten_qs"]?.ToString() ?? "" : "";
            await LoadNewSoct(DlgTaoBt.Ma_qs, _pendingTaggedRows.Count);
        }

        // ===================================================
        // XÁC NHẬN TẠO BÚT TOÁN (OK trong FrmTaobt WinForm)
        // ===================================================
        protected async void DlgTaoBt_OK()
        {
            // Validate giống WinForm ConfirmGridView_OnOk
            if (string.IsNullOrWhiteSpace(DlgTaoBt.Ma_qs))
            {
                myStateMN.SetNewThongbao("Chưa vào mã quyển sổ!");
                return;
            }
            if (string.IsNullOrWhiteSpace(DlgTaoBt.So_ct))
            {
                myStateMN.SetNewThongbao("Chưa vào số chứng từ!");
                return;
            }

            // Kiểm tra số CT trùng (CheckSoct_Trung)
            int trungResult = await CheckSoctTrung(DlgTaoBt.Ma_qs, DlgTaoBt.So_ct);
            if (trungResult == 2)
            {
                myStateMN.SetNewThongbao("Số chứng từ đã tồn tại.");
                return;
            }
            // trungResult == 1: cảnh báo nhưng cho phép nếu user đã biết (tiếp tục)

            DlgLoading = true;
            StateHasChanged();

            try
            {
                // Gọi SP FAbtpbts_Tao_F4 cho từng dòng đã tag (giống WinForm newButton_Click)
                int idx = 0;
                foreach (var dataRowView in _pendingTaggedRows)
                {
                    double num4  = DlgTaoBt.So_ct_num + idx++;
                    string str7  = string.IsNullOrEmpty(DlgTaoBt.So_ct_format)
                        ? DlgTaoBt.So_ct
                        : string.Format(DlgTaoBt.So_ct_format, num4).Trim().PadLeft(DlgTaoBt.So_ct_length);

                    string parNames = "@Nam;@thang;@ma_dvcs;@tk_no;@tk_co;@ma_qs;@so_ct;@ngay_ct;@dien_giai;@User_id";

                    int    namVal  = Convert.ToInt32(dataRowView["nam"].ToString().Trim());
                    int    kyVal   = Convert.ToInt32(dataRowView["ky"].ToString().Trim());
                    string ngayCt  = GetEndOfPeriod(namVal, kyVal).ToString("yyyy-MM-dd");
                    string maDvcs  = dataRowView["ma_dvcs"]?.ToString()?.Trim() ?? "";
                    string tkNo    = dataRowView["tk_cp"]?.ToString()?.Trim() ?? "";
                    string tkCo    = dataRowView["tk_kh"]?.ToString()?.Trim() ?? "";
                    string userId  = curUser?.User_id.ToString() ?? "0";

                    string parVals = $"{namVal};{kyVal};{maDvcs};{tkNo};{tkCo};" +
                                     $"{DlgTaoBt.Ma_qs.Trim()};{str7.Trim()};{ngayCt};" +
                                     $"{DlgTaoBt.Dien_giai};{userId}";

                    await myDb.LoadStoreProcedure("FAbtpbts_Tao_F4", parNames, parVals);
                }

                // Bỏ tag sau khi tạo xong (giống WinForm)
                if (TblPH != null)
                    foreach (var rv in _pendingTaggedRows)
                        if (TblPH.Columns.Contains("tag")) rv["tag"] = false;

                myStateMN.SetNewThongbao("Chương trình đã thực hiện xong!");
                ShowDlgTaoBt = false;
                await Loaddata();
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao("Lỗi tạo bút toán: " + ex.Message);
                Console.WriteLine("DlgTaoBt_OK error: " + ex.Message);
            }
            finally
            {
                DlgLoading = false;
                StateHasChanged();
            }
        }

        protected void DlgTaoBt_Cancel()
        {
            ShowDlgTaoBt = false;
            StateHasChanged();
        }

        // ===================================================
        // KIỂM TRA SỐ CT TRÙNG (CheckSoct_Trung trong WinForm)
        // 0 = không trùng, 1 = cảnh báo, 2 = lỗi
        // ===================================================
        private async Task<int> CheckSoctTrung(string maQs, string soCt)
        {
            try
            {
                // Lấy chkso_ct từ dmqs
                var dsQs = await myDb.LoadDataset($"SELECT chkso_ct FROM dmqs WHERE ma_qs='{maQs.Trim().Replace("'", "''")}'");
                if (dsQs?.Tables[0]?.Rows.Count == 0) return 0;
                string chk = dsQs.Tables[0].Rows[0]["chkso_ct"]?.ToString() ?? "0";
                if (chk == "0") return 0;

                var ds = await myDb.LoadDataset($"SELECT so_ct FROM ctpbtskc WHERE so_ct='{soCt.Trim().Replace("'", "''")}'");
                if (ds?.Tables[0]?.Rows.Count >= 1)
                {
                    if (chk == "1") return 1;
                    if (chk == "2") return 2;
                }
            }
            catch { }
            return 0;
        }

        // ===================================================
        // F8 — XÓA BÚT TOÁN (delButton_Click trong WinForm)
        // ===================================================
        protected void V_XoaBt()
        {
            var taggedRows = GetTaggedRows();
            if (taggedRows.Count == 0)
            {
                myStateMN.SetNewThongbao("Phải đánh dấu trước khi thực hiện chức năng này!");
                return;
            }
            _pendingTaggedRows = taggedRows;
            ShowConfirmDelete = true;
            StateHasChanged();
        }

        protected async void ConfirmDelete()
        {
            ShowConfirmDelete = false;
            try
            {
                // Kiểm tra khóa sổ
                string strKhoaSo = myOption.GetOptionsValue("M_NGAY_DONG_SO");
                if (!string.IsNullOrEmpty(strKhoaSo) && DateTime.TryParse(strKhoaSo, out DateTime ngayDongSo))
                {
                    DateTime ngayBatDau = new DateTime(FilterNam, FilterKy1, 1);
                    if (ngayBatDau <= ngayDongSo)
                    {
                        myStateMN.SetNewThongbao("Số liệu đã khóa sổ không xóa được!");
                        StateHasChanged();
                        return;
                    }
                }

                foreach (var dataRowView in _pendingTaggedRows)
                {
                    int    namVal = Convert.ToInt32(dataRowView["nam"].ToString().Trim());
                    int    kyVal  = Convert.ToInt32(dataRowView["ky"].ToString().Trim());
                    string maDvcs = dataRowView["ma_dvcs"]?.ToString()?.Trim() ?? "";
                    string tkNo   = dataRowView["tk_cp"]?.ToString()?.Trim() ?? "";
                    string tkCo   = dataRowView["tk_kh"]?.ToString()?.Trim() ?? "";
                    DateTime ngay1 = GetStartOfPeriod(namVal, kyVal);
                    DateTime ngay2 = GetEndOfPeriod(namVal, kyVal);

                    // Xóa ct00 (giống WinForm delButton_Click)
                    string sqlDel00 = $"DELETE FROM ct00 WHERE (ngay_ct BETWEEN '{ngay1:yyyy-MM-dd}' AND '{ngay2:yyyy-MM-dd}') " +
                                      $"AND LTRIM(RTRIM(ma_ct))='PK5' " +
                                      $"AND LTRIM(RTRIM(ma_dvcs))='{maDvcs.Replace("'", "''")}'";
                    await myDb.ExecuteSqlQueryAsync(sqlDel00);

                    // Gọi SP FAbtpbts_Tao_F8 (xóa ctpbtskc)
                    string parNames = "@Nam;@thang;@ma_dvcs;@tk_no;@tk_co";
                    string parVals  = $"{namVal};{kyVal};{maDvcs};{tkNo};{tkCo}";
                    await myDb.LoadStoreProcedure("FAbtpbts_Tao_F8", parNames, parVals);
                }

                // Bỏ tag
                if (TblPH != null)
                    foreach (var rv in _pendingTaggedRows)
                        if (TblPH.Columns.Contains("tag")) rv["tag"] = false;

                myStateMN.SetNewThongbao("Xóa thành công!");
                await Loaddata();
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao("Lỗi xóa bút toán: " + ex.Message);
                Console.WriteLine("ConfirmDelete error: " + ex.Message);
            }
            StateHasChanged();
        }

        protected void CancelDelete()
        {
            ShowConfirmDelete = false;
            StateHasChanged();
        }

        // ===================================================
        // F7 — IN / XEM BÁO CÁO (oBrowse_F7 trong WinForm)
        // WinForm: FrmIn { Rows = array }.ShowDialog()
        //   → In(): foreach row → GetData(row["stt_rec"]) → V_In/V_Xem
        //   → GetData(): EXEC [FAbtpbts_Tao_F7] @stt_rec  (truyền thẳng, không check)
        // ===================================================
        protected async void V_In()
        {
            var taggedRows = GetTaggedRows();
            if (taggedRows.Count == 0)
            {
                myStateMN.SetNewThongbao("Phải đánh dấu trước khi thực hiện chức năng này!");
                return;
            }

            try
            {
                DataTable tblResult = null;

                foreach (var row in taggedRows)
                {
                    string sttRec = row["stt_rec"]?.ToString()?.Trim() ?? "";

                    DataSet ds = await myDb.LoadStoreProcedure("FAbtpbts_Tao_F7", "@stt_rec", sttRec);

                    if (ds != null && ds.Tables.Count > 0)
                    {
                        DataTable tblDetail = ds.Tables[0];
                        if (tblResult == null)
                            tblResult = tblDetail.Copy();
                        else
                            tblResult.Merge(tblDetail);
                    }
                }

                if (tblResult == null)
                    tblResult = new DataTable();

                LSReportData = new List<FAbtpbts_TaoReportModel>();
                var reportModel = new FAbtpbts_TaoReportModel
                {
                    MaDVCS   = FilterMaDvcs ?? "",
                    Nam      = FilterNam,
                    Ky       = FilterKy1,
                    Ky2      = FilterKy2,
                    Ngay_ct2 = GetEndOfPeriod(FilterNam, FilterKy2),
                    LsData   = myDb.ConvertDataTable<FAbtpbts_Tao_F7Detail>(tblResult)
                };
                LSReportData.Add(reportModel);

                IsShowReport = true;
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao("Lỗi xem báo cáo: " + ex.Message);
                Console.WriteLine("V_In error: " + ex.Message);
            }
            StateHasChanged();
        }

        protected void CloseReportPopup()
        {
            IsShowReport = false;
            StateHasChanged();
        }

        // ===================================================
        // ĐÁNH DẤU / BỎ DẤU TẤT CẢ (Ctrl+A / Ctrl+U trong WinForm)
        // ===================================================
        protected void SelectAll(bool tag)
        {
            try
            {
                if (TblPH == null || !TblPH.Columns.Contains("tag")) return;
                foreach (DataRowView rv in TblPH.DefaultView)
                    rv["tag"] = tag;
                StateHasChanged();
            }
            catch { }
        }

        // ===================================================
        // FILTER POPUP OK
        // ===================================================
        protected async void BtnFilterOK()
        {
            if (!ValidateFilter()) return;
            PopupVisible = false;
            await Loaddata();
        }

        protected void CloseFilter()
        {
            PopupVisible = false;
            StateHasChanged();
        }

        // ===================================================
        // TIỆN ÍCH TÍNH NGÀY ĐẦU / CUỐI KỲ (NgayTC trong WinForm)
        // ===================================================
        protected static DateTime GetStartOfPeriod(int nam, int ky)
        {
            int m = Math.Max(1, Math.Min(12, ky));
            return new DateTime(nam, m, 1);
        }

        protected static DateTime GetEndOfPeriod(int nam, int ky)
        {
            int m = Math.Max(1, Math.Min(12, ky));
            return new DateTime(nam, m, DateTime.DaysInMonth(nam, m));
        }

        // ===================================================
        // EXPORT
        // ===================================================
        public override async void ExportData(int type)
        {
            if (oBrowsePH != null)
                oBrowsePH.ExPortGrid(type); // Sửa thành oBrowsePH
            myStateMN.SetNewThongbao("Xuất tệp thành công!");
        }

        public class PrintJob
        {
            public string Id { get; set; } = Guid.NewGuid().ToString(); // ID chống trùng lặp
            public string Title { get; set; }
            public List<object> Data { get; set; }
        }

        // 2. Đổi biến PrintQueue sang dùng class mới này
        protected List<PrintJob> PrintQueue = new List<PrintJob>();

        // 3. Nâng cấp hàm In (Lưu ý: nhớ đổi thành async void)
        protected virtual async void PrintVoucherAsync()
        {
            DataRow[] rows = TblPH.Select("tag='true'");
            if (rows == null || rows.Length == 0)
            {
                myStateMN.SetNewThongbao("Chưa chọn dữ liệu để in!");
                return;
            }

            // Dọn dẹp hàng đợi in cũ
            PrintQueue.Clear();

            foreach (DataRow row in rows)
            {
                string sttRec = row.Table.Columns.Contains("stt_rec") ? row["stt_rec"].ToString() : "";
                int nam = row.Table.Columns.Contains("nam") ? Convert.ToInt32(row["nam"]) : DateTime.Now.Year;
                int ky = row.Table.Columns.Contains("ky") ? Convert.ToInt32(row["ky"]) : DateTime.Now.Month;

                // 1. Gọi SP in chuẩn được ghi chú trong Model của bạn để lấy hạch toán (Nợ/Có)
                var dsPrint = await myDb.LoadDataset("FAbtpbts_Tao_F7", new Dictionary<string, object>
                {
                    ["@stt_rec"] = sttRec
                });

                // 2. Dùng Model có sẵn và ép dữ liệu tự động bằng ConvertDataTable (Giống chức năng cũ)
                var reportModel = new FAbtpbts_TaoReportModel
                {
                    MaDVCS = row.Table.Columns.Contains("ma_dvcs") ? row["ma_dvcs"].ToString() : "",
                    Nam = nam,
                    Ky = ky,
                    Ky2 = ky,
                    // Ngay_ct2 = GetEndOfPeriod(nam, ky), // (Nếu bạn có hàm tính ngày cuối kỳ)
                    LsData = new List<FAbtpbts_Tao_F7Detail>()
                };

                if (dsPrint != null && dsPrint.Tables.Count > 0)
                {
                    // Tự động map DataTable vào Model chi tiết
                    reportModel.LsData = myDb.ConvertDataTable<FAbtpbts_Tao_F7Detail>(dsPrint.Tables[0]);
                }

                // 3. Đóng gói Model vào List<object> để đưa cho ReportOptionComponent
                var lsDataPerVoucher = new List<object> { reportModel };

                // 4. Đưa vào hàng đợi in
                var job = new PrintJob
                {
                    Title = "In bút toán số: " + (row.Table.Columns.Contains("so_ct") ? row["so_ct"].ToString() : ""),
                    Data = lsDataPerVoucher
                };

                PrintQueue.Add(job);
            }

            // Ép giao diện render MỘT LẦN DUY NHẤT để popup đầu tiên tự động bật lên
            await InvokeAsync(StateHasChanged);
        }

        // 4. Hàm đóng cửa sổ cập nhật lại tham số
        protected void ClosePrintWindow(PrintJob job)
        {
            if (PrintQueue.Contains(job))
            {
                PrintQueue.Remove(job);
                StateHasChanged();
            }
        }
        // ===================================================
        // DISPOSE
        // ===================================================
        public override async void Dispose()
        {
            myStateMN.OnActionTaskChanged -= OnActionTaskchanged;
            myStateMN.OnLangguaChanged    -= OnLangguachange;
            myStateMN.OnDvcsChanged       -= OnDvcsChangedHandler;
        }
    }

}


