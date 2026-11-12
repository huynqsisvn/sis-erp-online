using DevExpress.XtraReports.UI;
using DevExpress.DataAccess.ObjectBinding;
using SisFx.Pages.Report.FXTHECC;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace SisFx.Pages.Report.FXTHECC.V
{
    public partial class FXTHECC1 : DevExpress.XtraReports.UI.XtraReport
    {
        private FXTHECCModel _model;

        public FXTHECC1()
        {
            InitializeComponent();

            this.BeforePrint += (s, e) => xtraReport1_BeforePrint(s, e);
            this.subreport1.BeforePrint += (s, e) => subreport1_BeforePrint(s, e);
            this.subreport3.BeforePrint += (s, e) => subreport3_BeforePrint(s, e);
        }

        private FXTHECCModel ResolveModel()
        {
            // Case 1: DataSource được gán trực tiếp = FXTHECCModel
            if (this.DataSource is FXTHECCModel directModel)
                return directModel;

            // Case 2: DataSource = ObjectDataSource wrapper → lấy .DataSource bên trong
            if (this.DataSource is ObjectDataSource ods && ods.DataSource is FXTHECCModel odsModel)
                return odsModel;

            // Case 3: objectDataSource1 đã được gán data từ bên ngoài
            if (objectDataSource1.DataSource is FXTHECCModel ds1Model)
                return ds1Model;

            return null;
        }

        private void xtraReport1_BeforePrint(object sender, EventArgs e)
        {
            _model = ResolveModel();
            if (_model == null) return;

            // Report cha DataMember="tmpInfo" → gán List<FXTHECCPrint1_Info> trực tiếp
            // Tương đương WinForm: ds.Tables["tmpInfo"] (Tables[2])
            objectDataSource1.DataSource = _model.tmpInfo;
            objectDataSource1.DataMember = null;
        }

        private void subreport1_BeforePrint(object sender, EventArgs e)
        {
            if (_model == null) return;
            var sub = sender as XRSubreport;
            if (sub?.ReportSource == null) return;

            // Tương đương WinForm: subreport1.ReportSource.DataSource = ds.Tables[0] (tMtemp1)
            // Gán trực tiếp List → subreport không cần DataMember nữa
            sub.ReportSource.DataSource = _model.tMtemp1;
            sub.ReportSource.DataMember = null;
        }

        private void subreport3_BeforePrint(object sender, EventArgs e)
        {
            if (_model == null) return;
            var sub = sender as XRSubreport;
            if (sub?.ReportSource == null) return;

            // Tương đương WinForm: subreport3.ReportSource.DataSource = ds.Tables[1] (Accessories)
            // Gán trực tiếp List → subreport không cần DataMember nữa
            sub.ReportSource.DataSource = _model.Accessories;
            sub.ReportSource.DataMember = null;
        }
    }
}
