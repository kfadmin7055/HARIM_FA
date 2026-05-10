using Core.Class;
using DevExpress.XtraEditors;
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
    public partial class mPlcConnSetting : DevExpress.XtraEditors.XtraForm
    {
        private string vPlcType = string.Empty;

        public mPlcConnSetting(string sPlcType)
        {
            InitializeComponent();

            vPlcType = sPlcType;
        }

        private void mPlcConnSetting_Load(object sender, EventArgs e)
        {
            bool bPlcYn = false;

            if (vPlcType == "Dos")
                bPlcYn = Properties.Settings.Default.MainPlc_Yn == "Y";
            else
                bPlcYn = Properties.Settings.Default.SubPlc_Yn == "Y";


            if (bPlcYn)
            {
                toggleSwitch_plcYn.IsOn = true;
            }
            else
            {
                toggleSwitch_plcYn.IsOn = false;
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (toggleSwitch_plcYn.IsOn)
            {
                if (vPlcType == "Dos")
                {
                    Properties.Settings.Default.MainPlc_Yn = "Y";
                    clsCommon._strMainPlcConnYn = "Y";
                }
                else
                {
                    Properties.Settings.Default.SubPlc_Yn = "Y";
                    clsCommon._strSubPlcConnYn = "Y";
                }
            }
            else
            {
                if (vPlcType == "Dos")
                {
                    Properties.Settings.Default.MainPlc_Yn = "N";
                    clsCommon._strMainPlcConnYn = "N";
                    
                }
                else
                {
                    Properties.Settings.Default.SubPlc_Yn = "N";
                    clsCommon._strSubPlcConnYn = "N";
                }

                MAIN.MainPlcConnChk = "M";
            }

            Properties.Settings.Default.Save();
            this.Close();
        }

        private void mPlcConnSetting_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_ok_Click(null, null);
            }
        }

        private void toggleSwitch_plcYn_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                btn_ok_Click(null, null);
            }
        }
    }
}