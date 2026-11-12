using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using SisLib.List;
using SisCom.Model;
using SisData.Service;
using SisData.Data;
using SisLib.Repo;

namespace SisFx.Pages.Input.FXBtpbcc
{
    public class FXBtpbccBase : ListBaseComponent
    {
        // ===================================================
        // BI?N FILTER
        // ===================================================
        protected DxPopup popFilter;
        protected bool PopupVisible      = false;
        protected int  FilterNam;
        protected int  FilterKy1         = 1;
        protected int  FilterKy2         = 12;
        protected string FilterMaDvcs    = "";
        protected string FilterMaCc      = "";   // ma_cc_me (mã công c?)
        protected string FilterMaBpts    = "";   // ma_bpcc  (mã b? ph?n s? d?ng)

        // ===================================================
        // BI?N CONFIRM GHI ?È
        // ===================================================
        protected bool ShowConfirmOverwrite = false;

        // ===================================================
        // KH?I T?O
        // ===================================================
        protected override void OnInitialized()
        {
            mPage        = "fxbtpbcc";
            sqlTableName = "ctpbcc";
            sqlTableView = "v_ctpbcc";
            SqlTableKey  = "ma_cc_me;nam;ky;ma_bpts";
            SqlItemName  = "ma_cc_me";
            IsShowCopy   = false;
            IsShowImport = false;

            if (string.IsNullOrEmpty(Menu_id))
                Menu_id = "wf.xc.05";

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
                    myStateMN.SetToolBar(new MyToolbar("New",     false));
                    myStateMN.SetToolBar(new MyToolbar("Copy",    false));
                    myStateMN.SetToolBar(new MyToolbar("Import",  false));
                    myStateMN.SetToolBar(new MyToolbar("Edit",    false));
                    myStateMN.SetToolBar(new MyToolbar("Save",    false));
                    myStateMN.SetToolBar(new MyToolbar("Delete",  false));
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

                var dvcs = myStateMN.GetMa_Dvcs();
                FilterMaDvcs = (dvcs == "ALL" || string.IsNullOrEmpty(dvcs)) ? "" : dvcs.Trim();

                PopupVisible = true;
                _ = Loaddata();
                StateHasChanged();
            }
            else
            {
                if (oBrowse != null)
                    await oBrowse.FocusGridAsync();
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
                    fbrowse   = IsLangEn ? curCommand.Vbrowse2 : curCommand.Vbrowse1;
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
        // LOAD DATA — hi?n th? k?t qu?
        // ===================================================
        public override async Task Loaddata()
        {
            SelectedItem = null;
            try
            {
                string maDvcsLike = string.IsNullOrEmpty(FilterMaDvcs) ? "%" : FilterMaDvcs + "%";
                string maCcCond   = string.IsNullOrEmpty(FilterMaCc)
                    ? "1=1"
                    : $"c.ma_cc_me LIKE '{FilterMaCc.Replace("'", "''")}%'";
                string maBpCond   = string.IsNullOrEmpty(FilterMaBpts)
                    ? "1=1"
                    : $"c.ma_bpts LIKE '{FilterMaBpts.Replace("'", "''")}%'";

                string sql = $"SELECT ROW_NUMBER() OVER (ORDER BY c.ma_cc_me, c.ma_bpts, c.nam, c.ky) AS stt, " +
                             $"c.ma_cc_me, d.ten_cc, d.ten_cc2, c.ma_bpts, c.nam, c.ky, c.ma_dvcs, " +
                             $"c.nguyen_gia, c.gt_da_kh, c.gt_cl, c.gt_kh_ky, " +
                             $"c.tk_cc, c.tk_kh, c.tk_cp, c.ma_phi, c.ma_bpht, c.ma_px, c.ma_vv, c.ma_nv " +
                             $"FROM {sqlTableView} c " +
                             $"LEFT JOIN dmcc d ON d.ma_cc_me = c.ma_cc_me " +
                             $"WHERE c.nam = {FilterNam} " +
                             $"AND c.ky BETWEEN {FilterKy1} AND {FilterKy2} " +
                             $"AND c.ma_dvcs LIKE '{maDvcsLike}' " +
                             $"AND ({maCcCond}) " +
                             $"AND ({maBpCond}) " +
                             $"ORDER BY c.ma_cc_me, c.ma_bpts, c.nam, c.ky";

                DataSet ds = await myDb.LoadDataset(sql);
                TblData = ds?.Tables[0];

                if (TblData != null && TblData.DefaultView.Count > 0)
                    SelectedItem = TblData.DefaultView[0];

                if (oBrowse != null)
                    await oBrowse.FocusGridAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Loaddata error: " + ex.Message);
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
                case ActionTask.Search:
                    PopupVisible = true;
                    StateHasChanged();
                    break;
                case ActionTask.Export:
                    ExportData(1);
                    break;
            }
        }

        // ===================================================
        // DVCS CHANGED
        // ===================================================
        private async void OnDvcsChangedHandler(string maDvcs)
        {
            try
            {
                FilterMaDvcs = (maDvcs == "ALL" || string.IsNullOrEmpty(maDvcs)) ? "" : maDvcs.Trim();
                // Khi ??i ?VCS, reset l?i công c? ?? tránh l?c chéo
                FilterMaCc = "";
                await ExecuteTinhPBCC();
            }
            catch (Exception ex)
            {
                Console.WriteLine("DvcsChanged error: " + ex.Message);
            }
        }

        // ===================================================
        // VALIDATE FILTER — gi?ng WinForm FABtpbtsLoc.ConfirmGridView_OnOk
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

            try
            {
                var ngayBatDau    = new DateTime(FilterNam, FilterKy1, 1);
                string strMoSo   = myOption.GetOptionsValue("M_NGAY_MO_SO");
                string strKhoaSo = myOption.GetOptionsValue("M_NGAY_KHOA_SO");

                if (!string.IsNullOrEmpty(strMoSo) && DateTime.TryParse(strMoSo, out DateTime ngayMoSo))
                {
                    if (ngayBatDau < ngayMoSo)
                    {
                        myStateMN.SetNewThongbao($"{Lap["Start period"]} ({ngayBatDau:MM/yyyy}) {Lap["cannot be before book opening date"]} ({ngayMoSo:dd/MM/yyyy}).");
                        return false;
                    }
                }
                if (!string.IsNullOrEmpty(strKhoaSo) && DateTime.TryParse(strKhoaSo, out DateTime ngayKhoaSo))
                {
                    if (ngayBatDau < ngayKhoaSo)
                    {
                        myStateMN.SetNewThongbao($"{Lap["Start period"]} ({ngayBatDau:MM/yyyy}) {Lap["cannot be before book closing date"]} ({ngayKhoaSo:dd/MM/yyyy}).");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Validate date error: " + ex.Message);
            }

            return true;
        }

        // ===================================================
        // NÚT TH?C HI?N TÍNH — ki?m tra ghi ?è tr??c
        // Gi?ng WinForm: SELECT COUNT(1) FROM v_ctpbcc WHERE ...
        // ===================================================
        protected async void BtnTinhPBCC_Click()
        {
            if (!ValidateFilter())
                return;

            try
            {
                string maDvcsLike = string.IsNullOrEmpty(FilterMaDvcs) ? "%" : FilterMaDvcs + "%";
                string safeMaCc   = string.IsNullOrEmpty(FilterMaCc) ? "" : FilterMaCc.Replace("'", "''");
                string safeMaBp   = string.IsNullOrEmpty(FilterMaBpts) ? "" : FilterMaBpts.Replace("'", "''");

                string sqlCheck =
                    $"SELECT ISNULL(COUNT(1),0) FROM v_ctpbcc " +
                    $"WHERE nam = {FilterNam} " +
                    $"AND (ky BETWEEN {FilterKy1} AND {FilterKy2}) " +
                    $"AND ma_cc_me LIKE '{safeMaCc}%' " +
                    $"AND ma_bpts LIKE '{safeMaBp}%' " +
                    $"AND ma_dvcs LIKE '{maDvcsLike}'";

                DataSet dsChk = await myDb.LoadDataset(sqlCheck);
                if (dsChk?.Tables[0]?.Rows.Count > 0)
                {
                    decimal existing = Convert.ToDecimal(dsChk.Tables[0].Rows[0][0]);
                    if (existing > 0)
                    {
                        ShowConfirmOverwrite = true;
                        StateHasChanged();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Check overwrite error: " + ex.Message);
            }

            await ExecuteTinhPBCC();
        }

        // ===================================================
        // TH?C THI SP TÍNH PHÂN B? CÔNG C? D?NG C?
        // Gi?ng WinForm StartUp.cs: cmdQuery v?i @Nam, @Ky, @Ky2, @ma_cc_me, @ma_bpcc, @Ma_dvcs
        // ===================================================
        protected async Task ExecuteTinhPBCC()
        {
            ShowConfirmOverwrite = false;
            ShowLoadding         = true;
            await InvokeAsync(StateHasChanged);

            try
            {
                string maDvcs = FilterMaDvcs ?? "";
                string maCc   = FilterMaCc   ?? "";
                string maBp   = FilterMaBpts ?? "";

                string paramNames  = "@Nam;@Ky;@Ky2;@ma_cc_me;@ma_bpcc;@Ma_dvcs";
                string paramValues = $"{FilterNam};{FilterKy1};{FilterKy2};{maCc};{maBp};{maDvcs}";

                // L?y tên SP t? menu wf.xc.05 ? store_proc (gi?ng WinForm strArray[0])
                string spName = "FXBtpbcc";
                if (curCommand != null && !string.IsNullOrEmpty(curCommand.Store_proc))
                {
                    var parts = curCommand.Store_proc.Split(';');
                    if (parts.Length > 0 && !string.IsNullOrEmpty(parts[0].Trim()))
                        spName = parts[0].Trim();
                }

                DataSet ds = await myDb.LoadStoreProcedure(spName, paramNames, paramValues);

                if (ds != null && ds.Tables.Count > 0)
                {
                    TblData      = ds.Tables[ds.Tables.Count - 1];
                    SelectedItem = TblData.DefaultView.Count > 0 ? TblData.DefaultView[0] : null;
                    myStateMN.SetNewThongbao($"{Lap["Calculate allocation successfully!"]} {Lap["Period"]} {FilterKy1}-{FilterKy2}/{FilterNam} — {TblData.Rows.Count} {Lap["tools"]}.");
                }
                else
                {
                    TblData = null;
                    myStateMN.SetNewThongbao(Lap["Allocation calculation completed but no data returned."]);
                }

                PopupVisible = false;
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao($"{Lap["Error calculating allocation:"]} {ex.Message}");
                Console.WriteLine("FXBtpbcc SP error: " + ex.Message);
            }
            finally
            {
                ShowLoadding = false;
                if (oBrowse != null)
                    await oBrowse.FocusGridAsync();
                await InvokeAsync(StateHasChanged);
            }
        }

        // ===================================================
        // HELPERS
        // ===================================================
        protected void CloseFilter()
        {
            PopupVisible = false;
            StateHasChanged();
        }

        protected void CancelOverwrite()
        {
            ShowConfirmOverwrite = false;
            StateHasChanged();
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
