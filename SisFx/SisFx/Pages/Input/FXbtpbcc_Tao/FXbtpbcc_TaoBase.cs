using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
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

namespace SisFx.Pages.Input.FXbtpbcc_Tao
{
    public class FXbtpbcc_TaoBase : ListBaseComponent
    {
        // ===================================================
        // BI?N FILTER
        // ===================================================
        protected DxPopup popFilter;
        protected bool PopupVisible   = false;
        protected int  FilterNam;
        protected int  FilterKy1      = 1;
        protected int  FilterKy2      = 12;
        protected string FilterMaDvcs = "";

        // fbrowseCt l?y t? VBrowse1 split '|' — gi?ng WinForm strArray[1]
        protected string FbrowseCT = "";

        // ===================================================
        // BI?N L??I PH + CT
        // ===================================================
        protected DataTable TblPH     = null;
        protected DataTable TblCT     = null;
        protected DataTable TblCTFull = null;
        protected bool ShowLoaddingVoucher = false;
        protected FrmBrowse3Component oBrowsePH;
        protected FrmBrowseComponent  oBrowseCT;

        // ===================================================
        // BI?N BÁO CÁO IN (F7)
        // ===================================================
        protected bool IsShowReport = false;
        protected List<FXbtpbcc_TaoReportModel> LSReportData;

        // ===================================================
        // BI?N DIALOG T?O BÚT TOÁN
        // ===================================================
        protected bool ShowDlgTaoBt   = false;
        protected TaoBtModel DlgTaoBt = new();
        protected bool DlgLoading     = false;
        protected string DlgSoCt2     = "";

        // ===================================================
        // BI?N CONFIRM XÓA
        // ===================================================
        protected bool ShowConfirmDelete = false;

        // ===================================================
        // BI?N CONFIRM GHI ?È
        // ===================================================
        protected bool ShowConfirmOverwrite = false;
        protected string OverwriteInfo      = "";

        // Context t?m cho confirm overwrite
        private string _pendingMaQs     = "";
        private string _pendingSoCt     = "";
        private string _pendingDienGiai = "";
        protected List<DataRowView> _pendingTaggedRows = new();

        // ===================================================
        // KH?I T?O
        // ===================================================
        protected override void OnInitialized()
        {
            mPage        = "fxbtpbcc_tao";
            sqlTableName = "ctpbbcc";
            sqlTableView = "ctpbbcc";
            SqlTableKey  = "stt_rec";
            SqlItemName  = "stt_rec";
            IsShowCopy   = false;
            IsShowImport = false;

            if (string.IsNullOrEmpty(Menu_id))
                Menu_id = "wf.xc.06";

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
                await Lap.LoadLocalizationAsync(string.Format("fx-{0}", UserLang.Trim()));
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
        }

