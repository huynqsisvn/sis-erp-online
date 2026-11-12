# FABCGIAM - B?ng t?ng h?p kh?u hao TSC? (theo 2 ch? tiêu)

- Menu ID: wf.bc.12
- Page: fabcgiam
- Mô t?: Component Blazor t??ng t? `FABCKHTH2` nh?ng dùng ?? hi?n th? b?ng t?ng h?p kh?u hao TSC? theo 2 ch? tiêu. D? li?u l?y t? store procedure `FABCGIAM` (?ã có s?n trong h? th?ng).

Files added:
- `FABCGIAM.cs` - report class placeholder (DevExpress report)
- `FABCGIAMCpn.razor` - Blazor component (form l?c + grid + in + export)
- `FABCGIAMModel.cs` - data model and row DTO

Notes:
- Form l?c t??ng t? `FABCKHTH2` v?i: N?m, T? k?/??n k?, Lý do gi?m, Ki?u nhóm, Mã ?VCS, M?u VND/ngo?i t?, ?i?u ki?n l?c nâng cao.
- Không t?o SQL m?i; component g?i stored procedure `FABCGIAM` (store name configurable via command metadata `curCommand`).
- Export và Print ??u s? d?ng chung c? ch? c?a n?n t?ng (toolbar Export/Print).
