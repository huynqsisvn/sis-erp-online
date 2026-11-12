using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SisCom;
using SisCom.Model;
using SisCom.Shared;
using SisHr.Pages.Input.DMXNNS;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Data;
using System.Threading.Tasks;

namespace SisHr.Pages.Input.DMXNNS
{
    public partial class FrmDmxnns
    {
        // ---- Tham chiếu thành phần ----
        private DxGrid GridCT;
        private object SelectedCt;
        public ElementReference GridWrapperCt;

        private DotNetObjectReference<FrmDmxnns> Pageobj;
        private string LastFocusedGrid = "CT";

        // ---- Dữ liệu ----
        public DmxnnsRow CurPh { get; set; } = new DmxnnsRow();
        public List<DmxnnsDtRow> LsCt { get; set; } = new List<DmxnnsDtRow>();

        // ---- Helper: so sánh curActionTask qua chuỗi (tránh reference trực tiếp ActionTask enum) ----
        private bool IsNew   => curActionTask.ToString() == "New";
        private bool IsEdit  => curActionTask.ToString() == "Edit";
        private bool IsView  => curActionTask.ToString() == "View";

        protected override void OnInitialized()
        {
            sqlTableName = "DMXNNS";
            sqlTableView = "v_DMXNNS";
            SqlTableKey = "ma_ns";
            CheckSqlTableKey = "ma_ns";
            ParentPage = string.IsNullOrEmpty(ParentPage) ? "dmxnns" : ParentPage;
            CurPh = new DmxnnsRow();
            LsCt = new List<DmxnnsDtRow>();

            if (string.IsNullOrEmpty(Menu_id))
                Menu_id = "wr.ct.10";

            base.OnInitialized();
        }

        private void CreateNewDM()
        {
            CurPh = new DmxnnsRow();
            CurPh.Ngay_ct = DateTime.Now.Date;
            CurPh.Dia_chi_giao = "";  // Đảm bảo không NULL
            CurPh.Noi_dung = "";
            CurPh.Ghi_chu = "";
            LsCt = new List<DmxnnsDtRow>();

            DmxnnsDtRow newRow = new DmxnnsDtRow();
            newRow.Ngay_ct = DateTime.Now.Date;
            newRow.Ban_sao = false;
            newRow.Ban_chinh = false;
            newRow.Stt = 1;
            LsCt.Add(newRow);

            AllowEditMode = true;
            AllowEditMa = true;
        }

