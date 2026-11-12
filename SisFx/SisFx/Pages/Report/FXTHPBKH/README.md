# FXTHPBKH – B?ng t?ng h?p phân b? chi phí CCDC

## Thông tin chung
- **Menu_id**: `wf.xb.10`
- **Trang**: `/fxthpbkh`
- **Stored Procedure**: `FXTHPBKH`
- **Ngu?n d? li?u**: View `v_fxbcpbkh`

## Ch?c n?ng
- L?c theo: N?m, T? k?/??n k?, Tk phân b?, Tk chi phí, Mã BPSD, Mã công c?.
- M?u VND/Ngo?i t? (read-only, m?c ??nh VND).
- K?t xu?t d? li?u ra Excel.
- Thay ??i ?VCS trên thanh tiêu ?? ? d? li?u l??i t? c?p nh?t.

## C?u trúc file
| File | Mô t? |
|------|-------|
| `FXTHPBKHModel.cs` | Model d? li?u (FXTHPBKHModel, FXTHPBKHRow) |
| `FXTHPBKHCpn.razor` | Component Blazor: form l?c + màn hình brow |
