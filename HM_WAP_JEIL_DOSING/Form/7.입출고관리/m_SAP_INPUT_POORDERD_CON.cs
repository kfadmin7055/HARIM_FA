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
    public partial class m_SAP_INPUT_POORDERD_CON : DevExpress.XtraEditors.XtraForm
    {
        private string vEBELN { get; set; }    // 발주번호
        public string vEBELP { get; set; }      // 발주항번
        public string vResourceNo { get; set; }      // 품목
        public string vPlantCode { get; set; }      // 품목

        public string[] SelectedValue { get; set; }
        private string SQL = String.Empty;

        public m_SAP_INPUT_POORDERD_CON(string sPlantCode, string sEBELN)
        {
            InitializeComponent();

            vPlantCode = sPlantCode;
            vEBELN = sEBELN;

            clsDevexpressGrid.ReadGridViewInit(viewPoorD, Properties.Settings.Default.FontSize);
        }

 
        private void m_EBELP_Load(object sender, EventArgs e)
        {
            try
            {
                XDetail_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "m_EBELP_Load", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void XDetail_Search()
        {
            SQL = $@"
            SELECT 
                  EBELN         -- 발주번호 / 구매오더 번호
                , EBELP         -- 발주 항번
                , MATNR         -- 자재번호
                , TXZ01         -- 자재명 또는 단가설명
                , WERKS         -- 플랜트 코드
                , CHARG         -- 모선코드
                , CHARG_TEXT    -- 모선명
                , RESLO         -- 출고 창고 부두 창고
                , LOGBE_ISSUU   -- 출고 창고명
                , LGORT         -- 입고 창고
                , LGOBE_RECV    -- 입고 창고명
                , MENGE         -- 주문 수량
                , MEINS         -- 주문 단위
                , ELIKZ         -- 오더 클로징 여부
                , LOEKZ         -- 삭제여부
                , EINDT         -- 납품예정일
            FROM SAP_INPUT_POORDERD_CON a
            WHERE EBELN = '{vEBELN}'
            ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);
            clsDevexpressGrid.BindGridControl(gridPoorD, viewPoorD, ds.Tables[0], false, true);

            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridPDscboRESOURCE_NO, clsCommon.GetResource(vPlantCode));

            clsDevexpressGrid.ItemSearchLookUpEditSetup(gridPDscboPLANT, clsCommon.GetPlant("", true));

            clsDevexpressGrid.ItemLookUpEditSetup(gridcboUNIT, clsCommon.GetUnit(), "", false, false);

            clsDevexpressGrid.ItemLookUpEditSetup(gridcboYN, clsCommon.GetYn(), "", false, false);
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void dtDate_TextChanged(object sender, EventArgs e)
        {
            XDetail_Search();
        }

        private void viewPoorD_DoubleClick(object sender, EventArgs e)
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
            var row = viewPoorD.GetFocusedDataRow();

            if (viewPoorD.RowCount > 0)
            {
                vEBELP = viewPoorD.GetFocusedRowCellValue("EBELP").ToString();
                vResourceNo = viewPoorD.GetFocusedRowCellValue("MATNR").ToString();
            }

            if (row != null && vEBELP != "")
            {
                arrResource.Add(vEBELP);
                arrResource.Add(vResourceNo);
                SelectedValue = arrResource.ToArray(); // 전달할 값 지정
                this.DialogResult = DialogResult.OK;            // 부모에 결과 알림
                this.Close();
            }
        }

        
    }
}