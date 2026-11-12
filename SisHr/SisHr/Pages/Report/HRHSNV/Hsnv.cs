using System;

namespace SisHr.Pages.Report.HRHSNV
{
    [Serializable]
    public class HsnvModel
    {
        public string Ma_ns { get; set; }
        public string Ten_ns { get; set; }
        public string Ma_bpns { get; set; }
        public string Ten_bpns { get; set; }
        public string Gioi_tinh { get; set; }
        public DateTime? Ngay_sinh { get; set; }
        public string Cmnd { get; set; }
        public string Dien_thoai { get; set; }
        public DateTime? Ngay_vao { get; set; }
        public string Ma_loai_hdong { get; set; }
        public int Status { get; set; }
    }

    [Serializable]
    public class HsnvDetailModel
    {
        // ===== Thông tin nhân viên cơ bản =====
        public string Ma_ns { get; set; }
        public string Ten_ns { get; set; }
        public string Ten_ns2 { get; set; }
        public string Ma_dvcs { get; set; }
        public string Ten_dvcs { get; set; } // Giữ nguyên để phục vụ UI (hiển thị tên)

        // ===== Thông tin cá nhân =====
        public string Gioi_tinh { get; set; }
        public DateTime? Ngay_sinh { get; set; }
        public string Bi_danh { get; set; }
        public string Noi_sinh { get; set; }
        public string Nguyen_quan { get; set; }
        public string Cmnd { get; set; }
        public DateTime? Ngay_cmnd { get; set; }
        public string Nc_cmnd { get; set; }
        public string Quoc_tich { get; set; }
        public string Quoc_tich2 { get; set; }
        public string Ma_dantoc { get; set; }
        public string Ma_quoctich { get; set; } // Mới (Mã quốc tịch)
        public string Ma_tongiao { get; set; }
        public string Ma_tp_banthan { get; set; }
        public string Ma_tp_giadinh { get; set; }
        public string Ma_ngoaingu { get; set; }
        public string Ma_tinhoc { get; set; }
        public string Ma_tt_honnhan { get; set; }
        public string Ma_td_vanhoa { get; set; }
        public string Ma_tt_suckhoe { get; set; }
        public string Ma_bangcap { get; set; }

        // ===== Địa chỉ & Liên hệ =====
        public string Dien_thoai { get; set; }
        public string Dien_thoai_co_dinh { get; set; }
        public string Fax { get; set; }
        public string E_mail { get; set; }
        public string Dia_chi { get; set; }
        public string Ho_khau { get; set; }     // BỔ SUNG (Từ SQL)
        public string Tinh_thanh { get; set; }  // BỔ SUNG (Từ SQL)
        public string Dc_cu_tru { get; set; }
        public string Khicannhantincho { get; set; }
        public string Diachinhantin { get; set; }

        // ===== Công ty & Bộ phận =====
        public string Ma_bpns { get; set; }
        public string Ma_todoi { get; set; }
        public string Khoi { get; set; }        // SỬA: Đổi sang string cho khớp SQL char(1)
        public DateTime? Ngay_vao { get; set; }
        public string Ma_loai_hdong { get; set; }
        public DateTime? Ngay_bien_che { get; set; }
        public DateTime? Ngay_hdld { get; set; } // BỔ SUNG (Từ SQL)
        public string Ma_trinhdo { get; set; }
        public string Ma_chuyennganh { get; set; }
        public string Ma_chucvu { get; set; }
        public string Ma_chucdanh { get; set; }
        public string Ma_loai_td { get; set; }  // BỔ SUNG (Từ SQL)
        public string Image_url { get; set; }   // BỔ SUNG (Từ SQL)

        // ===== Thuế & Pháp lý =====
        public string Ma_so_thue { get; set; }
        public DateTime? Ngay_mst { get; set; }
        public string Nc_mst { get; set; }
        public DateTime? Ngay_ctru { get; set; }

        // ===== Cơ chế thuế đặc thù =====
        public int? Dt_cu_tru { get; set; }
        public int? Thue_ck { get; set; }
        public int? Khu_kt_ck { get; set; }
        public int? Uq_qt_ck { get; set; }
        public int? Hdld3thang { get; set; }
        public string Hd_thue { get; set; }     // BỔ SUNG (Từ SQL)
        public string Ghi_chu { get; set; }
        public int? An_com { get; set; } // Trong SQL là kiểu bit (True/False hoặc 1/0)

