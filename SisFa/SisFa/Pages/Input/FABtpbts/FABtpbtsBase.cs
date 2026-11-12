using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using SisLib.List;
using SisCom.Model;
using SisData.Service;
using SisData.Data;
using SisLib.Repo;

namespace SisFa.Pages.Input.FABtpbts
{
    public class FABtpbtsBase : ListBaseComponent
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
        protected string FilterMaTs      = "";   // so_the_ts (mã tài sản)
        protected string FilterMaBpts    = "";   // ma_bpts   (bộ phận tính)

        // ===================================================
        // BIẾN MODAL CHỌN TÀI SẢN
        // ===================================================
        protected bool ShowModalDmts           = false;
        protected DataTable TblDmts            = null;
        private   DataTable DtMaTsFull         = null;
        protected string DmtsSearch            = "";
        protected HashSet<string> SelectedDmts = new(StringComparer.OrdinalIgnoreCase);

        protected bool IsAllDmtsChecked =>
            TblDmts != null
            && TblDmts.Rows.Count > 0
            && TblDmts.AsEnumerable().All(r => SelectedDmts.Contains(r["so_the_ts"]?.ToString() ?? ""));

        protected string ConfirmBtnText =>
            SelectedDmts.Count > 0 ? $"Xác nhận ({SelectedDmts.Count} tài sản)" : "Xác nhận";

        // ===================================================
        // BIẾN CONFIRM GHI ĐÈ
        // ===================================================
        protected bool ShowConfirmOverwrite = false;

