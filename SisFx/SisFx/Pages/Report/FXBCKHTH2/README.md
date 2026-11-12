# FXBCKHTH2 - Bảng tổng hợp chi phí CCDC

## Thông tin
- **Menu ID**: wf.xb.04
- **Page**: fxbckhth2
- **Store Procedure**: FXBCKHTH2 (tham số: nam, ky1, ky2, ma_bpcc, tk_cp, Nhom, Thoi_kh_ck, Het_kh_ck, advance)

## Files
- **FXBCKHTH2.cs**: Model dữ liệu (FXBCKHTH2Model, FXBCKHTH2Row)
- **FXBCKHTH2Cpn.razor**: Component chính (form lọc + lưới kết quả)

## Chức năng
- Tổng hợp chi phí phân bổ CCDC theo kỳ/năm
- Kiểu tổng hợp: theo loại cc / bpcc / nhóm cc / tk cp
- Lọc nâng cao: nhóm cc, loại cc, mã bpcc, tk phân bổ, tk cp, nhóm cc 1/2/3
- Tích chọn: lên cc đã thôi phân bổ, lên cc đã phân bổ hết
- Kết xuất Excel (toolbar Export)
- In báo cáo (toolbar Print / F7)
- Phím tắt: F7 (In), Ctrl+F (Mở bộ lọc)
