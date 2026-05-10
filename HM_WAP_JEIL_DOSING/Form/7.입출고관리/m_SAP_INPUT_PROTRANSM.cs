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
    public partial class m_SAP_INPUT_PROTRANSM : DevExpress.XtraEditors.XtraForm
    {
        public string vTKNUM { get; set; }    // 발주번호

        private string vPlantCode = string.Empty;
        public string[] SelectedValue { get; set; }
        private string SQL = string.Empty;

        public m_SAP_INPUT_PROTRANSM(string sPlantCode)
        {
            vPlantCode = sPlantCode;

            InitializeComponent();
            clsDevexpressGrid.ReadGridViewInit(viewMain, Properties.Settings.Default.FontSize);
        }

 
        private void m_EBELP_Load(object sender, EventArgs e)
        {
            try
            {
                dtDate.EditValue = DateTime.Now;  
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
                TKNUM, TRAID, WERKS_GI, 
                   WERKS_GR, TRANS_DATE, b.ERP_UP_YN
                FROM SAP_INPUT_PROTRANSM a
                    LEFT JOIN TMS_OUTPUT_RESULT b ON TO_CHAR(b.DISPATCHNO) = TO_CHAR(a.TKNUM)
                WHERE TRANS_DATE = '{string.Format("{0:yyyyMMdd}", dtDate.EditValue)}'
                    AND WERKS_GR = '{vPlantCode}'
                    AND NVL(b.ERP_UP_YN, 'X') != 'Y'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridMain, viewMain, ds.Tables[0], true);

                // 플랜트
                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboPLANT, clsCommon.GetPlant("", true));

                // 플랜트
                clsDevexpressGrid.ItemLookUpEditSetup(gridcboTransFlag, clsCommon.GetTransFlag());

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

        private void dtDate_TextChanged(object sender, EventArgs e)
        {
            XMain_Search();
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
                vTKNUM = viewMain.GetFocusedRowCellValue("TKNUM").ToString();
            }

            if (row != null && vTKNUM != "")
            {
                arrResource.Add(vTKNUM);
                SelectedValue = arrResource.ToArray(); // 전달할 값 지정
                this.DialogResult = DialogResult.OK;            // 부모에 결과 알림
                this.Close();
            }
        }

        private void dtDate_EditValueChanged(object sender, EventArgs e)
        {
            XMain_Search();
        }
    }
}