        // ===================================================
        // PAGELOAD — fbrowse và FbrowseCT l?y t? DB
        // WinForm: M_LAN=="V" ? VBrowse1 : EBrowse1, split '|' ? [0]=PH, [1]=CT
        // ===================================================
        public override async Task PageLoad()
        {
            try
            {
                if (myCommand == null) return;
                curCommand = await myCommand.GetSubMenuBymenuId(Menu_id.Replace("_", "."));
                if (curCommand != null)
                {
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
                Console.WriteLine("FXbtpbcc_Tao PageLoad error: " + ex.Message);
            }
        }

        // ===================================================
        // LOAD DATA
        // WinForm StartUp.cs: ds = ExcuteReader(cmd)
        //   table2 = ds.Tables[Count-2] ? PH (l??i trên)
        //   table1 = ds.Tables[Count-1] ? CT (l??i d??i)
        // ===================================================
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
                string spName = "FXbtpbcc_Tao";
                if (curCommand != null && !string.IsNullOrEmpty(curCommand.Store_proc))
                {
                    var parts = curCommand.Store_proc.Split(';');
                    if (parts.Length > 0 && !string.IsNullOrEmpty(parts[0].Trim()))
                        spName = parts[0].Trim();
                }

                string paramNames  = "@Nam;@ky;@ky2;@Ma_dvcs";
                string paramValues = $"{FilterNam};{FilterKy1};{FilterKy2};{FilterMaDvcs ?? ""}";

                DataSet ds = await myDb.LoadStoreProcedure(spName, paramNames, paramValues);

                if (ds != null && ds.Tables.Count >= 2)
                {
                    // WinForm: Tables[Count-2] = PH, Tables[Count-1] = CT
                    DataTable tblPh = ds.Tables[ds.Tables.Count - 2];
                    DataTable tblCt = ds.Tables[ds.Tables.Count - 1];

                    if (!tblPh.Columns.Contains("tag"))
                        tblPh.Columns.Add("tag", typeof(bool));
                    foreach (DataRow r in tblPh.Rows)
                        if (r["tag"] == DBNull.Value) r["tag"] = false;

                    TblPH     = tblPh;
                    TblCTFull = tblCt;
                    TblCT     = tblCt;

                    if (TblPH.DefaultView.Count > 0)
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
                myStateMN.SetNewThongbao($"{Lap["Error loading data:"]} {ex.Message}");
                Console.WriteLine("FXbtpbcc_Tao Loaddata error: " + ex.Message);
            }
            finally
            {
                ShowLoaddingVoucher = false;
                StateHasChanged();
            }
        }

