using Core.Class;
using DevExpress.CodeParser.CSharp;
using System;
using System.Data;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class m_packEnd : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;

        string argPlantCode = string.Empty;
        string argProcessKey = string.Empty;
        string argLCode = string.Empty;
        string argWorkNumb = string.Empty;
        string argWorkSeq = string.Empty;
        string argResourceNo = string.Empty;

        public bool IsAuto { get { return chkAuto.Checked; } }

        public m_packEnd(string plantCode, string process_key, string lCode, string work_numb, string work_seq, string resourceNo)
        {
            argPlantCode = plantCode;
            argProcessKey = process_key;
            argLCode = lCode;
            argWorkNumb = work_numb;
            argWorkSeq = work_seq;
            argResourceNo = resourceNo;

            InitializeComponent();
        }

        private void m_packEnd_Load(object sender, EventArgs e)
        {
            txt_workdate.Text = argWorkNumb;
            txt_workseq.Text = argWorkSeq;

            dateEdit_workStart.EditValue = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            dateEdit_workEnd.EditValue = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            InitControl();

            txt_proqty.Focus();
        }

        private void InitControl()
        {
            SQL = $@"
            SELECT ERP_ING.DESCRIPTION
            FROM PACK_ORDER PACK
                LEFT OUTER JOIN SAP_DI_PRODUCT ERP_ING ON PACK.RESOURCE_NO = ERP_ING.RESOURCE_NO
            WHERE PACK.PLANT_CODE = '{argPlantCode}'
                AND PACK.PROCESS_KEY = '{argProcessKey}' 
                AND PACK.L_CODE = '{argLCode}'   
                AND PACK.WORKDATE = '{argWorkNumb}' AND PACK.WORK_SEQ = '{argWorkSeq}'
            ";

            using (DataSet resDs = Dbconn.conn.ExecutDataset(SQL))
            {
                txt_resName.Text = Dbconn.conn.getData(resDs, "DESCRIPTION", 0).Trim();
            }

            clsDevexpressUtil.ItemLookUpEditSetup(le_badcode1, clsCommon.GetPackBad(), "", false, 0, true);
            le_badcode1.EditValue = clsCommon.GetCOMM_DTNAME("03", "70", "파포");

            clsDevexpressUtil.ItemLookUpEditSetup(le_badcode2, clsCommon.GetPackBad(), "", false, 0, true);
            le_badcode2.EditValue = clsCommon.GetCOMM_DTNAME("03", "70", "부적합");

            clsDevexpressUtil.ItemLookUpEditSetup(le_badcode3, clsCommon.GetPackBad(), "", false, 0, true);

            clsDevexpressUtil.ItemLookUpEditSetup(le_badcode4, clsCommon.GetPackBad(), "", false, 0, true);

            clsDevexpressUtil.ItemLookUpEditSetup(le_badcode5, clsCommon.GetPackBad(), "", false, 0, true);

            //clsDevexpressUtil.ItemSearchLookUpEditSetup(sCboResourceNo, clsCommon.GetResource(argPlantCode, argProcessKey, $"'{clsCommon.GetResourceTypeCode("포장재료")}'", "", 0, true));

            chkAuto.Checked = clsCommon.GetTransAuto(argPlantCode, argProcessKey) == "Y" ? true : false;
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_input_Click(object sender, EventArgs e)
        {
            int badbeginqty = int.Parse(txt_badbeginqty.Text);
            int badendqty = int.Parse(txt_badendqty.Text);
            int badqty1 = int.Parse(txt_badqty1.Text);
            int badqty2 = int.Parse(txt_badqty2.Text);
            int badremantsqty = int.Parse(txt_badremantsqty.Text);

            if (string.IsNullOrEmpty(txt_proqty.Text))
            {
                ShowMessageBox.XtraShowInformation("포장생산수량을 입력해주세요");
                this.DialogResult = DialogResult.None;
                return;
            }

            int time_diff = Convert.ToDateTime(dateEdit_workEnd.EditValue).CompareTo(Convert.ToDateTime(dateEdit_workStart.EditValue));
            if (time_diff < 0)
            {
                ShowMessageBox.XtraShowInformation("종료시간이 시작시간보다 빠릅니다");
                this.DialogResult = DialogResult.None;
                return;
            }

            SQL = $@"
            SELECT BU_P FROM SAP_DI_PRODUCT c WHERE c.PLANT_CODE = '{argPlantCode}' AND c.RESOURCE_NO = '{argResourceNo}'
            ";

            DataSet ds = Dbconn.conn.ExecutDataset(SQL);

            if (ds != null && ds.Tables.Count > 0 && (badbeginqty > 0 || badendqty > 0 || badqty1 > 0 || badqty2 > 0 || badremantsqty > 0))
            {
                if (ds.Tables[0].Rows[0]["BU_P"]?.ToString() == "")
                {
                    ShowMessageBox.XtraShowWarning("부산물 마스터가 입력 되어있지 않아 첫물, 끝물, 짜투리를 입력 할수 없습니다.");
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }



            SQL = $@"
            UPDATE PACK_ORDER
            SET PRO_QTY = '{txt_proqty.Text}'
                , ERP_ISTATUS = 'N'
                , ERP_OSTATUS = 'N'
                , F_Q = '{txt_badbeginqty.EditValue}'
                , E_Q = '{txt_badendqty.EditValue}'
                , PA_Q = '{txt_badremantsqty.EditValue}'
                , USE_PA_Q = '{txt_badremantsUseQty.EditValue}'
                , BAD_CODE1 = '{le_badcode1.EditValue}'
                , BAD_QTY1  = '{txt_badqty1.EditValue}'
                , BAD_CODE2 = '{le_badcode2.EditValue}'
                , BAD_QTY2  = '{txt_badqty2.EditValue}'
                , BAD_CODE3 = '{le_badcode3.EditValue}'
                , BAD_QTY3  = '{txt_badqty3.EditValue}'
                , BAD_CODE4 = '{le_badcode4.EditValue}'
                , BAD_QTY4  = '{txt_badqty4.EditValue}'
                , BAD_CODE5 = '{le_badcode5.EditValue}'
                , BAD_QTY5  = '{txt_badqty5.EditValue}'
                , USE_EMP_PACK = '{txtUseEmptyPack.EditValue}'
                , RUN_ST = TO_DATE('{Convert.ToDateTime(dateEdit_workStart.EditValue).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
                , RUN_ET = TO_DATE('{Convert.ToDateTime(dateEdit_workEnd.EditValue).ToString("yyyy-MM-dd HH:mm:ss")}', 'YYYY-MM-DD HH24:MI:SS')
            WHERE PLANT_CODE = '{argPlantCode}' AND PROCESS_KEY = '{argProcessKey}' AND L_CODE = '{argLCode}' AND WORKDATE = '{txt_workdate.Text}' AND WORK_SEQ = '{txt_workseq.Text}'";
           
            if (Dbconn.conn.SQLrun(SQL) < 0)
            {
                clsLog.logSave(this, "btn_input_Click", SQL);
                ShowMessageBox.XtraShowInformation("강제완료를 실행을 실패했습니다");
                this.DialogResult = DialogResult.None;
                return;
            }

        }
    }
}