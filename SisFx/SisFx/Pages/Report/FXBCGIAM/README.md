# FXBCGIAM - Bảng kê CCDC giảm trong kỳ

**Menu_id:** `wf.xb.22`  
**Route:** `/fxbcgiam`  
**Store Procedure:** `FXBCGIAM`  
**Params:** `@Nam, @tuKy, @denKy, @ma_bpcc, @ma_tg_cc, @Nhom, @Chittietsl, @condition`

## Điều kiện lọc
- Năm, Từ kỳ, Đến kỳ
- Lý do giảm (`dmtgcc`)
- Kiểu nhóm: 0 – Không nhóm, 1 – Theo loại cc, 2 – Theo BPSD, 3 – Theo nhóm cc, 4 – Theo TK chi phí
- Chi tiết: 1 – Đến số thẻ, 2 – Đến BPSD, 3 – Đến mã CCDC
- Mẫu VND/Ngoại tệ (read-only)
- Nâng cao: nhóm cc, loại cc, BPSD, TK CP, nhóm cc 1/2/3

## Chức năng
- **Kết xuất Excel** (Export)
- **In** (Print) – mẫu in V/ và E/

## Cấu trúc
```
FXBCGIAM/
  FXBCGIAM.cs           – Model (FXBCGIAMModel, FXBCGIAMRow)
  FXBCGIAMCpn.razor     – Blazor component chính
  V/Fxbcgiam.cs + Designer.cs  – Mẫu in tiếng Việt
  E/Fxbcgiam.cs + Designer.cs  – Mẫu in tiếng Anh
```