        public bool An_com_checked
        {
            get => An_com == 1;
            set => An_com = value ? 1 : 0;
        }

        // ===== Trạng thái =====
        public string Status { get; set; } // SỬA: Trong SQL là char(1)

        // ===== Bảo hiểm xã hội & Y tế =====
        public string So_so_bhxh { get; set; }
        public string So_the_bhyt { get; set; }
        public DateTime? Ngay_tham_gia_bhxh { get; set; }
        public DateTime? Ngay_cap_so_bhxh { get; set; }
        public DateTime? Ngay_het_han_bhyt { get; set; }
        public string Ma_noi_kcb { get; set; }
        public string Noi_cap_so_bhxh { get; set; }
        public string Ma_chinhsach { get; set; }
        public DateTime? Ngay_huong_chinhsach { get; set; }
        public string Ma_noi_cap_tbhyt { get; set; }

        // ===== Đoàn - Đảng - Quân đội =====
        public DateTime? Ngay_vao_doan { get; set; }
        public string Noi_ket_nap_doan { get; set; }
        public string Ma_chucvu_doan { get; set; }
        public DateTime? Ngay_vao_dang { get; set; }
        public DateTime? Ngay_chinh_thuc_vao_dang { get; set; }
        public string Noi_ket_nap_dang { get; set; }
        public string Ma_chucvu_dang { get; set; }
        public DateTime? Ngay_thamgia_cm { get; set; }
        public DateTime? Ngay_nhap_ngu { get; set; }
        public DateTime? Ngay_xuat_ngu { get; set; }
        public string Ma_chucvu_quandoi { get; set; }
        public string Trinh_do_trinh_tri { get; set; }
        public string Trinh_do_quan_ly { get; set; }

        // ===== Lương =====
        public decimal? He_so_luong { get; set; }
        public decimal? He_so_phu_cap { get; set; }
        public decimal? Bac_luong { get; set; } // SỬA: Đổi sang decimal? cho khớp kiểu udt_tien
        public DateTime? Ngay_huong_luong { get; set; }
        public decimal? Muc_luong { get; set; }

        // ===== Chuyển đơn vị / Nghỉ việc =====
        public DateTime? Ngay_chuyen_di { get; set; }
        public string Da_chuyen_di { get; set; } // SỬA: Trong SQL là char(1) thay vì bool
        public string Ma_lydonghi { get; set; }  // BỔ SUNG (Từ SQL)

        // ===== Giảm trừ =====
        public int? Giam_tru { get; set; }
        public decimal? Muc_gt { get; set; }
        public decimal? Muc_gt_nt { get; set; }  // BỔ SUNG (Từ SQL)

        public bool Giam_tru_checked
        {
            get => Giam_tru == 1;
            set => Giam_tru = value ? 1 : 0;
        }

        // ===== Trường tự do (Free fields) & Quản lý (BỔ SUNG TỪ SQL) =====
        public string Ma_td1 { get; set; }
        public string Ma_td2 { get; set; }
        public string Ma_td3 { get; set; }
        public DateTime? Ngay_td1 { get; set; }
        public DateTime? Ngay_td2 { get; set; }
        public DateTime? Ngay_td3 { get; set; }
        public decimal? Sl_td1 { get; set; }
        public decimal? Sl_td2 { get; set; }
        public decimal? Sl_td3 { get; set; }
        public string Gc_td1 { get; set; }
        public string Gc_td2 { get; set; }
        public string Gc_td3 { get; set; }
        public string Ma_tra_cuu { get; set; }
        public string Stt_rec { get; set; }
        public string Search { get; set; }

        // ===== RESIGNATION UPDATE (Biến phụ dùng cho giao diện xử lý nghỉ việc) =====
        public int? Nghi_viec { get; set; }
        public DateTime? Ngay_nv { get; set; }
        public string So_qd_nv { get; set; }
        public DateTime? Ngay_qd_nv { get; set; }
        public string Ly_do_nv { get; set; }
        public string Ghi_chu_nv { get; set; }
        public string Ma_quocgia
        {
            get => Quoc_tich;
            set => Quoc_tich = value;
        }

        public string Ma_tinhthanh
        {
            get => Nguyen_quan;
            set => Nguyen_quan = value;
        }

