# Cập nhật hệ số quy đổi sản phẩm (COHSQD)

## Mô tả
Module cập nhật hệ số quy đổi sản phẩm theo từng kỳ trong năm.

## Thông tin
- **Menu ID**: wo.gc.02
- **Page**: cohsqd
- **Bảng dữ liệu**: cosxlt-dmhssp
- **View**: v_cosxlt-dmhssp

## Cấu trúc Files
```
SisCo\SisCo\Pages\Input\Cohsqd\
├── Cohsqd.cs                  # Model class
├── CohsqdComponent.razor      # Component chính (danh sách)
├── EditCohsqd.razor           # Form thêm/sửa
└── README.md                  # Tài liệu
```

## Chức năng
1. **Xem danh sách** hệ số quy đổi sản phẩm
2. **Thêm mới** hệ số quy đổi cho sản phẩm
3. **Sửa** thông tin hệ số quy đổi
4. **Xóa** hệ số quy đổi
5. **Copy** từ bản ghi khác
6. **Import/Export** dữ liệu
7. **Chuyển hệ số** giữa các kỳ (F10)

## Các trường dữ liệu chính
- `ma_sp`: Mã sản phẩm (key)
- `nam`: Năm (key)
- `ma_sp0`: Mã sản phẩm chuẩn
- `he_so01` - `he_so12`: Hệ số quy đổi theo 12 kỳ
- `status`: Trạng thái

## Tính năng đặc biệt
- **Chuyển hệ số**: Cho phép copy hệ số từ kỳ này sang kỳ khác (phím F10)
- Form hiển thị 12 trường hệ số theo từng kỳ trong năm

## Route
- `/cohsqd` - Danh sách
- `/cohsqd/{Menu_id}` - Danh sách với Menu ID

## Quyền truy cập
Yêu cầu role: `admin` hoặc `cohsqd`
