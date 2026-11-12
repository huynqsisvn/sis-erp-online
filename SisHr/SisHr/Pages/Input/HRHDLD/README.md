# HRHDLD - Hợp đồng lao động / Labour Contract

**Menu_id:** `wr.ct.05`  
**Page route:** `/HRHDLD`  
**Table:** `dmhdld` / View: `v_dmhdld`  
**Key field:** `ma_hdld`

## Files
| File | Mô tả |
|---|---|
| `Hdld.cs` | Model entity |
| `HdldModel.cs` | Report model |
| `DMHDLD.cs` / `DMHDLD.Designer.cs` | DevExpress XtraReport |
| `FrmHrhdld.razor` | Trang danh sách (browse) |
| `EditHrhdld.razor` | Form thêm/sửa/xem |

## Chức năng
- Thêm (F4), Sửa (F3), Xem (F2), Xóa (F8), In (F7), Kết xuất Excel

## Validate
- Số hợp đồng, Ông/Bà đại diện, Nhân viên: bắt buộc
- Từ ngày ≤ Đến ngày
- Ngày hiệu lực ≥ Ngày hợp đồng
