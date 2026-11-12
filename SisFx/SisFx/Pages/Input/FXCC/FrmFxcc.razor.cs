using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SisCom;
using SisCom.Model;
using SisCom.Shared;
using SisFx.Pages.Input.FXCC;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Data;
using System.Threading.Tasks;

namespace SisFx.Pages.Input.FXCC
{
    public partial class FrmFxcc
    {
        // ---- Hằng dùng trong template razor ----
        protected const string kieukh1 = "1";

        // ---- Tham chiếu thành phần ----
        private DxTextBox txtMaCcMe;
        private DxTextBox txtMaTraCuu;
        private DxGrid GridCt;
        private DxGrid GridCtct;
        private object SelectedCtDmcc;
        private object SelectedCtCtcc;
        public ElementReference GridWrapperCt;
        public ElementReference GridWrapperCtct;

        private DotNetObjectReference<FrmFxcc> Pageobj;
        private int ActiveTabIndex = 0;
        private string LastFocusedGrid = "CT"; 

        // ---- Dữ liệu ----
        public Dmcc CurDmcc { get; set; } = new Dmcc();
        public List<CtDmcc> LstCtDmcc { get; set; } = new List<CtDmcc>();
        public List<CtCtcc> LstCtCtcc { get; set; } = new List<CtCtcc>();

        protected List<object> ListKieuKH = new List<object>()
        {
            new { Value = "1", Text = "1. Theo thời gian" },
            new { Value = "2", Text = "2. Phân bổ 2 lần" }
        };

        // ---- Lifecycle: OnInitialized moved to .razor @code ----
        // ---- OnAfterRenderAsync, CloseForm, CancelForm moved to .razor @code ----

        // ---- Helper: so sánh curActionTask qua chuỗi (tránh reference trực tiếp ActionTask enum trong .cs) ----
        private bool IsNew   => curActionTask.ToString() == "New";
        private bool IsCopy  => curActionTask.ToString() == "Copy";
        private bool IsEdit  => curActionTask.ToString() == "Edit";
        private bool IsView  => curActionTask.ToString() == "View";

        // ---- KHỞI TẠO / LOAD / COPY ----

        private void CreateNewDM()
        {
            string maDvcs = myStateMN.GetMa_Dvcs();
            if (string.IsNullOrWhiteSpace(maDvcs) || maDvcs.Trim().ToUpper() == "ALL")
            {
                myStateMN.SetNewThongbao(Lap["fxcc.err.no_dvcs"]);
                return;
            }

            CurDmcc = new Dmcc
            {
                Ma_dvcs = maDvcs,
                Ngay_mua = DateTime.Now,
                Ngay_kh0 = DateTime.Now,
                Kieu_kh_str = "1",
                So_ky_kh = 0,
                Tong_sl = 0,
                Tien_cl = 0
            };

            LstCtDmcc = new List<CtDmcc>
            {
                new CtDmcc { Stt = 1, Cc0 = 1, Sl_cc = 1 }
            };
            LstCtCtcc = new List<CtCtcc>();

            AllowEditMode = true;
            AllowEditMa = true;
        }

        private async Task LoadData(string id)
        {
            string safeId = Escape(id);
            string sql = $"SELECT * FROM {sqlTableName} WHERE {SqlTableKey} = N'{safeId}'";
            DataSet ds = await myDb.LoadDataset(sql);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var list = myDb.ConvertDataTable<Dmcc>(ds.Tables[0]);
                CurDmcc = list[0];
                // Trim vì bảng DB có thể là kiểu CHAR có trailing space
                CurDmcc.Ma_cc_me = CurDmcc.Ma_cc_me?.Trim();
                CurDmcc.Kieu_kh_str = CurDmcc.Kieu_kh.HasValue ? CurDmcc.Kieu_kh.Value.ToString("0") : "1";

                string sqlCt = $@"
                    SELECT a.*, b.ten_bpts AS ten_bpcc 
                    FROM ctdmcc a 
                    LEFT JOIN dmbpts b ON a.ma_bpcc = b.ma_bpts 
                    WHERE a.ma_cc_me = N'{safeId}' 
                    ORDER BY a.ma_cc";

                var dsCt = await myDb.LoadDataset(sqlCt);
                if (dsCt != null && dsCt.Tables.Count > 0)
                {
                    LstCtDmcc = myDb.ConvertDataTable<CtDmcc>(dsCt.Tables[0]);
                    int stt = 1;
                    LstCtDmcc.ForEach(x =>
                    {
                        x.Stt = stt++;
                        x.Ma_cc = x.Ma_cc?.Trim();
                        x.Ma_bpcc = x.Ma_bpcc?.Trim();
                        x.Ten_bpcc = x.Ten_bpcc?.Trim(); // Bổ sung Trim cho Tên BPSD
                        x.Tk_cp = x.Tk_cp?.Trim();
                        x.Ma_bpht = x.Ma_bpht?.Trim();
                        x.Ma_px = x.Ma_px?.Trim();
                        x.Ma_vv = x.Ma_vv?.Trim();
                        x.So_ct = x.So_ct?.Trim();
                    });
                }

                string sqlPt = $"SELECT * FROM ctctcc WHERE ma_cc_me = N'{safeId}' ORDER BY ten_ptkt";
                var dsPt = await myDb.LoadDataset(sqlPt);
                if (dsPt != null && dsPt.Tables.Count > 0)
                {
                    LstCtCtcc = myDb.ConvertDataTable<CtCtcc>(dsPt.Tables[0]);
                    int stt = 1;
                    LstCtCtcc.ForEach(x => x.Stt = stt++);
                }
            }

            if (IsView)
            {
                AllowEditMa = false;
                AllowEditMode = false;
            }
            else
            {
                AllowEditMode = true;

                if (IsNew || IsCopy)
                {
                    AllowEditMa = true;
                }
                else if (IsEdit)
                {
                    int checkResult = 1; // Mặc định là an toàn (chưa phát sinh)
                    try
                    {
                        string sqlCheck = $@"
                DECLARE @return_value int;
                EXEC @return_value = [CheckXoacc] @ma_cc_me = N'{safeId}';
                SELECT @return_value AS KetQua;";

                        var dsCheck = await myDb.LoadDataset(sqlCheck);
                        if (dsCheck != null && dsCheck.Tables.Count > 0 && dsCheck.Tables[0].Rows.Count > 0)
                        {
                            var val = dsCheck.Tables[0].Rows[0][0];
                            if (val != DBNull.Value) checkResult = Convert.ToInt32(val);
                        }
                    }
                    catch { }

                    // checkResult == 0 nghĩa là KHÔNG an toàn (đã có phát sinh) -> khóa mã
                    AllowEditMa = (checkResult != 0);
                }
            }
        }

        private async Task CopyDM(string id)
        {
            await LoadData(id);
            CurDmcc.Ma_cc_me = "";
            CurDmcc.Ngay_mua = DateTime.Now;
            CurDmcc.Ngay_kh0 = DateTime.Now;
            CurDmcc.Ngay_khc = null;

            // [ĐÃ SỬA]: Bỏ comment các dòng này để khi Copy KHÔNG bị mất dữ liệu cũ
            // CurDmcc.Ngay_kt_kh = null;
            // CurDmcc.Gt_da_kh = 0;
            // CurDmcc.Gt_cl = CurDmcc.Nguyen_gia;

            CurDmcc.Ngay_kh1 = null;
            CurDmcc.Ngay_giam = null;
            CurDmcc.Ma_giam_cc = "";
            CurDmcc.Ly_do_giam = "";

            int stt = 1;
            foreach (var ct in LstCtDmcc)
            {
                ct.Stt = stt++;
                ct.Ma_cc = "";
            }

            AllowEditMa = true;
            AllowEditMode = true;
        }
        // ---- LƯU ----