        // ===== Cấu trúc Danh sách (Giữ nguyên) =====
        public System.Collections.Generic.List<CtgtkRow> ListCtgtk { get; set; } = new();
        public System.Collections.Generic.List<CtnptRow> ListCtnpt { get; set; } = new();
        public System.Collections.Generic.List<HopDongRow> ListHopDong { get; set; } = new();
        public System.Collections.Generic.List<QtbhRow> ListQtbh { get; set; } = new();
        public System.Collections.Generic.List<QhgdRow> ListQhgd { get; set; } = new();
        public System.Collections.Generic.List<DieuChuyenRow> ListDieuChuyen { get; set; } = new();
        public System.Collections.Generic.List<QtctRow> ListQtct { get; set; } = new();
        public NghiViecRow NghiViecInfo { get; set; } = new NghiViecRow();

        // ===== Kiểm toán (Audit) =====
        public DateTime? Date0 { get; set; }
        public string Time0 { get; set; }
        public int? User_id0 { get; set; }
        public string User_name0 { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public int? User_id { get; set; }
        public string User_name { get; set; }
    }

    public class DvcsNode
    {
        public string Ma_dvcs { get; set; }
        public string Ten_dvcs { get; set; }
        public System.Collections.Generic.List<BpnsNode> DsBpns { get; set; }
            = new System.Collections.Generic.List<BpnsNode>();
    }

    public class BpnsNode
    {
        public string Ma_bpns { get; set; }
        public string Ten_bpns { get; set; }
        public string Ma_dvcs { get; set; }
    }

    // ===== GRID MODELS =====
    [Serializable]
    public class CtgtkRow
    {
        public int Stt { get; set; }
        public string Stt_rec { get; set; }     // udt_stt_rec
        public string Key_gt { get; set; }      // udt_ten_dien_giai
        public string Ma_dvcs { get; set; }     // udt_ma_dvcs
        public string Ma_ns { get; set; }       // udt_ma_ns
        public DateTime? Ngay_hl1 { get; set; } // udt_ngay_ct
        public DateTime? Ngay_hl2 { get; set; } // udt_ngay_ct
        public string Ma_gt { get; set; }       // udt_ma_gt
        public string Dien_giai { get; set; }   // udt_ten_dien_giai
        public decimal? Luong_thang_nt { get; set; } // udt_tien_nt
        public decimal? Luong_thang { get; set; }    // udt_tien
        public decimal? Luong_nt { get; set; }       // udt_tien_nt
        public decimal? Luong { get; set; }          // udt_tien
        public decimal? Tien_gt { get; set; }        // udt_tien
        public decimal? Tien_gt_nt { get; set; }     // udt_tien_nt
        
        // ===== Bảo hiểm và Giảm trừ (Thêm mới) =====
        public decimal? A_001 { get; set; }     // BH Thất nghiệp (1%)
        public decimal? A_002 { get; set; }     // BH Y tế (3%)
        public decimal? A_003 { get; set; }     // BH Xã hội (8%)
        public decimal? Gia_canh { get; set; }
    }

    [Serializable]
    public class CtnptRow
    {
        public string Stt_rec { get; set; }     // udt_stt_rec
        public string Ma_dvcs { get; set; }     // udt_ma_dvcs
        public string Ma_ns { get; set; }       // udt_ma_ns
        public string Ma_bpns { get; set; }     // udt_ma_bp
        public decimal? Stt { get; set; }       // numeric(12, 0)
        public string Ho_ten { get; set; }      // udt_ten_dien_giai
        public string Ho_ten2 { get; set; }     // udt_ten_dien_giai
        public string Ma_npt { get; set; }      // udt_ma_npt
        public DateTime? Ngay_sinh { get; set; } // udt_ngay_ct
        public string Ma_so_thue { get; set; }  // udt_ma_so_thue
        public string Cmnd { get; set; }        // char(16)
        public string Quan_he { get; set; }      // nvarchar(48)
        public string Quan_he2 { get; set; }     // nvarchar(50)
        public DateTime? Ngay_pt1 { get; set; } // udt_ngay_ct
        public DateTime? Ngay_pt2 { get; set; } // udt_ngay_ct
        public string Ct_kem { get; set; }      // nvarchar(128)
        public string Dien_giai { get; set; }   // udt_ten_dien_giai
        public decimal? Tien_gt { get; set; }   // udt_tien
        public decimal? Tien_gt_nt { get; set; } // udt_tien_nt
        public int? Bo_sung { get; set; }       // numeric(3, 0)
        public string So { get; set; }          // nvarchar(50)
        public string Quyen_so { get; set; }     // nvarchar(50)
        public string Quoc_gia { get; set; }    // nvarchar(50)
        public string Thanh_pho { get; set; }   // nvarchar(50)
        public string Quan { get; set; }        // nvarchar(50)
        public string Phuong { get; set; }      // nvarchar(50)
    }
    // ===== HOP DONG (HỢP ĐỒNG LAO ĐỘNG) =====
    [Serializable]
    public class HopDongRow
    {
        public int Stt { get; set; }
        
