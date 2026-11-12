using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SisCom;
using SisCom.Model;
using SisCom.Shared;
using SisHr.Pages.Input.HRCPCCDC;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace SisHr.Pages.Input.HRCPCCDC
{
    public partial class FrmHrcpccdc
    {
        [Inject] public ILocalStorageService myLcStore { get; set; }

        private DxGrid GridCT;
        private object SelectedCt;
        public ElementReference GridWrapperCt;

        private DotNetObjectReference<FrmHrcpccdc> Pageobj;
        private string LastFocusedGrid = "CT";

        public string UserLang = "";
        public bool IsLangEn = false;

        public HrcpccdcRow CurPh { get; set; } = new HrcpccdcRow();
        public List<HrcpccdcDtRow> LsCt { get; set; } = new List<HrcpccdcDtRow>();

        private bool IsNew  => curActionTask.ToString() == "New";
        private bool IsEdit => curActionTask.ToString() == "Edit";
        private bool IsView => curActionTask.ToString() == "View";

        protected override void OnInitialized()
        {
            sqlTableName     = "DMCPCCDC";
            sqlTableView     = "v_DMCPCCDCDTDetails";
            SqlTableKey      = "so_ct";
            CheckSqlTableKey = "so_ct";
            ParentPage       = string.IsNullOrEmpty(ParentPage) ? "hrcpccdc" : ParentPage;
            CurPh            = new HrcpccdcRow();
            LsCt             = new List<HrcpccdcDtRow>();

            if (string.IsNullOrEmpty(Menu_id))
                Menu_id = "wr.ct.31";

            base.OnInitialized();
        }

        private async Task CreateNewRecord()
        {
            CurPh           = new HrcpccdcRow();
            CurPh.Ngay_ct   = DateTime.Now.Date;
            CurPh.Stt_rec   = 0; // Initialize stt_rec
            LsCt            = new List<HrcpccdcDtRow>();

            var newRow = new HrcpccdcDtRow
            {
                Stt      = 1,
                Ngay_cap = DateTime.Now.Date,
                Ngay_ct  = DateTime.Now.Date,
                Stt_rec  = 0 // Initialize stt_rec
            };
            LsCt.Add(newRow);

            AllowEditMode = true;
            AllowEditMa   = true;
        }


        private async Task LoadData(string key)
        {
            try
            {
                string safe = key.Trim().Replace("'", "''");

                string sqlPh = "SELECT * FROM v_DMCPCCDC WHERE so_ct = '" + safe + "'";
                DataSet dsPh = await myDb.LoadDataset(sqlPh);
                if (dsPh != null && dsPh.Tables[0].Rows.Count > 0)
                {
                    List<HrcpccdcRow> list = myDb.ConvertDataTable<HrcpccdcRow>(dsPh.Tables[0]);
                    if (list != null && list.Count > 0)
                        CurPh = list[0];
                }

                string sqlDt = "SELECT * FROM v_DMCPCCDCDT WHERE so_ct = '" + safe + "'";
                DataSet dsDt = await myDb.LoadDataset(sqlDt);
                if (dsDt != null && dsDt.Tables[0].Rows.Count > 0)
                {
                    LsCt = myDb.ConvertDataTable<HrcpccdcDtRow>(dsDt.Tables[0]);
                    int stt = 1;
                    foreach (var ct in LsCt)
                    {
                        ct.Stt = stt++;
                        
                        // Load department name if ma_bpns exists but ten_bpns is empty
                        if (!string.IsNullOrWhiteSpace(ct.Ma_bpns) && string.IsNullOrWhiteSpace(ct.Ten_bpns))
                        {
                            string safeBp = ct.Ma_bpns.Trim().Replace("'", "''");
                            var resultBp = myDb.GetValue(
                                "SELECT ma_bpns FROM dmbpns WHERE ma_bpns = '" + safeBp + "'");
                            if (resultBp != null && resultBp != DBNull.Value)
                                ct.Ten_bpns = resultBp.ToString();
                        }
                        
                        // Load tool name if ma_cc_me exists but ten_cc is empty
                        if (!string.IsNullOrWhiteSpace(ct.Ma_cc_me) && string.IsNullOrWhiteSpace(ct.Ten_cc))
                        {
                            string safeCc = ct.Ma_cc_me.Trim().Replace("'", "''");
                            DataTable dtCc = myDb.GetTable("SELECT * FROM dmcc WHERE ma_cc = '" + safeCc + "'");
                            if (dtCc != null && dtCc.Rows.Count > 0)
                            {
                                string[] possibleColumns = { "ten_cc", "descript", "description", "ten_cc_me", "name" };
                                foreach (string colName in possibleColumns)
                                {
                                    if (dtCc.Columns.Contains(colName) && dtCc.Rows[0][colName] != DBNull.Value)
                                    {
                                        ct.Ten_cc = dtCc.Rows[0][colName].ToString();
                                        break;
                                    }
                                }
                            }
                        }
                    }
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

                    await JavacriptS.InvokeVoidAsync("eval", "document.getElementById('ngay_ct_input')?.focus()");

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

        void Ma_ns_Changed(ExpandoObject value, HrcpccdcDtRow item)
        {
            if (item == null) return;
            
            item.Ten_ns   = "";
            item.Ten_bpns = "";
            item.Ma_bpns  = "";
            item.Ma_chucvu = "";
            
            if (!string.IsNullOrWhiteSpace(item.Ma_ns))
            {
                string safe = item.Ma_ns.Trim().Replace("'", "''");
                DataTable dt = myDb.GetTable(
                    "SELECT ten_ns, ma_bpns, ma_chucvu FROM dmns WHERE ma_ns = '" + safe + "'");
                if (dt != null && dt.Rows.Count > 0)
                {
                    item.Ten_ns    = dt.Rows[0]["ten_ns"]?.ToString() ?? "";
                    item.Ma_bpns   = dt.Rows[0]["ma_bpns"]?.ToString() ?? "";
                    item.Ma_chucvu = dt.Rows[0]["ma_chucvu"]?.ToString() ?? "";

                    if (!string.IsNullOrWhiteSpace(item.Ma_bpns))
                    {
                        string safeBp = item.Ma_bpns.Trim().Replace("'", "''");
                        var resultBp = myDb.GetValue(
                            "SELECT ten_bpns FROM dmbpns WHERE ma_bpns = '" + safeBp + "'");
                        if (resultBp != null && resultBp != DBNull.Value)
                            item.Ten_bpns = resultBp.ToString();
                    }
                }
            }
            
            InvokeAsync(StateHasChanged);
        }

        void Ma_cc_Changed(ExpandoObject value, HrcpccdcDtRow item)
        {
            if (item == null) return;
            
            item.Ten_cc = "";
            if (!string.IsNullOrWhiteSpace(item.Ma_cc_me))
            {
                string safe = item.Ma_cc_me.Trim().Replace("'", "''");
                DataTable dt = myDb.GetTable("SELECT * FROM dmcc WHERE ma_cc = '" + safe + "'");
                
                if (dt != null && dt.Rows.Count > 0)
                {
                    string[] possibleColumns = { "ten_cc", "descript", "description", "ten_cc_me", "name" };
                    foreach (string colName in possibleColumns)
                    {
                        if (dt.Columns.Contains(colName) && dt.Rows[0][colName] != DBNull.Value)
                        {
                            item.Ten_cc = dt.Rows[0][colName].ToString();
                            break;
                        }
                    }
                }
            }
            
            InvokeAsync(StateHasChanged);
        }

        private void CopyRow(HrcpccdcDtRow src, HrcpccdcDtRow dst)
        {
            dst.Ma_ns     = src.Ma_ns;
            dst.Ten_ns    = src.Ten_ns;
            dst.Ma_bpns   = src.Ma_bpns;
            dst.Ten_bpns  = src.Ten_bpns;
            dst.Ma_chucvu = src.Ma_chucvu;
            dst.Ma_cc_me  = src.Ma_cc_me;
            dst.Ten_cc    = src.Ten_cc;
            dst.So_luong  = src.So_luong;
            dst.Ngay_cap  = src.Ngay_cap;
            dst.Ngay_ct   = src.Ngay_ct;
            dst.Ghi_chu   = src.Ghi_chu;
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
                var m        = (HrcpccdcDtRow)e.EditModel;
                m.Stt        = LsCt.Count > 0 ? LsCt.Max(a => a.Stt) + 1 : 1;
                m.Ngay_cap   = DateTime.Now.Date;
                m.Ngay_ct    = DateTime.Now.Date;
                m.Stt_rec    = 0; // Initialize stt_rec for new row
            }
        }

        async Task Grid_EditModelSaving(GridEditModelSavingEventArgs e)
        {
            var edit = (HrcpccdcDtRow)e.EditModel;

            // Skip new empty rows (no employee and no tool set)
            if (e.IsNew && string.IsNullOrWhiteSpace(edit.Ma_ns) && string.IsNullOrWhiteSpace(edit.Ma_cc_me))
                return;

            if (e.IsNew)
            {
                var newItem = new HrcpccdcDtRow();
                CopyRow(edit, newItem);
                newItem.Stt = LsCt.Count + 1;
                LsCt.Add(newItem);

                // Remove placeholder rows without ma_ns
                if (LsCt.Any(x => !string.IsNullOrWhiteSpace(x.Ma_ns) && x != newItem))
                    LsCt.RemoveAll(x => string.IsNullOrWhiteSpace(x.Ma_ns) && string.IsNullOrWhiteSpace(x.Ma_cc_me) && x != newItem);

                SelectedCt = newItem;
            }
            else
            {
                var dataItem = (HrcpccdcDtRow)e.DataItem;
                CopyRow(edit, dataItem);
                SelectedCt = dataItem;
            }

            int stt = 1;
            LsCt.ForEach(x => x.Stt = stt++);

            await InvokeAsync(StateHasChanged);
        }

        async Task HandleSaveAndMoveNext(DxGrid grid, List<HrcpccdcDtRow> list, object selectedItem, Action onAddAction)
        {
            if (grid != null && grid.IsEditing())
                await grid.SaveChangesAsync();

            await Task.Delay(100);

            if (selectedItem != null && selectedItem is HrcpccdcDtRow currentItem)
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
            var item = (HrcpccdcDtRow)e.DataItem;
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
                bool inside = await JavacriptS.InvokeAsync<bool>("focusInside", "grid-wrapper-cpccdc-ct");
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

                // Remove rows without both ma_ns and ma_cc_me (same WPF logic)
                LsCt.RemoveAll(x => (x.Ma_ns == null || x.Ma_ns == DBNull.Value.ToString())
                                 || (x.Ma_cc_me == null || x.Ma_cc_me == DBNull.Value.ToString())
                                 || (x.So_luong == 0 && string.IsNullOrWhiteSpace(x.Ma_ns)));

                if (LsCt.Count == 0)
                {
                    myStateMN.SetNewThongbao(Lap["Tool allocation detail is required"] + "!");
                    return;
                }

                int userId      = curUser != null ? curUser.User_id : 0;
                string userName = curUser != null ? curUser.User_name : "";
                DateTime now    = DateTime.Now;

                string safeSoCt = CurPh.So_ct.Trim().Replace("'", "''");

                if (IsNew)
                {
                    // Check duplicate so_ct
                    string sqlCheck = "SELECT COUNT(1) FROM DMCPCCDC WHERE so_ct='" + safeSoCt + "'";
                    DataTable dtChk = myDb.GetTable(sqlCheck);
                    if (dtChk != null && dtChk.Rows.Count > 0 && Convert.ToInt32(dtChk.Rows[0][0]) > 0)
                    {
                        myStateMN.SetNewThongbao(Lap["Duplicate voucher number"] + "!");
                        await JavacriptS.InvokeVoidAsync("eval", "document.getElementById('so_ct_input')?.focus()");
                        return;
                    }

                    CurPh.Stt_rec    = 0; // Set stt_rec for header
                    CurPh.Date0      = now.Date;
                    CurPh.Time0      = now.ToString("HH:mm:ss");
                    CurPh.Date       = now.Date;
                    CurPh.Time       = now.ToString("HH:mm:ss");
                    CurPh.User_id0   = userId;
                    CurPh.User_name0 = userName;
                    CurPh.User_id    = userId;
                    CurPh.User_name  = userName;

                    await myDb.InsertOneRow<HrcpccdcRow>(CurPh, "DMCPCCDC");

                    // Update dmqs so_ct counter (same WPF logic)
                    if (!string.IsNullOrWhiteSpace(CurPh.Ma_qs))
                    {
                        string safeQs = CurPh.Ma_qs.Trim().Replace("'", "''");
                        await myDb.ExecuteSqlQueryAsync(
                            "UPDATE dmqs SET so_ct = so_ct + 1 WHERE ma_qs LIKE '" + safeQs + "%'");
                    }

                    foreach (var ct in LsCt)
                    {
                        ct.So_ct      = CurPh.So_ct;
                        ct.Ngay_ct    = CurPh.Ngay_ct;
                        ct.Stt_rec    = 0; // Set stt_rec for detail
                        ct.Date0      = now.Date;
                        ct.Time0      = now.ToString("HH:mm:ss");
                        ct.Date       = now.Date;
                        ct.Time       = now.ToString("HH:mm:ss");
                        ct.User_id0   = userId;
                        ct.User_name0 = userName;
                        ct.User_id    = userId;
                        ct.User_name  = userName;
                        await myDb.InsertOneRow<HrcpccdcDtRow>(ct, "DMCPCCDCDT");
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
                    await myDb.UpdateOneRow1<HrcpccdcRow>(CurPh, "DMCPCCDC", "", whereClause);

                    // Delete old details then re-insert (same WPF logic)
                    await myDb.ExecuteSqlQueryAsync("DELETE FROM DMCPCCDCDT WHERE so_ct='" + safeSoCt + "'");

                    foreach (var ct in LsCt)
                    {
                        ct.So_ct      = CurPh.So_ct;
                        ct.Ngay_ct    = CurPh.Ngay_ct;
                        ct.Stt_rec    = 0; // Set stt_rec for detail
                        ct.Date0      = now.Date;
                        ct.Time0      = now.ToString("HH:mm:ss");
                        ct.Date       = now.Date;
                        ct.Time       = now.ToString("HH:mm:ss");
                        ct.User_id0   = userId;
                        ct.User_name0 = userName;
                        ct.User_id    = userId;
                        ct.User_name  = userName;
                        await myDb.InsertOneRow<HrcpccdcDtRow>(ct, "DMCPCCDCDT");
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

            return true;
        }

        public void Dispose()
        {
            Pageobj?.Dispose();
        }
    }
}