        public async Task SaveDM()
        {
            // 1. Dừng 200ms để nhường đường cho sự kiện LostFocus và các ô Textbox kịp Binding dữ liệu
            await Task.Delay(200);

            if (GridCt != null && GridCt.IsEditing())
                await GridCt.SaveChangesAsync();
            if (GridCtct != null && GridCtct.IsEditing())
                await GridCtct.SaveChangesAsync();

            // 2. Dừng thêm 100ms để hàm Grid_EditModelSavingCt có thời gian Add dòng mới vào LstCtDmcc
            await Task.Delay(100);

            // 3. Tiến hành kiểm tra dữ liệu
            if (!await CheckValid()) return;

            try
            {
                // Chuẩn hóa chuỗi
                CurDmcc.Ma_cc_me = CurDmcc.Ma_cc_me?.ToUpper().Trim();
                CurDmcc.Ten_cc = CurDmcc.Ten_cc?.Trim();
                CurDmcc.Ten_cc2 = string.IsNullOrWhiteSpace(CurDmcc.Ten_cc2) ? null : CurDmcc.Ten_cc2.Trim();
                CurDmcc.So_hieu_cc = string.IsNullOrWhiteSpace(CurDmcc.So_hieu_cc) ? null : CurDmcc.So_hieu_cc.Trim();
                CurDmcc.Dvt = CurDmcc.Dvt?.Trim() ?? "";
                CurDmcc.Ghi_chu = string.IsNullOrWhiteSpace(CurDmcc.Ghi_chu) ? null : CurDmcc.Ghi_chu.Trim();
                CurDmcc.So_bbbg = string.IsNullOrWhiteSpace(CurDmcc.So_bbbg) ? null : CurDmcc.So_bbbg.Trim();
                CurDmcc.Nuoc_sx = string.IsNullOrWhiteSpace(CurDmcc.Nuoc_sx) ? null : CurDmcc.Nuoc_sx.Trim();
                CurDmcc.Nh_cc = string.IsNullOrWhiteSpace(CurDmcc.Nh_cc) ? null : CurDmcc.Nh_cc.Trim();
                CurDmcc.Nh_cc1 = string.IsNullOrWhiteSpace(CurDmcc.Nh_cc1) ? null : CurDmcc.Nh_cc1.Trim();
                CurDmcc.Nh_cc2 = string.IsNullOrWhiteSpace(CurDmcc.Nh_cc2) ? null : CurDmcc.Nh_cc2.Trim();
                CurDmcc.Nh_cc3 = string.IsNullOrWhiteSpace(CurDmcc.Nh_cc3) ? null : CurDmcc.Nh_cc3.Trim();
                CurDmcc.Loai_cc = string.IsNullOrWhiteSpace(CurDmcc.Loai_cc) ? null : CurDmcc.Loai_cc.Trim();
                CurDmcc.Ma_tg_cc = string.IsNullOrWhiteSpace(CurDmcc.Ma_tg_cc) ? null : CurDmcc.Ma_tg_cc.Trim();
                CurDmcc.Tk_cc = string.IsNullOrWhiteSpace(CurDmcc.Tk_cc) ? null : CurDmcc.Tk_cc.Trim();
                CurDmcc.Tk_kh = CurDmcc.Tk_kh?.Trim() ?? "";
                CurDmcc.Tk_cp = CurDmcc.Tk_cp?.Trim() ?? "";
                CurDmcc.Ma_bpcc = string.IsNullOrWhiteSpace(CurDmcc.Ma_bpcc) ? null : CurDmcc.Ma_bpcc.Trim();
                CurDmcc.Ma_bpht = string.IsNullOrWhiteSpace(CurDmcc.Ma_bpht) ? null : CurDmcc.Ma_bpht.Trim();
                CurDmcc.Ma_vv = string.IsNullOrWhiteSpace(CurDmcc.Ma_vv) ? null : CurDmcc.Ma_vv.Trim();
                CurDmcc.Ma_px = string.IsNullOrWhiteSpace(CurDmcc.Ma_px) ? null : CurDmcc.Ma_px.Trim();
                CurDmcc.Ma_phi = string.IsNullOrWhiteSpace(CurDmcc.Ma_phi) ? null : CurDmcc.Ma_phi.Trim();
                CurDmcc.So_ct = string.IsNullOrWhiteSpace(CurDmcc.So_ct) ? null : CurDmcc.So_ct.Trim();
                CurDmcc.Ma_tra_cuu = string.IsNullOrWhiteSpace(CurDmcc.Ma_tra_cuu) ? null : CurDmcc.Ma_tra_cuu.Trim();
                CurDmcc.Ly_do_dc = string.IsNullOrWhiteSpace(CurDmcc.Ly_do_dc) ? null : CurDmcc.Ly_do_dc.Trim();
                CurDmcc.Cc_kt = string.IsNullOrWhiteSpace(CurDmcc.Cc_kt) ? null : CurDmcc.Cc_kt.Trim();

                // Chuẩn hóa ngày: chỉ lưu phần Date, không lưu giờ phút giây
                if (CurDmcc.Ngay_mua.HasValue) CurDmcc.Ngay_mua = CurDmcc.Ngay_mua.Value.Date;
                if (CurDmcc.Ngay_ct.HasValue) CurDmcc.Ngay_ct = CurDmcc.Ngay_ct.Value.Date;
                if (CurDmcc.Ngay_kh0.HasValue) CurDmcc.Ngay_kh0 = CurDmcc.Ngay_kh0.Value.Date;
                if (CurDmcc.Ngay_khc.HasValue) CurDmcc.Ngay_khc = CurDmcc.Ngay_khc.Value.Date;
                if (CurDmcc.Ngay_kt_kh.HasValue) CurDmcc.Ngay_kt_kh = CurDmcc.Ngay_kt_kh.Value.Date;
                if (CurDmcc.Ngay_bbbg.HasValue) CurDmcc.Ngay_bbbg = CurDmcc.Ngay_bbbg.Value.Date;
                if (CurDmcc.Ngay_dvsd.HasValue) CurDmcc.Ngay_dvsd = CurDmcc.Ngay_dvsd.Value.Date;
                if (CurDmcc.Ngay_dc.HasValue) CurDmcc.Ngay_dc = CurDmcc.Ngay_dc.Value.Date;

                CurDmcc.Kieu_kh = decimal.TryParse(CurDmcc.Kieu_kh_str, out decimal kk) ? kk : 1;

                // Tổng hợp từ lưới chi tiết
                if (LstCtDmcc.Count > 0)
                {
                    CurDmcc.Nguyen_gia = LstCtDmcc.Sum(x => x.Nguyen_gia ?? 0);
                    CurDmcc.Gt_da_kh = LstCtDmcc.Sum(x => x.Gt_da_kh ?? 0);
                    CurDmcc.Gt_cl = LstCtDmcc.Sum(x => x.Gt_cl ?? 0);
                    CurDmcc.Gt_kh_ky = LstCtDmcc.Sum(x => x.Gt_kh_ky ?? 0);
                    CurDmcc.Tong_sl = LstCtDmcc.Sum(x => x.Sl_cc ?? 0);
                }

                CurDmcc.Tinh_kh = (CurDmcc.Kieu_kh == 1 && CurDmcc.So_ky_kh > 0) ? 1 : 0;
                if (CurDmcc.Kieu_kh == 2 && (CurDmcc.Gt_cl ?? 0) > 0) CurDmcc.Tinh_kh = 1;
                if (CurDmcc.Nam_sx == 0) CurDmcc.Nam_sx = null;
                CurDmcc.Tien_cl     = CurDmcc.Tien_cl    ?? 0;
                CurDmcc.So_ky_kh    = CurDmcc.So_ky_kh   ?? 0;
                CurDmcc.Ty_le_kh    = CurDmcc.Ty_le_kh   ?? 0;
                CurDmcc.Tl_kh_nhanh = CurDmcc.Tl_kh_nhanh ?? 0;
                CurDmcc.So_ky_kh0   = CurDmcc.So_ky_kh0  ?? 0;
                CurDmcc.Gt_da_kh0   = CurDmcc.Gt_da_kh0  ?? 0;
                CurDmcc.Loai_pb     = CurDmcc.Loai_pb    ?? 0;

                DateTime now = DateTime.Now;
                string curTime = now.ToString("HH:mm:ss");
                int userId0 = curUser?.User_id ?? 0;
                string userNameStr = Escape(curUser?.User_name ?? "");

                if (IsNew || IsCopy)
                {
                    await InsertDmccSafe(now, curTime, userId0, userNameStr);
                    await myDb.ExecuteSqlQueryAsync($"UPDATE dmcc SET nam_sx = NULL WHERE ma_cc_me = N'{Escape(CurDmcc.Ma_cc_me)}' AND nam_sx = 0");
                    myStateMN.SetNewThongbao(string.Format(Lap["fxcc.msg.add_ok"], CurDmcc.Ma_cc_me));
                }
                else if (IsEdit)
                {
                    string safeOldKey = Escape(Ma_ma ?? CurDmcc.Ma_cc_me);
                    await UpdateDmccSafe(now, curTime, userId0, userNameStr, safeOldKey);
                    myStateMN.SetNewThongbao(string.Format(Lap["fxcc.msg.edit_ok"], CurDmcc.Ma_cc_me));
                }

                // Xóa & Insert lại chi tiết: Luôn dùng ma_cc_me mới
                string safeNewId = Escape(CurDmcc.Ma_cc_me);
                // Nếu Edit và ma_cc_me thay đổi, phải xóa cả mã cũ
                if (IsEdit && !string.IsNullOrEmpty(Ma_ma) && !Ma_ma.Equals(CurDmcc.Ma_cc_me))
                {
                    string safeOldId = Escape(Ma_ma);
                    // LƯU Ý: Phải xóa ctbpcc trước khi xóa ctdmcc vì nó link qua ma_cc
                    string sqlDelOld = string.Concat(
                        "DELETE FROM ctbpcc WHERE cc0 = '1' AND ma_cc IN (SELECT ma_cc FROM ctdmcc WHERE ma_cc_me = N'", safeOldId, "'); ",
                        "DELETE FROM ctdmcc WHERE ma_cc_me = N'", safeOldId, "'; ",
                        "DELETE FROM ctctcc WHERE ma_cc_me = N'", safeOldId, "'; "
                    );
                    await myDb.ExecuteSqlQueryAsync(sqlDelOld);
                }
                else
                {
                    string sqlDel = string.Concat(
                        "DELETE FROM ctbpcc WHERE cc0 = '1' AND ma_cc IN (SELECT ma_cc FROM ctdmcc WHERE ma_cc_me = N'", safeNewId, "'); ",
                        "DELETE FROM ctdmcc WHERE ma_cc_me = N'", safeNewId, "'; ",
                        "DELETE FROM ctctcc WHERE ma_cc_me = N'", safeNewId, "'; "
                    );
                    await myDb.ExecuteSqlQueryAsync(sqlDel);
                }

                string maCcMe = Escape(CurDmcc.Ma_cc_me);
                string sMaTgCc = Escape(CurDmcc.Ma_tg_cc ?? "");
                string sNgayTg = CurDmcc.Ngay_mua.HasValue ? string.Concat("'", CurDmcc.Ngay_mua.Value.ToString("yyyy-MM-dd"), "'") : "NULL";
                int nam = CurDmcc.Ngay_mua.HasValue ? CurDmcc.Ngay_mua.Value.Year : now.Year;
                int ky = CurDmcc.Ngay_mua.HasValue ? CurDmcc.Ngay_mua.Value.Month : now.Month;
                string sNgayKhH = CurDmcc.Ngay_kh0.HasValue ? string.Concat("'", CurDmcc.Ngay_kh0.Value.ToString("yyyy-MM-dd"), "'") : "NULL";
                string sTkCcH = Escape(CurDmcc.Tk_cc ?? "");
                string sTkKhH = Escape(CurDmcc.Tk_kh ?? "");
                string sTkCpH = Escape(CurDmcc.Tk_cp ?? "");
                string sMaBpccH = Escape(CurDmcc.Ma_bpcc ?? "");
                string sMaBphtH = Escape(CurDmcc.Ma_bpht ?? "");
                string sMaVvH = Escape(CurDmcc.Ma_vv ?? "");
                decimal tienCl = CurDmcc.Tien_cl ?? 0;
                string sNowDate = now.ToString("yyyy-MM-dd");
                string sUserId = (curUser?.User_id ?? 0).ToString();

                // Insert ctdmcc & ctbpcc
                foreach (var ct in LstCtDmcc)
                {
                    if (string.IsNullOrWhiteSpace(ct.Ma_cc))
                        continue;

                    string sMaCc = Escape(ct.Ma_cc ?? "");
                    string sMaBpcc = Escape(ct.Ma_bpcc ?? sMaBpccH);
                    string sTkCp = Escape(!string.IsNullOrWhiteSpace(ct.Tk_cp) ? ct.Tk_cp : CurDmcc.Tk_cp ?? "");
                    string sMaBpht = Escape(!string.IsNullOrWhiteSpace(ct.Ma_bpht) ? ct.Ma_bpht : CurDmcc.Ma_bpht ?? "");
                    string sMaVv = Escape(!string.IsNullOrWhiteSpace(ct.Ma_vv) ? ct.Ma_vv : CurDmcc.Ma_vv ?? "");
                    string sSoCt = Escape(ct.So_ct ?? "");
                    string sGhiChu = Escape(ct.Ghi_chu ?? "");
                    string sNgayCt = ct.Ngay_ct.HasValue ? string.Concat("'", ct.Ngay_ct.Value.ToString("yyyy-MM-dd"), "'") : "NULL";
                    string sl = Num(ct.Sl_cc ?? 1);
                    string ng = Num(ct.Nguyen_gia ?? 0);
                    string gtkh = Num(ct.Gt_da_kh ?? 0);
                    string gtcl = Num(ct.Gt_cl ?? 0);
                    string gtkhky = Num(ct.Gt_kh_ky ?? 0);
                    string sTienCl = Num(tienCl);

                    string sqlInsertCt = string.Concat(
                        "INSERT INTO ctdmcc(cc0,ma_cc_me,ma_cc,ngay_ct,so_ct,",
                        "ma_bpcc,tk_cp,ma_bpht,ma_vv,sl_cc,",
                        "nguyen_gia,gt_da_kh,gt_cl,gt_kh_ky,tien_cl,",
                        "ngay_kh,ngay_tg,ma_tg_cc,nam,ky,tang_giam,ghi_chu,",
                        "date0,time0,user_id0,date,time,user_id) VALUES(",
                        "'1',N'", maCcMe, "',N'", sMaCc, "',", sNgayCt, ",N'", sSoCt, "',",
                        "N'", sMaBpcc, "',N'", sTkCp, "',N'", sMaBpht, "',N'", sMaVv, "',", sl, ",",
                        ng, ",", gtkh, ",", gtcl, ",", gtkhky, ",", sTienCl, ",",
                        sNgayKhH, ",", sNgayTg, ",N'", sMaTgCc, "',", nam, ",", ky, ",1,N'", sGhiChu, "',",
                        "'", sNowDate, "','", curTime, "',", sUserId, ",",
                        "'", sNowDate, "','", curTime, "',", sUserId, ")"
                    );
                    await myDb.ExecuteSqlQueryAsync(sqlInsertCt);

                    if (!string.IsNullOrWhiteSpace(ct.Ma_cc))
                    {
                        // Sửa lỗi: Bỏ cột ma_cc_me khỏi câu lệnh INSERT của bảng ctbpcc
                        string sqlInsertBP = string.Concat(
                            "INSERT INTO ctbpcc(cc0,ma_cc,ngay_kh,tk_cc,tk_kh,tk_cp,ma_bpcc,ma_bpht,ma_vv,sl_cc,nam,ky,",
                            "date0,time0,user_id0,date,time,user_id) VALUES(",
                            "'1',N'", sMaCc, "',", sNgayKhH, ",N'", sTkCcH, "',N'", sTkKhH, "',N'", sTkCp, "',",
                            "N'", sMaBpcc, "',N'", sMaBpht, "',N'", sMaVv, "',", sl, ",", nam, ",", ky, ",",
                            "'", sNowDate, "','", curTime, "',", sUserId, ",",
                            "'", sNowDate, "','", curTime, "',", sUserId, ")"
                        );
                        await myDb.ExecuteSqlQueryAsync(sqlInsertBP);
                    }
                }

                // Insert ctctcc (phụ tùng)
                foreach (var pt in LstCtCtcc)
                {
                    if (string.IsNullOrWhiteSpace(pt.Ten_ptkt)) continue;
                    string sTen = Escape(pt.Ten_ptkt);
                    string sDvt = Escape(pt.Dvt ?? "");
                    string sSl = Num(pt.So_luong ?? 0);
                    string sGt = Num(pt.Gia_tri ?? 0);

                    string sqlPt = string.Concat(
                        "INSERT INTO ctctcc(cc0,ma_cc_me,ten_ptkt,dvt,so_luong,gia_tri) VALUES(",
                        "'1',N'", maCcMe, "',N'", sTen, "',N'", sDvt, "',", sSl, ",", sGt, ")"
                    );
                    await myDb.ExecuteSqlQueryAsync(sqlPt);
                }

                CloseForm();
            }
            catch (Exception ex)
            {
                // Hiển thị cả InnerException để dễ debug
                string msg = ex.Message;
                if (ex.InnerException != null) msg += " | Inner: " + ex.InnerException.Message;
                myStateMN.SetNewThongbao(string.Format(Lap["fxcc.err.save"], msg));
            }
        }

