# HRCV - Danh m?c ch?c v? (Position List)

- **Menu_id**: `wr.dm.03`
- **Page**: `/hrcv`
- **Table**: `dmcv`
- **View**: `v_dmcv`
- **Key**: `ma_cv`

## Ch?c n?ng

- Thêm, s?a, xem, copy, ??i mã, k?t xu?t, xóa ch?c v?.
- Validate không ???c xóa nh?ng ch?c v? ?ã phát sinh (s? d?ng `dbo.CheckDeleteListId`).

## C?u trúc file

| File | Mô t? |
|------|-------|
| `Dmcv.cs` | Model d? li?u |
| `HrcvComponent.razor` | Component danh sách (browse grid) |
| `EditHrcv.razor` | Component form thêm/s?a/xem |
| `FrmHrcv.razor` | Popup wrapper |
| `README.md` | File này |
