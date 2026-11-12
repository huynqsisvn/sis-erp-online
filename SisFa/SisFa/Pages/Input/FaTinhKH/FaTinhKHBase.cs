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

namespace SisFa.Pages.Input.FaTinhKH
{
    public class FaTinhKHBase : ListBaseComponent
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
        protected string FilterKeyTs     = "";
        protected List<string> FilterErrors = new();

        // ===================================================
        // BIẾN MODAL CHỌN TÀI SẢN
        // ===================================================
        protected bool ShowModalDmts         = false;
        protected DataTable TblDmts          = null;  // bảng đã lọc — hiển thị trong modal
        private   DataTable DtMaTsFull       = null;  // cache toàn bộ dmts nạp sẵn một lần
        protected string DmtsSearch          = "";
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
            mPage        = "fatinhkh";
            sqlTableName = "ctkhts";
            sqlTableView = "ctkhts";
            SqlTableKey  = "so_the_ts;nam;ky";
            SqlItemName  = "so_the_ts";
            IsShowCopy   = false;
            IsShowImport = false;

            if (string.IsNullOrEmpty(Menu_id))
                Menu_id = "wf.kh.01";

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

                // Lấy mã ĐVCS hiện tại từ myStateMN (realtime)
                var dvcs = myStateMN.GetMa_Dvcs();
                FilterMaDvcs = (dvcs == "ALL" || string.IsNullOrEmpty(dvcs)) ? "" : dvcs.Trim();

                PopupVisible = true;
                // Nạp sẵn toàn bộ dmts vào cache (giống Winform nạp tại FrmIngia_tbLoc_Loaded)
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
                // Truyền trực tiếp chuỗi lọc từ biến trạng thái hiện tại
                string maDvcsLike = string.IsNullOrEmpty(FilterMaDvcs) ? "%" : FilterMaDvcs + "%";
                string keyCondition = string.IsNullOrEmpty(FilterKeyTs) ? "1=1" : BuildInListCondition("c.so_the_ts", FilterKeyTs);

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
            // LUÔN LUÔN LỌC THEO ĐVCS (dùng d.ma_dvcs của bảng danh mục)
            string dvcsCondition = $"d.ma_dvcs LIKE '{maDvcsLike}'";

