using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace SisGl.Pages.General.Glso21
{
    public partial class GLSO2_TT200 : DevExpress.XtraReports.UI.XtraReport
    {
        public GLSO2_TT200()
        {
            InitializeComponent();
        }
        private void GLSO2_TT200_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var tb = (this.DataSource as DataSet)?.Tables["LsData"];
            if (tb == null || !tb.Columns.Contains("cfDienGiai"))
                return;

            // Nhóm theo stt_rec
            var groups = tb.AsEnumerable().GroupBy(r => r.Field<string>("stt_rec"));

            foreach (var group in groups)
            {
                // Lấy diễn giải đầu tiên (nếu rỗng thì dùng Ten_kh)
                string dienGiai = group.Select(r => r["dien_giai"]?.ToString()?.Trim())
                                       .FirstOrDefault(s => !string.IsNullOrEmpty(s));

                if (string.IsNullOrEmpty(dienGiai))
                    dienGiai = group.Select(r => r["ten_kh"]?.ToString()?.Trim())
                                    .FirstOrDefault(s => !string.IsNullOrEmpty(s));

                // Gán vào tất cả dòng trong nhóm
                foreach (DataRow row in group)
                    row["cfDienGiai"] = dienGiai;
            }
        }

        private void detailBand1_BeforePrint(object sender, CancelEventArgs e)
        {

        }
    }
}
