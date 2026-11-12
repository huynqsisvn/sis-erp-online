using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SisCom;
using SisCom.Model;
using SisCom.Shared;
using SisHr.Pages.Input.DMNVGL;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace SisHr.Pages.Input.DMNVGL
{
    public partial class FrmDmnvgl
    {
        // ---- Inject LocalStorage ----
        [Inject] public ILocalStorageService myLcStore { get; set; }

        // ---- Grid references ----
        private DxGrid GridCT;
        private object SelectedCt;
        public ElementReference GridWrapperCt;

        private DotNetObjectReference<FrmDmnvgl> Pageobj;
        private string LastFocusedGrid = "CT";

        // ---- Language ----
        public string UserLang = "";
        public bool IsLangEn = false;

        // ---- Data ----
        // Ngay_ct is shared for all rows in a session; stored separately for the header field
        public DateTime? CurNgayCt { get; set; }
        public List<DmnvglRow> LsRows { get; set; } = new List<DmnvglRow>();

        // Key of the record being edited (ngay_ct value as string for edit/delete)
        private string _editNgayCt = "";

        private bool IsNew  => curActionTask.ToString() == "New";
        private bool IsEdit => curActionTask.ToString() == "Edit";
        private bool IsView => curActionTask.ToString() == "View";

        protected override void OnInitialized()
        {
            sqlTableName     = "DMNVGL";
            sqlTableView     = "v_DMNVGL";
            SqlTableKey      = "ma_ma";
            CheckSqlTableKey = "ma_ma";
            ParentPage       = string.IsNullOrEmpty(ParentPage) ? "dmnvgl" : ParentPage;
            LsRows           = new List<DmnvglRow>();

            if (string.IsNullOrEmpty(Menu_id))
                Menu_id = "wr.ct.22";

            base.OnInitialized();
        }

        private void CreateNewRecord()
        {
            CurNgayCt = DateTime.Now.Date;
            LsRows    = new List<DmnvglRow>();

            var newRow = new DmnvglRow { Stt = 1 };
            LsRows.Add(newRow);

            AllowEditMode = true;
            AllowEditMa   = true;
        }

        private async Task LoadData(string key)
        {
            try
            {
                string safe = key.Trim().Replace("'", "''");

                string sql = "SELECT * FROM DMNVGL WHERE ma_ma = '" + safe + "'";
                DataSet ds = await myDb.LoadDataset(sql);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    List<DmnvglRow> list = myDb.ConvertDataTable<DmnvglRow>(ds.Tables[0]);
                    if (list != null && list.Count > 0)
                    {
                        // All rows share the same ngay_ct — read from first row
                        CurNgayCt   = list[0].Ngay_ct;
                        _editNgayCt = CurNgayCt.HasValue
                                      ? CurNgayCt.Value.ToString("yyyy-MM-dd")
                                      : "";

                        LsRows = list;
                        int stt = 1;
                        foreach (var r in LsRows)
                        {
                            r.Stt = stt++;
                            // Resolve employee name
                            if (!string.IsNullOrWhiteSpace(r.Ma_ns))
                            {
                                string safeMa = r.Ma_ns.Trim().Replace("'", "''");
                                r.Ten_ns = myDb.GetValue("SELECT ten_ns FROM dmns WHERE ma_ns='" + safeMa + "'")?.ToString() ?? "";
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
                        CreateNewRecord();
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

        // Called when employee code is selected in the grid cell editor
        void Ma_ns_GridChanged(DmnvglRow item, ExpandoObject value)
        {
            item.Ten_ns = "";
            if (!string.IsNullOrWhiteSpace(item.Ma_ns))
            {
                string safe = item.Ma_ns.Trim().Replace("'", "''");
                item.Ten_ns = myDb.GetValue("SELECT ten_ns FROM dmns WHERE ma_ns='" + safe + "'")?.ToString() ?? "";
            }
        }

        private void CopyDmnvglRow(DmnvglRow src, DmnvglRow dst)
        {
            dst.Ma_ns     = src.Ma_ns;
            dst.Ten_ns    = src.Ten_ns;
            dst.Dien_giai = src.Dien_giai;
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
                var m = (DmnvglRow)e.EditModel;
                m.Stt = LsRows.Count > 0 ? LsRows.Max(a => a.Stt) + 1 : 1;
            }
        }

        async Task Grid_EditModelSaving(GridEditModelSavingEventArgs e)
        {
            var edit = (DmnvglRow)e.EditModel;

            // Do not save a new empty row (validate: ma_ns required)
            if (e.IsNew && string.IsNullOrWhiteSpace(edit.Ma_ns)) return;

            if (e.IsNew)
            {
                // Resolve ten_ns if not yet populated
                if (string.IsNullOrWhiteSpace(edit.Ten_ns) && !string.IsNullOrWhiteSpace(edit.Ma_ns))
                {
                    string safe = edit.Ma_ns.Trim().Replace("'", "''");
                    edit.Ten_ns = myDb.GetValue("SELECT ten_ns FROM dmns WHERE ma_ns='" + safe + "'")?.ToString() ?? "";
                }

                var newItem = new DmnvglRow();
                CopyDmnvglRow(edit, newItem);
                newItem.Stt = LsRows.Count + 1;
                LsRows.Add(newItem);

                // Remove placeholder rows without ma_ns
                if (LsRows.Any(x => !string.IsNullOrWhiteSpace(x.Ma_ns) && x != newItem))
                    LsRows.RemoveAll(x => string.IsNullOrWhiteSpace(x.Ma_ns) && x != newItem);

                SelectedCt = newItem;
            }
            else
            {
                var dataItem = (DmnvglRow)e.DataItem;

                // Resolve ten_ns on edit
                if (string.IsNullOrWhiteSpace(edit.Ten_ns) && !string.IsNullOrWhiteSpace(edit.Ma_ns))
                {
                    string safe = edit.Ma_ns.Trim().Replace("'", "''");
                    edit.Ten_ns = myDb.GetValue("SELECT ten_ns FROM dmns WHERE ma_ns='" + safe + "'")?.ToString() ?? "";
                }

                CopyDmnvglRow(edit, dataItem);
                SelectedCt = dataItem;
            }

            int stt = 1;
            LsRows.ForEach(x => x.Stt = stt++);

            await InvokeAsync(StateHasChanged);
        }

        async Task HandleSaveAndMoveNext(DxGrid grid, List<DmnvglRow> list, object selectedItem, Action onAddAction)
        {
            if (grid != null && grid.IsEditing())
                await grid.SaveChangesAsync();

            await Task.Delay(100);

            if (selectedItem != null && selectedItem is DmnvglRow currentItem)
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
            await HandleSaveAndMoveNext(GridCT, LsRows, SelectedCt, AddNewRow);
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
            var item = (DmnvglRow)e.DataItem;
            LsRows.Remove(item);
            int stt = 1;
            foreach (var r in LsRows)
                r.Stt = stt++;
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

                // Remove rows without ma_ns (same WPF logic: skip empty rows)
                LsRows.RemoveAll(x => string.IsNullOrWhiteSpace(x.Ma_ns));

                if (LsRows.Count == 0)
                {
                    myStateMN.SetNewThongbao(Lap["Employee code is required"] + "!");
                    return;
                }

                int userId      = curUser != null ? curUser.User_id : 0;
                string userName = curUser != null ? curUser.User_name : "";
                DateTime now    = DateTime.Now;
                DateTime ngayCt = CurNgayCt.HasValue ? CurNgayCt.Value.Date : now.Date;

                if (IsNew)
                {
                    // Insert each row as a new record with a new GUID (same as WPF logic)
                    foreach (var row in LsRows)
                    {
                        row.Ma_ma      = Guid.NewGuid().ToString();
                        row.Ngay_ct    = ngayCt;
                        row.Date       = now.Date;
                        row.Time       = now.ToString("HH:mm:ss");
                        row.User_id    = userId;
                        row.User_name  = userName;
                        row.Date0      = now.Date;
                        row.Time0      = now.ToString("HH:mm:ss");
                        row.User_id0   = userId;
                        row.User_name0 = userName;
                        await myDb.InsertOneRow<DmnvglRow>(row, "DMNVGL");
                    }

                    myStateMN.SetNewThongbao(Lap["Successfully added new"] + ": " + ngayCt.ToString("dd/MM/yyyy"));
                }
                else if (IsEdit)
                {
                    // WPF edit logic: delete rows by ngay_ct + ma_ns then re-insert
                    string safeNgay = ngayCt.ToString("yyyy-MM-dd");

                    foreach (var row in LsRows)
                    {
                        string safeMa = row.Ma_ns.Trim().Replace("'", "''");
                        await myDb.ExecuteSqlQueryAsync(
                            "DELETE FROM DMNVGL WHERE ngay_ct='" + safeNgay + "' AND ma_ns='" + safeMa + "'");
                    }

                    foreach (var row in LsRows)
                    {
                        row.Ma_ma      = Guid.NewGuid().ToString();
                        row.Ngay_ct    = ngayCt;
                        row.Date0      = now.Date;
                        row.Time0      = now.ToString("HH:mm:ss");
                        row.User_id0   = userId;
                        row.User_name0 = userName;
                        row.Date       = now.Date;
                        row.Time       = now.ToString("HH:mm:ss");
                        row.User_id    = userId;
                        row.User_name  = userName;
                        await myDb.InsertOneRow<DmnvglRow>(row, "DMNVGL");
                    }

                    myStateMN.SetNewThongbao(Lap["Edit successfully"] + ": " + ngayCt.ToString("dd/MM/yyyy"));
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
            if (CurNgayCt == null)
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