        // ===================================================
        // L?C CT THEO DÒNG PH ?ANG CH?N
        // WinForm: key filter "ky;ma_dvcs;tk_kh;tk_cp"
        // ===================================================
        protected void FilterCtBySelectedPh()
        {
            try
            {
                if (SelectedItem == null || TblCTFull == null) return;
                var row   = (DataRowView)SelectedItem;
                var phTbl = row.Row.Table;

                if (!TblCTFull.Columns.Contains("ky") || !TblCTFull.Columns.Contains("ma_dvcs")) return;

                string ky     = phTbl.Columns.Contains("ky")      ? row["ky"]?.ToString()?.Trim()      ?? "" : "";
                string maDvcs = phTbl.Columns.Contains("ma_dvcs") ? row["ma_dvcs"]?.ToString()?.Trim() ?? "" : "";
                string tkKh   = phTbl.Columns.Contains("tk_kh")   ? row["tk_kh"]?.ToString()?.Trim()   ?? "" : "";
                string tkCp   = phTbl.Columns.Contains("tk_cp")   ? row["tk_cp"]?.ToString()?.Trim()   ?? "" : "";

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
        // CH?N DÒNG PH
        // ===================================================
        public override void CurItem(object obj)
        {
            if (obj != null)
            {
                SelectedItem = obj;
                FilterCtBySelectedPh();
            }
            else
            {
                SelectedItem = null;
                if (TblCT != null)
                    TblCT.DefaultView.RowFilter = "1=0";
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
                    V_TaoBt();
                    break;
                case ActionTask.Delete:
                    V_XoaBt();
                    break;
                case ActionTask.Print:
                    myStateMN.SetCurrentActionTask(ActionTask.Print);
                    PrintVoucherAsync();
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
                FilterMaDvcs = (maDvcs == "ALL" || string.IsNullOrEmpty(maDvcs)) ? "" : maDvcs.Trim();
                SelectedItem = null;
                if (TblCT != null)
                    TblCT.DefaultView.RowFilter = "1=0";
                await Loaddata();
                StateHasChanged();
            });
        }

        // ===================================================
        // VALIDATE FILTER (gi?ng WinForm StartFrm.CheckValidNam_Ky)
        // ===================================================
        protected bool ValidateFilter()
        {
            if (FilterNam < 1900 || FilterNam > 2079)
            {
                myStateMN.SetNewThongbao(Lap["Year is invalid (must be > 0)."]);
                return false;
            }
            if (FilterKy1 < 1 || FilterKy1 > 12)
            {
                myStateMN.SetNewThongbao(Lap["From period must be 1 to 12."]);
                return false;
            }
            if (FilterKy2 < 1 || FilterKy2 > 12)
            {
                myStateMN.SetNewThongbao(Lap["To period must be 1 to 12."]);
                return false;
            }
            if (FilterKy1 > FilterKy2)
            {
                myStateMN.SetNewThongbao(Lap["From period cannot be greater than To period."]);
                return false;
            }

            // Ki?m tra khóa s? (gi?ng WinForm StartFrm.CheckValidNam_Ky)
            try
            {
                string strDongSo = myOption.GetOptionsValue("M_NGAY_DONG_SO");
                if (!string.IsNullOrEmpty(strDongSo) && DateTime.TryParse(strDongSo, out DateTime ngayDongSo))
                {
                    DateTime ngayKetThuc = GetEndOfPeriod(FilterNam, FilterKy1);
                    if (ngayKetThuc <= ngayDongSo)
                    {
                        myStateMN.SetNewThongbao(Lap["Data is locked, cannot transfer!"]);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ValidateFilter date error: " + ex.Message);
            }

            return true;
        }

        // ===================================================
        // L?Y DANH SÁCH DÒNG ???C ?ÁNH D?U (tag = true)
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
        // ?ÁNH D?U / B? D?U T?T C? (Ctrl+A / Ctrl+U)
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
        // F4 — T?O BÚT TOÁN (newButton_Click trong WinForm)
        // ===================================================
        protected async void V_TaoBt()
        {
            var taggedRows = GetTaggedRows();
            if (taggedRows.Count == 0)
            {
                myStateMN.SetNewThongbao(Lap["Must mark rows before performing this action!"]);
                return;
            }

            // Ki?m tra khóa s?
            try
            {
                string strDongSo = myOption.GetOptionsValue("M_NGAY_DONG_SO");
                if (!string.IsNullOrEmpty(strDongSo) && DateTime.TryParse(strDongSo, out DateTime ngayDongSo))
                {
                    DateTime ngayBatDau = GetStartOfPeriod(FilterNam, FilterKy1);
                    if (ngayBatDau <= ngayDongSo)
                    {
                        myStateMN.SetNewThongbao(Lap["Data is locked, cannot transfer!"]);
                        return;
                    }
                }
            }
            catch { }

            // L?y danh sách stt_rec ?ã tag
            string sttRecList = string.Join(",",
                taggedRows.Where(r => !string.IsNullOrWhiteSpace(r["stt_rec"]?.ToString()))
                          .Select(r => r["stt_rec"].ToString()));

            // L?y thông tin m?c ??nh t? dòng ??u
            DataRowView firstRow = taggedRows[0];
            string ma_qs_default    = firstRow["ma_qs"]?.ToString()?.Trim() ?? "";
            string so_ct_default    = firstRow["so_ct"]?.ToString()?.Trim() ?? "";
            string dien_giai_default = firstRow["dien_giai"]?.ToString()?.Trim() ?? "";

            if (ma_qs_default == "" && SelectedItem != null)
                ma_qs_default = ((DataRowView)SelectedItem)["ma_qs"]?.ToString()?.Trim() ?? "";
            if (dien_giai_default == "" && SelectedItem != null)
                dien_giai_default = ((DataRowView)SelectedItem)["dien_giai"]?.ToString()?.Trim() ?? "";

            // Ki?m tra ?ã có bút toán ch?a (gi?ng WinForm newButton_Click: check ct00 ma_ct='PK6')
            if (!string.IsNullOrEmpty(sttRecList))
            {
                try
                {
                    string sqlChk = $"SELECT stt_rec FROM ct00 WHERE ma_ct='PK6' AND dbo.InList(stt_rec,'{sttRecList.Replace("'", "''")}',',')=1";
                    DataSet dsChk = await myDb.LoadDataset(sqlChk);
                    if (dsChk?.Tables[0]?.Rows.Count > 0)
                    {
                        OverwriteInfo        = sttRecList;
                        ShowConfirmOverwrite = true;
                        _pendingMaQs         = ma_qs_default;
                        _pendingSoCt         = so_ct_default;
                        _pendingDienGiai     = dien_giai_default;
                        _pendingTaggedRows   = taggedRows;
                        StateHasChanged();
                        return;
                    }
                }
                catch { }
            }

            await OpenDlgTaoBt(ma_qs_default, so_ct_default, dien_giai_default, taggedRows);
        }

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
                Ma_qs     = maQs,
                So_ct     = soCt,
                Dien_giai = dienGiai,
                IsOk      = false
            };
            DlgSoCt2   = "";
            DlgLoading = false;

            try
            {
                var lenDs = await myDb.LoadDataset("SELECT MAX(LEN(so_ct)) FROM ctpbbcc");
                if (lenDs?.Tables[0]?.Rows.Count > 0 && lenDs.Tables[0].Rows[0][0] != DBNull.Value)
                    DlgTaoBt.So_ct_length = Math.Max(Convert.ToInt32(lenDs.Tables[0].Rows[0][0]), 10);
            }
            catch { DlgTaoBt.So_ct_length = 10; }

            if (!string.IsNullOrEmpty(maQs))
                await LoadNewSoct(maQs, rows.Count);

            _pendingTaggedRows = rows;
            ShowDlgTaoBt = true;
            StateHasChanged();
        }

