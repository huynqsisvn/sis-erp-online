using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SisCom;
using SisCom.Model;
using SisCom.Shared;
using SisFx.Pages.Input.FXCC;
using SisFx.Pages.Input.FXCC2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;

namespace SisFx.Pages.Input.FXCC2
{
    public partial class FrmFxcc2
    {
        // =========================================================
        // HẰNG SỐ & THAM CHIẾU
        // =========================================================
        protected const string kieukh1 = "1";

        private DxGrid GridCt;
        private object SelectedCtDmcc;
        private object SelectedCtCtcc;
        private int ActiveTabIndex = 0;

        // =========================================================
        // DỮ LIỆU CHÍNH
        // =========================================================
        public Dmcc CurDmcc { get; set; } = new Dmcc();
        public List<CtDmcc> LstCtDmcc { get; set; } = new List<CtDmcc>();
        public List<CtCtcc> LstCtCtcc { get; set; } = new List<CtCtcc>();

        protected List<object> ListKieuKH => new List<object>
        {
            new { Value = "1", Text = Lap["1. By period"] },
            new { Value = "2", Text = Lap["2. Two-phase allocation"] }
        };

        // =========================================================
        // DANH MỤC DÙNG CHUNG CHO COMBOBOX
        // =========================================================
        private List<Fxcc2ListItem> LsBpts;
        private List<Fxcc2ListItem> LsTk;
        private List<Fxcc2ListItem> LsBpht;
        private List<Fxcc2ListItem> LsVv;
        private List<Fxcc2ListItem> LsMaGiamCc;
        private List<Fxcc2ListItem> LsMaHongCc;

        // =========================================================
        // DỮ LIỆU 5 TAB
        // =========================================================
        public List<CtBpcc> LstDccc { get; set; } = new List<CtBpcc>();
        public List<CcThoiKc> LstTamdung { get; set; } = new List<CcThoiKc>();
        public List<EditGiamCc> LstGiamcc { get; set; } = new List<EditGiamCc>();
        public List<EditHongCc> LstHongcc { get; set; } = new List<EditHongCc>();
        public List<EditThoiCc> LstThoicc { get; set; } = new List<EditThoiCc>();

        // =========================================================
        // POPUP XÁC NHẬN XÓA DÙNG CHUNG
        // =========================================================
        private bool PopupConfirmVisible { get; set; }
        private string PopupConfirmMessage { get; set; } = "";
        private Func<Task> OnConfirmAction { get; set; }

        private async Task OnPopupConfirmOk()
        {
            if (OnConfirmAction != null) await OnConfirmAction.Invoke();
            PopupConfirmVisible = false;
        }

        // =========================================================
        // LOAD DỮ LIỆU CHÍNH (GỌI STORE PROCEDURE LoadFXCC)
        // =========================================================
        private async Task LoadData(string id)
        {
            string safeId = Escape(id);

            try
            {
                // Gọi Store Procedure LoadFXCC
                var ds = await myDb.LoadDataset($"EXEC LoadFXCC '{safeId}'");

                if (ds?.Tables.Count > 0)
                {
                    // Table 0: DMCC (Master)
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        var list = myDb.ConvertDataTable<Dmcc>(ds.Tables[0]);
                        CurDmcc = list[0];
                        CurDmcc.Ma_cc_me = CurDmcc.Ma_cc_me?.Trim();
                        CurDmcc.Kieu_kh_str = CurDmcc.Kieu_kh.HasValue ? CurDmcc.Kieu_kh.Value.ToString("0") : "1";
                    }

                    // Table 1: CTDMCC (Chi tiết thẻ cc)
                    if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                    {
                        LstCtDmcc = myDb.ConvertDataTable<CtDmcc>(ds.Tables[1]);
                        int stt = 1;
                        LstCtDmcc.ForEach(x => { x.Stt = stt++; x.Ma_cc = x.Ma_cc?.Trim(); x.Ten_bpcc = x.Ten_bpcc?.Trim(); });
                    }
                    else LstCtDmcc = new List<CtDmcc>();

                    // Table 2: CTCTCC (Phụ tùng kèm theo)
                    if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                    {
                        LstCtCtcc = myDb.ConvertDataTable<CtCtcc>(ds.Tables[2]);
                        int stt = 1;
                        LstCtCtcc.ForEach(x => x.Stt = stt++);
                    }
                    else LstCtCtcc = new List<CtCtcc>();

                    // Table 3: CTBPCC (Điều chuyển - cc0 = 1, dùng cho lưới chi tiết)
                    // (sẽ xử lý sau khi có LstCtDmcc)

                    // Table 7: CTBPCC (Điều chuyển - TẤT CẢ, dùng cho dữ liệu tab)
                    if (ds.Tables.Count > 7 && ds.Tables[7].Rows.Count > 0)
                    {
                        LstDccc = myDb.ConvertDataTable<CtBpcc>(ds.Tables[7]);
                        // Renumber STT for each card separately
                        string currentMaCc = "";
                        int stt = 0;
                        foreach (var x in LstDccc)
                        {
                            if (x.Ma_cc != currentMaCc)
                            {
                                currentMaCc = x.Ma_cc;
                                stt = 0;
                            }
                            x.Stt = ++stt;
                            x.Ma_cc = x.Ma_cc?.Trim();
                            x.Ten_bpcc = x.Ten_bpcc?.Trim();
                        }
                    }
                    else LstDccc = new List<CtBpcc>();
                }

                // Load các tab khác từ query riêng
                await LoadSubTabsData(safeId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoadData SP Error: " + ex.Message);
                LstCtDmcc = new List<CtDmcc>();
                LstCtCtcc = new List<CtCtcc>();
                LstDccc = new List<CtBpcc>();
            }

            AllowEditMa = false;
            AllowEditMode = false;
        }

