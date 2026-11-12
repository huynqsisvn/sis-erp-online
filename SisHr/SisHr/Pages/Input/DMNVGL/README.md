# DMNVGL - Danh sách nhân viên g?i l??ng

**Menu_id:** `wr.ct.22`  
**Page route:** `/dmnvgl`  
**Th? m?c:** `SisHr/SisHr/Pages/Input/DMNVGL/`

## Các file

| File | Mô t? |
|---|---|
| `Dmnvgl.cs` | Model: `DmnvglRow` |
| `FrmDmnvgl.razor` | Form nh?p li?u (ngày ch?ng t? + l??i nhân viên) |
| `FrmDmnvgl.razor.cs` | Code-behind: load, save, validate, grid logic |
| `DmnvglComponent.razor` | Trang danh sách, toolbar, thêm/s?a/xem/xóa/k?t xu?t |

## B?ng d? li?u

- **B?ng:** `DMNVGL` — view: `v_DMNVGL`
- **Khóa chính:** `ma_ma` (GUID)
- **Các tr??ng chính:** `ma_ma`, `ngay_ct`, `ma_ns`, `dien_giai`

## Ch?c n?ng

- **Thêm (F4):** Nh?p ngày ch?ng t? + l??i nhân viên, m?i dòng t?o GUID m?i
- **S?a (F3):** Load d? li?u, xóa dòng c? theo `ngay_ct` + `ma_ns` r?i insert l?i
- **Xem (F2):** Ch? ??c
- **Xóa (F8):** Xóa theo `ma_ma`
- **K?t xu?t:** Export danh sách

## Validate

- Ngày ch?ng t? b?t bu?c
- Dòng l??i không có `ma_ns` s? b? b? qua khi l?u