        // ===== Thông tin cơ bản =====
        public string Ma_hdld { get; set; }      // udt_ma_dvcs (Mã hợp đồng)
        public string Ma_plhd { get; set; }      // udt_ma_dvcs (Mã phân loại hợp đồng)
        public string Ma_lhd { get; set; }       // udt_ma_dvcs (Mã loại hợp đồng)
        
        // ===== Thông tin nhân sự =====
        public string Ma_ns_ct { get; set; }     // udt_ma_ns (Mã NV - Người ký từ phía công ty)
        public string Ma_ns { get; set; }        // udt_ma_ns (Mã NV - Người ký từ phía NV)
        public string Ma_quocgia { get; set; }   // udt_ma_dvcs (Mã quốc gia)
        
        // ===== Thời gian hợp đồng =====
        public DateTime? Tu_ngay { get; set; }   // udt_ngay_ct (Từ ngày)
        public DateTime? Den_ngay { get; set; }  // udt_ngay_ct (Đến ngày)
        public DateTime? Ngay_hl { get; set; }   // udt_ngay_ct (Ngày hợp lệ)
        public DateTime? Ngay_hd { get; set; }   // udt_ngay_ct (Ngày hợp đồng / Ngày ký)
        
        // ===== Địa điểm làm việc =====
        public string Dia_diem_lam_viec { get; set; }  // udt_dia_chi (Địa điểm làm việc)
        public string Dia_chi_lam_viec { get; set; }   // udt_dia_chi (Địa chỉ làm việc)
        
        // ===== Chức vụ =====
        public string Ma_cv_ct { get; set; }    // udt_ma_dvcs (Mã CV - Công ty)
        public string Ma_cv { get; set; }       // udt_ma_dvcs (Mã CV - Nhân viên)
        
        // ===== Yêu cầu công việc =====
        public string Yeu_cau1 { get; set; }    // udt_dia_chi (Yêu cầu 1)
        public string Yeu_cau2 { get; set; }    // udt_dia_chi (Yêu cầu 2)
        
        // ===== Điều kiện làm việc =====
        public decimal? Thoi_gian_lv { get; set; }    // udt_so_luong (Thời gian làm việc - giờ/ngày)
        public string Phuong_tien { get; set; }       // udt_dia_chi (Phương tiện)
        public decimal? Muc_luong { get; set; }       // udt_tien (Mức lương)
        public string Phu_cap { get; set; }           // udt_dia_chi (Phụ cấp)
        
        // ===== Ghi chú =====
        public string Ghi_chu { get; set; }     // udt_ghi_chu
        
        // ===== Trạng thái =====
        public string Status { get; set; }      // char(1)
        
        // ===== Trường tự do =====
        public string Ma_td1 { get; set; }      // udt_ma_td
        public string Ma_td2 { get; set; }      // udt_ma_td
        public string Ma_td3 { get; set; }      // udt_ma_td
        public DateTime? Ngay_td1 { get; set; } // udt_ngay_ct
        public DateTime? Ngay_td2 { get; set; } // udt_ngay_ct
        public DateTime? Ngay_td3 { get; set; } // udt_ngay_ct
        public decimal? Sl_td1 { get; set; }    // udt_so_luong
        public decimal? Sl_td2 { get; set; }    // udt_so_luong
        public decimal? Sl_td3 { get; set; }    // udt_so_luong
        public string Gc_td1 { get; set; }      // udt_ghi_chu_td
        public string Gc_td2 { get; set; }      // udt_ghi_chu_td
        public string Gc_td3 { get; set; }      // udt_ghi_chu_td
        public string Ma_tra_cuu { get; set; }  // udt_ma_tra_cuu
        
