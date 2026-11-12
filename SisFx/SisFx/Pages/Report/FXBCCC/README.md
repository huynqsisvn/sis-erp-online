# FXBCCC - Bảng kê giá trị CCDC

## Thông tin
- **Menu ID**: wf.xb.12
- **Page**: fxbccc
- **Store Procedure**: FXBCCC (tham số: nam, ky, dau_cuoi, ma_bpcc, group, detail, advance)

## Files
- **FXBCCCModel.cs**: Model dữ liệu (FXBCCCModel, FXBCCCRow)
- **FXBCCCCpn.razor**: Component chính (form lọc + lưới kết quả)

## Chức năng
- Bảng kê giá trị CCDC theo kỳ/năm
- Lọc theo đầu kỳ hoặc cuối kỳ
- Kiểu nhóm số liệu: 0-không nhóm, 1-theo loại cc, 2-theo bpsd, 3-theo nhóm cc, 4-theo tk cp
- Chi tiết số liệu: 1-đến số thẻ, 2-đến bpsd, 3-đến mã ccdc
- Lọc nâng cao: nhóm cc, loại cc, mã bpsd, tk cp, nhóm cc 1/2/3
- Kết xuất Excel (toolbar Export)
- Không có chức năng in
