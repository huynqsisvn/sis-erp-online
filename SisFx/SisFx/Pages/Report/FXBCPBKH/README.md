# FXBCPBKH – Bảng tổng hợp chi phí CCDC theo nhiều chỉ tiêu

## Thông tin chung
- **Menu_id**: `wf.xb.08`
- **Trang**: `/fxbcpbkh`
- **Stored Procedure**: `FXBCPBKH`
- **Nguồn dữ liệu**: View `v_FXBCPBKH`

## Chức năng
- Lọc theo: Năm, Từ kỳ/Đến kỳ, Mã BPSD, Mã công cụ, các điều kiện nâng cao (TK phân bổ, TK chi phí, Mã BPHT, Mã phí, Mã dự án, Mã sản phẩm).
- Tùy chọn: Lên cc đã thôi phân bổ, lên cc đã phân bổ hết.
- Kết xuất dữ liệu ra Excel.

## Cấu trúc file
| File | Mô tả |
|------|-------|
| `FXBCPBKHModel.cs` | Model dữ liệu (FXBCPBKHModel, FXBCPBKHRow) |
| `FXBCPBKHCpn.razor` | Component Blazor: form lọc + màn hình brow |