        // ===== Kiểm toán (Audit) =====
        public DateTime? Date { get; set; }     // udt_ngay_ct
        public string Time { get; set; }        // udt_time
        public int? User_id { get; set; }       // udt_user_id
        public string User_name { get; set; }   // udt_user_name
        public DateTime? Date0 { get; set; }    // udt_ngay_ct
        public string Time0 { get; set; }       // udt_time
        public int? User_id0 { get; set; }      // udt_user_id
        public string User_name0 { get; set; }  // udt_user_name
        
        // ===== Tìm kiếm =====
        public string Search { get; set; }      // udt_search
    }

    // ===== QTBH (QUÁ TRÌNH THAM GIA BẢO HIỂM) =====
    [Serializable]
    public class QtbhRow
    {
        public int Stt { get; set; }
        
        // ===== Thông tin cơ bản =====
        public string Qtbhid { get; set; }           // char(36) (ID bảo hiểm)
        public string Ma_ns { get; set; }            // udt_ma_dvcs (Mã nhân viên)
        
        // ===== Thông tin bảo hiểm =====
        public string So_so_bhxh { get; set; }       // udt_ma_dvcs (Số sổ BHXH)
        public string So_the_bhxh { get; set; }      // udt_ma_dvcs (Số thẻ BHXH)
        public string So_the_bhyt { get; set; }      // udt_ma_dvcs (Số thẻ BHYT)
        
        // ===== Thời gian tham gia =====
        public DateTime? Tu_ngay { get; set; }       // udt_ngay_ct (Từ ngày)
        public DateTime? Den_ngay { get; set; }      // udt_ngay_ct (Đến ngày)
        
        // ===== Thông tin đơn vị =====
        public string Donvi_bh { get; set; }         // udt_ten_dien_giai (Đơn vị BH)
        public string Donvi_cv { get; set; }         // udt_ten_dien_giai (Đơn vị công vụ)
        
        // ===== Thông tin bảo hiểm y tế =====
        public DateTime? Bhyt_tungay { get; set; }   // udt_ngay_ct (BHYT từ ngày)
        public DateTime? Bhyt_denngay { get; set; }  // udt_ngay_ct (BHYT đến ngày)
        
        // ===== Ghi chú =====
        public string Ghi_chu { get; set; }          // udt_ghi_chu_td
        
        // ===== Trạng thái =====
        public string Status { get; set; }           // char(1)
        
        // ===== Trường tự do =====
        public string Ma_td1 { get; set; }           // udt_ma_td
        public string Ma_td2 { get; set; }           // udt_ma_td
        public string Ma_td3 { get; set; }           // udt_ma_td
        public DateTime? Ngay_td1 { get; set; }      // udt_ngay_ct
        public DateTime? Ngay_td2 { get; set; }      // udt_ngay_ct
        public DateTime? Ngay_td3 { get; set; }      // udt_ngay_ct
        public decimal? Sl_td1 { get; set; }         // udt_so_luong
        public decimal? Sl_td2 { get; set; }         // udt_so_luong
        public decimal? Sl_td3 { get; set; }         // udt_so_luong
        public string Gc_td1 { get; set; }           // udt_ghi_chu_td
        public string Gc_td2 { get; set; }           // udt_ghi_chu_td
        public string Gc_td3 { get; set; }           // udt_ghi_chu_td
        public string Ma_tra_cuu { get; set; }       // udt_ma_tra_cuu
        
        // ===== Kiểm toán (Audit) =====
        public DateTime? Date { get; set; }          // udt_ngay_ct
        public string Time { get; set; }             // udt_time
        public int? User_id { get; set; }            // udt_user_id
        public string User_name { get; set; }        // udt_user_name
        public DateTime? Date0 { get; set; }         // udt_ngay_ct
        public string Time0 { get; set; }            // udt_time
        public int? User_id0 { get; set; }           // udt_user_id
        public string User_name0 { get; set; }       // udt_user_name
        
        // ===== Tìm kiếm =====
        public string Search { get; set; }           // udt_search
    }

    // ===== QHGD (QUAN HỆ GIA ĐÌNH) =====
    [Serializable]
    public class QhgdRow
    {
        public int Stt { get; set; }
        
        // ===== Thông tin cơ bản =====
        public string Ma_tpgd { get; set; }          // char(36) (ID thành viên gia đình)
        public string Ma_ns { get; set; }            // udt_ma_dvcs (Mã nhân viên)
        public string Stt_row { get; set; }          // udt_ma_dvcs (STT)
        public string Ma_gioitinh { get; set; }      // udt_ma_dvcs (Mã giới tính)
        public string Ma_qhgd { get; set; }          // udt_ma_dvcs (Mã quan hệ gia đình)
        
