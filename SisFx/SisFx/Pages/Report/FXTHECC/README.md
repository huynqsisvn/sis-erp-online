# FXTHECC - Thẻ công cụ, dụng cụ

- **Menu ID**: wf.xb.16
- **Page**: fxthecc
- **Store Procedure**: FxTheCc (tham số: nNam, ma_bpcc, cKey)

## Files
- **FXTHECC.cs**: Model dữ liệu (FXTHECCModel, FXTHECCRow, FXTHECCTong)
- **FXTHECCCpn.razor**: Component chính (form lọc + lưới kết quả với chọn nhiều dòng để in)

## Chức năng
- Thẻ công cụ dụng cụ theo năm
- Lọc: năm, mã cc, mẫu VND/ngoại tệ (readonly), nhóm cc, loại cc, mã bpsd, tk cp, nhóm cc 1/2/3
- Lưới kết quả hỗ trợ chọn nhiều dòng (tag) để in nhiều bản ghi cùng lúc
- Kết xuất Excel (toolbar Export)
- In báo cáo (toolbar Print / F7)
