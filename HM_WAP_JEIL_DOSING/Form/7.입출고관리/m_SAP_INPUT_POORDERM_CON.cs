using Core.Class;
using DevExpress.CodeParser;
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
    public partial class m_SAP_INPUT_POORDERM_CON : DevExpress.XtraEditors.XtraForm
    {
        public string vISNO { get; set; }    // 발주번호
        public string vEBELN { get; set; }    // 발주번호
        public string vBSART { get; set; }

        public string[] SelectedValue { get; set; }
        private string SQL = String.Empty;

        public m_SAP_INPUT_POORDERM_CON(string sISNO)
        {
            vISNO = sISNO;
            InitializeComponent();
            clsDevexpressGrid.ReadGridViewInit(viewPoor, Properties.Settings.Default.FontSize);
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
                SELECT 
                      a.EBELN     -- 발주번호
                    , a.BSART     -- PO구분
                    , a.LIFNR     -- 내외자구분
                    , a.NAME1     -- 부두업체명
                    , a.BEDAT     -- 발주일자
                    , a.LIFN2     -- 운송업체
                    , a.NAME2     -- 운송업체명
                    , a.MTYPE     -- 메세지유형
                    , a.MESSAGE   -- 메시지
                FROM SAP_INPUT_POORDERM_CON a
                    JOIN SAP_INPUT_POORDERD_CON b ON b.EBELN = a.EBELN
                WHERE (b.EINDT = '{((DateTime)dtDate.EditValue).ToString("yyyyMMdd")}'
                        OR b.EINDT = '{((DateTime)dtDate.EditValue).ToString("yyyy-MM-dd")}')
                    AND b.EBELN NOT IN (SELECT a.EBELN FROM WAP_GOCAR a
                                        INNER JOIN WAP_GOCARD b ON b.EBELN = a.EBELN AND b.IS_NO = a.IS_NO
                                        INNER JOIN WAP_DECAR c ON c.IS_NO = a.IS_NO AND c.ERP_UP_YN IN ('Y', 'D', 'R')
                                        UNION ALL 
                                        SELECT EBELN FROM WAP_GOCAR WHERE IS_NO = '{vISNO}')
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridPoor, viewPoor, ds.Tables[0], false, true);

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboLIFNR, clsCommon.GetInOutType(), "", false, false);

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboMTYPE, clsCommon.GetMSGType(), "", false, false);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "pack_search", ex);
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

        private void viewPoor_DoubleClick(object sender, EventArgs e)
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
            var row = viewPoor.GetFocusedDataRow();

            if (viewPoor.RowCount > 0)
            {
                vEBELN = viewPoor.GetFocusedRowCellValue("EBELN").ToString();
                vBSART = viewPoor.GetFocusedRowCellValue("BSART").ToString();
            }

            if (row != null && vEBELN != "")
            {
                arrResource.Add(vEBELN);
                arrResource.Add(vBSART);
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