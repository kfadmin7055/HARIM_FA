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
    public partial class m_SAP_INPUT_PROTRANSD : DevExpress.XtraEditors.XtraForm
    {
        private string vTKNUM { get; set; }  // 발주번호
        public string vVBELN { get; set; }  // 출고지시서
        public string vPOSNR { get; set; }  // 출고지시항번
        public string vResourceNo { get; set; } // 품목
        public string vPlantCode { get; set; } // 품목

        public string[] SelectedValue { get; set; }
        private string SQL = String.Empty;

        public m_SAP_INPUT_PROTRANSD(string sPlantCode, string sTKNUM)
        {
            InitializeComponent();
            clsDevexpressGrid.ReadGridViewInit(viewDetail, Properties.Settings.Default.FontSize);

            vTKNUM = sTKNUM;
            vPlantCode = sPlantCode;
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
                SELECT 
                     TKNUM   -- 01. 배차번호
                   , VBELN   -- 02. 출고지시서
                   , POSNR   -- 03. 출고지시 항번
                   , MATNR   -- 04. 품목코드
                   , LFIMG   -- 05. 입고예정수량
                   , VRKME   -- 06. 단위
                FROM SAP_INPUT_PROTRANSD
                WHERE TKNUM = '{vTKNUM}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridDetail, viewDetail, ds.Tables[0], true);

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridDcboScboResourceNo, clsCommon.GetResource(vPlantCode));

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

        private void viewDetail_DoubleClick(object sender, EventArgs e)
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
            var row = viewDetail.GetFocusedDataRow();

            if (viewDetail.RowCount > 0)
            {
                vVBELN = viewDetail.GetFocusedRowCellValue("VBELN").ToString();
                vPOSNR = viewDetail.GetFocusedRowCellValue("POSNR").ToString();
                vResourceNo = viewDetail.GetFocusedRowCellValue("MATNR").ToString();
            }

            if (row != null && vVBELN != "")
            {
                arrResource.Add(vVBELN);
                SelectedValue = arrResource.ToArray(); // 전달할 값 지정
                this.DialogResult = DialogResult.OK;            // 부모에 결과 알림
                this.Close();
            }
        }
    }
}