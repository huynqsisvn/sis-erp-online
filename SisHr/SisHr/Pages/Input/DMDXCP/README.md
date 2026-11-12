# DMDXCP - ?? xu?t chi phí

**Menu_id:** `wr.ct.23`  
**Page route:** `/dmdxcp`  
**Th? m?c:** `SisHr/SisHr/Pages/Input/DMDXCP/`

## Các file

| File | Mô t? |
|---|---|
| `Dmdxcp.cs` | Model: `DmdxcpRow`, `DmdxcpDtRow` |
| `DmdxcpModel.cs` | Model in: `DmdxcpModel`, `DmdxcpDtPrint` |
| `FrmDmdxcp.razor` | Form nh?p li?u (header + l??i chi ti?t) |
| `FrmDmdxcp.razor.cs` | Code-behind: load, save, validate, grid logic |
| `DmdxcpComponent.razor` | Trang danh sách, toolbar, thêm/s?a/xem/xóa/in/k?t xu?t |
| `V/DMDXCP.cs` | Report class |
| `V/DMDXCP.Designer.cs` | Report designer |

## B?ng d? li?u

- **B?ng ph?n ??u:** `DMDXCP` — view: `v_DMDXCP`
- **B?ng chi ti?t:** `DMDXCPdt` — view: `v_DMDXCPdt`
- **View danh sách:** `v_DMDXCPchitiet`
- **Khóa chính:** `so_ct`

## Ch?c n?ng

- **Thêm (F4):** Nh?p s? ch?ng t?, ngày, mã nhân viên + l??i chi ti?t (n?i dung, ?VT, s? l??ng, ??n giá, thành ti?n, ghi chú)
- **S?a (F3):** Load d? li?u, xóa chi ti?t c? r?i insert l?i
- **Xem (F2):** Ch? ??c
- **Xóa (F8):** Xóa chi ti?t tr??c, sau ?ó xóa ph?n ??u
- **In (F7):** In m?u ?? xu?t chi phí
- **K?t xu?t:** Export danh sách

## Validate

- S? ch?ng t? b?t bu?c, không trùng (khi thêm m?i)
- Ngày ch?ng t? b?t bu?c
- Mã nhân viên b?t bu?c
- Dòng l??i không có n?i dung s? b? b? qua khi l?u
- `thanh_tien = so_luong × don_gia` t? ??ng tính
