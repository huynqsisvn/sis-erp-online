# FXBCTANG – B?ng kê CCDC t?ng trong k?

**Menu_id:** `wf.xb.21`  
**Route:** `/fxbctang`  
**Store Procedure:** `FXBCTANG`

## ?i?u ki?n l?c
- N?m, T? k?, ??n k?
- Lý do t?ng (`dmtgcc`, `loai_tg_cc=1`)
- Ki?u nhóm s? li?u: 0 – Không nhóm, 1 – Theo lo?i cc, 2 – Theo BPSD, 3 – Theo nhóm cc, 4 – Theo TK chi phí
- Chi ti?t s? li?u: 1 – ??n s? th?, 2 – ??n BPSD, 3 – ??n mã CCDC
- M?u VND/Ngo?i t? (read-only)
- Nâng cao: nhóm cc, lo?i cc, BPSD, TK CP, nhóm cc 1/2/3

## Ch?c n?ng
- **K?t xu?t Excel** (Export)
- **In** (Print) – s? d?ng report template V/E trong th? m?c `V/` và `E/`

## C?u trúc th? m?c
```
FXBCTANG/
  FXBCTANG.cs          – Model (FXBCTANGModel, FXBCTANGRow)
  FXBCTANGCpn.razor    – Blazor component chính
  V/FXBCTANG.cs + Designer.cs  – M?u in ti?ng Vi?t
  E/FXBCTANG.cs + Designer.cs  – M?u in ti?ng Anh
```
