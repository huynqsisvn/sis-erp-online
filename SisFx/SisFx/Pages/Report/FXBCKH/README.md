# FXBCKH - Bảng tính chi phí CCDC

- **Menu ID**: wf.xb.01
- **Page**: fxbckh
- **Store Procedure**: FXBCKH (tham số: nam, ky1, ky2, Thoi_kh_ck, Het_kh_ck, advance)

## Files
- **FXBCKH.cs**: Model dữ liệu (FXBCKHModel, FXBCKHRow, FXBCKHTong)
- **FXBCKHCpn.razor**: Component chính (form lọc + lưới kết quả)

## Chức năng
- Bảng tính chi phí phân bổ CCDC theo kỳ/năm
- Lọc: năm, từ kỳ, đến kỳ, loại cc, mã cc, nhóm cc, tk phân bổ, tk cp, nhóm cc 1/2/3
- Tích chọn: lên cc đã thôi phân bổ, lên cc đã phân bổ hết
- Kết xuất Excel (toolbar Export)
- In báo cáo (toolbar Print / F7)
