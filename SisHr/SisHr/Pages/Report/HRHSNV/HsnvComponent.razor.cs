using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SisCom.Model;
using SisCom.Shared;
using SisData.Service;
using SisHr.Pages.Report.HRHSNV;
using SisLib.List;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SisHr.Pages.Report.HRHSNV
{
    public partial class HsnvComponent : ListBaseComponent
    {
    // ===== Tree state (REMOVED) =====
    // Tree DVCS/BPNS đã xóa

        // ===== Dashboard state =====
        private bool ShowDashboard { get; set; } = true;
        private int TongNhanVien { get; set; } = 0;
        private int SoNhanVienMoi { get; set; } = 0;
        private int SoChuaCoBhxh { get; set; } = 0;
        private int SoThuViec { get; set; } = 0;
        private int SoChinhThuc { get; set; } = 0;
        private int SoSinhNhatThang { get; set; } = 0;
        private int SoChuaCoMst { get; set; } = 0;
        private int SoDangVien { get; set; } = 0;
        private string FilterMaDvcs { get; set; } = ""; // Filter DVCS
        private List<ChartDataPoint> ContractBreakdownData { get; set; } = new();
        private List<ChartDataPoint> GenderBreakdownData { get; set; } = new();
        private Dictionary<string, int> HopDongCount { get; set; } = new();
        // --- THÊM: Model dữ liệu biểu đồ ---
        public class ChartDataPoint
        {
            public string Category { get; set; } = string.Empty;
            public int Value { get; set; }
        }

        // ===== Quick Update =====
        private bool IsShowQuickUpdate { get; set; } = false;
        private string QuickMaNs { get; set; } = "";
        private string QuickTenNs { get; set; } = "";
        private string QuickMaBpns { get; set; } = "";
        private string QuickDienThoai { get; set; } = "";

        // ===== Filter state (REMOVED) =====

        protected override void OnInitialized()
        {
            mPage = "hrhsnv";
            sqlTableName = "dmns";

            // 1. Cập nhật tên View chính xác
            sqlTableView = "v_dmttns";

            SqlTableKey = "ma_ns";
            SqlItemName = "ten_ns";
            FrmType = typeof(FrmHsnv);
            if (string.IsNullOrEmpty(Menu_id))
                Menu_id = "wr.ct.01";

            base.OnInitialized();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    await Language();
                    await PageLoad();
                    
                    if (curCommand == null)
                    {
                        myStateMN.SetNewThongbao("Menu configuration not found for Menu_id: " + Menu_id);
                        return;
                    }
                    
                    await Lap.LoadLocalizationAsync($"hr-{(IsLangEn ? "en" : "vi")}");

                    string menuid = Menu_id.Replace("_", ".");
                    if (curUser != null && (curUser.Is_admin == 1 || curUser.CheckRight(menuid, "VIEW")))
                    {
                        myStateMN.SetToolBar(new MyToolbar("toolbar", true));
                        myStateMN.SetToolBar(new MyToolbar("New", true, "F4"));
                        myStateMN.SetToolBar(new MyToolbar("Edit", true, "F3"));
                        myStateMN.SetToolBar(new MyToolbar("Save", false));
                        myStateMN.SetToolBar(new MyToolbar("Delete", true, "F8"));
                        myStateMN.SetToolBar(new MyToolbar("View", true, "F2"));
                        myStateMN.SetToolBar(new MyToolbar("Aproval", false));
                        myStateMN.SetToolBar(new MyToolbar("Print", false));
                        myStateMN.SetToolBar(new MyToolbar("Export", true));
                    }
                    else
                    {
                        myStateMN.SetToolBar(new MyToolbar("toolbar", false));
                    }

                    myStateMN.OnActionTaskChanged += OnActionTaskchanged;
                    myStateMN.OnLangguaChanged += OnLangguachange;
                    myStateMN.OnDvcsChanged += OnDvcsChangedHandler;

                    // ✅ Lấy DVCS được chọn hiện tại từ State Manager
                    string maDvcs = myStateMN.GetMa_Dvcs();
                    FilterMaDvcs = (maDvcs == "ALL" || string.IsNullOrEmpty(maDvcs)) ? "" : maDvcs;

                    await Loaddata();

                    StateHasChanged();
                }
                catch (Exception ex)
                {
                    myStateMN.SetNewThongbao("Error: " + ex.Message);
                }
            }
            else
            {
                if (oBrowse != null)
                    await oBrowse.FocusGridAsync();
            }
        }
        // ===================================================
        // PAGELOAD (LOAD CẤU HÌNH LƯỚI TỪ DATABASE)
        // ===================================================
        public override async Task PageLoad()
        {
            try
            {
                if (myCommand == null) return;

                // Đọc cấu hình chức năng hiện tại dựa vào Menu_id
                curCommand = await myCommand.GetSubMenuBymenuId(Menu_id.Replace("_", "."));

                if (curCommand != null)
                {
                    // Gán fbrowse động từ Vbrowse1 (Tiếng Việt) hoặc Ebrowse1 (Tiếng Anh)
                    fbrowse = IsLangEn ? curCommand.Ebrowse1 : curCommand.Vbrowse1;

                    // Đọc số dòng trên 1 trang từ tùy chọn hệ thống
                    string ps = myOption.GetOptionsValue("M_GRID_NROW");
                    Page_size = string.IsNullOrEmpty(ps) ? 50 : int.Parse(ps.Trim());
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine("PageLoad error: " + ex.Message);
            }
        }
        // ===================================================
        // TOOLBAR ACTIONS
        // ===================================================
        public override void OnActionTaskchanged(ActionTask ac)
        {
            switch (ac)
            {
                case ActionTask.New:
                    myStateMN.SetCurrentActionTask(ActionTask.New);
                    V_Moi();
                    break;
                case ActionTask.Edit:
                    myStateMN.SetCurrentActionTask(ActionTask.Edit);
                    V_Sua();
                    break;
                case ActionTask.View:
                    myStateMN.SetCurrentActionTask(ActionTask.View);
                    V_Xem();
                    break;
                case ActionTask.Delete:
                    V_Xoa();
                    break;
                case ActionTask.Export:
                    ExportData(1);
                    break;
                case ActionTask.Export2:
                    ExportData(2);
                    break;
                case ActionTask.Export3:
                    ExportData(3);
                    break;
                case ActionTask.Search:
                    OpenQuickUpdate();
                    break;
            }
        }

        // ===================================================
        // LOAD DATA
        // ===================================================
        public override async Task Loaddata()
        {
            try
            {
                // Query join với view v_dmns để lấy tất cả các trường cần thiết
                string where = "";
                if (!string.IsNullOrEmpty(FilterMaDvcs))
                {
                    where = $" WHERE v.ma_dvcs = N'{Esc(FilterMaDvcs)}'";
                }

                string sql = $@"
                    SELECT 
                        v.*
                    FROM {sqlTableView} v
                    {where}
                    ORDER BY v.ma_ns";

                DataSet ds = await myDb.LoadDataset(sql);
                TblData = ds?.Tables.Count > 0 ? ds.Tables[0] : new DataTable();

                // Thêm cột STT tự động
                if (TblData.Columns.Contains("STT") == false)
                {
                    TblData.Columns.Add("STT", typeof(int));
                }

                for (int i = 0; i < TblData.Rows.Count; i++)
                {
                    TblData.Rows[i]["STT"] = i + 1;
                }

                if (TblData.DefaultView.Count > 0)
                    SelectedItem = TblData.DefaultView[0];
                else
                    SelectedItem = null;

                ComputeDashboard();


                if (oBrowse != null)
                    await oBrowse.FocusGridAsync();
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao("Lỗi tải dữ liệu: " + ex.Message);
                TblData = new DataTable();
            }

            StateHasChanged();
        }

        string BuildWhereClause()
        {
            return ""; // Hiển thị tất cả nhân viên
        }

        void ComputeDashboard()
        {
            // 1. Reset biến thẻ
            TongNhanVien = 0;
            SoNhanVienMoi = 0;
            SoChuaCoBhxh = 0;
            SoSinhNhatThang = 0;
            SoChuaCoMst = 0;
            SoDangVien = 0;

            // --- THÊM MỚI: TẢI DANH MỤC GIỚI TÍNH TỪ DB LÊN ---
            Dictionary<string, string> dictGioiTinh = new();
            try
            {
                DataTable dtGt = myDb.GetTable("SELECT ma_gioitinh, ten_gioitinh, ten_gioitinh2 FROM dmgioitinh");
                if (dtGt != null)
                {
                    foreach (DataRow r in dtGt.Rows)
                    {
                        string ma = r["ma_gioitinh"]?.ToString()?.Trim().ToUpper() ?? "";

                        // Hỗ trợ song ngữ: Nếu dùng tiếng Anh và có cột ten_gioitinh2 thì lấy, ngược lại lấy tiếng Việt
                        string ten = IsLangEn && r.Table.Columns.Contains("ten_gioitinh2") && !string.IsNullOrWhiteSpace(r["ten_gioitinh2"]?.ToString())
                                     ? r["ten_gioitinh2"].ToString().Trim()
                                     : r["ten_gioitinh"]?.ToString()?.Trim() ?? "";

                        if (!string.IsNullOrEmpty(ma))
                        {
                            dictGioiTinh[ma] = ten;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi load danh mục giới tính: " + ex.Message);
            }
            
            // --- THÊM MỚI: TẢI DANH MỤC LOẠI HỢP ĐỒNG TỪ DB LÊN ---
            Dictionary<string, string> dictLoaiHd = new();
            try
            {
                DataTable dtLhd = myDb.GetTable("SELECT ma_lhdong, ten_lhdong, ten_lhdong2 FROM dmlhdong");
                if (dtLhd != null)
                {
                    foreach (DataRow r in dtLhd.Rows)
                    {
                        string ma = r["ma_lhdong"]?.ToString()?.Trim().ToUpper() ?? "";

                        // Hỗ trợ song ngữ: Ưu tiên ten_lhdong2 nếu đang dùng tiếng Anh
                        string ten = IsLangEn && r.Table.Columns.Contains("ten_lhdong2") && !string.IsNullOrWhiteSpace(r["ten_lhdong2"]?.ToString())
                                     ? r["ten_lhdong2"].ToString().Trim()
                                     : r["ten_lhdong"]?.ToString()?.Trim() ?? "";

                        if (!string.IsNullOrEmpty(ma))
                        {
                            dictLoaiHd[ma] = ten;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi load danh mục loại hợp đồng: " + ex.Message);
            }
            // 2. Khởi tạo lại MỚI hoàn toàn các List/Dictionary
            ContractBreakdownData = new List<ChartDataPoint>();
            GenderBreakdownData = new List<ChartDataPoint>();
            HopDongCount = new Dictionary<string, int>();
            Dictionary<string, int> GioiTinhCount = new(); // Dùng Dictionary để tự động đếm mọi giới tính

            if (TblData == null || TblData.Rows.Count == 0) return;

            TongNhanVien = TblData.Rows.Count;
            DateTime currentMonth = DateTime.Now;

            foreach (DataRow row in TblData.Rows)
            {
                // --- A. BIỂU ĐỒ: TÍNH TOÁN GIỚI TÍNH ĐỘNG ---
                string gioiTinhCode = row.Table.Columns.Contains("gioi_tinh") ? row["gioi_tinh"]?.ToString()?.Trim().ToUpper() ?? "" : "";
                string tenGioiTinh = Lap["Unknown"] ?? "Chưa xác định";

                if (!string.IsNullOrEmpty(gioiTinhCode))
                {
                    // Tìm tên giới tính trong Dictionary dựa vào mã
                    if (dictGioiTinh.ContainsKey(gioiTinhCode))
                    {
                        tenGioiTinh = dictGioiTinh[gioiTinhCode];
                    }
                    else
                    {
                        tenGioiTinh = gioiTinhCode; // Fallback: Nếu DB chưa có tên thì hiện mã
                    }
                }

                // Cộng dồn vào biểu đồ
                if (GioiTinhCount.ContainsKey(tenGioiTinh))
                    GioiTinhCount[tenGioiTinh]++;
                else
                    GioiTinhCount[tenGioiTinh] = 1;

                // --- B. BIỂU ĐỒ: TÍNH TOÁN HỢP ĐỒNG (Đã sửa thành động) ---
                string maLoaiHd = row.Table.Columns.Contains("ma_loai_hdong") ? row["ma_loai_hdong"]?.ToString()?.Trim().ToUpper() ?? "" : "";
                string tenLhdong = Lap["Unknown"] ?? "Chưa phân loại";

                if (!string.IsNullOrEmpty(maLoaiHd))
                {
                    // Tìm tên loại hợp đồng trong Dictionary vừa load
                    if (dictLoaiHd.ContainsKey(maLoaiHd))
                    {
                        tenLhdong = dictLoaiHd[maLoaiHd];
                    }
                    else
                    {
                        tenLhdong = maLoaiHd; // Nếu không có trong danh mục thì hiện mã để dễ kiểm tra
                    }
                }

                if (HopDongCount.ContainsKey(tenLhdong))
                    HopDongCount[tenLhdong]++;
                else
                    HopDongCount[tenLhdong] = 1;

                // --- C. THẺ MỚI 1: NHÂN SỰ MỚI (TRONG THÁNG) ---
                if (row.Table.Columns.Contains("ngay_vao") && row["ngay_vao"] != DBNull.Value)
                {
                    if (DateTime.TryParse(row["ngay_vao"].ToString(), out DateTime ngayVao))
                    {
                        if (ngayVao.Year == currentMonth.Year && ngayVao.Month == currentMonth.Month)
                            SoNhanVienMoi++;
                    }
                }

                // --- D. THẺ MỚI 2: CHƯA CÓ SỔ BHXH ---
                string soBhxh = row.Table.Columns.Contains("so_so_bhxh") ? row["so_so_bhxh"]?.ToString()?.Trim() ?? "" : "";
                if (string.IsNullOrEmpty(soBhxh))
                {
                    SoChuaCoBhxh++;
                }

                // --- E. THẺ MỚI 3: SINH NHẬT THÁNG NÀY ---
                if (row.Table.Columns.Contains("ngay_sinh") && row["ngay_sinh"] != DBNull.Value)
                {
                    if (DateTime.TryParse(row["ngay_sinh"].ToString(), out DateTime ngaySinh))
                    {
                        if (ngaySinh.Month == currentMonth.Month)
                            SoSinhNhatThang++;
                    }
                }

                // --- F. THẺ MỚI 4: THIẾU MÃ SỐ THUẾ ---
                string mst = row.Table.Columns.Contains("ma_so_thue") ? row["ma_so_thue"]?.ToString()?.Trim() ?? "" : "";
                if (string.IsNullOrEmpty(mst))
                {
                    SoChuaCoMst++;
                }

                // --- G. THẺ MỚI 5: ĐẢNG VIÊN ---
                string ngayVaoDang = row.Table.Columns.Contains("ngay_vao_dang") ? row["ngay_vao_dang"]?.ToString()?.Trim() ?? "" : "";
                if (!string.IsNullOrEmpty(ngayVaoDang))
                {
                    SoDangVien++;
                }
            }

            // --- ĐƯA DỮ LIỆU VÀO BIỂU ĐỒ ---
            foreach (var item in HopDongCount)
            {
                ContractBreakdownData.Add(new ChartDataPoint { Category = item.Key, Value = item.Value });
            }

            // Đẩy toàn bộ danh sách Giới tính đã đếm được vào biểu đồ
            foreach (var item in GioiTinhCount)
            {
                GenderBreakdownData.Add(new ChartDataPoint { Category = item.Key, Value = item.Value });
            }
        }

        void ToggleDashboard()
        {
            ShowDashboard = !ShowDashboard;
            StateHasChanged();
        }

        // ===================================================
        // OPEN FORM: VIEW / EDIT
        // ===================================================
        private void OpenForm(ActionTask task)
        {
            DataRowView rview = (DataRowView)SelectedItem;
            if (rview == null)
            {
                myStateMN.SetNewThongbao(Lap["No data selected"]);
                return;
            }

            string key = rview["ma_ns"]?.ToString()?.Trim() ?? "";
            Par.Clear();
            Par.Add("Ma_ma", (object)key);
            Par.Add("Menu_id", (object)Menu_id);
            Par.Add("ShowAdd", (object)true);
            Par.Add("curActionTask", (object)task);
            Par.Add("OnClose", EventCallback.Factory.Create(this, CloseForm));
            FrmType = typeof(FrmHsnv);
            IsShowFrm = true;
            StateHasChanged();
        }

        public override void V_Xem()
        {
            OpenForm(ActionTask.View);
        }

        public override void V_Sua()
        {
            OpenForm(ActionTask.Edit);
        }

        public override void V_Moi()
        {
            Par.Clear();
            Par.Add("Ma_ma", (object)"");
            Par.Add("Menu_id", (object)Menu_id);
            Par.Add("ShowAdd", (object)true);
            Par.Add("curActionTask", (object)ActionTask.New);
            Par.Add("OnClose", EventCallback.Factory.Create(this, CloseForm));
            FrmType = typeof(FrmHsnv);
            IsShowFrm = true;
            StateHasChanged();
        }

        public override async void CloseForm()
        {
            IsShowFrm = false;
            await Loaddata();
            StateHasChanged();
        }

        // ===================================================
        // QUICK UPDATE
        // ===================================================
        void OpenQuickUpdate()
        {
            DataRowView rview = (DataRowView)SelectedItem;
            if (rview == null)
            {
                myStateMN.SetNewThongbao(Lap["No data selected"]);
                return;
            }

            QuickMaNs = rview.Row.Table.Columns.Contains("ma_ns") ? rview["ma_ns"]?.ToString()?.Trim() ?? "" : "";
            QuickTenNs = rview.Row.Table.Columns.Contains("ten_ns") ? rview["ten_ns"]?.ToString()?.Trim() ?? "" : "";
            QuickMaBpns = rview.Row.Table.Columns.Contains("ma_bpns") ? rview["ma_bpns"]?.ToString()?.Trim() ?? "" : "";
            QuickDienThoai = rview.Row.Table.Columns.Contains("dien_thoai") ? rview["dien_thoai"]?.ToString()?.Trim() ?? "" : "";

            IsShowQuickUpdate = true;
            StateHasChanged();
        }

        async void SaveQuickUpdate()
        {
            try
            {
                if (string.IsNullOrEmpty(QuickMaNs)) return;

                string safeMaNs = Esc(QuickMaNs);
                string safeBpns = Esc(QuickMaBpns);
                string safeDt = Esc(QuickDienThoai);

                string sql = $"UPDATE {sqlTableName} SET ma_bpns = N'{safeBpns}', dien_thoai = N'{safeDt}' WHERE ma_ns = N'{safeMaNs}'";
                await myDb.ExecuteSqlQueryAsync(sql);

                myStateMN.SetNewThongbao(Lap["Edit successfully"] + ": " + QuickMaNs);
                IsShowQuickUpdate = false;
                await Loaddata();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao(Lap["Save failed"] + ": " + ex.Message);
            }
        }

        // ===================================================
        // DVCS CHANGE (from top bar)
        // ===================================================
        private async void OnDvcsChangedHandler(string maDvcs)
        {
            // Cập nhật filter DVCS và reload dữ liệu
            FilterMaDvcs = (maDvcs == "ALL" || string.IsNullOrEmpty(maDvcs)) ? "" : maDvcs;
            await Loaddata();
            StateHasChanged();
        }

        // ===================================================
        // LANGUAGE
        // ===================================================
        private async void OnLangguachange(MyLang lang)
        {
            if (lang.IsnewLang)
                await Language();
        }

        private new async Task Language()
        {
            try
            {
                IsLangEn = false;
                UserLang = await myLcStore.GetItemAsync<string>("userlang");
                if (!string.IsNullOrEmpty(UserLang) && UserLang.Equals("En"))
                    IsLangEn = true;

                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // ===================================================
        // DISPOSE
        // ===================================================
        public override void Dispose()
        {
            myStateMN.OnActionTaskChanged -= OnActionTaskchanged;
            myStateMN.OnLangguaChanged -= OnLangguachange;
            myStateMN.OnDvcsChanged -= OnDvcsChangedHandler;
        }

        private static string Esc(string s) => s?.Trim().Replace("'", "''") ?? "";
    }
}
