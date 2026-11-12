using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace SisFa.Pages.Input.FAbtpbts_Tao
{
    public partial class FABTPBTS : DevExpress.XtraReports.UI.XtraReport
    {
        public FABTPBTS()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Bind report data model and set header labels.
        /// Because Designer fields are private, only this partial class can access them.
        /// </summary>
        public void SetReportData(FAbtpbts_TaoReportModel model, string tenCty, string diaChi, string title, string tenKtt)
        {
            lblten_cty.Text = tenCty ?? "";
            lblDia_chi.Text = diaChi ?? "";
            lbltitle.Text   = title  ?? "";
            lblten_ktt.Text = tenKtt ?? "";

            objectDataSource1.DataSource = new List<FAbtpbts_TaoReportModel> { model };
        }
    }
}
