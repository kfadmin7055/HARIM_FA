namespace HARIM_FA_DOSING
{
    partial class m_BinSeqDupChack
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(m_BinSeqDupChack));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.btn_reflash = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn_SCALE_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_RESOURCE_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_DESCRIPTION = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_LOCATION = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_SEQ = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCboBinCode = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.reptemCheckEdit_SEQ = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.btn_save = new DevExpress.XtraEditors.SimpleButton();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.splashScreenManager = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::HARIM_FA_DOSING.WaitForm), true, false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCboBinCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reptemCheckEdit_SEQ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.btn_reflash);
            this.layoutControl.Controls.Add(this.gridControl);
            this.layoutControl.Controls.Add(this.btn_save);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(2098, 173, 1314, 884);
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(851, 420);
            this.layoutControl.TabIndex = 6;
            this.layoutControl.Text = "layoutControl1";
            // 
            // btn_reflash
            // 
            this.btn_reflash.AllowFocus = false;
            this.btn_reflash.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_reflash.ImageOptions.Image")));
            this.btn_reflash.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btn_reflash.Location = new System.Drawing.Point(686, 26);
            this.btn_reflash.Margin = new System.Windows.Forms.Padding(0);
            this.btn_reflash.Name = "btn_reflash";
            this.btn_reflash.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_reflash.Size = new System.Drawing.Size(36, 42);
            this.btn_reflash.StyleController = this.layoutControl;
            this.btn_reflash.TabIndex = 16;
            this.btn_reflash.ToolTip = "새로고침(F5)";
            this.btn_reflash.Click += new System.EventHandler(this.btn_reflash_Click);
            // 
            // gridControl
            // 
            this.gridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.gridControl.Location = new System.Drawing.Point(6, 72);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gridControl.Name = "gridControl";
            this.gridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.reptemCheckEdit_SEQ,
            this.gridCboBinCode});
            this.gridControl.Size = new System.Drawing.Size(839, 342);
            this.gridControl.TabIndex = 5;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn_SCALE_CODE,
            this.gridColumn_RESOURCE_NO,
            this.gridColumn_DESCRIPTION,
            this.gridColumn_LOCATION,
            this.gridColumn_SEQ});
            this.gridView.DetailHeight = 404;
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            this.gridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridView.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always;
            this.gridView.OptionsFind.FindNullPrompt = "검색할 문자열을 입력해 주세요...";
            this.gridView.OptionsFind.ShowFindButton = false;
            this.gridView.OptionsView.AllowCellMerge = true;
            this.gridView.OptionsView.ColumnAutoWidth = false;
            this.gridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.gridView.OptionsView.ShowGroupPanel = false;
            this.gridView.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gridView_RowCellStyle);
            // 
            // gridColumn_SCALE_CODE
            // 
            this.gridColumn_SCALE_CODE.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_SCALE_CODE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_SCALE_CODE.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_SCALE_CODE.Caption = "스케일명";
            this.gridColumn_SCALE_CODE.FieldName = "SCALE_CODE";
            this.gridColumn_SCALE_CODE.Name = "gridColumn_SCALE_CODE";
            this.gridColumn_SCALE_CODE.OptionsColumn.AllowFocus = false;
            this.gridColumn_SCALE_CODE.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn_SCALE_CODE.OptionsColumn.ReadOnly = true;
            this.gridColumn_SCALE_CODE.Visible = true;
            this.gridColumn_SCALE_CODE.VisibleIndex = 0;
            this.gridColumn_SCALE_CODE.Width = 80;
            // 
            // gridColumn_RESOURCE_NO
            // 
            this.gridColumn_RESOURCE_NO.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_RESOURCE_NO.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_RESOURCE_NO.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_RESOURCE_NO.Caption = "원료코드";
            this.gridColumn_RESOURCE_NO.FieldName = "RESOURCE_NO";
            this.gridColumn_RESOURCE_NO.Name = "gridColumn_RESOURCE_NO";
            this.gridColumn_RESOURCE_NO.OptionsColumn.AllowFocus = false;
            this.gridColumn_RESOURCE_NO.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn_RESOURCE_NO.OptionsColumn.ReadOnly = true;
            this.gridColumn_RESOURCE_NO.Visible = true;
            this.gridColumn_RESOURCE_NO.VisibleIndex = 1;
            this.gridColumn_RESOURCE_NO.Width = 250;
            // 
            // gridColumn_DESCRIPTION
            // 
            this.gridColumn_DESCRIPTION.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_DESCRIPTION.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_DESCRIPTION.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_DESCRIPTION.Caption = "원재료명";
            this.gridColumn_DESCRIPTION.FieldName = "DESCRIPTION";
            this.gridColumn_DESCRIPTION.Name = "gridColumn_DESCRIPTION";
            this.gridColumn_DESCRIPTION.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn_DESCRIPTION.OptionsColumn.ReadOnly = true;
            this.gridColumn_DESCRIPTION.Visible = true;
            this.gridColumn_DESCRIPTION.VisibleIndex = 2;
            this.gridColumn_DESCRIPTION.Width = 268;
            // 
            // gridColumn_LOCATION
            // 
            this.gridColumn_LOCATION.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_LOCATION.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_LOCATION.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_LOCATION.Caption = "빈코드";
            this.gridColumn_LOCATION.FieldName = "LOCATION";
            this.gridColumn_LOCATION.Name = "gridColumn_LOCATION";
            this.gridColumn_LOCATION.OptionsColumn.AllowFocus = false;
            this.gridColumn_LOCATION.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_LOCATION.OptionsColumn.ReadOnly = true;
            this.gridColumn_LOCATION.Visible = true;
            this.gridColumn_LOCATION.VisibleIndex = 3;
            this.gridColumn_LOCATION.Width = 113;
            // 
            // gridColumn_SEQ
            // 
            this.gridColumn_SEQ.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_SEQ.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_SEQ.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_SEQ.Caption = "우선순위빈코드";
            this.gridColumn_SEQ.ColumnEdit = this.reptemCheckEdit_SEQ;
            this.gridColumn_SEQ.FieldName = "SEQ";
            this.gridColumn_SEQ.Name = "gridColumn_SEQ";
            this.gridColumn_SEQ.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_SEQ.Visible = true;
            this.gridColumn_SEQ.VisibleIndex = 4;
            this.gridColumn_SEQ.Width = 92;
            // 
            // gridCboBinCode
            // 
            this.gridCboBinCode.AutoHeight = false;
            this.gridCboBinCode.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridCboBinCode.Name = "gridCboBinCode";
            // 
            // reptemCheckEdit_SEQ
            // 
            this.reptemCheckEdit_SEQ.AutoHeight = false;
            this.reptemCheckEdit_SEQ.Name = "reptemCheckEdit_SEQ";
            this.reptemCheckEdit_SEQ.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.reptemCheckEdit_SEQ.ValueChecked = 1;
            this.reptemCheckEdit_SEQ.ValueUnchecked = 2;
            this.reptemCheckEdit_SEQ.CheckedChanged += new System.EventHandler(this.reptemCheckEdit_SEQ_CheckedChanged);
            // 
            // btn_save
            // 
            this.btn_save.AllowFocus = false;
            this.btn_save.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_save.ImageOptions.Image")));
            this.btn_save.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_save.ImageOptions.ImageToTextIndent = 10;
            this.btn_save.Location = new System.Drawing.Point(746, 26);
            this.btn_save.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_save.Name = "btn_save";
            this.btn_save.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_save.Size = new System.Drawing.Size(99, 42);
            this.btn_save.StyleController = this.layoutControl;
            this.btn_save.TabIndex = 7;
            this.btn_save.Text = "저 장";
            this.btn_save.ToolTip = "저장(Ctrl + S)";
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.Root.Size = new System.Drawing.Size(851, 420);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup
            // 
            this.layoutControlGroup.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlGroup.CaptionImageOptions.Image")));
            this.layoutControlGroup.CustomizationFormText = "같은 스케일의 빈원료 우선순위를 변경합니다";
            this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem3,
            this.emptySpaceItem2,
            this.layoutControlItem6});
            this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup.Name = "layoutControlGroup";
            this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(1, 1, 1, 1);
            this.layoutControlGroup.Size = new System.Drawing.Size(851, 420);
            this.layoutControlGroup.Text = "같은 스케일의 빈원료 우선순위를 변경합니다";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 46);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(843, 346);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btn_save;
            this.layoutControlItem3.Location = new System.Drawing.Point(740, 0);
            this.layoutControlItem3.MaxSize = new System.Drawing.Size(103, 46);
            this.layoutControlItem3.MinSize = new System.Drawing.Size(103, 46);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(103, 46);
            this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(0, 0);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(680, 46);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.btn_reflash;
            this.layoutControlItem6.Location = new System.Drawing.Point(680, 0);
            this.layoutControlItem6.MaxSize = new System.Drawing.Size(60, 46);
            this.layoutControlItem6.MinSize = new System.Drawing.Size(60, 46);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(60, 46);
            this.layoutControlItem6.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem6.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 20, 0, 0);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // splashScreenManager
            // 
            this.splashScreenManager.ClosingDelay = 500;
            // 
            // m_BinSeqDupChack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(851, 420);
            this.Controls.Add(this.layoutControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("m_BinSeqDupChack.IconOptions.Image")));
            this.Name = "m_BinSeqDupChack";
            this.Text = "빈 우선순위";
            this.Load += new System.EventHandler(this.m_BinSeqDupChack_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCboBinCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reptemCheckEdit_SEQ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraEditors.SimpleButton btn_reflash;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraEditors.SimpleButton btn_save;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_SCALE_CODE;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_RESOURCE_NO;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_LOCATION;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_SEQ;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit reptemCheckEdit_SEQ;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_DESCRIPTION;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit gridCboBinCode;
    }
}