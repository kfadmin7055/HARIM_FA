using Core.Class;
using DevExpress.Spreadsheet.Charts;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HM_WAP_JEIL_DOSING
{
    public partial class frm_ElUseResult : DevExpress.XtraEditors.XtraForm
    {
        public frm_ElUseResult()
        {
            InitializeComponent();
        }

        private void XMain_Search()
        {
            try
            {
                pivotGridControl.DataSource = null;
                pivotGridControl.BeginUpdate();

                string SQL =
                "SELECT CONVERT(NVARCHAR(10), CONVERT(DATE, EL.GET_DATE)) AS GET_DATE, EL.GUBUN, EL.VAL " +
                "FROM ( " +
                "SELECT GET_DATE, '분쇄1호기' AS GUBUN,  " +
                "MAX(G1_P) -  LAG(MAX(G1_P)) OVER(ORDER BY GET_DATE) AS VAL " +
                "FROM EL_USE " +
                "GROUP BY GET_DATE " +
                "UNION  " +
                "SELECT GET_DATE, '분쇄2호기',  " +
                "MAX(G2_P) -  LAG(MAX(G2_P)) OVER(ORDER BY GET_DATE) " +
                "FROM EL_USE " +
                "GROUP BY GET_DATE " +
                "UNION  " +
                "SELECT GET_DATE, '분쇄3호기',  " +
                "MAX(G3_P) -  LAG(MAX(G3_P)) OVER(ORDER BY GET_DATE) " +
                "FROM EL_USE " +
                "GROUP BY GET_DATE " +
                "UNION  " +
                "SELECT GET_DATE, '배합믹서기',  " +
                "MAX(M1_P) -  LAG(MAX(M1_P)) OVER(ORDER BY GET_DATE) " +
                "FROM EL_USE " +
                "GROUP BY GET_DATE " +
                "UNION  " +
                "SELECT GET_DATE, '펠렛1호기',  " +
                "MAX(P1_P) -  LAG(MAX(P1_P)) OVER(ORDER BY GET_DATE) " +
                "FROM EL_USE " +
                "GROUP BY GET_DATE " +
                "UNION  " +
                "SELECT GET_DATE, '펠렛2호기',  " +
                "MAX(P2_P) -  LAG(MAX(P2_P)) OVER(ORDER BY GET_DATE) " +
                "FROM EL_USE " +
                "GROUP BY GET_DATE " +
                "UNION  " +
                "SELECT GET_DATE, '콤프레셔1',  " +
                "MAX(C1_P) -  LAG(MAX(C1_P)) OVER(ORDER BY GET_DATE) " +
                "FROM EL_USE " +
                "GROUP BY GET_DATE " +
                "UNION  " +
                "SELECT GET_DATE, '콤프레셔2',  " +
                "MAX(C2_P) -  LAG(MAX(C2_P)) OVER(ORDER BY GET_DATE) " +
                "FROM EL_USE " +
                "GROUP BY GET_DATE " +
                "UNION  " +
                "SELECT GET_DATE, '보일러',  " +
                "MAX(B1_P) -  LAG(MAX(B1_P)) OVER(ORDER BY GET_DATE) " +
                "FROM EL_USE " +
                "GROUP BY GET_DATE " +
                ") EL " +
                "WHERE EL.GET_DATE BETWEEN '{0}' AND '{1}' " +
                "ORDER BY EL.GET_DATE ";


                SQL = string.Format(SQL,
                     string.Format("{0:yyyyMMdd}", dateEdit_workStartDate.EditValue),
                      string.Format("{0:yyyyMMdd}", dateEdit_workEndDate.EditValue)
                    );
                DataSet XMain_SearchDs = Dbconn.conn.ExecutDataset(SQL);

                pivotGridControl.DataSource = XMain_SearchDs.Tables[0];

                pivotGridControl.EndUpdate();
                pivotGridControl.OptionsView.ShowFilterHeaders = false;

                pivotGridControl.BestFit();

                //chart
                SQL =
                    "SELECT CONVERT(DATE, EL.GET_DATE) AS GET_DATE, EL.G1_P, EL.G2_P, EL.G3_P, EL.M1_P, EL.P1_P, EL.P2_P, EL.B1_P,  " +
                    "EL.G1_P_DAY, EL.G2_P_DAY, EL.G3_P_DAY, EL.M1_P_DAY, EL.P1_P_DAY, EL.P2_P_DAY, EL.B1_P_DAY " +
                    "FROM ( " +
                    "SELECT GET_DATE,  " +
                    "MAX(G1_P) AS G1_P, MAX(G2_P) AS G2_P, MAX(G3_P) AS G3_P,  " +
                    "MAX(M1_P) AS M1_P, MAX(P1_P) AS P1_P, MAX(P2_P) AS P2_P,  " +
                    "MAX(B1_P) AS B1_P, " +
                    "MAX(G1_P) -  LAG(MAX(G1_P)) OVER(ORDER BY GET_DATE) AS G1_P_DAY, " +
                    "MAX(G2_P) -  LAG(MAX(G2_P)) OVER(ORDER BY GET_DATE) AS G2_P_DAY, " +
                    "MAX(G3_P) -  LAG(MAX(G3_P)) OVER(ORDER BY GET_DATE) AS G3_P_DAY, " +
                    "MAX(M1_P) -  LAG(MAX(M1_P)) OVER(ORDER BY GET_DATE) AS M1_P_DAY, " +
                    "MAX(P1_P) -  LAG(MAX(P1_P)) OVER(ORDER BY GET_DATE) AS P1_P_DAY, " +
                    "MAX(P2_P) -  LAG(MAX(P2_P)) OVER(ORDER BY GET_DATE) AS P2_P_DAY, " +
                    "MAX(C1_P) -  LAG(MAX(C1_P)) OVER(ORDER BY GET_DATE) AS C1_P_DAY, " +
                    "MAX(C2_P) -  LAG(MAX(C2_P)) OVER(ORDER BY GET_DATE) AS C2_P_DAY, " +
                    "MAX(B1_P) -  LAG(MAX(B1_P)) OVER(ORDER BY GET_DATE) AS B1_P_DAY " +
                    "FROM EL_USE " +
                    "GROUP BY GET_DATE " +
                    ") EL " +
                    "WHERE EL.GET_DATE BETWEEN '{0}' AND '{1}' " +
                    "ORDER BY EL.GET_DATE ";


                SQL = string.Format(SQL,
                     string.Format("{0:yyyyMMdd}", dateEdit_workStartDate.EditValue),
                      string.Format("{0:yyyyMMdd}", dateEdit_workEndDate.EditValue)
                    );
                DataSet chartDs = Dbconn.conn.ExecutDataset(SQL);

                chartControl.Series.Clear();


                //title
                chartControl.Titles.Clear();
                DevExpress.XtraCharts.ChartTitle chartTitle = new DevExpress.XtraCharts.ChartTitle();
                chartTitle.Text = "전력사용량 (단위: kwh)";
                chartTitle.Font = new System.Drawing.Font("맑은 고딕", 10F, FontStyle.Bold);
                chartControl.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] { chartTitle });

                chartControl.Legend.Visible = true;

                //series add 
                chartControl.Series.Add(new DevExpress.XtraCharts.Series("분쇄1호기", ViewType.Spline));
                chartControl.Series.Add(new DevExpress.XtraCharts.Series("분쇄2호기", ViewType.Spline));
                chartControl.Series.Add(new DevExpress.XtraCharts.Series("분쇄3호기", ViewType.Spline));
                chartControl.Series.Add(new DevExpress.XtraCharts.Series("배합믹서기", ViewType.Spline));
                chartControl.Series.Add(new DevExpress.XtraCharts.Series("펠렛1호기", ViewType.Spline));
                chartControl.Series.Add(new DevExpress.XtraCharts.Series("펠렛2호기", ViewType.Spline));
                chartControl.Series.Add(new DevExpress.XtraCharts.Series("콤프레셔1", ViewType.Spline));
                chartControl.Series.Add(new DevExpress.XtraCharts.Series("콤프레셔2", ViewType.Spline));
                chartControl.Series.Add(new DevExpress.XtraCharts.Series("보일러", ViewType.Spline));

                chartControl.Series["분쇄1호기"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                chartControl.Series["분쇄1호기"].Label.TextPattern = "{V:N2} kwh";
                chartControl.Series["분쇄2호기"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                chartControl.Series["분쇄2호기"].Label.TextPattern = "{V:N2} kwh";
                chartControl.Series["분쇄3호기"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                chartControl.Series["분쇄3호기"].Label.TextPattern = "{V:N2} kwh";
                chartControl.Series["배합믹서기"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                chartControl.Series["배합믹서기"].Label.TextPattern = "{V:N2} kwh";
                chartControl.Series["펠렛1호기"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                chartControl.Series["펠렛1호기"].Label.TextPattern = "{V:N2} kwh";
                chartControl.Series["펠렛2호기"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                chartControl.Series["펠렛2호기"].Label.TextPattern = "{V:N2} kwh";
                chartControl.Series["콤프레셔1"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                chartControl.Series["콤프레셔1"].Label.TextPattern = "{V:N2} kwh";
                chartControl.Series["콤프레셔2"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                chartControl.Series["콤프레셔2"].Label.TextPattern = "{V:N2} kwh";
                chartControl.Series["보일러"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                chartControl.Series["보일러"].Label.TextPattern = "{V:N2} kwh";

                chartControl.Legend.MarkerMode = LegendMarkerMode.CheckBoxAndMarker;
                chartControl.AppearanceName = "Light";
                chartControl.PaletteName = "Median";
                chartControl.IndicatorsPaletteName = "Default";


                (chartControl.Series["분쇄1호기"].View as LineSeriesView).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
                (chartControl.Series["분쇄2호기"].View as LineSeriesView).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
                (chartControl.Series["분쇄3호기"].View as LineSeriesView).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
                (chartControl.Series["배합믹서기"].View as LineSeriesView).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
                (chartControl.Series["펠렛1호기"].View as LineSeriesView).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
                (chartControl.Series["펠렛2호기"].View as LineSeriesView).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
                (chartControl.Series["콤프레셔1"].View as LineSeriesView).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
                (chartControl.Series["콤프레셔2"].View as LineSeriesView).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
                (chartControl.Series["보일러"].View as LineSeriesView).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;

                //(chartControl.Series["분쇄1호기"].View as LineSeriesView).LineMarkerOptions.Kind = MarkerKind.Plus;

                for (int i = 0; i < Dbconn.conn.getRowCnt(chartDs); i++)
                {
                    chartControl.Series["분쇄1호기"].Points.Add(new SeriesPoint(Dbconn.conn.getData(chartDs, "GET_DATE", i), new double[] { Convert.ToDouble(Dbconn.conn.getData(chartDs, "G1_P_DAY", i)) }));
                    chartControl.Series["분쇄2호기"].Points.Add(new SeriesPoint(Dbconn.conn.getData(chartDs, "GET_DATE", i), new double[] { Convert.ToDouble(Dbconn.conn.getData(chartDs, "G2_P_DAY", i)) }));
                    chartControl.Series["분쇄3호기"].Points.Add(new SeriesPoint(Dbconn.conn.getData(chartDs, "GET_DATE", i), new double[] { Convert.ToDouble(Dbconn.conn.getData(chartDs, "G3_P_DAY", i)) }));
                    chartControl.Series["배합믹서기"].Points.Add(new SeriesPoint(Dbconn.conn.getData(chartDs, "GET_DATE", i), new double[] { Convert.ToDouble(Dbconn.conn.getData(chartDs, "M1_P_DAY", i)) }));
                    chartControl.Series["펠렛1호기"].Points.Add(new SeriesPoint(Dbconn.conn.getData(chartDs, "GET_DATE", i), new double[] { Convert.ToDouble(Dbconn.conn.getData(chartDs, "P1_P_DAY", i)) }));
                    chartControl.Series["펠렛2호기"].Points.Add(new SeriesPoint(Dbconn.conn.getData(chartDs, "GET_DATE", i), new double[] { Convert.ToDouble(Dbconn.conn.getData(chartDs, "P2_P_DAY", i)) }));
                    chartControl.Series["콤프레셔1"].Points.Add(new SeriesPoint(Dbconn.conn.getData(chartDs, "GET_DATE", i), new double[] { Convert.ToDouble(Dbconn.conn.getData(chartDs, "C1_P_DAY", i)) }));
                    chartControl.Series["콤프레셔2"].Points.Add(new SeriesPoint(Dbconn.conn.getData(chartDs, "GET_DATE", i), new double[] { Convert.ToDouble(Dbconn.conn.getData(chartDs, "C2_P_DAY", i)) }));
                    chartControl.Series["보일러"].Points.Add(new SeriesPoint(Dbconn.conn.getData(chartDs, "GET_DATE", i), new double[] { Convert.ToDouble(Dbconn.conn.getData(chartDs, "B1_P_DAY", i)) }));
                }

                //zoom
                XYDiagram xyDiagram = (XYDiagram)chartControl.Diagram;
                xyDiagram.EnableAxisXZooming = true;
                xyDiagram.EnableAxisYZooming = true;
                xyDiagram.ZoomingOptions.UseKeyboard = true;
                xyDiagram.ZoomingOptions.UseKeyboardWithMouse = true;
                xyDiagram.ZoomingOptions.UseMouseWheel = true;
                xyDiagram.ZoomingOptions.UseTouchDevice = true;
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());
            }

        }

        private void frm_ElUseResult_Load(object sender, EventArgs e)
        {
            dateEdit_workStartDate.EditValue = DateTime.Today.AddDays(-7);
            dateEdit_workEndDate.EditValue = DateTime.Today.AddDays(1);

            XMain_Search();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void btn_excelExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "XLSX File(*.xlsx)|*.xlsx";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    pivotGridControl.ExportToXlsx(sfd.FileName);
                    System.Diagnostics.Process.Start(sfd.FileName);
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "btn_excelExport_Click", ex);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());
            }
        }

        private void checkEdit_chartLabelView_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkEdit_chartLabelView.Checked)
                {
                    chartControl.Series["분쇄1호기"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                    chartControl.Series["분쇄2호기"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                    chartControl.Series["분쇄3호기"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                    chartControl.Series["배합믹서기"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                    chartControl.Series["펠렛1호기"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                    chartControl.Series["펠렛2호기"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                    chartControl.Series["콤프레셔1"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                    chartControl.Series["콤프레셔2"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                    chartControl.Series["보일러"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                }
                else
                {
                    chartControl.Series["분쇄1호기"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
                    chartControl.Series["분쇄2호기"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
                    chartControl.Series["분쇄3호기"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
                    chartControl.Series["배합믹서기"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
                    chartControl.Series["펠렛1호기"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
                    chartControl.Series["펠렛2호기"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
                    chartControl.Series["콤프레셔1"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
                    chartControl.Series["콤프레셔2"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
                    chartControl.Series["보일러"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "checkEdit_chartLabelView_CheckedChanged", ex);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());
            }
        }

        private void gridControl_KeyDown(object sender, KeyEventArgs e)
        {
            // 조회
            if (e.KeyCode == Keys.F5)
            {
                XMain_Search();
            }

            //// 신규 행 추가
            //if (e.KeyCode == Keys.F3)
            //{
            //    btn_rowAdd_Click(sender, e);
            //}

            //// 행 삭제
            //if (e.KeyCode == Keys.Delete)
            //{
            //    btn_rowDel_Click(sender, e);
            //}

            //// 저장
            //if (e.Control && e.KeyCode == Keys.S)
            //{
            //    XMain_Save();
            //}

            //// 삭제
            //if (e.Control && e.KeyCode == Keys.D)
            //{
            //    XMain_Delete();
            //}
        }

        private void frm_Shown(object sender, EventArgs e)
        {
            pivotGridControl.Focus();
        }
    }
}