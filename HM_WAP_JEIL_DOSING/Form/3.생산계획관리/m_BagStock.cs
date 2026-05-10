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
    public partial class m_BagStock : DevExpress.XtraEditors.XtraForm
    {
        private string SQL = String.Empty;

        string argWorkNumb = string.Empty;
        string argWorkSeq = string.Empty;
        string argSbno = string.Empty;

        private string vPlantCode = string.Empty;

        public m_BagStock(string sPlantcode)
        {
            vPlantCode = sPlantcode;

            InitializeComponent();
        }

        public m_BagStock()
        {
            InitializeComponent();
        }

        private void m_WorkGroup_Load(object sender, EventArgs e)
        {
            ControlInit();
        }

        private void ControlInit()
        {
            dtEND_YM.EditValue = DateTime.Today.ToString("yyyy-MM-dd");

            clsDevexpressUtil.ItemSearchLookUpEditSetup(sCboResourceNo, clsCommon.GetResource(vPlantCode, clsCommon.GetProcessKey("타이콘")), "", true);
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

                if (string.IsNullOrEmpty(txtEND_Q.EditValue?.ToString()))
                {
                    ShowMessageBox.XtraShowWarning("작업일자를 입력하여 주세요");
                    return;
                }

                if (string.IsNullOrEmpty(sCboResourceNo.EditValue?.ToString()))
                {
                    ShowMessageBox.XtraShowWarning("근무시작시간을 입력하여 주세요");
                    return;
                }

                string ENDYM = dtEND_YM.EditValue == null
                                            ? ""
                                            : (dtEND_YM.EditValue is DateOnly dt
                                                ? dt.ToDateTime(TimeOnly.MinValue).ToString("yyyyMMdd")
                                                : Convert.ToDateTime(dtEND_YM.EditValue).ToString("yyyyMMdd"));

                string SQL = $@"
                INSERT INTO BAG_C_STOCK   -- TB01
                     ( EMPLOYEE_NO    -- 01
                     , END_Q          -- 02
                     , END_YM         -- 03
                     , PLANT_CODE     -- 04
                     , RESOURCE_NO    -- 05
                     )
                VALUES ( '{clsCommon.UserId}'    -- 01
                     , '{txtEND_Q.EditValue}'            -- 02
                     , '{ENDYM}'           -- 03
                     , '{vPlantCode}'       -- 04
                     , '{sCboResourceNo.EditValue}'      -- 05
                     )
                ";

                if (Dbconn.conn.SQLrun(SQL) < 1)
                {
                    Dbconn.conn.Rollback();
                    clsLog.logSave(this.Text, "btn_input_Click", SQL);
                    ShowMessageBox.XtraShowWarning("데이터 입력에 실패했습니다");
                    return;
                }

                Dbconn.conn.Commit();

                ShowMessageBox.XtraShowWarning("타이콘 재고를 추가 했습니다");
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