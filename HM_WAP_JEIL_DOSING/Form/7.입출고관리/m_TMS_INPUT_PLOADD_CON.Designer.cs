namespace HARIM_FA_DOSING
{ 
    partial class m_TMS_INPUT_PLOADD_CON
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(m_TMS_INPUT_PLOADD_CON));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnInput = new DevExpress.XtraEditors.SimpleButton();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.gridDetail = new DevExpress.XtraGrid.GridControl();
            this.viewDetail = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ORDERNO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ORDERLINENO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TOLOCATIONCODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridcboCUSTOMER = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.FROMLOCATIONCODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ORDERTYPECODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SOLDTOCODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DELIVERYSEQUENCE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ITEMCODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridcboDRESOURCE_NO = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.PLANQTY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PD_YN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCHK = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridcboITEMCODE = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboCUSTOMER)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboDRESOURCE_NO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCHK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboITEMCODE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
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
            this.layoutControl.Size = new System.Drawing.Size(759, 420);
            this.layoutControl.TabIndex = 3;
            this.layoutControl.Text = "layoutControl1";
            // 
            // btn_cancel
            // 
            this.btn_cancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_cancel.ImageOptions.Image")));
            this.btn_cancel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_cancel.ImageOptions.ImageToTextIndent = 10;
            this.btn_cancel.Location = new System.Drawing.Point(390, 390);
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
            this.btnInput.Location = new System.Drawing.Point(170, 390);
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
            this.Root.Size = new System.Drawing.Size(759, 420);
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
            this.layoutControlGroup.Size = new System.Drawing.Size(759, 388);
            this.layoutControlGroup.Text = "주문오더번호를 선택하여 주세요";
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 388);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(168, 32);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btnInput;
            this.layoutControlItem2.Location = new System.Drawing.Point(168, 388);
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
            this.layoutControlItem4.Location = new System.Drawing.Point(388, 388);
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
            this.emptySpaceItem2.Location = new System.Drawing.Point(608, 388);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(151, 32);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // gridDetail
            // 
            this.gridDetail.Location = new System.Drawing.Point(8, 28);
            this.gridDetail.MainView = this.viewDetail;
            this.gridDetail.Name = "gridDetail";
            this.gridDetail.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.gridCHK,
            this.gridcboITEMCODE,
            this.gridcboCUSTOMER,
            this.gridcboDRESOURCE_NO});
            this.gridDetail.Size = new System.Drawing.Size(743, 352);
            this.gridDetail.TabIndex = 9;
            this.gridDetail.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewDetail});
            // 
            // viewDetail
            // 
            this.viewDetail.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ORDERNO,
            this.ORDERLINENO,
            this.TOLOCATIONCODE,
            this.FROMLOCATIONCODE,
            this.ORDERTYPECODE,
            this.SOLDTOCODE,
            this.DELIVERYSEQUENCE,
            this.ITEMCODE,
            this.PLANQTY,
            this.PD_YN});
            this.viewDetail.GridControl = this.gridDetail;
            this.viewDetail.Name = "viewDetail";
            this.viewDetail.OptionsView.ShowGroupPanel = false;
            this.viewDetail.DoubleClick += new System.EventHandler(this.viewDetail_DoubleClick);
            // 
            // ORDERNO
            // 
            this.ORDERNO.Caption = "주문번호";
            this.ORDERNO.FieldName = "ORDERNO";
            this.ORDERNO.MinWidth = 100;
            this.ORDERNO.Name = "ORDERNO";
            this.ORDERNO.Visible = true;
            this.ORDERNO.VisibleIndex = 0;
            this.ORDERNO.Width = 104;
            // 
            // ORDERLINENO
            // 
            this.ORDERLINENO.Caption = "라인번호";
            this.ORDERLINENO.FieldName = "ORDERLINENO";
            this.ORDERLINENO.MinWidth = 60;
            this.ORDERLINENO.Name = "ORDERLINENO";
            this.ORDERLINENO.Visible = true;
            this.ORDERLINENO.VisibleIndex = 1;
            this.ORDERLINENO.Width = 62;
            // 
            // TOLOCATIONCODE
            // 
            this.TOLOCATIONCODE.Caption = "배송처코드";
            this.TOLOCATIONCODE.ColumnEdit = this.gridcboCUSTOMER;
            this.TOLOCATIONCODE.FieldName = "TOLOCATIONCODE";
            this.TOLOCATIONCODE.MinWidth = 100;
            this.TOLOCATIONCODE.Name = "TOLOCATIONCODE";
            this.TOLOCATIONCODE.Visible = true;
            this.TOLOCATIONCODE.VisibleIndex = 2;
            this.TOLOCATIONCODE.Width = 104;
            // 
            // gridcboCUSTOMER
            // 
            this.gridcboCUSTOMER.AutoHeight = false;
            this.gridcboCUSTOMER.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridcboCUSTOMER.Name = "gridcboCUSTOMER";
            // 
            // FROMLOCATIONCODE
            // 
            this.FROMLOCATIONCODE.Caption = "출하센터";
            this.FROMLOCATIONCODE.ColumnEdit = this.gridcboCUSTOMER;
            this.FROMLOCATIONCODE.FieldName = "FROMLOCATIONCODE";
            this.FROMLOCATIONCODE.MaxWidth = 120;
            this.FROMLOCATIONCODE.MinWidth = 100;
            this.FROMLOCATIONCODE.Name = "FROMLOCATIONCODE";
            this.FROMLOCATIONCODE.Visible = true;
            this.FROMLOCATIONCODE.VisibleIndex = 3;
            this.FROMLOCATIONCODE.Width = 104;
            // 
            // ORDERTYPECODE
            // 
            this.ORDERTYPECODE.Caption = "오더유형코드";
            this.ORDERTYPECODE.FieldName = "ORDERTYPECODE";
            this.ORDERTYPECODE.MinWidth = 60;
            this.ORDERTYPECODE.Name = "ORDERTYPECODE";
            this.ORDERTYPECODE.Visible = true;
            this.ORDERTYPECODE.VisibleIndex = 4;
            this.ORDERTYPECODE.Width = 85;
            // 
            // SOLDTOCODE
            // 
            this.SOLDTOCODE.Caption = "거래처코드";
            this.SOLDTOCODE.ColumnEdit = this.gridcboCUSTOMER;
            this.SOLDTOCODE.FieldName = "SOLDTOCODE";
            this.SOLDTOCODE.MinWidth = 100;
            this.SOLDTOCODE.Name = "SOLDTOCODE";
            this.SOLDTOCODE.Visible = true;
            this.SOLDTOCODE.VisibleIndex = 5;
            this.SOLDTOCODE.Width = 100;
            // 
            // DELIVERYSEQUENCE
            // 
            this.DELIVERYSEQUENCE.Caption = "배송순서";
            this.DELIVERYSEQUENCE.FieldName = "DELIVERYSEQUENCE";
            this.DELIVERYSEQUENCE.MinWidth = 60;
            this.DELIVERYSEQUENCE.Name = "DELIVERYSEQUENCE";
            this.DELIVERYSEQUENCE.Visible = true;
            this.DELIVERYSEQUENCE.VisibleIndex = 6;
            this.DELIVERYSEQUENCE.Width = 60;
            // 
            // ITEMCODE
            // 
            this.ITEMCODE.Caption = "상품";
            this.ITEMCODE.ColumnEdit = this.gridcboDRESOURCE_NO;
            this.ITEMCODE.FieldName = "ITEMCODE";
            this.ITEMCODE.MinWidth = 160;
            this.ITEMCODE.Name = "ITEMCODE";
            this.ITEMCODE.Visible = true;
            this.ITEMCODE.VisibleIndex = 7;
            this.ITEMCODE.Width = 160;
            // 
            // gridcboDRESOURCE_NO
            // 
            this.gridcboDRESOURCE_NO.AutoHeight = false;
            this.gridcboDRESOURCE_NO.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridcboDRESOURCE_NO.Name = "gridcboDRESOURCE_NO";
            // 
            // PLANQTY
            // 
            this.PLANQTY.Caption = "계획수량";
            this.PLANQTY.DisplayFormat.FormatString = "{0:n0}";
            this.PLANQTY.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.PLANQTY.FieldName = "PLANQTY";
            this.PLANQTY.MinWidth = 60;
            this.PLANQTY.Name = "PLANQTY";
            this.PLANQTY.Visible = true;
            this.PLANQTY.VisibleIndex = 8;
            this.PLANQTY.Width = 60;
            // 
            // PD_YN
            // 
            this.PD_YN.Caption = "상차확인";
            this.PD_YN.ColumnEdit = this.gridCHK;
            this.PD_YN.FieldName = "PD_YN";
            this.PD_YN.MinWidth = 40;
            this.PD_YN.Name = "PD_YN";
            this.PD_YN.Visible = true;
            this.PD_YN.VisibleIndex = 9;
            this.PD_YN.Width = 40;
            // 
            // gridCHK
            // 
            this.gridCHK.AutoHeight = false;
            this.gridCHK.Name = "gridCHK";
            // 
            // gridcboITEMCODE
            // 
            this.gridcboITEMCODE.AutoHeight = false;
            this.gridcboITEMCODE.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridcboITEMCODE.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "코드"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "품목")});
            this.gridcboITEMCODE.Name = "gridcboITEMCODE";
            this.gridcboITEMCODE.NullText = "";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridDetail;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(747, 356);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // m_TMS_INPUT_PLOADD_CON
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 420);
            this.Controls.Add(this.layoutControl);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "m_TMS_INPUT_PLOADD_CON";
            this.Text = "주문 오더번호 선택";
            this.Load += new System.EventHandler(this.m_EBELP_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboCUSTOMER)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboDRESOURCE_NO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCHK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboITEMCODE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
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
        private DevExpress.XtraGrid.Columns.GridColumn ORDERNO;
        private DevExpress.XtraGrid.Columns.GridColumn ORDERLINENO;
        private DevExpress.XtraGrid.Columns.GridColumn TOLOCATIONCODE;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit gridcboCUSTOMER;
        private DevExpress.XtraGrid.Columns.GridColumn FROMLOCATIONCODE;
        private DevExpress.XtraGrid.Columns.GridColumn ORDERTYPECODE;
        private DevExpress.XtraGrid.Columns.GridColumn SOLDTOCODE;
        private DevExpress.XtraGrid.Columns.GridColumn DELIVERYSEQUENCE;
        private DevExpress.XtraGrid.Columns.GridColumn ITEMCODE;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit gridcboDRESOURCE_NO;
        private DevExpress.XtraGrid.Columns.GridColumn PLANQTY;
        private DevExpress.XtraGrid.Columns.GridColumn PD_YN;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit gridCHK;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit gridcboITEMCODE;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
    }
}