# HRTG - Danh m?c Tôn giáo

## Thông tin
- **Menu_id**: `wr.dm.12`
- **Page**: `hrtg`
- **B?ng d? li?u**: `dmtg` / View: `v_dmtg`
- **Khóa chính**: `ma_tg`

## Các file
| File | Mô t? |
|------|--------|
| `Dmtg.cs` | Model class ánh x? b?ng `dmtg` |
| `HrtgComponent.razor` | Trang danh sách chính (`/hrtg`) |
| `EditHrtg.razor` | Form thêm/s?a/xem |
| `FrmHrtg.razor` | Popup wrapper (`/frmhrtg`) |

## Ch?c n?ng
- Thêm, S?a, Xem, Copy, ??i mã, K?t xu?t, Xóa
- Validate: mã và tên b?t bu?c; không xóa khi ?ã phát sinh (`CheckDeleteListId`)
- Ngôn ng?: ti?ng Vi?t/Anh qua `hr-vi.xml`
