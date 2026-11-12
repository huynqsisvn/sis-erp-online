# HRCD - Danh m?c ch?c danh (Position Title)

- **Menu_id**: `wr.dm.29`
- **Page**: `/hrcd`
- **Table**: `dmcd`
- **View**: `v_dmcd`
- **Key**: `ma_cd`

## Ch?c n?ng

- Thêm, s?a, xem, copy, ??i mã, k?t xu?t, xóa ch?c danh.
- Validate không ???c xóa nh?ng ch?c danh ?ã phát sinh (s? d?ng `dbo.CheckDeleteListId`).

## C?u trúc file

| File | Mô t? |
|------|-------|
| `Dmcd.cs` | Model d? li?u |
| `HrcdComponent.razor` | Component danh sách (browse grid) |
| `EditHrcd.razor` | Component form thêm/s?a/xem |
| `FrmHrcd.razor` | Popup wrapper |
| `README.md` | File này |
