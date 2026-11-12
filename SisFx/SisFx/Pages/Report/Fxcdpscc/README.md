# Fxcdpscc – Bảng cân đối phát sinh CCDC

- **Menu_id**: `wf.xb.25`
- **Page route**: `/fxcdpscc`
- **Store procedure**: `Fxcdpscc` (tham số: `@Nam`, `@ky1`, `@ky2`, `@advance`)

## Điều kiện lọc

| Trường | Mô tả |
|--------|-------|
| Năm | Năm cần xem báo cáo |
| Từ kỳ | Kỳ bắt đầu (1–12) |
| Đến kỳ | Kỳ kết thúc (1–12) |
| Mẫu VND/ngoại tệ | Read-only, mặc định VND |
| Nhóm công cụ | `dmnhomcc` |
| Loại công cụ | `dmplcc` |
| TK chi phí | `dmtk` |
| Nhóm CC 1/2/3 | `dmnhcc1`, `dmnhcc2`, `dmnhcc3` |

## Cột mẫu in chính

`stt`, `mã cc`, `tên cc`, `ngày tăng`, `tk cp`, `số kỳ p.bổ`,  
số đầu kỳ (nguyên giá, đã p.bổ, GTCL),  
phát sinh (tăng/giảm NG, tăng/giảm KH, p.bổ trong kỳ),  
số cuối kỳ (nguyên giá, đã p.bổ, GTCL).

## Chức năng

- Lọc dữ liệu theo form popup, cập nhật lưới khi đổi ĐVCS trên thanh tiêu đề.
- Kết xuất Excel qua nút Export.
- In báo cáo qua `ReportOptionComponent` (mẫu in do người dùng tự tạo).
