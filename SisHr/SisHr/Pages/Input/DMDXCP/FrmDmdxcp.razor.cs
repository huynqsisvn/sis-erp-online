using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SisCom;
using SisCom.Model;
using SisCom.Shared;
using SisHr.Pages.Input.DMDXCP;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace SisHr.Pages.Input.DMDXCP
{
    public partial class FrmDmdxcp
    {
        // ---- Inject LocalStorage ----
        [Inject] public ILocalStorageService myLcStore { get; set; }

        // ---- Grid references ----
        private DxGrid GridCT;
        private object SelectedCt;
        public ElementReference GridWrapperCt;

        private DotNetObjectReference<FrmDmdxcp> Pageobj;
        private string LastFocusedGrid = "CT";

        // ---- Language ----
        public string UserLang = "";
        public bool IsLangEn = false;

        // ---- Data ----
        public DmdxcpRow CurPh { get; set; } = new DmdxcpRow();
        public List<DmdxcpDtRow> LsCt { get; set; } = new List<DmdxcpDtRow>();

        private bool IsNew  => curActionTask.ToString() == "New";
        private bool IsEdit => curActionTask.ToString() == "Edit";
        private bool IsView => curActionTask.ToString() == "View";

        protected override void OnInitialized()
        {
            sqlTableName     = "DMDXCP";
            sqlTableView     = "v_DMDXCPchitiet";
            SqlTableKey      = "so_ct";
            CheckSqlTableKey = "so_ct";
            ParentPage       = string.IsNullOrEmpty(ParentPage) ? "dmdxcp" : ParentPage;
            CurPh            = new DmdxcpRow();
            LsCt             = new List<DmdxcpDtRow>();

            if (string.IsNullOrEmpty(Menu_id))
                Menu_id = "wr.ct.23";

            base.OnInitialized();
        }

        private async Task CreateNewRecord()
        {
            CurPh         = new DmdxcpRow();
            CurPh.Ngay_ct = DateTime.Now.Date;
            CurPh.So_ct   = await GenerateNewSoCt();
            LsCt          = new List<DmdxcpDtRow>();

            var newRow = new DmdxcpDtRow { Stt = 1 };
            LsCt.Add(newRow);

            AllowEditMode = true;
            AllowEditMa   = true;
        }

        private async Task<string> GenerateNewSoCt()
        {
            try
            {
                // Get the auto-generation setting
                string autoGen = myOption.GetOptionsValue("M_AUTO_LIST_NUM");
                if (autoGen == "1")
                {
                    // Query dmdm table to check if DMDXCP has auto-increment enabled (increase_type = 2)
                    string sqlCheck = "SELECT increase_type FROM dmdm WHERE ma_dm = 'DMDXCP'";
                    DataTable dtCheck = myDb.GetTable(sqlCheck);
                    
                    if (dtCheck != null && dtCheck.Rows.Count > 0)
                    {
                        int increaseType = Convert.ToInt32(dtCheck.Rows[0]["increase_type"]);
                        
                        // If increase_type = 2, get the last number and increment
                        if (increaseType == 2)
                        {
                            // Get the last so_ct from database and increment it
                            string sqlLast = "SELECT TOP 1 so_ct FROM DMDXCP ORDER BY so_ct DESC";
                            DataTable dtLast = myDb.GetTable(sqlLast);
                            
                            if (dtLast != null && dtLast.Rows.Count > 0)
                            {
                                string lastSoCt = dtLast.Rows[0]["so_ct"].ToString().Trim();
                                return IncrementCode(lastSoCt);
                            }
                        }
                    }
                }
                
                // Default: use date-based format if no auto-increment or M_AUTO_LIST_NUM = 0
                DateTime now = DateTime.Now;
                string datePrefix = now.Year.ToString()
                                  + now.Month.ToString().PadLeft(2, '0')
                                  + now.Day.ToString().PadLeft(2, '0');
                
                // Get the last so_ct with today's date prefix
                string sqlToday = "SELECT TOP 1 so_ct FROM DMDXCP WHERE so_ct LIKE '" + datePrefix + "%' ORDER BY so_ct DESC";
                DataTable dtToday = myDb.GetTable(sqlToday);
                
                if (dtToday != null && dtToday.Rows.Count > 0)
                {
                    string lastSoCt = dtToday.Rows[0]["so_ct"].ToString().Trim();
                    return IncrementCode(lastSoCt);
                }
                
                // If no records for today, start with 01
                return datePrefix + "01";
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao("Error generating voucher number: " + ex.Message);
                // Fallback: return date-based code
                DateTime now = DateTime.Now;
                return now.Year.ToString()
                     + now.Month.ToString().PadLeft(2, '0')
                     + now.Day.ToString().PadLeft(2, '0')
                     + "01";
            }
        }

        /// <summary>
        /// Increment a code by 1 (same logic as WPF SysFunc.IncreaseCode)
        /// Assumes the last N digits are numeric and increments them
        /// </summary>
        private string IncrementCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return "01";

            // Find where the numeric part starts (from the end)
            int numLen = 0;
            for (int i = code.Length - 1; i >= 0; i--)
            {
                if (char.IsDigit(code[i]))
                    numLen++;
                else
                    break;
            }

            if (numLen == 0)
                return code + "01"; // No numeric part, append 01

            // Extract prefix and numeric parts
            string prefix = code.Substring(0, code.Length - numLen);
            string numPart = code.Substring(code.Length - numLen);

            // Increment the numeric part
            if (long.TryParse(numPart, out long num))
            {
                num++;
                return prefix + num.ToString().PadLeft(numLen, '0');
            }

            return code + "01";
        }

        private async Task LoadData(string key)
        {
            try
            {
                string safe = key.Trim().Replace("'", "''");

                string sqlPh = "SELECT * FROM v_DMDXCP WHERE so_ct = '" + safe + "'";
                DataSet dsPh = await myDb.LoadDataset(sqlPh);
                if (dsPh != null && dsPh.Tables[0].Rows.Count > 0)
                {
                    List<DmdxcpRow> list = myDb.ConvertDataTable<DmdxcpRow>(dsPh.Tables[0]);
                    if (list != null && list.Count > 0)
                    {
                        CurPh = list[0];
                        LoadLookupNames();
                    }
                }

                string sqlDt = "SELECT * FROM v_DMDXCPdt WHERE so_ct = '" + safe + "'";
                DataSet dsDt = await myDb.LoadDataset(sqlDt);
                if (dsDt != null && dsDt.Tables[0].Rows.Count > 0)
                {
                    LsCt = myDb.ConvertDataTable<DmdxcpDtRow>(dsDt.Tables[0]);
                    int stt = 1;
                    foreach (var ct in LsCt)
                        ct.Stt = stt++;
                }

                if (IsView)
                {
                    AllowEditMa   = false;
                    AllowEditMode = false;
                }
                else if (IsEdit)
                {
                    AllowEditMode = true;
                    AllowEditMa   = false;
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
                    UserLang = await myLcStore.GetItemAsync<string>("userlang");
                    if (!string.IsNullOrEmpty(UserLang))
                        IsLangEn = !UserLang.Equals("Vi");

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
                        await CreateNewRecord();
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
                CurPh.Ten_ns   = myDb.GetValue("SELECT ten_ns FROM dmns WHERE ma_ns = '" + safe + "'")?.ToString() ?? "";
                CurPh.Ten_bpns = myDb.GetValue("SELECT b.ten_bpns FROM dmns n LEFT JOIN dmbpns b ON n.ma_bpns = b.ma_bpns WHERE n.ma_ns = '" + safe + "'")?.ToString() ?? "";
            }
        }

        void Ma_ns_Changed(ExpandoObject value)
        {
            CurPh.Ten_ns   = "";
            CurPh.Ten_bpns = "";
            if (!string.IsNullOrWhiteSpace(CurPh.Ma_ns))
            {
                string safe = CurPh.Ma_ns.Trim().Replace("'", "''");
                CurPh.Ten_ns   = myDb.GetValue("SELECT ten_ns FROM dmns WHERE ma_ns = '" + safe + "'")?.ToString() ?? "";
                CurPh.Ten_bpns = myDb.GetValue("SELECT b.ten_bpns FROM dmns n LEFT JOIN dmbpns b ON n.ma_bpns = b.ma_bpns WHERE n.ma_ns = '" + safe + "'")?.ToString() ?? "";
            }
            StateHasChanged();
        }

        private void CopyDmdxcpDt(DmdxcpDtRow src, DmdxcpDtRow dst)
        {
            dst.Noi_dung   = src.Noi_dung;
            dst.Dvt        = src.Dvt;
            dst.So_luong   = src.So_luong;
            dst.Don_gia    = src.Don_gia;
            dst.Thanh_tien = src.Thanh_tien;
            dst.Ghi_chu    = src.Ghi_chu;
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
                var m = (DmdxcpDtRow)e.EditModel;
                m.Stt = LsCt.Count > 0 ? LsCt.Max(a => a.Stt) + 1 : 1;
            }
        }

        async Task Grid_EditModelSaving(GridEditModelSavingEventArgs e)
        {
            var edit = (DmdxcpDtRow)e.EditModel;

            // Do not save a new empty row (validate: noi_dung required, same WPF logic)
            if (e.IsNew && string.IsNullOrWhiteSpace(edit.Noi_dung)) return;

            // Auto-calculate thanh_tien
            edit.Thanh_tien = edit.So_luong * edit.Don_gia;

            if (e.IsNew)
            {
                var newItem = new DmdxcpDtRow();
                CopyDmdxcpDt(edit, newItem);
                newItem.Stt = LsCt.Count + 1;
                LsCt.Add(newItem);

                // Remove placeholder rows without noi_dung (same WPF logic)
                if (LsCt.Any(x => !string.IsNullOrWhiteSpace(x.Noi_dung) && x != newItem))
                    LsCt.RemoveAll(x => string.IsNullOrWhiteSpace(x.Noi_dung) && x != newItem);

                SelectedCt = newItem;
            }
            else
            {
                var dataItem = (DmdxcpDtRow)e.DataItem;
                CopyDmdxcpDt(edit, dataItem);
                SelectedCt = dataItem;
            }

            int stt = 1;
            LsCt.ForEach(x => x.Stt = stt++);

            await InvokeAsync(StateHasChanged);
        }

        async Task HandleSaveAndMoveNext(DxGrid grid, List<DmdxcpDtRow> list, object selectedItem, Action onAddAction)
        {
            if (grid != null && grid.IsEditing())
                await grid.SaveChangesAsync();

            await Task.Delay(100);

            if (selectedItem != null && selectedItem is DmdxcpDtRow currentItem)
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
            var item = (DmdxcpDtRow)e.DataItem;
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

                // Remove empty rows (same WPF logic: skip rows without noi_dung)
                LsCt.RemoveAll(x => string.IsNullOrWhiteSpace(x.Noi_dung));

                if (LsCt.Count == 0)
                {
                    myStateMN.SetNewThongbao(Lap["Expense content is required"] + "!");
                    return;
                }

                // Ensure thanh_tien is correct for each row
                foreach (var ct in LsCt)
                    ct.Thanh_tien = ct.So_luong * ct.Don_gia;

                int userId      = curUser != null ? curUser.User_id : 0;
                string userName = curUser != null ? curUser.User_name : "";
                DateTime now    = DateTime.Now;

                string safeSoCt = CurPh.So_ct.Trim().Replace("'", "''");

                if (IsNew)
                {
                    // Check duplicate so_ct (same WPF logic)
                    string sqlCheck = "SELECT COUNT(1) FROM DMDXCP WHERE so_ct='" + safeSoCt + "'";
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

                    await myDb.InsertOneRow<DmdxcpRow>(CurPh, "DMDXCP");

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
                        await myDb.InsertOneRow<DmdxcpDtRow>(ct, "DMDXCPdt");
                    }

                    myStateMN.SetNewThongbao(Lap["Successfully added new"] + ": " + CurPh.So_ct);
                }
                else if (IsEdit)
                {
                    CurPh.Date      = now.Date;
                    CurPh.Time      = now.ToString("HH:mm:ss");
                    CurPh.User_id   = userId;
                    CurPh.User_name = userName;

                    string whereClause = "so_ct = '" + safeSoCt + "'";
                    await myDb.UpdateOneRow1<DmdxcpRow>(CurPh, "DMDXCP", "", whereClause);

                    // Delete old details then re-insert (same WPF logic)
                    await myDb.ExecuteSqlQueryAsync("DELETE FROM DMDXCPdt WHERE so_ct='" + safeSoCt + "'");

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
                        await myDb.InsertOneRow<DmdxcpDtRow>(ct, "DMDXCPdt");
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
