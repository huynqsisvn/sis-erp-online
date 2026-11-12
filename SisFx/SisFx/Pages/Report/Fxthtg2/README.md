# Fxthtg2 - B?ng t?ng h?p t?ng (gi?m) CCDC nhóm theo 2 ch? tiêu

## Thông tin
- **Menu ID**: wf.xb.24
- **Route**: `/fxthtg2`
- **Store Procedure**: `Fxthtg2`
- **Params**: `@Loai, @nam, @tuKy, @denKy, @ma_bpcc, @Ma_tg_cc, @Nhom, @Chi_tiet, @condition`

## C?u trúc
```
Pages/Report/Fxthtg2/
??? Fxthtg2.cs          – Model (Fxthtg2Model, Fxthtg2Row)
??? Fxthtg2Cpn.razor    – Blazor component (form l?c + browse)
??? README.md
```

## Form l?c
- N?m, T? k?, ??n k?
- Ki?u báo cáo: 1 – T?ng, 2 – Gi?m
- Lý do t?ng/gi?m (`dmtgts`)
- Nhóm theo (1–9), Chi ti?t theo (1–9) — ph?i khác nhau
- M?u VND/Ngo?i t? (read-only)
- Nâng cao: nhóm cc, lo?i cc, BPSD, TK CP, nhóm cc 1/2/3

## Vbrowse
`stt:45:h=Stt:FL:HR;ma2:120:h=Mã nhóm:FL;ma:100:h=Mã ch? tiêu:FL;ten:300:h=Tên ch? tiêu:S=T?ng c?ng&HC:FL;nguyen_gia:130:h=Nguyên giá:SF=tag equal 0:N0:I0;gt_da_kh:130:h=Giá tr? ?ã KH:SF=tag equal 0:N0:I0;gt_cl:130:h=Giá tr? còn l?i:SF=tag equal 0:N0:I0`

## Ch?c n?ng
- L?c d? li?u theo b? l?c
- Thay ??i ?VCS trên thanh tiêu ?? ? d? li?u t? c?p nh?t
- K?t xu?t Excel (Export)