        // ===== Thông tin cá nhân =====
        public string Ten_tpgd { get; set; }         // udt_ten_dien_giai (Tên thành viên GĐ)
        public string Ten_tpgd2 { get; set; }        // udt_ten_dien_giai (Tên thành viên GĐ 2)
        public string Ma_so_thue { get; set; }       // udt_ma_dvcs (Mã số thuế)
        public string Nam_sinh { get; set; }         // char(4) (Năm sinh)
        public string Cmnd { get; set; }             // char(16) (CMND/Hộ chiếu)
        
        // ===== Thông tin công việc =====
        public string Nghe_nghiep { get; set; }      // udt_ten_dien_giai (Nghề nghiệp)
        public string Noi_lam_viec { get; set; }     // udt_dia_chi (Nơi làm việc)
        public string Cho_o_hien_nay { get; set; }   // udt_dia_chi (Chỗ ở hiện nay)
        
        // ===== Thông tin chính trị =====
        public string Trinh_do_chinh_tri { get; set; }  // udt_ten_dien_giai (Trình độ chính trị)
        public string Truoc_cach_mang { get; set; }     // udt_ten_dien_giai (Trước cách mạng)
        public string Trong_cach_mang { get; set; }     // udt_ten_dien_giai (Trong cách mạng)
        public string Sau_cach_mang { get; set; }       // udt_ten_dien_giai (Sau cách mạng)
        
        // ===== Thông tin khác =====
        public string Nguoi_phu_thuoc { get; set; }  // char(1) (Người phụ thuộc)
        public string Ghi_chu { get; set; }          // udt_ghi_chu_td (Ghi chú)
        
        // ===== Trạng thái =====
        public string Status { get; set; }           // char(1)
        
        // ===== Trường tự do =====
        public string Ma_td1 { get; set; }           // udt_ma_td
        public string Ma_td2 { get; set; }           // udt_ma_td
        public string Ma_td3 { get; set; }           // udt_ma_td
        public DateTime? Ngay_td1 { get; set; }      // udt_ngay_ct
        public DateTime? Ngay_td2 { get; set; }      // udt_ngay_ct
        public DateTime? Ngay_td3 { get; set; }      // udt_ngay_ct
        public decimal? Sl_td1 { get; set; }         // udt_so_luong
        public decimal? Sl_td2 { get; set; }         // udt_so_luong
        public decimal? Sl_td3 { get; set; }         // udt_so_luong
        public string Gc_td1 { get; set; }           // udt_ghi_chu_td
        public string Gc_td2 { get; set; }           // udt_ghi_chu_td
        public string Gc_td3 { get; set; }           // udt_ghi_chu_td
        public string Ma_tra_cuu { get; set; }       // udt_ma_tra_cuu
        
        // ===== Kiểm toán (Audit) =====
        public DateTime? Date { get; set; }          // udt_ngay_ct
        public string Time { get; set; }             // udt_time
        public int? User_id { get; set; }            // udt_user_id
        public string User_name { get; set; }        // udt_user_name
        public DateTime? Date0 { get; set; }         // udt_ngay_ct
        public string Time0 { get; set; }            // udt_time
        public int? User_id0 { get; set; }           // udt_user_id
        public string User_name0 { get; set; }       // udt_user_name
        
        // ===== Tìm kiếm =====
        public string Search { get; set; }           // udt_search
    }

    // ===== DIEU CHUYEN (ĐIỀU CHUYỂN NHÂN SỰ) =====
    [Serializable]
    public class DieuChuyenRow
    {
        public int Stt { get; set; }
        
        // ===== Thông tin cơ bản =====
        public string Qtdcid { get; set; }           // char(36) (ID điều chuyển)
        public string Ma_ns { get; set; }            // udt_ma_dvcs (Mã nhân viên)
        
        // ===== Điều chuyển cũ =====
        public DateTime? Ngay_dc_cu { get; set; }    // udt_ngay_ct (Ngày điều chuyển cũ)
        public string Ma_bpns_cu { get; set; }       // udt_ma_dvcs (Mã BP cũ)
        public string Ma_cv_cu { get; set; }         // udt_ma_dvcs (Mã chức vụ cũ)
        public string Ma_cd_cu { get; set; }         // udt_ma_dvcs (Mã chức danh cũ)
        public string Ghi_chu_cu { get; set; }       // udt_ghi_chu_td (Ghi chú cũ)
        
