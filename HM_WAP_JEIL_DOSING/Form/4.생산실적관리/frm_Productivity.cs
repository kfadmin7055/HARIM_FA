using Core.Class;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraPivotGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class frm_Productivity : DevExpress.XtraEditors.XtraForm
    {
        private string[] sValid = null;

        public frm_Productivity()
        {
            InitializeComponent();
            clsDevexpressGrid.ReadGridViewInit(gridView, Properties.Settings.Default.FontSize);
        }

        private void XMain_Search()
        {
            try
            {
                string SQL =
                "SELECT YEAR +'-'+ MONTH AS DATE, DEPART, QTY, MEN, OFFICETIME, WORKTIME, PRODUCT, MH, MP " +
                "FROM TMP_PRODUCTION_INDICATOR  " +
                "WHERE YEAR + MONTH BETWEEN '{0}' AND '{1}' " +
                "ORDER BY YEAR, MONTH, CASE WHEN DEPART = '원료하차' THEN 1 WHEN DEPART = '원료투입' THEN 2 " +
                "WHEN DEPART = '가공' THEN 3 WHEN DEPART = '배합' THEN 4 " +
                "WHEN DEPART = '대용유배합' THEN 5 WHEN DEPART = '지대포장' THEN 6 " +
                "WHEN DEPART = '톤백포장' THEN 7 WHEN DEPART = '지대상차' THEN 8 " +
                "WHEN DEPART = '톤백상차' THEN 9 END ";



                /*                DataSet XMain_SearchDs = Dbconn.conn.ExecutDataset(SQL);
                                clsDevexpressGrid.BindGridControl(gridControl, gridView, XMain_SearchDs.Tables[0], true);*/


                SQL =
                " SELECT DATE   " +
                "      , depart + ' ' + dept AS NAME  " +
                "      , ABS(VAL) AS VAL  " +
                "FROM  " +
                "(SELECT YEAR +'-'+ MONTH  AS DATE, DEPART,  " +
                "CONVERT(numeric(18,3), MEN) AS 작업인원,  " +
                "OFFICETIME as 작업시간, WORKTIME as 근무시간,  CONVERT(numeric(18,3), QTY/1000) as 작업량,  " +
                "CONVERT(numeric(18,3), PRODUCT/1000) AS 생산량,  " +
                "MH as 'M/H', CONVERT(numeric(18,3), MP/1000) as 'M/P'  " +
                "FROM  " +
                "TMP_PRODUCTION_INDICATOR  " +
                "WHERE YEAR + MONTH BETWEEN '{0}' AND '{1}'   " +
                ") A  " +
                "UNPIVOT (VAL FOR dept IN ([작업인원],[작업시간], [근무시간],[작업량],[생산량],[M/H],[M/P])) AS unpivot_result  " +
                "ORDER BY DATE, CASE WHEN DEPART = '원료하차' THEN 1 WHEN DEPART = '원료투입' THEN 2   " +
                "WHEN DEPART = '가공' THEN 3 WHEN DEPART = '배합' THEN 4   " +
                "WHEN DEPART = '대용유배합' THEN 5 WHEN DEPART = '지대포장' THEN 6   " +
                "WHEN DEPART = '톤백포장' THEN 7 WHEN DEPART = '지대상차' THEN 8   " +
                "WHEN DEPART = '톤백상차' THEN 9 END, dept";

                SQL = string.Format(SQL,
                  string.Format("{0:yyyyMM}", dateEdit_workStartDate.EditValue),
                  string.Format("{0:yyyyMM}", dateEdit_workEndDate.EditValue)
                );

                DataSet XMain_SearchDs2 = Dbconn.conn.ExecutDataset(SQL);

                pivotGridControl.DataSource = null;

                pivotGridControl.BeginUpdate();
                pivotGridControl.DataSource = XMain_SearchDs2.Tables[0];

                pivotGridControl.EndUpdate();

                pivotGridControl.OptionsView.ShowFilterHeaders = false;
                //pivotGridControl.BestFit();


                //chart
                SQL =
                "SELECT YEAR +'-'+ MONTH AS DATE, DEPART, (QTY / 1000) as QTY, MEN, OFFICETIME, WORKTIME, PRODUCT, MH, (MP/1000) AS MP " +
                "FROM TMP_PRODUCTION_INDICATOR  " +
                "WHERE YEAR + MONTH BETWEEN '{0}' AND '{1}' " +
                "AND DEPART = '{2}' " +
                "ORDER BY YEAR, MONTH, CASE WHEN DEPART = '원료하차' THEN 1 WHEN DEPART = '원료투입' THEN 2 " +
                "WHEN DEPART = '가공' THEN 3 WHEN DEPART = '배합' THEN 4 " +
                "WHEN DEPART = '대용유배합' THEN 5 WHEN DEPART = '지대포장' THEN 6 " +
                "WHEN DEPART = '톤백포장' THEN 7 WHEN DEPART = '지대상차' THEN 8 " +
                "WHEN DEPART = '톤백상차' THEN 9 END ";

                SQL = string.Format(SQL,
                  string.Format("{0:yyyyMM}", dateEdit_workStartDate.EditValue),
                  string.Format("{0:yyyyMM}", dateEdit_workEndDate.EditValue),
                  comboBoxEdit_pcSel.Text
                );

                DataSet chartDs = Dbconn.conn.ExecutDataset(SQL);

                chartControl.Series.Clear();

                //title
                chartControl.Titles.Clear();
                DevExpress.XtraCharts.ChartTitle chartTitle = new DevExpress.XtraCharts.ChartTitle();
                chartTitle.Text = comboBoxEdit_pcSel.Text + " 종합생산성 차트  /  단위: 작업량(톤), M/H(킬로그램), M/P(톤), 시간(분)";
                chartTitle.Font = new System.Drawing.Font("맑은 고딕", 10F, FontStyle.Bold);
                chartControl.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] { chartTitle });

                chartControl.Legend.Visible = true;


                //원료하차
                //원료투입
                //가공
                //배합
                //대용유배합
                //지대포장
                //톤백포장
                //지대상차
                //톤백상차

                //series add 
                chartControl.Series.Add(new DevExpress.XtraCharts.Series(comboBoxEdit_pcSel.Text + " 작업량", ViewType.Bar));
                chartControl.Series.Add(new DevExpress.XtraCharts.Series(comboBoxEdit_pcSel.Text + " M/H", ViewType.Spline));
                chartControl.Series.Add(new DevExpress.XtraCharts.Series(comboBoxEdit_pcSel.Text + " M/P", ViewType.Spline));



                chartControl.Series[comboBoxEdit_pcSel.Text + " 작업량"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                chartControl.Series[comboBoxEdit_pcSel.Text + " 작업량"].Label.TextPattern = "{V:N0} ton";
                chartControl.Series[comboBoxEdit_pcSel.Text + " M/H"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                chartControl.Series[comboBoxEdit_pcSel.Text + " M/H"].Label.TextPattern = "{V:N0} kg";
                chartControl.Series[comboBoxEdit_pcSel.Text + " M/P"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                chartControl.Series[comboBoxEdit_pcSel.Text + " M/P"].Label.TextPattern = "{V:N0} ton";

                chartControl.Legend.MarkerMode = LegendMarkerMode.CheckBoxAndMarker;
                chartControl.AppearanceName = "Light";
                chartControl.PaletteName = "Median";
                chartControl.IndicatorsPaletteName = "Default";


                // (chartControl.Series[comboBoxEdit_pcSel.Text + " 작업량"].View as LineSeriesView).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
                (chartControl.Series[comboBoxEdit_pcSel.Text + " M/H"].View as LineSeriesView).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
                (chartControl.Series[comboBoxEdit_pcSel.Text + " M/P"].View as LineSeriesView).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;

                XYDiagram diagram = (XYDiagram)chartControl.Diagram;
                diagram.AxisX.DateTimeGridAlignment = DateTimeMeasurementUnit.Month;
                diagram.AxisX.DateTimeMeasureUnit = DateTimeMeasurementUnit.Month;
                diagram.AxisX.DateTimeOptions.Format = DateTimeFormat.Custom;
                diagram.AxisX.DateTimeOptions.FormatString = "MMMM";

                //(chartControl.Series["분쇄1호기"].View as LineSeriesView).LineMarkerOptions.Kind = MarkerKind.Plus;

                (chartControl.Series[comboBoxEdit_pcSel.Text + " M/H"].View as LineSeriesView).LineMarkerOptions.Kind = MarkerKind.Star;
                (chartControl.Series[comboBoxEdit_pcSel.Text + " M/P"].View as LineSeriesView).LineMarkerOptions.Kind = MarkerKind.Pentagon;

                for (int i = 0; i < Dbconn.conn.getRowCnt(chartDs); i++)
                {
                    chartControl.Series[Dbconn.conn.getData(chartDs, "DEPART", i) + " 작업량"].Points.Add(new SeriesPoint(Dbconn.conn.getData(chartDs, "DATE", i), new double[] { Convert.ToDouble(Dbconn.conn.getData(chartDs, "QTY", i)) }));
                    chartControl.Series[Dbconn.conn.getData(chartDs, "DEPART", i) + " M/H"].Points.Add(new SeriesPoint(Dbconn.conn.getData(chartDs, "DATE", i), new double[] { Convert.ToDouble(Dbconn.conn.getData(chartDs, "MH", i)) }));
                    chartControl.Series[Dbconn.conn.getData(chartDs, "DEPART", i) + " M/P"].Points.Add(new SeriesPoint(Dbconn.conn.getData(chartDs, "DATE", i), new double[] { Convert.ToDouble(Dbconn.conn.getData(chartDs, "MP", i)) }));
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


        private void gridView_ColumnPositionChanged(object sender, EventArgs e)
        {
            // clsDevexpressGrid.SaveGridColumnSeq(this.Name, gridControl, gridView);
        }

        private void frm_Productivity_Load(object sender, EventArgs e)
        {

            try
            {
                clsDevexpressGrid.LoadGridColumnSeq(this.Name, gridControl, gridView);

                dateEdit_workStartDate.EditValue = DateTime.Today.AddMonths(-DateTime.Today.Month - 1);
                dateEdit_workEndDate.EditValue = DateTime.Today.AddDays(1);

                // 플랜트
                clsDevexpressUtil.ItemLookUpEditSetup(cboPlant_Code, clsCommon.GetPlant(), "", false, 0, false);
                cboPlant_Code.EditValue = clsCommon.PlantCode;

                comboBoxEdit_pcSel.Properties.Items.BeginUpdate();
                comboBoxEdit_pcSel.Properties.Items.Clear();
                comboBoxEdit_pcSel.Properties.Items.Add("원료하차");
                comboBoxEdit_pcSel.Properties.Items.Add("원료투입");
                comboBoxEdit_pcSel.Properties.Items.Add("가공");
                comboBoxEdit_pcSel.Properties.Items.Add("배합");
                comboBoxEdit_pcSel.Properties.Items.Add("대용유배합");
                comboBoxEdit_pcSel.Properties.Items.Add("지대포장");
                comboBoxEdit_pcSel.Properties.Items.Add("톤백포장");
                comboBoxEdit_pcSel.Properties.Items.Add("지대상차");
                comboBoxEdit_pcSel.Properties.Items.Add("톤백상차");

                comboBoxEdit_pcSel.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                comboBoxEdit_pcSel.Properties.Items.EndUpdate();

                comboBoxEdit_pcSel.SelectedIndex = 0;

                XMain_Search();

                // Application.AddMessageFilter(new GridMouseWheelFilter(gridControl));
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "frm_Productivity_Load", ex);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());
            }

        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void checkEdit_chartLabelView_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkEdit_chartLabelView.Checked)
                {
                    chartControl.Series[comboBoxEdit_pcSel.Text + " 작업량"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                    chartControl.Series[comboBoxEdit_pcSel.Text + " M/H"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                    chartControl.Series[comboBoxEdit_pcSel.Text + " M/P"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
                }
                else
                {
                    chartControl.Series[comboBoxEdit_pcSel.Text + " 작업량"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
                    chartControl.Series[comboBoxEdit_pcSel.Text + " M/H"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
                    chartControl.Series[comboBoxEdit_pcSel.Text + " M/P"].LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "checkEdit_chartLabelView_CheckedChanged", ex);
                ShowMessageBox.XtraShowWarning(ex.Message.ToString());
            }
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
            gridControl.Focus();
            gridView.FocusedRowHandle = 0;
            gridView.FocusedColumn = gridView.VisibleColumns[0];
        }

        private void cboProcess_Key_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void cboPlant_Code_EditValueChanged(object sender, EventArgs e)
        {
            // 공정
            clsDevexpressUtil.ItemLookUpEditSetup(cboProcess_Key, clsCommon.GetProcess(cboPlant_Code.EditValue?.ToString()), "", false, 0, true);

            //라인
            clsDevexpressUtil.ItemLookUpEditSetup(cboL_Code, clsCommon.GetLine(cboPlant_Code.EditValue?.ToString(), cboProcess_Key.EditValue?.ToString()), "", false, 0, false);

            XMain_Search();
        }

        private void cboL_Code_EditValueChanged(object sender, EventArgs e)
        {

            XMain_Search();

        }

        private void dateEdit_workStartDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void dateEdit_workEndDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void comboBoxEdit_pcSel_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}