        // ===================================================
        // L?Y S? CT M?I (GetNewSoct trong WinForm)
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
                    ? $"SELECT transform, so_ct + 1 AS so_ct FROM dmqs WHERE ma_qs='{maQs.Trim().Replace("'", "''")}'"
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
                        // Luôn tính DlgSoCt2 — gi?ng WinForm txtso_ct2 luôn hi?n th?
                        // Khi 1 dòng: DlgSoCt2 = DlgTaoBt.So_ct (s? CT t? = s? CT ??n)
                        DlgSoCt2 = string.Format(DlgTaoBt.So_ct_format, DlgTaoBt.So_ct_num + rowCount - 1)
                                         .Trim().PadLeft(DlgTaoBt.So_ct_length);
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
        // KHI THAY ??I MÃ QS TRONG DIALOG
        // ===================================================
        protected async void OnDlgMaQsChanged(ExpandoObject value)
        {
            if (value == null) return;
            var dict = value as IDictionary<string, object>;
            DlgTaoBt.Ma_qs  = dict?.ContainsKey("ma_qs")  == true ? dict["ma_qs"]?.ToString()  ?? "" : "";
            DlgTaoBt.Ten_qs = dict?.ContainsKey("ten_qs") == true ? dict["ten_qs"]?.ToString() ?? "" : "";
            await LoadNewSoct(DlgTaoBt.Ma_qs, _pendingTaggedRows.Count);
        }

