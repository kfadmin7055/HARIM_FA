using Core.Class;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid;
using DevExpress.XtraScheduler.Printing;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace HARIM_FA_DOSING
{
    public partial class m_Input_Ploadm : DevExpress.XtraEditors.XtraForm
    {
        public string   vDISPATCHNO  { get; set; }    // 배차번호
        public string   vORDERNO     { get; set; }    // 주문번호
        public string[] vORDERLINENO { get; set; }    // 라인번호
        public string[] vRESOURCE_NO { get; set; }    // 품목코드
        public string vPlantCode { get; set; }    // 품목코드

        public string[] SelectedValue { get; set; }
        private string SQL = String.Empty;
        DataSet argDataSet;
        string resourceNo = string.Empty;
        string suGubun = string.Empty;

        private string sIS_NO = string.Empty;

        public m_Input_Ploadm(string sPlantCode, string sVEHICLENO)
        {
            InitializeComponent();
            clsDevexpressGrid.ReadGridViewInit(viewMain, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(viewDetail, Properties.Settings.Default.FontSize);

            txtVEHICLENO.Text = sVEHICLENO;
            vPlantCode = sPlantCode;

            dtFromDeliveryDate.EditValue = DateTime.Today.AddDays(-7);
            dtToDeliveryDate.EditValue = DateTime.Today;
        }

        private void m_Input_Ploadm_Load(object sender, EventArgs e)
        {
                XMain_Search();
        }

        #region 조회함수
        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                SELECT 
                   DISPATCHNO, VEHICLECODE, TMSDIVISIONCODE, 
                   TMSLOGISTICGROUP, LFART, DELIVERYDATE, 
                   CARRIERCODE, CARRIERNAME, VEHICLENO, 
                   VEHICLETONCODE, VEHICLETONNAME, VEHICLEGROUPCODE, 
                   VEHICLEGROUPNAME, DRIVERNAME, DRIVERMOBILE, 
                   ROTATIONNUMBER, DISPATCHMEMO, REGISTERAT, 
                   REGISTERBY, PDE_YN
                FROM TMS_INPUT_PLOADM_CON
                WHERE ('{txtVEHICLENO.Text}' IS NOT NULL AND VEHICLENO = '{txtVEHICLENO.Text}')
                      AND DELIVERYDATE BETWEEN TO_CHAR(TO_DATE('{dtFromDeliveryDate.EditValue}', 'YYYY-MM-DD ""오전"" HH12:MI:SS'), 'YYYYMMDD') 
                                        AND TO_CHAR(TO_DATE('{dtToDeliveryDate.EditValue}', 'YYYY-MM-DD ""오전"" HH12:MI:SS'), 'YYYYMMDD')
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridMain, viewMain, ds.Tables[0], true);

                // 전송구분
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboTRANS_FLAG, clsCommon.GetTransFlag());

                // 차량그룹
                clsDevexpressGrid.ItemLookUpEditSetup(gridcbovehicleGroupCode, clsCommon.GetCarGroup());

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void Detail_Select(string sDISPATCHNO)
        {
            try
            {
                SQL = $@"
                SELECT 
                   'N' AS CHK, b.DISPATCHNO, b.ORDERNO, b.ORDERLINENO, 
                   b.TOLOCATIONCODE, b.FROMLOCATIONCODE, b.ORDERTYPECODE, 
                   b.SOLDTOCODE, b.DELIVERYSEQUENCE, b.ITEMCODE, b.PLANQTY
                   , c.PD_YN, c.RESOURCE_NO, c.ZERO_W, c.QTY, c.WEIGHT
                   , c.CH_YN, c.I_TIME
                FROM TMS_INPUT_PLOADM_CON a
                    INNER JOIN TMS_INPUT_PLOADD_CON b ON b.DISPATCHNO = a.DISPATCHNO
	                LEFT JOIN TMS_OUTPUT_RESULT c ON TO_CHAR(c.DISPATCHNO) = TO_CHAR(b.DISPATCHNO)
                        AND c.ORDERNO = b.ORDERNO AND c.ORDERLINENO = b.ORDERLINENO
                WHERE ('{sDISPATCHNO}' IS NULL OR a.DISPATCHNO = '{sDISPATCHNO}')
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridDetail, viewDetail, ds.Tables[0], true);

                gridCHK.ValueChecked = "Y";
                gridCHK.ValueUnchecked = "N";
                gridCHK.NullStyle = StyleIndeterminate.Unchecked;
                gridCHK.CheckStyle = CheckStyles.Standard;

                // 상품
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboITEMCODE, clsCommon.GetResource(vPlantCode));

                //출하센터
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboCUSTOMER, clsCommon.GetCustomer(), "센터명이 없습니다.", false);

                //아이템코드
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboDRESOURCE_NO, clsCommon.GetResource(vPlantCode), "센터명이 없습니다.", false);

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "Detail_Select", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        #endregion

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            SetSelectParam();
        }

        private void SetSelectParam()
        {
            string DISPATCHNO = string.Empty;
            string ORDERNO = string.Empty;
            List<string> lORDERLINENO = new List<string>();
            List<string> lRESOURCE_NO = new List<string>();

            clsDevexpressGrid.GridEndEdit(viewDetail);
            DataTable DT = (DataTable)gridDetail.DataSource;

            DataTable copyTable = DT.Clone();

            foreach (DataRow row in DT.Select("CHK='Y'"))
            {
                copyTable.ImportRow(row);
            }

            if (copyTable.Rows.Count == 0)
            {
                ShowMessageBox.XtraShowWarning("자재를 먼저 선택 해주세요.");
                return;
            }

            DISPATCHNO = copyTable.Rows[0]["DISPATCHNO"].ToString();
            ORDERNO = copyTable.Rows[0]["ORDERNO"].ToString();

            foreach (DataRow dr in copyTable.Rows)
            {
                dr.ClearErrors();

                lORDERLINENO.Add(dr["ORDERLINENO"].ToString());
                lRESOURCE_NO.Add(dr["ITEMCODE"].ToString());

                dr.AcceptChanges();

            } //foreach

            if (lRESOURCE_NO != null)
            {
                vDISPATCHNO = DISPATCHNO;
                vORDERNO = ORDERNO;
                vORDERLINENO = lORDERLINENO.ToArray();
                vRESOURCE_NO = lRESOURCE_NO.ToArray();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void viewMain_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
                try
                {
                    string sDISPATCHNO = clsDevexpressGrid.GetFocusedRowCellValue(viewMain, "DISPATCHNO");

                    Detail_Select(sDISPATCHNO);
                }
                catch (Exception ex)
                {
                    clsLog.logSave(this, "gridView_product_FocusedRowChanged", ex);
                }
        }

        private void viewDetail_MouseDown(object sender, MouseEventArgs e)
        {
            GridHitInfo hitInfo = viewDetail.CalcHitInfo(e.Location);

            // 헤더 셀 클릭 감지
            if (hitInfo.InColumn && hitInfo.RowHandle == GridControl.InvalidRowHandle && hitInfo.Column.FieldName == "CHK")
            {
                for (int i = 0; i < viewDetail.RowCount; i++)
                {
                    viewDetail.SetRowCellValue(i, "CHK", viewDetail.GetRowCellValue(i, "CHK").ToString() == "N" ? "Y" : "N");
                }
            }
        }

        private void dtFromDeliveryDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void dtToDeliveryDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void txtVEHICLENO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                XMain_Search();
            }
        }
    }
}