        // Load dữ liệu cho các tab phụ (Tab Tạm dừng, Giảm cc, Hỏng cc, Thôi p.bổ)
        private async Task LoadSubTabsData(string safeId)
        {
            // Tạo danh sách ma_cc trong ngoặc để dùng IN (...)
            string inMaCc = LstCtDmcc.Count > 0
                ? "N'" + string.Join("',N'", LstCtDmcc.Select(x => Escape(x.Ma_cc ?? ""))) + "'"
                : "N''";
            // Load Tab Điều chuyển BPSD bằng thủ tục chuẩn của Winform
            try
            {
                var dsDc = await myDb.LoadDataset($"EXEC InitFXCC_dccc N'{safeId}'");
                if (dsDc?.Tables.Count > 1 && dsDc.Tables[1].Rows.Count > 0)
                {
                    LstDccc = myDb.ConvertDataTable<CtBpcc>(dsDc.Tables[1]);
                    // Tên user, tên bộ phận đã được SQL lo sẵn trong SP này
                    foreach (var x in LstDccc)
                    {
                        x.Ma_cc = x.Ma_cc?.Trim();
                        x.Ten_bpcc = x.Ten_bpcc?.Trim();
                    }
                }
                else LstDccc = new List<CtBpcc>();
            }
            catch (Exception ex) { Console.WriteLine("LoadDccc: " + ex.Message); LstDccc = new List<CtBpcc>(); }
            
            // Tab Tạm dừng phân bổ: dmccthoikc – lọc theo ma_cc
            try
            {
                var dsTd = await myDb.LoadDataset($"EXEC InitFXCC_tamdung N'{safeId}'");
                if (dsTd?.Tables.Count > 1 && dsTd.Tables[1].Rows.Count > 0)
                {
                    LstTamdung = myDb.ConvertDataTable<CcThoiKc>(dsTd.Tables[1]);

                    foreach (var x in LstTamdung)
                    {
                        x.Ma_cc = x.Ma_cc?.Trim();
                        // Nếu model của bạn có các trường text khác cần Trim thì bổ sung ở đây
                    }
                }
                else
                {
                    LstTamdung = new List<CcThoiKc>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoadTamdung: " + ex.Message);
                LstTamdung = new List<CcThoiKc>();
            }

            // Tab Khai báo giảm cc: TẤT CẢ thẻ ctdmcc (kể cả chưa có ngay_giam)
            try
            {
                var dsGiam = await myDb.LoadDataset($"EXEC InitFXCC_giam N'{safeId}'");

                // SP này chỉ trả về 1 bảng duy nhất nên ta lấy Tables[0]
                if (dsGiam?.Tables.Count > 0 && dsGiam.Tables[0].Rows.Count > 0)
                {
                    LstGiamcc = myDb.ConvertDataTable<EditGiamCc>(dsGiam.Tables[0]);
                    foreach (var x in LstGiamcc)
                    {
                        x.Ma_cc = x.Ma_cc?.Trim();
                        x.Ten_bpcc = x.Ten_bpcc?.Trim();
                        x.Ten_tg_cc = x.Ten_tg_cc?.Trim();
                    }
                }
                else LstGiamcc = new List<EditGiamCc>();
            }
            catch { LstGiamcc = new List<EditGiamCc>(); }

            // =======================================================
            // Tab Khai báo hỏng CC: Gọi SP chuẩn của Winform
            // =======================================================
            try
            {
                var dsHong = await myDb.LoadDataset($"EXEC InitFXCC_hong N'{safeId}'");

                // SP này chỉ trả về 1 bảng duy nhất nên ta lấy Tables[0]
                if (dsHong?.Tables.Count > 0 && dsHong.Tables[0].Rows.Count > 0)
                {
                    LstHongcc = myDb.ConvertDataTable<EditHongCc>(dsHong.Tables[0]);
                    foreach (var x in LstHongcc)
                    {
                        x.Ma_cc = x.Ma_cc?.Trim();
                        x.Ten_bpcc = x.Ten_bpcc?.Trim();
                        // Nếu có trường Ten_tg_cc (Lý do hỏng) thì bổ sung Trim ở đây
                    }
                }
                else LstHongcc = new List<EditHongCc>();
            }
            catch { LstHongcc = new List<EditHongCc>(); }

            // Tab Thôi phân bổ: TẤT CẢ thẻ ctdmcc
            try
            {
                var dsTh = await myDb.LoadDataset($@"
                    SELECT c.id, c.ma_cc, c.ma_cc_me, c.so_ct, c.ngay_kh1, c.ly_do_thoi,
                           c.sl_cc, c.nguyen_gia, c.gt_da_kh, c.gt_cl, c.gt_kh_ky, c.ghi_chu,
                           c.date_giam0, c.time_giam0, c.user_id_giam0,
                           c.date_giam, c.time_giam, c.user_id_giam,
                           c.date_thoi0, c.time_thoi0, c.user_id_thoi0,
                           c.date_thoi, c.time_thoi, c.user_id_thoi,
                           ISNULL(b.ten_bpts,'') AS ten_bpcc
                    FROM ctdmcc c
                    LEFT JOIN dmbpts b ON b.ma_bpts = c.ma_bpcc
                    WHERE c.ma_cc_me = N'{safeId}' AND ISNULL(c.cc0,'1') = '1'
                    ORDER BY c.ma_cc");
                if (dsTh?.Tables.Count > 0 && dsTh.Tables[0].Rows.Count > 0)
                {
                    LstThoicc = myDb.ConvertDataTable<EditThoiCc>(dsTh.Tables[0]);
                    int stt = 1; LstThoicc.ForEach(x => { x.Stt = stt++; x.Ma_cc = x.Ma_cc?.Trim(); });
                }
                else LstThoicc = new List<EditThoiCc>();
            }
            catch (Exception ex) { Console.WriteLine("LoadThoicc: " + ex.Message); LstThoicc = new List<EditThoiCc>(); }
        }

        // Load danh mục combobox
        private async Task LoadDanhmuc()
        {
            try
            {
                async Task<List<Fxcc2ListItem>> Load(string sql)
                {
                    var ds = await myDb.LoadDataset(sql);
                    if (ds?.Tables.Count > 0) return myDb.ConvertDataTable<Fxcc2ListItem>(ds.Tables[0]);
                    return new List<Fxcc2ListItem>();
                }

                LsBpts = await Load("SELECT ma_bpts AS ma_item, ISNULL(ten_bpts,'') AS ten_item FROM dmbpts ORDER BY ma_bpts");
                LsTk = await Load("SELECT tk AS ma_item, ISNULL(ten_tk,'') AS ten_item FROM dmtk ORDER BY tk");
                LsBpht = await Load("SELECT ma_bpht AS ma_item, ISNULL(ten_bpht,'') AS ten_item FROM dmbpht ORDER BY ma_bpht");
                LsVv = await Load("SELECT ma_vv AS ma_item, ISNULL(ten_vv,'') AS ten_item FROM dmvv ORDER BY ma_vv");
                LsMaGiamCc = await Load("SELECT ma_tg_cc AS ma_item, ISNULL(ten_tg_cc,'') AS ten_item FROM dmtgcc WHERE loai_tg_cc = 2 ORDER BY ma_tg_cc");
                LsMaHongCc = await Load("SELECT ma_tg_cc AS ma_item, ISNULL(ten_tg_cc,'') AS ten_item FROM dmtgcc WHERE loai_tg_cc = 3 ORDER BY ma_tg_cc");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi load danh mục FXCC2: " + ex.Message);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadDanhmuc();
        }

        // =========================================================
        // LƯU (chỉ đóng form – không chỉnh sửa master)
        // =========================================================
        public Task SaveDM()
        {
            CloseForm();
            return Task.CompletedTask;
        }

        // =========================================================
        // TAB 4 – ĐIỀU CHUYỂN BPSD (ctbpcc)
        // =========================================================
        private DxGrid GridDccc;
        private object _selectedDcccCard;
        public object SelectedDcccCard
        {
            get => _selectedDcccCard;
            set
            {
                if (_selectedDcccCard != value)
                {
                    _selectedDcccCard = value;
                    InvokeAsync(StateHasChanged);
                }
            }
        }
        private bool isShowPopupDccc, isViewingDccc, isNewDccc;
        private CtBpcc editItemDccc = new CtBpcc();
        private DxComboBox<Fxcc2ListItem, string> cbDcccBpcc;
        private DxDateEdit<DateTime?> deDcccNgayKh;

        private List<CtBpcc> FilteredLstDccc
        {
            get
            {
                if (SelectedDcccCard is not CtDmcc card || string.IsNullOrEmpty(card.Ma_cc))
                    return new List<CtBpcc>();

                // Sắp xếp: Dòng gốc (Cc0 = "1") lên đầu tiên, sau đó xếp theo ngày
                var list = LstDccc.Where(x => x.Ma_cc == card.Ma_cc)
                                  .OrderByDescending(x => x.Cc0)
                                  .ThenBy(x => x.Ngay_kh)
                                  .ToList();

                // Đánh lại STT bắt đầu từ 1
                int stt = 1;
                list.ForEach(x => x.Stt = stt++);
                return list;
            }
        }

        private void AddNewDccc()
        {
            if (SelectedDcccCard is not CtDmcc card || string.IsNullOrEmpty(card.Ma_cc))
            {
                myStateMN.SetNewThongbao(Lap["Please select a tool card!"]);
                return;
            }
            isViewingDccc = false; isNewDccc = true;
            editItemDccc = new CtBpcc { Ma_cc = card.Ma_cc, Sl_cc = card.Sl_cc, Ngay_kh = DateTime.Today, Cc0 = "0" };
            isShowPopupDccc = true;
        }

        private void OpenEditDccc(CtBpcc item)
        {
            if (item.Cc0 == "1") { myStateMN.SetNewThongbao(Lap["Cannot edit the original record!"]); return; }
            isViewingDccc = false; isNewDccc = false;
            editItemDccc = CopyOf(item);
            isShowPopupDccc = true;
        }

        private void OpenViewDccc(CtBpcc item)
        {
            isViewingDccc = true; isNewDccc = false;
            editItemDccc = CopyOf(item);
            isShowPopupDccc = true;
        }

        private async Task OnPopupShownDccc()
        {
            if (cbDcccBpcc != null && !isViewingDccc) { await Task.Delay(80); await cbDcccBpcc.FocusAsync(); }
        }

        private async Task SaveDccc()
        {
            if (string.IsNullOrWhiteSpace(editItemDccc.Ma_bpcc))
            { myStateMN.SetNewThongbao(Lap["Use department is required!"]); return; }
            if (!editItemDccc.Ngay_kh.HasValue)
            { myStateMN.SetNewThongbao(Lap["Allocation start date is required!"]); return; }
            if (CurDmcc.Ngay_mua.HasValue && editItemDccc.Ngay_kh.Value.Date < CurDmcc.Ngay_mua.Value.Date)
            { myStateMN.SetNewThongbao(Lap["Allocation start date must be after the purchase date!"]); return; }

            string sBpcc = Escape(editItemDccc.Ma_bpcc);
            string sMaCc = Escape(editItemDccc.Ma_cc ?? "");
            string sNgay = SqlDate(editItemDccc.Ngay_kh);
            string sTkCp = Escape(editItemDccc.Tk_cp ?? "");
            string sBpht = Escape(editItemDccc.Ma_bpht ?? "");
            string sVv = Escape(editItemDccc.Ma_vv ?? "");
            string sGhiChu = Escape(editItemDccc.Ghi_chu ?? "");
            string sNow = SqlDate(DateTime.Now), sTime = $"'{DateTime.Now:HH:mm:ss}'";
            string sUserId = (curUser?.User_id ?? 0).ToString();
            string sUserName = Escape(curUser?.User_name ?? "");
            string sSlCc = (editItemDccc.Sl_cc ?? 0).ToString(System.Globalization.CultureInfo.InvariantCulture);
            if (isNewDccc)
            {
                await myDb.ExecuteSqlQueryAsync($@"
            INSERT INTO ctbpcc(ma_cc, ma_bpcc, sl_cc, ngay_kh, tk_cp, ma_bpht, ma_vv, ghi_chu, cc0, date0, time0, user_id0, [date], [time], user_id)
            VALUES (N'{sMaCc}', N'{sBpcc}', {sSlCc}, {sNgay}, N'{sTkCp}', N'{sBpht}', N'{sVv}', N'{sGhiChu}', '0',
                    {sNow}, {sTime}, {sUserId}, {sNow}, {sTime}, {sUserId})");
            }
            else
            {
                await myDb.ExecuteSqlQueryAsync($@"
            UPDATE ctbpcc SET
                ma_bpcc = N'{sBpcc}', sl_cc = {sSlCc}, ngay_kh = {sNgay}, tk_cp = N'{sTkCp}',
                ma_bpht = N'{sBpht}', ma_vv = N'{sVv}', ghi_chu = N'{sGhiChu}',
                [date] = {sNow}, [time] = {sTime}, user_id = {sUserId}
            WHERE id = {editItemDccc.Id}");
            }
            isShowPopupDccc = false;
            await LoadSubTabsData(Escape(CurDmcc.Ma_cc_me));
            myStateMN.SetNewThongbao(Lap["Department transfer saved successfully!"]);
            StateHasChanged();
        }

        private void ConfirmDeleteDccc(CtBpcc item)
        {
            if (item.Cc0 == "1") { myStateMN.SetNewThongbao(Lap["Cannot delete the original record!"]); return; }
            PopupConfirmMessage = string.Format(Lap["Are you sure you want to delete this department transfer record (ID={0})?"], item.Id);
            OnConfirmAction = async () =>
            {
                await myDb.ExecuteSqlQueryAsync($"DELETE FROM ctbpcc WHERE id = {item.Id}");
                await LoadSubTabsData(Escape(CurDmcc.Ma_cc_me));
                myStateMN.SetNewThongbao(Lap["Department transfer deleted successfully!"]);
                StateHasChanged();
            };
            PopupConfirmVisible = true;
        }

        // =========================================================
        // TAB 5 – TẠM DỪNG PHÂN BỔ (dmccthoikc)
        // =========================================================
        private DxGrid GridTamdung;
        private object _selectedTdCard;
        public object SelectedTdCard
        {
            get => _selectedTdCard;
            set
            {
                if (_selectedTdCard != value)
                {
                    _selectedTdCard = value;
                    InvokeAsync(StateHasChanged);
                }
            }
        }
        private bool isShowPopupTamdung, isViewingTamdung, isNewTamdung;
        private CcThoiKc editItemTamdung = new CcThoiKc();
        private DxDateEdit<DateTime?> deTamdungNgay1;

        private List<CcThoiKc> FilteredLstTamdung
        {
            get
            {
                if (SelectedTdCard is not CtDmcc card || string.IsNullOrEmpty(card.Ma_cc))
                    return new List<CcThoiKc>();

                // Sắp xếp theo ngày
                var list = LstTamdung.Where(x => x.Ma_cc == card.Ma_cc)
                                     .OrderBy(x => x.Ngay_thoi_kh1)
                                     .ToList();

                // Đánh lại STT bắt đầu từ 1
                int stt = 1;
                list.ForEach(x => x.Stt = stt++);
                return list;
            }
        }

        private void AddNewTamdung()
        {
            isViewingTamdung = false; isNewTamdung = true;
            string maCcDefault = SelectedTdCard is CtDmcc card ? (card.Ma_cc ?? "") : "";
            editItemTamdung = new CcThoiKc { Ma_cc = maCcDefault, Ngay_thoi_kh1 = DateTime.Today };
            isShowPopupTamdung = true;
        }

        private void OpenEditTamdung(CcThoiKc item)
        {
            isViewingTamdung = false; isNewTamdung = false;
            editItemTamdung = CopyOf(item);
            isShowPopupTamdung = true;
        }

        private void OpenViewTamdung(CcThoiKc item)
        {
            isViewingTamdung = true; isNewTamdung = false;
            editItemTamdung = CopyOf(item);
            isShowPopupTamdung = true;
        }

        private async Task OnPopupShownTamdung()
        {
            if (deTamdungNgay1 != null && !isViewingTamdung) { await Task.Delay(80); await deTamdungNgay1.FocusAsync(); }
        }

        private async Task SaveTamdung()
        {
            if (!editItemTamdung.Ngay_thoi_kh1.HasValue)
            { myStateMN.SetNewThongbao(Lap["From date is required!"]); return; }
            if (!editItemTamdung.Ngay_thoi_kh2.HasValue)
            { myStateMN.SetNewThongbao(Lap["To date is required!"]); return; }
            if (editItemTamdung.Ngay_thoi_kh1.Value > editItemTamdung.Ngay_thoi_kh2.Value)
            { myStateMN.SetNewThongbao(Lap["From date must be less than or equal to To date!"]); return; }

            // Kiểm tra trùng khoảng thời gian (giống WinForm)
            var others = isNewTamdung
                ? LstTamdung
                : LstTamdung.Where(x => x.Id != editItemTamdung.Id).ToList();
            if (others.Any(x => x.Ngay_thoi_kh2.HasValue && x.Ngay_thoi_kh2.Value >= editItemTamdung.Ngay_thoi_kh1.Value &&
                                (!string.IsNullOrEmpty(editItemTamdung.Ma_cc) ? x.Ma_cc == editItemTamdung.Ma_cc : true)))
            { myStateMN.SetNewThongbao(Lap["The date range overlaps with another record!"]); return; }

            string sId = Escape(CurDmcc.Ma_cc_me);
            string sMaCc = Escape(editItemTamdung.Ma_cc ?? "");
            string sNgay1 = SqlDate(editItemTamdung.Ngay_thoi_kh1);
            string sNgay2 = SqlDate(editItemTamdung.Ngay_thoi_kh2);
            string sLyDo = Escape(editItemTamdung.Ly_do_thoi ?? "");
            string sNow = SqlDate(DateTime.Now), sTime = $"'{DateTime.Now:HH:mm:ss}'";
            string sUserId = (curUser?.User_id ?? 0).ToString();
            string sUserName = Escape(curUser?.User_name ?? "");

            if (isNewTamdung)
            {
                await myDb.ExecuteSqlQueryAsync($@"
                    EXEC FXCC_Them_thoi_cc N'{sMaCc}', '1', {sNgay1}, {sNgay2}, N'{sLyDo}', {sUserId}");
            }
            else
            {
                await myDb.ExecuteSqlQueryAsync($@"
                    UPDATE dmccthoikc SET
                        ngay_thoi_kh1 = {sNgay1}, ngay_thoi_kh2 = {sNgay2}, ly_do_thoi = N'{sLyDo}',
                        date = {sNow}, time = {sTime}, user_id = {sUserId}, user_name = N'{sUserName}'
                    WHERE id = {editItemTamdung.Id}");
            }
            isShowPopupTamdung = false;
            await LoadSubTabsData(Escape(CurDmcc.Ma_cc_me));
            myStateMN.SetNewThongbao(Lap["Allocation suspension saved successfully!"]);
            StateHasChanged();
        }

        private void ConfirmDeleteTamdung(CcThoiKc item)
        {
            PopupConfirmMessage = Lap["Are you sure you want to delete this allocation suspension record?"];
            OnConfirmAction = async () =>
            {
                await myDb.ExecuteSqlQueryAsync($"DELETE FROM dmccthoikc WHERE id = {item.Id}");
                await LoadSubTabsData(Escape(CurDmcc.Ma_cc_me));
                myStateMN.SetNewThongbao(Lap["Allocation suspension deleted successfully!"]);
                StateHasChanged();
            };
            PopupConfirmVisible = true;
        }

        // =========================================================
        // TAB 6 – KHAI BÁO GIẢM CC (ctdmcc.ngay_giam)
        // =========================================================
        private DxGrid GridGiamcc;
        private bool isShowPopupGiamcc, isViewingGiamcc;
        private EditGiamCc editItemGiamcc = new EditGiamCc();
        private DxComboBox<Fxcc2ListItem, string> cbGiamccMa;

        private void AddNewGiamcc()
        {
            if (LstGiamcc == null || LstGiamcc.Count == 0)
            {
                myStateMN.SetNewThongbao(Lap["No tool cards available!"]);
                return;
            }
            isViewingGiamcc = false;
            editItemGiamcc = new EditGiamCc();

            // Tự động gán ngày hiện tại cho tiện lợi
            editItemGiamcc.Ngay_giam = DateTime.Now.Date;
            editItemGiamcc.Ngay_kh1 = DateTime.Now.Date;
            isShowPopupGiamcc = true;
        }

        private void OpenEditGiamcc(EditGiamCc item)
        {
            // --- VALIDATE CHẶN CHÉO: Đã hỏng thì không cho sửa thông tin giảm ---
            if (item.Ngay_hong.HasValue)
            {
                myStateMN.SetNewThongbao(Lap["This tool card has already been declared as damaged and cannot be updated for disposal!"]); return;
            }

            isViewingGiamcc = false;
            editItemGiamcc = CopyOf(item);
            isShowPopupGiamcc = true;
        }
        private void OpenViewGiamcc(EditGiamCc item)
        {
            isViewingGiamcc = true;
            editItemGiamcc = CopyOf(item);
            isShowPopupGiamcc = true;
        }

        private async Task OnPopupShownGiamcc()
        {
            if (cbGiamccMa != null && !isViewingGiamcc) { await Task.Delay(80); await cbGiamccMa.FocusAsync(); }
        }

        private void OnNgayGiamccChanged(DateTime? v)
        {
            editItemGiamcc.Ngay_giam = v;
            if (v.HasValue && !editItemGiamcc.Ngay_kh1.HasValue)
                editItemGiamcc.Ngay_kh1 = v; // Auto-fill ngày thôi p.bổ = ngày giảm
        }

        private async Task SaveGiamcc()
        {
            // --- VALIDATE BẮT BUỘC NHẬP ---
            if (string.IsNullOrWhiteSpace(editItemGiamcc.Ma_giam_cc))
            { myStateMN.SetNewThongbao(Lap["Please select a disposal reason!"]); return; }
            if (!editItemGiamcc.Ngay_giam.HasValue)
            { myStateMN.SetNewThongbao(Lap["Disposal date is required!"]); return; }

            // --- VALIDATE CHẶN CHÉO (Phòng trường hợp ấn Thêm Mới rồi chọn nhầm thẻ đã hỏng) ---
            var card = LstGiamcc.FirstOrDefault(x => x.Ma_cc == editItemGiamcc.Ma_cc);
            if (card != null && card.Ngay_hong.HasValue)
            { myStateMN.SetNewThongbao(Lap["This tool card has already been declared as damaged and cannot be updated for disposal!"]); return; }

            // Logic check ngày tháng gốc của bạn
            if (editItemGiamcc.Ngay_giam.HasValue && CurDmcc.Ngay_mua.HasValue &&
                editItemGiamcc.Ngay_giam.Value.Date < CurDmcc.Ngay_mua.Value.Date)
            { myStateMN.SetNewThongbao(Lap["Disposal date must be after the purchase date!"]); return; }
            if (editItemGiamcc.Ngay_kh1.HasValue && CurDmcc.Ngay_mua.HasValue &&
                editItemGiamcc.Ngay_kh1.Value.Date < CurDmcc.Ngay_mua.Value.Date)
            { myStateMN.SetNewThongbao(Lap["Stop allocation date must be after the purchase date!"]); return; }

            // --- TỰ ĐỘNG ĐIỀN NGÀY THÔI P.BỔ NẾU NGƯỜI DÙNG XÓA TRẮNG ---
            if (!editItemGiamcc.Ngay_kh1.HasValue)
                editItemGiamcc.Ngay_kh1 = editItemGiamcc.Ngay_giam;

            string sMaCc = Escape(editItemGiamcc.Ma_cc ?? "");
            string sMaGiam = Escape(editItemGiamcc.Ma_giam_cc ?? "");
            string sNgayGiam = editItemGiamcc.Ngay_giam.HasValue ? SqlDate(editItemGiamcc.Ngay_giam) : "NULL";
            string sNgayKh1 = editItemGiamcc.Ngay_kh1.HasValue ? SqlDate(editItemGiamcc.Ngay_kh1) : "NULL";
            string sSoCt = Escape(editItemGiamcc.So_ct_giam ?? "");
            string sLyDo = Escape(editItemGiamcc.Ly_do_giam ?? "");
            string sNow = SqlDate(DateTime.Now), sTime = $"'{DateTime.Now:HH:mm:ss}'";
            string sUserId = (curUser?.User_id ?? 0).ToString();

            await myDb.ExecuteSqlQueryAsync($@"
                UPDATE ctdmcc SET
                    ma_giam_cc = N'{sMaGiam}', ngay_giam = {sNgayGiam}, ngay_kh1 = {sNgayKh1},
                    so_ct_giam = N'{sSoCt}', ly_do_giam = N'{sLyDo}',
                    date_giam0 = ISNULL(date_giam0, {sNow}),
                    time_giam0 = CASE WHEN ISNULL(time_giam0,'') = '' THEN {sTime} ELSE time_giam0 END,
                    user_id_giam0 = CASE WHEN ISNULL(user_id_giam0,0) = 0 THEN {sUserId} ELSE user_id_giam0 END,
                    date_giam = {sNow}, time_giam = {sTime}, user_id_giam = {sUserId}
                WHERE ma_cc = N'{sMaCc}'");

            isShowPopupGiamcc = false;
            await LoadSubTabsData(Escape(CurDmcc.Ma_cc_me));
            myStateMN.SetNewThongbao(Lap["Disposal declaration updated successfully!"]);
            StateHasChanged();
        }

        private void ConfirmDeleteGiamcc(EditGiamCc item)
        {
            PopupConfirmMessage = string.Format(Lap["Are you sure you want to delete the disposal info for card {0}?"], item.Ma_cc);
            OnConfirmAction = async () =>
            {
                string sMaCc = Escape(item.Ma_cc ?? "");
                string sNow = SqlDate(DateTime.Now), sTime = $"'{DateTime.Now:HH:mm:ss}'";
                string sUserId = (curUser?.User_id ?? 0).ToString();
                await myDb.ExecuteSqlQueryAsync($@"
                    UPDATE ctdmcc SET
                        ma_giam_cc = '', ngay_giam = NULL, ngay_kh1 = NULL,
                        so_ct_giam = '', ly_do_giam = '',
                        date_giam = {sNow}, time_giam = {sTime}, user_id_giam = {sUserId}
                    WHERE ma_cc = N'{sMaCc}'");
                await LoadSubTabsData(Escape(CurDmcc.Ma_cc_me));
                myStateMN.SetNewThongbao(Lap["Disposal information deleted successfully!"]);
                StateHasChanged();
            };
            PopupConfirmVisible = true;
        }

        // =========================================================
        // TAB 7 – KHAI BÁO HỎNG CC (ctdmcc.ngay_hong)
        // =========================================================
        private DxGrid GridHongcc;
        private bool isShowPopupHongcc, isViewingHongcc;
        private EditHongCc editItemHongcc = new EditHongCc();
        private DxComboBox<Fxcc2ListItem, string> cbHongccMa;

        private void OpenEditHongcc(EditHongCc item)
        {
            // --- VALIDATE CHẶN CHÉO: Đã giảm thì không cho sửa thông tin hỏng ---
            if (item.Ngay_giam.HasValue)
            {
                myStateMN.SetNewThongbao(Lap["This tool card has already been declared as disposed and cannot be updated for damage!"]); return;
            }

            isViewingHongcc = false;
            editItemHongcc = CopyOf(item);
            isShowPopupHongcc = true;
        }

        private void OpenViewHongcc(EditHongCc item)
        {
            isViewingHongcc = true;
            editItemHongcc = CopyOf(item);
            isShowPopupHongcc = true;
        }

        private async Task OnPopupShownHongcc()
        {
            if (cbHongccMa != null && !isViewingHongcc) { await Task.Delay(80); await cbHongccMa.FocusAsync(); }
        }

        private void OnNgayHongccChanged(DateTime? v)
        {
            editItemHongcc.Ngay_hong = v;
            if (v.HasValue && !editItemHongcc.Ngay_kh1.HasValue)
                editItemHongcc.Ngay_kh1 = v;
        }

        private async Task SaveHongcc()
        {
            // --- VALIDATE BẮT BUỘC NHẬP ---
            if (string.IsNullOrWhiteSpace(editItemHongcc.Ma_hong_cc))
            { myStateMN.SetNewThongbao(Lap["Please select a damage reason!"]); return; }
            if (!editItemHongcc.Ngay_hong.HasValue)
            { myStateMN.SetNewThongbao(Lap["Damage date is required!"]); return; }

            // --- VALIDATE CHẶN CHÉO ---
            var card = LstHongcc.FirstOrDefault(x => x.Ma_cc == editItemHongcc.Ma_cc);
            if (card != null && card.Ngay_giam.HasValue)
            { myStateMN.SetNewThongbao(Lap["This tool card has already been declared as disposed and cannot be updated for damage!"]); return; }

            if (editItemHongcc.Ngay_hong.HasValue && CurDmcc.Ngay_mua.HasValue &&
                editItemHongcc.Ngay_hong.Value.Date < CurDmcc.Ngay_mua.Value.Date)
            { myStateMN.SetNewThongbao(Lap["Damage date must be after the purchase date!"]); return; }

            // --- TỰ ĐỘNG ĐIỀN NGÀY THÔI P.BỔ ---
            if (!editItemHongcc.Ngay_kh1.HasValue)
                editItemHongcc.Ngay_kh1 = editItemHongcc.Ngay_hong;

            string sMaCc = Escape(editItemHongcc.Ma_cc ?? "");
            string sMaHong = Escape(editItemHongcc.Ma_hong_cc ?? "");
            string sNgayHong = editItemHongcc.Ngay_hong.HasValue ? SqlDate(editItemHongcc.Ngay_hong) : "NULL";
            string sNgayKh1 = editItemHongcc.Ngay_kh1.HasValue ? SqlDate(editItemHongcc.Ngay_kh1) : "NULL";
            string sSoCt = Escape(editItemHongcc.So_ct_hong ?? "");
            string sLyDo = Escape(editItemHongcc.Ly_do_hong ?? "");
            string sNow = SqlDate(DateTime.Now), sTime = $"'{DateTime.Now:HH:mm:ss}'";
            string sUserId = (curUser?.User_id ?? 0).ToString();

            await myDb.ExecuteSqlQueryAsync($@"
                UPDATE ctdmcc SET
                    ma_hong_cc = N'{sMaHong}', ngay_hong = {sNgayHong}, ngay_kh1 = {sNgayKh1},
                    so_ct_hong = N'{sSoCt}', ly_do_hong = N'{sLyDo}',
                    date_hong0 = ISNULL(date_hong0, {sNow}),
                    time_hong0 = CASE WHEN ISNULL(time_hong0,'') = '' THEN {sTime} ELSE time_hong0 END,
                    user_id_hong0 = CASE WHEN ISNULL(user_id_hong0,0) = 0 THEN {sUserId} ELSE user_id_hong0 END,
                    date_hong = {sNow}, time_hong = {sTime}, user_id_hong = {sUserId}
                WHERE ma_cc = N'{sMaCc}'");

            isShowPopupHongcc = false;
            await LoadSubTabsData(Escape(CurDmcc.Ma_cc_me));
            myStateMN.SetNewThongbao(Lap["Damage declaration updated successfully!"]);
            StateHasChanged();
        }

        private void ConfirmDeleteHongcc(EditHongCc item)
        {
            PopupConfirmMessage = string.Format(Lap["Are you sure you want to delete the damage info for card {0}?"], item.Ma_cc);
            OnConfirmAction = async () =>
            {
                string sMaCc = Escape(item.Ma_cc ?? "");
                string sNow = SqlDate(DateTime.Now), sTime = $"'{DateTime.Now:HH:mm:ss}'";
                string sUserId = (curUser?.User_id ?? 0).ToString();
                await myDb.ExecuteSqlQueryAsync($@"
                    UPDATE ctdmcc SET
                        ma_hong_cc = '', ngay_hong = NULL, ngay_kh1 = NULL,
                        so_ct_hong = '', ly_do_hong = '',
                        date_hong = {sNow}, time_hong = {sTime}, user_id_hong = {sUserId}
                    WHERE ma_cc = N'{sMaCc}'");
                await LoadSubTabsData(Escape(CurDmcc.Ma_cc_me));
                myStateMN.SetNewThongbao(Lap["Damage information deleted successfully!"]);
                StateHasChanged();
            };
            PopupConfirmVisible = true;
        }

        // =========================================================
        // TAB 8 – THÔI PHÂN BỔ (ctdmcc.ngay_kh1 / ly_do_thoi)
        // =========================================================
        private DxGrid GridThoicc;
        private bool isShowPopupThoicc, isViewingThoicc;
        private EditThoiCc editItemThoicc = new EditThoiCc();
        private DxDateEdit<DateTime?> deThoiccNgay;

        private void OpenEditThoicc(EditThoiCc item)
        {
            isViewingThoicc = false;
            editItemThoicc = CopyOf(item);
            isShowPopupThoicc = true;
        }

        private void OpenViewThoicc(EditThoiCc item)
        {
            isViewingThoicc = true;
            editItemThoicc = CopyOf(item);
            isShowPopupThoicc = true;
        }

        private async Task OnPopupShownThoicc()
        {
            if (deThoiccNgay != null && !isViewingThoicc) { await Task.Delay(80); await deThoiccNgay.FocusAsync(); }
        }

        private async Task SaveThoicc()
        {
            if (!editItemThoicc.Ngay_kh1.HasValue)
            { myStateMN.SetNewThongbao(Lap["Stop allocation date is required!"]); return; }
            if (CurDmcc.Ngay_mua.HasValue && editItemThoicc.Ngay_kh1.Value.Date < CurDmcc.Ngay_mua.Value.Date)
            { myStateMN.SetNewThongbao(Lap["Stop allocation date must be after the purchase date!"]); return; }

            string sMaCc = Escape(editItemThoicc.Ma_cc ?? "");
            string sNgay = SqlDate(editItemThoicc.Ngay_kh1);
            string sLyDo = Escape(editItemThoicc.Ly_do_thoi ?? "");
            string sNow = SqlDate(DateTime.Now), sTime = $"'{DateTime.Now:HH:mm:ss}'";
            string sUserId = (curUser?.User_id ?? 0).ToString();

            await myDb.ExecuteSqlQueryAsync($@"
                UPDATE ctdmcc SET
                    ngay_kh1 = {sNgay}, ly_do_thoi = N'{sLyDo}',
                    date_thoi0 = ISNULL(date_thoi0, {sNow}),
                    time_thoi0 = CASE WHEN ISNULL(time_thoi0,'') = '' THEN {sTime} ELSE time_thoi0 END,
                    user_id_thoi0 = CASE WHEN ISNULL(user_id_thoi0,0) = 0 THEN {sUserId} ELSE user_id_thoi0 END,
                    date_thoi = {sNow}, time_thoi = {sTime}, user_id_thoi = {sUserId}
                WHERE ma_cc = N'{sMaCc}'");

            isShowPopupThoicc = false;
            await LoadSubTabsData(Escape(CurDmcc.Ma_cc_me));
            myStateMN.SetNewThongbao(Lap["Stop allocation updated successfully!"]);
            StateHasChanged();
        }

        private void ConfirmDeleteThoicc(EditThoiCc item)
        {
            PopupConfirmMessage = string.Format(Lap["Are you sure you want to delete the stop allocation info for card {0}?"], item.Ma_cc);
            OnConfirmAction = async () =>
            {
                string sMaCc = Escape(item.Ma_cc ?? "");
                string sNow = SqlDate(DateTime.Now), sTime = $"'{DateTime.Now:HH:mm:ss}'";
                string sUserId = (curUser?.User_id ?? 0).ToString();
                await myDb.ExecuteSqlQueryAsync($@"
                    UPDATE ctdmcc SET
                        ngay_kh1 = NULL, ly_do_thoi = '',
                        date_thoi = {sNow}, time_thoi = {sTime}, user_id_thoi = {sUserId}
                    WHERE ma_cc = N'{sMaCc}'");
                await LoadSubTabsData(Escape(CurDmcc.Ma_cc_me));
                myStateMN.SetNewThongbao(Lap["Stop allocation information deleted successfully!"]);
                StateHasChanged();
            };
            PopupConfirmVisible = true;
        }

        // =========================================================
        // HELPERS
        // =========================================================
        private static string Escape(string s) => (s ?? "").Replace("'", "''");
        private static string SqlDate(DateTime? d) => d.HasValue ? $"'{d.Value:yyyy-MM-dd}'" : "NULL";

        private static T CopyOf<T>(T src) where T : new()
        {
            var dst = new T();
            foreach (var p in typeof(T).GetProperties())
                if (p.CanRead && p.CanWrite) p.SetValue(dst, p.GetValue(src));
            return dst;
        }

        public void Dispose() { }
    }
}
