namespace HARIM_FA_DOSING
{ 
    partial class m_Note
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(m_Note));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.gridNote = new DevExpress.XtraGrid.GridControl();
            this.bandViewNote = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.bDictionary = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.mIDNRK = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.mIDNRK_DESC = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridPDscboRESOURCE_NO = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.repositoryItemSearchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridPDscboPLANT = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridcboUNIT = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridcboYN = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridDate = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandViewNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridPDscboRESOURCE_NO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridPDscboPLANT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboUNIT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboYN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDate.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.gridNote);
            this.layoutControl.Controls.Add(this.btn_cancel);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(1163, 620);
            this.layoutControl.TabIndex = 3;
            this.layoutControl.Text = "layoutControl1";
            // 
            // gridNote
            // 
            this.gridNote.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gridNote.Location = new System.Drawing.Point(8, 28);
            this.gridNote.MainView = this.bandViewNote;
            this.gridNote.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gridNote.Name = "gridNote";
            this.gridNote.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.gridPDscboRESOURCE_NO,
            this.gridPDscboPLANT,
            this.gridcboUNIT,
            this.gridcboYN,
            this.gridDate});
            this.gridNote.Size = new System.Drawing.Size(1147, 552);
            this.gridNote.TabIndex = 9;
            this.gridNote.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.bandViewNote});
            // 
            // bandViewNote
            // 
            this.bandViewNote.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.bDictionary});
            this.bandViewNote.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.mIDNRK,
            this.mIDNRK_DESC});
            this.bandViewNote.DetailHeight = 404;
            this.bandViewNote.GridControl = this.gridNote;
            this.bandViewNote.Name = "bandViewNote";
            this.bandViewNote.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.bandViewNote.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always;
            this.bandViewNote.OptionsFind.FindNullPrompt = "검색할 문자열을 입력해 주세요...";
            this.bandViewNote.OptionsFind.ShowFindButton = false;
            this.bandViewNote.OptionsView.ColumnAutoWidth = false;
            this.bandViewNote.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.bandViewNote.OptionsView.ShowGroupPanel = false;
            this.bandViewNote.ViewCaption = "<color=lightgreen>■</color> 입고원료 정보입력";
            // 
            // bDictionary
            // 
            this.bDictionary.AppearanceHeader.Options.UseTextOptions = true;
            this.bDictionary.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bDictionary.Caption = "전 배합비 항목";
            this.bDictionary.Columns.Add(this.mIDNRK);
            this.bDictionary.Columns.Add(this.mIDNRK_DESC);
            this.bDictionary.Name = "bDictionary";
            this.bDictionary.VisibleIndex = 0;
            this.bDictionary.Width = 250;
            // 
            // mIDNRK
            // 
            this.mIDNRK.AppearanceHeader.Options.UseTextOptions = true;
            this.mIDNRK.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.mIDNRK.Caption = "원료코드";
            this.mIDNRK.FieldName = "IDNRK";
            this.mIDNRK.MinWidth = 90;
            this.mIDNRK.Name = "mIDNRK";
            this.mIDNRK.Visible = true;
            this.mIDNRK.Width = 90;
            // 
            // mIDNRK_DESC
            // 
            this.mIDNRK_DESC.AppearanceCell.Options.UseTextOptions = true;
            this.mIDNRK_DESC.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.mIDNRK_DESC.AppearanceHeader.Options.UseTextOptions = true;
            this.mIDNRK_DESC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.mIDNRK_DESC.Caption = "원료명";
            this.mIDNRK_DESC.FieldName = "IDNRK_DESC";
            this.mIDNRK_DESC.MinWidth = 160;
            this.mIDNRK_DESC.Name = "mIDNRK_DESC";
            this.mIDNRK_DESC.Visible = true;
            this.mIDNRK_DESC.Width = 160;
            // 
            // gridPDscboRESOURCE_NO
            // 
            this.gridPDscboRESOURCE_NO.AutoHeight = false;
            this.gridPDscboRESOURCE_NO.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridPDscboRESOURCE_NO.Name = "gridPDscboRESOURCE_NO";
            this.gridPDscboRESOURCE_NO.PopupView = this.repositoryItemSearchLookUpEdit1View;
            // 
            // repositoryItemSearchLookUpEdit1View
            // 
            this.repositoryItemSearchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemSearchLookUpEdit1View.Name = "repositoryItemSearchLookUpEdit1View";
            this.repositoryItemSearchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemSearchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // gridPDscboPLANT
            // 
            this.gridPDscboPLANT.AutoHeight = false;
            this.gridPDscboPLANT.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridPDscboPLANT.Name = "gridPDscboPLANT";
            this.gridPDscboPLANT.PopupView = this.gridView1;
            // 
            // gridView1
            // 
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridcboUNIT
            // 
            this.gridcboUNIT.AutoHeight = false;
            this.gridcboUNIT.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridcboUNIT.Name = "gridcboUNIT";
            // 
            // gridcboYN
            // 
            this.gridcboYN.AutoHeight = false;
            this.gridcboYN.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridcboYN.Name = "gridcboYN";
            // 
            // gridDate
            // 
            this.gridDate.AutoHeight = false;
            this.gridDate.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridDate.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridDate.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.gridDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridDate.EditFormat.FormatString = "yyyy-MM-dd";
            this.gridDate.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridDate.MaskSettings.Set("mask", "yyyy-MM-dd");
            this.gridDate.Name = "gridDate";
            // 
            // btn_cancel
            // 
            this.btn_cancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_cancel.ImageOptions.Image")));
            this.btn_cancel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_cancel.ImageOptions.ImageToTextIndent = 10;
            this.btn_cancel.Location = new System.Drawing.Point(490, 590);
            this.btn_cancel.Margin = new System.Windows.Forms.Padding(4);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_cancel.Size = new System.Drawing.Size(216, 28);
            this.btn_cancel.StyleController = this.layoutControl;
            this.btn_cancel.TabIndex = 8;
            this.btn_cancel.Text = "확 인";
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup,
            this.emptySpaceItem1,
            this.layoutControlItem4,
            this.emptySpaceItem2});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.Root.Size = new System.Drawing.Size(1163, 620);
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
            this.layoutControlGroup.Size = new System.Drawing.Size(1163, 588);
            this.layoutControlGroup.Text = "배합 버전 변경 이력";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridNote;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1151, 556);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 588);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(488, 32);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btn_cancel;
            this.layoutControlItem4.Location = new System.Drawing.Point(488, 588);
            this.layoutControlItem4.MaxSize = new System.Drawing.Size(220, 32);
            this.layoutControlItem4.MinSize = new System.Drawing.Size(220, 32);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(220, 32);
            this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(708, 588);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(455, 32);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // m_Note
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1163, 620);
            this.Controls.Add(this.layoutControl);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "m_Note";
            this.Text = "배합 버전 이력";
            this.Load += new System.EventHandler(this.m_Note_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandViewNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridPDscboRESOURCE_NO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridPDscboPLANT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboUNIT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboYN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDate.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraEditors.SimpleButton btn_cancel;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraGrid.GridControl gridNote;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit gridPDscboRESOURCE_NO;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemSearchLookUpEdit1View;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit gridPDscboPLANT;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit gridcboUNIT;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit gridcboYN;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit gridDate;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bandViewNote;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn mIDNRK;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn mIDNRK_DESC;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand bDictionary;
    }
}