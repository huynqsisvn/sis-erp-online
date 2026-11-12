
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace SisCa
{
    internal class DataF8
    {
        private DataF8()
        {
        }

        public static void Delete(object recPh, object recCt)
        {
           // new DataF8() { Ph = recPh, Ct = recCt }.Delete();
        }

        private object Ph { get; set; }

        private object Ct { get; set; }

        private string Stt_rec { get; set; }

        private string Stt_rec_tt { get; set; }

        private Decimal Tien_con_pt { get; set; }

        private Decimal Tien_con_pt0 { get; set; }

        private Decimal Tien_dt { get; set; }

        private string Ma_ct { get; set; }

        private string Ma_nt { get; set; }

        private string Tk { get; set; }

        /*

        private void Delete()
        {
            if (this.Ph == null || this.Ct == null)
                return;
            this.Load();
            if (this.DeleteOnServer() == 0)
            {
                int num = (int)ExMessageBox.Show(-1295, StartupBase.SasObj, "Chưa phân bổ hoặc phân bổ đã bị xóa!", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
                this.DeleteView();
        }

        private void Load()
        {
            this.Stt_rec = this.Ph.Cells["stt_rec"].Value.ToString();
            this.Stt_rec_tt = this.Ct.Cells["stt_rec"].Value.ToString();
            this.Ma_ct = this.Ph.Cells["ma_ct"].Value.ToString();
            this.Ma_nt = this.Ct.Cells["ma_nt"].Value.ToString();
            this.Tk = this.Ph.Cells["tk_i"].Value.ToString();
            this.Tien_con_pt0 = PhanBo.ToDec(this.Ct.Cells["tien_con_pt0"].Value);
            this.Tien_con_pt = PhanBo.ToDec(this.Ct.Cells["tien_con_pt"].Value);
            this.Tien_dt = PhanBo.ToDec(this.Ct.Cells["tien_dt"].Value);
        }

        private int DeleteOnServer()
        {
            SqlCommand sqlcmd = new SqlCommand(StartUp.Procedure[3]);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.Add("@Stt_rec", SqlDbType.VarChar).Value = (object)this.Stt_rec;
            sqlcmd.Parameters.Add("@Stt_rec_tt", SqlDbType.VarChar).Value = (object)this.Stt_rec_tt;
            sqlcmd.Parameters.Add("@Ma_ct", SqlDbType.VarChar).Value = (object)this.Ma_ct;
            sqlcmd.Parameters.Add("@Tk", SqlDbType.VarChar).Value = (object)this.Tk;
            object obj = StartupBase.SasObj.ExcuteScalar(sqlcmd);
            return obj == null || obj is DBNull ? 0 : (int)obj;
        }

        private void DeleteView()
        {
            DataRowView dataItem1 = this.Ph.DataItem as DataRowView;
            DataRowView dataItem2 = this.Ct.DataItem as DataRowView;
            dataItem2["tien_con_pt"] = (object)this.Tien_con_pt0;
            dataItem2["tien_dt"] = (object)0;
            dataItem2["tag"] = (object)"";
            dataItem1["da_pb"] = (object)(PhanBo.ToDec(dataItem1["da_pb"]) - this.Tien_dt);
            dataItem1["tien_chua_pb"] = (object)(PhanBo.ToDec(dataItem1["t_tien_nt"]) - PhanBo.ToDec(dataItem1["da_pb"]));
            dataItem2["date_pb"] = (object)DBNull.Value;
            dataItem2["time_pb"] = (object)DBNull.Value;
            dataItem2["user_id_pb"] = (object)DBNull.Value;
            dataItem2["user_name_pb"] = (object)DBNull.Value;
        }

        */
    }
}
