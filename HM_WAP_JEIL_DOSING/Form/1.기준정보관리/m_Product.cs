using Core.Class;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraScheduler.Printing;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class m_Product : DevExpress.XtraEditors.XtraForm
    {
        public string[] SelectedValue { get; set; }
        public string[] SelectedUom { get; set; }
        private string SQL = String.Empty;
        DataSet argDataSet;
        string vResourceNo = string.Empty;
        string vResourceType = string.Empty;
        string vUOM = string.Empty;
        string suGubun = string.Empty;
        string vPlantCode = string.Empty;

        public m_Product(string sPlantCode, string resourceType = "", string sUOM = "")
        {
            vPlantCode = sPlantCode;
            vResourceType = resourceType == "" ? "''" : resourceType;
            vUOM = sUOM;
            InitializeComponent();
            clsDevexpressGrid.ReadGridViewInit(viewProduct, Properties.Settings.Default.FontSize);
        }

        private void m_selOrder_Load(object sender, EventArgs e)
        {
            try
            {
                clsDevexpressUtil.ItemLookUpEditSetup(cboSU, clsCommon.GetEtcResourceType(), "", false, 0, true);

                XMain_Search();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "m_selOrder_Load", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }

            // Application.AddMessageFilter(new GridMouseWheelFilter(gridProduct));
        }

        #region 조회함수
        private void XMain_Search()
        {
            try
            {
                SQL = $@"
                SELECT DISTINCT
                     a.PLANT_CODE, a.RESOURCE_NO, a.DESCRIPTION, a.UOM
                   , a.RESOURCE_TYPE, a.RESOURCE_TYPE_DESC
                   , a.ZSPEC, a.P_TYPE, a.STD_LOT_SIZE
                   , a.PACK_SIZE, a.ZDOMESTIC_IMPORT, a.LVORM
                   , a.HAND_YN, a.I_TIME, a.SU_CODE, a.PLANNER, a.SOURCE_DESC
                   , a.HOME_LOCATION, a.COST_CENTER1, a.COST_CENTER3
                   , a.COST_DIVISION, a.POU, a.BUYER
                   , a.MANF_ENGR, a.INSPECTOR, a.MAT_MSTR
                   , a.ITEM_GROUP_V, a.ITEM_GROUP_W, a.AP_CLOSE_TYPE
                   , CASE WHEN b.RESOURCE_NO IS NULL THEN 'N' ELSE 'Y' END CON
                   , CASE WHEN c.RESOURCE_NO IS NULL THEN 'N' ELSE 'Y' END RC
                FROM SAP_DI_PRODUCT a
                    LEFT JOIN SAP_IN_PRODUCT_CH b ON b.PLANT_CODE = a.PLANT_CODE AND b.RESOURCE_NO = a.RESOURCE_NO
                    LEFT JOIN SAP_IN_PRODUCT_RC c ON c.PLANT_CODE = a.PLANT_CODE AND c.RESOURCE_NO = a.RESOURCE_NO
                WHERE a.PLANT_CODE = '{vPlantCode}'
                    AND (a.DESCRIPTION LIKE '%{txtRESOURCE.EditValue}%' OR a.RESOURCE_NO LIKE '%{txtRESOURCE.EditValue}%')
                    AND ({vResourceType.Substring(0, vResourceType.Contains(",") ? vResourceType.IndexOf(",") : vResourceType.Length)} IS NULL OR a.RESOURCE_TYPE IN ({vResourceType}))
                    AND  ('{vUOM}' IS NULL OR a.UOM = '{vUOM}')
                    {suGubun}
                    --AND a.JD_GUBUN IN ('', 'E')
                ORDER BY a.PLANT_CODE, a.RESOURCE_TYPE, a.DESCRIPTION
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridProduct, viewProduct, ds.Tables[0], true);


                ds.Dispose();

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboPlant, clsCommon.GetPlant("", true));

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridProdscboRESOURCE_NO, clsCommon.GetResource(vPlantCode, "", "", "", 0, true));

                // 제품유형
                repoLookup_TYPE.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                clsDevexpressGrid.ItemLookUpEditSetup(repoLookup_TYPE, clsCommon.GetResourceType(), "", false, false);

                // 단위
                repoCombo_Unit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                clsDevexpressGrid.ItemLookUpEditSetup(repoCombo_Unit, clsCommon.GetUnit(), "", false, false);

                // 저장위치
                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboHOME_LOCATION, clsCommon.GetLocation(vPlantCode));

                // 사용여부
                repItemLkUpEdit_USEYN.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                clsDevexpressGrid.ItemLookUpEditSetup(repItemLkUpEdit_USEYN, clsCommon.GetYn());

                XPartSearch(vPlantCode, vResourceNo);
                XResSearch(vPlantCode, vResourceNo);
                XCPSearch(vPlantCode, vResourceNo);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XMain_Search", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void XPartSearch(string sPlantCode, string vResourceNo)
        {
            try
            {
                SQL = $@"
                SELECT PLANT_CODE, RESOURCE_NO, RESOURCE_NO_2, PART_P, 
                   EMPLOYEE_NO, I_TIME
                FROM SAP_IN_PRODUCT_CH
                WHERE PLANT_CODE = '{sPlantCode}' AND RESOURCE_NO = '{vResourceNo}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridCH, viewCH, ds.Tables[0], true);


                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridscboRESOURCE_NO, clsCommon.GetResource(vPlantCode), "품목을 선택 해주세요.");

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XPartSearch", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void XResSearch(string sPlantCode, string vResourceNo)
        {
            try
            {
                SQL = $@"
                SELECT PLANT_CODE,
                RESOURCE_NO, RESOURCE_NO_2, PART_P, 
                   EMPLOYEE_NO, I_TIME
                FROM SAP_IN_PRODUCT_RC
                WHERE PLANT_CODE = '{sPlantCode}' AND RESOURCE_NO = '{vResourceNo}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridRC, viewRC, ds.Tables[0], true);

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridRCcboRESOURCE, clsCommon.GetResource(vPlantCode), "품목을 선택 해주세요.");

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XResSearch", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        private void XCPSearch(string sPlantCode, string vResourceNo)
        {
            try
            {
                SQL = $@"
                SELECT 
                PLANT_CODE, RESOURCE_NO, RESOURCE_NO_2, 
                   RESOURCE_NO_3, PART_P, EMPLOYEE_NO, 
                   I_TIME
                FROM SAP_IN_PRODUCT_CP
                WHERE PLANT_CODE = '{sPlantCode}' AND RESOURCE_NO = '{vResourceNo}'
                ";

                DataSet ds = Dbconn.conn.ExecutDataset(SQL);
                clsDevexpressGrid.BindGridControl(gridCP, viewCP, ds.Tables[0], true);

                clsDevexpressGrid.ItemSearchLookUpEditSetup(gridCPScboResourceNo, clsCommon.GetResource(vPlantCode));

                ds.Dispose();
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "XResSearch", ex);
                ShowMessageBox.XtraShowWarning("조회를 실행하는 도중 에러가 발생했습니다");
            }
        }

        #endregion

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void viewProduct_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                object val = clsDevexpressGrid.GetFocusedRowCellValue(viewProduct, "PLANT_CODE");

                if (val != null && val != DBNull.Value && !string.IsNullOrWhiteSpace(val.ToString()))
                {
                    vPlantCode = val.ToString();
                }

                val = clsDevexpressGrid.GetFocusedRowCellValue(viewProduct, "RESOURCE_NO");

                if (val != null && val != DBNull.Value && !string.IsNullOrWhiteSpace(val.ToString()))
                {
                    vResourceNo = val.ToString();
                }

                XPartSearch(vPlantCode, vResourceNo);
                XResSearch(vPlantCode, vResourceNo);
                XCPSearch(vPlantCode, vResourceNo);
            }
            catch (Exception ex)
            {
                clsLog.logSave(this, "gridView_product_FocusedRowChanged", ex);
            }
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            SetSelectParam();
        }

        private void viewProduct_DoubleClick(object sender, EventArgs e)
        {
            SetSelectParam();
        }

        private void SetSelectParam()
        {
            List<string> arrResource = new List<string>();
            var row = viewProduct.GetFocusedDataRow();

            if (vResourceType == "")
            {
                if (viewRC.RowCount > 0)
                {
                    for (int i = 0; i < viewRC.RowCount; i++)
                    {
                        arrResource.Add(viewRC.GetRowCellValue(i, "RESOURCE_NO_2").ToString());
                    }
                }

                if (viewCP.RowCount > 0)
                {
                    for (int i = 0; i < viewCP.RowCount; i++)
                    {
                        arrResource.Add(viewCP.GetRowCellValue(i, "RESOURCE_NO").ToString());
                    }
                }
            }

            if (row != null)
            {
                if (arrResource.Count == 0)
                {
                    arrResource.Add(viewProduct.GetFocusedRowCellValue("RESOURCE_NO").ToString());
                    arrResource.Add(viewProduct.GetFocusedRowCellValue("UOM").ToString());
                }

                SelectedValue = arrResource.ToArray(); // 전달할 값 지정
                this.DialogResult = DialogResult.OK;            // 부모에 결과 알림
                this.Close();
            }
        }

        private void cboSU_EditValueChanged(object sender, EventArgs e)
        {
            if (cboSU.EditValue?.ToString() == clsCommon.GetEtcResourceTypeCode("제품대체"))
            {
                suGubun = @"AND (a.RESOURCE_NO IN (SELECT RESOURCE_NO_2 FROM SAP_IN_PRODUCT_CP)
                        OR a.RESOURCE_NO IN (SELECT RESOURCE_NO FROM SAP_IN_PRODUCT_CP))";
            }
            else if (cboSU.EditValue?.ToString() == clsCommon.GetEtcResourceTypeCode("원료대체"))
            {
                suGubun = @"AND (a.RESOURCE_NO IN (SELECT RESOURCE_NO_2 FROM SAP_IN_PRODUCT_CH)
                        OR a.RESOURCE_NO IN (SELECT RESOURCE_NO FROM SAP_IN_PRODUCT_CH))";
            }
            else if (cboSU.EditValue?.ToString() == clsCommon.GetEtcResourceTypeCode("부산물"))
            {
                suGubun = @"AND (a.RESOURCE_NO IN (SELECT RESOURCE_NO_2 FROM SAP_IN_PRODUCT_RC)
                        OR a.RESOURCE_NO IN (SELECT RESOURCE_NO FROM SAP_IN_PRODUCT_RC))";
            }

            XMain_Search();
        }

        private void txtRESOURCE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // 엔터 입력 시 동작
                XMain_Search();
                e.Handled = true;
                e.SuppressKeyPress = true; // 삑 소리 방지
            }
        }
    }
}