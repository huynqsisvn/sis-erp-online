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
using System.Linq;
using System.Threading.Tasks;

namespace SisFx.Pages.Input.FxSuaKH
{
    public class FxSuaKHBase : ListBaseComponent
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
        protected string FilterKeyCc       = "";
        protected bool FilterSuaKh         = false;   // Checkbox "Lọc các công cụ đã điều chỉnh p.bổ"
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

        protected string ConfirmBtnText =>
            SelectedDmcc.Count > 0 ? $"{Lap["Confirm"]} ({SelectedDmcc.Count} {Lap["tools"]})" : Lap["Confirm"];

        // ===================================================
        // Store procedure names
        // ===================================================
        private string _storeQuery  = "";
        protected string _storeUpdate = "";

        // ===================================================
        // DxGrid / Browse ref
        // ===================================================
        protected FrmBrowse3Component oBrowse3;

        // ===================================================
        // KHỞI TẠO
        // ===================================================
        protected override void OnInitialized()
        {
            mPage        = "fxsuakh";
            sqlTableName = "ctkhcc";
            sqlTableView = "ctkhcc";
            SqlTableKey  = "ma_cc_me;nam;ky;ma_bpcc";
            SqlItemName  = "ma_cc_me";
            IsShowCopy   = false;
            IsShowImport = false;
            FrmType      = typeof(FrmFxSuaKH);

            if (string.IsNullOrEmpty(Menu_id))
                Menu_id = "wf.xc.04";

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

                var dvcs = myStateMN.GetMa_Dvcs();
                FilterMaDvcs = (dvcs == "ALL" || string.IsNullOrEmpty(dvcs)) ? "" : dvcs.Trim();

                PopupVisible = true;
                _ = PreloadDmccAsync();
                await LoadStoreProcNames();
                StateHasChanged();
            }
            else
            {
                if (oBrowse != null)
                    await oBrowse.FocusGridAsync();
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
                TblData = null;
                await InvokeAsync(StateHasChanged);

                // Xây dựng chuỗi @Advance giống WinForm StartUp.KeyFilter
                string advanceFilter = "";

                // Lọc theo danh sách công cụ (dbo.InList giống WinForm)
                if (!string.IsNullOrEmpty(FilterKeyCc))
                {
                    string safeList = string.Join(",",
                        FilterKeyCc.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => x.Trim().Replace("'", "''"))
                            .Where(x => !string.IsNullOrEmpty(x)));
                    if (!string.IsNullOrEmpty(safeList))
                        advanceFilter += $" AND dbo.InList(ma_cc_me, '{safeList}', ',') = 1";
                }

                // Lọc theo ĐVCS
                if (!string.IsNullOrEmpty(FilterMaDvcs))
                    advanceFilter += $" AND ma_dvcs LIKE '{FilterMaDvcs.Replace("'", "''").Trim()}%'";

                // Checkbox: Nếu KHÔNG tick → chỉ lấy công cụ chưa điều chỉnh (sua_kh = 0)
                if (!FilterSuaKh)
                    advanceFilter += " AND sua_kh = 0";

                if (!string.IsNullOrEmpty(_storeQuery))
                {
                    string paramNames  = "@Nam;@Ky1;@Ky2;@Advance";
                    string paramValues = $"{FilterNam};{FilterKy1};{FilterKy2};{advanceFilter}";
                    DataSet ds = await myDb.LoadStoreProcedure(_storeQuery, paramNames, paramValues);

                    if (ds != null && ds.Tables.Count > 0)
                        TblData = ds.Tables[ds.Tables.Count - 1];
                }
                else
                {
                    string maDvcsLike   = string.IsNullOrEmpty(FilterMaDvcs) ? "%" : FilterMaDvcs + "%";
                    string keyCondition = string.IsNullOrEmpty(FilterKeyCc) ? "1=1" : BuildInListCondition("c.ma_cc_me", FilterKeyCc);
                    string suaKhCond    = FilterSuaKh ? "1=1" : "c.sua_kh = 0";

                    string sql = $@"SELECT ROW_NUMBER() OVER (ORDER BY c.ma_cc_me, c.nam, c.ky) AS stt,
                        c.ma_cc_me, c.ma_cc, d.ten_cc, d.ten_cc2, c.ma_bpcc, c.nam, c.ky, c.ma_dvcs, 
                        c.nguyen_gia, c.gt_da_kh, c.gt_cl,
                        ISNULL(c.gt_kh_ky0, c.gt_kh_ky) AS gt_kh_ky0, -- THÊM DÒNG NÀY
                        c.gt_kh_ky,
                        ISNULL(c.gt_cl2, c.gt_cl) AS gt_cl2,
                        ISNULL(c.gt_cl2, c.gt_cl) AS gt_cl2,
                        ISNULL(c.sua_kh, 0) AS sua_kh,
                        ISNULL(c.dien_giai, '') AS dien_giai,
                        d.tk_cc, d.tk_kh, d.tk_cp, d.ma_phi, d.ma_bpht, d.ma_px, d.ma_vv, d.ma_nv
                        FROM {sqlTableView} c
                        LEFT JOIN dmcc d ON d.ma_cc_me = c.ma_cc_me
                        WHERE c.nam = {FilterNam}
                        AND c.ky BETWEEN {FilterKy1} AND {FilterKy2}
                        AND d.ma_dvcs LIKE '{maDvcsLike}'
                        AND ({keyCondition})
                        AND ({suaKhCond})
                        ORDER BY c.ma_cc_me, c.nam, c.ky";

                    DataSet ds = await myDb.LoadDataset(sql);
                    TblData = ds?.Tables[0];
                }

