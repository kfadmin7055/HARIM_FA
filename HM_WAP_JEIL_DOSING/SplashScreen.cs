using System;
using Core.Class;

namespace HARIM_FA_DOSING
{
    public partial class SplashScreen : DevExpress.XtraSplashScreen.SplashScreen
    {
        public SplashScreen()
        {
            InitializeComponent();
            this.labelCopyright.Text = "Copyright © 2022-" + DateTime.Now.Year.ToString();
        }

        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum SplashScreenCommand
        {
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            if (clsCommon.PlantCode == "P101" || clsCommon.PlantCode == "P102")
                clsCommon.SetSplashLogo(peImage, "하림");
            else if (clsCommon.PlantCode == "PJ01" || clsCommon.PlantCode == "PJ02" || clsCommon.PlantCode == "PJ04" || clsCommon.PlantCode == "PJ05")
                clsCommon.SetSplashLogo(peImage, "제일");
            else if (clsCommon.PlantCode == "P201")
                clsCommon.SetSplashLogo(peImage, "올품");
        }
    }
}