using Core.Class;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
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
    public partial class m_TMS_INPUT_PLOADD_CON : DevExpress.XtraEditors.XtraForm
    {
        private string vsDISPATCHNO { get; set; }    // 발주번호
        public string vORDERNO { get; set; }        // 발주번호
        public string vResourceNo { get; set; }     // 품목
        public string vORDERLINENO { get; set; }    // 오더라인
        private string vPlantCode { get; set; }

        public string[] SelectedValue { get; set; }
        private string SQL = String.Empty;

        public m_TMS_INPUT_PLOADD_CON(string sPlantCode, string sDISPATCHNO)
        {
            InitializeComponent();

            vsDISPATCHNO = sDISPATCHNO;
            vPlantCode = sPlantCode;
            clsDevexpressGrid.ReadGridViewInit(viewDetail, Properties.Settings.Default.FontSize);
        }

 
        private void m_EBELP_Load(object sender, EventArgs e)
        {
            try
            {
                Detail_Select();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "m_EBELP_Load", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void Detail_Select()
        {
            try
            {
                SQL = $@"
                SELECT 
                     b.DISPATCHNO         -- 배차번호
                   , b.ORDERNO            -- 주문번호
                   , b.ORDERLINENO        -- 주문라인번호
                   , b.TOLOCATIONCODE     -- 도착지 코드
                   , b.FROMLOCATIONCODE   -- 출발지 코드
                   , b.ORDERTYPECODE      -- 주문유형 코드
                   , b.SOLDTOCODE         -- 납품처 코드
                   , b.DELIVERYSEQUENCE   -- 납품순번
                   , b.ITEMCODE           -- 품목코드
                   , b.PLANQTY            -- 계획수량
                   , c.PD_YN              -- 상차확인 여부
                   , c.QTY                -- 상차수량
                   , c.WEIGHT             -- 상차중량
                FROM TMS_INPUT_PLOADM_CON a
                    INNER JOIN TMS_INPUT_PLOADD_CON b ON b.DISPATCHNO = a.DISPATCHNO
	                LEFT JOIN TMS_OUTPUT_RESULT c ON TO_CHAR(c.DISPATCHNO) = TO_CHAR(b.DISPATCHNO)
                        AND c.ORDERNO = b.ORDERNO AND c.ORDERLINENO = b.ORDERLINENO
                WHERE a.DISPATCHNO = '{vsDISPATCHNO}'
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

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void dtDate_TextChanged(object sender, EventArgs e)
        {
            Detail_Select();
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
                vORDERNO = viewDetail.GetFocusedRowCellValue("ORDERNO").ToString();
                vResourceNo = viewDetail.GetFocusedRowCellValue("ITEMCODE").ToString();
                vORDERLINENO = viewDetail.GetFocusedRowCellValue("ORDERLINENO").ToString();
            }

            if (row != null && vORDERNO != "")
            {
                arrResource.Add(vORDERNO);
                SelectedValue = arrResource.ToArray(); // 전달할 값 지정
                this.DialogResult = DialogResult.OK;            // 부모에 결과 알림
                this.Close();
            }
        }

        
    }
}