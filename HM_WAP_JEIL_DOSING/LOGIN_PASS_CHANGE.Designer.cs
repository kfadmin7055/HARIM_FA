namespace HARIM_FA_DOSING
{
    partial class LOGIN_PASS_CHANGE
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LOGIN_PASS_CHANGE));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.txt_changePassword = new DevExpress.XtraEditors.TextEdit();
            this.txt_changeRePassword = new DevExpress.XtraEditors.TextEdit();
            this.btn_save = new DevExpress.XtraEditors.SimpleButton();
            this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.txt_password = new DevExpress.XtraEditors.TextEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_changePassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_changeRePassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_password.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.txt_changePassword);
            this.layoutControl.Controls.Add(this.txt_changeRePassword);
            this.layoutControl.Controls.Add(this.btn_save);
            this.layoutControl.Controls.Add(this.btn_cancel);
            this.layoutControl.Controls.Add(this.txt_password);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(399, 108, 650, 400);
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(448, 157);
            this.layoutControl.TabIndex = 1;
            this.layoutControl.Text = "layoutControl1";
            // 
            // txt_changePassword
            // 
            this.txt_changePassword.Location = new System.Drawing.Point(146, 41);
            this.txt_changePassword.Name = "txt_changePassword";
            this.txt_changePassword.Properties.ContextImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("txt_changePassword.Properties.ContextImageOptions.Image")));
            this.txt_changePassword.Size = new System.Drawing.Size(290, 24);
            this.txt_changePassword.StyleController = this.layoutControl;
            this.txt_changePassword.TabIndex = 4;
            // 
            // txt_changeRePassword
            // 
            this.txt_changeRePassword.Location = new System.Drawing.Point(145, 69);
            this.txt_changeRePassword.Name = "txt_changeRePassword";
            this.txt_changeRePassword.Properties.ContextImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("txt_changeRePassword.Properties.ContextImageOptions.Image")));
            this.txt_changeRePassword.Size = new System.Drawing.Size(291, 24);
            this.txt_changeRePassword.StyleController = this.layoutControl;
            this.txt_changeRePassword.TabIndex = 5;
            // 
            // btn_save
            // 
            this.btn_save.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_save.ImageOptions.Image")));
            this.btn_save.Location = new System.Drawing.Point(12, 122);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(209, 22);
            this.btn_save.StyleController = this.layoutControl;
            this.btn_save.TabIndex = 6;
            this.btn_save.Text = "비밀번호 변경";
            this.btn_save.ToolTip = "저장(Ctrl + S)";
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_cancel.ImageOptions.Image")));
            this.btn_cancel.Location = new System.Drawing.Point(225, 122);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(211, 22);
            this.btn_cancel.StyleController = this.layoutControl;
            this.btn_cancel.TabIndex = 7;
            this.btn_cancel.Text = "다음에 변경하기";
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // txt_password
            // 
            this.txt_password.Location = new System.Drawing.Point(146, 13);
            this.txt_password.Name = "txt_password";
            this.txt_password.Properties.ContextImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("txt_password.Properties.ContextImageOptions.Image")));
            this.txt_password.Properties.PasswordChar = '*';
            this.txt_password.Size = new System.Drawing.Size(290, 24);
            this.txt_password.StyleController = this.layoutControl;
            this.txt_password.TabIndex = 4;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.emptySpaceItem1,
            this.layoutControlItem5});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(448, 157);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem1.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem1.Control = this.txt_changePassword;
            this.layoutControlItem1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlItem1.ImageOptions.Image")));
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 28);
            this.layoutControlItem1.MaxSize = new System.Drawing.Size(428, 28);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(428, 28);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(428, 28);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.Text = "새 비밀번호";
            this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(90, 17);
            this.layoutControlItem1.TextToControlDistance = 44;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem2.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem2.Control = this.txt_changeRePassword;
            this.layoutControlItem2.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlItem2.ImageOptions.Image")));
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 56);
            this.layoutControlItem2.MaxSize = new System.Drawing.Size(428, 28);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(428, 28);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(428, 28);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.Text = "비밀번호 확인";
            this.layoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(100, 14);
            this.layoutControlItem2.TextToControlDistance = 33;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btn_save;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 109);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(213, 26);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btn_cancel;
            this.layoutControlItem4.Location = new System.Drawing.Point(213, 109);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(215, 26);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.BestFitWeight = 50;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 84);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(428, 25);
            this.emptySpaceItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem5.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem5.Control = this.txt_password;
            this.layoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem5.CustomizationFormText = "새 비밀번호";
            this.layoutControlItem5.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlItem5.ImageOptions.Image")));
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem5.MaxSize = new System.Drawing.Size(428, 28);
            this.layoutControlItem5.MinSize = new System.Drawing.Size(428, 28);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(428, 28);
            this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem5.Text = "기존 비밀번호";
            this.layoutControlItem5.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem5.TextSize = new System.Drawing.Size(97, 16);
            this.layoutControlItem5.TextToControlDistance = 37;
            // 
            // LOGIN_PASS_CHANGE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 157);
            this.Controls.Add(this.layoutControl);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("LOGIN_PASS_CHANGE.IconOptions.Image")));
            this.MaximizeBox = false;
            this.Name = "LOGIN_PASS_CHANGE";
            this.Text = "비밀번호변경";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txt_changePassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_changeRePassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_password.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraEditors.TextEdit txt_changePassword;
        private DevExpress.XtraEditors.TextEdit txt_changeRePassword;
        private DevExpress.XtraEditors.SimpleButton btn_save;
        private DevExpress.XtraEditors.SimpleButton btn_cancel;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraEditors.TextEdit txt_password;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
    }
}