        // ===== Điều chuyển mới =====
        public DateTime? Ngay_dc_moi { get; set; }   // udt_ngay_ct (Ngày điều chuyển mới)
        public string Ma_bpns_moi { get; set; }      // udt_ma_dvcs (Mã BP mới)
        public string Ma_cv_moi { get; set; }        // udt_ma_dvcs (Mã chức vụ mới)
        public string Ma_cd_moi { get; set; }        // udt_ma_dvcs (Mã chức danh mới)
        public string Ghi_chu_moi { get; set; }      // udt_ghi_chu_td (Ghi chú mới)
        
        // ===== Trạng thái =====
        public string Status { get; set; }           // char(1)
        
        // ===== Trường tự do =====
        public string Ma_td1 { get; set; }           // udt_ma_td
        public string Ma_td2 { get; set; }           // udt_ma_td
        public string Ma_td3 { get; set; }           // udt_ma_td
        public DateTime? Ngay_td1 { get; set; }      // udt_ngay_ct
        public DateTime? Ngay_td2 { get; set; }      // udt_ngay_ct
        public DateTime? Ngay_td3 { get; set; }      // udt_ngay_ct
        public decimal? Sl_td1 { get; set; }         // udt_so_luong
        public decimal? Sl_td2 { get; set; }         // udt_so_luong
        public decimal? Sl_td3 { get; set; }         // udt_so_luong
        public string Gc_td1 { get; set; }           // udt_ghi_chu_td
        public string Gc_td2 { get; set; }           // udt_ghi_chu_td
        public string Gc_td3 { get; set; }           // udt_ghi_chu_td
        public string Ma_tra_cuu { get; set; }       // udt_ma_tra_cuu
        
        // ===== Kiểm toán (Audit) =====
        public DateTime? Date { get; set; }          // udt_ngay_ct
        public string Time { get; set; }             // udt_time
        public int? User_id { get; set; }            // udt_user_id
        public string User_name { get; set; }        // udt_user_name
        public DateTime? Date0 { get; set; }         // udt_ngay_ct
        public string Time0 { get; set; }            // udt_time
        public int? User_id0 { get; set; }           // udt_user_id
        public string User_name0 { get; set; }       // udt_user_name
        
        // ===== Tìm kiếm =====
        public string Search { get; set; }           // udt_search
    }

    // ===== QTCT (QUÁ TRÌNH CÔNG TÁC) =====
    [Serializable]
    public class QtctRow
    {
        public int Stt { get; set; }
        
        // ===== Thông tin cơ bản =====
        public int? Nvctid { get; set; }             // int IDENTITY (ID công tác)
        public string Ma_quyet_dinh { get; set; }    // udt_ghi_chu_td (Mã quyết định)
        public string Ma_ns { get; set; }            // udt_ma_dvcs (Mã nhân viên)
        public string Ma_nct { get; set; }           // udt_ma_dvcs (Mã công tác)
        
        // ===== Thời gian công tác =====
        public DateTime? Ngay_di { get; set; }       // udt_ngay_ct (Ngày đi)
        public DateTime? Ngay_ve { get; set; }       // udt_ngay_ct (Ngày về)
        
        // ===== Thông tin chi phí =====
        public decimal? Chi_phi { get; set; }        // udt_tien (Chi phí)
        
        // ===== Nội dung công tác =====
        public string Noi_dung { get; set; }         // udt_ghi_chu_td (Nội dung)
        public string Ket_qua { get; set; }          // udt_ghi_chu_td (Kết quả)
        public string Ghi_chu { get; set; }          // udt_ghi_chu_td (Ghi chú)
        
        // ===== Trạng thái =====
        public string Status { get; set; }           // char(1)
        
        // ===== Trường tự do =====
        public string Ma_td1 { get; set; }           // udt_ma_td
        public string Ma_td2 { get; set; }           // udt_ma_td
        public string Ma_td3 { get; set; }           // udt_ma_td
        public DateTime? Ngay_td1 { get; set; }      // udt_ngay_ct
        public DateTime? Ngay_td2 { get; set; }      // udt_ngay_ct
        public DateTime? Ngay_td3 { get; set; }      // udt_ngay_ct
        public decimal? Sl_td1 { get; set; }         // udt_so_luong
        public decimal? Sl_td2 { get; set; }         // udt_so_luong
        public decimal? Sl_td3 { get; set; }         // udt_so_luong
        public string Gc_td1 { get; set; }           // udt_ghi_chu_td
        public string Gc_td2 { get; set; }           // udt_ghi_chu_td
        public string Gc_td3 { get; set; }           // udt_ghi_chu_td
        public string Ma_tra_cuu { get; set; }       // udt_ma_tra_cuu
        
