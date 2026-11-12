# FXBCCCTH1 - Bảng tổng hợp giá trị CCDC nhóm theo 2 chỉ tiêu

## Thông tin chức năng
- **Menu_id**: `wf.xb.14`
- **Page route**: `/fxbcccth1`
- **Store Procedure**: `FxBcCcTh1`

## Cấu trúc thư mục
```
Pages/Report/FXBCCCTH1/
├── FXBCCCTH1.cs          # Model (FXBCCCTH1Model, FXBCCCTH1Row)
├── FXBCCCTH1Cpn.razor    # Blazor component (form lọc + browse)
├── V/
│   ├── FXBCCCTH1.cs      # Report class (Tiếng Việt)
│   └── FXBCCCTH1.Designer.cs
└── E/
    ├── FXBCCCTH1.cs      # Report class (Tiếng Anh)
    └── FXBCCCTH1.Designer.cs
```

## Form lọc
- Năm, Kỳ (1-12), Số đầu kỳ/cuối kỳ
- Nhóm theo (8 lựa chọn), Chi tiết theo (4 lựa chọn)
- Mẫu VND/Ngoại tệ (readonly)
- Điều kiện nâng cao: nhóm cc, loại cc, bpsd, tk cp, nhóm cc 1/2/3

## Vbrowse
`stt:45:h=Stt:FL:HR;ma:100:h=Mã chỉ tiêu:FL;ten:300:h=Tên chỉ tiêu:S=Tổng cộng&HC:FL;ngia1:130:h=Nguyên giá:SF=ftag equal 0:N0:I0;gt_da_kh1:130:h=Giá trị đã p.bổ:SF=ftag equal 0:N0:I0;gt_cl1:130:h=Giá trị còn lại:SF=ftag equal 0:N0:I0`

## Chức năng
- Lọc dữ liệu theo bộ lọc
- Kết xuất Excel (Export)
- In báo cáo (Print) — cột: STT, Mã chỉ tiêu, Tên chỉ tiêu, Nguyên giá, GT đã phân bổ, GT còn lại
