namespace HARIM_FA_DOSING
{
    partial class m_BagStock
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(m_BagStock));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.sCboResourceNo = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txtEND_Q = new DevExpress.XtraEditors.TextEdit();
            this.btn_input = new DevExpress.XtraEditors.SimpleButton();
            this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.dtEND_YM = new DevExpress.XtraEditors.DateEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem15 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem14 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sCboResourceNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEND_Q.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEND_YM.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEND_YM.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.sCboResourceNo);
            this.layoutControl.Controls.Add(this.txtEND_Q);
            this.layoutControl.Controls.Add(this.btn_input);
            this.layoutControl.Controls.Add(this.btn_cancel);
            this.layoutControl.Controls.Add(this.dtEND_YM);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(434, 141);
            this.layoutControl.TabIndex = 1;
            this.layoutControl.Text = "layoutControl1";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup1});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.Root.Size = new System.Drawing.Size(434, 141);
            this.Root.TextVisible = false;
            // 
            // sCboResourceNo
            // 
            this.sCboResourceNo.Location = new System.Drawing.Point(119, 64);
            this.sCboResourceNo.Name = "sCboResourceNo";
            this.sCboResourceNo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.sCboResourceNo.Properties.PopupView = this.searchLookUpEdit1View;
            this.sCboResourceNo.Size = new System.Drawing.Size(304, 24);
            this.sCboResourceNo.StyleController = this.layoutControl;
            this.sCboResourceNo.TabIndex = 18;
            // 
            // searchLookUpEdit1View
            // 
            this.searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            this.searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // txtEND_Q
            // 
            this.txtEND_Q.Location = new System.Drawing.Point(338, 36);
            this.txtEND_Q.Name = "txtEND_Q";
            this.txtEND_Q.Size = new System.Drawing.Size(81, 24);
            this.txtEND_Q.StyleController = this.layoutControl;
            this.txtEND_Q.TabIndex = 17;
            // 
            // btn_input
            // 
            this.btn_input.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_input.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_input.ImageOptions.Image")));
            this.btn_input.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_input.ImageOptions.ImageToTextIndent = 20;
            this.btn_input.Location = new System.Drawing.Point(11, 92);
            this.btn_input.Name = "btn_input";
            this.btn_input.Size = new System.Drawing.Size(216, 37);
            this.btn_input.StyleController = this.layoutControl;
            this.btn_input.TabIndex = 15;
            this.btn_input.Text = "입  력";
            this.btn_input.Click += new System.EventHandler(this.btn_input_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_cancel.ImageOptions.Image")));
            this.btn_cancel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_cancel.ImageOptions.ImageToTextIndent = 20;
            this.btn_cancel.Location = new System.Drawing.Point(231, 92);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(192, 37);
            this.btn_cancel.StyleController = this.layoutControl;
            this.btn_cancel.TabIndex = 16;
            this.btn_cancel.Text = "취 소";
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // dtEND_YM
            // 
            this.dtEND_YM.EditValue = "";
            this.dtEND_YM.Location = new System.Drawing.Point(107, 36);
            this.dtEND_YM.Name = "dtEND_YM";
            this.dtEND_YM.Properties.AllowFocused = false;
            this.dtEND_YM.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtEND_YM.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtEND_YM.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Classic;
            this.dtEND_YM.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.dtEND_YM.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtEND_YM.Properties.EditFormat.FormatString = "yyyy-MM-dd";
            this.dtEND_YM.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtEND_YM.Properties.MaskSettings.Set("mask", "yyyy-MM-dd");
            this.dtEND_YM.Properties.UseMaskAsDisplayFormat = true;
            this.dtEND_YM.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.MonthView;
            this.dtEND_YM.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.False;
            this.dtEND_YM.Size = new System.Drawing.Size(116, 24);
            this.dtEND_YM.StyleController = this.layoutControl;
            this.dtEND_YM.TabIndex = 13;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlGroup1.CaptionImageOptions.Image")));
            this.layoutControlGroup1.GroupStyle = DevExpress.Utils.GroupStyle.Title;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem10,
            this.layoutControlItem15,
            this.layoutControlItem14,
            this.layoutControlItem1,
            this.layoutControlItem2});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 6, 6);
            this.layoutControlGroup1.Size = new System.Drawing.Size(430, 137);
            this.layoutControlGroup1.Text = "타이콘 재고 입력";
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem10.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem10.Control = this.dtEND_YM;
            this.layoutControlItem10.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlItem10.ImageOptions.Image")));
            this.layoutControlItem10.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem10.MaxSize = new System.Drawing.Size(216, 28);
            this.layoutControlItem10.MinSize = new System.Drawing.Size(216, 28);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Size = new System.Drawing.Size(216, 28);
            this.layoutControlItem10.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem10.Text = "작업일자";
            this.layoutControlItem10.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem10.TextSize = new System.Drawing.Size(69, 16);
            this.layoutControlItem10.TextToControlDistance = 27;
            // 
            // layoutControlItem15
            // 
            this.layoutControlItem15.Control = this.btn_cancel;
            this.layoutControlItem15.Location = new System.Drawing.Point(220, 56);
            this.layoutControlItem15.MinSize = new System.Drawing.Size(75, 26);
            this.layoutControlItem15.Name = "layoutControlItem15";
            this.layoutControlItem15.Size = new System.Drawing.Size(196, 41);
            this.layoutControlItem15.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem15.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem15.TextVisible = false;
            // 
            // layoutControlItem14
            // 
            this.layoutControlItem14.Control = this.btn_input;
            this.layoutControlItem14.Location = new System.Drawing.Point(0, 56);
            this.layoutControlItem14.MinSize = new System.Drawing.Size(79, 26);
            this.layoutControlItem14.Name = "layoutControlItem14";
            this.layoutControlItem14.Size = new System.Drawing.Size(220, 41);
            this.layoutControlItem14.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem14.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem14.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem1.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem1.Control = this.txtEND_Q;
            this.layoutControlItem1.ImageOptions.Image = global::HARIM_FA_DOSING.Properties.Resources.pagenext_16x16;
            this.layoutControlItem1.Location = new System.Drawing.Point(216, 0);
            this.layoutControlItem1.MaxSize = new System.Drawing.Size(193, 28);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(193, 28);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(200, 28);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.Text = " 마감수량(KG)";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(96, 16);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.sCboResourceNo;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 28);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(416, 28);
            this.layoutControlItem2.Text = "마감품목";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(96, 15);
            // 
            // m_BagStock
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 141);
            this.Controls.Add(this.layoutControl);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "m_BagStock";
            this.Text = "타이콘 재고 입력";
            this.Load += new System.EventHandler(this.m_WorkGroup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sCboResourceNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEND_Q.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEND_YM.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEND_YM.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraEditors.SimpleButton btn_input;
        private DevExpress.XtraEditors.SimpleButton btn_cancel;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem10;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem15;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem14;
        private DevExpress.XtraEditors.DateEdit dtEND_YM;
        private DevExpress.XtraEditors.TextEdit txtEND_Q;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.SearchLookUpEdit sCboResourceNo;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
    }
}