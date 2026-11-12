# FXBCCCTH2 - Bảng tổng hợp giá trị CCDC - theo nhiều chỉ tiêu

## Thông tin
- **Menu ID**: wf.xb.15
- **Page**: fxbcccth2
- **Store Procedure**: FXBCCCTH2 (tham số: nNam, nKy, cDau_cuoi, ma_bp, cKey, inGroup)

## Files
- **FXBCCCTH2.cs**: Model dữ liệu (FXBCCCTH2Model, FXBCCCTH2Row)
- **FXBCCCTH2Cpn.razor**: Component chính (form lọc + lưới kết quả)

## Chức năng
- Tổng hợp giá trị CCDC theo kỳ/năm với nhiều chỉ tiêu nhóm
- **Tiêu chí tổng hợp** (có thể chọn nhiều):
  1. Theo loại cc
  2. Theo BPSD
  3. Theo nhóm cc
  4. Theo tk cp
  5. Theo ĐVCS
  6. Theo nhóm cc 1
  7. Theo nhóm cc 2
  8. Theo nhóm cc 3
- **Lọc nâng cao**: nhóm cc, loại cc, mã BPSD, tk phân bổ, tk cp, nhóm cc 1/2/3
- Kết xuất Excel (toolbar Export)
- Phím tắt: Ctrl+F (Mở bộ lọc)

## Lưu ý
- Không có chức năng in
- Khi thay đổi ĐVCS thì lưới dữ liệu tự động cập nhật
- Tham số `inGroup` được tạo từ các cột tương ứng với tiêu chí được chọn