            return $"SELECT ROW_NUMBER() OVER (ORDER BY c.so_the_ts, c.ma_bpts, c.nam, c.ky) AS stt, " +
                   $"c.so_the_ts, d.ten_ts, d.ten_ts2, c.ma_bpts, c.nam, c.ky, c.ma_dvcs, " +
                   $"c.nguyen_gia, c.gt_da_kh, c.gt_cl, c.gt_kh_ky, " +
                   // ĐÃ SỬA: Đổi c. thành d. cho các cột tk_ts, tk_kh, ma_phi...
                   $"d.tk_ts, d.tk_kh, d.tk_cp, d.ma_phi, d.ma_bpht, d.ma_px, d.ma_vv, d.ma_nv " +
                   $"FROM {sqlTableView} c " +
                   $"LEFT JOIN dmts d ON d.so_the_ts = c.so_the_ts " +
                   $"WHERE c.nam = {FilterNam} " +
                   $"AND c.ky BETWEEN {FilterKy1} AND {FilterKy2} " +
                   $"AND ({dvcsCondition}) " +
                   $"AND ({keyCondition}) " +
                   $"ORDER BY c.so_the_ts, c.ma_bpts, c.nam, c.ky";
        }
        // Helper: build dbo.InList condition hoặc fallback IN list
        private static string BuildInListCondition(string column, string commaList)
        {
            if (string.IsNullOrWhiteSpace(commaList)) return "1=1";
            // Dùng hàm scalar dbo.InList nếu SP yêu cầu, ngược lại dùng IN (...)
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
                // 1. Cập nhật ngay mã ĐVCS mới nhất vào biến
                FilterMaDvcs = (maDvcs == "ALL" || string.IsNullOrEmpty(maDvcs)) ? "" : maDvcs.Trim();

                // 2. Tự động THỰC HIỆN TÍNH KHẤU HAO luôn cho ĐVCS mới
                // Hàm này sẽ chạy Stored Procedure và tự động đẩy kết quả mới nhất ra lưới
                await ExecuteTinhKH();
            }
            catch (Exception ex)
            {
                Console.WriteLine("DvcsChanged error: " + ex.Message);
            }
        }

        protected void OnDvcsSelectChanged(ExpandoObject value)
        {
            // Khi đổi ĐVCS → xóa danh sách TS đã chọn (vì TS cũ có thể không thuộc ĐVCS mới)
            FilterKeyTs = "";
            SelectedDmts.Clear();
            StateHasChanged();
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

            // Validate NgayBatDau >= NgayMoSo và NgayKhoaSo
            try
            {
                var ngayBatDau     = new DateTime(FilterNam, FilterKy1, 1);
                string strMoSo    = myOption.GetOptionsValue("M_NGAY_MO_SO");
                string strKhoaSo  = myOption.GetOptionsValue("M_NGAY_KHOA_SO");

                if (!string.IsNullOrEmpty(strMoSo) && DateTime.TryParse(strMoSo, out DateTime ngayMoSo))
                {
                    if (ngayBatDau < ngayMoSo)
                        FilterErrors.Add($"Kỳ bắt đầu ({ngayBatDau:MM/yyyy}) không được trước Ngày mở sổ ({ngayMoSo:dd/MM/yyyy}).");
                }
                if (!string.IsNullOrEmpty(strKhoaSo) && DateTime.TryParse(strKhoaSo, out DateTime ngayKhoaSo))
                {
                    if (ngayBatDau < ngayKhoaSo)
                        FilterErrors.Add($"Kỳ bắt đầu ({ngayBatDau:MM/yyyy}) không được trước Ngày khóa sổ ({ngayKhoaSo:dd/MM/yyyy}).");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Validate date error: " + ex.Message);
            }

            return FilterErrors.Count == 0;
        }

        // ===================================================
        // NÚT THỰC HIỆN TÍNH KHẤU HAO — kiểm tra ghi đè trước
        // ===================================================
        protected async void BtnTinhKH_Click()
        {
            if (!ValidateFilter())
            {
                StateHasChanged();
                return;
            }

            try
            {
                // Lấy realtime giống hệt Inctpnd
                string dvcs = myStateMN.GetMa_Dvcs();
                string currentDvcs = (dvcs == "ALL" || string.IsNullOrEmpty(dvcs)) ? "" : dvcs.Trim();
                string maDvcsLike = string.IsNullOrEmpty(currentDvcs) ? "%" : currentDvcs + "%";

                string keyCond = BuildSpKeyParam(FilterKeyTs);
                string dvcsCondition = string.IsNullOrEmpty(FilterKeyTs)
                    ? $"ma_dvcs LIKE '{maDvcsLike}'"
                    : "1=1";

                string sqlCheck =
                    $"SELECT ISNULL(SUM(gt_kh_ky),0) FROM ctkhts " +
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

            await ExecuteTinhKH();
        }

        protected async Task ExecuteTinhKH()
        {
            ShowConfirmOverwrite = false;
            ShowLoadding = true;
            await InvokeAsync(StateHasChanged); // Ép hiện loading

            try
            {
                string keyParam = BuildSpKeyParam(FilterKeyTs);

                // SỬ DỤNG TRỰC TIẾP BIẾN FilterMaDvcs ĐÃ ĐƯỢC GÁN Ở SỰ KIỆN TRÊN
                string maDvcs = FilterMaDvcs ?? "";
                int userId = curUser?.User_id ?? 0;

                string paramNames = "@nam;@ky1;@ky2;@key;@user_id;@Ma_dvcs";
                string paramValues = $"{FilterNam};{FilterKy1};{FilterKy2};{keyParam};{userId};{maDvcs}";

                // Chạy SP Tính khấu hao
                DataSet ds = await myDb.LoadStoreProcedure("Fatinhkh", paramNames, paramValues);

                // Cập nhật lưới với kết quả trả về
                if (ds != null && ds.Tables.Count > 0)
                {
                    TblData = ds.Tables[ds.Tables.Count - 1];
                    SelectedItem = TblData.DefaultView.Count > 0 ? TblData.DefaultView[0] : null;
                    myStateMN.SetNewThongbao($"Tính khấu hao thành công! Kỳ {FilterKy1}-{FilterKy2}/{FilterNam} — {TblData.Rows.Count} tài sản.");
                }
                else
                {
                    TblData = null;
                    myStateMN.SetNewThongbao($"Tính khấu hao hoàn tất nhưng không có dữ liệu trả về.");
                }

                PopupVisible = false;
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao("Lỗi khi tính khấu hao: " + ex.Message);
                Console.WriteLine("Fatinhkh SP error: " + ex.Message);
            }
            finally
            {
                ShowLoadding = false;
                if (oBrowse != null)
                    await oBrowse.FocusGridAsync();
                await InvokeAsync(StateHasChanged);
            }
        }
        /// <summary>
        /// Chuyển danh sách mã TS nối phẩy thành điều kiện SQL hợp lệ để truyền vào @key của SP.
        /// Winform truyền: "1=1" khi không lọc, hoặc "dbo.InList(so_the_ts,'TS001,TS002',',')=1"
        /// </summary>
        private static string BuildSpKeyParam(string filterKeyTs)
        {
            if (string.IsNullOrWhiteSpace(filterKeyTs))
                return "1=1";

            // Làm sạch từng mã: loại bỏ khoảng trắng và dấu nháy đơn
            var maCodes = filterKeyTs
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim().Replace("'", "''"))
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();

            if (maCodes.Count == 0)
                return "1=1";

            // Ghép lại thành chuỗi an toàn, ví dụ: TS001,TS002
            string safeList = string.Join(",", maCodes);

            // Trả về điều kiện SQL dùng hàm dbo.InList — đúng cú pháp Winform
            return $"dbo.InList(so_the_ts,'{safeList}',',')=1";
        }

        // ===================================================
        // HELPERS CHO RAZOR
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
            FilterKeyTs = "";
            StateHasChanged();
        }

        // ===================================================
        // MODAL CHỌN TÀI SẢN
        // ===================================================

        /// <summary>
        /// Nạp sẵn toàn bộ dmts một lần vào cache DtMaTsFull
        /// (giống Winform nạp tại FrmIngia_tbLoc_Loaded)
        /// </summary>
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

            // Rebuild SelectedDmts từ FilterKeyTs (đây là nguồn thực dữ liệu đã confirm)
            // Không Clear() trước rồi Add sau — phải ghi đè hoàn toàn tránh cộng dồn
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

            // Nếu cache chưa có (trường hợp mở modal trước khi preload xong) thì load ngay
            if (DtMaTsFull == null)
                await PreloadDmtsAsync();

            // Lọc in-memory theo ĐVCS và từ khóa tìm kiếm
            ApplyDmtsFilter();
            ShowModalDmts = true;
            StateHasChanged();
        }

        /// <summary>
        /// Lọc in-memory từ DtMaTsFull theo FilterMaDvcs và DmtsSearch
        /// (giống Winform dùng DefaultView.RowFilter)
        /// </summary>
        private void ApplyDmtsFilter()
        {
            if (DtMaTsFull == null)
            {
                TblDmts = new DataTable();
                return;
            }

            // LUÔN CHO PHÉP LẤY TÀI SẢN TỪ TẤT CẢ ĐVCS
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
            // Khôi phục SelectedDmts về đúng FilterKeyTs khi đóng không confirm
            // tránh trường hợp tick thêm rồi bấm đóng — lần sau mở lại bị cộng dồn
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