        // CloseForm() được định nghĩa trong FrmFxcc.razor @code block

        // ---- VALIDATE ----

        private async Task<bool> CheckValid()
        {
            if (string.IsNullOrEmpty(CurDmcc.Ma_cc_me))
            {
                myStateMN.SetNewThongbao(Lap["fxcc.err.no_ma"]);
                return false;
            }
            if (CurDmcc.Ma_cc_me.Contains(" "))
            {
                myStateMN.SetNewThongbao(Lap["fxcc.err.ma_space"]);
                return false;
            }
            bool isMaChanged = IsEdit && !string.IsNullOrEmpty(Ma_ma) && !Ma_ma.Equals(CurDmcc.Ma_cc_me, StringComparison.OrdinalIgnoreCase);

            if (IsNew || IsCopy || isMaChanged)
            {
                string sqlCheck = string.Concat("SELECT COUNT(*) FROM ", sqlTableName, " WHERE ", SqlTableKey, " = N'", Escape(CurDmcc.Ma_cc_me), "'");
                var ds = await myDb.LoadDataset(sqlCheck);
                if (ds != null && ds.Tables.Count > 0 && Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
                {
                    myStateMN.SetNewThongbao(Lap["fxcc.err.ma_exist"]);
                    return false;
                }
            }
            if (string.IsNullOrEmpty(CurDmcc.Ten_cc?.Trim()))
            {
                myStateMN.SetNewThongbao(Lap["fxcc.err.no_ten"]);
                return false;
            }
            if (string.IsNullOrEmpty(CurDmcc.Nh_cc?.Trim()))
            {
                myStateMN.SetNewThongbao(Lap["fxcc.err.no_nhom"]);
                return false;
            }
            if (string.IsNullOrEmpty(CurDmcc.Loai_cc?.Trim()))
            {
                myStateMN.SetNewThongbao(Lap["fxcc.err.no_loai"]);
                return false;
            }
            if (string.IsNullOrEmpty(CurDmcc.Ma_tg_cc?.Trim()))
            {
                myStateMN.SetNewThongbao(Lap["fxcc.err.no_ly_do"]);
                return false;
            }
            if (!CurDmcc.Ngay_mua.HasValue)
            {
                myStateMN.SetNewThongbao(Lap["fxcc.err.no_ngay_tang"]);
                return false;
            }
            if (string.IsNullOrEmpty(CurDmcc.Dvt?.Trim()))
            {
                myStateMN.SetNewThongbao(Lap["fxcc.err.no_dvt"]);
                return false;
            }
            if (!CurDmcc.Ngay_kh0.HasValue)
            {
                myStateMN.SetNewThongbao(Lap["fxcc.err.no_ngay_pb"]);
                return false;
            }

            var dateMua = new DateTime(CurDmcc.Ngay_mua.Value.Year, CurDmcc.Ngay_mua.Value.Month, 1);
            var dateKh = new DateTime(CurDmcc.Ngay_kh0.Value.Year, CurDmcc.Ngay_kh0.Value.Month, 1);
            if (dateKh < dateMua)
            {
                myStateMN.SetNewThongbao(Lap["fxcc.err.ngay_pb_invalid"]);
                return false;
            }
            if (string.IsNullOrEmpty(CurDmcc.Tk_kh?.Trim()))
            {
                myStateMN.SetNewThongbao(Lap["fxcc.err.no_tk_pb"]);
                return false;
            }
            if (string.IsNullOrEmpty(CurDmcc.Tk_cp?.Trim()))
            {
                myStateMN.SetNewThongbao(Lap["fxcc.err.no_tk_cp"]);
                return false;
            }

            LstCtDmcc.RemoveAll(x => string.IsNullOrEmpty(x.Ma_cc?.Trim()));
            if (LstCtDmcc.Count == 0)
            {
                myStateMN.SetNewThongbao(Lap["fxcc.err.no_ct"]);
                return false;
            }
            foreach (var ct in LstCtDmcc)
            {
                if (string.IsNullOrEmpty(ct.Ma_cc?.Trim()))
                {
                    myStateMN.SetNewThongbao(string.Format(Lap["fxcc.err.ct_no_ma"], ct.Stt));
                    return false;
                }
                if (string.IsNullOrEmpty(ct.Ma_bpcc?.Trim()))
                {
                    myStateMN.SetNewThongbao(string.Format(Lap["fxcc.err.ct_no_bp"], ct.Stt));
                    return false;
                }
            }

            var allMaCc = LstCtDmcc.Select(x => x.Ma_cc?.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (allMaCc.Distinct().Count() != allMaCc.Count)
            {
                myStateMN.SetNewThongbao(Lap["fxcc.err.ct_dup_ma"]);
                return false;
            }

            LstCtCtcc.RemoveAll(x => string.IsNullOrEmpty(x.Ten_ptkt?.Trim()));

            // [MỚI] Validate không cho nhập trùng thẻ công cụ của công cụ khác
            if (!await CheckToolCardDuplicate())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Kiểm tra không cho nhập trùng thẻ công cụ (Ma_cc) với các công cụ khác.
        /// Tham khảo logic từ WinForm: CheckFxcc_ma_cc stored procedure
        /// </summary>
        private async Task<bool> CheckToolCardDuplicate()
        {
            // Lấy danh sách mã thẻ công cụ có giá trị từ lưới (loại bỏ các dòng chưa nhập)
            var allMaCc = LstCtDmcc
                .Select(x => x.Ma_cc?.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();

            if (allMaCc.Count == 0)
                return true;

            // Xây dựng chuỗi IN clause với proper escaping
            // Ví dụ: N'1111', N'2222'
            string inClause = string.Join(", ", allMaCc.Select(x => $"N'{Escape(x)}'"));

            // Tham số: Ma_cc_me của công cụ hiện tại
            string maCcMe = Escape(CurDmcc.Ma_cc_me?.Trim() ?? "");

            // Xây dựng SQL query với proper parameterization
            string sql = $@"
                SELECT ma_cc_me, ma_cc 
                FROM ctdmcc 
                WHERE ma_cc IN ({inClause})
                  AND ma_cc_me <> N'{maCcMe}'
            ";

            try
            {
                var ds = await myDb.LoadDataset(sql);
                
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // Có dữ liệu trùng -> Lỗi
                    string conflictTools = string.Join(", ", 
                        ds.Tables[0].Rows.Cast<DataRow>()
                            .GroupBy(r => r["ma_cc_me"].ToString())
                            .Select(g => g.Key));
                    
                    myStateMN.SetNewThongbao(
                        string.Format(Lap["fxcc.err.tool_card_dup"], conflictTools));
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao($"Error checking tool card duplicate: {ex.Message}");
                return false;
            }
        }

        // ---- KIỂU PHÂN BỔ & TÍNH TOÁN ----

        private void OnKieuKhChanged(string v)
        {
            CurDmcc.Kieu_kh_str = v;
            CurDmcc.Kieu_kh = decimal.TryParse(v, out decimal kk) ? kk : 1;

            // Nếu phân bổ 2 lần -> Bắt buộc gán số kỳ = 2
            if (CurDmcc.Kieu_kh_str == "2")
            {
                CurDmcc.So_ky_kh = 2;
            }

            CalculateEndDate();
            RecalculateAllCtGrid();
        }

        private void CalculateEndDate()
        {
            if (CurDmcc.Kieu_kh_str == "1" && CurDmcc.Ngay_kh0.HasValue && (CurDmcc.So_ky_kh ?? 0) > 0)
                CurDmcc.Ngay_kt_kh = CurDmcc.Ngay_kh0.Value.AddMonths((int)CurDmcc.So_ky_kh.Value).AddDays(-1);
            else if (CurDmcc.Kieu_kh_str == "2" && CurDmcc.Ngay_kh0.HasValue)
                CurDmcc.Ngay_kt_kh = CurDmcc.Ngay_kh0.Value.AddMonths(2).AddDays(-1);
            else
                CurDmcc.Ngay_kt_kh = null;
        }

        private void RecalculateAllCtGrid()
        {
            // Tính trên Header
            decimal ngHeader = CurDmcc.Nguyen_gia ?? 0;
            decimal gtkhHeader = CurDmcc.Gt_da_kh ?? 0;
            CurDmcc.Gt_cl = ngHeader - gtkhHeader;

            if (CurDmcc.Kieu_kh_str == "2")
            {
                CurDmcc.Gt_kh_ky = SysRound(ngHeader / 2m);
            }
            else
            {
                decimal soKy = CurDmcc.So_ky_kh ?? 0;
                CurDmcc.Gt_kh_ky = soKy > 0 ? SysRound(ngHeader / soKy) : 0;
            }

            // Kích hoạt tính lại cho toàn bộ dòng trên lưới
            foreach (var ct in LstCtDmcc)
            {
                CalculateCtRow(ct);
            }
            StateHasChanged();
        }

        private void CalculateCtRow(CtDmcc ct)
        {
            decimal ng = ct.Nguyen_gia ?? 0;
            decimal gtkh = ct.Gt_da_kh ?? 0;

            // [ĐÃ SỬA]: Luôn áp dụng công thức GT còn lại = Nguyên giá - GT đã phân bổ cho MỌI TRƯỜNG HỢP
            ct.Gt_cl = ng - gtkh;

            // Kiểu phân bổ chỉ tác động đến GT phân bổ 1 kỳ
            if (CurDmcc.Kieu_kh_str == "2")
            {
                ct.Gt_kh_ky = SysRound(ng / 2m);
            }
            else
            {
                decimal soKy = CurDmcc.So_ky_kh ?? 0;
                ct.Gt_kh_ky = soKy > 0 ? SysRound(ng / soKy) : 0;
            }
        }

        // Round theo chuẩn winform (M_ROUND = 0 tức làm tròn đến đơn vị)
        private static decimal SysRound(decimal v)
            => Math.Round(v, 0, MidpointRounding.AwayFromZero);

        private void UpdateMasterTotals()
        {
            // [ĐÃ SỬA]: Do Header lưu Đơn giá chuẩn, tuyệt đối KHÔNG cộng dồn tiền từ lưới lên Header nữa!
            // Chỉ cộng tổng số lượng công cụ.
            if (LstCtDmcc.Count > 0)
            {
                CurDmcc.Tong_sl = LstCtDmcc.Sum(x => x.Sl_cc ?? 0);
            }
            else
            {
                CurDmcc.Tong_sl = 0;
            }
            StateHasChanged();
        }

        // ---- EVENT HANDLERS LƯỚI CT ----

        private void OnCtGrid_BpccChanged(ExpandoObject v, CtDmcc item)
        {
            if (v == null) return;
            var d = v as IDictionary<string, object>;
            if (d != null && d.ContainsKey("ten_bpts"))
                item.Ten_bpcc = d["ten_bpts"]?.ToString();
        }

        private void OnCtGrid_SlChanged(decimal? v, CtDmcc item)
        {
            item.Sl_cc = v;
            decimal sl = v ?? 0;

            // [ĐÃ SỬA]: Giá trị lưới = Đơn giá (Header) * Số lượng (Lưới)
            item.Nguyen_gia = sl * (CurDmcc.Nguyen_gia ?? 0);
            item.Gt_da_kh = sl * (CurDmcc.Gt_da_kh ?? 0);
            item.Gt_cl = item.Nguyen_gia - item.Gt_da_kh; // GT còn lại = Nguyên giá - GT đã pb
            item.Gt_kh_ky = sl * (CurDmcc.Gt_kh_ky ?? 0);

            UpdateMasterTotals();
        }

        private void OnCtGrid_NguyenGiaChanged(decimal? v, CtDmcc item)
        {
            item.Nguyen_gia = v;
            CalculateCtRow(item);
            UpdateMasterTotals();
        }

        private void OnCtGrid_GtDaKhChanged(decimal? v, CtDmcc item)
        {
            item.Gt_da_kh = v;
            CalculateCtRow(item);
            UpdateMasterTotals();
        }

        /// <summary>
        /// Đồng bộ các trường từ header xuống TẤT Cᡂ dòng CT chưa có ma_cc.
        /// Nếu dòng đã có ma_cc (tức đã nhập) thì chỉ đồng bộ các trường không ảnh hưởng đến giá trị riêng.
        /// </summary>
        private void SyncHeaderToFirstCtRow()
        {
            // 1. Tính toán lại trên Header trước (Không ép cứng Gt_da_kh nữa)
            decimal ngHeader = CurDmcc.Nguyen_gia ?? 0;
            decimal gtkhHeader = CurDmcc.Gt_da_kh ?? 0;

            CurDmcc.Gt_cl = ngHeader - gtkhHeader; // Công thức luôn đúng

            if (CurDmcc.Kieu_kh_str == "2")
            {
                CurDmcc.Gt_kh_ky = SysRound(ngHeader / 2m);
            }
            else
            {
                decimal soKy = CurDmcc.So_ky_kh ?? 0;
                CurDmcc.Gt_kh_ky = soKy > 0 ? SysRound(ngHeader / soKy) : 0;
            }

            // 2. Đẩy xuống lưới: Lấy Đơn giá * Số lượng của dòng đó
            foreach (var ct in LstCtDmcc)
            {
                ct.Ngay_ct = CurDmcc.Ngay_ct ?? CurDmcc.Ngay_mua ?? DateTime.Now;
                ct.So_ct = CurDmcc.So_ct;
                ct.Tk_cp = CurDmcc.Tk_cp;
                ct.Ma_bpht = CurDmcc.Ma_bpht;
                ct.Ma_px = CurDmcc.Ma_px;
                ct.Ma_vv = CurDmcc.Ma_vv;

                // Chỉ chạy công thức cho dòng mới chưa lưu mã thẻ
                if (string.IsNullOrEmpty(ct.Ma_cc?.Trim()))
                {
                    decimal sl = ct.Sl_cc ?? 1;
                    ct.Nguyen_gia = sl * ngHeader;
                    ct.Gt_da_kh = sl * gtkhHeader;
                    ct.Gt_cl = ct.Nguyen_gia - ct.Gt_da_kh; // Tính luôn cho dòng lưới
                    ct.Gt_kh_ky = sl * (CurDmcc.Gt_kh_ky ?? 0);
                }
            }
            StateHasChanged();
        }
        /// <summary>
        /// Khi người dùng sửa GT p.bổ 1 kỳ trực tiếp ở header,
        /// cập nhật xuống tất cả dòng CT chưa có ma_cc.
        /// </summary>
        private void SyncHeaderGtKhKyToCtRow()
        {
            foreach (var ct in LstCtDmcc)
            {
                if (string.IsNullOrEmpty(ct.Ma_cc?.Trim()))
                    ct.Gt_kh_ky = CurDmcc.Gt_kh_ky;
            }
            StateHasChanged();
        }

        // ---- GRID CT CRUD ----

        private void Grid_CustomizeEditModelCt(GridCustomizeEditModelEventArgs e)
        {
            if (e.IsNew)
            {
                var item = (CtDmcc)e.EditModel;
                item.Stt      = LstCtDmcc.Count + 1;
                item.Cc0      = 1;
                item.Sl_cc    = 1;
                item.Ngay_ct  = CurDmcc.Ngay_ct ?? CurDmcc.Ngay_mua ?? DateTime.Now;
                item.So_ct    = CurDmcc.So_ct;
                item.Tk_cp    = CurDmcc.Tk_cp;
                item.Ma_bpht  = CurDmcc.Ma_bpht;
                item.Ma_px    = CurDmcc.Ma_px;
                item.Ma_vv    = CurDmcc.Ma_vv;
                item.Nguyen_gia = CurDmcc.Nguyen_gia;
                item.Gt_da_kh   = CurDmcc.Gt_da_kh;
                item.Gt_cl      = CurDmcc.Gt_cl;
                item.Gt_kh_ky   = CurDmcc.Gt_kh_ky;
            }
            else
            {
                CalculateCtRow((CtDmcc)e.EditModel);
            }
        }

        private async Task Grid_EditModelSavingCt(GridEditModelSavingEventArgs e)
        {
            var edit = (CtDmcc)e.EditModel;
            if (e.IsNew && string.IsNullOrWhiteSpace(edit.Ma_cc)) return;

            // Tính lại gt_cl và gt_kh_ky trước khi lưu
            edit.Sl_cc = edit.Sl_cc ?? 1;
            CalculateCtRow(edit);

            if (e.IsNew)
            {
                var newItem = new CtDmcc();
                CopyCtDmcc(edit, newItem);
                newItem.Stt = LstCtDmcc.Count + 1;
                LstCtDmcc.Add(newItem);
                // Sau khi đã thêm dòng mới hợp lệ, mới xóa các dòng placeholder (chưa có ma_cc)
                // nhưng chỉ xóa nếu còn ít nhất 1 dòng có ma_cc
                if (LstCtDmcc.Any(x => !string.IsNullOrEmpty(x.Ma_cc?.Trim()) && x != newItem))
                    LstCtDmcc.RemoveAll(x => string.IsNullOrEmpty(x.Ma_cc?.Trim()) && x != newItem);
                SelectedCtDmcc = newItem;
            }
            else
            {
                var dataItem = (CtDmcc)e.DataItem;
                CopyCtDmcc(edit, dataItem);
                SelectedCtDmcc = dataItem;
            }

            // Cập nhật lại STT
            int stt = 1;
            LstCtDmcc.ForEach(x => x.Stt = stt++);

            UpdateMasterTotals();
            await InvokeAsync(StateHasChanged);
        }

        private void Grid_DataItemDeletingCt(GridDataItemDeletingEventArgs e)
        {
            LstCtDmcc.Remove((CtDmcc)e.DataItem);
            int stt = 1;
            LstCtDmcc.ForEach(x => x.Stt = stt++);
            UpdateMasterTotals();
        }

        private void CopyCtDmcc(CtDmcc src, CtDmcc dst)
        {
            dst.Ma_cc     = src.Ma_cc;
            dst.Ngay_ct   = src.Ngay_ct;
            dst.So_ct     = src.So_ct;
            dst.Ma_bpcc   = src.Ma_bpcc;
            dst.Ten_bpcc  = src.Ten_bpcc;
            dst.Sl_cc     = src.Sl_cc;
            dst.Nguyen_gia = src.Nguyen_gia;
            dst.Gt_da_kh  = src.Gt_da_kh;
            dst.Gt_cl     = src.Gt_cl;
            dst.Gt_kh_ky  = src.Gt_kh_ky;
            dst.Tk_cp     = src.Tk_cp;
            dst.Ma_bpht   = src.Ma_bpht;
            dst.Ma_px     = src.Ma_px;
            dst.Ma_vv     = src.Ma_vv;
            dst.Ghi_chu   = src.Ghi_chu;
            dst.Cc0       = src.Cc0;
        }

        private async Task StartEditCtByClick(GridRowClickEventArgs e)
        {
            if (!AllowEditMode) return;
            LastFocusedGrid = "CT"; // Lưu vết đang nhập Chi tiết
            if (GridCt.IsEditing()) await GridCt.SaveChangesAsync();
            var item = GridCt.GetDataItem(e.VisibleIndex);
            if (item != null) await GridCt.StartEditDataItemAsync(item);
        }

        //private async Task GridCt_OnKeyDown(KeyboardEventArgs e)
        //{
        //    if (!AllowEditMode) return;
        //    switch (e.Key)
        //    {
        //        case "Enter":
        //            await GridCt.SaveChangesAsync();
        //            await Task.Delay(80);
        //            if (SelectedCtDmcc is CtDmcc cur)
        //            {
        //                int idx = LstCtDmcc.IndexOf(cur);
        //                if (idx >= 0 && idx < LstCtDmcc.Count - 1)
        //                    await GridCt.StartEditDataItemAsync(LstCtDmcc[idx + 1]);
        //                else
        //                    await GridCt.StartEditNewRowAsync();
        //            }
        //            break;
        //        case "F4":
        //            await GridCt.StartEditNewRowAsync();
        //            break;
        //        case "F8":
        //            if (SelectedCtDmcc != null)
        //                GridCt.ShowDataItemDeleteConfirmation(SelectedCtDmcc);
        //            break;
        //        case "Escape":
        //            try { await GridCt.CancelEditAsync(); } catch { }
        //            break;
        //    }
        //}

        async void OnGridCtLostFocus(FocusEventArgs e)
        {
            try
            {
                // [ĐÃ SỬA]: Chờ 150ms để nhường đường cho sự kiện Click của nút Lưu trên Form chạy trước
                await Task.Delay(150);

                bool inside = await JavacriptS.InvokeAsync<bool>("focusInside", "grid-wrapper-ct");
                if (!inside && GridCt != null && GridCt.IsEditing())
                    await GridCt.SaveChangesAsync();
            }
            catch { }
        }

        // ---- GRID CTCT CRUD ----

        private void Grid_CustomizeEditModelCtct(GridCustomizeEditModelEventArgs e)
        {
            if (e.IsNew)
            {
                var item = (CtCtcc)e.EditModel;
                item.Stt = LstCtCtcc.Count + 1;
                item.Cc0 = 1;
            }
        }

        private async Task Grid_EditModelSavingCtct(GridEditModelSavingEventArgs e)
        {
            var edit = (CtCtcc)e.EditModel;
            if (e.IsNew && string.IsNullOrWhiteSpace(edit.Ten_ptkt)) return;

            if (e.IsNew)
            {
                var newItem = new CtCtcc
                {
                    Stt = LstCtCtcc.Count + 1,
                    Cc0 = 1,
                    Ten_ptkt = edit.Ten_ptkt,
                    Dvt = edit.Dvt,
                    So_luong = edit.So_luong,
                    Gia_tri = edit.Gia_tri
                };
                LstCtCtcc.Add(newItem);
                SelectedCtCtcc = newItem;
            }
            else
            {
                var dataItem = (CtCtcc)e.DataItem;
                dataItem.Ten_ptkt = edit.Ten_ptkt;
                dataItem.Dvt = edit.Dvt;
                dataItem.So_luong = edit.So_luong;
                dataItem.Gia_tri = edit.Gia_tri;
                SelectedCtCtcc = dataItem;
            }
            await InvokeAsync(StateHasChanged);
        }

        private void Grid_DataItemDeletingCtct(GridDataItemDeletingEventArgs e)
        {
            LstCtCtcc.Remove((CtCtcc)e.DataItem);
            int stt = 1;
            LstCtCtcc.ForEach(x => x.Stt = stt++);
        }

        private async Task StartEditCtctByClick(GridRowClickEventArgs e)
        {
            if (!AllowEditMode) return;
            LastFocusedGrid = "CTCT"; // Lưu vết đang nhập Phụ tùng
            if (GridCtct.IsEditing()) await GridCtct.SaveChangesAsync();
            var item = GridCtct.GetDataItem(e.VisibleIndex);
            if (item != null) await GridCtct.StartEditDataItemAsync(item);
        }

        //private async Task GridCtct_OnKeyDown(KeyboardEventArgs e)
        //{
        //    if (!AllowEditMode) return;
        //    switch (e.Key)
        //    {
        //        case "Enter":
        //            await GridCtct.SaveChangesAsync();
        //            await Task.Delay(80);
        //            if (SelectedCtCtcc is CtCtcc cur)
        //            {
        //                int idx = LstCtCtcc.IndexOf(cur);
        //                if (idx >= 0 && idx < LstCtCtcc.Count - 1)
        //                    await GridCtct.StartEditDataItemAsync(LstCtCtcc[idx + 1]);
        //                else
        //                    await GridCtct.StartEditNewRowAsync();
        //            }
        //            break;
        //        case "F4":
        //            await GridCtct.StartEditNewRowAsync();
        //            break;
        //        case "F8":
        //            if (SelectedCtCtcc != null)
        //                GridCtct.ShowDataItemDeleteConfirmation(SelectedCtCtcc);
        //            break;
        //        case "Escape":
        //            try { await GridCtct.CancelEditAsync(); } catch { }
        //            break;
        //    }
        //}

        async void OnGridCtctLostFocus(FocusEventArgs e)
        {
            try
            {
                // [ĐÃ SỬA]: Chờ 150ms để nhường đường cho sự kiện Click của nút Lưu trên Form chạy trước
                await Task.Delay(150);

                bool inside = await JavacriptS.InvokeAsync<bool>("focusInside", "grid-wrapper-ctct");
                if (!inside && GridCtct != null && GridCtct.IsEditing())
                    await GridCtct.SaveChangesAsync();
            }
            catch { }
        }

        // ---- HELPER ----

        /// <summary>Escape single-quote trong SQL string.</summary>
        private static string Escape(string s) => (s ?? "").Replace("'", "''");

        /// <summary>Format số thập phân theo chuẩn SQL (InvariantCulture).</summary>
        private static string Num(decimal v) => v.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Lấy danh sách cột thực tế của bảng từ DB (giống winform dùng TableFields).
        /// Cache lại để tránh query nhiều lần.
        /// </summary>
        private HashSet<string> _dmccColumns;
        private async Task<HashSet<string>> GetDmccColumns()
        {
            if (_dmccColumns != null) return _dmccColumns;
            var ds = await myDb.LoadDataset(
                $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{sqlTableName}'");
            _dmccColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (ds?.Tables.Count > 0)
                foreach (DataRow r in ds.Tables[0].Rows)
                    _dmccColumns.Add(r[0].ToString());
            return _dmccColumns;
        }

        /// <summary>
        /// Xây dựng dictionary col→value cho tất cả các trường của Dmcc,
        /// sau đó chỉ giữ lại cột nào thực sự có trong bảng DB.
        /// </summary>
        private Dictionary<string, string> BuildDmccColValues(DateTime now, string curTime, int userId0, string userNameStr, bool isInsert)
        {
            string FmtDate(DateTime? d) => d.HasValue ? $"'{d.Value:yyyy-MM-dd}'" : "NULL";
            string FmtNum(decimal? v)   => Num(v ?? 0);

            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["ma_cc_me"]   = $"N'{Escape(CurDmcc.Ma_cc_me ?? "")}'",
                ["ma_tra_cuu"] = $"N'{Escape(CurDmcc.Ma_tra_cuu ?? "")}'",
                ["ten_cc"]     = $"N'{Escape(CurDmcc.Ten_cc ?? "")}'",
                ["ten_cc2"]    = $"N'{Escape(CurDmcc.Ten_cc2 ?? "")}'",
                ["so_hieu_cc"] = $"N'{Escape(CurDmcc.So_hieu_cc ?? "")}'",
                ["ma_dvcs"]    = $"N'{Escape(CurDmcc.Ma_dvcs ?? "")}'",
                ["dvt"]        = $"N'{Escape(CurDmcc.Dvt ?? "")}'",
                ["nuoc_sx"]    = $"N'{Escape(CurDmcc.Nuoc_sx ?? "")}'",
                ["nam_sx"]     = (CurDmcc.Nam_sx.HasValue && CurDmcc.Nam_sx > 0) ? FmtNum(CurDmcc.Nam_sx) : "NULL",
                ["nh_cc"]      = $"N'{Escape(CurDmcc.Nh_cc ?? "")}'",
                ["nh_cc1"]     = $"N'{Escape(CurDmcc.Nh_cc1 ?? "")}'",
                ["nh_cc2"]     = $"N'{Escape(CurDmcc.Nh_cc2 ?? "")}'",
                ["nh_cc3"]     = $"N'{Escape(CurDmcc.Nh_cc3 ?? "")}'",
                ["tinh_kh"]    = FmtNum(CurDmcc.Tinh_kh),
                ["ma_tg_cc"]   = $"N'{Escape(CurDmcc.Ma_tg_cc ?? "")}'",
                ["ngay_mua"]   = FmtDate(CurDmcc.Ngay_mua),
                ["ngay_ct"]    = FmtDate(CurDmcc.Ngay_ct),
                ["ngay_kh0"]   = FmtDate(CurDmcc.Ngay_kh0),
                ["ngay_khc"]   = FmtDate(CurDmcc.Ngay_khc),
                ["ngay_kt_kh"] = FmtDate(CurDmcc.Ngay_kt_kh),
                ["ngay_kh1"]   = FmtDate(CurDmcc.Ngay_kh1),
                ["so_ky_kh"]   = FmtNum(CurDmcc.So_ky_kh),
                ["tien_cl"]    = FmtNum(CurDmcc.Tien_cl),
                ["kieu_kh"]    = FmtNum(CurDmcc.Kieu_kh),
                ["tong_sl"]    = FmtNum(CurDmcc.Tong_sl),
                ["ma_bpcc"]    = $"N'{Escape(CurDmcc.Ma_bpcc ?? "")}'",
                ["ma_bpht"]    = $"N'{Escape(CurDmcc.Ma_bpht ?? "")}'",
                ["tk_cc"]      = $"N'{Escape(CurDmcc.Tk_cc ?? "")}'",
                ["tk_kh"]      = $"N'{Escape(CurDmcc.Tk_kh ?? "")}'",
                ["tk_cp"]      = $"N'{Escape(CurDmcc.Tk_cp ?? "")}'",
                ["loai_cc"]    = $"N'{Escape(CurDmcc.Loai_cc ?? "")}'",
                ["cc_kt"]      = $"N'{Escape(CurDmcc.Cc_kt ?? "")}'",
                ["ngay_dc"]    = FmtDate(CurDmcc.Ngay_dc),
                ["ly_do_dc"]   = $"N'{Escape(CurDmcc.Ly_do_dc ?? "")}'",
                ["ngay_giam"]  = FmtDate(CurDmcc.Ngay_giam),
                ["ma_giam_cc"] = $"N'{Escape(CurDmcc.Ma_giam_cc ?? "")}'",
                ["ly_do_giam"] = $"N'{Escape(CurDmcc.Ly_do_giam ?? "")}'",
                ["ghi_chu"]    = $"N'{Escape(CurDmcc.Ghi_chu ?? "")}'",
                ["status"]     = $"N'{Escape(CurDmcc.Status ?? "")}'",
                ["nguyen_gia"] = FmtNum(CurDmcc.Nguyen_gia),
                ["gt_da_kh"]   = FmtNum(CurDmcc.Gt_da_kh),
                ["gt_da_kh0"]  = FmtNum(CurDmcc.Gt_da_kh0),
                ["gt_cl"]      = FmtNum(CurDmcc.Gt_cl),
                ["gt_kh_ky"]   = FmtNum(CurDmcc.Gt_kh_ky),
                ["ty_le_kh"]   = FmtNum(CurDmcc.Ty_le_kh),
                ["tl_kh_nhanh"]= FmtNum(CurDmcc.Tl_kh_nhanh),
                ["so_ky_kh0"]  = FmtNum(CurDmcc.So_ky_kh0),
                ["loai_pb"]    = FmtNum(CurDmcc.Loai_pb),
                ["ma_vv"]      = $"N'{Escape(CurDmcc.Ma_vv ?? "")}'",
                ["so_bbbg"]    = $"N'{Escape(CurDmcc.So_bbbg ?? "")}'",
                ["ngay_bbbg"]  = FmtDate(CurDmcc.Ngay_bbbg),
                ["ly_do_thoi"] = $"N'{Escape(CurDmcc.Ly_do_thoi ?? "")}'",
                ["ma_phi"]     = $"N'{Escape(CurDmcc.Ma_phi ?? "")}'",
                ["ma_px"]      = $"N'{Escape(CurDmcc.Ma_px ?? "")}'",
                ["search"]     = $"N'{Escape(CurDmcc.Search ?? "")}'",
                ["ngay_dvsd"]  = FmtDate(CurDmcc.Ngay_dvsd),
                ["so_ct"]      = $"N'{Escape(CurDmcc.So_ct ?? "")}'",
                ["ngay_hong"]  = FmtDate(CurDmcc.Ngay_hong),
                ["so_cth"]     = $"N'{Escape(CurDmcc.So_cth ?? "")}'",
                ["ly_do_hong"] = $"N'{Escape(CurDmcc.Ly_do_hong ?? "")}'",
                ["ma_td1"]     = $"N'{Escape(CurDmcc.Ma_td1 ?? "")}'",
                ["ma_td2"]     = $"N'{Escape(CurDmcc.Ma_td2 ?? "")}'",
                ["ma_td3"]     = $"N'{Escape(CurDmcc.Ma_td3 ?? "")}'",
                ["ngay_td1"]   = FmtDate(CurDmcc.Ngay_td1),
                ["ngay_td2"]   = FmtDate(CurDmcc.Ngay_td2),
                ["ngay_td3"]   = FmtDate(CurDmcc.Ngay_td3),
                ["gc_td1"]     = $"N'{Escape(CurDmcc.Gc_td1 ?? "")}'",
                ["gc_td2"]     = $"N'{Escape(CurDmcc.Gc_td2 ?? "")}'",
                ["gc_td3"]     = $"N'{Escape(CurDmcc.Gc_td3 ?? "")}'",
                ["sl_td1"]     = FmtNum(CurDmcc.Sl_td1),
                ["sl_td2"]     = FmtNum(CurDmcc.Sl_td2),
                ["sl_td3"]     = FmtNum(CurDmcc.Sl_td3),
                ["date"]       = $"'{now:yyyy-MM-dd}'",
                ["time"]       = $"'{curTime}'",
                ["user_id"]    = userId0.ToString(),
                ["user_name"]  = $"N'{userNameStr}'",
            };

            if (isInsert)
            {
                map["date0"]      = $"'{now:yyyy-MM-dd}'";
                map["time0"]      = $"'{curTime}'";
                map["user_id0"]   = userId0.ToString();
                map["user_name0"] = $"N'{userNameStr}'";
            }

            return map;
        }

        private async Task InsertDmccSafe(DateTime now, string curTime, int userId0, string userNameStr)
        {
            var cols = await GetDmccColumns();
            var all  = BuildDmccColValues(now, curTime, userId0, userNameStr, isInsert: true);
            // Chỉ lấy cột thực có trong bảng, bỏ cột id (identity)
            var valid = all.Where(kv => cols.Contains(kv.Key) &&
                                        !kv.Key.Equals("id", StringComparison.OrdinalIgnoreCase))
                           .ToList();
            string colList = string.Join(",", valid.Select(kv => kv.Key));
            string valList = string.Join(",", valid.Select(kv => kv.Value));
            await myDb.ExecuteSqlQueryAsync($"INSERT INTO {sqlTableName}({colList}) VALUES({valList})");
        }

        private async Task UpdateDmccSafe(DateTime now, string curTime, int userId0, string userNameStr, string safeOldKey)
        {
            var cols = await GetDmccColumns();
            var all  = BuildDmccColValues(now, curTime, userId0, userNameStr, isInsert: false);
            // Bỏ cột khóa chính và các cột audit gốc khi update
            var skipOnUpdate = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                { "id", "date0", "time0", "user_id0", "user_name0" };
            var valid = all.Where(kv => cols.Contains(kv.Key) && !skipOnUpdate.Contains(kv.Key))
                           .ToList();
            string setClause = string.Join(",", valid.Select(kv => $"{kv.Key}={kv.Value}"));
            await myDb.ExecuteSqlQueryAsync(
                $"UPDATE {sqlTableName} SET {setClause} WHERE ma_cc_me = N'{safeOldKey}'");
        }

        //[JSInvokable]
        //public void CallGridEditMode(string value)
        //{
        //    if (AllowEditMode)
        //    {
        //        // Kiểm tra ưu tiên giá trị JS truyền vào (nếu có), nếu không có thì dùng biến lưu vết
        //        if (value == "grid-wrapper-ctct" || (string.IsNullOrEmpty(value) && LastFocusedGrid == "CTCT"))
        //        {
        //            _ = GridCtct?.StartEditNewRowAsync();
        //        }
        //        else
        //        {
        //            _ = GridCt?.StartEditNewRowAsync();
        //        }
        //        StateHasChanged();
        //    }
        //}
        // 1. Hàm dùng chung bắt sự kiện KeyDown
        async Task HandleGridKeyDown<T>(KeyboardEventArgs e, DxGrid grid, List<T> list, object selectedItem,
                                        Func<Task> onEnterAction, Action onAddAction)
        {
            if (!AllowEditMode) return;
            switch (e.Key)
            {
                case "Enter":
                    await onEnterAction(); // Gọi hàm Save & Next
                    break;
                case "F4":
                    onAddAction();
                    break;
                case "F8":
                    if (selectedItem != null) grid.ShowDataItemDeleteConfirmation(selectedItem);
                    break;
                case "Escape":
                    try { await grid.CancelEditAsync(); grid.ClearSelection(); } catch { }
                    break;
            }
        }

        // 2. Hàm dùng chung xử lý Lưu và Xuống dòng
        async Task HandleSaveAndMoveNext<T>(DxGrid grid, List<T> list, object selectedItem, Action onAddAction)
        {
            if (grid != null && grid.IsEditing())
                await grid.SaveChangesAsync();

            await Task.Delay(100); // Bắt buộc delay để lưới render xong

            if (selectedItem != null && selectedItem is T currentItem)
            {
                var index = list.IndexOf(currentItem);
                // Nếu chưa phải dòng cuối -> Xuống dòng kế tiếp
                if (index >= 0 && index < list.Count - 1)
                    await grid.StartEditDataItemAsync(list[index + 1]);
                else
                    onAddAction(); // Dòng cuối -> Thêm dòng mới
            }
            else
            {
                onAddAction();
            }
        }
        // BẮT BUỘC TÊN HÀM NÀY PHẢI CHUẨN ĐỂ JAVASCRIPT GỌI
        [JSInvokable]
        public async Task SaveAndMoveNextRow()
        {
            await HandleSaveAndMoveNext(GridCt, LstCtDmcc, SelectedCtDmcc, AddNewCt);
        }

        async void AddNewCt()
        {
            if (AllowEditMode)
            {
                if (GridCt != null && GridCt.IsEditing()) await GridCt.SaveChangesAsync();
                await GridCt.StartEditNewRowAsync();
            }
        }

        private async Task GridCt_OnKeyDown(KeyboardEventArgs e)
        {
            await HandleGridKeyDown(e, GridCt, LstCtDmcc, SelectedCtDmcc, SaveAndMoveNextRow, AddNewCt);
        }
        [JSInvokable]
        public async Task SaveAndMoveNextRowPhuTung()
        {
            await HandleSaveAndMoveNext(GridCtct, LstCtCtcc, SelectedCtCtcc, AddNewCtct);
        }

        async void AddNewCtct()
        {
            if (AllowEditMode)
            {
                if (GridCtct != null && GridCtct.IsEditing()) await GridCtct.SaveChangesAsync();
                await GridCtct.StartEditNewRowAsync();
            }
        }

        private async Task GridCtct_OnKeyDown(KeyboardEventArgs e)
        {
            await HandleGridKeyDown(e, GridCtct, LstCtCtcc, SelectedCtCtcc, SaveAndMoveNextRowPhuTung, AddNewCtct);
        }

        // SỬA LẠI HÀM NÀY GIỐNG Y HỆT BÊN ZCFATS
        [JSInvokable]
        public void CallGridEditMode(string value)
        {
            if (AllowEditMode)
            {
                // Tab Phụ tùng kèm theo có index = 2 (0: Chính, 1: Phụ, 2: Phụ tùng)
                if (ActiveTabIndex == 2)
                    AddNewCtct();
                else
                    AddNewCt();

                StateHasChanged();
            }
        }
        public void Dispose()
        {
            Pageobj?.Dispose();
        }
    }
}
