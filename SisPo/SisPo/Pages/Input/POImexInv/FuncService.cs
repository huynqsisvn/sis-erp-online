using SisData.Data;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SisPo.Pages.Input.POImexInv
{
    /// <summary>
    /// Interface for Func Service - Blazor compliant
    /// </summary>
    public interface IFuncService
    {
        Task UploadInvAsync(string token, DataSet ds);
        Task CheckMstBanAsync(DataTable tbl);
        Task CheckDoanhNghiepRuiRoThueAsync(DataSet ds);
        Task CheckCtAsync(string idHd);
        Task CreateBkhddvtctAsync(string idHd);
    }

    /// <summary>
    /// Func Service - Refactored for Blazor with DI pattern
    /// Note: This is a partial refactoring. Full implementation requires refactoring
    /// of dependent classes like JsonData, SmImexInv, Web, etc.
    /// </summary>
    public class FuncService
    {
        private readonly IDbManager _dbManager;        

        public FuncService(IDbManager dbManager)
        {
            _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));            
        }

        public async Task UploadInvAsync(string token, DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
                return;

            // Implementation will need to be completed once dependent services are refactored
            await Task.CompletedTask;
            throw new NotImplementedException("This method requires additional refactoring of dependent services");
        }

        public async Task CheckMstBanAsync(DataTable tbl)
        {
            // Implementation placeholder
            await Task.CompletedTask;
            throw new NotImplementedException("This method requires ITaxInfoService and additional refactoring");
        }

        public async Task CheckDoanhNghiepRuiRoThueAsync(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
                return;

            try
            {
                const string errorText = "Doanh nghi?p r?i ro theo công v?n s? 1798/TCT-TTKT ngày 16/5/2023.";
                
                var dataTable = ds.Tables[0].Copy();
                var riskCompanies = _dbManager.GetTable(
                    "SELECT RTRIM(LTRIM(ma_so_thue)) as ma_so_thue FROM dmdnruiro WHERE status = '1'");

                if (riskCompanies.Rows.Count == 0)
                    return;

                var commands = new System.Text.StringBuilder();
                var checkedMst = new System.Collections.Generic.HashSet<string>();

                foreach (DataRow row in dataTable.Rows)
                {
                    var mstBan = row["mst_ban"]?.ToString()?.Trim();
                    var idHd = row["id_hd"]?.ToString()?.Trim();

                    if (string.IsNullOrEmpty(mstBan) || string.IsNullOrEmpty(idHd))
                        continue;

                    if (checkedMst.Contains(mstBan))
                    {
                        commands.AppendLine(BuildUpdateErrorQuery(errorText, idHd));
                        continue;
                    }

                    var isRisk = false;
                    foreach (DataRow riskRow in riskCompanies.Rows)
                    {
                        if (riskRow["ma_so_thue"]?.ToString() == mstBan)
                        {
                            isRisk = true;
                            break;
                        }
                    }

                    if (isRisk)
                    {
                        checkedMst.Add(mstBan);
                        commands.AppendLine(BuildUpdateErrorQuery(errorText, idHd));
                    }
                }

                if (commands.Length > 0)
                {
                    _dbManager.ExecuteSqlQueryAsync(commands.ToString());
                }
            }
            catch (Exception)
            {
                // Log error
                throw;
            }
        }

        public async Task CheckCtAsync(string idHd)
        {
            // Implementation placeholder
            await Task.CompletedTask;
            throw new NotImplementedException("This method requires additional refactoring of SmImexInv");
        }

        public async Task CreateBkhddvtctAsync(string idHd,DbManager myDb)
        {
            if (string.IsNullOrWhiteSpace(idHd))
                return;

            try
            {                
                var cmd = new System.Data.SqlClient.SqlCommand("EXEC dbo.POImexInv_Create_Bkhddvtct @Id_hd");
                cmd.Parameters.Add("@Id_hd", System.Data.SqlDbType.VarChar).Value = idHd;
                await Task.Run(() => myDb.ExecuteSqlCommandAsync(cmd));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string BuildUpdateErrorQuery(string errorText, string idHd)
        {
            return $@"UPDATE ph92 
                     SET noi_dung_loi = CASE 
                         WHEN ISNULL(noi_dung_loi, '') = '' THEN N'{errorText}' 
                         ELSE noi_dung_loi + N'; {errorText}' 
                     END 
                     WHERE id_hd = '{idHd}';";
        }
    }
}
