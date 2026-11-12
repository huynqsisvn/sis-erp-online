# FXBCKHTH4 - B?ng t?ng h?p chi phí CCDC - theo nhi?u ch? tiêu

## Thông tin
- **Menu ID**: wf.xb.06
- **Page**: fxbckhth4
- **Store Procedure**: FXBCKHTH4 (tham s?: nam, ky1, ky2, ma_bpcc, tk_cp, Nhom, Thoi_kh_ck, Het_kh_ck, advance)

## Files
- **FXBCKHTH4.cs**: Model d? li?u (FXBCKHTH4Model, FXBCKHTH4Row)
- **FXBCKHTH4Cpn.razor**: Component chính (form l?c + l??i k?t qu?)

## Ch?c n?ng
- T?ng h?p chi phí phân b? CCDC theo k?/n?m v?i nhi?u ch? tiêu
- **Tiêu chí t?ng h?p** (có th? ch?n nhi?u qua checkboxes):
  1. Theo lo?i cc (công c?)
  2. Theo bpcc (b? ph?n s? d?ng)
  3. Theo nhóm cc
  4. Theo tk cp (tài kho?n chi phí)
  5. Theo nhóm cc 1/2/3 (phân nhóm con)
- **L?c theo**:
  - Mã b? ph?n s? d?ng (ma_bpcc)
  - Tài kho?n chi phí (tk_cp)
- **Tùy ch?n**:
  - Lên cc ?ã thôi phân b?
  - Lên cc ?ã phân b? h?t
- K?t xu?t Excel (toolbar Export)
- Phím t?t: Ctrl+F (M? b? l?c)

## C?u trúc d? li?u
B?ng k?t qu? ch?a:
- Mã và tên các tiêu chí t?ng h?p (lo?i cc, mã bpcc, nhóm cc, tk cp, nhóm cc 1/2/3)
- Nguyên giá (nguyen_gia)
- Giá tr? khoán k? hi?n t?i (gt_kh_ky)
- Giá tr? ?ã phân b? (gt_da_kh)
- T?ng giá tr? (gt_da_kh1 = gt_da_kh + gt_kh_ky)
- Giá tr? còn l?i (gt_cl)

## L?u ý
- Form l?c ch? ch?a các filter ???c h? tr? b?i SP: `ma_bpcc`, `tk_cp`, `ma_dvcs`
- Tiêu chí t?ng h?p ???c ch?n qua checkbox groups ?? d? dàng thay ??i cách t?ng h?p d? li?u
- Các ?i?u ki?n l?c khác ???c x? lý b?i SP thông qua tham s? `@advance`
