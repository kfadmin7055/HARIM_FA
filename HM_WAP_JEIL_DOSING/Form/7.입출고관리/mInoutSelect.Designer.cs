namespace HARIM_FA_DOSING
{
    partial class mInoutSelect
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mInoutSelect));
            this.btn_outSelect = new DevExpress.XtraEditors.SimpleButton();
            this.btn_inSelect = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // btn_outSelect
            // 
            this.btn_outSelect.Appearance.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_outSelect.Appearance.Options.UseFont = true;
            this.btn_outSelect.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btn_outSelect.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_outSelect.ImageOptions.Image")));
            this.btn_outSelect.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_outSelect.ImageOptions.ImageToTextIndent = 10;
            this.btn_outSelect.Location = new System.Drawing.Point(250, 26);
            this.btn_outSelect.Name = "btn_outSelect";
            this.btn_outSelect.Size = new System.Drawing.Size(231, 38);
            this.btn_outSelect.TabIndex = 1;
            this.btn_outSelect.Text = "중간계근/ 출차처리";
            // 
            // btn_inSelect
            // 
            this.btn_inSelect.Appearance.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_inSelect.Appearance.Options.UseFont = true;
            this.btn_inSelect.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btn_inSelect.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_inSelect.ImageOptions.Image")));
            this.btn_inSelect.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_inSelect.ImageOptions.ImageToTextIndent = 10;
            this.btn_inSelect.Location = new System.Drawing.Point(38, 26);
            this.btn_inSelect.Name = "btn_inSelect";
            this.btn_inSelect.Size = new System.Drawing.Size(164, 38);
            this.btn_inSelect.TabIndex = 0;
            this.btn_inSelect.Text = "입차처리";
            // 
            // mInoutSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 100);
            this.Controls.Add(this.btn_outSelect);
            this.Controls.Add(this.btn_inSelect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("mInoutSelect.IconOptions.Image")));
            this.MaximizeBox = false;
            this.Name = "mInoutSelect";
            this.Text = "입/출차처리 선택";
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btn_inSelect;
        private DevExpress.XtraEditors.SimpleButton btn_outSelect;
    }
}