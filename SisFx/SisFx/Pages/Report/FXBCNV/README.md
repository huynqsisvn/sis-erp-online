# FXBCNV - Bảng tổng hợp giá trị CCDC

## Thông tin
- **Menu ID**: wf.xb.13
- **Page**: /fxbcnv
- **Store Procedure**: FXBCNV (tham số: nam, ky, dau_cuoi, ma_bpcc, advance, group)

## Files
- **FXBCNV.cs**: Model dữ liệu (FXBCNVModel, FXBCNVRow)
- **FXBCNVCpn.razor**: Component chính (form lọc + lưới kết quả)
- **V/FXBCNV.cs + Designer.cs**: Mẫu in tiếng Việt
- **E/FXBCNV.cs + Designer.cs**: Mẫu in tiếng Anh

## Chức năng
- Tổng hợp giá trị CCDC theo kỳ/năm, đầu kỳ hoặc cuối kỳ
- Kiểu tổng hợp (1-8): theo loại cc, BPSD, nhóm cc, tk cp, DVCS, nhóm cc 1/2/3
- Lọc nâng cao: nhóm cc, loại cc, mã bpsd, tk cp, nhóm cc 1/2/3
- Mẫu VND/Ngoại tệ: mặc định VND (read only)
- Kết xuất Excel (toolbar Export)
- In báo cáo (toolbar Print / F7): STT, mã chỉ tiêu, tên chỉ tiêu, nguyên giá, GT đã phân bổ, GT còn lại
