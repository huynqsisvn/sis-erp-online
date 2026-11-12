# FxSocc1B – S? CCDC t?i n?i s? d?ng

## Thông tin chung
- **Menu_id**: `wf.xb.18`
- **Trang**: `/fxsocc1b`
- **Stored Procedure**: `FxSocc1B`

## ?i?u ki?n l?c
- N?m
- Mã BPTS (B? ph?n s? d?ng - b?t bu?c) — t? b?ng dmbpts
- Không hi?n các CCDC gi?m tr??c ngày
- Ngày m? s?
- M?u VND/Ngo?i t? (read-only, m?c ??nh VND)
- L?c nâng cao: nhóm công c?, lo?i cc, TK chi phí, nhóm cc 1/2/3

## Ch?c n?ng
- K?t xu?t Excel (Export)
- In báo cáo (Print) — t? ??ng xu?t PDF theo ngôn ng? hi?n t?i (V/ ho?c E/)
- Thay ??i ?VCS trên thanh tiêu ?? ? d? li?u l??i t? c?p nh?t

## C?u trúc file

| File | Mô t? |
|------|-------|
| `FxSocc1BModel.cs` | Model d? li?u (FxSocc1BModel, FxSocc1BRow) — dùng cho browse grid |
| `FxSocc1BPrintModel.cs` | Model d? li?u in (FxSocc1BPrintModel, FxSocc1BPrintRow) — dùng cho report |
| `FxSocc1BComponentCpn.razor` | Component Blazor: form l?c + màn hình brow + ch?c n?ng in/export |
| `V/FXSOCC1B_TT200.cs` | M?u in ti?ng Vi?t (code-behind) |
| `V/FXSOCC1B_TT200.Designer.cs` | M?u in ti?ng Vi?t (designer) |
| `E/FXSOCC1B_TT200.cs` | M?u in ti?ng Anh (code-behind) |
| `E/FXSOCC1B_TT200.Designer.cs` | M?u in ti?ng Anh (designer) |
| `README.md` | File này |

## Khác bi?t v?i FxSocc1

- Form l?c b?t bu?c nh?p **Mã BPTS** (b? ph?n s? d?ng - t? b?ng dmbpts)
- Không có tr??ng **Mã BPCC** trong ?i?u ki?n l?c
- Stored Procedure dùng tham s? `@Ma_bpsd` thay vì `@Ma_bpcc`
