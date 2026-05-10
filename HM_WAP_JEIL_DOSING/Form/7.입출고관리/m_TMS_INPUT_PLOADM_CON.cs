using Core.Class;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class m_TMS_INPUT_PLOADM_CON : DevExpress.XtraEditors.XtraForm
    {
        public string vDISPATCHNO { get; set; }    // 발주번호

        public string[] SelectedValue { get; set; }
        private string SQL = String.Empty;

        public m_TMS_INPUT_PLOADM_CON(object dtFromDate, object dtToDate, string sVEHICLECODE)
        {
            InitializeComponent();
            clsDevexpressGrid.ReadGridViewInit(viewMain, Properties.Settings.Default.FontSize);

            txtVEHICLENO.Text = sVEHICLECODE;

            dtFromDeliveryDate.EditValue = dtFromDate;
            dtToDeliveryDate.EditValue = dtToDate;
        }


        private void m_EBELP_Load(object sender, EventArgs e)
        {
            try
            {
                XMain_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "m_EBELP_Load", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                SELECT DISTINCT
                     a.DISPATCHNO                 --  배차번호
                   , a.VEHICLECODE                -- 차량코드
                   , a.TMSDIVISIONCODE            -- 디비전코드
                   , a.TMSLOGISTICGROUP           -- 물류운영그룹코드
                   , a.LFART                      -- 납품유형
                   , a.DELIVERYDATE               -- 배송일자
                   , a.CARRIERCODE                -- 운송사코드
                   , a.CARRIERNAME                -- 운송사명
                   , a.VEHICLENO                  -- 차량번호
                   , a.VEHICLETONCODE             -- 차량톤급
                   , a.VEHICLETONNAME             -- 차량톤급명칭
                   , a.VEHICLEGROUPCODE           -- 차량그룹코드
                   , a.VEHICLEGROUPNAME           -- 차량그룹명칭
                   , a.DRIVERNAME                 -- 기사이름
                   , a.DRIVERMOBILE               -- 휴대폰
                   , a.ROTATIONNUMBER             -- 회전수
                   , a.DISPATCHMEMO               -- 배차메모사항
                   , a.REGISTERAT                            
                   , a.REGISTERBY
                   , a.PDE_YN                     -- 상차마감여부
                   , a.ERP_UP_YN
                   , a.ERP_TNUMBER
                   , a.TMS_UP_YN
                   , a.TMS_TNUMBER
                FROM TMS_INPUT_PLOADM_CON a
                    INNER JOIN TMS_INPUT_PLOADD_CON b ON b.DISPATCHNO = a.DISPATCHNO
                    INNER JOIN SAP_INPUT_SHIP_ORDERM_CON c ON c.VBELN = b.ORDERNO AND c.ZTM_CRE_FLAG = b.ORDERLINENO
                WHERE NVL(a.PDE_YN, 'X') != 'Y'
                    AND ('{txtVEHICLENO.Text}' IS NOT NULL AND a.VEHICLENO = '{txtVEHICLENO.Text}')
                    OR ('{txtVEHICLENO.Text}' IS NULL AND a.DELIVERYDATE BETWEEN TO_CHAR(TO_DATE('{dtFromDeliveryDate.EditValue}', 'YYYY-MM-DD ""오전"" HH12:MI:SS'), 'YYYYMMDD') 
                                        AND TO_CHAR(TO_DATE('{dtToDeliveryDate.EditValue}', 'YYYY-MM-DD ""오전"" HH12:MI:SS'), 'YYYYMMDD'))
                ORDER BY DECODE(a.ERP_UP_YN, 'N', 1, 'M', 2, 'C', 3, 'F', 4, 'U', 5, 'D', 6, '', 7, 8)
                        , DECODE(a.TMS_UP_YN, 'N', 1, 'M', 2, 'C', 3, 'F', 4, 'U', 5, 'D', 6, '', 7, 8)
                        ASC, a.DISPATCHNO DESC, a.DELIVERYDATE DESC
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridMain, viewMain, ds.Tables[0], true);

                // 상차 마감여부
                clsDevexpressGrid.ItemLookUpEditSetup(gridMcboPDEYN, clsCommon.GetPDYn(), "", false, false);

                // 납품유형
                clsDevexpressGrid.ItemLookUpEditSetup(gridCboLFART, clsCommon.GetLFART());

                // 차량톤급
                clsDevexpressGrid.ItemLookUpEditSetup(gridCboVEHICLETON, clsCommon.GetVEHICLETON(), "", false, false);

                // 차량그룹
                clsDevexpressGrid.ItemLookUpEditSetup(gridCboVEHICLEGROUP, clsCommon.GetVEHICLEGROUP(), "", false, false);

                // SAP 전송여부
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboTRANS_FLAG, clsCommon.GetTransFlag());
                // 차량그룹
                clsDevexpressGrid.ItemLookUpEditSetup(gridCboVEHICLEGROUP, clsCommon.GetCarGroup());

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void dtFromDeliveryDate_TextChanged(object sender, EventArgs e)
        {
            if (DateCheck()) XMain_Search();
        }

        private void dtToDeliveryDate_EditValueChanged(object sender, EventArgs e)
        {

        }

        private bool DateCheck()
        {
            bool isCheck = true;
            if (dtFromDeliveryDate.EditValue == null || dtToDeliveryDate.EditValue == null)
            {
                isCheck = false;
            }

            DateTime fromDate = Convert.ToDateTime(dtFromDeliveryDate.EditValue);
            DateTime toDate = Convert.ToDateTime(dtToDeliveryDate.EditValue);

            if (fromDate > toDate)
            {
                isCheck = false;
            }

            return isCheck;
        }

        private void viewMain_DoubleClick(object sender, EventArgs e)
        {
            SetSelectParam();
        }

        private void btnInput_Click(object sender, EventArgs e)
        {
            SetSelectParam();
        }

        private void SetSelectParam()
        {
            List<string> arrResource = new List<string>();
            var row = viewMain.GetFocusedDataRow();

            if (viewMain.RowCount > 0)
            {
                vDISPATCHNO = viewMain.GetFocusedRowCellValue("DISPATCHNO").ToString();
            }

            if (row != null && vDISPATCHNO != "")
            {
                arrResource.Add(vDISPATCHNO);
                SelectedValue = arrResource.ToArray(); // 전달할 값 지정
                this.DialogResult = DialogResult.OK;            // 부모에 결과 알림
                this.Close();
            }
        }

        private void txtVEHICLENO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                XMain_Search();
            }
        }

        private void dtFromDeliveryDate_EditValueChanged(object sender, EventArgs e)
        {
            //XMain_Search();
        }
    }
}