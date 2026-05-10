namespace HARIM_FA_DOSING
{ 
    partial class m_SAP_INPUT_PROTRANSD
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(m_SAP_INPUT_PROTRANSD));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.gridDetail = new DevExpress.XtraGrid.GridControl();
            this.viewDetail = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.detailTKNUM = new DevExpress.XtraGrid.Columns.GridColumn();
            this.detailVBELN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.detailPOSNR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.detailMATNR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridDcboScboResourceNo = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView8 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.detailLFIMG = new DevExpress.XtraGrid.Columns.GridColumn();
            this.detailVRKME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridDcboVRKME = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnInput = new DevExpress.XtraEditors.SimpleButton();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDcboScboResourceNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDcboVRKME)).BeginInit();
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
            this.layoutControl.Controls.Add(this.gridDetail);
            this.layoutControl.Controls.Add(this.btn_cancel);
            this.layoutControl.Controls.Add(this.btnInput);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(649, 420);
            this.layoutControl.TabIndex = 3;
            this.layoutControl.Text = "layoutControl1";
            // 
            // gridDetail
            // 
            this.gridDetail.Location = new System.Drawing.Point(8, 28);
            this.gridDetail.MainView = this.viewDetail;
            this.gridDetail.Name = "gridDetail";
            this.gridDetail.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.gridDcboScboResourceNo,
            this.gridDcboVRKME});
            this.gridDetail.Size = new System.Drawing.Size(633, 352);
            this.gridDetail.TabIndex = 9;
            this.gridDetail.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewDetail});
            // 
            // viewDetail
            // 
            this.viewDetail.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.detailTKNUM,
            this.detailVBELN,
            this.detailPOSNR,
            this.detailMATNR,
            this.detailLFIMG,
            this.detailVRKME});
            this.viewDetail.GridControl = this.gridDetail;
            this.viewDetail.Name = "viewDetail";
            this.viewDetail.OptionsView.ShowGroupPanel = false;
            this.viewDetail.DoubleClick += new System.EventHandler(this.viewDetail_DoubleClick);
            // 
            // detailTKNUM
            // 
            this.detailTKNUM.Caption = "배차번호";
            this.detailTKNUM.FieldName = "TKNUM";
            this.detailTKNUM.Name = "detailTKNUM";
            // 
            // detailVBELN
            // 
            this.detailVBELN.Caption = "출고지시서";
            this.detailVBELN.FieldName = "VBELN";
            this.detailVBELN.Name = "detailVBELN";
            this.detailVBELN.Visible = true;
            this.detailVBELN.VisibleIndex = 0;
            this.detailVBELN.Width = 110;
            // 
            // detailPOSNR
            // 
            this.detailPOSNR.Caption = "출고지시 항번";
            this.detailPOSNR.FieldName = "POSNR";
            this.detailPOSNR.Name = "detailPOSNR";
            this.detailPOSNR.Visible = true;
            this.detailPOSNR.VisibleIndex = 1;
            this.detailPOSNR.Width = 110;
            // 
            // detailMATNR
            // 
            this.detailMATNR.Caption = "품목";
            this.detailMATNR.ColumnEdit = this.gridDcboScboResourceNo;
            this.detailMATNR.FieldName = "MATNR";
            this.detailMATNR.MinWidth = 160;
            this.detailMATNR.Name = "detailMATNR";
            this.detailMATNR.Visible = true;
            this.detailMATNR.VisibleIndex = 2;
            this.detailMATNR.Width = 237;
            // 
            // gridDcboScboResourceNo
            // 
            this.gridDcboScboResourceNo.AutoHeight = false;
            this.gridDcboScboResourceNo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridDcboScboResourceNo.Name = "gridDcboScboResourceNo";
            this.gridDcboScboResourceNo.NullText = "";
            this.gridDcboScboResourceNo.PopupView = this.gridView8;
            // 
            // gridView8
            // 
            this.gridView8.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView8.Name = "gridView8";
            this.gridView8.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView8.OptionsView.ShowGroupPanel = false;
            // 
            // detailLFIMG
            // 
            this.detailLFIMG.Caption = "입고예정수량";
            this.detailLFIMG.DisplayFormat.FormatString = "{0:n0}";
            this.detailLFIMG.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.detailLFIMG.FieldName = "LFIMG";
            this.detailLFIMG.Name = "detailLFIMG";
            this.detailLFIMG.Visible = true;
            this.detailLFIMG.VisibleIndex = 3;
            this.detailLFIMG.Width = 85;
            // 
            // detailVRKME
            // 
            this.detailVRKME.Caption = "단위";
            this.detailVRKME.ColumnEdit = this.gridDcboVRKME;
            this.detailVRKME.FieldName = "VRKME";
            this.detailVRKME.Name = "detailVRKME";
            this.detailVRKME.Visible = true;
            this.detailVRKME.VisibleIndex = 4;
            this.detailVRKME.Width = 71;
            // 
            // gridDcboVRKME
            // 
            this.gridDcboVRKME.AutoHeight = false;
            this.gridDcboVRKME.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridDcboVRKME.Name = "gridDcboVRKME";
            this.gridDcboVRKME.NullText = "";
            // 
            // btn_cancel
            // 
            this.btn_cancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_cancel.ImageOptions.Image")));
            this.btn_cancel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_cancel.ImageOptions.ImageToTextIndent = 10;
            this.btn_cancel.Location = new System.Drawing.Point(324, 390);
            this.btn_cancel.Margin = new System.Windows.Forms.Padding(4);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_cancel.Size = new System.Drawing.Size(216, 28);
            this.btn_cancel.StyleController = this.layoutControl;
            this.btn_cancel.TabIndex = 8;
            this.btn_cancel.Text = "취 소";
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btnInput
            // 
            this.btnInput.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnInput.ImageOptions.Image")));
            this.btnInput.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnInput.ImageOptions.ImageToTextIndent = 10;
            this.btnInput.Location = new System.Drawing.Point(104, 390);
            this.btnInput.Margin = new System.Windows.Forms.Padding(4);
            this.btnInput.Name = "btnInput";
            this.btnInput.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnInput.Size = new System.Drawing.Size(216, 28);
            this.btnInput.StyleController = this.layoutControl;
            this.btnInput.TabIndex = 6;
            this.btnInput.Text = "입 력";
            this.btnInput.ToolTip = "저장(Ctrl + S)";
            this.btnInput.Click += new System.EventHandler(this.btnInput_Click);
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
            this.Root.Size = new System.Drawing.Size(649, 420);
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
            this.layoutControlGroup.Size = new System.Drawing.Size(649, 388);
            this.layoutControlGroup.Text = "주문오더번호를 선택하여 주세요";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridDetail;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(637, 356);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 388);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(102, 32);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btnInput;
            this.layoutControlItem2.Location = new System.Drawing.Point(102, 388);
            this.layoutControlItem2.MaxSize = new System.Drawing.Size(220, 32);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(220, 32);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(220, 32);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btn_cancel;
            this.layoutControlItem4.Location = new System.Drawing.Point(322, 388);
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
            this.emptySpaceItem2.Location = new System.Drawing.Point(542, 388);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(107, 32);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // m_SAP_INPUT_PROTRANSD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 420);
            this.Controls.Add(this.layoutControl);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "m_SAP_INPUT_PROTRANSD";
            this.Text = "주문 오더번호 선택";
            this.Load += new System.EventHandler(this.m_EBELP_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDcboScboResourceNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDcboVRKME)).EndInit();
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
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
        private DevExpress.XtraEditors.SimpleButton btnInput;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraEditors.SimpleButton btn_cancel;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraGrid.GridControl gridDetail;
        private DevExpress.XtraGrid.Views.Grid.GridView viewDetail;
        private DevExpress.XtraGrid.Columns.GridColumn detailTKNUM;
        private DevExpress.XtraGrid.Columns.GridColumn detailVBELN;
        private DevExpress.XtraGrid.Columns.GridColumn detailPOSNR;
        private DevExpress.XtraGrid.Columns.GridColumn detailMATNR;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit gridDcboScboResourceNo;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView8;
        private DevExpress.XtraGrid.Columns.GridColumn detailLFIMG;
        private DevExpress.XtraGrid.Columns.GridColumn detailVRKME;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit gridDcboVRKME;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
    }
}