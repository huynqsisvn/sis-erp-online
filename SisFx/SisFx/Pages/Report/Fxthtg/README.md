# Fxthtg - Báo cáo tổng hợp tăng (giảm) CCDC

## Thông tin
- **Menu ID**: wf.xb.23
- **Route**: `/fxthtg`
- **Store Procedure**: `Fxthtg` — params: `@Loai, @Nam, @tuKy, @denKy, @ma_bpcc, @Ma_tg_ts, @Nhom, @condition`

## Cấu trúc
```
Pages/Report/Fxthtg/
├── Fxthtg.cs           – Model (FxthtgModel, FxthtgRow)
├── FxthtgCpn.razor     – Blazor component (form lọc + browse)
├── V/
│   ├── Fxthtg.cs
│   └── Fxthtg.Designer.cs
└── E/
    ├── Fxthtg.cs
    └── Fxthtg.Designer.cs
```

## Điều kiện lọc
- Năm, Từ kỳ, Đến kỳ
- Kiểu báo cáo: 1 – Tăng, 2 – Giảm
- Lý do tăng/giảm (`dmtgts`)
- Kiểu nhóm số liệu (1–9)
- Mẫu VND/Ngoại tệ (read-only)
- Nâng cao: nhóm cc, loại cc, BPSD, TK CP, nhóm cc 1/2/3

## Chức năng
- Kết xuất Excel (Export)
- Thay đổi ĐVCS trên thanh tiêu đề → dữ liệu lưới tự cập nhật
