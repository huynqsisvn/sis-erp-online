using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SisCom;
using SisCom.Model;
using SisCom.Shared;
using SisHr.Pages.Input.HRDCCCDC;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace SisHr.Pages.Input.HRDCCCDC
{
    public partial class FrmHrdcccdc
    {
        [Inject] public ILocalStorageService myLcStore { get; set; }

        private DxGrid GridCT;
        private object SelectedCt;
        public ElementReference GridWrapperCt;

        private DotNetObjectReference<FrmHrdcccdc> Pageobj;
        private string LastFocusedGrid = "CT";

        public string UserLang = "";
        public bool IsLangEn = false;

        public HrdccccdcRow CurPh { get; set; } = new HrdccccdcRow();
        public List<HrdccccdcDtRow> LsCt { get; set; } = new List<HrdccccdcDtRow>();

        // "0" = Thu hồi, "1" = Điều chuyển
        public bool IsDieuChuyen => CurPh.Loai == "1";

        private bool IsNew  => curActionTask.ToString() == "New";
        private bool IsEdit => curActionTask.ToString() == "Edit";
        private bool IsView => curActionTask.ToString() == "View";

        protected override void OnInitialized()
        {
            sqlTableName     = "DMDCCCDC";
            sqlTableView     = "v_DMDCCCDCDTDetails";
            SqlTableKey      = "so_ct";
            CheckSqlTableKey = "so_ct";
            ParentPage       = string.IsNullOrEmpty(ParentPage) ? "hrdcccdc" : ParentPage;
            CurPh            = new HrdccccdcRow();
            LsCt             = new List<HrdccccdcDtRow>();

            if (string.IsNullOrEmpty(Menu_id))
                Menu_id = "wr.ct.32";

            base.OnInitialized();
        }

        private async Task CreateNewRecord()
        {
            CurPh         = new HrdccccdcRow();
            CurPh.Ngay_ct = DateTime.Now.Date;
            CurPh.Loai    = "0";
            CurPh.Ten_loai = "Thu hồi";
            CurPh.Stt_rec = 0;
            LsCt          = new List<HrdccccdcDtRow>();

            var newRow = new HrdccccdcDtRow
            {
                Stt      = 1,
                Ngay_cap = DateTime.Now.Date,
                Ngay_dc  = DateTime.Now.Date,
                Stt_rec  = 0
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

                string sqlPh = "SELECT * FROM v_DMDCCCDC WHERE so_ct = '" + safe + "'";
                DataSet dsPh = await myDb.LoadDataset(sqlPh);
                if (dsPh != null && dsPh.Tables[0].Rows.Count > 0)
                {
                    List<HrdccccdcRow> list = myDb.ConvertDataTable<HrdccccdcRow>(dsPh.Tables[0]);
                    if (list != null && list.Count > 0)
                        CurPh = list[0];
                }

                string sqlDt = "SELECT * FROM v_DMDCCCDCDT WHERE so_ct = '" + safe + "'";
                DataSet dsDt = await myDb.LoadDataset(sqlDt);
                if (dsDt != null && dsDt.Tables[0].Rows.Count > 0)
                {
                    LsCt = myDb.ConvertDataTable<HrdccccdcDtRow>(dsDt.Tables[0]);
                    int stt = 1;
                    foreach (var ct in LsCt)
                    {
                        ct.Stt = stt++;

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

        void Loai_Changed(string value)
        {
            CurPh.Loai = value;
            if (value == "0")
            {
                CurPh.Ten_loai = "Thu hồi";
                // Clear sang employee when switching to Thu hồi (same WPF logic)
                foreach (var ct in LsCt)
                    ct.Ma_ns_sang = "";
            }
            else
            {
                CurPh.Ten_loai = "Điều chuyển";
            }
            InvokeAsync(StateHasChanged);
        }

        void Ma_ns_tu_Changed(ExpandoObject value, HrdccccdcDtRow item)
        {
            if (item == null) return;

            item.Ten_ns_tu = "";

            if (!string.IsNullOrWhiteSpace(item.Ma_ns_tu))
            {
                string safe = item.Ma_ns_tu.Trim().Replace("'", "''");
                DataTable dt = myDb.GetTable(
                    "SELECT ten_ns, ma_bpns FROM dmns WHERE ma_ns = '" + safe + "'");
                if (dt != null && dt.Rows.Count > 0)
                {
                    item.Ten_ns_tu = dt.Rows[0]["ten_ns"]?.ToString() ?? "";
                }
            }

            InvokeAsync(StateHasChanged);
        }

        void Ma_ns_sang_Changed(ExpandoObject value, HrdccccdcDtRow item)
        {
            if (item == null) return;

            item.Ten_ns_sang = "";

            if (!string.IsNullOrWhiteSpace(item.Ma_ns_sang))
            {
                string safe = item.Ma_ns_sang.Trim().Replace("'", "''");
                DataTable dt = myDb.GetTable(
                    "SELECT ten_ns FROM dmns WHERE ma_ns = '" + safe + "'");
                if (dt != null && dt.Rows.Count > 0)
                    item.Ten_ns_sang = dt.Rows[0]["ten_ns"]?.ToString() ?? "";
            }

            InvokeAsync(StateHasChanged);
        }

        void Ma_cc_Changed(ExpandoObject value, HrdccccdcDtRow item)
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

        private void CopyRow(HrdccccdcDtRow src, HrdccccdcDtRow dst)
        {
            dst.Ma_ns_tu    = src.Ma_ns_tu;
            dst.Ten_ns_tu   = src.Ten_ns_tu;
            dst.Ma_ns_sang  = src.Ma_ns_sang;
            dst.Ten_ns_sang = src.Ten_ns_sang;
            dst.Ma_cc_me    = src.Ma_cc_me;
            dst.Ten_cc      = src.Ten_cc;
            dst.So_luong    = src.So_luong;
            dst.Ngay_cap    = src.Ngay_cap;
            dst.Ly_do_cap   = src.Ly_do_cap;
            dst.Ngay_dc     = src.Ngay_dc;
            dst.Ly_do_dc    = src.Ly_do_dc;
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
                var m      = (HrdccccdcDtRow)e.EditModel;
                m.Stt      = LsCt.Count > 0 ? LsCt.Max(a => a.Stt) + 1 : 1;
                m.Ngay_cap = DateTime.Now.Date;
                m.Ngay_dc  = DateTime.Now.Date;
                m.Stt_rec  = 0;
            }
        }

        async Task Grid_EditModelSaving(GridEditModelSavingEventArgs e)
        {
            var edit = (HrdccccdcDtRow)e.EditModel;

            if (e.IsNew && string.IsNullOrWhiteSpace(edit.Ma_ns_tu) && string.IsNullOrWhiteSpace(edit.Ma_cc_me))
                return;

            // Populate display values from database (same WPF logic)
            if (!string.IsNullOrWhiteSpace(edit.Ma_ns_tu))
            {
                string safe = edit.Ma_ns_tu.Trim().Replace("'", "''");
                DataTable dt = myDb.GetTable("SELECT ten_ns FROM dmns WHERE ma_ns = '" + safe + "'");
                if (dt != null && dt.Rows.Count > 0)
                    edit.Ten_ns_tu = dt.Rows[0]["ten_ns"]?.ToString() ?? "";
            }

            if (!string.IsNullOrWhiteSpace(edit.Ma_ns_sang))
            {
                string safe = edit.Ma_ns_sang.Trim().Replace("'", "''");
                DataTable dt = myDb.GetTable("SELECT ten_ns FROM dmns WHERE ma_ns = '" + safe + "'");
                if (dt != null && dt.Rows.Count > 0)
                    edit.Ten_ns_sang = dt.Rows[0]["ten_ns"]?.ToString() ?? "";
            }

            if (!string.IsNullOrWhiteSpace(edit.Ma_cc_me))
            {
                string safe = edit.Ma_cc_me.Trim().Replace("'", "''");
                DataTable dt = myDb.GetTable("SELECT * FROM dmcc WHERE ma_cc = '" + safe + "'");
                if (dt != null && dt.Rows.Count > 0)
                {
                    string[] possibleColumns = { "ten_cc", "descript", "description", "ten_cc_me", "name" };
                    foreach (string colName in possibleColumns)
                    {
                        if (dt.Columns.Contains(colName) && dt.Rows[0][colName] != DBNull.Value)
                        {
                            edit.Ten_cc = dt.Rows[0][colName].ToString();
                            break;
                        }
                    }
                }
            }

            if (e.IsNew)
            {
                var newItem = new HrdccccdcDtRow();
                CopyRow(edit, newItem);
                newItem.Stt = LsCt.Count + 1;
                LsCt.Add(newItem);

                if (LsCt.Any(x => !string.IsNullOrWhiteSpace(x.Ma_ns_tu) && x != newItem))
                    LsCt.RemoveAll(x => string.IsNullOrWhiteSpace(x.Ma_ns_tu) && string.IsNullOrWhiteSpace(x.Ma_cc_me) && x != newItem);

                SelectedCt = newItem;
            }
            else
            {
                var dataItem = (HrdccccdcDtRow)e.DataItem;
                CopyRow(edit, dataItem);
                SelectedCt = dataItem;
            }

            int stt = 1;
            LsCt.ForEach(x => x.Stt = stt++);

            await InvokeAsync(StateHasChanged);
        }

        async Task HandleSaveAndMoveNext(DxGrid grid, List<HrdccccdcDtRow> list, object selectedItem, Action onAddAction)
        {
            if (grid != null && grid.IsEditing())
                await grid.SaveChangesAsync();

            await Task.Delay(100);

            if (selectedItem != null && selectedItem is HrdccccdcDtRow currentItem)
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
            var item = (HrdccccdcDtRow)e.DataItem;
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
                bool inside = await JavacriptS.InvokeAsync<bool>("focusInside", "grid-wrapper-dcccdc-ct");
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

                // Same WPF logic: remove rows missing required fields (ma_ns_tu, ma_cc_me, so_luong)
                List<HrdccccdcDtRow> rowsToRemove = new List<HrdccccdcDtRow>();
                foreach (var ct in LsCt)
                {
                    if (ct.Ma_ns_tu == null || string.IsNullOrWhiteSpace(ct.Ma_ns_tu) ||
                        ct.Ma_cc_me == null || string.IsNullOrWhiteSpace(ct.Ma_cc_me) ||
                        ct.So_luong == 0)
                    {
                        rowsToRemove.Add(ct);
                    }
                }
                foreach (var row in rowsToRemove)
                    LsCt.Remove(row);

                if (LsCt.Count == 0)
                {
                    myStateMN.SetNewThongbao(Lap["Tool transfer detail is required"] + "!");
                    return;
                }

                // Same WPF validate: Ma_ns_tu != Ma_ns_sang (only for Điều chuyển)
                foreach (var ct in LsCt)
                {
                    if (!string.IsNullOrWhiteSpace(ct.Ma_ns_tu) && !string.IsNullOrWhiteSpace(ct.Ma_ns_sang))
                    {
                        if (ct.Ma_ns_tu.Trim() == ct.Ma_ns_sang.Trim())
                        {
                            myStateMN.SetNewThongbao(Lap["From employee and To employee must be different"] + "!");
                            return;
                        }
                    }
                }

                int userId      = curUser != null ? curUser.User_id : 0;
                string userName = curUser != null ? curUser.User_name : "";
                DateTime now    = DateTime.Now;

                string safeSoCt = CurPh.So_ct.Trim().Replace("'", "''");

                if (IsNew)
                {
                    string sqlCheck = "SELECT COUNT(1) FROM DMDCCCDC WHERE so_ct='" + safeSoCt + "'";
                    DataTable dtChk = myDb.GetTable(sqlCheck);
                    if (dtChk != null && dtChk.Rows.Count > 0 && Convert.ToInt32(dtChk.Rows[0][0]) > 0)
                    {
                        myStateMN.SetNewThongbao(Lap["Duplicate voucher number"] + "!");
                        await JavacriptS.InvokeVoidAsync("eval", "document.getElementById('so_ct_input')?.focus()");
                        return;
                    }

                    CurPh.Stt_rec    = 0;
                    CurPh.Date0      = now.Date;
                    CurPh.Time0      = now.ToString("HH:mm:ss");
                    CurPh.Date       = now.Date;
                    CurPh.Time       = now.ToString("HH:mm:ss");
                    CurPh.User_id0   = userId;
                    CurPh.User_name0 = userName;
                    CurPh.User_id    = userId;
                    CurPh.User_name  = userName;

                    await myDb.InsertOneRow<HrdccccdcRow>(CurPh, "DMDCCCDC");

                    if (!string.IsNullOrWhiteSpace(CurPh.Ma_qs))
                    {
                        string safeQs = CurPh.Ma_qs.Trim().Replace("'", "''");
                        await myDb.ExecuteSqlQueryAsync(
                            "UPDATE dmqs SET so_ct = so_ct + 1 WHERE ma_qs LIKE '" + safeQs + "%'");
                    }

                    foreach (var ct in LsCt)
                    {
                        ct.So_ct      = CurPh.So_ct;
                        ct.Stt_rec    = 0;
                        ct.Date0      = now.Date;
                        ct.Time0      = now.ToString("HH:mm:ss");
                        ct.Date       = now.Date;
                        ct.Time       = now.ToString("HH:mm:ss");
                        ct.User_id0   = userId;
                        ct.User_name0 = userName;
                        ct.User_id    = userId;
                        ct.User_name  = userName;
                        await myDb.InsertOneRow<HrdccccdcDtRow>(ct, "DMDCCCDCDT");
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
                    await myDb.UpdateOneRow1<HrdccccdcRow>(CurPh, "DMDCCCDC", "", whereClause);

                    // Delete old details then re-insert (same WPF logic)
                    await myDb.ExecuteSqlQueryAsync("DELETE FROM DMDCCCDCDT WHERE so_ct='" + safeSoCt + "'");

                    foreach (var ct in LsCt)
                    {
                        ct.So_ct      = CurPh.So_ct;
                        ct.Stt_rec    = 0;
                        ct.Date0      = now.Date;
                        ct.Time0      = now.ToString("HH:mm:ss");
                        ct.Date       = now.Date;
                        ct.Time       = now.ToString("HH:mm:ss");
                        ct.User_id0   = userId;
                        ct.User_name0 = userName;
                        ct.User_id    = userId;
                        ct.User_name  = userName;
                        await myDb.InsertOneRow<HrdccccdcDtRow>(ct, "DMDCCCDCDT");
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