        private async Task LoadData(string key)
        {
            try
            {
                string safeMa = key.Trim().Replace("'", "''");

                string sqlPh = "SELECT * FROM v_DMXNNS WHERE ma_ns = '" + safeMa + "'";
                DataSet dsPh = await myDb.LoadDataset(sqlPh);
                if (dsPh != null && dsPh.Tables[0].Rows.Count > 0)
                {
                    List<DmxnnsRow> list = myDb.ConvertDataTable<DmxnnsRow>(dsPh.Tables[0]);
                    if (list != null && list.Count > 0)
                    {
                        CurPh = list[0];
                        // Đảm bảo các trường bắt buộc không NULL - điều này QUAN TRỌNG để tránh lỗi INSERT
                        CurPh.Dia_chi_giao = string.IsNullOrWhiteSpace(CurPh.Dia_chi_giao) ? "" : CurPh.Dia_chi_giao;
                        CurPh.Noi_dung = string.IsNullOrWhiteSpace(CurPh.Noi_dung) ? "" : CurPh.Noi_dung;
                        CurPh.Ghi_chu = string.IsNullOrWhiteSpace(CurPh.Ghi_chu) ? "" : CurPh.Ghi_chu;
                        LoadLookupNames();
                    }
                }

                string sqlDt = "SELECT * FROM v_DMXNNSdt WHERE ma_ns = '" + safeMa + "'";
                DataSet dsDt = await myDb.LoadDataset(sqlDt);
                if (dsDt != null && dsDt.Tables[0].Rows.Count > 0)
                {
                    LsCt = myDb.ConvertDataTable<DmxnnsDtRow>(dsDt.Tables[0]);
                    int stt = 1;
                    foreach (var ct in LsCt)
                        ct.Stt = stt++;
                }

                // Set permission dựa theo ActionTask
                if (IsView)
                {
                    AllowEditMa = false;
                    AllowEditMode = false;
                }
                else if (IsEdit)
                {
                    AllowEditMode = true;
                    AllowEditMa = false;
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao("Data load error: " + ex.Message);
                StateHasChanged();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (firstRender)
                {
                    // Xử lý load dữ liệu dựa theo ActionTask
                    string action = curActionTask.ToString();
                    if (action == "View" || action == "Edit")
                    {
                        if (!string.IsNullOrEmpty(Ma_ma))
                            await LoadData(Ma_ma);
                    }
                    else if (action == "New")
                    {
                        CreateNewDM();
                    }

                    // Focus vào input field đầu tiên
                    await JavacriptS.InvokeVoidAsync("eval", "document.getElementById('ma_ns_input')?.focus()");

                    // Setup JS interop
                    Pageobj = DotNetObjectReference.Create(this);
                    await JavacriptS.InvokeVoidAsync("SetPageObj", Pageobj);
                    await JavacriptS.InvokeVoidAsync("AttachEnterAsTab");

                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void LoadLookupNames()
        {
            if (!string.IsNullOrWhiteSpace(CurPh.Ma_ns))
            {
                string safe = CurPh.Ma_ns.Trim().Replace("'", "''");
                CurPh.Ten_ns_giao = myDb.GetValue("SELECT ten_ns FROM dmns WHERE ma_ns = '" + safe + "'")?.ToString() ?? "";
                CurPh.Ma_bpns_giao = myDb.GetValue("SELECT ma_bpns FROM dmns WHERE ma_ns = '" + safe + "'")?.ToString() ?? "";
            }

            if (!string.IsNullOrWhiteSpace(CurPh.Ma_ns_nhan))
            {
                string safe = CurPh.Ma_ns_nhan.Trim().Replace("'", "''");
                CurPh.Ten_ns_nhan_display = myDb.GetValue("SELECT ten_ns FROM dmns WHERE ma_ns = '" + safe + "'")?.ToString() ?? "";
                CurPh.Ma_bpns_nhan = myDb.GetValue("SELECT ma_bpns FROM dmns WHERE ma_ns = '" + safe + "'")?.ToString() ?? "";
            }
        }

        void Ma_ns_Changed(ExpandoObject value)
        {
            CurPh.Ten_ns_giao = "";
            CurPh.Ma_bpns_giao = "";
            if (!string.IsNullOrWhiteSpace(CurPh.Ma_ns))
            {
                string safe = CurPh.Ma_ns.Trim().Replace("'", "''");
                CurPh.Ten_ns_giao = myDb.GetValue("SELECT ten_ns FROM dmns WHERE ma_ns = '" + safe + "'")?.ToString() ?? "";
                CurPh.Ma_bpns_giao = myDb.GetValue("SELECT ma_bpns FROM dmns WHERE ma_ns = '" + safe + "'")?.ToString() ?? "";
            }
            StateHasChanged();
        }

        void Ma_ns_nhan_Changed(ExpandoObject value)
        {
            CurPh.Ten_ns_nhan_display = "";
            CurPh.Ma_bpns_nhan = "";
            if (!string.IsNullOrWhiteSpace(CurPh.Ma_ns_nhan))
            {
                string safe = CurPh.Ma_ns_nhan.Trim().Replace("'", "''");
                CurPh.Ten_ns_nhan_display = myDb.GetValue("SELECT ten_ns FROM dmns WHERE ma_ns = '" + safe + "'")?.ToString() ?? "";
                CurPh.Ma_bpns_nhan = myDb.GetValue("SELECT ma_bpns FROM dmns WHERE ma_ns = '" + safe + "'")?.ToString() ?? "";
            }
            StateHasChanged();
        }
        private void CopyDmxnnsDt(DmxnnsDtRow src, DmxnnsDtRow dst)
        {
            dst.Ten_giayto = src.Ten_giayto;
            dst.Ban_chinh = src.Ban_chinh;
            dst.Ban_sao = src.Ban_sao;
            dst.So_luong = src.So_luong;
            dst.Ngay_ct = src.Ngay_ct;
            dst.Ghi_chu = src.Ghi_chu;
        }

        async void StartEditRow()
        {
            if (GridCT != null && SelectedCt != null && AllowEditMode)
            {
                if (GridCT.IsEditing())
                    await GridCT.SaveChangesAsync();
                await GridCT.StartEditDataItemAsync(SelectedCt);
            }
        }

        void Grid_CustomizeEditModel(GridCustomizeEditModelEventArgs e)
        {
            if (e.IsNew)
            {
                var m = (DmxnnsDtRow)e.EditModel;
                m.Stt = LsCt.Count > 0 ? LsCt.Max(a => a.Stt) + 1 : 1;
                m.Ngay_ct = DateTime.Now.Date;
                m.Ban_sao = false;
                m.Ban_chinh = false;
            }
        }

        async Task Grid_EditModelSaving(GridEditModelSavingEventArgs e)
        {
            var edit = (DmxnnsDtRow)e.EditModel;

            // Nếu thêm mới mà Tên giấy tờ trống thì không lưu (Giống logic chặn Ma_cc bên FXCC)
            if (e.IsNew && string.IsNullOrWhiteSpace(edit.Ten_giayto)) return;

            if (e.IsNew)
            {
                var newItem = new DmxnnsDtRow();
                CopyDmxnnsDt(edit, newItem);
                newItem.Stt = LsCt.Count + 1;
                LsCt.Add(newItem);

                // LOGIC QUAN TRỌNG: Xóa dòng placeholder chưa có dữ liệu (giống hệt FXCC)
                if (LsCt.Any(x => !string.IsNullOrWhiteSpace(x.Ten_giayto) && x != newItem))
                {
                    LsCt.RemoveAll(x => string.IsNullOrWhiteSpace(x.Ten_giayto) && x != newItem);
                }

                SelectedCt = newItem;
            }
            else
            {
                var dataItem = (DmxnnsDtRow)e.DataItem;
                CopyDmxnnsDt(edit, dataItem);
                SelectedCt = dataItem;
            }

            // Cập nhật lại STT
            int stt = 1;
            LsCt.ForEach(x => x.Stt = stt++);

            await InvokeAsync(StateHasChanged);
        }
        // Hàm xử lý chung giống FXCC
        async Task HandleSaveAndMoveNext(DxGrid grid, List<DmxnnsDtRow> list, object selectedItem, Action onAddAction)
        {
            if (grid != null && grid.IsEditing())
                await grid.SaveChangesAsync();

            await Task.Delay(100); // Bắt buộc delay để lưới render xong

            if (selectedItem != null && selectedItem is DmxnnsDtRow currentItem)
            {
                var index = list.IndexOf(currentItem);
                // Nếu chưa phải dòng cuối -> Xuống dòng kế tiếp
                if (index >= 0 && index < list.Count - 1)
                    await grid.StartEditDataItemAsync(list[index + 1]);
                else
                    onAddAction(); // Dòng cuối -> Thêm dòng mới
            }
            else
            {
                onAddAction();
            }
        }

        // Bắt sự kiện Javascript gọi
        [JSInvokable]
        public async Task SaveAndMoveNextRow()
        {
            await HandleSaveAndMoveNext(GridCT, LsCt, SelectedCt, AddNewRow);
        }

        // Hàm thêm dòng
        async void AddNewRow()
        {
            if (AllowEditMode)
            {
                if (GridCT != null && GridCT.IsEditing()) await GridCT.SaveChangesAsync();
                await GridCT.StartEditNewRowAsync();
            }
        }

        // Bắt phím tắt trên lưới
        private async Task GridCT_OnKeyDown(KeyboardEventArgs e)
        {
            if (!AllowEditMode) return;
            switch (e.Key)
            {
                case "Enter":
                    await SaveAndMoveNextRow();
                    break;
                case "F4":
                    AddNewRow();
                    break;
                case "F8":
                    if (SelectedCt != null) GridCT.ShowDataItemDeleteConfirmation(SelectedCt);
                    break;
                case "Escape":
                    try { await GridCT.CancelEditAsync(); GridCT.ClearSelection(); } catch { }
                    break;
            }
        }

        // Hàm Javascript kích hoạt lưới khi focus
        [JSInvokable]
        public void CallGridEditMode(string value)
        {
            if (AllowEditMode)
            {
                AddNewRow();
                StateHasChanged();
            }
        }

        async Task Grid_DataItemDeleting(GridDataItemDeletingEventArgs e)
        {
            var item = (DmxnnsDtRow)e.DataItem;
            LsCt.Remove(item);
            int stt = 1;
            foreach (var ct in LsCt)
                ct.Stt = stt++;
        }

        async void OnGridCtLostFocus(FocusEventArgs e)
        {
            try
            {
                // Ch? 150ms ?? nh??ng ???ng cho s? ki?n Click c?a nút L?u trên Form ch?y tr??c
                await Task.Delay(150);

                bool inside = await JavacriptS.InvokeAsync<bool>("focusInside", "grid-wrapper-ct");
                if (!inside && GridCT != null && GridCT.IsEditing())
                    await GridCT.SaveChangesAsync();
            }
            catch { }
        }

        async Task SaveData()
        {
            try
            {
                // 1. Dừng 200ms để nhường đường cho LostFocus event và Binding dữ liệu
                await Task.Delay(200);
                
                if (GridCT != null && GridCT.IsEditing())
                    await GridCT.SaveChangesAsync();

                // 2. Dừng thêm 100ms để Grid_EditModelSaving có thời gian Add dòng mới
                await Task.Delay(100);

                if (!CheckValid())
                    return;

                // Remove empty rows (rows without ten_giayto)
                LsCt.RemoveAll(x => string.IsNullOrWhiteSpace(x.Ten_giayto));

                // Đảm bảo các trường bắt buộc không NULL - TRƯỚC khi insert/update
                // Đảm bảo xử lý đúng tên cột dia_chi trước khi Insert/Update
                CurPh.Dia_chi = string.IsNullOrWhiteSpace(CurPh.Dia_chi) ? "" : CurPh.Dia_chi.Trim();
                CurPh.Noi_dung = string.IsNullOrWhiteSpace(CurPh.Noi_dung) ? "" : CurPh.Noi_dung.Trim();
                CurPh.Ghi_chu = string.IsNullOrWhiteSpace(CurPh.Ghi_chu) ? "" : CurPh.Ghi_chu.Trim();
                CurPh.Ma_ns = string.IsNullOrWhiteSpace(CurPh.Ma_ns) ? "" : CurPh.Ma_ns.Trim();
                
                // Process ban_chinh / ban_sao -> so_luong_banchinh / so_luong_bansao
                int userId = curUser != null ? curUser.User_id : 0;
                string userName = curUser != null ? curUser.User_name : "";
                DateTime now = DateTime.Now;

                foreach (var ct in LsCt)
                {
                    ct.So_luong_banchinh = ct.Ban_chinh ? 1 : 0;
                    ct.So_luong_bansao = ct.Ban_sao ? 1 : 0;
                    ct.Ma_ns = CurPh.Ma_ns;
                }

                string safeMa = CurPh.Ma_ns.Trim().Replace("'", "''");

                if (curActionTask.ToString() == "New")
                {
                    // Check duplicate
                    string sqlCheck = "SELECT COUNT(1) FROM DMXNNS WHERE ma_ns='" + safeMa + "'";
                    DataTable dtChk = myDb.GetTable(sqlCheck);
                    if (dtChk != null && dtChk.Rows.Count > 0 && Convert.ToInt32(dtChk.Rows[0][0]) > 0)
                    {
                        myStateMN.SetNewThongbao(Lap["Duplicate employee code"] + "!");
                        await JavacriptS.InvokeVoidAsync("eval", "document.getElementById('ma_ns_input')?.focus()");
                        return;
                    }

                    CurPh.Date0 = now.Date;
                    CurPh.Time0 = now.ToString("HH:mm:ss");
                    CurPh.Date = now.Date;
                    CurPh.Time = now.ToString("HH:mm:ss");
                    CurPh.User_id0 = userId;
                    CurPh.User_name0 = userName;
                    CurPh.User_id = userId;
                    CurPh.User_name = userName;

                    await myDb.InsertOneRow<DmxnnsRow>(CurPh, "DMXNNS");

                    foreach (var ct in LsCt)
                    {
                        ct.Date0 = now.Date;
                        ct.Time0 = now.ToString("HH:mm:ss");
                        ct.Date = now.Date;
                        ct.Time = now.ToString("HH:mm:ss");
                        ct.User_id0 = userId;
                        ct.User_name0 = userName;
                        ct.User_id = userId;
                        ct.User_name = userName;
                        await myDb.InsertOneRow<DmxnnsDtRow>(ct, "DMXNNSDT");
                    }

                    myStateMN.SetNewThongbao(Lap["Successfully added new"] + ": " + CurPh.Ma_ns);
                }
                else if (curActionTask.ToString() == "Edit")
                {
                    CurPh.Date = now.Date;
                    CurPh.Time = now.ToString("HH:mm:ss");
                    CurPh.User_id = userId;
                    CurPh.User_name = userName;

                    string whereClause = "ma_ns = '" + safeMa + "'";
                    await myDb.UpdateOneRow1<DmxnnsRow>(CurPh, "DMXNNS", "", whereClause);

                    // Delete old details and re-insert
                    await myDb.ExecuteSqlQueryAsync("DELETE FROM DMXNNSDT WHERE ma_ns='" + safeMa + "'");

                    foreach (var ct in LsCt)
                    {
                        ct.Ma_ns = CurPh.Ma_ns;
                        ct.Date0 = now.Date;
                        ct.Time0 = now.ToString("HH:mm:ss");
                        ct.Date = now.Date;
                        ct.Time = now.ToString("HH:mm:ss");
                        ct.User_id0 = userId;
                        ct.User_name0 = userName;
                        ct.User_id = userId;
                        ct.User_name = userName;
                        await myDb.InsertOneRow<DmxnnsDtRow>(ct, "DMXNNSDT");
                    }

                    myStateMN.SetNewThongbao(Lap["Edit successfully"] + ": " + CurPh.Ma_ns);
                }

                if (DxWindowmodel != null)
                    myModal.CloseModal(DxWindowmodel);

                if (ShowAdd == true)
                    OnClose.InvokeAsync();
                else
                    myNavi.NavigateTo(ParentPage);
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao(Lap["Save failed"] + ": " + ex.Message);
                StateHasChanged();
            }
        }

        bool CheckValid()
        {
            if (string.IsNullOrWhiteSpace(CurPh.Ma_ns))
            {
                myStateMN.SetNewThongbao(Lap["Employee code is required"] + "!");
                return false;
            }

            if (CurPh.Ngay_ct == null)
            {
                myStateMN.SetNewThongbao(Lap["Voucher date is required"] + "!");
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            Pageobj?.Dispose();
        }
    }
}
