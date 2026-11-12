# Glkcck - Bút toán kết chuyển cuối kỳ

## Thông tin
- **Menu_id**: `wg.ct.16`
- **Page**: `glkcck`
- **Thư mục**: `SisGl/Pages/Input/Glkcck`

## Chức năng
Hiện Bảng cân đối phát sinh tài khoản theo từ kỳ đến kỳ, hỗ trợ kết chuyển và hủy kết chuyển cuối kỳ.

## Các thao tác
| Nút | Mô tả |
|-----|-------|
| **Kết chuyển** (Button1) | Chọn tài khoản (checkbox), nhập tham số (ngày, số CT, kỳ), lưu bút toán kết chuyển |
| **Hủy kết chuyển** (Button2) | Xóa các bút toán kết chuyển đã chọn |
| **Export** | Xuất dữ liệu ra Excel/PDF |

## Stored Procedures cần tạo
- **Store_proc từ `curCommand`** (giống `glcd1`) — tải bảng CĐPS tài khoản
  - Tham số: `Ngay_ct1` = ngày đầu kỳ của Từ kỳ, `Ngay_ct2` = ngày cuối kỳ của Đến kỳ, `Ma_dvcs`, `Advance`
- **`dbo.Glkcck`** với `@Type`:
  - `CHECK` — Kiểm tra đã có bút toán kết chuyển chưa
  - `NEW` — Lưu bút toán kết chuyển (thêm `@Ngay_ct`, `@So_ct`, `@Dien_giai`)
  - `DEL` — Hủy bút toán kết chuyển