        // ===== Kiểm toán (Audit) =====
        public DateTime? Date { get; set; }          // udt_ngay_ct
        public string Time { get; set; }             // udt_time
        public int? User_id { get; set; }            // udt_user_id
        public string User_name { get; set; }        // udt_user_name
        public DateTime? Date0 { get; set; }         // udt_ngay_ct
        public string Time0 { get; set; }            // udt_time
        public int? User_id0 { get; set; }           // udt_user_id
        public string User_name0 { get; set; }       // udt_user_name
        
        // ===== Tìm kiếm =====
        public string Search { get; set; }           // udt_search
    }

    // ===== NGHI VIEC (NHÂN SỰ NGHỈ VIỆC) =====
    [Serializable]
    public class NghiViecRow
    {
        // ===== Thông tin cơ bản =====
        public string Ma_ns { get; set; }                      // udt_ma_dvcs (Mã nhân viên - Primary Key)
        public string Ma_nghi { get; set; }                    // udt_ma_dvcs (Mã nghỉ việc)
        
        // ===== Thông tin đơn từ =====
        public DateTime? Ngay_lam_don { get; set; }            // udt_ngay_ct (Ngày làm đơn)
        public DateTime? Ngay_thoi_viec { get; set; }          // udt_ngay_ct (Ngày thôi việc)
        public DateTime? Ngay_thoi_thuc_te { get; set; }       // udt_ngay_ct (Ngày thôi việc thực tế)
        
        // ===== Thông tin trợ cấp =====
        public string Tro_cap { get; set; }                    // char(1) (Trợ cấp - Y/N)
        public DateTime? Ngay_chot_so { get; set; }            // udt_ngay_ct (Ngày chốt sổ)
        public DateTime? Ngay_tra_so { get; set; }             // udt_ngay_ct (Ngày trả sổ)
        
        // ===== Quyết định =====
        public string Quyet_dinh_cham_dut_hd { get; set; }     // udt_ten_dien_giai (QĐ chấm dứt HĐ)
        public string Quyet_dinh_tro_cap { get; set; }         // udt_ten_dien_giai (QĐ trợ cấp)
        
        // ===== Trạng thái =====
        public string Status { get; set; }                     // char(1)
        
        // ===== Trường tự do =====
        public string Ma_td1 { get; set; }                     // udt_ma_td
        public string Ma_td2 { get; set; }                     // udt_ma_td
        public string Ma_td3 { get; set; }                     // udt_ma_td
        public DateTime? Ngay_td1 { get; set; }                // udt_ngay_ct
        public DateTime? Ngay_td2 { get; set; }                // udt_ngay_ct
        public DateTime? Ngay_td3 { get; set; }                // udt_ngay_ct
        public decimal? Sl_td1 { get; set; }                   // udt_so_luong
        public decimal? Sl_td2 { get; set; }                   // udt_so_luong
        public decimal? Sl_td3 { get; set; }                   // udt_so_luong
        public string Gc_td1 { get; set; }                     // udt_ghi_chu_td
        public string Gc_td2 { get; set; }                     // udt_ghi_chu_td
        public string Gc_td3 { get; set; }                     // udt_ghi_chu_td
        public string Ma_tra_cuu { get; set; }                 // udt_ma_tra_cuu
        
        // ===== Kiểm toán (Audit) =====
        public DateTime? Date { get; set; }                    // udt_ngay_ct
        public string Time { get; set; }                       // udt_time
        public int? User_id { get; set; }                      // udt_user_id
        public string User_name { get; set; }                  // udt_user_name
        public DateTime? Date0 { get; set; }                   // udt_ngay_ct
        public string Time0 { get; set; }                      // udt_time
        public int? User_id0 { get; set; }                     // udt_user_id
        public string User_name0 { get; set; }                 // udt_user_name
        
        // ===== Tìm kiếm =====
        public string Search { get; set; }                     // udt_search
    }
}
