# HRBP - Danh m?c b? ph?n nhân s? (HR Department)

- **Menu_id**: `wr.dm.01`
- **Page**: `/hrbp`
- **Table**: `DMBPNS`
- **View**: `v_DMBPNS`
- **Key**: `ma_bpns`

## Ch?c n?ng

- Thêm, s?a, xem, copy, ??i mã, k?t xu?t, xóa b? ph?n nhân s?.
- Khi thay ??i ?VCS trên thanh tiêu ??, l??i d? li?u t? ??ng l?c theo ?VCS ?ó.
- Validate không ???c xóa nh?ng b? ph?n ?ã phát sinh (s? d?ng `dbo.CheckDeleteListId`).

## C?u trúc file

| File | Mô t? |
|------|-------|
| `Dmbpns.cs` | Model d? li?u |
| `HrbpComponent.razor` | Component danh sách (browse grid) |
| `EditHrbp.razor` | Component form thêm/s?a/xem |
| `FrmHrbp.razor` | Popup wrapper |
| `README.md` | File này |
