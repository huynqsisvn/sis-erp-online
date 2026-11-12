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

namespace SisFx.Pages.Input.FxTinhKH
{
    public class FxTinhKHBase : ListBaseComponent
    {
        // ===================================================
        // BIẾN FILTER
        // ===================================================
        protected DxPopup popFilter;
        protected bool PopupVisible      = false;
        protected int  FilterNam;
        protected int  FilterKy1         = 1;
        protected int  FilterKy2         = 12;
        protected string FilterMaDvcs    = "";
        protected string FilterKeyCc     = "";
        protected List<string> FilterErrors = new();

        // ===================================================
        // BIẾN MODAL CHỌN CÔNG CỤ
        // ===================================================
        protected bool ShowModalDmcc         = false;
        protected DataTable TblDmcc          = null;
        private   DataTable DtMaCcFull       = null;
        protected string DmccSearch          = "";
        protected HashSet<string> SelectedDmcc = new(StringComparer.OrdinalIgnoreCase);

        protected bool IsAllDmccChecked =>
            TblDmcc != null
            && TblDmcc.Rows.Count > 0
            && TblDmcc.AsEnumerable().All(r => SelectedDmcc.Contains(r["ma_cc_me"]?.ToString() ?? ""));

        // ===================================================
        // BIẾN CONFIRM GHI ĐÈ
        // ===================================================
        protected bool ShowConfirmOverwrite = false;

