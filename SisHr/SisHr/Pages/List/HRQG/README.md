# HRQG - Qu?c gia

## Thông tin
- **Menu_id**: `wr.dm.14`
- **Trang**: `hrqg`
- **B?ng**: `dmqg`, **View**: `v_dmqg`, **Khóa**: `ma_quocgia`

## Các file
| File | Mô t? |
|------|-------|
| `Dmqg.cs` | Model d? li?u |
| `HrqgComponent.razor` | Trang danh sách (`/hrqg`) |
| `FrmHrqg.razor` | Popup wrapper (`/frmhrqg`) |
| `EditHrqg.razor` | Form thêm/s?a/xem |

## Ch?c n?ng
- Thêm, s?a, xem, copy, ??i mã, k?t xu?t, xóa
- Validate: mã và tên không ???c ?? tr?ng
- Không xóa n?u ?ã có phát sinh (`CheckDeleteListId`)
- H? tr? song ng? Vi?t / Anh
