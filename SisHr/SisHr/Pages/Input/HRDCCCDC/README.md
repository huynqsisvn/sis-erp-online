# HRDCCCDC - Thu h?i, ?i?u chuy?n Công c? D?ng c?

## Menu_id
`wr.ct.32`

## Page URL
`/hrdcccdc`

## Th? m?c
`SisHr/SisHr/Pages/Input/HRDCCCDC/`

## Các file chính
| File | Mô t? |
|------|-------|
| `Hrdcccdc.cs` | Row models: `HrdccccdcRow`, `HrdccccdcDtRow` |
| `HrdccccdcModel.cs` | Print model: `HrdccccdcModel`, `HrdccccdcDtPrint` |
| `HrdccccdcComponent.razor` | Trang danh sách (browse), toolbar, xóa, in |
| `FrmHrdcccdc.razor` | Form nh?p li?u (thêm/s?a/xem) |
| `FrmHrdcccdc.razor.cs` | Code-behind form nh?p li?u |
| `V/DMDCCCDC.cs` | Report class |
| `V/DMDCCCDC.Designer.cs` | Report designer |

## B?ng d? li?u
- Header: `DMDCCCDC` / View: `v_DMDCCCDC`
- Detail: `DMDCCCDCDT` / View: `v_DMDCCCDCDT`, `v_DMDCCCDCDTDetails`

## Ch?c n?ng
- Thêm m?i, s?a, xem, xóa, in, k?t xu?t Excel
- Lo?i: **Thu h?i** (loai=0) ho?c **?i?u chuy?n** (loai=1)
- Khi lo?i là ?i?u chuy?n: hi?n th? thêm c?t "Sang nhân viên"
- Validate: mã nhân viên t? ? sang, s? ch?ng t? không trùng

## Ngôn ng?
Keys ???c thêm vào `hr-vi.xml` và `hr-en.xml` v?i prefix `Tool transfer`.