        // ===================================================
        // KHỞI TẠO
        // ===================================================
        protected override void OnInitialized()
        {
            mPage        = "fxtinhkh";
            sqlTableName = "ctkhcc";
            sqlTableView = "ctkhcc";
            SqlTableKey  = "ma_cc_me;nam;ky";
            SqlItemName  = "ma_cc_me";
            IsShowCopy   = false;
            IsShowImport = false;

            if (string.IsNullOrEmpty(Menu_id))
                Menu_id = "wf.xc.03";

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
                _ = PreloadDmccAsync();
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
        // LOAD DATA
        // ===================================================
        public override async Task Loaddata()
        {
            SelectedItem = null;
            try
            {
                string maDvcsLike   = string.IsNullOrEmpty(FilterMaDvcs) ? "%" : FilterMaDvcs + "%";
                string keyCondition = string.IsNullOrEmpty(FilterKeyCc)  ? "1=1" : BuildInListCondition("c.ma_cc_me", FilterKeyCc);

                string sql = BuildResultSql(maDvcsLike, keyCondition);
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

        private string BuildResultSql(string maDvcsLike, string keyCondition)
        {
            string dvcsCondition = $"d.ma_dvcs LIKE '{maDvcsLike}'";

            return $"SELECT ROW_NUMBER() OVER (ORDER BY c.ma_cc_me, c.nam, c.ky) AS stt, " +
                   $"c.ma_cc_me, d.ten_cc, d.ten_cc2, c.nam, c.ky, c.ma_dvcs, " +
                   $"c.nguyen_gia, c.gt_da_kh, c.gt_cl, c.gt_kh_ky, " +
                   $"d.tk_cc, d.tk_kh, d.tk_cp, d.ma_phi, d.ma_bpht, d.ma_px, d.ma_vv, d.ma_nv " +
                   $"FROM {sqlTableView} c " +
                   $"LEFT JOIN dmcc d ON d.ma_cc_me = c.ma_cc_me " +
                   $"WHERE c.nam = {FilterNam} " +
                   $"AND c.ky BETWEEN {FilterKy1} AND {FilterKy2} " +
                   $"AND ({dvcsCondition}) " +
                   $"AND ({keyCondition}) " +
                   $"ORDER BY c.ma_cc_me, c.nam, c.ky";
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
                FilterKeyCc = "";
                SelectedDmcc.Clear();
                DtMaCcFull = null;
                await ExecuteTinhPhanBo();
            }
            catch (Exception ex)
            {
                Console.WriteLine("DvcsChanged error: " + ex.Message);
            }
        }

        protected void OnDvcsSelectChanged()
        {
            FilterKeyCc = "";
            SelectedDmcc.Clear();
            DtMaCcFull = null;
            StateHasChanged();
        }

        // ===================================================
        // VALIDATE FILTER
        // ===================================================
        protected bool ValidateFilter()
        {
            FilterErrors.Clear();

            if (FilterNam <= 0)
                FilterErrors.Add(Lap["Year is invalid (must be > 0)."]);
            if (FilterKy1 < 1 || FilterKy1 > 12)
                FilterErrors.Add(Lap["From period must be 1 to 12."]);
            if (FilterKy2 < 1 || FilterKy2 > 12)
                FilterErrors.Add(Lap["To period must be 1 to 12."]);
            if (FilterKy1 > FilterKy2)
                FilterErrors.Add(Lap["From period cannot be greater than To period."]);

            try
            {
                var ngayBatDau    = new DateTime(FilterNam, FilterKy1, 1);
                string strMoSo   = myOption.GetOptionsValue("M_NGAY_MO_SO");
                string strKhoaSo = myOption.GetOptionsValue("M_NGAY_KHOA_SO");

                if (!string.IsNullOrEmpty(strMoSo) && DateTime.TryParse(strMoSo, out DateTime ngayMoSo))
                {
                    if (ngayBatDau < ngayMoSo)
                        FilterErrors.Add($"{Lap["Start period"]} ({ngayBatDau:MM/yyyy}) {Lap["cannot be before book opening date"]} ({ngayMoSo:dd/MM/yyyy}).");
                }
                if (!string.IsNullOrEmpty(strKhoaSo) && DateTime.TryParse(strKhoaSo, out DateTime ngayKhoaSo))
                {
                    if (ngayBatDau < ngayKhoaSo)
                        FilterErrors.Add($"{Lap["Start period"]} ({ngayBatDau:MM/yyyy}) {Lap["cannot be before book closing date"]} ({ngayKhoaSo:dd/MM/yyyy}).");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Validate date error: " + ex.Message);
            }

            return FilterErrors.Count == 0;
        }

        // ===================================================
        // NÚT THỰC HIỆN TÍNH PHÂN BỔ — kiểm tra ghi đè trước
        // ===================================================
        protected async void BtnTinhPhanBo_Click()
        {
            if (!ValidateFilter())
            {
                StateHasChanged();
                return;
            }

            try
            {
                string dvcs = myStateMN.GetMa_Dvcs();
                string currentDvcs  = (dvcs == "ALL" || string.IsNullOrEmpty(dvcs)) ? "" : dvcs.Trim();
                string maDvcsLike   = string.IsNullOrEmpty(currentDvcs) ? "%" : currentDvcs + "%";

                string keyCond      = BuildSpKeyParam(FilterKeyCc);
                string dvcsCondition = string.IsNullOrEmpty(FilterKeyCc)
                    ? $"ma_dvcs LIKE '{maDvcsLike}'"
                    : "1=1";

                string sqlCheck =
                    $"SELECT ISNULL(SUM(gt_kh_ky),0) FROM ctkhcc " +
                    $"WHERE nam={FilterNam} AND ky BETWEEN {FilterKy1} AND {FilterKy2} " +
                    $"AND ({dvcsCondition}) AND ({keyCond})";

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

            await ExecuteTinhPhanBo();
        }

        protected async Task ExecuteTinhPhanBo()
        {
            ShowConfirmOverwrite = false;
            ShowLoadding = true;
            await InvokeAsync(StateHasChanged);

            try
            {
                string keyParam = BuildSpKeyParam(FilterKeyCc);
                string maDvcs   = FilterMaDvcs ?? "";
                int userId      = curUser?.User_id ?? 0;

                string paramNames  = "@nam;@ky1;@ky2;@key;@user_id;@Ma_dvcs";
                string paramValues = $"{FilterNam};{FilterKy1};{FilterKy2};{keyParam};{userId};{maDvcs}";

                DataSet ds = await myDb.LoadStoreProcedure("Fxtinhkh", paramNames, paramValues);

                if (ds != null && ds.Tables.Count > 0)
                {
                    TblData = ds.Tables[ds.Tables.Count - 1];
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
                Console.WriteLine("Fxtinhkh SP error: " + ex.Message);
            }
            finally
            {
                ShowLoadding = false;
                if (oBrowse != null)
                    await oBrowse.FocusGridAsync();
                await InvokeAsync(StateHasChanged);
            }
        }

        private static string BuildSpKeyParam(string filterKeyCc)
        {
            if (string.IsNullOrWhiteSpace(filterKeyCc))
                return "1=1";

            var maCodes = filterKeyCc
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim().Replace("'", "''"))
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();

            if (maCodes.Count == 0)
                return "1=1";

            string safeList = string.Join(",", maCodes);
            return $"dbo.InList(ma_cc_me,'{safeList}',',')=1";
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

        protected void ClearKeyCc()
        {
            FilterKeyCc = "";
            StateHasChanged();
        }

        // ===================================================
        // MODAL CHỌN CÔNG CỤ
        // ===================================================
        private async Task PreloadDmccAsync()
        {
            try
            {
                string dvcsWhere = string.IsNullOrEmpty(FilterMaDvcs)
                    ? ""
                    : $" WHERE ma_dvcs LIKE '{FilterMaDvcs.Replace("'", "''")}%'";
                string sql = $"SELECT CAST(0 AS BIT) AS tag, ma_cc_me, ten_cc, ten_cc2, ma_dvcs FROM dmcc{dvcsWhere} ORDER BY ma_cc_me";
                DataSet ds = await myDb.LoadDataset(sql);
                DtMaCcFull = ds?.Tables[0] ?? new DataTable();
            }
            catch (Exception ex)
            {
                Console.WriteLine("PreloadDmcc error: " + ex.Message);
                DtMaCcFull = new DataTable();
            }
        }

        protected async Task OpenDmccModal()
        {
            DmccSearch = "";

            SelectedDmcc = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(FilterKeyCc))
            {
                foreach (var ma in FilterKeyCc.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    string trimmed = ma.Trim();
                    if (!string.IsNullOrEmpty(trimmed))
                        SelectedDmcc.Add(trimmed);
                }
            }

            if (DtMaCcFull == null)
                await PreloadDmccAsync();

            ApplyDmccFilter();
            ShowModalDmcc = true;
            StateHasChanged();
        }

        private void ApplyDmccFilter()
        {
            if (DtMaCcFull == null)
            {
                TblDmcc = new DataTable();
                return;
            }

            var filters = new List<string>();

            if (!string.IsNullOrEmpty(FilterMaDvcs))
            {
                string dvcs = FilterMaDvcs.Trim().Replace("'", "''");
                filters.Add($"ma_dvcs LIKE '{dvcs}%'");
            }

            if (!string.IsNullOrEmpty(DmccSearch))
            {
                string s = DmccSearch.Trim().Replace("'", "''");
                filters.Add($"(ma_cc_me LIKE '%{s}%' OR ten_cc LIKE '%{s}%' OR ten_cc2 LIKE '%{s}%')");
            }

            DtMaCcFull.DefaultView.RowFilter = filters.Count > 0 ? string.Join(" AND ", filters) : "";
            TblDmcc = DtMaCcFull.DefaultView.ToTable();

            foreach (DataRow r in TblDmcc.Rows)
            {
                if (r["ma_cc_me"] != DBNull.Value)
                    r["ma_cc_me"] = r["ma_cc_me"].ToString().Trim();
            }
        }

        protected void DoSearchDmcc()
        {
            ApplyDmccFilter();
            StateHasChanged();
        }

        protected void ToggleDmccRow(string maCc)
        {
            if (SelectedDmcc.Contains(maCc))
                SelectedDmcc.Remove(maCc);
            else
                SelectedDmcc.Add(maCc);
            StateHasChanged();
        }

        protected void ToggleAllDmcc(ChangeEventArgs e)
        {
            bool check = e.Value is bool b && b;
            if (check)
            {
                foreach (DataRow row in TblDmcc.Rows)
                {
                    string ma = row["ma_cc_me"]?.ToString() ?? "";
                    if (!string.IsNullOrEmpty(ma)) SelectedDmcc.Add(ma);
                }
            }
            else
            {
                foreach (DataRow row in TblDmcc.Rows)
                    SelectedDmcc.Remove(row["ma_cc_me"]?.ToString() ?? "");
            }
            StateHasChanged();
        }

        protected void SelectAllDmcc()
        {
            if (TblDmcc == null) return;
            foreach (DataRow row in TblDmcc.Rows)
            {
                string ma = row["ma_cc_me"]?.ToString() ?? "";
                if (!string.IsNullOrEmpty(ma)) SelectedDmcc.Add(ma);
            }
            StateHasChanged();
        }

        protected void ClearDmccSelection()
        {
            SelectedDmcc.Clear();
            StateHasChanged();
        }

        protected void ConfirmDmccSelection()
        {
            FilterKeyCc = SelectedDmcc.Count > 0
                ? string.Join(",", SelectedDmcc.Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).OrderBy(x => x))
                : "";
            ShowModalDmcc = false;
            StateHasChanged();
        }

        protected void CloseModalDmcc()
        {
            ShowModalDmcc = false;
            SelectedDmcc = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(FilterKeyCc))
            {
                foreach (var ma in FilterKeyCc.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    string trimmed = ma.Trim();
                    if (!string.IsNullOrEmpty(trimmed))
                        SelectedDmcc.Add(trimmed);
                }
            }
            StateHasChanged();
        }

        // ===================================================
        // EXPORT
        // ===================================================
        public override async void ExportData(int type)
        {
            if (oBrowse != null)
                oBrowse.ExPortGrid(type);
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
