# DMVPNV - Nhân viên vi ph?m

**Menu_id:** `wr.ct.15`  
**Page route:** `/dmvpnv`  
**Th? m?c:** `SisHr/SisHr/Pages/Input/DMVPNV/`

## Các file

| File | Mô t? |
|---|---|
| `Dmvpnv.cs` | Model: `DmvpnvRow` (header), `DmvpnvDtRow` (detail) |
| `FrmDmvpnv.razor` | Form nh?p li?u (header + l??i chi ti?t) |
| `FrmDmvpnv.razor.cs` | Code-behind form: load, save, validate, grid logic |
| `DmvpnvComponent.razor` | Trang danh sách, toolbar, thêm/s?a/xem/xóa/k?t xu?t |

## B?ng d? li?u

- **Header:** `DMVPNV` — view: `v_DMVPNV`
- **Detail:** `DMVPNVdt` — view: `v_DMVPNVdt`
- **Browse view:** `v_DMVPNVchitiet`
- **Khóa chính:** `so_ct`

## Ch?c n?ng

- **Thêm (F4):** T?o s? ch?ng t? t? ??ng, nh?p header + l??i chi ti?t vi ph?m
- **S?a (F3):** Load d? li?u, xóa detail c? và insert l?i
- **Xem (F2):** Ch? ??c
- **Xóa (F8):** Xóa detail tr??c, sau ?ó xóa header
- **K?t xu?t:** Export danh sách

## Validate

- S? ch?ng t? b?t bu?c, không trùng (khi thêm m?i)
- Ngày ch?ng t? b?t bu?c
- Mã nhân viên b?t bu?c
- Dòng l??i không có `noi_dung_ky_luat` s? b? b? qua khi l?u

## Ngôn ng?

Các Lap key ???c thêm vào `wwwroot/Lang/hr-vi.xml` và `hr-en.xml`.
