using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SisCom;
using SisCom.Model;
using SisCom.Shared;
using SisHr.Pages.Input.DMVPNV;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace SisHr.Pages.Input.DMVPNV
{
    public partial class FrmDmvpnv
    {
        // ---- Inject LocalStorage ----
        [Inject] public ILocalStorageService myLcStore { get; set; }
        
        // ---- Grid references ----
        private DxGrid GridCT;
        private object SelectedCt;
        public ElementReference GridWrapperCt;

        private DotNetObjectReference<FrmDmvpnv> Pageobj;
        private string LastFocusedGrid = "CT";

        // ---- Language ----
        public string UserLang = "";
        public bool IsLangEn = false;

        // ---- Data ----
        public DmvpnvRow CurPh { get; set; } = new DmvpnvRow();
        public List<DmvpnvDtRow> LsCt { get; set; } = new List<DmvpnvDtRow>();

        private bool IsNew  => curActionTask.ToString() == "New";
        private bool IsEdit => curActionTask.ToString() == "Edit";
        private bool IsView => curActionTask.ToString() == "View";

        protected override void OnInitialized()
        {
            sqlTableName = "DMVPNV";
            sqlTableView = "v_DMVPNVchitiet";
            SqlTableKey  = "so_ct";
            CheckSqlTableKey = "so_ct";
            ParentPage   = string.IsNullOrEmpty(ParentPage) ? "dmvpnv" : ParentPage;
            CurPh        = new DmvpnvRow();
            LsCt         = new List<DmvpnvDtRow>();

            if (string.IsNullOrEmpty(Menu_id))
                Menu_id = "wr.ct.15";

            base.OnInitialized();
        }

        private void CreateNewRecord()
        {
            CurPh = new DmvpnvRow();
            CurPh.Ngay_ct = DateTime.Now.Date;
            CurPh.So_ct = GenerateSoCt();
            CurPh.Ma_ns = "";
            CurPh.Ten_ns = "";
            LsCt = new List<DmvpnvDtRow>();

            var newRow = new DmvpnvDtRow();
            newRow.Stt = 1;
            LsCt.Add(newRow);

            AllowEditMode = true;
            AllowEditMa = true;
        }

        private string GenerateSoCt()
        {
            var now = DateTime.Now;
            return now.Year.ToString() + now.Month.ToString("D2") + now.Day.ToString("D2") + "01";
        }

        private async Task LoadData(string key)
        {
            try
            {
                string safeSo = key.Trim().Replace("'", "''");

                string sqlPh = "SELECT * FROM v_DMVPNV WHERE so_ct = '" + safeSo + "'";
                DataSet dsPh = await myDb.LoadDataset(sqlPh);
                if (dsPh != null && dsPh.Tables[0].Rows.Count > 0)
                {
                    List<DmvpnvRow> list = myDb.ConvertDataTable<DmvpnvRow>(dsPh.Tables[0]);
                    if (list != null && list.Count > 0)
                    {
                        CurPh = list[0];
                        LoadLookupNames();
                    }
                }

                string sqlDt = "SELECT * FROM v_DMVPNVdt WHERE so_ct = '" + safeSo + "'";
                DataSet dsDt = await myDb.LoadDataset(sqlDt);
                if (dsDt != null && dsDt.Tables[0].Rows.Count > 0)
                {
                    LsCt = myDb.ConvertDataTable<DmvpnvDtRow>(dsDt.Tables[0]);
                    int stt = 1;
                    foreach (var ct in LsCt)
                        ct.Stt = stt++;
                }

                if (IsView)
                {
                    AllowEditMa  = false;
                    AllowEditMode = false;
                }
                else if (IsEdit)
                {
                    AllowEditMode = true;
                    AllowEditMa  = false;
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao(Lap["Error loading data"] + ": " + ex.Message);
                StateHasChanged();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (firstRender)
                {
                    // Load language t? localization file
                    UserLang = await myLcStore.GetItemAsync<string>("userlang");
                    if (!string.IsNullOrEmpty(UserLang))
                    {
                        IsLangEn = !UserLang.Equals("Vi");
                    }
                    
                    // Load Lap labels t? hr-vi.xml ho?c hr-en.xml
                    string langCode = IsLangEn ? "en" : "vi";
                    await Lap.LoadLocalizationAsync($"hr-{langCode}");
                    
                    string action = curActionTask.ToString();
                    if (action == "View" || action == "Edit")
                    {
                        if (!string.IsNullOrEmpty(Ma_ma))
                            await LoadData(Ma_ma);
                    }
                    else if (action == "New")
                    {
                        CreateNewRecord();
                    }

                    await JavacriptS.InvokeVoidAsync("eval", "document.getElementById('so_ct_input')?.focus()");

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
                CurPh.Ten_ns = myDb.GetValue("SELECT ten_ns FROM dmns WHERE ma_ns = '" + safe + "'")?.ToString() ?? "";
            }
        }

        void Ma_ns_Changed(ExpandoObject value)
        {
            CurPh.Ten_ns = "";
            if (!string.IsNullOrWhiteSpace(CurPh.Ma_ns))
            {
                string safe = CurPh.Ma_ns.Trim().Replace("'", "''");
                CurPh.Ten_ns = myDb.GetValue("SELECT ten_ns FROM dmns WHERE ma_ns = '" + safe + "'")?.ToString() ?? "";
            }
            StateHasChanged();
        }

        private void CopyDmvpnvDt(DmvpnvDtRow src, DmvpnvDtRow dst)
        {
            dst.Noi_dung_ky_luat  = src.Noi_dung_ky_luat;
            dst.Hinh_thuc_ky_luat = src.Hinh_thuc_ky_luat;
            dst.Muc_do_vi_pham    = src.Muc_do_vi_pham;
            dst.Trang_thai_xu_ly  = src.Trang_thai_xu_ly;
            dst.Ghi_chu           = src.Ghi_chu;
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
                var m = (DmvpnvDtRow)e.EditModel;
                m.Stt = LsCt.Count > 0 ? LsCt.Max(a => a.Stt) + 1 : 1;
            }
        }

        async Task Grid_EditModelSaving(GridEditModelSavingEventArgs e)
        {
            var edit = (DmvpnvDtRow)e.EditModel;

            if (e.IsNew && string.IsNullOrWhiteSpace(edit.Noi_dung_ky_luat)) return;

            if (e.IsNew)
            {
                var newItem = new DmvpnvDtRow();
                CopyDmvpnvDt(edit, newItem);
                newItem.Stt = LsCt.Count + 1;
                LsCt.Add(newItem);

                if (LsCt.Any(x => !string.IsNullOrWhiteSpace(x.Noi_dung_ky_luat) && x != newItem))
                    LsCt.RemoveAll(x => string.IsNullOrWhiteSpace(x.Noi_dung_ky_luat) && x != newItem);

                SelectedCt = newItem;
            }
            else
            {
                var dataItem = (DmvpnvDtRow)e.DataItem;
                CopyDmvpnvDt(edit, dataItem);
                SelectedCt = dataItem;
            }

            int stt = 1;
            LsCt.ForEach(x => x.Stt = stt++);

            await InvokeAsync(StateHasChanged);
        }

        async Task HandleSaveAndMoveNext(DxGrid grid, List<DmvpnvDtRow> list, object selectedItem, Action onAddAction)
        {
            if (grid != null && grid.IsEditing())
                await grid.SaveChangesAsync();

            await Task.Delay(100);

            if (selectedItem != null && selectedItem is DmvpnvDtRow currentItem)
            {
                var index = list.IndexOf(currentItem);
                if (index >= 0 && index < list.Count - 1)
                    await grid.StartEditDataItemAsync(list[index + 1]);
                else
                    onAddAction();
            }
            else
            {
                onAddAction();
            }
        }

        [JSInvokable]
        public async Task SaveAndMoveNextRow()
        {
            await HandleSaveAndMoveNext(GridCT, LsCt, SelectedCt, AddNewRow);
        }

        async void AddNewRow()
        {
            if (AllowEditMode)
            {
                if (GridCT != null && GridCT.IsEditing()) await GridCT.SaveChangesAsync();
                await GridCT.StartEditNewRowAsync();
            }
        }

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
            var item = (DmvpnvDtRow)e.DataItem;
            LsCt.Remove(item);
            int stt = 1;
            foreach (var ct in LsCt)
                ct.Stt = stt++;
        }

        async void OnGridCtLostFocus(FocusEventArgs e)
        {
            try
            {
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
                await Task.Delay(200);

                if (GridCT != null && GridCT.IsEditing())
                    await GridCT.SaveChangesAsync();

                await Task.Delay(100);

                if (!CheckValid())
                    return;

                // Remove rows without noi_dung_ky_luat (validate like WPF)
                LsCt.RemoveAll(x => string.IsNullOrWhiteSpace(x.Noi_dung_ky_luat));

                int userId   = curUser != null ? curUser.User_id : 0;
                string userName = curUser != null ? curUser.User_name : "";
                DateTime now = DateTime.Now;

                string safeSo = CurPh.So_ct.Trim().Replace("'", "''");

                if (IsNew)
                {
                    // Check duplicate so_ct
                    string sqlCheck = "SELECT COUNT(1) FROM DMVPNV WHERE so_ct='" + safeSo + "'";
                    DataTable dtChk = myDb.GetTable(sqlCheck);
                    if (dtChk != null && dtChk.Rows.Count > 0 && Convert.ToInt32(dtChk.Rows[0][0]) > 0)
                    {
                        myStateMN.SetNewThongbao(Lap["Duplicate voucher number"] + "!");
                        await JavacriptS.InvokeVoidAsync("eval", "document.getElementById('so_ct_input')?.focus()");
                        return;
                    }

                    CurPh.Date0      = now.Date;
                    CurPh.Time0      = now.ToString("HH:mm:ss");
                    CurPh.Date       = now.Date;
                    CurPh.Time       = now.ToString("HH:mm:ss");
                    CurPh.User_id0   = userId;
                    CurPh.User_name0 = userName;
                    CurPh.User_id    = userId;
                    CurPh.User_name  = userName;

                    await myDb.InsertOneRow<DmvpnvRow>(CurPh, "DMVPNV");

                    foreach (var ct in LsCt)
                    {
                        ct.So_ct      = CurPh.So_ct;
                        ct.Date0      = now.Date;
                        ct.Time0      = now.ToString("HH:mm:ss");
                        ct.Date       = now.Date;
                        ct.Time       = now.ToString("HH:mm:ss");
                        ct.User_id0   = userId;
                        ct.User_name0 = userName;
                        ct.User_id    = userId;
                        ct.User_name  = userName;
                        await myDb.InsertOneRow<DmvpnvDtRow>(ct, "DMVPNVdt");
                    }

                    myStateMN.SetNewThongbao(Lap["Successfully added new"] + ": " + CurPh.So_ct);
                }
                else if (IsEdit)
                {
                    CurPh.Date      = now.Date;
                    CurPh.Time      = now.ToString("HH:mm:ss");
                    CurPh.User_id   = userId;
                    CurPh.User_name = userName;

                    string whereClause = "so_ct = '" + safeSo + "'";
                    await myDb.UpdateOneRow1<DmvpnvRow>(CurPh, "DMVPNV", "", whereClause);

                    // Delete old details and re-insert (same logic as WPF)
                    await myDb.ExecuteSqlQueryAsync("DELETE FROM DMVPNVdt WHERE so_ct='" + safeSo + "'");

                    foreach (var ct in LsCt)
                    {
                        ct.So_ct      = CurPh.So_ct;
                        ct.Date0      = now.Date;
                        ct.Time0      = now.ToString("HH:mm:ss");
                        ct.Date       = now.Date;
                        ct.Time       = now.ToString("HH:mm:ss");
                        ct.User_id0   = userId;
                        ct.User_name0 = userName;
                        ct.User_id    = userId;
                        ct.User_name  = userName;
                        await myDb.InsertOneRow<DmvpnvDtRow>(ct, "DMVPNVdt");
                    }

                    myStateMN.SetNewThongbao(Lap["Edit successfully"] + ": " + CurPh.So_ct);
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
            if (string.IsNullOrWhiteSpace(CurPh.So_ct))
            {
                myStateMN.SetNewThongbao(Lap["Voucher number is required"] + "!");
                return false;
            }

            if (CurPh.Ngay_ct == null)
            {
                myStateMN.SetNewThongbao(Lap["Voucher date is required"] + "!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(CurPh.Ma_ns))
            {
                myStateMN.SetNewThongbao(Lap["Employee code is required"] + "!");
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
