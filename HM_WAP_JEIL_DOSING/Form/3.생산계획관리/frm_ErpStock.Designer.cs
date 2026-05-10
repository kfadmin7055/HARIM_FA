namespace HARIM_FA_DOSING
{
    partial class frm_ErpStock
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_ErpStock));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.lblRemark = new DevExpress.XtraEditors.LabelControl();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.WERKS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridcboPLANT = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.LGORT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridcboLocation = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.BKLAS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MATNR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridcboRESOURCE_NO = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.CHARG = new DevExpress.XtraGrid.Columns.GridColumn();
            this.LABST = new DevExpress.XtraGrid.Columns.GridColumn();
            this.INSME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SPEME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MEINS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridUNIT = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.I_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btn_rowAdd = new DevExpress.XtraEditors.SimpleButton();
            this.btn_delete = new DevExpress.XtraEditors.SimpleButton();
            this.btn_save = new DevExpress.XtraEditors.SimpleButton();
            this.btn_rowDel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_search = new DevExpress.XtraEditors.SimpleButton();
            this.cboPlant_Code = new DevExpress.XtraEditors.LookUpEdit();
            this.cboLocation = new DevExpress.XtraEditors.LookUpEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem12 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.splashScreenManager = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::HARIM_FA_DOSING.WaitForm), true, false);
            this.timerRemark = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboPLANT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboLocation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboRESOURCE_NO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridUNIT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPlant_Code.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLocation.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.lblRemark);
            this.layoutControl.Controls.Add(this.gridControl);
            this.layoutControl.Controls.Add(this.btn_rowAdd);
            this.layoutControl.Controls.Add(this.btn_delete);
            this.layoutControl.Controls.Add(this.btn_save);
            this.layoutControl.Controls.Add(this.btn_rowDel);
            this.layoutControl.Controls.Add(this.btn_search);
            this.layoutControl.Controls.Add(this.cboPlant_Code);
            this.layoutControl.Controls.Add(this.cboLocation);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(1760, 620);
            this.layoutControl.TabIndex = 3;
            this.layoutControl.Text = "layoutControl1";
            // 
            // lblRemark
            // 
            this.lblRemark.Location = new System.Drawing.Point(612, 53);
            this.lblRemark.Name = "lblRemark";
            this.lblRemark.Size = new System.Drawing.Size(211, 15);
            this.lblRemark.StyleController = this.layoutControl;
            this.lblRemark.TabIndex = 24;
            this.lblRemark.Text = "ERP에 재고 정보 갱신을 요청 중입니다.";
            // 
            // gridControl
            // 
            this.gridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gridControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gridControl.Location = new System.Drawing.Point(6, 72);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gridControl.Name = "gridControl";
            this.gridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.gridcboRESOURCE_NO,
            this.gridcboLocation,
            this.gridcboPLANT,
            this.gridUNIT});
            this.gridControl.Size = new System.Drawing.Size(1748, 542);
            this.gridControl.TabIndex = 5;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            this.gridControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridControl_KeyDown);
            // 
            // gridView
            // 
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.WERKS,
            this.LGORT,
            this.BKLAS,
            this.MATNR,
            this.CHARG,
            this.LABST,
            this.INSME,
            this.SPEME,
            this.MEINS,
            this.I_TIME});
            this.gridView.DetailHeight = 404;
            this.gridView.GridControl = this.gridControl;
            this.gridView.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "QTY_ON_HAND", null, "합계 : {0:n2}"),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "QTY_ON_HAND", this.CHARG, "{0:n2}")});
            this.gridView.Name = "gridView";
            this.gridView.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always;
            this.gridView.OptionsFind.FindNullPrompt = "검색할 문자열을 입력해 주세요...";
            this.gridView.OptionsFind.ShowFindButton = false;
            this.gridView.OptionsView.AllowCellMerge = true;
            this.gridView.OptionsView.ColumnAutoWidth = false;
            this.gridView.OptionsView.EnableAppearanceOddRow = true;
            this.gridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.gridView.OptionsView.ShowFooter = true;
            this.gridView.OptionsView.ShowGroupPanel = false;
            this.gridView.OptionsView.WaitAnimationOptions = DevExpress.XtraEditors.WaitAnimationOptions.Indicator;
            this.gridView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView_CustomDrawRowIndicator);
            // 
            // WERKS
            // 
            this.WERKS.AppearanceHeader.Options.UseTextOptions = true;
            this.WERKS.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.WERKS.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.WERKS.Caption = "플랜트";
            this.WERKS.ColumnEdit = this.gridcboPLANT;
            this.WERKS.FieldName = "WERKS";
            this.WERKS.Name = "WERKS";
            this.WERKS.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.WERKS.Visible = true;
            this.WERKS.VisibleIndex = 0;
            this.WERKS.Width = 160;
            // 
            // gridcboPLANT
            // 
            this.gridcboPLANT.AutoHeight = false;
            this.gridcboPLANT.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridcboPLANT.Name = "gridcboPLANT";
            // 
            // LGORT
            // 
            this.LGORT.Caption = "저장위치";
            this.LGORT.ColumnEdit = this.gridcboLocation;
            this.LGORT.FieldName = "LGORT";
            this.LGORT.Name = "LGORT";
            this.LGORT.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.LGORT.Visible = true;
            this.LGORT.VisibleIndex = 1;
            this.LGORT.Width = 180;
            // 
            // gridcboLocation
            // 
            this.gridcboLocation.AutoHeight = false;
            this.gridcboLocation.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridcboLocation.Name = "gridcboLocation";
            // 
            // BKLAS
            // 
            this.BKLAS.Caption = "평가 클래스";
            this.BKLAS.FieldName = "BKLAS";
            this.BKLAS.Name = "BKLAS";
            this.BKLAS.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.BKLAS.Visible = true;
            this.BKLAS.VisibleIndex = 2;
            // 
            // MATNR
            // 
            this.MATNR.AppearanceHeader.Options.UseTextOptions = true;
            this.MATNR.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.MATNR.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.MATNR.Caption = "품목";
            this.MATNR.ColumnEdit = this.gridcboRESOURCE_NO;
            this.MATNR.FieldName = "MATNR";
            this.MATNR.MinWidth = 180;
            this.MATNR.Name = "MATNR";
            this.MATNR.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.MATNR.Visible = true;
            this.MATNR.VisibleIndex = 3;
            this.MATNR.Width = 260;
            // 
            // gridcboRESOURCE_NO
            // 
            this.gridcboRESOURCE_NO.AutoHeight = false;
            this.gridcboRESOURCE_NO.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridcboRESOURCE_NO.Name = "gridcboRESOURCE_NO";
            // 
            // CHARG
            // 
            this.CHARG.AppearanceHeader.Options.UseTextOptions = true;
            this.CHARG.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.CHARG.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.CHARG.Caption = "배치번호";
            this.CHARG.DisplayFormat.FormatString = "{0:n2}";
            this.CHARG.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.CHARG.FieldName = "CHARG";
            this.CHARG.Name = "CHARG";
            this.CHARG.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.CHARG.Visible = true;
            this.CHARG.VisibleIndex = 4;
            this.CHARG.Width = 112;
            // 
            // LABST
            // 
            this.LABST.AppearanceHeader.Options.UseTextOptions = true;
            this.LABST.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LABST.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.LABST.Caption = "가용재고";
            this.LABST.DisplayFormat.FormatString = "{0:n2}";
            this.LABST.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.LABST.FieldName = "LABST";
            this.LABST.Name = "LABST";
            this.LABST.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.LABST.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "LABST", "{0:n0} KG")});
            this.LABST.Visible = true;
            this.LABST.VisibleIndex = 5;
            this.LABST.Width = 95;
            // 
            // INSME
            // 
            this.INSME.AppearanceHeader.Options.UseTextOptions = true;
            this.INSME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.INSME.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.INSME.Caption = "품질중재고";
            this.INSME.DisplayFormat.FormatString = "{0:n2}";
            this.INSME.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.INSME.FieldName = "INSME";
            this.INSME.Name = "INSME";
            this.INSME.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.INSME.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "INSME", "{0:n0} KG")});
            this.INSME.Visible = true;
            this.INSME.VisibleIndex = 6;
            this.INSME.Width = 124;
            // 
            // SPEME
            // 
            this.SPEME.Caption = "보류재고";
            this.SPEME.DisplayFormat.FormatString = "{0:n2}";
            this.SPEME.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.SPEME.FieldName = "SPEME";
            this.SPEME.Name = "SPEME";
            this.SPEME.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.SPEME.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "SPEME", "{0:n0} KG")});
            this.SPEME.Visible = true;
            this.SPEME.VisibleIndex = 7;
            // 
            // MEINS
            // 
            this.MEINS.Caption = "기본단위";
            this.MEINS.ColumnEdit = this.gridUNIT;
            this.MEINS.FieldName = "MEINS";
            this.MEINS.Name = "MEINS";
            this.MEINS.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.MEINS.Visible = true;
            this.MEINS.VisibleIndex = 8;
            // 
            // gridUNIT
            // 
            this.gridUNIT.AutoHeight = false;
            this.gridUNIT.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridUNIT.Name = "gridUNIT";
            // 
            // I_TIME
            // 
            this.I_TIME.Caption = "등록일자";
            this.I_TIME.FieldName = "I_TIME";
            this.I_TIME.Name = "I_TIME";
            this.I_TIME.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.I_TIME.Visible = true;
            this.I_TIME.VisibleIndex = 9;
            this.I_TIME.Width = 160;
            // 
            // btn_rowAdd
            // 
            this.btn_rowAdd.AllowFocus = false;
            this.btn_rowAdd.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_rowAdd.ImageOptions.Image")));
            this.btn_rowAdd.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btn_rowAdd.Location = new System.Drawing.Point(1429, 27);
            this.btn_rowAdd.Margin = new System.Windows.Forms.Padding(0);
            this.btn_rowAdd.Name = "btn_rowAdd";
            this.btn_rowAdd.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_rowAdd.Size = new System.Drawing.Size(34, 40);
            this.btn_rowAdd.StyleController = this.layoutControl;
            this.btn_rowAdd.TabIndex = 21;
            this.btn_rowAdd.ToolTip = "신규(F3)";
            this.btn_rowAdd.Click += new System.EventHandler(this.btn_rowAdd_Click);
            // 
            // btn_delete
            // 
            this.btn_delete.Appearance.ForeColor = System.Drawing.Color.Red;
            this.btn_delete.Appearance.Options.UseForeColor = true;
            this.btn_delete.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_delete.ImageOptions.Image")));
            this.btn_delete.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_delete.ImageOptions.ImageToTextIndent = 10;
            this.btn_delete.Location = new System.Drawing.Point(1644, 27);
            this.btn_delete.Margin = new System.Windows.Forms.Padding(4);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_delete.Size = new System.Drawing.Size(109, 40);
            this.btn_delete.StyleController = this.layoutControl;
            this.btn_delete.TabIndex = 8;
            this.btn_delete.Text = "삭 제";
            this.btn_delete.ToolTip = "삭제(Ctrl + D)";
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // btn_save
            // 
            this.btn_save.AllowFocus = false;
            this.btn_save.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_save.ImageOptions.Image")));
            this.btn_save.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_save.ImageOptions.ImageToTextIndent = 10;
            this.btn_save.Location = new System.Drawing.Point(1528, 26);
            this.btn_save.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_save.Name = "btn_save";
            this.btn_save.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_save.Size = new System.Drawing.Size(111, 42);
            this.btn_save.StyleController = this.layoutControl;
            this.btn_save.TabIndex = 7;
            this.btn_save.Text = "저 장";
            this.btn_save.ToolTip = "저장(Ctrl + S)";
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_rowDel
            // 
            this.btn_rowDel.AllowFocus = false;
            this.btn_rowDel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_rowDel.ImageOptions.Image")));
            this.btn_rowDel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btn_rowDel.Location = new System.Drawing.Point(1469, 27);
            this.btn_rowDel.Margin = new System.Windows.Forms.Padding(0);
            this.btn_rowDel.Name = "btn_rowDel";
            this.btn_rowDel.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_rowDel.Size = new System.Drawing.Size(34, 40);
            this.btn_rowDel.StyleController = this.layoutControl;
            this.btn_rowDel.TabIndex = 20;
            this.btn_rowDel.ToolTip = "취소 (ESC)";
            this.btn_rowDel.Click += new System.EventHandler(this.btn_rowDel_Click);
            // 
            // btn_search
            // 
            this.btn_search.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_search.ImageOptions.Image")));
            this.btn_search.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_search.ImageOptions.ImageToTextIndent = 10;
            this.btn_search.Location = new System.Drawing.Point(447, 27);
            this.btn_search.Margin = new System.Windows.Forms.Padding(4);
            this.btn_search.Name = "btn_search";
            this.btn_search.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_search.Size = new System.Drawing.Size(160, 40);
            this.btn_search.StyleController = this.layoutControl;
            this.btn_search.TabIndex = 7;
            this.btn_search.Text = "ERP 재고 조 회(F5)";
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // cboPlant_Code
            // 
            this.cboPlant_Code.Location = new System.Drawing.Point(47, 35);
            this.cboPlant_Code.Name = "cboPlant_Code";
            this.cboPlant_Code.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboPlant_Code.Size = new System.Drawing.Size(175, 24);
            this.cboPlant_Code.StyleController = this.layoutControl;
            this.cboPlant_Code.TabIndex = 23;
            this.cboPlant_Code.EditValueChanged += new System.EventHandler(this.cboPlant_Code_EditValueChanged);
            // 
            // cboLocation
            // 
            this.cboLocation.Location = new System.Drawing.Point(279, 35);
            this.cboLocation.Name = "cboLocation";
            this.cboLocation.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboLocation.Size = new System.Drawing.Size(163, 24);
            this.cboLocation.StyleController = this.layoutControl;
            this.cboLocation.TabIndex = 23;
            this.cboLocation.EditValueChanged += new System.EventHandler(this.cboLocation_EditValueChanged);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.Root.Size = new System.Drawing.Size(1760, 620);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup
            // 
            this.layoutControlGroup.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlGroup.CaptionImageOptions.Image")));
            this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem8,
            this.layoutControlItem3,
            this.layoutControlItem10,
            this.layoutControlItem11,
            this.layoutControlItem5,
            this.layoutControlItem12,
            this.layoutControlItem2,
            this.emptySpaceItem3,
            this.emptySpaceItem4,
            this.emptySpaceItem1,
            this.layoutControlItem4});
            this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup.Name = "layoutControlGroup";
            this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(1, 1, 1, 1);
            this.layoutControlGroup.Size = new System.Drawing.Size(1760, 620);
            this.layoutControlGroup.Text = "SAP 원료 및 제품 재고 관리";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 46);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1752, 546);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.btn_delete;
            this.layoutControlItem8.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem8.CustomizationFormText = "layoutControlItem4";
            this.layoutControlItem8.Location = new System.Drawing.Point(1637, 0);
            this.layoutControlItem8.MaxSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem8.MinSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlItem8.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem8.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem8.Text = "layoutControlItem4";
            this.layoutControlItem8.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem8.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btn_save;
            this.layoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem3.Location = new System.Drawing.Point(1522, 0);
            this.layoutControlItem3.MaxSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem3.MinSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this.btn_rowDel;
            this.layoutControlItem10.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem10.CustomizationFormText = "layoutControlItem10";
            this.layoutControlItem10.Location = new System.Drawing.Point(1462, 0);
            this.layoutControlItem10.MinSize = new System.Drawing.Size(60, 34);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 23, 3, 3);
            this.layoutControlItem10.Size = new System.Drawing.Size(60, 46);
            this.layoutControlItem10.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem10.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem10.TextVisible = false;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Control = this.btn_rowAdd;
            this.layoutControlItem11.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem11.CustomizationFormText = "layoutControlItem11";
            this.layoutControlItem11.Location = new System.Drawing.Point(1422, 0);
            this.layoutControlItem11.MaxSize = new System.Drawing.Size(40, 46);
            this.layoutControlItem11.MinSize = new System.Drawing.Size(40, 46);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlItem11.Size = new System.Drawing.Size(40, 46);
            this.layoutControlItem11.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem11.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem11.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btn_search;
            this.layoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem5.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem5.Location = new System.Drawing.Point(440, 0);
            this.layoutControlItem5.MinSize = new System.Drawing.Size(122, 34);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlItem5.Size = new System.Drawing.Size(166, 46);
            this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem5.Text = "layoutControlItem3";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem12
            // 
            this.layoutControlItem12.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem12.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem12.Control = this.cboPlant_Code;
            this.layoutControlItem12.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem12.CustomizationFormText = "플랜트";
            this.layoutControlItem12.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem12.MaxSize = new System.Drawing.Size(220, 46);
            this.layoutControlItem12.MinSize = new System.Drawing.Size(220, 46);
            this.layoutControlItem12.Name = "layoutControlItem12";
            this.layoutControlItem12.Size = new System.Drawing.Size(220, 46);
            this.layoutControlItem12.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem12.Text = "플랜트";
            this.layoutControlItem12.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem12.TextSize = new System.Drawing.Size(36, 15);
            this.layoutControlItem12.TextToControlDistance = 5;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem2.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem2.Control = this.cboLocation;
            this.layoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem2.CustomizationFormText = "플랜트";
            this.layoutControlItem2.Location = new System.Drawing.Point(220, 0);
            this.layoutControlItem2.MaxSize = new System.Drawing.Size(220, 46);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(220, 46);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(220, 46);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.Text = "저장위치";
            this.layoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(48, 15);
            this.layoutControlItem2.TextToControlDistance = 5;
            // 
            // emptySpaceItem3
            // 
            this.emptySpaceItem3.AllowHotTrack = false;
            this.emptySpaceItem3.Location = new System.Drawing.Point(606, 0);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(314, 27);
            this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem4
            // 
            this.emptySpaceItem4.AllowHotTrack = false;
            this.emptySpaceItem4.Location = new System.Drawing.Point(821, 27);
            this.emptySpaceItem4.Name = "emptySpaceItem4";
            this.emptySpaceItem4.Size = new System.Drawing.Size(99, 19);
            this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(920, 0);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(502, 46);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.lblRemark;
            this.layoutControlItem4.Location = new System.Drawing.Point(606, 27);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(215, 19);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // splashScreenManager
            // 
            this.splashScreenManager.ClosingDelay = 500;
            // 
            // timerRemark
            // 
            this.timerRemark.Interval = 500;
            this.timerRemark.Tick += new System.EventHandler(this.timerRemark_Tick);
            // 
            // frm_ErpStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1760, 620);
            this.Controls.Add(this.layoutControl);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("frm_ErpStock.IconOptions.Image")));
            this.Name = "frm_ErpStock";
            this.Text = "SAP 원료 및 제품 재고 관리";
            this.Load += new System.EventHandler(this.frm_ErpStock_Load);
            this.Shown += new System.EventHandler(this.frm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboPLANT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboLocation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboRESOURCE_NO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridUNIT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPlant_Code.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLocation.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraGrid.Columns.GridColumn WERKS;
        private DevExpress.XtraGrid.Columns.GridColumn MATNR;
        private DevExpress.XtraGrid.Columns.GridColumn CHARG;
        private DevExpress.XtraGrid.Columns.GridColumn LABST;
        private DevExpress.XtraGrid.Columns.GridColumn INSME;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraGrid.Columns.GridColumn BKLAS;
        private DevExpress.XtraGrid.Columns.GridColumn LGORT;
        private DevExpress.XtraGrid.Columns.GridColumn SPEME;
        private DevExpress.XtraGrid.Columns.GridColumn MEINS;
        private DevExpress.XtraGrid.Columns.GridColumn I_TIME;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit gridcboRESOURCE_NO;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit gridcboLocation;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit gridcboPLANT;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit gridUNIT;
        private DevExpress.XtraEditors.SimpleButton btn_rowAdd;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem11;
        private DevExpress.XtraEditors.SimpleButton btn_delete;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraEditors.SimpleButton btn_save;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.SimpleButton btn_rowDel;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem10;
        private DevExpress.XtraEditors.SimpleButton btn_search;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraEditors.LookUpEdit cboPlant_Code;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem12;
        private DevExpress.XtraEditors.LookUpEdit cboLocation;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
        private System.Windows.Forms.Timer timerRemark;
        private DevExpress.XtraEditors.LabelControl lblRemark;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
    }
}