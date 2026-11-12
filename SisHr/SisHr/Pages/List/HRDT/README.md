# HRDT - Danh m?c dân t?c (Ethnic)

- **Menu_id**: `wr.dm.25`
- **Page**: `/hrdt`
- **Table**: `dmdt`
- **View**: `v_dmdt`
- **Key**: `ma_dt`

## Ch?c n?ng

- Thêm, s?a, xem, copy, ??i mã, k?t xu?t, xóa dân t?c.
- Khi thay ??i DVCS trên thanh tiêu ??, l??i d? li?u t? ??ng l?c theo DVCS ?ó.
- Validate không ???c xóa nh?ng dân t?c ?ã phát sinh (s? d?ng `dbo.CheckDeleteListId`).

## C?u trúc file

| File | Mô t? |
|------|-------|
| `Dmdt.cs` | Model d? li?u |
| `HrdtComponent.razor` | Component danh sách (browse grid) |
| `EditHrdt.razor` | Component form thêm/s?a/xem |
| `FrmHrdt.razor` | Popup wrapper |
| `README.md` | File này |