        // ===================================================
        // KHỞI TẠO
        // ===================================================
        protected override void OnInitialized()
        {
            mPage        = "fabtpbts";
            sqlTableName = "ctpbts";
            sqlTableView = "v_ctpbts";
            SqlTableKey  = "so_the_ts;nam;ky;ma_bpts";
            SqlItemName  = "so_the_ts";
            IsShowCopy   = false;
            IsShowImport = false;

            if (string.IsNullOrEmpty(Menu_id))
                Menu_id = "wf.ct.08";

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
                _ = PreloadDmtsAsync();

                // Lần đầu render — tải dữ liệu theo ĐVCS hiện tại
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
        // LOAD DATA — hiển thị kết quả sau khi tính
        // ===================================================
        public override async Task Loaddata()
        {
            SelectedItem = null;
            try
            {
                string maDvcsLike  = string.IsNullOrEmpty(FilterMaDvcs) ? "%" : FilterMaDvcs + "%";
                string maTsCond    = BuildInListCondition("c.so_the_ts", FilterMaTs);
                string maBptsCond  = string.IsNullOrEmpty(FilterMaBpts)
                    ? "1=1"
                    : $"c.ma_bpts LIKE '{FilterMaBpts.Replace("'", "''")}%'";

                string sql = $"SELECT ROW_NUMBER() OVER (ORDER BY c.so_the_ts, c.ma_bpts, c.nam, c.ky) AS stt, " +
                             $"c.so_the_ts, d.ten_ts, d.ten_ts2, c.ma_bpts, c.nam, c.ky, c.ma_dvcs, " +
                             $"c.nguyen_gia, c.gt_da_kh, c.gt_cl, c.gt_kh_ky, " +
                             $"c.tk_ts, c.tk_kh, c.tk_cp, c.ma_phi, c.ma_bpht, c.ma_px, c.ma_vv, c.ma_nv " +
                             $"FROM {sqlTableView} c " +
                             $"LEFT JOIN dmts d ON d.so_the_ts = c.so_the_ts " +
                             $"WHERE c.nam = {FilterNam} " +
                             $"AND c.ky BETWEEN {FilterKy1} AND {FilterKy2} " +
                             $"AND c.ma_dvcs LIKE '{maDvcsLike}' " +
                             $"AND ({maTsCond}) " +
                             $"AND ({maBptsCond}) " +
                             $"ORDER BY c.so_the_ts, c.ma_bpts, c.nam, c.ky";

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
        // DVCS CHANGED (từ header toolbar)
        // ===================================================
        private async void OnDvcsChangedHandler(string maDvcs)
        {
            try
            {
                FilterMaDvcs = (maDvcs == "ALL" || string.IsNullOrEmpty(maDvcs)) ? "" : maDvcs.Trim();
                // Khi đổi ĐVCS, reset lại tài sản để tránh lỗi lọc chéo
                FilterMaTs = "";
                SelectedDmts.Clear();

                // Tự động chạy lệnh TÍNH TOÁN giống hệt FaTinhKH
                await ExecuteTinhPBTS();
            }
            catch (Exception ex) { Console.WriteLine("DvcsChanged error: " + ex.Message); }
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

            try
            {
                var ngayBatDau    = new DateTime(FilterNam, FilterKy1, 1);
                string strMoSo   = myOption.GetOptionsValue("M_NGAY_MO_SO");
                string strKhoaSo = myOption.GetOptionsValue("M_NGAY_KHOA_SO");

                if (!string.IsNullOrEmpty(strMoSo) && DateTime.TryParse(strMoSo, out DateTime ngayMoSo))
                {
                    if (ngayBatDau < ngayMoSo)
                    {
                        myStateMN.SetNewThongbao($"Kỳ bắt đầu ({ngayBatDau:MM/yyyy}) không được trước Ngày mở sổ ({ngayMoSo:dd/MM/yyyy}).");
                        return false;
                    }
                }
                if (!string.IsNullOrEmpty(strKhoaSo) && DateTime.TryParse(strKhoaSo, out DateTime ngayKhoaSo))
                {
                    if (ngayBatDau < ngayKhoaSo)
                    {
                        myStateMN.SetNewThongbao($"Kỳ bắt đầu ({ngayBatDau:MM/yyyy}) không được trước Ngày khóa sổ ({ngayKhoaSo:dd/MM/yyyy}).");
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
        // NÚT THỰC HIỆN TÍNH — kiểm tra ghi đè trước
        // ===================================================
        protected async void BtnTinhPBTS_Click()
        {
            if (!ValidateFilter())
                return;

            try
            {
                string maDvcsLike = string.IsNullOrEmpty(FilterMaDvcs) ? "%" : FilterMaDvcs + "%";
                string keyCond    = BuildSpKeyParam(FilterMaTs);

                // Winform FABtpbtsLoc: SELECT COUNT(1) FROM v_ctpbts WHERE nam=@Nam ...
                string sqlCheck =
                    $"SELECT ISNULL(COUNT(1),0) FROM v_ctpbts " +
                    $"WHERE nam={FilterNam} AND (ky BETWEEN {FilterKy1} AND {FilterKy2}) " +
                    $"AND ma_dvcs LIKE '{maDvcsLike}' " +
                    $"AND so_the_ts LIKE '{FilterMaTs.Replace("'", "''")}%' " +
                    $"AND ma_bpts LIKE '{FilterMaBpts.Replace("'", "''")}%'";

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

            await ExecuteTinhPBTS();
        }

        // ===================================================
        // THỰC THI SP — tương ứng store_proc trong drCommandInfo (Winform StartUp.cs)
        // ===================================================
        protected async Task ExecuteTinhPBTS()
        {
            ShowConfirmOverwrite = false;
            ShowLoadding         = true;
            StateHasChanged();

            try
            {
                // Tham số truyền vào SP (theo Winform StartUp.cs):
                // @Nam, @Ky, @Ky2, @so_the_ts, @ma_bpts, @Ma_dvcs
                string maDvcs  = FilterMaDvcs ?? "";
                string maTs    = FilterMaTs   ?? "";
                string maBpts  = FilterMaBpts ?? "";

                string paramNames  = "@Nam;@Ky;@Ky2;@so_the_ts;@ma_bpts;@Ma_dvcs";
                string paramValues = $"{FilterNam};{FilterKy1};{FilterKy2};{maTs};{maBpts};{maDvcs}";

                // Tên SP lấy từ menu wf.ct.08 → store_proc (giống Winform dùng strArray[0])
                string spName = "FABtpbts";
                if (curCommand != null && !string.IsNullOrEmpty(curCommand.Store_proc))
                {
                    var parts = curCommand.Store_proc.Split(';');
                    if (parts.Length > 0 && !string.IsNullOrEmpty(parts[0].Trim()))
                        spName = parts[0].Trim();
                }

                DataSet ds = await myDb.LoadStoreProcedure(spName, paramNames, paramValues);

                // Lấy bảng cuối cùng trong DataSet — giống Winform Tables[Count-1].DefaultView
                if (ds != null && ds.Tables.Count > 0)
                {
                    TblData      = ds.Tables[ds.Tables.Count - 1];
                    SelectedItem = TblData.DefaultView.Count > 0 ? TblData.DefaultView[0] : null;
                    myStateMN.SetNewThongbao($"Tính phân bổ khấu hao chi tiết thành công! Kỳ {FilterKy1}-{FilterKy2}/{FilterNam}.");
                }
                else
                {
                    TblData = null;
                    myStateMN.SetNewThongbao("Tính phân bổ khấu hao hoàn tất nhưng không có dữ liệu trả về.");
                }

                PopupVisible = false;
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao("Lỗi khi tính phân bổ khấu hao: " + ex.Message);
                Console.WriteLine("FABtpbts SP error: " + ex.Message);
            }
            finally
            {
                ShowLoadding = false;
                if (oBrowse != null)
                    await oBrowse.FocusGridAsync();
                StateHasChanged();
            }
        }

        /// <summary>
        /// Chuyển danh sách mã TS nối phẩy thành điều kiện SQL cho @key của SP.
        /// Trả về "1=1" khi không lọc, hoặc "dbo.InList(so_the_ts,'TS001,TS002',',')=1".
        /// </summary>
        private static string BuildSpKeyParam(string filterMaTs)
        {
            if (string.IsNullOrWhiteSpace(filterMaTs))
                return "1=1";

            var maCodes = filterMaTs
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim().Replace("'", "''"))
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();

            if (maCodes.Count == 0)
                return "1=1";

            string safeList = string.Join(",", maCodes);
            return $"dbo.InList(so_the_ts,'{safeList}',',')=1";
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

        protected void ClearKeyTs()
        {
            FilterMaTs = "";
            StateHasChanged();
        }

        // ===================================================
        // MODAL CHỌN TÀI SẢN
        // ===================================================
        private async Task PreloadDmtsAsync()
        {
            try
            {
                string sql = "SELECT CAST(0 AS BIT) AS tag, so_the_ts, ten_ts, ten_ts2, ma_bpts, ma_dvcs FROM dmts ORDER BY so_the_ts";
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
            if (!string.IsNullOrEmpty(FilterMaTs))
            {
                foreach (var ma in FilterMaTs.Split(',', StringSplitOptions.RemoveEmptyEntries))
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
            FilterMaTs = SelectedDmts.Count > 0
                ? string.Join(",", SelectedDmts.Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).OrderBy(x => x))
                : "";
            ShowModalDmts = false;
            StateHasChanged();
        }

        protected void CloseModalDmts()
        {
            ShowModalDmts = false;
            SelectedDmts  = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(FilterMaTs))
            {
                foreach (var ma in FilterMaTs.Split(',', StringSplitOptions.RemoveEmptyEntries))
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
            if (oBrowse != null)
                oBrowse.ExPortGrid(type);
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
    }

}
