# FABCKHBP2 - B?ng tính kh?u hao TSC? theo bpsd

Thông tin ng?n:
- Menu ID: `wf.bc.03`
- Page: `fabckhbp2`
- Th? m?c: `SisFa/SisFa/Pages/Opening/FABCKHBP2/`

Ch?c n?ng chính:
- Hi?n th? form l?c (n?m, t? k?, ??n k?, lo?i TS, mã TS, mã BPSD, mã ?VCS, VND/ngo?i t?, ch?n lên TS ?ã thôi kh?u hao / ?ã kh?u hao h?t, ki?u xem 1/2)
- Hi?n th? b?ng k?t qu? theo ?i?u ki?n l?c
- K?t xu?t Excel (Export)
- In báo cáo (Print)

Ghi chú:
- D? li?u và stored procedures ?ã có s?n; không c?n t?o query m?i t?i ?ây.
- C?u trúc component t??ng t? các báo cáo khác trong th? m?c `Opening`.

Files (d? ki?n):
- `FABCKHBP2.cs` - model d? li?u
- `FABCKHBP2Cpn.razor` - component chính (form l?c + grid)

Ng??i phát tri?n: Áp d?ng logic và form l?c tham kh?o t? ?ng d?ng WinForms (th? m?c `FABCKHBP2` trong ngu?n m?u).