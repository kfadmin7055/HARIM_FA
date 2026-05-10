using Core.Class;
using DevExpress.Office.History;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraRichEdit.Model;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HARIM_FA_DOSING
{
    public partial class m_WorkDateCopy : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;

        string argPlantCode = String.Empty;
        string argProcessKey = String.Empty;
        string argLCode = String.Empty;

        string argAWorkDate = string.Empty;
        string argBWorkDate = string.Empty;
        string argSeq = string.Empty;

        public string SelectedWorkDate { get; set; }

        public m_WorkDateCopy(string sPlantCode, string sProcessKey, string sLCode, string sBWorkDate, string sSeq)
        {
            InitializeComponent();
            this.argPlantCode = sPlantCode;
            this.argProcessKey = sProcessKey;
            this.argLCode = sLCode;

            this.argBWorkDate = sBWorkDate;
            this.argSeq = sSeq;
        }

        public m_WorkDateCopy()
        {
            InitializeComponent();
        }

        private void m_WorkGroup_Load(object sender, EventArgs e)
        {
            ControlInit();
        }

        private void ControlInit()
        {
            dtAWorkDate.EditValue = DateTime.ParseExact(
                                                        argBWorkDate,
                                                        "yyyyMMdd",
                                                        CultureInfo.InvariantCulture
            );
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_input_Click(object sender, EventArgs e)
        {
            try
            {
                Dbconn.conn.BeginTransaction();

                string sAWorkDate = dtAWorkDate.EditValue == null
                                            ? ""
                                            : (dtAWorkDate.EditValue is DateOnly dt
                                                ? dt.ToDateTime(TimeOnly.MinValue).ToString("yyyyMMdd")
                                                : Convert.ToDateTime(dtAWorkDate.EditValue).ToString("yyyyMMdd"));

                SelectedWorkDate = sAWorkDate;
            }
            catch (Exception ex)
            {
                Dbconn.conn.Rollback();
                clsLog.logSave(this, "btn_input_Click", ex);
                ShowMessageBox.XtraShowWarning("저장을 실행하는 도중 에러가 발생했습니다");
            }
        }
    }
}