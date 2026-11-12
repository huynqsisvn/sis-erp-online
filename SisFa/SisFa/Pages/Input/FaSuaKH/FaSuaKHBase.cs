using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using SisCom.Model;
using SisCom.Shared;
using SisData.Data;
using SisData.Service;
using SisFa.Pages.Input.FaTinhKH;
using SisLib.List;
using SisLib.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SisFa.Pages.Input.FaSuaKH
{
    public class FaSuaKHBase : ListBaseComponent
    {
        // ===================================================
        // BIẾN FILTER
        // ===================================================
        protected DxPopup popFilter;
        protected bool PopupVisible        = false;
        protected int  FilterNam;
        protected int  FilterKy1           = 1;
        protected int  FilterKy2           = 12;
        protected string FilterMaDvcs      = "";
        protected string FilterKeyTs       = "";
        protected bool FilterSuaKh         = false;   // Checkbox "Lọc các tài sản đã điều chỉnh k.hao"
        protected List<string> FilterErrors = new();
        protected List<DvcsItem> LsDvcs    = new();

        // ===================================================
        // BIẾN MODAL CHỌN TÀI SẢN
        // ===================================================
        protected bool ShowModalDmts         = false;
        protected DataTable TblDmts          = null;
        private   DataTable DtMaTsFull       = null;
        protected string DmtsSearch          = "";
        protected HashSet<string> SelectedDmts = new(StringComparer.OrdinalIgnoreCase);

        protected bool IsAllDmtsChecked =>
            TblDmts != null
            && TblDmts.Rows.Count > 0
            && TblDmts.AsEnumerable().All(r => SelectedDmts.Contains(r["so_the_ts"]?.ToString() ?? ""));

        protected string ConfirmBtnText =>
            SelectedDmts.Count > 0 ? $"Xác nhận ({SelectedDmts.Count} tài sản)" : "Xác nhận";

        // ===================================================
        // Store procedure names
        // ===================================================
        private string _storeQuery  = "";
        protected string _storeUpdate = "";

        // ===================================================
        // DxGrid cho lưới chính (có cột tag, giống FAbtpbts_Tao)
        // ===================================================
        protected DxGrid gridMain;
        protected FrmBrowse3Component oBrowse3;
        // ===================================================
        // KHỞI TẠO
        // ===================================================
        protected override void OnInitialized()
        {
            mPage        = "fasuakh";
            sqlTableName = "ctkhts";
            sqlTableView = "ctkhts";
            SqlTableKey  = "so_the_ts;nam;ky;ma_bpts;id_ma_nv";
            SqlItemName  = "so_the_ts";
            IsShowCopy   = false;
            IsShowImport = false;
            FrmType      = typeof(FrmFaSuaKH);

            if (string.IsNullOrEmpty(Menu_id))
                Menu_id = "wf.ct.06";

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
                    myStateMN.SetToolBar(new MyToolbar("New",     false));
                    myStateMN.SetToolBar(new MyToolbar("Copy",    false));
                    myStateMN.SetToolBar(new MyToolbar("Import",  false));
                    myStateMN.SetToolBar(new MyToolbar("Edit",    true, "F3"));
                    myStateMN.SetToolBar(new MyToolbar("Save",    false));
                    myStateMN.SetToolBar(new MyToolbar("Delete",  true, "F8"));
                    myStateMN.SetToolBar(new MyToolbar("Change",  false));
                    myStateMN.SetToolBar(new MyToolbar("View",    false));
                    myStateMN.SetToolBar(new MyToolbar("Aproval", false));
                    myStateMN.SetToolBar(new MyToolbar("Print",   false));
                    myStateMN.SetToolBar(new MyToolbar("Export",  true));
                    myStateMN.SetToolBar(new MyToolbar("Search",  true, "Ctrl + F"));
                    myStateMN.SetToolBar(new MyToolbar("Quit",    false));
                }
                else
                {
                    myStateMN.SetToolBar(new MyToolbar("toolbar", false));
                }

                myStateMN.OnActionTaskChanged += OnActionTaskchanged;
                myStateMN.OnLangguaChanged    += OnLangguachange;
                myStateMN.OnDvcsChanged       += OnDvcsChangedHandler;

                var dvcs = myStateMN.GetDvcs()?.Ma_dvcs?.Trim() ?? "";
                FilterMaDvcs = dvcs == "ALL" ? "" : dvcs;

                PopupVisible = true;
                _ = PreloadDmtsAsync();
                await LoadStoreProcNames();
                StateHasChanged();
            }
        }

        // ===================================================
        // LẤY TÊN STORED PROCEDURE TỪ DB
        // ===================================================
        private async Task LoadStoreProcNames()
        {
            try
            {
                if (curCommand != null)
                {
                    string storeProc = curCommand.Store_proc ?? "";
                    string[] parts = storeProc.Split(';');
                    if (parts.Length > 0)
                        _storeQuery = parts[0].Trim();
                    if (parts.Length > 1)
                        _storeUpdate = parts[1].Trim();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoadStoreProcNames error: " + ex.Message);
            }
        }

        // ===================================================
        // PAGELOAD
        // ===================================================
        public override async Task PageLoad()
        {
            try
            {
                if (myCommand == null) return;
                curCommand = await myCommand.GetSubMenuBymenuId(Menu_id.Replace("_", "."));
                if (curCommand != null)
                {
                    fbrowse   = IsLangEn ? curCommand.Ebrowse1 : curCommand.Vbrowse1;
                    string ps = myOption.GetOptionsValue("M_GRID_NROW");
                    Page_size = string.IsNullOrEmpty(ps) ? 50 : int.Parse(ps.Trim());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("PageLoad error: " + ex.Message);
            }
        }

        // ===================================================
        // LOAD DATA — hiển thị kết quả sau khi lọc
        // ===================================================
        public override async Task Loaddata()
        {
            SelectedItem = null;
            try
            {
                // 1. Ép render trắng lưới để tạo hiệu ứng nháy đổi dữ liệu
                TblData = null;
                await InvokeAsync(StateHasChanged);

                // Xây dựng chuỗi @Advance giống WinForm StartUp.KeyFilter
                string advanceFilter = "";

                // Lọc theo danh sách tài sản
                if (!string.IsNullOrEmpty(FilterKeyTs))
                {
                    string safeList = string.Join(",",
                        FilterKeyTs.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => x.Trim().Replace("'", "''"))
                            .Where(x => !string.IsNullOrEmpty(x)));
                    if (!string.IsNullOrEmpty(safeList))
                        advanceFilter += $" AND dbo.InList(so_the_ts, '{safeList}', ',') = 1";
                }

                // Lọc theo ĐVCS trực tiếp từ biến FilterMaDvcs realtime
                if (!string.IsNullOrEmpty(FilterMaDvcs))
                    advanceFilter += $" AND ma_dvcs LIKE '{FilterMaDvcs.Replace("'", "''").Trim()}%'";

                // Checkbox: Nếu KHÔNG tick → chỉ lấy tài sản chưa điều chỉnh (sua_kh = 0)
                if (!FilterSuaKh)
                    advanceFilter += " AND sua_kh = 0";

                // Gọi Stored Procedure hoặc query trực tiếp
                if (!string.IsNullOrEmpty(_storeQuery))
                {
                    string paramNames = "@Nam;@Ky1;@Ky2;@Advance";
                    string paramValues = $"{FilterNam};{FilterKy1};{FilterKy2};{advanceFilter}";
                    DataSet ds = await myDb.LoadStoreProcedure(_storeQuery, paramNames, paramValues);

                    if (ds != null && ds.Tables.Count > 0)
                    {
                        TblData = ds.Tables[ds.Tables.Count - 1];
                    }
                }
                else
                {
                    // Fallback: query trực tiếp nếu không có SP
                    string maDvcsLike = string.IsNullOrEmpty(FilterMaDvcs) ? "%" : FilterMaDvcs + "%";
                    string keyCondition = string.IsNullOrEmpty(FilterKeyTs) ? "1=1" : BuildInListCondition("c.so_the_ts", FilterKeyTs);
                    string suaKhCond = FilterSuaKh ? "1=1" : "c.sua_kh = 0";

                    // Lọc theo bảng danh mục (dmts) y hệt như FaTinhKH
                    string dvcsCondition = $"d.ma_dvcs LIKE '{maDvcsLike}'";

                    string sql = $@"SELECT ROW_NUMBER() OVER (ORDER BY c.so_the_ts, c.ma_bpts, c.nam, c.ky) AS stt,
                c.so_the_ts, d.ten_ts, d.ten_ts2, c.ma_bpts, c.nam, c.ky, c.ma_dvcs,
                c.nguyen_gia, c.gt_da_kh, c.gt_cl, ISNULL(c.gt_cl0, c.gt_cl) AS gt_cl0,
                c.gt_kh_ky, ISNULL(c.gt_kh_dc, 0) AS gt_kh_dc, ISNULL(c.gt_cl2, c.gt_cl) AS gt_cl2,
                ISNULL(c.id_ma_nv, 0) AS id_ma_nv, c.ma_nv, ISNULL(c.sua_kh, 0) AS sua_kh, c.dien_giai,
                d.tk_ts, d.tk_kh, d.tk_cp, d.ma_phi, d.ma_bpht, d.ma_px, d.ma_vv
                FROM {sqlTableView} c
                LEFT JOIN dmts d ON d.so_the_ts = c.so_the_ts
                WHERE c.nam = {FilterNam}
                AND c.ky BETWEEN {FilterKy1} AND {FilterKy2}
                AND ({dvcsCondition})
                AND ({keyCondition})
                AND ({suaKhCond})
                ORDER BY c.so_the_ts, c.ma_bpts, c.nam, c.ky";

                    DataSet ds = await myDb.LoadDataset(sql);
                    TblData = ds?.Tables[0];
                }

                // Đảm bảo cột tag tồn tại (phục vụ cho checkbox trên lưới của FaSuaKH)
                if (TblData != null)
                {
                    if (!TblData.Columns.Contains("tag"))
                        TblData.Columns.Add("tag", typeof(bool));
                    foreach (DataRow r in TblData.Rows)
                        if (r["tag"] == DBNull.Value) r["tag"] = false;
                }

                if (TblData != null && TblData.DefaultView.Count > 0)
                    SelectedItem = TblData.DefaultView[0];

                if (oBrowse != null)
                    await oBrowse.FocusGridAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Loaddata error: " + ex.Message);
            }

            // Ép render lại lần cuối để hiển thị dữ liệu
            await InvokeAsync(StateHasChanged);
        }

        private static string BuildInListCondition(string column, string commaList)
        {
            if (string.IsNullOrWhiteSpace(commaList)) return "1=1";
            var items = commaList
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim().Replace("'", "''"))
                .Where(x => !string.IsNullOrEmpty(x));
            string inList = string.Join(",", items.Select(x => $"'{x}'"));
            return $"{column} IN ({inList})";
        }

        // ===================================================
        // TOOLBAR ACTIONS
        // ===================================================
        public override void OnActionTaskchanged(ActionTask ac)
        {
            switch (ac)
            {
                case ActionTask.Search:
                    PopupVisible = true;
                    StateHasChanged();
                    break;
                case ActionTask.Edit:
                    myStateMN.SetCurrentActionTask(ActionTask.Edit);
                    V_Sua();
                    break;
                case ActionTask.Delete:
                    V_Xoa();
                    break;
                case ActionTask.Export:
                    ExportData(1);
                    break;
            }
        }

        /// <summary>Override V_Sua to open DxWindow edit form (like Zcdmslts / WinForm oBrowse_F3)</summary>
        public override void V_Sua()
        {
            DataRowView rview = SelectedItem as DataRowView;
            if (rview == null) { myStateMN.SetNewThongbao("Chưa chọn dòng dữ liệu!"); return; }

            // Clone dòng dữ liệu từ grid (giống WinForm: dataTable.Clone() + Rows.Add(ItemArray))
            DataTable dtClone = rview.Row.Table.Clone();
            dtClone.Rows.Add(rview.Row.ItemArray);

            Par.Clear();
            Par.Add("Menu_id",       (object)Menu_id);
            Par.Add("ShowAdd",       (object)true);
            Par.Add("curActionTask", (object)ActionTask.Edit);
            Par.Add("StoreUpdate",   (object)_storeUpdate);
            Par.Add("EditRowData",   (object)dtClone);
            Par.Add("OnClose",       EventCallback.Factory.Create(this, CloseForm));
            IsShowFrm = true;
            StateHasChanged();
        }

        public async void CloseForm()
        {
            IsShowFrm = false;
            await Loaddata();
            StateHasChanged();
        }

        // ===================================================
        // GRID — Parse Fbrowse thành danh sách cột (giống FAbtpbts_Tao)
        // ===================================================
        protected record GridCol(string Field, string Caption, string Width, bool IsDate, bool IsNumber, bool IsHidden, string Format);

        protected IEnumerable<GridCol> GridColumns
        {
            get
            {
                if (string.IsNullOrEmpty(fbrowse)) yield break;
                foreach (var seg in fbrowse.Split(';'))
                {
                    if (string.IsNullOrWhiteSpace(seg)) continue;
                    var p = seg.Trim().Split(':');
                    if (p.Length < 1) continue;
                    string field = p[0].Trim().ToLower();
                    if (field == "tag") continue;
                    string width  = p.Length > 1 ? p[1].Trim() + "px" : "100px";
                    string caption = "";
                    bool isDate = false, isNum = false, isHidden = false;
                    string fmt = "";
                    for (int i = 2; i < p.Length; i++)
                    {
                        string f = p[i].Trim();
                        if (f.StartsWith("h=", StringComparison.OrdinalIgnoreCase))
                            caption = f.Substring(2).Split('&')[0];
                        else if (f.Equals("D", StringComparison.OrdinalIgnoreCase))
                            isDate = true;
                        else if (f.Equals("IV", StringComparison.OrdinalIgnoreCase))
                            isHidden = true;
                        else if (f.StartsWith("N", StringComparison.OrdinalIgnoreCase) && f.Length <= 3)
                        { isNum = true; fmt = f.ToUpper() == "N" ? "N0" : f.ToUpper(); }
                    }
                    if (string.IsNullOrEmpty(caption)) caption = field;
                    yield return new GridCol(field, caption, width, isDate, isNum, isHidden, fmt);
                }
            }
        }

        protected void OnGridRowClick(GridRowClickEventArgs e)
        {
            if (e.Grid.GetDataItem(e.VisibleIndex) is DataRowView rv)
            {
                SelectedItem = rv;
                StateHasChanged();
            }
        }

        protected void OnCustomizeElement(GridCustomizeElementEventArgs e)
        {
            if (e.ElementType == GridElementType.DataRow && SelectedItem is DataRowView selRv)
            {
                if (e.Grid.GetDataItem(e.VisibleIndex) is DataRowView rowRv && rowRv.Row == selRv.Row)
                    e.Style = "background-color: #f0f0f0;";
            }
        }

        protected void OnTagChanged(DataRowView rv, bool value)
        {
            rv["tag"] = value;
            InvokeAsync(StateHasChanged);
        }

        // ===================================================
        // DVCS CHANGED
        // ===================================================
        private async void OnDvcsChangedHandler(string maDvcs)
        {
            // Bắt buộc dùng InvokeAsync để ép giao diện Blazor render lại lưới ngay lập tức
            await InvokeAsync(async () =>
            {
                try
                {
                    // Cập nhật mã ĐVCS mới nhất từ thanh tiêu đề
                    FilterMaDvcs = (maDvcs == "ALL" || string.IsNullOrEmpty(maDvcs)) ? "" : maDvcs.Trim();

                    // KHÔNG xóa FilterKeyTs để giữ lại danh sách 4 tài sản (ví dụ) bạn đã chọn

                    // Tải lại lưới dữ liệu
                    await Loaddata();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DvcsChanged error: " + ex.Message);
                }
            });
        } 

        // ===================================================
        // VALIDATE FILTER
        // ===================================================
        protected bool ValidateFilter()
        {
            FilterErrors.Clear();

            if (FilterNam <= 0)
                FilterErrors.Add("Năm không hợp lệ (phải > 0).");
            if (FilterKy1 < 1 || FilterKy1 > 12)
                FilterErrors.Add("Từ kỳ phải từ 1 đến 12.");
            if (FilterKy2 < 1 || FilterKy2 > 12)
                FilterErrors.Add("Đến kỳ phải từ 1 đến 12.");
            if (FilterKy1 > FilterKy2)
                FilterErrors.Add("Từ kỳ không được lớn hơn Đến kỳ.");

            try
            {
                var ngayBatDau = new DateTime(FilterNam, FilterKy1, 1);
                string strMoSo   = myOption.GetOptionsValue("M_NGAY_MO_SO");
                string strKhoaSo = myOption.GetOptionsValue("M_NGAY_KHOA_SO");

                if (!string.IsNullOrEmpty(strMoSo) && DateTime.TryParse(strMoSo, out DateTime ngayMoSo))
                {
                    if (ngayBatDau < ngayMoSo)
                        FilterErrors.Add($"Năm phải lớn hơn hoặc bằng năm của kỳ mở sổ ({ngayMoSo:dd/MM/yyyy}).");
                }
                if (!string.IsNullOrEmpty(strKhoaSo) && DateTime.TryParse(strKhoaSo, out DateTime ngayKhoaSo))
                {
                    if (ngayBatDau < ngayKhoaSo)
                        FilterErrors.Add($"Không thể điều chỉnh khấu hao vào kỳ khóa sổ ({ngayKhoaSo:dd/MM/yyyy}).");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Validate date error: " + ex.Message);
            }

            return FilterErrors.Count == 0;
        }

        // ===================================================
        // NÚT LỌC DỮ LIỆU
        // ===================================================
        protected async void BtnFilter_Click()
        {
            if (!ValidateFilter())
            {
                StateHasChanged();
                return;
            }

            ShowLoadding = true;
            StateHasChanged();

            try
            {
                await Loaddata();
                PopupVisible = false;
                myStateMN.SetNewThongbao($"Lọc dữ liệu thành công! Kỳ {FilterKy1}-{FilterKy2}/{FilterNam} — {TblData?.Rows?.Count ?? 0} tài sản.");
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao("Lỗi khi lọc dữ liệu: " + ex.Message);
            }
            finally
            {
                ShowLoadding = false;
                StateHasChanged();
            }
        }

        // ===================================================
        // LẤY DANH SÁCH DÒNG ĐƯỢC ĐÁNH DẤU (tag = true)
        // Giống FAbtpbts_Tao: GetTaggedRows()
        // ===================================================
        protected List<DataRowView> GetTaggedRows()
        {
            var result = new List<DataRowView>();
            try
            {
                if (TblData == null || !TblData.Columns.Contains("tag")) return result;
                foreach (DataRowView rv in TblData.DefaultView)
                {
                    if (rv["tag"] != DBNull.Value && Convert.ToBoolean(rv["tag"]))
                        result.Add(rv);
                }
            }
            catch { }
            return result;
        }

        // ===================================================
        // XÓA — xóa tất cả dòng đã đánh dấu tag (giống FAbtpbts_Tao)
        // Nếu không có dòng nào đánh dấu → xóa dòng đang chọn
        // ===================================================
        public virtual async void V_Xoa()
        {
            var taggedRows = GetTaggedRows();

            // Nếu không có dòng nào đánh dấu → lấy dòng đang chọn
            if (taggedRows.Count == 0)
            {
                if (SelectedItem is DataRowView sel)
                    taggedRows.Add(sel);
            }

            if (taggedRows.Count == 0)
            {
                myStateMN.SetNewThongbao("Chưa chọn dòng dữ liệu!");
                return;
            }

            // Xây dựng thông báo xác nhận
            string confirmText;
            if (taggedRows.Count == 1)
            {
                var rv = taggedRows[0];
                string soTheTs = rv["so_the_ts"]?.ToString()?.Trim() ?? "";
                string maBpts  = rv["ma_bpts"]?.ToString()?.Trim() ?? "";
                string nam     = rv["nam"]?.ToString()?.Trim() ?? "";
                string ky      = rv["ky"]?.ToString()?.Trim() ?? "";
                confirmText = $"Bạn có muốn xóa [{soTheTs} - {maBpts} - Năm {nam}, Kỳ {ky}] không?";
            }
            else
            {
                confirmText = $"Bạn có muốn xóa {taggedRows.Count} dòng đã đánh dấu không?";
            }

            var confirm = await myDlSvc.ConfirmAsync(new MessageBoxOptions
            {
                Text             = confirmText,
                Title            = "Xác nhận xóa",
                OkButtonText     = "Có",
                CancelButtonText = "Không",
                ShowIcon         = true,
                ShowCloseButton  = false,
                RenderStyle      = MessageBoxRenderStyle.Danger
            });

            if (confirm)
            {
                try
                {
                    int deleted = 0;
                    foreach (var rview in taggedRows)
                    {
                        string soTheTs = rview["so_the_ts"]?.ToString()?.Trim() ?? "";
                        string nam     = rview["nam"]?.ToString()?.Trim() ?? "";
                        string ky      = rview["ky"]?.ToString()?.Trim() ?? "";
                        string maBpts  = rview["ma_bpts"]?.ToString()?.Trim() ?? "";
                        string idMaNv  = rview.Row.Table.Columns.Contains("id_ma_nv") ? rview["id_ma_nv"]?.ToString()?.Trim() ?? "0" : "0";

                        string sqlDel = $"DELETE ctkhts WHERE nam = {nam} AND ky = {ky} AND so_the_ts = '{soTheTs.Replace("'", "''")}' AND ma_bpts = '{maBpts.Replace("'", "''")}' AND id_ma_nv = {idMaNv}";
                        await myDb.ExecuteSqlQueryAsync(sqlDel);
                        deleted++;
                    }

                    myStateMN.SetNewThongbao($"Xóa thành công {deleted} dòng!");
                    await Loaddata();
                }
                catch (Exception ex)
                {
                    myStateMN.SetNewThongbao("Lỗi khi xóa: " + ex.Message);
                }
            }
            StateHasChanged();
        }

        // ===================================================
        // HELPERS CHO RAZOR
        // ===================================================
        protected void CloseFilter()
        {
            PopupVisible = false;
            StateHasChanged();
        }

        protected void ClearKeyTs()
        {
            FilterKeyTs = "";
            StateHasChanged();
        }

        // ===================================================
        // MODAL CHỌN TÀI SẢN
        // ===================================================
        private async Task PreloadDmtsAsync()
        {
            try
            {
                string sql = "SELECT CAST(0 AS BIT) AS tag, so_the_ts, ten_ts, ten_ts2, ma_dvcs FROM dmts ORDER BY so_the_ts";
                DataSet ds = await myDb.LoadDataset(sql);
                DtMaTsFull = ds?.Tables[0] ?? new DataTable();
            }
            catch (Exception ex)
            {
                Console.WriteLine("PreloadDmts error: " + ex.Message);
                DtMaTsFull = new DataTable();
            }
        }

        protected async Task OpenDmtsModal()
        {
            DmtsSearch = "";
            SelectedDmts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(FilterKeyTs))
            {
                foreach (var ma in FilterKeyTs.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    string trimmed = ma.Trim();
                    if (!string.IsNullOrEmpty(trimmed))
                        SelectedDmts.Add(trimmed);
                }
            }
            if (DtMaTsFull == null)
                await PreloadDmtsAsync();
            ApplyDmtsFilter();
            ShowModalDmts = true;
            StateHasChanged();
        }

        private void ApplyDmtsFilter()
        {
            if (DtMaTsFull == null)
            {
                TblDmts = new DataTable();
                return;
            }
            string dvcsFilter = "1=1";
            string searchFilter = "";
            if (!string.IsNullOrEmpty(DmtsSearch))
            {
                string s = DmtsSearch.Trim().Replace("'", "''");
                searchFilter = $" AND (so_the_ts LIKE '%{s}%' OR ten_ts LIKE '%{s}%' OR ten_ts2 LIKE '%{s}%')";
            }
            DtMaTsFull.DefaultView.RowFilter = dvcsFilter + searchFilter;
            TblDmts = DtMaTsFull.DefaultView.ToTable();
            foreach (DataRow r in TblDmts.Rows)
            {
                if (r["so_the_ts"] != DBNull.Value)
                    r["so_the_ts"] = r["so_the_ts"].ToString().Trim();
            }
        }

        protected void DoSearchDmts()
        {
            ApplyDmtsFilter();
            StateHasChanged();
        }

        protected void ToggleDmtsRow(string maTs)
        {
            if (SelectedDmts.Contains(maTs))
                SelectedDmts.Remove(maTs);
            else
                SelectedDmts.Add(maTs);
            StateHasChanged();
        }

        protected void ToggleAllDmts(ChangeEventArgs e)
        {
            bool check = e.Value is bool b && b;
            if (check)
            {
                foreach (DataRow row in TblDmts.Rows)
                {
                    string ma = row["so_the_ts"]?.ToString() ?? "";
                    if (!string.IsNullOrEmpty(ma)) SelectedDmts.Add(ma);
                }
            }
            else
            {
                foreach (DataRow row in TblDmts.Rows)
                    SelectedDmts.Remove(row["so_the_ts"]?.ToString() ?? "");
            }
            StateHasChanged();
        }

        protected void SelectAllDmts()
        {
            if (TblDmts == null) return;
            foreach (DataRow row in TblDmts.Rows)
            {
                string ma = row["so_the_ts"]?.ToString() ?? "";
                if (!string.IsNullOrEmpty(ma)) SelectedDmts.Add(ma);
            }
            StateHasChanged();
        }

        protected void ClearDmtsSelection()
        {
            SelectedDmts.Clear();
            StateHasChanged();
        }

        protected void ConfirmDmtsSelection()
        {
            FilterKeyTs = SelectedDmts.Count > 0
                ? string.Join(",", SelectedDmts.Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).OrderBy(x => x))
                : "";
            ShowModalDmts = false;
            StateHasChanged();
        }

        protected void CloseModalDmts()
        {
            ShowModalDmts = false;
            SelectedDmts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(FilterKeyTs))
            {
                foreach (var ma in FilterKeyTs.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    string trimmed = ma.Trim();
                    if (!string.IsNullOrEmpty(trimmed))
                        SelectedDmts.Add(trimmed);
                }
            }
            StateHasChanged();
        }

        // ===================================================
        // EXPORT
        // ===================================================
        public override async void ExportData(int type)
        {
            if (oBrowse3 != null)
                oBrowse3.ExPortGrid(type);
            myStateMN.SetNewThongbao("Xuất tệp thành công!");
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
        public class DvcsItem
        {
            public string Ma_dvcs { get; set; } = "";
            public string Ten_dvcs { get; set; } = "";
            public string Display => string.IsNullOrEmpty(Ma_dvcs) ? Ten_dvcs : $"{Ma_dvcs} - {Ten_dvcs}";
        }
    }
}
