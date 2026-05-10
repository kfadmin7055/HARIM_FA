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
using DevExpress.DataAccess.Native.Excel;
using DevExpress.XtraTreeList.Design;

namespace HARIM_FA_DOSING
{
    public partial class m_Wap_Decar : DevExpress.XtraEditors.XtraForm
    {
        public string   vIS_NO  { get; set; }    // 발급번ㄴ호
        public string   vEBELN { get; set; }    // 발주번호
        private string vPlantCode {  get; set; }

        public string[] SelectedValue { get; set; }
        private string SQL = String.Empty;
        DataSet argDataSet;
        string resourceNo = string.Empty;
        string suGubun = string.Empty;

        private string sIS_NO = string.Empty;

        public m_Wap_Decar(string vPlantCode)
        {
            InitializeComponent();
            clsDevexpressGrid.ReadGridViewInit(viewMain, Properties.Settings.Default.FontSize);
            clsDevexpressGrid.EditGridViewInit(viewDetail, Properties.Settings.Default.FontSize);

            dtFromDeliveryDate.EditValue = DateTime.Today.AddDays(-1);
            dtToDeliveryDate.EditValue = DateTime.Today;
            this.vPlantCode = vPlantCode;
        }

        private void m_Wap_Decar_Load(object sender, EventArgs e)
        {
                XMain_Search();
        }

        #region 조회함수
        private void XMain_Search()
        {
            try
            {
                DateTime dtFrom = dtFromDeliveryDate.DateTime.Date;
                DateTime dtTo = dtToDeliveryDate.DateTime.Date;

                SQL = $@"
                SELECT 
                     IS_NO              -- 발급번호
                   , CAR_TYPE           -- 차량입고타입
                   , INCAR_NO           -- 차량전체번호
                   , VEHICLEGROUPCODE   -- 차량그룹코드
                   , WEIGHT_KG          -- 계근번호
                   , IN_WEIGHT          -- 입차중량
                   , OUT_WEIGHT         -- 출차중량
                   , TR_YN              -- 트레일러유무
                   , TR_WEIGHT          -- 트레일러무게
                   , USER_ID            -- 확인관리자
                   , INCAR_DATE         -- 입차일시
                   , OUTCAR_DATE        -- 출차일시
                   , PC_STATUS          -- 진행상태
                   , ERP_UP_YN          -- ERP 전송상태 
                   , ERP_TNUMBER        -- ERP 전송일련번호
                   , DEL_FLAG           -- 삭제여부
                   , TEM_TYPE           -- 수동여부
                   , PRINT_YN           -- 프린터 여부
                   , I_TIME             -- 입력시간
                   , I_USER             -- 입력자
                FROM WAP_DECAR
                WHERE CAR_TYPE = '{clsCommon.GetCarInputTypeCode("제품출고")}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridMain, viewMain, ds.Tables[0], true);

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboCAR_TYPE, clsCommon.GetCarInputType());

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboPC_STATUS, clsCommon.GetCarStatus());

                clsDevexpressGrid.ItemLookUpEditSetup(gridcboYN, clsCommon.GetYn(), "", false, false);

                //clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboINCAR_NO, clsCommon.GetCarMaster());

                Dictionary<string, string> parameterDict = new Dictionary<string, string>
                {
                    { "TYPE", "운송사" }
                };

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboINCAR_NO, clsCommon.GetCarMaster(), "", true, parameterDict);

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }

        }

        private void xDetail_Select()
        {
            try
            {
                SQL = $@"
                SELECT 
                     RT_TYPE         -- 실적유형
                   , IS_NO           -- 발급번호
                   , DISPATCHNO      -- 배차번호
                   , ORDERNO         -- 주문번호
                   , ORDERLINENO     -- 라인번호
                   , PD_YN           -- 상차확인여부
                   , RESOURCE_NO     -- 품목코드
                   , ZERO_W          -- 공차중량
                   , QTY             -- 상차수량
                   , WEIGHT          -- 상차중량
                   , CH_YN           -- 확인일자
                   , I_TIME          -- 계근일자
                   , PLANT_CODE      -- 플랜트
                FROM TMS_OUTPUT_RESULT
                WHERE IS_NO = '{vIS_NO}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridDetail, viewDetail, ds.Tables[0], true);

                clsDevexpressGrid.ItemLookUpEditSetup(gridOutcboYN, clsCommon.GetYn(), "", false, false);

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridOutscboRESOURCE_NO, clsCommon.GetResource(vPlantCode));

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "xDetail_Select", ex);
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

        private void viewMain_DoubleClick(object sender, EventArgs e)
        {
            SetSelectParam();
        }

        private void SetSelectParam()
        {
            List<string> arrResource = new List<string>();
            var row = viewMain.GetFocusedDataRow();

            if (viewMain.RowCount > 0)
            {
                vIS_NO = viewMain.GetFocusedRowCellValue("IS_NO").ToString();
                vEBELN = viewDetail.GetRowCellValue(0, "DISPATCHNO").ToString();
            }

            if (row != null)
            {
                SelectedValue = arrResource.ToArray(); // 전달할 값 지정
                this.DialogResult = DialogResult.OK;            // 부모에 결과 알림
                this.Close();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            XMain_Search();
        }

        private void viewMain_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0 && e.FocusedRowHandle < viewMain.RowCount)
            {
                try
                {
                    vIS_NO = clsDevexpressGrid.GetFocusedRowCellValue(viewMain, "IS_NO");

                    xDetail_Select();
                }
                catch (Exception ex)
                {
                    clsLog.logSave(this, "gridView_product_FocusedRowChanged", ex);
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
            if (e.KeyCode == Keys.Escape)
            {
                XMain_Search();
            }
        }
    }
}