                // Đảm bảo cột tag tồn tại
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

        /// <summary>Mở form chỉnh sửa (F3) — giống WinForm oBrowse_F3</summary>
        public override void V_Sua()
        {
            DataRowView rview = SelectedItem as DataRowView;
            if (rview == null) { myStateMN.SetNewThongbao(Lap["No row selected!"]); return; }

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
        // XÓA — xóa các dòng đã đánh dấu tag (giống WinForm oBrowse_F8)
        // ===================================================
        public override async void V_Xoa()
        {
            var taggedRows = GetTaggedRows();

            if (taggedRows.Count == 0)
            {
                if (SelectedItem is DataRowView sel)
                    taggedRows.Add(sel);
            }

            if (taggedRows.Count == 0)
            {
                myStateMN.SetNewThongbao(Lap["No row selected!"]);
                return;
            }

            string confirmText;
            if (taggedRows.Count == 1)
            {
                var rv = taggedRows[0];
                string maCc  = rv["ma_cc_me"]?.ToString()?.Trim() ?? "";
                string maBp  = rv["ma_bpcc"]?.ToString()?.Trim() ?? "";
                string nam   = rv["nam"]?.ToString()?.Trim() ?? "";
                string ky    = rv["ky"]?.ToString()?.Trim() ?? "";
                confirmText = $"{Lap["Confirm delete"]} [{maCc} - {maBp} - {Lap["Year"]} {nam}, {Lap["Period"]} {ky}]?";
            }
            else
            {
                confirmText = $"{Lap["Confirm delete"]} {taggedRows.Count} {Lap["marked rows"]}?";
            }

            var confirm = await myDlSvc.ConfirmAsync(new MessageBoxOptions
            {
                Text             = confirmText,
                Title            = Lap["Confirm delete"],
                OkButtonText     = Lap["Yes"],
                CancelButtonText = Lap["No"],
                ShowIcon         = true,
                ShowCloseButton  = false,
                RenderStyle      = MessageBoxRenderStyle.Danger
            });

            if (confirm)
            {
                try
                {
                    // Giống WinForm: DELETE ctkhcc WHERE (nam=.. AND ky=.. AND ma_cc_me=.. AND ISNULL(ma_bpcc,'')=..)
                    string conditions = string.Join(" OR ", taggedRows.Select(rv =>
                    {
                        string nam   = rv["nam"]?.ToString()?.Trim() ?? "0";
                        string ky    = rv["ky"]?.ToString()?.Trim() ?? "0";
                        string maCc  = (rv["ma_cc_me"]?.ToString()?.Trim() ?? "").Replace("'", "''");
                        string maBp  = (rv["ma_bpcc"]?.ToString()?.Trim() ?? "").Replace("'", "''");
                        return $"(nam = {nam} AND ky = {ky} AND ma_cc_me = '{maCc}' AND ISNULL(ma_bpcc, '') = '{maBp}')";
                    }));

                    string sqlDel = $"DELETE ctkhcc WHERE ({conditions})";
                    await myDb.ExecuteSqlQueryAsync(sqlDel);

                    myStateMN.SetNewThongbao($"{Lap["Delete successfully"]} {taggedRows.Count} {Lap["rows"]}!");
                    await Loaddata();
                }
                catch (Exception ex)
                {
                    myStateMN.SetNewThongbao($"{Lap["Error deleting:"]} {ex.Message}");
                }
            }
            StateHasChanged();
        }

        private List<DataRowView> GetTaggedRows()
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
            await InvokeAsync(async () =>
            {
                try
                {
                    FilterMaDvcs = (maDvcs == "ALL" || string.IsNullOrEmpty(maDvcs)) ? "" : maDvcs.Trim();
                    FilterKeyCc = "";
                    SelectedDmcc.Clear();
                    DtMaCcFull = null;
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
                myStateMN.SetNewThongbao($"{Lap["Filter data successfully!"]} {Lap["Period"]} {FilterKy1}-{FilterKy2}/{FilterNam} — {TblData?.Rows?.Count ?? 0} {Lap["tools"]}.");
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao($"{Lap["Error filtering data:"]} {ex.Message}");
            }
            finally
            {
                ShowLoadding = false;
                StateHasChanged();
            }
        }

        // ===================================================
        // HELPERS CHO RAZOR
        // ===================================================
        protected void CloseFilter()
        {
            PopupVisible = false;
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
            if (oBrowse3 != null)
                oBrowse3.ExPortGrid(type);
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
