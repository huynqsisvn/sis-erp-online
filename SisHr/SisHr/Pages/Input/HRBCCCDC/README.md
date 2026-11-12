# HRBCCCDC - Báo cáo t?ng h?p CCDC nhân s?

## Thông tin
- **Menu_id**: `wr.ct.33`
- **Page route**: `/hrbcccdc`
- **Module**: SisFx
- **Th? m?c**: `SisFx/SisFx/Pages/Report/HRBCCCDC/`

## Các file
| File | Mô t? |
|------|-------|
| `HRBCCCDCModel.cs` | Model d? li?u báo cáo (`HRBCCCDCModel`, `HRBCCCDCRow`) |
| `HRBCCCDCCpn.razor` | Blazor component: form l?c, l??i d? li?u, xu?t, in |

## ?i?u ki?n l?c
- **??n ngày**: ngày c?n xem báo cáo (b?t bu?c)
- **Phòng ban**: l?c theo `dmbpns`
- **Nhân viên**: l?c theo `dmns`

## Stored Procedure
G?i SP t? `CommandInfo["store_proc"]` v?i tham s?:
- `@denngay` – ??n ngày
- `@filter` – ?i?u ki?n WHERE ??ng

## VBrowse
```
ma_ns:100:h=Mã nhân viên CP;ten_ns_cp:100:h=Nhân viên CP;ma_cc_me_cp:100:h=Mã CCDC CP;
ten_cc_cp:120:h=Tên CCDC;so_luong_cp:100:h=S? l??ng CP;ngay_cap_cp:120:h=Ngày c?p phát:D;
ngay_het_han_cp:120:h=Ngày h?t h?n:D;ghi_chu_cp:150:h=Ghi chú;ten_loai:100:h=Lo?i;
ngay_ct:120:h=Ngày CT:D;ma_cc_me_th:100:h=Mã CCDC;ten_cc_th:120:h=Tên CCDC;
so_luong_th:100:h=S? l??ng:N0;ma_ns_tu:100:h=T? nhân viên;ten_ns_tu:100:h=Nhân viên;
ma_ns_sang:100:h=Sang nhân viên;ten_ns_sang:100:h=Nhân viên;ngay_cap:120:h=Ngày c?p:D;
ly_do_cap:150:h=Lý do c?p;ngay_dc:120:h=Ngày ?C;ly_do_dc:150:h=Lý do ?C
```

## Ngôn ng?
C?p nh?t file `SISERPSME/wwwroot/Lang/fx-vi.xml` v?i các key `HRBCCCDC`.
