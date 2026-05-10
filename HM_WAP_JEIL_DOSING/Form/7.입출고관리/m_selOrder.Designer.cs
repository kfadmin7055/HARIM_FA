namespace HARIM_FA_DOSING
{ 
    partial class m_selOrder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(m_selOrder));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_save = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn_ORDER_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_LINE_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_CL_SER_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_CL_LN_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_VENDOR_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_STORAGE_LOC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.btn_cancel);
            this.layoutControl.Controls.Add(this.btn_save);
            this.layoutControl.Controls.Add(this.gridControl);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(648, 420);
            this.layoutControl.TabIndex = 3;
            this.layoutControl.Text = "layoutControl1";
            // 
            // btn_cancel
            // 
            this.btn_cancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_cancel.ImageOptions.Image")));
            this.btn_cancel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_cancel.ImageOptions.ImageToTextIndent = 10;
            this.btn_cancel.Location = new System.Drawing.Point(320, 390);
            this.btn_cancel.Margin = new System.Windows.Forms.Padding(4);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_cancel.Size = new System.Drawing.Size(169, 28);
            this.btn_cancel.StyleController = this.layoutControl;
            this.btn_cancel.TabIndex = 8;
            this.btn_cancel.Text = "취 소";
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_save
            // 
            this.btn_save.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_save.ImageOptions.Image")));
            this.btn_save.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_save.ImageOptions.ImageToTextIndent = 10;
            this.btn_save.Location = new System.Drawing.Point(151, 390);
            this.btn_save.Margin = new System.Windows.Forms.Padding(4);
            this.btn_save.Name = "btn_save";
            this.btn_save.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_save.Size = new System.Drawing.Size(165, 28);
            this.btn_save.StyleController = this.layoutControl;
            this.btn_save.TabIndex = 6;
            this.btn_save.Text = "입 력";
            this.btn_save.ToolTip = "저장(Ctrl + S)";
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // gridControl
            // 
            this.gridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gridControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gridControl.Location = new System.Drawing.Point(8, 28);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(632, 352);
            this.gridControl.TabIndex = 5;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn_ORDER_NO,
            this.gridColumn_LINE_NO,
            this.gridColumn_CL_SER_NO,
            this.gridColumn_CL_LN_NO,
            this.gridColumn_VENDOR_NAME,
            this.gridColumn_STORAGE_LOC,
            this.gridColumn1});
            this.gridView.DetailHeight = 404;
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            this.gridView.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always;
            this.gridView.OptionsFind.FindNullPrompt = "검색할 문자열을 입력해 주세요...";
            this.gridView.OptionsFind.ShowFindButton = false;
            this.gridView.OptionsView.ColumnAutoWidth = false;
            this.gridView.OptionsView.EnableAppearanceOddRow = true;
            this.gridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.gridView.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn_ORDER_NO
            // 
            this.gridColumn_ORDER_NO.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_ORDER_NO.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_ORDER_NO.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_ORDER_NO.Caption = "구매주문서번호";
            this.gridColumn_ORDER_NO.FieldName = "order_no";
            this.gridColumn_ORDER_NO.Name = "gridColumn_ORDER_NO";
            this.gridColumn_ORDER_NO.Visible = true;
            this.gridColumn_ORDER_NO.VisibleIndex = 0;
            this.gridColumn_ORDER_NO.Width = 114;
            // 
            // gridColumn_LINE_NO
            // 
            this.gridColumn_LINE_NO.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_LINE_NO.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_LINE_NO.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_LINE_NO.Caption = "라인번호";
            this.gridColumn_LINE_NO.FieldName = "line_no";
            this.gridColumn_LINE_NO.Name = "gridColumn_LINE_NO";
            this.gridColumn_LINE_NO.Visible = true;
            this.gridColumn_LINE_NO.VisibleIndex = 1;
            this.gridColumn_LINE_NO.Width = 121;
            // 
            // gridColumn_CL_SER_NO
            // 
            this.gridColumn_CL_SER_NO.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_CL_SER_NO.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_CL_SER_NO.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_CL_SER_NO.Caption = "통관관리번호";
            this.gridColumn_CL_SER_NO.FieldName = "cl_ser_no";
            this.gridColumn_CL_SER_NO.Name = "gridColumn_CL_SER_NO";
            this.gridColumn_CL_SER_NO.Visible = true;
            this.gridColumn_CL_SER_NO.VisibleIndex = 2;
            this.gridColumn_CL_SER_NO.Width = 131;
            // 
            // gridColumn_CL_LN_NO
            // 
            this.gridColumn_CL_LN_NO.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_CL_LN_NO.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_CL_LN_NO.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_CL_LN_NO.Caption = "통관라인번호";
            this.gridColumn_CL_LN_NO.FieldName = "cl_ln_no";
            this.gridColumn_CL_LN_NO.Name = "gridColumn_CL_LN_NO";
            this.gridColumn_CL_LN_NO.Visible = true;
            this.gridColumn_CL_LN_NO.VisibleIndex = 3;
            this.gridColumn_CL_LN_NO.Width = 112;
            // 
            // gridColumn_VENDOR_NAME
            // 
            this.gridColumn_VENDOR_NAME.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_VENDOR_NAME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_VENDOR_NAME.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_VENDOR_NAME.Caption = "거래처";
            this.gridColumn_VENDOR_NAME.FieldName = "vendor_name";
            this.gridColumn_VENDOR_NAME.Name = "gridColumn_VENDOR_NAME";
            this.gridColumn_VENDOR_NAME.Visible = true;
            this.gridColumn_VENDOR_NAME.VisibleIndex = 4;
            this.gridColumn_VENDOR_NAME.Width = 95;
            // 
            // gridColumn_STORAGE_LOC
            // 
            this.gridColumn_STORAGE_LOC.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_STORAGE_LOC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_STORAGE_LOC.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_STORAGE_LOC.Caption = "저장소";
            this.gridColumn_STORAGE_LOC.FieldName = "storage_loc";
            this.gridColumn_STORAGE_LOC.Name = "gridColumn_STORAGE_LOC";
            this.gridColumn_STORAGE_LOC.Visible = true;
            this.gridColumn_STORAGE_LOC.VisibleIndex = 5;
            this.gridColumn_STORAGE_LOC.Width = 124;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "gridColumn1";
            this.gridColumn1.FieldName = "in_type";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup,
            this.emptySpaceItem1,
            this.layoutControlItem2,
            this.layoutControlItem4,
            this.emptySpaceItem2});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.Root.Size = new System.Drawing.Size(648, 420);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup
            // 
            this.layoutControlGroup.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlGroup.CaptionImageOptions.Image")));
            this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup.Name = "layoutControlGroup";
            this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlGroup.Size = new System.Drawing.Size(648, 388);
            this.layoutControlGroup.Text = "주문오더번호를 선택하여 주세요";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(636, 356);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 388);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(149, 32);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btn_save;
            this.layoutControlItem2.Location = new System.Drawing.Point(149, 388);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(169, 32);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btn_cancel;
            this.layoutControlItem4.Location = new System.Drawing.Point(318, 388);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(173, 32);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(491, 388);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(157, 32);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // m_selOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 420);
            this.Controls.Add(this.layoutControl);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "m_selOrder";
            this.Text = "주문 오더번호 선택";
            this.Load += new System.EventHandler(this.m_selOrder_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_ORDER_NO;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_LINE_NO;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_CL_SER_NO;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_CL_LN_NO;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_VENDOR_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_STORAGE_LOC;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.SimpleButton btn_save;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraEditors.SimpleButton btn_cancel;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
    }
}