        // ===================================================
        // XÁC NH?N T?O BÚT TOÁN (OK)
        // WinForm newButton_Click: g?i FXbtpbcc_Tao_F4
        // ===================================================
        protected async void DlgTaoBt_OK()
        {
            if (string.IsNullOrWhiteSpace(DlgTaoBt.Ma_qs))
            {
                myStateMN.SetNewThongbao(Lap["Voucher book code is required!"]);
                return;
            }
            if (string.IsNullOrWhiteSpace(DlgTaoBt.So_ct))
            {
                myStateMN.SetNewThongbao(Lap["Voucher number is required!"]);
                return;
            }

            DlgLoading = true;
            StateHasChanged();

            try
            {
                // WinForm: FXbtpbcc_Tao_F4 @Nam, @ky, @ma_dvcs, @tk_no, @tk_co, @ma_qs, @so_ct, @ngay_ct, @dien_giai, @User_id
                int idx = 0;
                foreach (var dataRowView in _pendingTaggedRows)
                {
                    double soCtNum = DlgTaoBt.So_ct_num + idx++;
                    string soCtStr = string.IsNullOrEmpty(DlgTaoBt.So_ct_format)
                        ? DlgTaoBt.So_ct
                        : string.Format(DlgTaoBt.So_ct_format, soCtNum).Trim().PadLeft(DlgTaoBt.So_ct_length);

                    int    namVal  = Convert.ToInt32(dataRowView["nam"].ToString().Trim());
                    int    kyVal   = Convert.ToInt32(dataRowView["ky"].ToString().Trim());
                    string ngayCt  = GetEndOfPeriod(namVal, kyVal).ToString("yyyy-MM-dd");
                    string maDvcs  = dataRowView["ma_dvcs"]?.ToString()?.Trim() ?? "";
                    string tkNo    = dataRowView["tk_cp"]?.ToString()?.Trim() ?? "";
                    string tkCo    = dataRowView["tk_kh"]?.ToString()?.Trim() ?? "";
                    string userId  = curUser?.User_id.ToString() ?? "0";

                    string parNames = "@Nam;@thang;@ma_dvcs;@tk_no;@tk_co;@ma_qs;@so_ct;@ngay_ct;@dien_giai;@User_id";
                    string parVals  = $"{namVal};{kyVal};{maDvcs};{tkNo};{tkCo};" +
                                      $"{DlgTaoBt.Ma_qs.Trim()};{soCtStr.Trim()};{ngayCt};" +
                                      $"{DlgTaoBt.Dien_giai};{userId}";

                    await myDb.LoadStoreProcedure("FXbtpbcc_Tao_F4", parNames, parVals);
                }

                // B? tag sau khi t?o xong
                if (TblPH != null)
                    foreach (var rv in _pendingTaggedRows)
                        if (TblPH.Columns.Contains("tag")) rv["tag"] = false;

                myStateMN.SetNewThongbao(Lap["Operation completed successfully!"]);
                ShowDlgTaoBt = false;
                await Loaddata();
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao($"{Lap["Error creating voucher:"]} {ex.Message}");
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
        // F8 — XÓA BÚT TOÁN (delButton_Click trong WinForm)
        // ===================================================
        protected void V_XoaBt()
        {
            var taggedRows = GetTaggedRows();
            if (taggedRows.Count == 0)
            {
                myStateMN.SetNewThongbao(Lap["Must mark rows before performing this action!"]);
                return;
            }
            _pendingTaggedRows = taggedRows;
            ShowConfirmDelete  = true;
            StateHasChanged();
        }

        protected async void ConfirmDelete()
        {
            ShowConfirmDelete = false;
            try
            {
                // Ki?m tra khóa s? (gi?ng WinForm delButton_Click)
                string strDongSo = myOption.GetOptionsValue("M_NGAY_DONG_SO");
                if (!string.IsNullOrEmpty(strDongSo) && DateTime.TryParse(strDongSo, out DateTime ngayDongSo))
                {
                    DateTime ngayBatDau = GetStartOfPeriod(FilterNam, FilterKy1);
                    if (ngayBatDau <= ngayDongSo)
                    {
                        myStateMN.SetNewThongbao(Lap["Data is locked, cannot delete!"]);
                        StateHasChanged();
                        return;
                    }
                }

                // WinForm: g?i FXbtpbcc_Tao_F8 cho t?ng dòng ???c ?ánh d?u
                // Dùng ExecuteSqlQueryAsync ?? th?c thi DELETE (gi?ng FAbtpbts_TaoBase.ConfirmDelete)
                foreach (var dataRowView in _pendingTaggedRows)
                {
                    int    namVal = Convert.ToInt32(dataRowView["nam"].ToString().Trim());
                    int    kyVal  = Convert.ToInt32(dataRowView["ky"].ToString().Trim());
                    string maDvcs = dataRowView["ma_dvcs"]?.ToString()?.Trim() ?? "";
                    string tkNo   = dataRowView["tk_cp"]?.ToString()?.Trim() ?? "";
                    string tkCo   = dataRowView["tk_kh"]?.ToString()?.Trim() ?? "";

                    string safeMaDvcs = maDvcs.Replace("'", "''");
                    string safeTkNo   = tkNo.Replace("'", "''");
                    string safeTkCo   = tkCo.Replace("'", "''");

                    // Th?c thi SP xóa qua ExecuteSqlQueryAsync (gi?ng WinForm ExcuteNonQuery)
                    string sqlExec = $"EXEC [dbo].[FXbtpbcc_Tao_F8] {namVal}, {kyVal}, N'{safeMaDvcs}', N'{safeTkNo}', N'{safeTkCo}'";
                    await myDb.ExecuteSqlQueryAsync(sqlExec);
                }

                // B? tag
                if (TblPH != null)
                    foreach (var rv in _pendingTaggedRows)
                        if (TblPH.Columns.Contains("tag")) rv["tag"] = false;

                myStateMN.SetNewThongbao(Lap["Operation completed successfully!"]);

                // Reload d? li?u ?? refresh grid (gi?ng WinForm oBrowse_CTRL_R)
                await Loaddata();
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao($"{Lap["Error deleting voucher:"]} {ex.Message}");
                Console.WriteLine("ConfirmDelete error: " + ex.Message);
            }
            finally
            {
                StateHasChanged();
            }
        }

        protected void CancelDelete()
        {
            ShowConfirmDelete = false;
            StateHasChanged();
        }

        // ===================================================
        // F7 — IN / XEM BÁO CÁO
        // WinForm: FrmIn { Rows = array }.ShowDialog()
        //   ? GetData(row["stt_rec"]): EXEC [FXbtpbcc_Tao_F7] @stt_rec
        // ===================================================
        protected async void PrintVoucherAsync()
        {
            var taggedRows = GetTaggedRows();
            if (taggedRows.Count == 0)
            {
                myStateMN.SetNewThongbao(Lap["Must mark rows before performing this action!"]);
                return;
            }

            PrintQueue.Clear();

            foreach (var row in taggedRows)
            {
                string sttRec = row["stt_rec"]?.ToString()?.Trim() ?? "";
                int    namVal = row.Row.Table.Columns.Contains("nam") ? Convert.ToInt32(row["nam"]) : FilterNam;
                int    kyVal  = row.Row.Table.Columns.Contains("ky")  ? Convert.ToInt32(row["ky"])  : FilterKy1;

                DataSet dsPrint = await myDb.LoadStoreProcedure("FXbtpbcc_Tao_F7", "@stt_rec", sttRec);

                var reportModel = new FXbtpbcc_TaoReportModel
                {
                    MaDVCS   = row.Row.Table.Columns.Contains("ma_dvcs") ? row["ma_dvcs"]?.ToString() ?? "" : FilterMaDvcs,
                    Nam      = namVal,
                    Ky       = kyVal,
                    Ky2      = kyVal,
                    Ngay_ct2 = GetEndOfPeriod(namVal, kyVal),
                    LsData   = new List<FXbtpbcc_Tao_F7Detail>()
                };

                if (dsPrint?.Tables.Count > 0)
                    reportModel.LsData = myDb.ConvertDataTable<FXbtpbcc_Tao_F7Detail>(dsPrint.Tables[0]);

                PrintQueue.Add(new PrintJob
                {
                    Title = $"{Lap["Allocation voucher no."]} {row["so_ct"]?.ToString() ?? ""}",
                    Data  = new List<object> { reportModel }
                });
            }

            await InvokeAsync(StateHasChanged);
        }

        protected void ClosePrintWindow(PrintJob job)
        {
            if (PrintQueue.Contains(job))
            {
                PrintQueue.Remove(job);
                StateHasChanged();
            }
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
        // TI?N ÍCH TÍNH NGÀY ??U / CU?I K?
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
        // HÀNG ??I IN
        // ===================================================
        public class PrintJob
        {
            public string Id    { get; set; } = Guid.NewGuid().ToString();
            public string Title { get; set; }
            public List<object> Data { get; set; }
        }

        protected List<PrintJob> PrintQueue = new List<PrintJob>();

        // ===================================================
        // EXPORT
        // ===================================================
        public override async void ExportData(int type)
        {
            if (oBrowsePH != null)
                oBrowsePH.ExPortGrid(type);
            myStateMN.SetNewThongbao(Lap["Export file successfully!"]);
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
