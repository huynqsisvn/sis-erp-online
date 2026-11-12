# FABCKHTH2 - Bảng tổng hợp khấu hao TSCĐ

## Thông tin
- **Menu ID**: wf.bc.02
- **Page**: fabckhth2
- **Store Procedure**: FABCKHTH2 (tham số: nam, ky1, ky2, ma_bpts, tk_cp, Nhom, Thoi_kh_ck, Het_kh_ck, advance)

## Files
- **FABCKHTH2.cs**: Model dữ liệu (FABCKHTH2Model, FABCKHTH2Row)
- **FABCKHTH2Cpn.razor**: Component chính (form lọc + lưới kết quả)

## Chức năng
- Tổng hợp khấu hao TSCĐ theo kỳ/năm
- Kiểu tổng hợp: theo loại ts / bpsd / nhóm ts / tk cp
- Lọc nâng cao: nhóm ts, mã bpsd, loại ts, tk cp, tk khấu hao, nhóm ts 1/2/3
- Tích chọn: lên ts đã thôi khấu hao, lên ts đã khấu hao hết
- Kết xuất Excel (toolbar Export)
- In báo cáo (toolbar Print)
- Phím tắt: F7 (In), Ctrl+F (Mở bộ lọc)
