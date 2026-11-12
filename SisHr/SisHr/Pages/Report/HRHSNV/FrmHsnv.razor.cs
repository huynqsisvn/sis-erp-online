using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SisHr.Pages.Report.HRHSNV.Tabs;
using SisCom.Model;
using SisCom.Shared;
using SisLib.Lib;
using SisLib.List;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SisHr.Pages.Report.HRHSNV
{
    /// <summary>
    /// Code-behind for FrmHsnv.razor
    /// Handles data loading, validation, and saving for Employee (Nhân sự) form
    /// </summary>
    public partial class FrmHsnv
    {
        // ===== DATA MODEL =====
        public HsnvDetailModel CurRow { get; set; } = new HsnvDetailModel();

        // ===== UI STATE =====
        private ElementReference ScrollContainer;
        private int ActiveSection { get; set; } = 1;
        public bool ShowLoadding { get; set; } = false;

        // ===== SECTION VISIBILITY =====
        // Tab 1-7 luôn hiển thị
        private bool IsShowSocialInsurance { get; set; } = true;
        private bool IsShowUnionParty { get; set; } = true;
        private bool IsShowSalary { get; set; } = true;
        private bool IsShowTransferUnit { get; set; } = true;
        // Tab 8-13: chỉ hiển thị ở chế độ View
        private bool IsShowHopDong { get; set; } = false;
        private bool IsShowQtbh { get; set; } = false;
        private bool IsShowQhgd { get; set; } = false;
        private bool IsShowDieuChuyen { get; set; } = false;
        private bool IsShowNghiViec { get; set; } = false;
        private bool IsShowQtct { get; set; } = false;

        // ===== TAB REFERENCES (13 Tab) =====
        private TabMainInfo1 tabMainInfo;
        private TabDeduction2 tabDeduction;
        private TabSocialInsurance4 tabSocialInsurance;
        private TabUnionParty5 tabUnionParty;
        private TabSalary6 tabSalary;
        private TabTransferUnit7 tabTransferUnit;
        private TabResignation12 tabResignation;
        private DotNetObjectReference<FrmHsnv> Pageobj;

        // ===== PHOTO STATE =====
        private byte[] _pendingPhotoBytes = null;

        // ===== INITIALIZATION =====
        protected override void OnInitialized()
        {
            sqlTableName = "dmns";
            sqlTableView = "v_dmns";
            SqlTableKey = "ma_ns";
            CheckSqlTableKey = "ma_ns";
            ParentPage = string.IsNullOrEmpty(ParentPage) ? "hrhsnv" : ParentPage;
            CurRow = new HsnvDetailModel();

            if (string.IsNullOrEmpty(Menu_id))
                Menu_id = "wr.bc.01";

            base.OnInitialized();
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await PageLoad();
                await Language();

                if (curActionTask.ToString() == "View" || curActionTask.ToString() == "0")
                {
                    if (!string.IsNullOrEmpty(Ma_ma))
                        await ViewRecord(Ma_ma);
                }
                else if (curActionTask.ToString() == "Edit" || curActionTask.ToString() == "1")
                {
                    if (!string.IsNullOrEmpty(Ma_ma))
                        await EditRecord(Ma_ma);
                }
                else if (curActionTask.ToString() == "New" || curActionTask.ToString() == "2")
                {
                    await NewRecord(Ma_ma);
                }
                else
                {
                    if (!string.IsNullOrEmpty(Ma_ma))
                        await ViewRecord(Ma_ma);
                }
            }
            catch (ArgumentNullException)
            {
                // Ignore system menu loading errors
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao("Error: " + ex.Message);
                StateHasChanged();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    await base.OnAfterRenderAsync(firstRender);
                    Pageobj = DotNetObjectReference.Create(this);

                    // 2. Gửi cho JS biết đây là đối tượng sẽ nhận lệnh Enter
                    await JS.InvokeVoidAsync("SetPageObj", Pageobj);

                    // 3. Kích hoạt tính năng Enter as Tab
                    await JS.InvokeVoidAsync("AttachEnterAsTab");
                }
                catch (ArgumentNullException)
                {
                    // Ignore null reference in menu system
                }
                catch (JSDisconnectedException)
                {
                    // Ignore JS interop errors during circuit disposal
                }
                catch (Exception ex)
                {
                    // Log error
                }
            }
        }

        // ===== VIEW / EDIT / NEW =====
        private async Task ViewRecord(string key)
        {
            CurRow = new HsnvDetailModel();
            AllowEditMode = false;
            AllowEditMa = false;
            _pendingPhotoBytes = null;
            // ✅ Ở chế độ View: hiển thị đủ 13 tab
            IsShowHopDong = true;
            IsShowQtbh = true;
            IsShowQhgd = true;
            IsShowDieuChuyen = true;
            IsShowNghiViec = true;
            IsShowQtct = true;
            await LoadData(key);
        }

        private async Task EditRecord(string key)
        {
            CurRow = new HsnvDetailModel();
            AllowEditMode = true;
            AllowEditMa = false;
            _pendingPhotoBytes = null;
            // ✅ Ở chế độ Edit: hiển thị đủ 13 tab (giống View)
            IsShowHopDong = true;
            IsShowQtbh = true;
            IsShowQhgd = true;
            IsShowDieuChuyen = true;
            IsShowNghiViec = true;
            IsShowQtct = true;
            await LoadData(key);
        }

        private async Task NewRecord(string key)
        {
            // ✅ Lấy mã DVCS từ State Manager (giống như FrmZcfats)
            string ma_dvcs = myStateMN.GetMa_Dvcs();
            if (string.IsNullOrEmpty(ma_dvcs) || ma_dvcs.Trim().ToUpper() == "ALL")
            {
                ma_dvcs = ""; // Để trống nếu không có DVCS được chọn
            }

            CurRow = new HsnvDetailModel
            {
                Ma_ns = key ?? "",
                Ten_ns = "",
                Ten_ns2 = "",
                Ma_dvcs = ma_dvcs,  // ✅ Lấy mã DVCS từ State Manager
                Ten_dvcs = "",
                Status = "1",
                Ngay_vao = DateTime.Now.Date,
                Gioi_tinh = "",
                Bi_danh = "",
                Noi_sinh = "",
                Nguyen_quan = "",
                Cmnd = "",
                Nc_cmnd = "",
                Quoc_tich = "",
                Quoc_tich2 = "",
                Ma_dantoc = "",
                Ma_quoctich = "",
                Ma_tongiao = "",
                Ma_tp_banthan = "",
                Ma_tp_giadinh = "",
                Ma_ngoaingu = "",
                Ma_tinhoc = "",
                Ma_tt_honnhan = "",
                Ma_td_vanhoa = "",
                Ma_tt_suckhoe = "",
                Ma_bangcap = "",
                Dien_thoai = "",
                Dien_thoai_co_dinh = "",
                Fax = "",
                Dia_chi = "",
                Ho_khau = "",
                Tinh_thanh = "",
                Dc_cu_tru = "",
                Khicannhantincho = "",
                Diachinhantin = "",
                Ma_bpns = "",
                Ma_todoi = "",
                Khoi = "",
                Ma_loai_hdong = "",
                Ngay_bien_che = null,
                Ngay_hdld = null,
                Ma_trinhdo = "",
                Ma_chuyennganh = "",
                Ma_chucvu = "",
                Ma_chucdanh = "",
                Ma_loai_td = "",
                Image_url = "",
                E_mail = "",
                Ma_so_thue = "",
                Ngay_mst = null,
                Nc_mst = "",
                Ngay_ctru = null,
                Hd_thue = "",
                So_so_bhxh = "",
                So_the_bhyt = "",
                Ma_noi_kcb = "",
                Noi_cap_so_bhxh = "",
                Ma_chinhsach = "",
                Ma_noi_cap_tbhyt = "",
                Noi_ket_nap_doan = "",
                Ma_chucvu_doan = "",
                Ma_chucvu_dang = "",
                Ma_chucvu_quandoi = "",
                Trinh_do_trinh_tri = "",
                Trinh_do_quan_ly = "",
                Ma_lydonghi = "",
                Ghi_chu = "",
                Dt_cu_tru = 1,
                Thue_ck = 0,
                Khu_kt_ck = 0,
                Uq_qt_ck = 0,
                Hdld3thang = 0,
                An_com = 0,
                Da_chuyen_di = "",
                He_so_luong = 0,
                He_so_phu_cap = 0,
                Bac_luong = 0,
                Muc_luong = 0,
                Giam_tru = 0,
                Muc_gt = 0,
                Muc_gt_nt = 0,
                Ma_td1 = "",
                Ma_td2 = "",
                Ma_td3 = "",
                Ngay_td1 = null,
                Ngay_td2 = null,
                Ngay_td3 = null,
                Sl_td1 = 0,
                Sl_td2 = 0,
                Sl_td3 = 0,
                Gc_td1 = "",
                Gc_td2 = "",
                Gc_td3 = "",
                Ma_tra_cuu = "",
                Stt_rec = "",
                Search = "",
                Nghi_viec = 0,
                Ngay_nv = null,
                So_qd_nv = "",
                Ngay_qd_nv = null,
                Ly_do_nv = "",
                Ghi_chu_nv = ""
            };
            AllowEditMode = true;
            AllowEditMa = true;
            // ✅ Ở chế độ New: giữ nguyên (chỉ 7 tab)
            IsShowHopDong = false;
            IsShowQtbh = false;
            IsShowQhgd = false;
            IsShowDieuChuyen = false;
            IsShowNghiViec = false;
            IsShowQtct = false;
            StateHasChanged();
        }

        private async Task OnEdit()
        {
            AllowEditMode = true;
            AllowEditMa = false;
            StateHasChanged();
        }

        // ===== LOAD DATA FROM MULTIPLE TABLES =====
        private async Task LoadData(string key)
        {
            try
            {
                ShowLoadding = true;
                StateHasChanged();

                string safeKey = Esc(key);

                // Load main employee info
                string sqlMain = $@"
                    SELECT 
                        n.*, 
                        d.ten_dvcs, 
                        d.ten_dvcs2 
                    FROM {sqlTableName} n 
                    LEFT JOIN dmdvcs d ON n.ma_dvcs = d.ma_dvcs 
                    WHERE n.ma_ns = N'{safeKey}'";

                DataSet dsMain = await myDb.LoadDataset(sqlMain);

                if (dsMain != null && dsMain.Tables.Count > 0 && dsMain.Tables[0].Rows.Count > 0)
                {
                    var list = myDb.ConvertDataTable<HsnvDetailModel>(dsMain.Tables[0]);
                    if (list != null && list.Count > 0)
                    {
                        CurRow = list[0];
                        var row = dsMain.Tables[0].Rows[0];
                        CurRow.Nguyen_quan = (CurRow.Nguyen_quan ?? "").PadRight(16, ' ');
                        CurRow.Quoc_tich = (CurRow.Quoc_tich ?? "").PadRight(16, ' ');
                        InitializeNullableFields();
                    }
                }
                else
                {
                    CurRow = new HsnvDetailModel() { Ma_ns = key };
                    InitializeNullableFields();
                }

                // Load Tab 2: Deduction (ctgtk)
                // ✅ ĐÚNG MAPPING: GT01=Gia_canh, GT02=A_001(BH TN), GT03=A_002(BH YT), GT04=A_003(BH XH), GT05=Tien_gt
                var dsCtgtk = await myDb.LoadDataset($@"
                    SELECT ngay_hl1, ngay_hl2, key_gt,
                           MAX(luong_thang_nt) as luong_thang_nt, MAX(luong_thang) as luong_thang,
                           MAX(luong_nt) as luong_nt, MAX(luong) as luong,
                           MAX(CASE WHEN ma_gt = 'GT01' THEN tien_gt ELSE 0 END) AS Gia_canh,
                           MAX(CASE WHEN ma_gt = 'GT02' THEN tien_gt ELSE 0 END) AS A_001,
                           MAX(CASE WHEN ma_gt = 'GT03' THEN tien_gt ELSE 0 END) AS A_002,
                           MAX(CASE WHEN ma_gt = 'GT04' THEN tien_gt ELSE 0 END) AS A_003,
                           MAX(CASE WHEN ma_gt = 'GT05' THEN tien_gt ELSE 0 END) AS Tien_gt,
                           MAX(dien_giai) as Dien_giai
                    FROM ctgtk WHERE ma_ns = N'{safeKey}'
                    GROUP BY ngay_hl1, ngay_hl2, key_gt ORDER BY key_gt");
                CurRow.ListCtgtk = dsCtgtk?.Tables.Count > 0 ? myDb.ConvertDataTable<CtgtkRow>(dsCtgtk.Tables[0]) ?? new() : new();
                SetStt(CurRow.ListCtgtk);

                var dsCtnpt = await myDb.LoadDataset($"SELECT * FROM ctnpt WHERE ma_ns = N'{safeKey}'");
                CurRow.ListCtnpt = dsCtnpt?.Tables.Count > 0 ? myDb.ConvertDataTable<CtnptRow>(dsCtnpt.Tables[0]) ?? new() : new();
                SetStt(CurRow.ListCtnpt);

                // Load Tab 8: Labor contract (dmhdld)
                var dsHdld = await myDb.LoadDataset($"SELECT * FROM dmhdld WHERE ma_ns = N'{safeKey}' ORDER BY ngay_hd");
                CurRow.ListHopDong = dsHdld?.Tables.Count > 0 ? myDb.ConvertDataTable<HopDongRow>(dsHdld.Tables[0]) ?? new() : new();
                SetStt(CurRow.ListHopDong);

                // Load Tab 9: Insurance history (dmqtbh)
                var dsQtbh = await myDb.LoadDataset($"SELECT * FROM dmqtbh WHERE ma_ns = N'{safeKey}' ORDER BY tu_ngay");
                CurRow.ListQtbh = dsQtbh?.Tables.Count > 0 ? myDb.ConvertDataTable<QtbhRow>(dsQtbh.Tables[0]) ?? new() : new();
                SetStt(CurRow.ListQtbh);

                // Load Tab 10: Family relationship (dmtpgd)
                var dsQhgd = await myDb.LoadDataset($"SELECT * FROM dmtpgd WHERE ma_ns = N'{safeKey}'");
                CurRow.ListQhgd = dsQhgd?.Tables.Count > 0 ? myDb.ConvertDataTable<QhgdRow>(dsQhgd.Tables[0]) ?? new() : new();
                SetStt(CurRow.ListQhgd);

                // Load Tab 11: Transfer (dmqtdc)
                var dsQtdc = await myDb.LoadDataset($"SELECT * FROM dmqtdc WHERE ma_ns = N'{safeKey}' ORDER BY ngay_dc_moi");
                CurRow.ListDieuChuyen = dsQtdc?.Tables.Count > 0 ? myDb.ConvertDataTable<DieuChuyenRow>(dsQtdc.Tables[0]) ?? new() : new();
                SetStt(CurRow.ListDieuChuyen);

                // Load Tab 13: Working process (dmnvct)
                var dsQtct = await myDb.LoadDataset($"SELECT * FROM dmnvct WHERE ma_ns = N'{safeKey}' ORDER BY ngay_di");
                CurRow.ListQtct = dsQtct?.Tables.Count > 0 ? myDb.ConvertDataTable<QtctRow>(dsQtct.Tables[0]) ?? new() : new();
                SetStt(CurRow.ListQtct);

                // Load Tab 12: Resignation (dmnvn)
                var dsNvn = await myDb.LoadDataset($"SELECT * FROM dmnvn WHERE ma_ns = N'{safeKey}'");
                if (dsNvn != null && dsNvn.Tables.Count > 0 && dsNvn.Tables[0].Rows.Count > 0)
                {
                    var listNvn = myDb.ConvertDataTable<NghiViecRow>(dsNvn.Tables[0]);
                    if (listNvn != null && listNvn.Count > 0)
                        CurRow.NghiViecInfo = listNvn[0];
                }
                if (CurRow.NghiViecInfo == null)
                    CurRow.NghiViecInfo = new NghiViecRow();

                // Load ảnh nhân viên từ bảng dmnsimg
                await LoadEmployeePhoto(safeKey);
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao("Lỗi tải dữ liệu: " + ex.Message);
            }
            finally
            {
                ShowLoadding = false;
                StateHasChanged();
            }
        }

        private async Task LoadEmployeePhoto(string safeKey)
        {
            try
            {
                var dsImg = await myDb.LoadDataset($"SELECT img_ns FROM dmnsimg WHERE ma_ns = N'{safeKey}'");
                if (dsImg != null && dsImg.Tables.Count > 0 && dsImg.Tables[0].Rows.Count > 0)
                {
                    var imgValue = dsImg.Tables[0].Rows[0]["img_ns"];
                    if (imgValue != null && imgValue != DBNull.Value)
                    {
                        byte[] imgBytes = (byte[])imgValue;
                        string base64 = Convert.ToBase64String(imgBytes);
                        CurRow.Image_url = $"data:image/png;base64,{base64}";
                    }
                }
            }
            catch
            {
                // Không có ảnh thì bỏ qua
            }
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }

        private void SetStt<T>(List<T> list)
        {
            if (list == null || list.Count == 0) return;
            int stt = 1;
            foreach (dynamic item in list)
            {
                item.Stt = stt++;
            }
        }

        /// <summary>
        /// Kiểm tra nhân viên có hợp đồng lao động hay không
        /// Dùng cho Tab 9 (Quá trình tham gia bảo hiểm) - chỉ hiện khi có HD
        /// </summary>
        private bool HasLaborContract()
        {
            return CurRow?.ListHopDong != null && CurRow.ListHopDong.Count > 0;
        }

        private void InitializeNullableFields()
        {
            CurRow.Ten_ns = CurRow.Ten_ns ?? "";
            CurRow.Ten_ns2 = CurRow.Ten_ns2 ?? "";
            CurRow.Gioi_tinh = CurRow.Gioi_tinh ?? "";
            CurRow.Bi_danh = CurRow.Bi_danh ?? "";
            CurRow.Quoc_tich = CurRow.Quoc_tich ?? "";
            CurRow.Nguyen_quan = CurRow.Nguyen_quan ?? "";
            CurRow.Noi_sinh = CurRow.Noi_sinh ?? "";
            CurRow.Cmnd = CurRow.Cmnd ?? "";
            CurRow.Nc_cmnd = CurRow.Nc_cmnd ?? "";
            CurRow.Quoc_tich2 = CurRow.Quoc_tich2 ?? "";
            CurRow.Ma_dantoc = CurRow.Ma_dantoc ?? "";
            CurRow.Ma_quoctich = CurRow.Ma_quoctich ?? "";
            CurRow.Ma_tongiao = CurRow.Ma_tongiao ?? "";
            CurRow.Ma_tp_banthan = CurRow.Ma_tp_banthan ?? "";
            CurRow.Ma_tp_giadinh = CurRow.Ma_tp_giadinh ?? "";
            CurRow.Ma_ngoaingu = CurRow.Ma_ngoaingu ?? "";
            CurRow.Ma_tinhoc = CurRow.Ma_tinhoc ?? "";
            CurRow.Ma_tt_honnhan = CurRow.Ma_tt_honnhan ?? "";
            CurRow.Ma_td_vanhoa = CurRow.Ma_td_vanhoa ?? "";
            CurRow.Ma_tt_suckhoe = CurRow.Ma_tt_suckhoe ?? "";
            CurRow.Ma_bangcap = CurRow.Ma_bangcap ?? "";
            CurRow.Dien_thoai = CurRow.Dien_thoai ?? "";
            CurRow.Dien_thoai_co_dinh = CurRow.Dien_thoai_co_dinh ?? "";
            CurRow.Fax = CurRow.Fax ?? "";
            CurRow.Dia_chi = CurRow.Dia_chi ?? "";
            CurRow.Ho_khau = CurRow.Ho_khau ?? "";
            CurRow.Tinh_thanh = CurRow.Tinh_thanh ?? "";
            CurRow.Dc_cu_tru = CurRow.Dc_cu_tru ?? "";
            CurRow.E_mail = CurRow.E_mail ?? "";
            CurRow.Ma_so_thue = CurRow.Ma_so_thue ?? "";
            CurRow.Nc_mst = CurRow.Nc_mst ?? "";
            CurRow.Ma_trinhdo = CurRow.Ma_trinhdo ?? "";
            CurRow.Ma_chuyennganh = CurRow.Ma_chuyennganh ?? "";
            CurRow.Ma_chucvu = CurRow.Ma_chucvu ?? "";
            CurRow.Ma_chucdanh = CurRow.Ma_chucdanh ?? "";
            CurRow.Khicannhantincho = CurRow.Khicannhantincho ?? "";
            CurRow.Diachinhantin = CurRow.Diachinhantin ?? "";
            CurRow.So_so_bhxh = CurRow.So_so_bhxh ?? "";
            CurRow.So_the_bhyt = CurRow.So_the_bhyt ?? "";
            CurRow.Ma_noi_kcb = CurRow.Ma_noi_kcb ?? "";
            CurRow.Noi_cap_so_bhxh = CurRow.Noi_cap_so_bhxh ?? "";
            CurRow.Ma_chinhsach = CurRow.Ma_chinhsach ?? "";
            CurRow.Ma_noi_cap_tbhyt = CurRow.Ma_noi_cap_tbhyt ?? "";
            CurRow.Noi_ket_nap_doan = CurRow.Noi_ket_nap_doan ?? "";
            CurRow.Ma_chucvu_doan = CurRow.Ma_chucvu_doan ?? "";
            CurRow.Ma_chucvu_dang = CurRow.Ma_chucvu_dang ?? "";
            CurRow.Ma_chucvu_quandoi = CurRow.Ma_chucvu_quandoi ?? "";
            CurRow.Trinh_do_trinh_tri = CurRow.Trinh_do_trinh_tri ?? "";
            CurRow.Trinh_do_quan_ly = CurRow.Trinh_do_quan_ly ?? "";
            CurRow.Ma_loai_td = CurRow.Ma_loai_td ?? "";
            CurRow.Image_url = CurRow.Image_url ?? "";
            CurRow.Khoi = CurRow.Khoi ?? "";
            CurRow.Hd_thue = CurRow.Hd_thue ?? "";
            CurRow.Ma_lydonghi = CurRow.Ma_lydonghi ?? "";
            CurRow.Noi_ket_nap_dang = CurRow.Noi_ket_nap_dang ?? "";
            CurRow.Da_chuyen_di = CurRow.Da_chuyen_di ?? "";
            CurRow.Ghi_chu = CurRow.Ghi_chu ?? "";
            CurRow.Ma_td1 = CurRow.Ma_td1 ?? "";
            CurRow.Ma_td2 = CurRow.Ma_td2 ?? "";
            CurRow.Ma_td3 = CurRow.Ma_td3 ?? "";
            CurRow.Gc_td1 = CurRow.Gc_td1 ?? "";
            CurRow.Gc_td2 = CurRow.Gc_td2 ?? "";
            CurRow.Gc_td3 = CurRow.Gc_td3 ?? "";
            CurRow.Ma_tra_cuu = CurRow.Ma_tra_cuu ?? "";
            CurRow.Stt_rec = CurRow.Stt_rec ?? "";
            CurRow.Search = CurRow.Search ?? "";
            CurRow.So_qd_nv = CurRow.So_qd_nv ?? "";
            CurRow.Ly_do_nv = CurRow.Ly_do_nv ?? "";
            CurRow.Ghi_chu_nv = CurRow.Ghi_chu_nv ?? "";
        }

        // ===== PHOTO HANDLING =====
        private async Task OnPhotoSelectedHandler(InputFileChangeEventArgs e)
        {
            try
            {
                var file = e.File;
                if (file == null) return;

                const long maxFileSize = 5 * 1024 * 1024;
                if (file.Size > maxFileSize)
                {
                    myStateMN.SetNewThongbao(Lap["File size must not exceed 5MB"] ?? "File size must not exceed 5MB");
                    return;
                }

                using var stream = file.OpenReadStream(maxFileSize);
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                _pendingPhotoBytes = ms.ToArray();

                string base64 = Convert.ToBase64String(_pendingPhotoBytes);
                string mime = file.ContentType ?? "image/jpeg";
                CurRow.Image_url = $"data:{mime};base64,{base64}";

                myStateMN.SetNewThongbao(Lap["Photo updated successfully"] ?? "Ảnh đã được cập nhật");
                StateHasChanged();
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao("Error uploading photo: " + ex.Message);
            }
        }

        // ===== VALIDATION & SAVE ORCHESTRATION =====

        /// <summary>
        /// Validate chỉ những tab đang show (IsShow* = true)
        /// - Tab 1, 2, 3 luôn validate (luôn show)
        /// - Tab 10, 11, 13 luôn validate (luôn show)
        /// - Tab 4-7 chỉ validate nếu IsShow* = true
        /// - Tab 8 (Hợp đồng) luôn validate nếu show
        /// </summary>
        private bool ValidateAllTabs()
        {
            // Tab 1, 2, 3 luôn hiển thị trên UI nên luôn validate
            if (!ValidateTabMainInfo()) return false;
            if (!ValidateTabDeduction()) return false;
            if (!ValidateTabDependent()) return false;

            // CÁC TAB KHÁC: CHỈ VALIDATE NẾU ĐANG BẬT HIỂN THỊ (IsShow... == true)

            // Tab 4, 5, 6, 7
            if (IsShowSocialInsurance && !ValidateTabSocialInsurance()) return false;
            if (IsShowUnionParty && !ValidateTabUnionParty()) return false;
            if (IsShowSalary && !ValidateTabSalary()) return false;
            if (IsShowTransferUnit && !ValidateTabTransferUnit()) return false;

            // Tab 8: Hợp đồng lao động - luôn validate nếu show (chế độ thêm mới, sửa, xem)
            if (IsShowHopDong && !ValidateTabContract()) return false;

            // Tab 9: Quá trình tham gia bảo hiểm - validate nếu show và Edit/View + có HD
            if (IsShowQtbh && !ValidateTabInsuranceHistory()) return false;

            // Tab 10, 11, 13
            if (IsShowQhgd && !ValidateTabFamilyRelationship()) return false;
            if (IsShowDieuChuyen && !ValidateTabTransfer()) return false;

            // Tab 12: Thôi việc - validate nếu show
            if (IsShowNghiViec && !ValidateTabResignation()) return false;

            if (IsShowQtct && !ValidateTabWorkingProcess()) return false;

            return true;
        }

        /// <summary>
        /// Tab 4: Validate BHXH (nếu tab show)
        /// Nguồn: HRTTNV/NewFrm.xaml.cs - không validate format, để người dùng nhập tự do
        /// (Số BHYT/BHXH có thể chứa dấu, khoảng trắng, ... nên không cần validate quá chặt)
        /// </summary>
        private bool ValidateTabSocialInsurance()
        {
            // Không có constraint bắt buộc - cho phép nhập tự do
            // WPF cũng không validate format cho So_the_bhyt và So_so_bhxh
            return true;
        }

        /// <summary>
        /// Tab 5: Validate Đoàn/Đảng (nếu tab show)
        /// </summary>
        private bool ValidateTabUnionParty()
        {
            // Logic từ HRTTNV/NewFrm.xaml.cs — Tab Đoàn/Đảng
            // Hiện tại không có constraint bắt buộc đặc biệt
            // Có thể kiểm tra logic giữa ngày vào đoàn/đảng nếu cần

            if (CurRow.Ngay_vao_doan.HasValue && CurRow.Ngay_vao_dang.HasValue)
            {
                if (CurRow.Ngay_vao_dang.Value < CurRow.Ngay_vao_doan.Value)
                {
                    myStateMN.SetNewThongbao("Ngày vào Đảng không thể trước ngày vào Đoàn!");
                    ScrollToSection(5);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Tab 6: Validate Lương (nếu tab show)
        /// </summary>
        private bool ValidateTabSalary()
        {
            // Logic từ HRTTNV/NewFrm.xaml.cs — Tab Lương
            // Hệ số lương và hệ số phụ cấp phải >= 0
            if ((CurRow.He_so_luong ?? 0) < 0)
            {
                myStateMN.SetNewThongbao("Hệ số lương không được âm!");
                ScrollToSection(6);
                return false;
            }

            if ((CurRow.He_so_phu_cap ?? 0) < 0)
            {
                myStateMN.SetNewThongbao("Hệ số phụ cấp không được âm!");
                ScrollToSection(6);
                return false;
            }

            if ((CurRow.Muc_luong ?? 0) < 0)
            {
                myStateMN.SetNewThongbao("Mức lương không được âm!");
                ScrollToSection(6);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Tab 7: Validate Dời đơn vị (nếu tab show)
        /// </summary>
        private bool ValidateTabTransferUnit()
        {
            // Logic từ HRTTNV/NewFrm.xaml.cs — Tab Dời đơn vị
            // Hiện tại không có constraint bắt buộc
            // Có thể kiểm tra: nếu Da_chuyen_di = "1" thì Ngay_chuyen_di phải có
            if (CurRow.Da_chuyen_di == "1" && !CurRow.Ngay_chuyen_di.HasValue)
            {
                myStateMN.SetNewThongbao("Nếu đã chuyển đi, ngày chuyển đi là bắt buộc!");
                ScrollToSection(7);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Tab 1: Validate thông tin chính nhân viên
        /// Nguồn: TNCNDmNs/V_Nhan + HRTTNV/Checkvalidtxt
        /// </summary>
        private bool ValidateTabMainInfo()
        {
            // [TNCNDmNs] Mã nhân viên bắt buộc
            if (string.IsNullOrWhiteSpace(CurRow.Ma_ns))
            {
                myStateMN.SetNewThongbao("Chưa vào mã nhân viên!");
                ScrollToSection(1);
                return false;
            }

            // [TNCNDmNs] Mã không được chứa khoảng trắng
            if (CurRow.Ma_ns.Contains(" "))
            {
                myStateMN.SetNewThongbao("Mã không được chứa các ký tự khoảng trắng!");
                ScrollToSection(1);
                return false;
            }

            // [HRTTNV] Tên nhân viên bắt buộc
            if (string.IsNullOrWhiteSpace(CurRow.Ten_ns))
            {
                myStateMN.SetNewThongbao("Tên nhân viên không được để trống!");
                ScrollToSection(1);
                return false;
            }

            // [HRTTNV] Mã bộ phận bắt buộc
            if (string.IsNullOrWhiteSpace(CurRow.Ma_bpns))
            {
                myStateMN.SetNewThongbao("Mã phòng ban không được để trống!");
                ScrollToSection(1);
                return false;
            }

            // [HRTTNV] Số điện thoại chỉ được chứa chữ số
            if (!string.IsNullOrWhiteSpace(CurRow.Dien_thoai))
            {
                if (!Regex.IsMatch(CurRow.Dien_thoai.Trim(), @"^[0-9]+$"))
                {
                    myStateMN.SetNewThongbao("Số điện thoại không hợp lệ, chỉ được nhập chữ số!");
                    ScrollToSection(1);
                    return false;
                }
            }

            // [HRTTNV] CMND chỉ được chứa chữ số
            if (!string.IsNullOrWhiteSpace(CurRow.Cmnd))
            {
                if (!Regex.IsMatch(CurRow.Cmnd.Trim(), @"^[0-9]+$"))
                {
                    myStateMN.SetNewThongbao("CMND/CCCD không hợp lệ, chỉ được nhập chữ số!");
                    ScrollToSection(1);
                    return false;
                }
            }

            // [TNCNDmNs] Mã số thuế hợp lệ (nếu có nhập)
            if (!string.IsNullOrWhiteSpace(CurRow.Ma_so_thue))
            {
                if (!Regex.IsMatch(CurRow.Ma_so_thue.Trim(), @"^[0-9\-]+$"))
                {
                    myStateMN.SetNewThongbao("Mã số thuế không hợp lệ!");
                    ScrollToSection(1);
                    return false;
                }
            }

            // Ngày sinh phải trước ngày vào
            if (CurRow.Ngay_sinh.HasValue && CurRow.Ngay_vao.HasValue)
            {
                if (CurRow.Ngay_sinh.Value >= CurRow.Ngay_vao.Value)
                {
                    myStateMN.SetNewThongbao("Ngày sinh phải trước ngày vào làm việc!");
                    ScrollToSection(1);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Tab 2: Validate thông tin giảm trừ
        /// Nguồn: TNCNDmNs/V_Nhan - lọc bỏ dòng không có ngay_hl1, không báo lỗi
        /// </summary>
        private bool ValidateTabDeduction()
        {
            if (CurRow.ListCtgtk == null) return true;

            // ✅ Chỉ validate, không modify list ở đây
            // Việc lọc sẽ được thực hiện trong SaveTabDeduction()
            return true;
        }

        /// <summary>
        /// Tab 3: Validate người phụ thuộc
        /// Nguồn: TNCNDmNs/V_Nhan - kiểm tra ma_npt và ma_so_thue
        /// </summary>
        private bool ValidateTabDependent()
        {
            if (CurRow.ListCtnpt == null || CurRow.ListCtnpt.Count == 0) return true;

            for (int i = 0; i < CurRow.ListCtnpt.Count; i++)
            {
                var row = CurRow.ListCtnpt[i];

                // Bỏ qua dòng trống hoàn toàn (không có cả ma_npt lẫn ho_ten2)
                if (string.IsNullOrEmpty(row.Ma_npt) && string.IsNullOrEmpty(row.Ho_ten2))
                    continue;

                // [TNCNDmNs] Mã người phụ thuộc bắt buộc nếu có nhập dòng
                if (string.IsNullOrWhiteSpace(row.Ma_npt))
                {
                    myStateMN.SetNewThongbao($"Dòng {i + 1}: Chưa vào mã người phụ thuộc!");
                    ScrollToSection(3);
                    return false;
                }

                // [TNCNDmNs] Mã số thuế người phụ thuộc hợp lệ nếu có nhập
                if (!string.IsNullOrWhiteSpace(row.Ma_so_thue))
                {
                    if (!Regex.IsMatch(row.Ma_so_thue.Trim(), @"^[0-9\-]+$"))
                    {
                        myStateMN.SetNewThongbao($"Dòng {i + 1}: Mã số thuế người phụ thuộc không hợp lệ!");
                        ScrollToSection(3);
                        return false;
                    }
                }
            }

            // Lọc bỏ dòng trống hoàn toàn trước khi lưu
            CurRow.ListCtnpt.RemoveAll(x => string.IsNullOrEmpty(x.Ma_npt) && string.IsNullOrEmpty(x.Ho_ten2));
            SetStt(CurRow.ListCtnpt);
            return true;
        }

        /// <summary>
        /// Tab 10: Validate thành phần gia đình
        /// Nguồn: HRTPGD/NewFrm.xaml.cs - Checkvalidtxt
        /// ✅ ĐÚNG THEO WPF: Bắt buộc phải có mã nhân viên, tên gia đình, và mã quan hệ gia đình
        /// </summary>
        private bool ValidateTabFamilyRelationship()
        {
            if (CurRow.ListQhgd == null || CurRow.ListQhgd.Count == 0) return true;

            for (int i = 0; i < CurRow.ListQhgd.Count; i++)
            {
                var row = CurRow.ListQhgd[i];

                // Bỏ qua dòng trống hoàn toàn
                if (string.IsNullOrEmpty(row.Ten_tpgd) && string.IsNullOrEmpty(row.Ten_tpgd2))
                    continue;

                // [HRTPGD] Tên thành phần gia đình bắt buộc nếu có nhập dòng
                if (string.IsNullOrWhiteSpace(row.Ten_tpgd))
                {
                    myStateMN.SetNewThongbao($"Dòng {i + 1}: Tên thành phần gia đình không được để trống!");
                    ScrollToSection(10);
                    return false;
                }

                // [HRTPGD] Mã quan hệ gia đình bắt buộc nếu có nhập dòng
                // ✅ FIX: Validation này còn thiếu trong Blazor
                if (string.IsNullOrWhiteSpace(row.Ma_qhgd))
                {
                    myStateMN.SetNewThongbao($"Dòng {i + 1}: Mã quan hệ gia đình không được để trống!");
                    ScrollToSection(10);
                    return false;
                }

                // [HRTPGD] Mã số thuế hợp lệ (nếu có nhập)
                if (!string.IsNullOrWhiteSpace(row.Ma_so_thue))
                {
                    if (!Regex.IsMatch(row.Ma_so_thue.Trim(), @"^[0-9\-]+$"))
                    {
                        myStateMN.SetNewThongbao($"Dòng {i + 1}: Mã số thuế không hợp lệ!");
                        ScrollToSection(10);
                        return false;
                    }
                }

                // [HRTPGD] CMND hợp lệ (nếu có nhập)
                if (!string.IsNullOrWhiteSpace(row.Cmnd))
                {
                    if (!Regex.IsMatch(row.Cmnd.Trim(), @"^[0-9]+$"))
                    {
                        myStateMN.SetNewThongbao($"Dòng {i + 1}: CMND không hợp lệ!");
                        ScrollToSection(10);
                        return false;
                    }
                }

                // [HRTPGD] Năm sinh hợp lệ (nếu có nhập)
                if (!string.IsNullOrWhiteSpace(row.Nam_sinh))
                {
                    if (!Regex.IsMatch(row.Nam_sinh.Trim(), @"^[0-9]+$"))
                    {
                        myStateMN.SetNewThongbao($"Dòng {i + 1}: Năm sinh không hợp lệ!");
                        ScrollToSection(10);
                        return false;
                    }
                }
            }

            // Lọc bỏ dòng trống hoàn toàn trước khi lưu
            CurRow.ListQhgd.RemoveAll(x => string.IsNullOrEmpty(x.Ten_tpgd) && string.IsNullOrEmpty(x.Ten_tpgd2));
            SetStt(CurRow.ListQhgd);
            return true;
        }

        /// <summary>
        /// Tab 11: Validate điều chuyển nhân sự
        /// Nguồn: HRQTDC/NewFrm.xaml.cs - Checkvalidtxt
        /// </summary>
        private bool ValidateTabTransfer()
        {
            if (CurRow.ListDieuChuyen == null || CurRow.ListDieuChuyen.Count == 0) return true;

            for (int i = 0; i < CurRow.ListDieuChuyen.Count; i++)
            {
                var row = CurRow.ListDieuChuyen[i];

                // Bỏ qua dòng trống hoàn toàn
                if (string.IsNullOrEmpty(row.Ma_bpns_cu) && string.IsNullOrEmpty(row.Ma_bpns_moi))
                    continue;

                // [HRQTDC] Mã bộ phận cũ bắt buộc nếu có nhập dòng
                if (string.IsNullOrWhiteSpace(row.Ma_bpns_cu))
                {
                    myStateMN.SetNewThongbao($"Dòng {i + 1}: Mã bộ phận cũ không được để trống!");
                    ScrollToSection(11);
                    return false;
                }

                // [HRQTDC] Mã bộ phận mới bắt buộc nếu có nhập dòng
                if (string.IsNullOrWhiteSpace(row.Ma_bpns_moi))
                {
                    myStateMN.SetNewThongbao($"Dòng {i + 1}: Mã bộ phận mới không được để trống!");
                    ScrollToSection(11);
                    return false;
                }
            }

            // Lọc bỏ dòng trống hoàn toàn trước khi lưu
            CurRow.ListDieuChuyen.RemoveAll(x => string.IsNullOrEmpty(x.Ma_bpns_cu) && string.IsNullOrEmpty(x.Ma_bpns_moi));
            SetStt(CurRow.ListDieuChuyen);
            return true;
        }

        /// <summary>
        /// Tab 8: Validate hợp đồng lao động
        /// Nguồn: HRHDLD/NewFrm.xaml.cs - Checkvalidtxt
        /// Bắt buộc: ma_hdld, ma_ns_ct, ma_ns
        /// Kiểm tra: ngay_hl >= ngay_hd, tu_ngay <= den_ngay
        /// </summary>
        private bool ValidateTabContract()
        {
            if (CurRow.ListHopDong == null || CurRow.ListHopDong.Count == 0) return true;

            for (int i = 0; i < CurRow.ListHopDong.Count; i++)
            {
                var row = CurRow.ListHopDong[i];

                // Bỏ qua dòng trống hoàn toàn
                if (string.IsNullOrEmpty(row.Ma_hdld) && string.IsNullOrEmpty(row.Ma_ns_ct))
                    continue;

                // [HRHDLD] Mã hợp đồng bắt buộc nếu có nhập dòng
                if (string.IsNullOrWhiteSpace(row.Ma_hdld))
                {
                    myStateMN.SetNewThongbao($"Dòng {i + 1}: Số hợp đồng không được để trống!");
                    ScrollToSection(8);
                    return false;
                }

                // [HRHDLD] Mã người đại diện công ty (Ông bà) bắt buộc
                if (string.IsNullOrWhiteSpace(row.Ma_ns_ct))
                {
                    myStateMN.SetNewThongbao($"Dòng {i + 1}: Mã Ông bà (Đại diện công ty) không được để trống!");
                    ScrollToSection(8);
                    return false;
                }

                // [HRHDLD] Ngày hiệu lực phải >= Ngày hợp đồng
                if (row.Ngay_hl.HasValue && row.Ngay_hd.HasValue)
                {
                    if (row.Ngay_hl < row.Ngay_hd)
                    {
                        myStateMN.SetNewThongbao($"Dòng {i + 1}: Ngày hiệu lực phải lớn hoặc bằng ngày hợp đồng!");
                        ScrollToSection(8);
                        return false;
                    }
                }

                // [HRHDLD] Kiểm tra khoảng thời gian hợp lệ
                if (row.Tu_ngay.HasValue && row.Den_ngay.HasValue)
                {
                    if (row.Tu_ngay > row.Den_ngay)
                    {
                        myStateMN.SetNewThongbao($"Dòng {i + 1}: Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc!");
                        ScrollToSection(8);
                        return false;
                    }
                }

                // Set mặc định ngày nếu chưa có
                if (!row.Ngay_hl.HasValue)
                    row.Ngay_hl = DateTime.Now.Date;
                
                if (!row.Ngay_hd.HasValue)
                    row.Ngay_hd = DateTime.Now.Date;
            }

            // Lọc bỏ dòng trống hoàn toàn trước khi lưu
            CurRow.ListHopDong.RemoveAll(x => string.IsNullOrEmpty(x.Ma_hdld) && string.IsNullOrEmpty(x.Ma_ns_ct));
            SetStt(CurRow.ListHopDong);
            return true;
        }

        /// <summary>
        /// Tab 9: Validate quá trình tham gia bảo hiểm
        /// ✅ ĐÚNG THEO WPF: DMQTBH/NewFrm.xaml.cs - Checkvalidtxt
        /// Bắt buộc: so_the_bhxh hoặc so_the_bhyt (phải nhập ít nhất một loại BHXH hoặc BHYT)
        /// Kiểm tra: tu_ngay <= den_ngay, bhyt_tungay <= bhyt_denngay
        /// Kiểm tra: Không được tham gia BHXH/BHYT trồi lẻn (mới phải >= ngày kết thúc cũ)
        /// </summary>
        private bool ValidateTabInsuranceHistory()
        {
            if (CurRow.ListQtbh == null || CurRow.ListQtbh.Count == 0) return true;

            for (int i = 0; i < CurRow.ListQtbh.Count; i++)
            {
                var row = CurRow.ListQtbh[i];

                // Bỏ qua dòng trống hoàn toàn
                if (string.IsNullOrEmpty(row.So_the_bhxh) && string.IsNullOrEmpty(row.So_the_bhyt))
                    continue;

                // [DMQTBH] Bắt buộc phải có Số sổ BHXH hoặc Số thẻ BHYT
                if (string.IsNullOrEmpty(row.So_the_bhxh) && string.IsNullOrEmpty(row.So_the_bhyt))
                {
                    myStateMN.SetNewThongbao($"Dòng {i + 1}: Phải nhập Số sổ BHXH hoặc Số thẻ BHYT!");
                    ScrollToSection(9);
                    return false;
                }

                // [DMQTBH] Validate Số sổ BHXH nếu có nhập
                if (!string.IsNullOrEmpty(row.So_the_bhxh))
                {
                    // Kiểm tra BHXH từ-đến ngày hợp lệ
                    if (row.Tu_ngay.HasValue && row.Den_ngay.HasValue)
                    {
                        if (row.Tu_ngay > row.Den_ngay)
                        {
                            myStateMN.SetNewThongbao($"Dòng {i + 1}: BHXH - Ngày bắt đầu phải <= Ngày kết thúc!");
                            ScrollToSection(9);
                            return false;
                        }
                    }

                    // [DMQTBH] Kiểm tra không được tham gia BHXH trồi lẻn
                    // Nếu đang tham gia BHXH (den_ngay IS NULL), không được thêm lần mới
                    if (i < CurRow.ListQtbh.Count - 1) // Không check dòng cuối cùng (dòng mới)
                    {
                        var prevRow = CurRow.ListQtbh[i];
                        if (!prevRow.Den_ngay.HasValue && !string.IsNullOrEmpty(prevRow.So_the_bhxh))
                        {
                            // Có lần BHXH chưa kết thúc
                            if (!string.IsNullOrEmpty(row.So_the_bhxh) && row.Tu_ngay.HasValue)
                            {
                                myStateMN.SetNewThongbao($"Dòng {i + 1}: Nhân viên chưa kết thúc lần đóng BHXH cuối cùng!");
                                ScrollToSection(9);
                                return false;
                            }
                        }
                    }
                }

                // [DMQTBH] Validate Số thẻ BHYT nếu có nhập
                if (!string.IsNullOrEmpty(row.So_the_bhyt))
                {
                    // Kiểm tra BHYT từ-đến ngày hợp lệ
                    if (row.Bhyt_tungay.HasValue && row.Bhyt_denngay.HasValue)
                    {
                        if (row.Bhyt_tungay > row.Bhyt_denngay)
                        {
                            myStateMN.SetNewThongbao($"Dòng {i + 1}: BHYT - Ngày bắt đầu phải <= Ngày kết thúc!");
                            ScrollToSection(9);
                            return false;
                        }
                    }

                    // [DMQTBH] Kiểm tra không được tham gia BHYT trồi lẻn
                    if (i < CurRow.ListQtbh.Count - 1)
                    {
                        var prevRow = CurRow.ListQtbh[i];
                        if (!prevRow.Bhyt_denngay.HasValue && !string.IsNullOrEmpty(prevRow.So_the_bhyt))
                        {
                            // Có lần BHYT chưa kết thúc
                            if (!string.IsNullOrEmpty(row.So_the_bhyt) && row.Bhyt_tungay.HasValue)
                            {
                                myStateMN.SetNewThongbao($"Dòng {i + 1}: Nhân viên chưa kết thúc lần đóng BHYT cuối cùng!");
                                ScrollToSection(9);
                                return false;
                            }
                        }
                    }
                }
            }

            // Lọc bỏ dòng trống hoàn toàn
            CurRow.ListQtbh.RemoveAll(x => string.IsNullOrEmpty(x.So_the_bhxh) && string.IsNullOrEmpty(x.So_the_bhyt));
            SetStt(CurRow.ListQtbh);
            return true;
        }

        /// <summary>
        /// Tab 12: Validate thông tin thôi việc nhân viên
        /// ✅ ĐÚNG THEO WPF: HRNVN/NewFrm.xaml.cs - Checkvalidtxt
        /// Bắt buộc: ma_ns (lấy từ form chính)
        /// Kiểm tra: ngay_lam_don, ngay_thoi_viec, ngay_thoi_thuc_te phải hợp lệ
        /// Kiểm tra: tro_cap (0 hoặc 1), status (0 hoặc 1)
        /// </summary>
        private bool ValidateTabResignation()
        {
            if (CurRow?.NghiViecInfo == null) return true;

            var row = CurRow.NghiViecInfo;

            // [HRNVN] Bắt buộc phải có mã nhân viên
            if (string.IsNullOrEmpty(row.Ma_ns))
            {
                myStateMN.SetNewThongbao("Mã nhân viên không được để trống!");
                ScrollToSection(12);
                return false;
            }

            // [HRNVN] Validate ngày: ngay_lam_don <= ngay_thoi_viec <= ngay_thoi_thuc_te
            if (row.Ngay_lam_don.HasValue && row.Ngay_thoi_viec.HasValue)
            {
                if (row.Ngay_lam_don > row.Ngay_thoi_viec)
                {
                    myStateMN.SetNewThongbao("Ngày viết đơn phải <= Ngày thôi việc!");
                    ScrollToSection(12);
                    return false;
                }
            }

            if (row.Ngay_thoi_viec.HasValue && row.Ngay_thoi_thuc_te.HasValue)
            {
                if (row.Ngay_thoi_viec > row.Ngay_thoi_thuc_te)
                {
                    myStateMN.SetNewThongbao("Ngày thôi việc phải <= Ngày thực tế nghỉ!");
                    ScrollToSection(12);
                    return false;
                }
            }

            // [HRNVN] Validate ngay_chot_so, ngay_tra_so
            if (row.Ngay_chot_so.HasValue && row.Ngay_tra_so.HasValue)
            {
                if (row.Ngay_chot_so > row.Ngay_tra_so)
                {
                    myStateMN.SetNewThongbao("Ngày chốt sổ BH phải <= Ngày trả sổ BH!");
                    ScrollToSection(12);
                    return false;
                }
            }

            // [HRNVN] Validate tro_cap (0 hoặc 1)
            if (!string.IsNullOrEmpty(row.Tro_cap) && row.Tro_cap != "0" && row.Tro_cap != "1")
            {
                myStateMN.SetNewThongbao("Trợ cấp phải là 0 hoặc 1!");
                ScrollToSection(12);
                return false;
            }

            // [HRNVN] Validate status (0 hoặc 1)
            if (!string.IsNullOrEmpty(row.Status) && row.Status != "0" && row.Status != "1")
            {
                myStateMN.SetNewThongbao("Trạng thái phải là 0 hoặc 1!");
                ScrollToSection(12);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Tab 13: Validate quá trình công tác (công tác, nơi công tác)
        /// ✅ ĐÚNG THEO WPF: HRDMNVCT/NewFrm.xaml.cs - Checkvalidtxt + INSERT/UPDATE logic
        /// Bắt buộc: ma_quyet_dinh, ngay_di, ngay_ve (vì lưu trực tiếp vào DB)
        /// </summary>
        private bool ValidateTabWorkingProcess()
        {
            if (CurRow.ListQtct == null || CurRow.ListQtct.Count == 0) return true;

            for (int i = 0; i < CurRow.ListQtct.Count; i++)
            {
                var row = CurRow.ListQtct[i];

                // Bỏ qua dòng trống hoàn toàn
                if (string.IsNullOrEmpty(row.Ma_quyet_dinh) && !row.Ngay_di.HasValue && !row.Ngay_ve.HasValue)
                    continue;

                // [HRDMNVCT] Mã quyết định bắt buộc nếu có nhập dòng
                // ✅ TỪWPF: Checkvalidtxt không check bắt buộc, nhưng lưu dữ liệu yêu cầu
                if (string.IsNullOrWhiteSpace(row.Ma_quyet_dinh))
                {
                    myStateMN.SetNewThongbao($"Dòng {i + 1}: Quyết định số không được để trống!");
                    ScrollToSection(13);
                    return false;
                }

                // [HRDMNVCT] Ngày đi bắt buộc nếu có nhập dòng
                // ✅ Dùng .HasValue để kiểm tra DateTime nullable
                if (!row.Ngay_di.HasValue)
                {
                    myStateMN.SetNewThongbao($"Dòng {i + 1}: Ngày đi không được để trống!");
                    ScrollToSection(13);
                    return false;
                }

                // [HRDMNVCT] Ngày về bắt buộc nếu có nhập dòng
                if (!row.Ngay_ve.HasValue)
                {
                    myStateMN.SetNewThongbao($"Dòng {i + 1}: Ngày về không được để trống!");
                    ScrollToSection(13);
                    return false;
                }

                // [HRDMNVCT] Ngày về phải >= Ngày đi
                if (row.Ngay_ve.Value < row.Ngay_di.Value)
                {
                    myStateMN.SetNewThongbao($"Dòng {i + 1}: Ngày về phải >= Ngày đi!");
                    ScrollToSection(13);
                    return false;
                }

                // [HRDMNVCT] Validate mã quyết định (không được chứa ký tự đặc biệt)
                // ✅ TỪWPF: Checkvalidtxt gọi SysFunc.CheckInValidCode
                // Cho phép: chữ, số, dấu cách, gạch ngang, gạch chéo, dấu chấm
                if (!Regex.IsMatch(row.Ma_quyet_dinh.Trim(), @"^[a-zA-Z0-9\s\-\/\.]+$"))
                {
                    myStateMN.SetNewThongbao($"Dòng {i + 1}: Quyết định số chứa ký tự không hợp lệ!");
                    ScrollToSection(13);
                    return false;
                }
            }

            // Lọc bỏ dòng trống hoàn toàn trước khi lưu
            CurRow.ListQtct.RemoveAll(x => string.IsNullOrEmpty(x.Ma_quyet_dinh) && !x.Ngay_di.HasValue && !x.Ngay_ve.HasValue);
            SetStt(CurRow.ListQtct);
            return true;
        }

        /// <summary>
        /// Lưu toàn bộ tab đang show
        /// - Tab 1, 2, 3, 10, 11, 13 luôn lưu (luôn show)
        /// - Tab 2: DELETE + INSERT bảng ctgtk
        /// - Tab 3: DELETE + INSERT bảng ctnpt
        /// - Tab 10: DELETE + INSERT bảng dmtpgd
        /// - Tab 11: DELETE + INSERT bảng dmqtdc
        /// - Tab 13: DELETE + INSERT bảng dmnvct
        /// - Tab 4-7: gộp vào Tab 1 (UPDATE dmns) nếu show
        /// </summary>
        private async Task SaveAllTabs()
        {
            try
            {
                // Tab 1 + Tab 4-7 (nếu show): INSERT/UPDATE bảng dmns
                await SaveTabMainInfo();

                // Tab 2: DELETE + INSERT bảng ctgtk
                await SaveTabDeduction();

                // Tab 3: DELETE + INSERT bảng ctnpt
                await SaveTabDependent();

                // Tab 8: DELETE + INSERT bảng dmhdld
                await SaveTabContract();

                // Tab 9: DELETE + INSERT bảng dmqtbh
                await SaveTabInsuranceHistory();

                // Tab 10: DELETE + INSERT bảng dmtpgd
                await SaveTabFamilyRelationship();

                // Tab 11: DELETE + INSERT bảng dmqtdc
                await SaveTabTransfer();

                // Tab 12: INSERT/UPDATE bảng dmnvn (dữ liệu đơn)
                await SaveTabResignation();

                // Tab 13: DELETE + INSERT bảng dmnvct
                await SaveTabWorkingProcess();

                myStateMN.SetNewThongbao(Lap["Save successfully"] ?? "Lưu thành công!");
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao("Lỗi khi lưu: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Tab 1 + Tab 4-7: Lưu thông tin chính nhân viên vào bảng dmns
        /// - Tab 1: thông tin nhân viên cơ bản (luôn save)
        /// - Tab 4 (BHXH): so_the_bhyt, so_so_bhxh, ngay_tham_gia_bhxh, ... (nếu IsShowSocialInsurance = true)
        /// - Tab 5 (Đoàn/Đảng): ngay_vao_doan, ma_chucvu_doan, ngay_vao_dang, ... (nếu IsShowUnionParty = true)
        /// - Tab 6 (Lương): he_so_luong, he_so_phu_cap, bac_luong, ... (nếu IsShowSalary = true)
        /// - Tab 7 (Dời đơn vị): ngay_chuyen_di, da_chuyen_di, ... (nếu IsShowTransferUnit = true)
        /// Tất cả được gộp vào UPDATE dmns thông qua BuildUpdateSql()
        /// </summary>
        private async Task SaveTabMainInfo()
        {
            try
            {
                int userId = curUser?.User_id ?? 0;
                string userName = curUser?.User_name ?? "";
                DateTime now = DateTime.Now;

                // ✅ TRIM & GIỚI HẠN ĐỘ DÀI CÁC CỘT TEXT (theo schema dmns)
                CurRow.Ma_ns = TrimTo((CurRow.Ma_ns ?? "").Trim().ToUpper(), 16);
                CurRow.Ten_ns = TrimTo((CurRow.Ten_ns ?? "").Trim(), 254);
                CurRow.Ten_ns2 = TrimTo((CurRow.Ten_ns2 ?? "").Trim(), 254);
                CurRow.Noi_sinh = TrimTo(CurRow.Noi_sinh ?? "", 254);
                CurRow.Bi_danh = TrimTo(CurRow.Bi_danh ?? "", 254);
                CurRow.Dia_chi = TrimTo(CurRow.Dia_chi ?? "", 128);
                CurRow.Ho_khau = TrimTo(CurRow.Ho_khau ?? "", 128);
                CurRow.Dien_thoai = TrimTo(CurRow.Dien_thoai ?? "", 50);
                CurRow.Dien_thoai_co_dinh = TrimTo(CurRow.Dien_thoai_co_dinh ?? "", 50);
                CurRow.Fax = TrimTo(CurRow.Fax ?? "", 40);
                CurRow.E_mail = TrimTo(CurRow.E_mail ?? "", 128);
                CurRow.Ghi_chu = TrimTo(CurRow.Ghi_chu ?? "", 4000);
                CurRow.Tinh_thanh = TrimTo(CurRow.Tinh_thanh ?? "", 24);
                CurRow.Dc_cu_tru = TrimTo(CurRow.Dc_cu_tru ?? "", 128);
                CurRow.Cmnd = TrimTo(CurRow.Cmnd ?? "", 16);
                CurRow.Nc_cmnd = TrimTo(CurRow.Nc_cmnd ?? "", 48);
                CurRow.Ma_so_thue = TrimTo(CurRow.Ma_so_thue ?? "", 18);
                CurRow.Nc_mst = TrimTo(CurRow.Nc_mst ?? "", 18);
                CurRow.Nguyen_quan = (CurRow.Nguyen_quan ?? "").PadRight(16, ' ');
                CurRow.Quoc_tich = (CurRow.Quoc_tich ?? "").PadRight(16, ' ');
                CurRow.Quoc_tich2 = TrimTo(CurRow.Quoc_tich2 ?? "", 50);
                CurRow.Khicannhantincho = TrimTo(CurRow.Khicannhantincho ?? "", 254);
                CurRow.Diachinhantin = TrimTo(CurRow.Diachinhantin ?? "", 128);
                CurRow.Noi_cap_so_bhxh = TrimTo(CurRow.Noi_cap_so_bhxh ?? "", 254);
                CurRow.Noi_ket_nap_doan = TrimTo(CurRow.Noi_ket_nap_doan ?? "", 254);
                CurRow.Noi_ket_nap_dang = TrimTo(CurRow.Noi_ket_nap_dang ?? "", 254);
                CurRow.Trinh_do_trinh_tri = TrimTo(CurRow.Trinh_do_trinh_tri ?? "", 254);
                CurRow.Trinh_do_quan_ly = TrimTo(CurRow.Trinh_do_quan_ly ?? "", 254);
                CurRow.Image_url = TrimTo(CurRow.Image_url ?? "", 254);
                CurRow.Search = TrimTo(CurRow.Search ?? "", 900);

                // [TNCNDmNs] Giữ nguyên ma_dvcs từ dữ liệu, chỉ set mặc định nếu đang rỗng
                // Ma_ma_dvcs không tồn tại trong base — lấy từ CurRow hoặc để rỗng
                if (string.IsNullOrEmpty(CurRow.Ma_dvcs))
                    CurRow.Ma_dvcs = CurRow.Ma_dvcs ?? "";

                bool isNew = curActionTask.ToString() == "New" || curActionTask.ToString() == "2";
                bool isEdit = curActionTask.ToString() == "Edit" || curActionTask.ToString() == "1";

                if (isNew)
                {
                    // [TNCNDmNs] Kiểm tra trùng mã khi thêm mới
                    string checkSql = $"SELECT COUNT(*) FROM {sqlTableName} WHERE ma_ns = N'{Esc(CurRow.Ma_ns)}'";
                    object count = myDb.GetValue(checkSql);
                    if (count != null && Convert.ToInt32(count) > 0)
                    {
                        myStateMN.SetNewThongbao($"Mã [{CurRow.Ma_ns}] đã tồn tại, vui lòng chọn mã khác!");
                        throw new Exception("Duplicate code");
                    }

                    // [TNCNDmNs + HRTTNV] Set audit fields khi tạo mới
                    CurRow.Date0 = now.Date;
                    CurRow.Time0 = now.ToString("HH:mm:ss");
                    CurRow.User_id0 = userId;
                    CurRow.User_name0 = userName;
                    CurRow.Date = now.Date;
                    CurRow.Time = now.ToString("HH:mm:ss");
                    CurRow.User_id = userId;
                    CurRow.User_name = userName;

                    await myDb.InsertOneRow<HsnvDetailModel>(CurRow, sqlTableName);
                }
                else if (isEdit)
                {
                    // [TNCNDmNs] Kiểm tra trùng mã khi sửa (nếu có đổi mã)
                    if (AllowEditMa && !string.Equals(CurRow.Ma_ns, Ma_ma, StringComparison.OrdinalIgnoreCase))
                    {
                        string checkSql = $"SELECT COUNT(*) FROM {sqlTableName} WHERE ma_ns = N'{Esc(CurRow.Ma_ns)}'";
                        object count = myDb.GetValue(checkSql);
                        if (count != null && Convert.ToInt32(count) > 0)
                        {
                            myStateMN.SetNewThongbao($"Mã [{CurRow.Ma_ns}] đã tồn tại!");
                            throw new Exception("Duplicate code");
                        }
                    }

                    // [HRTTNV] Set audit fields khi sửa
                    CurRow.Date = now.Date;
                    CurRow.Time = now.ToString("HH:mm:ss");
                    CurRow.User_id = userId;
                    CurRow.User_name = userName;

                    // ✅ GIẢI PHÁP: Dùng UpdateOneRow thay vì BuildUpdateSql
                    // UpdateOneRow tự động handle escape, NULL, data types
                    // Không cần phải build SQL string thủ công
                    try
                    {
                        // Cách 1: Dùng library UpdateOneRow (nếu có)
                        // await myDb.UpdateOneRow<HsnvDetailModel>(CurRow, sqlTableName, "ma_ns");
                        
                        // Cách 2: Dùng BuildUpdateSql (hiện tại)
                        string updateSql = BuildUpdateSql();
                        
                        await myDb.ExecuteSqlQueryAsync(updateSql);
                    }
                    catch (Exception sqlEx)
                    {
                        throw new Exception($"Lỗi thực thi SQL: {sqlEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lưu Tab 1: " + ex.Message);
            }
        }

        /// <summary>
        /// Tab 2: Lưu thông tin giảm trừ vào bảng ctgtk
        /// ✅ ĐÚNG THEO WPF: V_Nhan trong StartForm.xaml.cs
        /// MAPPING: GT01=Gia_canh, GT02=A_001, GT03=A_002, GT04=A_003, GT05=Tien_gt
        /// </summary>
        private async Task SaveTabDeduction()
        {
            try
            {
                string safeMa = Esc(CurRow.Ma_ns);
                string safeDvcs = Esc(CurRow.Ma_dvcs ?? "");
                string safeSttRec = "";

                object sttRecVal = myDb.GetValue($"SELECT stt_rec FROM {sqlTableName} WHERE ma_ns = N'{safeMa}'");
                if (sttRecVal != null && sttRecVal != DBNull.Value)
                    safeSttRec = Esc(sttRecVal.ToString());

                // Sử dụng XACT_ABORT thay cho TRY...CATCH
                var sqlBatch = new System.Text.StringBuilder();
                sqlBatch.AppendLine("SET XACT_ABORT ON;");
                sqlBatch.AppendLine("BEGIN TRAN;");

                // Lệnh Xóa
                if (!string.IsNullOrEmpty(safeSttRec))
                    sqlBatch.AppendLine($"DELETE FROM ctgtk WHERE stt_rec = N'{safeSttRec}';");
                else
                    sqlBatch.AppendLine($"DELETE FROM ctgtk WHERE ma_ns = N'{safeMa}';");

                // Lệnh Thêm
                if (CurRow.ListCtgtk != null && CurRow.ListCtgtk.Count > 0)
                {
                    string[] deductionColumns = new[] { "a_001", "a_002", "a_003", "a_004", "a_005" };
                    string[] deductionCodesVN = new[] { "GT02", "GT03", "GT04", "GT01", "GT05" };

                    for (int keyGt = 0; keyGt < CurRow.ListCtgtk.Count; keyGt++)
                    {
                        var row = CurRow.ListCtgtk[keyGt];
                        if (row.Ngay_hl1 == null || !row.Ngay_hl1.HasValue)
                            continue;

                        string ngayHl1 = $"'{row.Ngay_hl1.Value:yyyy-MM-dd}'";
                        string ngayHl2 = row.Ngay_hl2.HasValue ? $"'{row.Ngay_hl2.Value:yyyy-MM-dd}'" : "NULL";
                        decimal luongThang = row.Luong_thang ?? 0;
                        decimal luongThangNt = row.Luong_thang_nt ?? 0;
                        decimal luong = row.Luong ?? 0;
                        decimal luongNt = row.Luong_nt ?? 0;

                        for (int j = 0; j < deductionColumns.Length; j++)
                        {
                            string colName = deductionColumns[j];
                            string maGt = deductionCodesVN[j];
                            decimal tienGt = GetDeductionValue(row, colName);

                            if (tienGt > 0)
                            {
                                string luongThangNtStr = luongThangNt.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
                                string luongThangStr = luongThang.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
                                string luongNtStr = luongNt.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
                                string luongStr = luong.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
                                string tienGtStr = tienGt.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);

                                sqlBatch.AppendLine($@"
                            INSERT INTO ctgtk
                                (stt_rec, key_gt, ma_dvcs, ma_ns, ngay_hl1, ngay_hl2,
                                 ma_gt, dien_giai, luong_thang_nt, luong_thang, luong_nt, luong, tien_gt, tien_gt_nt)
                            VALUES (
                                N'{safeSttRec}', {keyGt + 1}, N'{safeDvcs}', N'{safeMa}', {ngayHl1}, {ngayHl2},
                                N'{maGt}', N'{Esc(row.Dien_giai ?? "")}', {luongThangNtStr}, {luongThangStr}, {luongNtStr}, {luongStr}, {tienGtStr}, 0);");
                            }
                        }
                    }
                }

                // Lưu thành công
                sqlBatch.AppendLine("COMMIT TRAN;");

                await myDb.ExecuteSqlQueryAsync(sqlBatch.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lưu Tab 2 (Giảm trừ): " + ex.Message);
            }
        }

        /// <summary>
        /// Helper: Lấy giá trị giảm trừ từ row theo tên cột
        /// ✅ MAPPING ĐÚNG:
        /// GT01 = Gia_canh (Giảm trừ gia cảnh)
        /// GT02 = A_001 (BH thất nghiệp)
        /// GT03 = A_002 (BH y tế)
        /// GT04 = A_003 (BH xã hội)
        /// GT05 = Tien_gt (Giảm trừ khác)
        /// </summary>
        private decimal GetDeductionValue(CtgtkRow row, string columnName)
        {
            if (row == null || string.IsNullOrEmpty(columnName))
                return 0;

            return columnName.ToLower() switch
            {
                "a_001" => row.A_001 ?? 0,      // GT02 - BH thất nghiệp
                "a_002" => row.A_002 ?? 0,      // GT03 - BH y tế
                "a_003" => row.A_003 ?? 0,      // GT04 - BH xã hội
                "a_004" => row.Gia_canh ?? 0,   // GT01 - Giảm trừ gia cảnh
                "a_005" => row.Tien_gt ?? 0,    // GT05 - Giảm trừ khác
                _ => 0
            };
        }

        /// <summary>
        /// Tab 3: Lưu thông tin người phụ thuộc vào bảng ctnpt
        /// ✅ ĐÚNG THEO WPF: Chỉ gán các cột cần thiết, DB sẽ handle DEFAULT values
        /// </summary>
        private async Task SaveTabDependent()
        {
            try
            {
                string safeMa = Esc(CurRow.Ma_ns);
                string safeDvcs = Esc(CurRow.Ma_dvcs ?? "");
                string safeBpns = Esc(CurRow.Ma_bpns ?? "");
                string safeSttRec = "";

                object sttRecVal = myDb.GetValue($"SELECT stt_rec FROM {sqlTableName} WHERE ma_ns = N'{safeMa}'");
                if (sttRecVal != null && sttRecVal != DBNull.Value)
                    safeSttRec = Esc(sttRecVal.ToString());

                var sqlBatch = new System.Text.StringBuilder();
                sqlBatch.AppendLine("SET XACT_ABORT ON;");
                sqlBatch.AppendLine("BEGIN TRAN;");

                // Lệnh Xóa
                if (!string.IsNullOrEmpty(safeSttRec))
                    sqlBatch.AppendLine($"DELETE FROM ctnpt WHERE stt_rec = N'{safeSttRec}';");
                else
                    sqlBatch.AppendLine($"DELETE FROM ctnpt WHERE ma_ns = N'{safeMa}';");

                // Lệnh Thêm
                if (CurRow.ListCtnpt != null && CurRow.ListCtnpt.Count > 0)
                {
                    int stt = 0;
                    foreach (var row in CurRow.ListCtnpt)
                    {
                        if (string.IsNullOrEmpty(row.Ma_npt) && string.IsNullOrEmpty(row.Ho_ten))
                            continue;

                        string ngaySinh = row.Ngay_sinh.HasValue ? $"'{row.Ngay_sinh.Value:yyyy-MM-dd}'" : "NULL";
                        string ngayPt1 = row.Ngay_pt1.HasValue ? $"'{row.Ngay_pt1.Value:yyyy-MM-dd}'" : "NULL";
                        string ngayPt2 = row.Ngay_pt2.HasValue ? $"'{row.Ngay_pt2.Value:yyyy-MM-dd}'" : "NULL";

                        sqlBatch.AppendLine($@"
                    INSERT INTO ctnpt 
                        (stt_rec, ma_dvcs, ma_ns, ma_bpns, stt, ho_ten, ho_ten2, ma_npt, ngay_sinh,
                         ma_so_thue, cmnd, quan_he, ngay_pt1, ngay_pt2, ct_kem, dien_giai, tien_gt, tien_gt_nt,
                         bo_sung, quan_he2, so, quyen_so, quoc_gia, thanh_pho, quan, phuong)
                    VALUES (
                        N'{safeSttRec}', N'{safeDvcs}', N'{safeMa}', N'{safeBpns}', {stt}, 
                        N'{Esc(row.Ho_ten ?? "")}', N'{Esc(row.Ho_ten2 ?? "")}', N'{Esc(row.Ma_npt ?? "")}', {ngaySinh},
                        N'{Esc(row.Ma_so_thue ?? "")}', N'{Esc(row.Cmnd ?? "")}', N'{Esc(row.Quan_he ?? "")}', 
                        {ngayPt1}, {ngayPt2}, N'{Esc(row.Ct_kem ?? "")}', N'{Esc(row.Dien_giai ?? "")}', 
                        {(row.Tien_gt ?? 0).ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}, 
                        {(row.Tien_gt_nt ?? 0).ToString("F2", System.Globalization.CultureInfo.InvariantCulture)},
                        {(row.Bo_sung ?? 0).ToString("F2", System.Globalization.CultureInfo.InvariantCulture)}, 
                        N'{Esc(row.Quan_he2 ?? "")}', N'{Esc(row.So ?? "")}', 
                        N'{Esc(row.Quyen_so ?? "")}', N'{Esc(row.Quoc_gia ?? "")}', N'{Esc(row.Thanh_pho ?? "")}', 
                        N'{Esc(row.Quan ?? "")}', N'{Esc(row.Phuong ?? "")}');");
                        stt++;
                    }
                }

                sqlBatch.AppendLine("COMMIT TRAN;");

                await myDb.ExecuteSqlQueryAsync(sqlBatch.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lưu Tab 3 (Người phụ thuộc): " + ex.Message);
            }
        }

        /// <summary>
        /// Tab 8: Lưu thông tin hợp đồng lao động vào bảng dmhdld
        /// ✅ ĐÚNG THEO WPF: HRHDLD/NewFrm.xaml.cs - Checkvalidtxt + saveCustomer
        /// Logic: DELETE toàn bộ dòng cũ theo ma_ns, sau đó INSERT dòng mới
        /// Table: dmhdld (ma_hdld, ma_ns, ma_ns_ct, ma_cv_ct, ma_plhd, ma_lhd, ma_cv, 
        ///                tu_ngay, den_ngay, ngay_hd, ngay_hl, dia_diem_lam_viec, dia_chi_lam_viec,
        ///                yeu_cau1, yeu_cau2, thoi_gian_lv, phuong_tien, muc_luong, phu_cap,
        ///                ghi_chu, status, date0, time0, user_id0, date, time, user_id)
        /// </summary>
        private async Task SaveTabContract()
        {
            try
            {
                string safeMa = Esc(CurRow.Ma_ns);

                var sqlBatch = new System.Text.StringBuilder();
                sqlBatch.AppendLine("SET XACT_ABORT ON;");
                sqlBatch.AppendLine("BEGIN TRAN;");

                // [HRHDLD] Xóa toàn bộ dữ liệu cũ theo ma_ns
                sqlBatch.AppendLine($"DELETE FROM dmhdld WHERE ma_ns = N'{safeMa}';");

                // [HRHDLD] Thêm dòng mới
                if (CurRow.ListHopDong != null && CurRow.ListHopDong.Count > 0)
                {
                    foreach (var row in CurRow.ListHopDong)
                    {
                        // Bỏ qua dòng trống
                        if (string.IsNullOrEmpty(row.Ma_hdld) && string.IsNullOrEmpty(row.Ma_ns_ct))
                            continue;

                        // Format dates - nếu không có giá trị thì NULL hoặc mặc định hôm nay
                        string tuNgay = row.Tu_ngay.HasValue ? $"'{row.Tu_ngay.Value:yyyy-MM-dd}'" : "NULL";
                        string denNgay = row.Den_ngay.HasValue ? $"'{row.Den_ngay.Value:yyyy-MM-dd}'" : "NULL";
                        string ngayHd = row.Ngay_hd.HasValue ? $"'{row.Ngay_hd.Value:yyyy-MM-dd}'" : $"'{DateTime.Now.Date:yyyy-MM-dd}'";
                        string ngayHl = row.Ngay_hl.HasValue ? $"'{row.Ngay_hl.Value:yyyy-MM-dd}'" : $"'{DateTime.Now.Date:yyyy-MM-dd}'";

                        // Format số tiền
                        decimal mucLuong = row.Muc_luong ?? 0;
                        string mucLuongStr = mucLuong.ToString("F0", System.Globalization.CultureInfo.InvariantCulture);
                        decimal thoiGianLv = row.Thoi_gian_lv ?? 0;
                        string thoiGianLvStr = thoiGianLv.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);

                        sqlBatch.AppendLine($@"
                    INSERT INTO dmhdld 
                        (ma_hdld, ma_ns, ma_ns_ct, ma_cv_ct, ma_plhd, ma_lhd, ma_cv, 
                         tu_ngay, den_ngay, ngay_hd, ngay_hl, dia_diem_lam_viec, dia_chi_lam_viec,
                         yeu_cau1, yeu_cau2, thoi_gian_lv, phuong_tien, muc_luong, phu_cap,
                         ghi_chu, status, date0, time0, user_id0, date, time, user_id)
                    VALUES (
                        N'{Esc(row.Ma_hdld ?? "")}', N'{safeMa}', N'{Esc(row.Ma_ns_ct ?? "")}', N'{Esc(row.Ma_cv_ct ?? "")}', 
                        N'{Esc(row.Ma_plhd ?? "")}', N'{Esc(row.Ma_lhd ?? "")}', N'{Esc(row.Ma_cv ?? "")}', 
                        {tuNgay}, {denNgay}, {ngayHd}, {ngayHl}, N'{Esc(row.Dia_diem_lam_viec ?? "")}', 
                        N'{Esc(row.Dia_chi_lam_viec ?? "")}', N'{Esc(row.Yeu_cau1 ?? "")}', N'{Esc(row.Yeu_cau2 ?? "")}', 
                        {thoiGianLvStr}, N'{Esc(row.Phuong_tien ?? "")}', {mucLuongStr}, N'{Esc(row.Phu_cap ?? "")}',
                        N'{Esc(row.Ghi_chu ?? "")}', N'{Esc(row.Status ?? "1")}',
                        '{DateTime.Now.Date:yyyy-MM-dd}', '{DateTime.Now:HH:mm:ss}', {curUser?.User_id ?? 0},
                        '{DateTime.Now.Date:yyyy-MM-dd}', '{DateTime.Now:HH:mm:ss}', {curUser?.User_id ?? 0});");
                    }
                }

                sqlBatch.AppendLine("COMMIT TRAN;");

                await myDb.ExecuteSqlQueryAsync(sqlBatch.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lưu Tab 8 (Hợp đồng lao động): " + ex.Message);
            }
        }

        /// <summary>
        /// Tab 9: Lưu thông tin quá trình tham gia bảo hiểm vào bảng dmqtbh
        /// ✅ ĐÚNG THEO WPF: DMQTBH/NewFrm.xaml.cs - saveCustomer + ListFunc.inserRowInDataBase
        /// Logic: DELETE toàn bộ dòng cũ theo ma_ns, sau đó INSERT dòng mới
        /// Table: dmqtbh (qtbhid, ma_ns, so_the_bhxh, tu_ngay, den_ngay, donvi_bh, donvi_cv,
        ///                 so_the_bhyt, bhyt_tungay, bhyt_denngay, ghi_chu, status, date0, time0, user_id0, date, time, user_id)
        /// </summary>
        private async Task SaveTabInsuranceHistory()
        {
            try
            {
                string safeMa = Esc(CurRow.Ma_ns);

                var sqlBatch = new System.Text.StringBuilder();
                sqlBatch.AppendLine("SET XACT_ABORT ON;");
                sqlBatch.AppendLine("BEGIN TRAN;");

                // [DMQTBH] Xóa toàn bộ dữ liệu cũ theo ma_ns
                sqlBatch.AppendLine($"DELETE FROM dmqtbh WHERE ma_ns = N'{safeMa}';");

                // [DMQTBH] Thêm dòng mới
                if (CurRow.ListQtbh != null && CurRow.ListQtbh.Count > 0)
                {
                    foreach (var row in CurRow.ListQtbh)
                    {
                        // Bỏ qua dòng trống
                        if (string.IsNullOrEmpty(row.So_the_bhxh) && string.IsNullOrEmpty(row.So_the_bhyt))
                            continue;

                        // Format dates - nếu không có giá trị thì NULL
                        string tuNgay = row.Tu_ngay.HasValue ? $"'{row.Tu_ngay.Value:yyyy-MM-dd}'" : "NULL";
                        string denNgay = row.Den_ngay.HasValue ? $"'{row.Den_ngay.Value:yyyy-MM-dd}'" : "NULL";
                        string bhytTungay = row.Bhyt_tungay.HasValue ? $"'{row.Bhyt_tungay.Value:yyyy-MM-dd}'" : "NULL";
                        string bhytDenngay = row.Bhyt_denngay.HasValue ? $"'{row.Bhyt_denngay.Value:yyyy-MM-dd}'" : "NULL";

                        sqlBatch.AppendLine($@"
                    INSERT INTO dmqtbh 
                        (qtbhid, ma_ns, so_the_bhxh, tu_ngay, den_ngay, donvi_bh, donvi_cv,
                         so_the_bhyt, bhyt_tungay, bhyt_denngay, ghi_chu, status,
                         date0, time0, user_id0, date, time, user_id)
                    VALUES (
                        '{Guid.NewGuid().ToString()}', N'{safeMa}', N'{Esc(row.So_the_bhxh ?? "")}', 
                        {tuNgay}, {denNgay}, N'{Esc(row.Donvi_bh ?? "")}', N'{Esc(row.Donvi_cv ?? "")}',
                        N'{Esc(row.So_the_bhyt ?? "")}', {bhytTungay}, {bhytDenngay}, 
                        N'{Esc(row.Ghi_chu ?? "")}', N'{Esc(row.Status ?? "1")}',
                        '{DateTime.Now.Date:yyyy-MM-dd}', '{DateTime.Now:HH:mm:ss}', {curUser?.User_id ?? 0},
                        '{DateTime.Now.Date:yyyy-MM-dd}', '{DateTime.Now:HH:mm:ss}', {curUser?.User_id ?? 0});");
                    }
                }

                sqlBatch.AppendLine("COMMIT TRAN;");

                await myDb.ExecuteSqlQueryAsync(sqlBatch.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lưu Tab 9 (Quá trình tham gia bảo hiểm): " + ex.Message);
            }
        }

        /// <summary>
        /// Tab 12: Lưu thông tin thôi việc nhân viên vào bảng dmnvn
        /// ✅ ĐÚNG THEO WPF: HRNVN/NewFrm.xaml.cs - INSERT/UPDATE logic
        /// Logic: 
        /// - Thêm mới: INSERT dòng mới
        /// - Sửa: UPDATE dòng cũ (chỉ update nếu dòng đã tồn tại)
        /// Table: dmnvn (ma_ns, ngay_lam_don, ngay_thoi_viec, ngay_thoi_thuc_te, tro_cap,
        ///             ngay_chot_so, ngay_tra_so, quyet_dinh_cham_dut_hd, quyet_dinh_tro_cap,
        ///             status, date0, time0, user_id0, date, time, user_id)
        /// </summary>
        private async Task SaveTabResignation()
        {
            try
            {
                if (CurRow?.NghiViecInfo == null) return;

                var row = CurRow.NghiViecInfo;
                string safeMa = Esc(CurRow.Ma_ns);
                int userId = curUser?.User_id ?? 0;
                string userName = curUser?.User_name ?? "";
                DateTime now = DateTime.Now;

                // Kiểm tra xem dữ liệu thôi việc này đã tồn tại chưa
                object existsCheck = myDb.GetValue($"SELECT COUNT(*) FROM dmnvn WHERE ma_ns = N'{safeMa}'");
                bool exists = existsCheck != null && Convert.ToInt32(existsCheck) > 0;

                if (exists)
                {
                    // [HRNVN] UPDATE dòng cũ
                    string updateSql = $@"
                    UPDATE dmnvn SET 
                        ma_ns = N'{safeMa}',
                        ngay_lam_don = {(row.Ngay_lam_don.HasValue ? $"'{row.Ngay_lam_don.Value:yyyy-MM-dd}'" : "NULL")},
                        ngay_thoi_viec = {(row.Ngay_thoi_viec.HasValue ? $"'{row.Ngay_thoi_viec.Value:yyyy-MM-dd}'" : "NULL")},
                        ngay_thoi_thuc_te = {(row.Ngay_thoi_thuc_te.HasValue ? $"'{row.Ngay_thoi_thuc_te.Value:yyyy-MM-dd}'" : "NULL")},
                        tro_cap = N'{Esc(row.Tro_cap ?? "0")}',
                        ngay_chot_so = {(row.Ngay_chot_so.HasValue ? $"'{row.Ngay_chot_so.Value:yyyy-MM-dd}'" : "NULL")},
                        ngay_tra_so = {(row.Ngay_tra_so.HasValue ? $"'{row.Ngay_tra_so.Value:yyyy-MM-dd}'" : "NULL")},
                        quyet_dinh_cham_dut_hd = N'{Esc(row.Quyet_dinh_cham_dut_hd ?? "")}',
                        quyet_dinh_tro_cap = N'{Esc(row.Quyet_dinh_tro_cap ?? "")}',
                        status = N'{Esc(row.Status ?? "1")}',
                        date0 = '{now.Date:yyyy-MM-dd}',
                        time0 = '{now:HH:mm:ss}',
                        user_id0 = {userId},
                        date = '{now.Date:yyyy-MM-dd}',
                        time = '{now:HH:mm:ss}',
                        user_id = {userId}
                    WHERE ma_ns = N'{safeMa}'";

                    await myDb.ExecuteSqlQueryAsync(updateSql);
                }
                else
                {
                    // [HRNVN] INSERT dòng mới
                    string insertSql = $@"
                    INSERT INTO dmnvn 
                        (ma_ns, ngay_lam_don, ngay_thoi_viec, ngay_thoi_thuc_te, tro_cap,
                         ngay_chot_so, ngay_tra_so, quyet_dinh_cham_dut_hd, quyet_dinh_tro_cap,
                         status, date0, time0, user_id0, date, time, user_id)
                    VALUES (
                        N'{safeMa}', 
                        {(row.Ngay_lam_don.HasValue ? $"'{row.Ngay_lam_don.Value:yyyy-MM-dd}'" : "NULL")},
                        {(row.Ngay_thoi_viec.HasValue ? $"'{row.Ngay_thoi_viec.Value:yyyy-MM-dd}'" : "NULL")},
                        {(row.Ngay_thoi_thuc_te.HasValue ? $"'{row.Ngay_thoi_thuc_te.Value:yyyy-MM-dd}'" : "NULL")},
                        N'{Esc(row.Tro_cap ?? "0")}',
                        {(row.Ngay_chot_so.HasValue ? $"'{row.Ngay_chot_so.Value:yyyy-MM-dd}'" : "NULL")},
                        {(row.Ngay_tra_so.HasValue ? $"'{row.Ngay_tra_so.Value:yyyy-MM-dd}'" : "NULL")},
                        N'{Esc(row.Quyet_dinh_cham_dut_hd ?? "")}',
                        N'{Esc(row.Quyet_dinh_tro_cap ?? "")}',
                        N'{Esc(row.Status ?? "1")}',
                        '{now.Date:yyyy-MM-dd}', 
                        '{now:HH:mm:ss}', 
                        {userId},
                        '{now.Date:yyyy-MM-dd}', 
                        '{now:HH:mm:ss}', 
                        {userId})";

                    await myDb.ExecuteSqlQueryAsync(insertSql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lưu Tab 12 (Thôi việc): " + ex.Message);
            }
        }

        /// <summary>
        /// Tab 10: Lưu thông tin thành phần gia đình vào bảng dmtpgd
        /// ✅ ĐÚNG THEO WPF: NewFrm.xaml.cs - Checkvalidtxt
        /// Logic: DELETE toàn bộ dòng cũ theo ma_ns, sau đó INSERT dòng mới
        /// </summary>
        private async Task SaveTabFamilyRelationship()
        {
            try
            {
                string safeMa = Esc(CurRow.Ma_ns);

                var sqlBatch = new System.Text.StringBuilder();
                sqlBatch.AppendLine("SET XACT_ABORT ON;");
                sqlBatch.AppendLine("BEGIN TRAN;");

                // [HRTPGD] Xóa toàn bộ dữ liệu cũ theo ma_ns
                sqlBatch.AppendLine($"DELETE FROM dmtpgd WHERE ma_ns = N'{safeMa}';");

                // [HRTPGD] Thêm dòng mới
                if (CurRow.ListQhgd != null && CurRow.ListQhgd.Count > 0)
                {
                    int stt = 1;
                    foreach (var row in CurRow.ListQhgd)
                    {
                        // Bỏ qua dòng trống
                        if (string.IsNullOrEmpty(row.Ten_tpgd) && string.IsNullOrEmpty(row.Ten_tpgd2))
                            continue;

                        string ngaySinh = string.IsNullOrEmpty(row.Nam_sinh) ? "NULL" : $"'{row.Nam_sinh}'";

                        sqlBatch.AppendLine($@"
                    INSERT INTO dmtpgd 
                        (ma_tpgd, ma_ns, stt, ma_qhgd, ten_tpgd, ten_tpgd2, ma_gioitinh, 
                         nam_sinh, cmnd, ma_so_thue, nguoi_phu_thuoc, nghe_nghiep, 
                         noi_lam_viec, cho_o_hien_nay, trinh_do_chinh_tri, 
                         truoc_cach_mang, trong_cach_mang, sau_cach_mang, ghi_chu, status,
                         date0, time0, user_id0, date, time, user_id)
                    VALUES (
                        '{Guid.NewGuid().ToString().Substring(0, 16)}', N'{safeMa}', {stt}, N'{Esc(row.Ma_qhgd ?? "")}', 
                        N'{Esc(row.Ten_tpgd ?? "")}', N'{Esc(row.Ten_tpgd2 ?? "")}', N'{Esc(row.Ma_gioitinh ?? "")}', 
                        {ngaySinh}, N'{Esc(row.Cmnd ?? "")}', N'{Esc(row.Ma_so_thue ?? "")}', 
                        N'{Esc(row.Nguoi_phu_thuoc ?? "0")}', N'{Esc(row.Nghe_nghiep ?? "")}', 
                        N'{Esc(row.Noi_lam_viec ?? "")}', N'{Esc(row.Cho_o_hien_nay ?? "")}', 
                        N'{Esc(row.Trinh_do_chinh_tri ?? "")}', 
                        N'{Esc(row.Truoc_cach_mang ?? "")}', N'{Esc(row.Trong_cach_mang ?? "")}', 
                        N'{Esc(row.Sau_cach_mang ?? "")}', N'{Esc(row.Ghi_chu ?? "")}', N'{Esc(row.Status ?? "1")}',
                        '{DateTime.Now.Date:yyyy-MM-dd}', '{DateTime.Now:HH:mm:ss}', {curUser?.User_id ?? 0},
                        '{DateTime.Now.Date:yyyy-MM-dd}', '{DateTime.Now:HH:mm:ss}', {curUser?.User_id ?? 0});");

                        stt++;
                    }
                }

                sqlBatch.AppendLine("COMMIT TRAN;");

                await myDb.ExecuteSqlQueryAsync(sqlBatch.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lưu Tab 10 (Thành phần gia đình): " + ex.Message);
            }
        }

        /// <summary>
        /// Tab 11: Lưu thông tin điều chuyển nhân sự vào bảng dmqtdc
        /// ✅ ĐÚNG THEO WPF: HRQTDC/NewFrm.xaml.cs - Checkvalidtxt
        /// Logic: DELETE toàn bộ dòng cũ theo ma_ns, sau đó INSERT dòng mới
        /// Table: dmqtdc (qtdcid, ma_ns, ngay_dc_cu, ma_bpns_cu, ma_cv_cu, ma_cd_cu, ghi_chu_cu,
        ///                     ngay_dc_moi, ma_bpns_moi, ma_cv_moi, ma_cd_moi, ghi_chu_moi, status, date0, time0, user_id0, date, time, user_id)
        /// </summary>
        private async Task SaveTabTransfer()
        {
            try
            {
                string safeMa = Esc(CurRow.Ma_ns);

                var sqlBatch = new System.Text.StringBuilder();
                sqlBatch.AppendLine("SET XACT_ABORT ON;");
                sqlBatch.AppendLine("BEGIN TRAN;");

                // [HRQTDC] Xóa toàn bộ dữ liệu cũ theo ma_ns
                sqlBatch.AppendLine($"DELETE FROM dmqtdc WHERE ma_ns = N'{safeMa}';");

                // [HRQTDC] Thêm dòng mới
                if (CurRow.ListDieuChuyen != null && CurRow.ListDieuChuyen.Count > 0)
                {
                    foreach (var row in CurRow.ListDieuChuyen)
                    {
                        // Bỏ qua dòng trống
                        if (string.IsNullOrEmpty(row.Ma_bpns_cu) && string.IsNullOrEmpty(row.Ma_bpns_moi))
                            continue;

                        // Format dates - nếu không có giá trị thì NULL
                        string ngayDcCu = row.Ngay_dc_cu.HasValue ? $"'{row.Ngay_dc_cu.Value:yyyy-MM-dd}'" : "NULL";
                        string ngayDcMoi = row.Ngay_dc_moi.HasValue ? $"'{row.Ngay_dc_moi.Value:yyyy-MM-dd}'" : "NULL";

                        sqlBatch.AppendLine($@"
                    INSERT INTO dmqtdc 
                        (qtdcid, ma_ns, ngay_dc_cu, ma_bpns_cu, ma_cv_cu, ma_cd_cu, ghi_chu_cu,
                         ngay_dc_moi, ma_bpns_moi, ma_cv_moi, ma_cd_moi, ghi_chu_moi, status,
                         date0, time0, user_id0, date, time, user_id)
                    VALUES (
                        '{Guid.NewGuid().ToString()}', N'{safeMa}', {ngayDcCu}, N'{Esc(row.Ma_bpns_cu ?? "")}', 
                        N'{Esc(row.Ma_cv_cu ?? "")}', N'{Esc(row.Ma_cd_cu ?? "")}', N'{Esc(row.Ghi_chu_cu ?? "")}',
                        {ngayDcMoi}, N'{Esc(row.Ma_bpns_moi ?? "")}', N'{Esc(row.Ma_cv_moi ?? "")}', 
                        N'{Esc(row.Ma_cd_moi ?? "")}', N'{Esc(row.Ghi_chu_moi ?? "")}', N'{Esc(row.Status ?? "1")}',
                        '{DateTime.Now.Date:yyyy-MM-dd}', '{DateTime.Now:HH:mm:ss}', {curUser?.User_id ?? 0},
                        '{DateTime.Now.Date:yyyy-MM-dd}', '{DateTime.Now:HH:mm:ss}', {curUser?.User_id ?? 0});");
                    }
                }

                sqlBatch.AppendLine("COMMIT TRAN;");

                await myDb.ExecuteSqlQueryAsync(sqlBatch.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lưu Tab 11 (Điều chuyển): " + ex.Message);
            }
        }

        /// <summary>
        /// Tab 13: Lưu thông tin quá trình công tác vào bảng dmnvct
        /// ✅ ĐÚNG THEO WPF: HRDMNVCT/NewFrm.xaml.cs - Checkvalidtxt
        /// Logic: DELETE toàn bộ dòng cũ theo ma_ns, sau đó INSERT dòng mới
        /// Table: dmnvct (ma_ns, ma_quyet_dinh, ngay_di, ngay_ve, ma_nct, chi_phi, noi_dung, ket_qua, ghi_chu, status, date0, time0, user_id0, date, time, user_id)
        /// </summary>
        private async Task SaveTabWorkingProcess()
        {
            try
            {
                string safeMa = Esc(CurRow.Ma_ns);

                var sqlBatch = new System.Text.StringBuilder();
                sqlBatch.AppendLine("SET XACT_ABORT ON;");
                sqlBatch.AppendLine("BEGIN TRAN;");

                // [HRDMNVCT] Xóa toàn bộ dữ liệu cũ theo ma_ns
                sqlBatch.AppendLine($"DELETE FROM dmnvct WHERE ma_ns = N'{safeMa}';");

                // [HRDMNVCT] Thêm dòng mới
                if (CurRow.ListQtct != null && CurRow.ListQtct.Count > 0)
                {
                    foreach (var row in CurRow.ListQtct)
                    {
                        // Bỏ qua dòng trống
                        if (string.IsNullOrEmpty(row.Ma_quyet_dinh))
                            continue;

                        // Format dates - nếu không có giá trị thì NULL
                        string ngayDi = row.Ngay_di.HasValue ? $"'{row.Ngay_di.Value:yyyy-MM-dd}'" : "NULL";
                        string ngayVe = row.Ngay_ve.HasValue ? $"'{row.Ngay_ve.Value:yyyy-MM-dd}'" : "NULL";

                        // Format chi phí - nếu là 0 thì để 0
                        decimal chiPhi = row.Chi_phi ?? 0;
                        string chiPhiStr = chiPhi.ToString("F0", System.Globalization.CultureInfo.InvariantCulture);

                        sqlBatch.AppendLine($@"
                    INSERT INTO dmnvct 
                        (ma_ns, ma_quyet_dinh, ngay_di, ngay_ve, ma_nct, chi_phi, noi_dung, ket_qua, ghi_chu, status,
                         date0, time0, user_id0, date, time, user_id)
                    VALUES (
                        N'{safeMa}', N'{Esc(row.Ma_quyet_dinh ?? "")}', {ngayDi}, {ngayVe}, N'{Esc(row.Ma_nct ?? "")}', 
                        {chiPhiStr}, N'{Esc(row.Noi_dung ?? "")}', N'{Esc(row.Ket_qua ?? "")}', N'{Esc(row.Ghi_chu ?? "")}', 
                        N'{Esc(row.Status ?? "1")}',
                        '{DateTime.Now.Date:yyyy-MM-dd}', '{DateTime.Now:HH:mm:ss}', {curUser?.User_id ?? 0},
                        '{DateTime.Now.Date:yyyy-MM-dd}', '{DateTime.Now:HH:mm:ss}', {curUser?.User_id ?? 0});");
                    }
                }

                sqlBatch.AppendLine("COMMIT TRAN;");

                await myDb.ExecuteSqlQueryAsync(sqlBatch.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lưu Tab 13 (Quá trình công tác): " + ex.Message);
            }
        }

        // ===== SAVE =====
        private async Task SaveHsnv()
        {
            try
            {
                // Trim trước khi validate (giống WPF AcceptChanges rồi mới validate)
                CurRow.Ma_ns = (CurRow.Ma_ns ?? "").Trim().ToUpper();
                CurRow.Ten_ns = (CurRow.Ten_ns ?? "").Trim();

                // Tab 2: Giảm trừ (Luôn hiển thị)
                if (GridCtgtk != null && GridCtgtk.IsEditing())
                {
                    await GridCtgtk.SaveChangesAsync();
                    if (GridCtgtk.IsEditing()) return;
                }

                // Tab 3: Người phụ thuộc (Luôn hiển thị)
                if (GridCtnpt != null && GridCtnpt.IsEditing())
                {
                    await GridCtnpt.SaveChangesAsync();
                    if (GridCtnpt.IsEditing()) return;
                }

                // Tab 8: Hợp đồng lao động
                if (IsShowHopDong && GridHopDong != null && GridHopDong.IsEditing())
                {
                    await GridHopDong.SaveChangesAsync();
                    if (GridHopDong.IsEditing()) return;
                }

                // Tab 9: Quá trình tham gia bảo hiểm
                if (IsShowQtbh && GridQtbh != null && GridQtbh.IsEditing())
                {
                    await GridQtbh.SaveChangesAsync();
                    if (GridQtbh.IsEditing()) return;
                }

                // Tab 10: Quan hệ gia đình
                if (IsShowQhgd && GridQhgd != null && GridQhgd.IsEditing())
                {
                    await GridQhgd.SaveChangesAsync();
                    if (GridQhgd.IsEditing()) return;
                }

                // Tab 11: Điều chuyển nhân sự
                if (IsShowDieuChuyen && GridDieuChuyen != null && GridDieuChuyen.IsEditing())
                {
                    await GridDieuChuyen.SaveChangesAsync();
                    if (GridDieuChuyen.IsEditing()) return;
                }

                // Tab 13: Quá trình công tác
                if (IsShowQtct && GridQtct != null && GridQtct.IsEditing())
                {
                    await GridQtct.SaveChangesAsync();
                    if (GridQtct.IsEditing()) return;
                }

                // ===== VALIDATE ALL TABS =====
                if (!ValidateAllTabs())
                    return;

                // ===== SAVE EMPLOYEE PHOTO =====
                await SaveEmployeePhoto(CurRow.Ma_ns);

                // ===== SAVE ALL TABS =====
                await SaveAllTabs();

                // ===== CLOSE FORM =====
                if (DxWindowmodel != null)
                    myModal.CloseModal(DxWindowmodel);

                if (ShowAdd == true)
                    OnClose.InvokeAsync();
                else
                    myNavi.NavigateTo(ParentPage);
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao(Lap["Save failed"] + ": " + ex.Message);
            }
        }

        // ===== SAVE ẢNH VÀO DB =====
        private async Task SaveEmployeePhoto(string maNs)
        {
            if (_pendingPhotoBytes == null || _pendingPhotoBytes.Length == 0) return;
            try
            {
                string safeMa = Esc(maNs);
                // Encode binary thành hex string để dùng trong T-SQL
                string hexStr = BitConverter.ToString(_pendingPhotoBytes).Replace("-", "");

                // Lưu ảnh vào bảng dmnsimg
                await myDb.ExecuteSqlQueryAsync($"DELETE FROM dmnsimg WHERE ma_ns = N'{safeMa}'");
                await myDb.ExecuteSqlQueryAsync(
                    $"INSERT INTO dmnsimg (ma_ns, img_ns) VALUES (N'{safeMa}', CONVERT(VARBINARY(MAX), '{hexStr}', 2))");

                _pendingPhotoBytes = null;
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao("Lỗi lưu ảnh: " + ex.Message);
            }
        }

        // ===== TAB SPECIFIC SAVE METHODS =====

        // ===== HELPER METHODS =====
        
        /// <summary>
        /// Helper: insert 1 dòng ctgtk theo loại giảm trừ (ma_gt)
        /// ✅ ĐÚNG THEO WPF: V_Nhan - StartForm.xaml.cs
        /// Tham số: maGt, keyGt, tienGt, luongThang, luongThangNt, luong, luongNt
        /// </summary>
        private async Task InsertDeductionRow(string maGt, int keyGt, decimal tienGt, CtgtkRow row,
            string safeSttRec, string safeDvcs, string safeMa,
            string ngayHl1, string ngayHl2,
            decimal luongThang, decimal luongThangNt, decimal luong, decimal luongNt)
        {
            // ✅ FIX: Format decimal với InvariantCulture (dấu . không phải ,)
            string luongThangNtStr = luongThangNt.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            string luongThangStr = luongThang.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            string luongNtStr = luongNt.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            string luongStr = luong.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            string tienGtStr = tienGt.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);

            string sql = $@"
                INSERT INTO ctgtk
                    (stt_rec, key_gt, ma_dvcs, ma_ns, ngay_hl1, ngay_hl2,
                     ma_gt, dien_giai,
                     luong_thang_nt, luong_thang, luong_nt, luong,
                     tien_gt, tien_gt_nt)
                VALUES (
                    N'{safeSttRec}', {keyGt}, N'{safeDvcs}', N'{safeMa}', {ngayHl1}, {ngayHl2},
                    N'{maGt}', N'{Esc(row.Dien_giai ?? "")}',
                    {luongThangNtStr}, {luongThangStr}, {luongNtStr}, {luongStr},
                    {tienGtStr}, 0)";

            await myDb.ExecuteSqlQueryAsync(sql);
        }

        // ===== UI NAVIGATION =====
        private void ScrollToSection(int section)
        {
            ActiveSection = section;
            _ = JavacriptS.InvokeVoidAsync("scrollToElement", $"section-{section}");
            StateHasChanged();
        }

        private void ShowSectionSelector()
        {
            _ = JavacriptS.InvokeVoidAsync("SIS_ShowModal", "sectionSelectorModal");
            StateHasChanged();
        }

        private void HideSectionSelector()
        {
            _ = JavacriptS.InvokeVoidAsync("SIS_HideModal", "sectionSelectorModal");
            StateHasChanged();
        }

        private void ApplySectionSelection()
        {
            _ = JavacriptS.InvokeVoidAsync("SIS_HideModal", "sectionSelectorModal");
            StateHasChanged();
        }

        // ===== LOCALIZATION =====
        private async Task Language()
        {
            try
            {
                IsLangEn = false;
                UserLang = await myLcStore.GetItemAsync<string>("userlang");
                if (!string.IsNullOrEmpty(UserLang) && UserLang.Equals("En"))
                    IsLangEn = true;

                await Lap.LoadLocalizationAsync($"hr-{(IsLangEn ? "en" : "vi")}");
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // ===== UTILITIES =====
        private static string TrimTo(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return "";
            value = value.Trim();
            return value.Length > maxLength ? value.Substring(0, maxLength) : value;
        }

        /// <summary>
        /// Escape SQL string values properly
        /// - Removes leading/trailing whitespace
        /// - Escapes single quotes by doubling them
        /// - Handles null values
        /// </summary>
        private static string Esc(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s.Trim().Replace("'", "''");
        }

        /// <summary>
        /// Build safe SQL value for UPDATE statement
        /// Ensures proper quoting and escaping for nvarchar fields
        /// </summary>
        private static string SqlString(string value)
        {
            if (string.IsNullOrEmpty(value)) return "N''";
            // Escape single quotes and wrap with N'...'
            return "N'" + value.Trim().Replace("'", "''") + "'";
        }

        // Đảm bảo chữ ký hàm phải có đủ 3 chữ: async, ValueTask, và DisposeAsync
        public async ValueTask DisposeAsync()
        {
            Pageobj?.Dispose();

            try
            {
                await JavacriptS.InvokeVoidAsync("eval", "document.querySelectorAll('.modal-backdrop').forEach(el => el.remove()); document.body.classList.remove('modal-open'); document.body.style.paddingRight = '';");
            }
            catch { }
        }

        // ===== BUILD UPDATE SQL =====
        private string BuildUpdateSql()
        {
            string safe = Esc(Ma_ma);
            DateTime now = DateTime.Now;
            
            return @"UPDATE " + sqlTableName + @" SET 
                ten_ns = " + SqlString(CurRow.Ten_ns) + @",
                ten_ns2 = " + SqlString(CurRow.Ten_ns2) + @",
                gioi_tinh = " + SqlString(CurRow.Gioi_tinh) + @",
                ngay_sinh = " + (CurRow.Ngay_sinh.HasValue ? $"'{CurRow.Ngay_sinh.Value:yyyy-MM-dd}'" : "NULL") + @",
                cmnd = " + SqlString(CurRow.Cmnd) + @",
                ngay_cmnd = " + (CurRow.Ngay_cmnd.HasValue ? $"'{CurRow.Ngay_cmnd.Value:yyyy-MM-dd}'" : "NULL") + @",
                nc_cmnd = " + SqlString(CurRow.Nc_cmnd) + @",
                quoc_tich = " + (string.IsNullOrEmpty(CurRow.Quoc_tich) ? "NULL" : $"N'{CurRow.Quoc_tich.Replace("'", "''")}'") + @",
                quoc_tich2 = " + SqlString(CurRow.Quoc_tich2) + @",
                nguyen_quan = " + (string.IsNullOrEmpty(CurRow.Nguyen_quan) ? "NULL" : $"N'{CurRow.Nguyen_quan.Replace("'", "''")}'") + @",
                dien_thoai = " + SqlString(CurRow.Dien_thoai) + @",
                dien_thoai_co_dinh = " + SqlString(CurRow.Dien_thoai_co_dinh) + @",
                fax = " + SqlString(CurRow.Fax) + @",
                e_mail = " + SqlString(CurRow.E_mail) + @",
                dia_chi = " + SqlString(CurRow.Dia_chi) + @",
                ho_khau = " + SqlString(CurRow.Ho_khau) + @",
                tinh_thanh = " + SqlString(CurRow.Tinh_thanh) + @",
                dc_cu_tru = " + SqlString(CurRow.Dc_cu_tru) + @",
                ngay_ctru = " + (CurRow.Ngay_ctru.HasValue ? $"'{CurRow.Ngay_ctru.Value:yyyy-MM-dd}'" : "NULL") + @",
                ma_bpns = " + SqlString(CurRow.Ma_bpns) + @",
                ma_todoi = " + SqlString(CurRow.Ma_todoi) + @",
                khoi = " + SqlString(CurRow.Khoi) + @",
                ngay_vao = " + (CurRow.Ngay_vao.HasValue ? $"'{CurRow.Ngay_vao.Value:yyyy-MM-dd}'" : "NULL") + @",
                ma_loai_hdong = " + SqlString(CurRow.Ma_loai_hdong) + @",
                ngay_bien_che = " + (CurRow.Ngay_bien_che.HasValue ? $"'{CurRow.Ngay_bien_che.Value:yyyy-MM-dd}'" : "NULL") + @",
                ngay_hdld = " + (CurRow.Ngay_hdld.HasValue ? $"'{CurRow.Ngay_hdld.Value:yyyy-MM-dd}'" : "NULL") + @",
                ma_so_thue = " + SqlString(CurRow.Ma_so_thue) + @",
                ngay_mst = " + (CurRow.Ngay_mst.HasValue ? $"'{CurRow.Ngay_mst.Value:yyyy-MM-dd}'" : "NULL") + @",
                nc_mst = " + SqlString(CurRow.Nc_mst) + @",
                status = " + SqlString(CurRow.Status) + @",
                bi_danh = " + SqlString(CurRow.Bi_danh) + @",
                noi_sinh = " + SqlString(CurRow.Noi_sinh) + @",
                ma_dantoc = " + SqlString(CurRow.Ma_dantoc) + @",
                ma_quoctich = " + SqlString(CurRow.Ma_quoctich) + @",
                ma_tongiao = " + SqlString(CurRow.Ma_tongiao) + @",
                ma_tp_banthan = " + SqlString(CurRow.Ma_tp_banthan) + @",
                ma_tp_giadinh = " + SqlString(CurRow.Ma_tp_giadinh) + @",
                ma_ngoaingu = " + SqlString(CurRow.Ma_ngoaingu) + @",
                ma_tinhoc = " + SqlString(CurRow.Ma_tinhoc) + @",
                ma_tt_honnhan = " + SqlString(CurRow.Ma_tt_honnhan) + @",
                ma_td_vanhoa = " + SqlString(CurRow.Ma_td_vanhoa) + @",
                ma_tt_suckhoe = " + SqlString(CurRow.Ma_tt_suckhoe) + @",
                ma_bangcap = " + SqlString(CurRow.Ma_bangcap) + @",
                ma_trinhdo = " + SqlString(CurRow.Ma_trinhdo) + @",
                ma_chuyennganh = " + SqlString(CurRow.Ma_chuyennganh) + @",
                ma_chucvu = " + SqlString(CurRow.Ma_chucvu) + @",
                ma_chucdanh = " + SqlString(CurRow.Ma_chucdanh) + @",
                ma_loai_td = " + SqlString(CurRow.Ma_loai_td) + @",
                image_url = " + SqlString(CurRow.Image_url) + @",
                khicannhantincho = " + SqlString(CurRow.Khicannhantincho) + @",
                diachinhantin = " + SqlString(CurRow.Diachinhantin) + @",
                dt_cu_tru = " + ((CurRow.Dt_cu_tru ?? 1).ToString("F2", System.Globalization.CultureInfo.InvariantCulture)) + @",
                thue_ck = " + ((CurRow.Thue_ck ?? 0).ToString("F2", System.Globalization.CultureInfo.InvariantCulture)) + @",
                khu_kt_ck = " + ((CurRow.Khu_kt_ck ?? 0).ToString("F2", System.Globalization.CultureInfo.InvariantCulture)) + @",
                uq_qt_ck = " + ((CurRow.Uq_qt_ck ?? 0).ToString("F2", System.Globalization.CultureInfo.InvariantCulture)) + @",
                hdld3thang = " + ((CurRow.Hdld3thang ?? 0).ToString("F2", System.Globalization.CultureInfo.InvariantCulture)) + @",
                ghi_chu = " + SqlString(CurRow.Ghi_chu) + @",
                an_com = " + ((CurRow.An_com ?? 0).ToString("F2", System.Globalization.CultureInfo.InvariantCulture)) + @",
                giam_tru = " + (CurRow.Giam_tru ?? 0) + @",
                muc_gt = " + ((CurRow.Muc_gt ?? 0).ToString("F2", System.Globalization.CultureInfo.InvariantCulture)) + @",
                muc_gt_nt = " + ((CurRow.Muc_gt_nt ?? 0).ToString("F2", System.Globalization.CultureInfo.InvariantCulture)) + @",
                hd_thue = " + SqlString(CurRow.Hd_thue) + @",
                so_the_bhyt = " + SqlString(CurRow.So_the_bhyt) + @",
                so_so_bhxh = " + SqlString(CurRow.So_so_bhxh) + @",
                ngay_tham_gia_bhxh = " + (CurRow.Ngay_tham_gia_bhxh.HasValue ? $"'{CurRow.Ngay_tham_gia_bhxh.Value:yyyy-MM-dd}'" : "NULL") + @",
                ngay_cap_so_bhxh = " + (CurRow.Ngay_cap_so_bhxh.HasValue ? $"'{CurRow.Ngay_cap_so_bhxh.Value:yyyy-MM-dd}'" : "NULL") + @",
                ngay_het_han_bhyt = " + (CurRow.Ngay_het_han_bhyt.HasValue ? $"'{CurRow.Ngay_het_han_bhyt.Value:yyyy-MM-dd}'" : "NULL") + @",
                ma_noi_kcb = " + SqlString(CurRow.Ma_noi_kcb) + @",
                noi_cap_so_bhxh = " + SqlString(CurRow.Noi_cap_so_bhxh) + @",
                ma_chinhsach = " + SqlString(CurRow.Ma_chinhsach) + @",
                ngay_huong_chinhsach = " + (CurRow.Ngay_huong_chinhsach.HasValue ? $"'{CurRow.Ngay_huong_chinhsach.Value:yyyy-MM-dd}'" : "NULL") + @",
                ma_noi_cap_tbhyt = " + SqlString(CurRow.Ma_noi_cap_tbhyt) + @",
                noi_ket_nap_doan = " + SqlString(CurRow.Noi_ket_nap_doan) + @",
                ngay_vao_doan = " + (CurRow.Ngay_vao_doan.HasValue ? $"'{CurRow.Ngay_vao_doan.Value:yyyy-MM-dd}'" : "NULL") + @",
                ma_chucvu_doan = " + SqlString(CurRow.Ma_chucvu_doan) + @",
                ngay_vao_dang = " + (CurRow.Ngay_vao_dang.HasValue ? $"'{CurRow.Ngay_vao_dang.Value:yyyy-MM-dd}'" : "NULL") + @",
                ngay_chinh_thuc_vao_dang = " + (CurRow.Ngay_chinh_thuc_vao_dang.HasValue ? $"'{CurRow.Ngay_chinh_thuc_vao_dang.Value:yyyy-MM-dd}'" : "NULL") + @",
                noi_ket_nap_dang = " + SqlString(CurRow.Noi_ket_nap_dang) + @",
                ma_chucvu_dang = " + SqlString(CurRow.Ma_chucvu_dang) + @",
                ngay_thamgia_cm = " + (CurRow.Ngay_thamgia_cm.HasValue ? $"'{CurRow.Ngay_thamgia_cm.Value:yyyy-MM-dd}'" : "NULL") + @",
                ngay_nhap_ngu = " + (CurRow.Ngay_nhap_ngu.HasValue ? $"'{CurRow.Ngay_nhap_ngu.Value:yyyy-MM-dd}'" : "NULL") + @",
                ngay_xuat_ngu = " + (CurRow.Ngay_xuat_ngu.HasValue ? $"'{CurRow.Ngay_xuat_ngu.Value:yyyy-MM-dd}'" : "NULL") + @",
                ma_chucvu_quandoi = " + SqlString(CurRow.Ma_chucvu_quandoi) + @",
                trinh_do_trinh_tri = " + SqlString(CurRow.Trinh_do_trinh_tri) + @",
                trinh_do_quan_ly = " + SqlString(CurRow.Trinh_do_quan_ly) + @",
                he_so_luong = " + ((CurRow.He_so_luong ?? 0).ToString("F2", System.Globalization.CultureInfo.InvariantCulture)) + @",
                he_so_phu_cap = " + ((CurRow.He_so_phu_cap ?? 0).ToString("F2", System.Globalization.CultureInfo.InvariantCulture)) + @",
                bac_luong = " + ((CurRow.Bac_luong ?? 0).ToString("F2", System.Globalization.CultureInfo.InvariantCulture)) + @",
                ngay_huong_luong = " + (CurRow.Ngay_huong_luong.HasValue ? $"'{CurRow.Ngay_huong_luong.Value:yyyy-MM-dd}'" : "NULL") + @",
                muc_luong = " + ((CurRow.Muc_luong ?? 0).ToString("F2", System.Globalization.CultureInfo.InvariantCulture)) + @",
                ngay_chuyen_di = " + (CurRow.Ngay_chuyen_di.HasValue ? $"'{CurRow.Ngay_chuyen_di.Value:yyyy-MM-dd}'" : "NULL") + @",
                ma_lydonghi = " + SqlString(CurRow.Ma_lydonghi) + @",
                da_chuyen_di = " + SqlString(CurRow.Da_chuyen_di) + @",
                ma_td1 = " + SqlString(CurRow.Ma_td1) + @",
                ma_td2 = " + SqlString(CurRow.Ma_td2) + @",
                ma_td3 = " + SqlString(CurRow.Ma_td3) + @",
                sl_td1 = " + ((CurRow.Sl_td1 ?? 0).ToString("F2", System.Globalization.CultureInfo.InvariantCulture)) + @",
                sl_td2 = " + ((CurRow.Sl_td2 ?? 0).ToString("F2", System.Globalization.CultureInfo.InvariantCulture)) + @",
                sl_td3 = " + ((CurRow.Sl_td3 ?? 0).ToString("F2", System.Globalization.CultureInfo.InvariantCulture)) + @",
                gc_td1 = " + SqlString(CurRow.Gc_td1) + @",
                gc_td2 = " + SqlString(CurRow.Gc_td2) + @",
                gc_td3 = " + SqlString(CurRow.Gc_td3) + @",
                ma_tra_cuu = " + SqlString(CurRow.Ma_tra_cuu) + @",
                stt_rec = " + SqlString(CurRow.Stt_rec) + @",
                search = " + SqlString(CurRow.Search) + @",
                date = '" + now.ToString("yyyy-MM-dd") + @"',
                time = '" + now.ToString("HH:mm:ss") + @"',
                user_id = " + (CurRow.User_id ?? 0) + @",
                user_name = " + SqlString(CurRow.User_name) + @"
            WHERE ma_ns = N'" + safe + "'";
        }
    }
}
