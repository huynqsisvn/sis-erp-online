# HRXLNS - X?p lo?i nhân s? / Employee Classification

- **Menu_id**: `wr.ct.12`
- **Page route**: `/hrxlns`
- **Th? m?c**: `SisHr/SisHr/Pages/Input/HRXLNS/`
- **B?ng d? li?u**: `dmxlns` (view: `v_dmxlns`)
- **Khóa chính**: `idxl` (GUID)

## Files

| File | Mô t? |
|------|-------|
| `Hrxlns.cs` | Model class |
| `FrmHrxlns.razor` | Form thêm/s?a/xem |
| `FrmHrxlns.razor.cs` | Code-behind c?a form |
| `HrxlnsComponent.razor` | Trang danh sách (browse + toolbar) |

## Ch?c n?ng

- **F2 / Xem**: Xem thông tin x?p lo?i
- **F3 / S?a**: S?a thông tin x?p lo?i
- **F4 / Thêm**: Thêm x?p lo?i m?i
- **F8 / Xóa**: Xóa x?p lo?i (có xác nh?n)
- **K?t xu?t**: Xu?t d? li?u ra file

## Tr??ng d? li?u (form thêm/s?a)

| Tr??ng | Ki?u | B?t bu?c | Mô t? |
|--------|------|----------|-------|
| `nam` | int | ? | N?m x?p lo?i (1900–2500) |
| `thang` | int | ? | Tháng x?p lo?i (1–12) |
| `ma_ns` | string | ? | Mã nhân viên |
| `ma_xlld` | string | ? | Mã x?p lo?i lao ??ng |
| `ghi_chu` | string | | Ghi chú |
| `status` | string | | Tr?ng thái (0/1) |

## Validate

- N?m ph?i trong kho?ng 1900–2500
- Tháng ph?i trong kho?ng 1–12
- Mã nhân viên không ???c ?? tr?ng
- Mã x?p lo?i không ???c ?? tr?ng
- Khi thêm m?i: ki?m tra trùng ma_ns + nam + thang
