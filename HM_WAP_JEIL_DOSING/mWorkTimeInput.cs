using Core.Class;
using System;
using System.Data;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class mWorkTimeInput : DevExpress.XtraEditors.XtraForm
    {
        string SQL = string.Empty;
        string argWorkData = string.Empty;
        string argWorkNum = string.Empty;

        public mWorkTimeInput(string workDate, string workNum)
        {
            argWorkData = workDate;
            argWorkNum = workNum;

            InitializeComponent();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btn_input_Click(object sender, EventArgs e)
        {
            SQL = "UPDATE WORK_ORDER SET  START_TIME = CONVERT(DATETIME, '{2}') , END_TIME = CONVERT(DATETIME, '{3}')   WHERE PROCESS_KEY = 'P05' AND WORKDATE = '{0}' AND NUM = '{1}' ";
            SQL = string.Format(SQL,
                argWorkData,
                argWorkNum,
                Convert.ToDateTime(dateEdit_workStart.EditValue).ToString("yyyy-MM-dd HH:mm:ss"),
                Convert.ToDateTime(dateEdit_workEnd.EditValue).ToString("yyyy-MM-dd HH:mm:ss")
                );

            Dbconn.conn.SQLrun(SQL);

            this.DialogResult = DialogResult.OK;
        }

        private void mWorkTimeInput_Load(object sender, EventArgs e)
        {
            txt_workdate.Text = argWorkData;
            txt_workseq.Text = argWorkNum;

            dateEdit_workStart.EditValue = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            dateEdit_workEnd.EditValue = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            SQL = $"SELECT ERP_ING.DESCRIPTION FROM WORK_ORDER WO LEFT OUTER JOIN SAP_DI_PRODUCT ERP_ING " +
                "ON WO.RESOURCE_NO = ERP_ING.RESOURCE_NO " +
                "WHERE WO.PROCESS_KEY = 'P05' AND WO.WORKDATE = '{0}' AND WO.NUM = '{1}' ";

            SQL = string.Format(SQL, argWorkData, argWorkNum);

            using (DataSet resDs = Dbconn.conn.ExecutDataset(SQL))
            {
                txt_resName.Text = Dbconn.conn.getData(resDs, "DESCRIPTION", 0).Trim();

            }
        }
    }
}