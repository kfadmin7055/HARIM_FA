namespace HARIM_FA_DOSING
{
    partial class frm_MainIngredReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_MainIngredReport));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.btn_rowDel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_rowAdd = new DevExpress.XtraEditors.SimpleButton();
            this.btn_save = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn_WORK_SEQ = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_RESOURCE_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit_RESOURCE_NO = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn_LOCATION = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_INPUT_QTY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_START_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemDateEdit_INPUT_TIME = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.gridColumn_END_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_INCAR_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_DRIVER_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_REMARK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit_EMPLOYEE_NO = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.dateEdit_workDate = new DevExpress.XtraEditors.DateEdit();
            this.btn_search = new DevExpress.XtraEditors.SimpleButton();
            this.btn_report = new DevExpress.XtraEditors.SimpleButton();
            this.memoEdit_Remark = new DevExpress.XtraEditors.MemoEdit();
            this.btn_loadData = new DevExpress.XtraEditors.SimpleButton();
            this.btn_delete = new DevExpress.XtraEditors.SimpleButton();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup_binInput = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem_workdate = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem14 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem13 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.splashScreenManager = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::HARIM_FA_DOSING.WaitForm), true, true);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit_RESOURCE_NO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit_INPUT_TIME)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit_INPUT_TIME.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit_EMPLOYEE_NO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit_Remark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup_binInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem_workdate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.btn_rowDel);
            this.layoutControl.Controls.Add(this.btn_rowAdd);
            this.layoutControl.Controls.Add(this.btn_save);
            this.layoutControl.Controls.Add(this.gridControl);
            this.layoutControl.Controls.Add(this.dateEdit_workDate);
            this.layoutControl.Controls.Add(this.btn_search);
            this.layoutControl.Controls.Add(this.btn_report);
            this.layoutControl.Controls.Add(this.memoEdit_Remark);
            this.layoutControl.Controls.Add(this.btn_loadData);
            this.layoutControl.Controls.Add(this.btn_delete);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(2134, 451, 650, 400);
            this.layoutControl.OptionsView.DrawAdornerLayer = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControl.OptionsView.DrawItemBorders = true;
            this.layoutControl.OptionsView.ShareLookAndFeelWithChildren = false;
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(1648, 900);
            this.layoutControl.TabIndex = 15;
            this.layoutControl.Text = "layoutControl1";
            // 
            // btn_rowDel
            // 
            this.btn_rowDel.AllowFocus = false;
            this.btn_rowDel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_rowDel.ImageOptions.Image")));
            this.btn_rowDel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_rowDel.Location = new System.Drawing.Point(1241, 28);
            this.btn_rowDel.Margin = new System.Windows.Forms.Padding(0);
            this.btn_rowDel.Name = "btn_rowDel";
            this.btn_rowDel.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_rowDel.Size = new System.Drawing.Size(34, 40);
            this.btn_rowDel.TabIndex = 22;
            this.btn_rowDel.ToolTip = "취소 (ESC)";
            this.btn_rowDel.Click += new System.EventHandler(this.btn_rowDel_Click);
            // 
            // btn_rowAdd
            // 
            this.btn_rowAdd.AllowFocus = false;
            this.btn_rowAdd.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_rowAdd.ImageOptions.Image")));
            this.btn_rowAdd.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btn_rowAdd.Location = new System.Drawing.Point(1200, 27);
            this.btn_rowAdd.Margin = new System.Windows.Forms.Padding(0);
            this.btn_rowAdd.Name = "btn_rowAdd";
            this.btn_rowAdd.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_rowAdd.Size = new System.Drawing.Size(36, 42);
            this.btn_rowAdd.TabIndex = 23;
            this.btn_rowAdd.ToolTip = "신규(F3)";
            this.btn_rowAdd.Click += new System.EventHandler(this.btn_rowAdd_Click);
            // 
            // btn_save
            // 
            this.btn_save.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_save.ImageOptions.Image")));
            this.btn_save.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_save.ImageOptions.ImageToTextIndent = 10;
            this.btn_save.Location = new System.Drawing.Point(1416, 28);
            this.btn_save.Margin = new System.Windows.Forms.Padding(4);
            this.btn_save.Name = "btn_save";
            this.btn_save.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_save.Size = new System.Drawing.Size(109, 40);
            this.btn_save.TabIndex = 18;
            this.btn_save.Text = "저 장";
            this.btn_save.ToolTip = "저장(Ctrl + S)";
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // gridControl
            // 
            this.gridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gridControl.Location = new System.Drawing.Point(8, 94);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gridControl.Name = "gridControl";
            this.gridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit_RESOURCE_NO,
            this.repositoryItemLookUpEdit_EMPLOYEE_NO,
            this.repositoryItemDateEdit_INPUT_TIME});
            this.gridControl.Size = new System.Drawing.Size(1632, 691);
            this.gridControl.TabIndex = 5;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            this.gridControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridControl_KeyDown);
            // 
            // gridView
            // 
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn_WORK_SEQ,
            this.gridColumn_RESOURCE_NO,
            this.gridColumn_LOCATION,
            this.gridColumn_INPUT_QTY,
            this.gridColumn_START_TIME,
            this.gridColumn_END_TIME,
            this.gridColumn_INCAR_NO,
            this.gridColumn_DRIVER_NAME,
            this.gridColumn_REMARK});
            this.gridView.DetailHeight = 404;
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            this.gridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridView.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always;
            this.gridView.OptionsFind.FindNullPrompt = "검색할 문자열을 입력해 주세요...";
            this.gridView.OptionsFind.ShowFindButton = false;
            this.gridView.OptionsSelection.CheckBoxSelectorColumnWidth = 40;
            this.gridView.OptionsSelection.MultiSelect = true;
            this.gridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.gridView.OptionsView.ColumnAutoWidth = false;
            this.gridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.gridView.OptionsView.ShowGroupPanel = false;
            this.gridView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView_CustomDrawRowIndicator);
            // 
            // gridColumn_WORK_SEQ
            // 
            this.gridColumn_WORK_SEQ.Caption = "WORK_SEQ";
            this.gridColumn_WORK_SEQ.FieldName = "WORK_SEQ";
            this.gridColumn_WORK_SEQ.Name = "gridColumn_WORK_SEQ";
            // 
            // gridColumn_RESOURCE_NO
            // 
            this.gridColumn_RESOURCE_NO.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_RESOURCE_NO.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_RESOURCE_NO.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_RESOURCE_NO.Caption = "원료명";
            this.gridColumn_RESOURCE_NO.ColumnEdit = this.repositoryItemLookUpEdit_RESOURCE_NO;
            this.gridColumn_RESOURCE_NO.FieldName = "RESOURCE_NO";
            this.gridColumn_RESOURCE_NO.Name = "gridColumn_RESOURCE_NO";
            this.gridColumn_RESOURCE_NO.Visible = true;
            this.gridColumn_RESOURCE_NO.VisibleIndex = 1;
            this.gridColumn_RESOURCE_NO.Width = 250;
            // 
            // repositoryItemLookUpEdit_RESOURCE_NO
            // 
            this.repositoryItemLookUpEdit_RESOURCE_NO.AutoHeight = false;
            this.repositoryItemLookUpEdit_RESOURCE_NO.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit_RESOURCE_NO.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "품목코드"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "품목명"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("TYPE", "품목타입")});
            this.repositoryItemLookUpEdit_RESOURCE_NO.Name = "repositoryItemLookUpEdit_RESOURCE_NO";
            // 
            // gridColumn_LOCATION
            // 
            this.gridColumn_LOCATION.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_LOCATION.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_LOCATION.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_LOCATION.Caption = "투입빈";
            this.gridColumn_LOCATION.FieldName = "LOCATION";
            this.gridColumn_LOCATION.Name = "gridColumn_LOCATION";
            this.gridColumn_LOCATION.Visible = true;
            this.gridColumn_LOCATION.VisibleIndex = 2;
            this.gridColumn_LOCATION.Width = 74;
            // 
            // gridColumn_INPUT_QTY
            // 
            this.gridColumn_INPUT_QTY.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_INPUT_QTY.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_INPUT_QTY.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_INPUT_QTY.Caption = "하차량 (KG)";
            this.gridColumn_INPUT_QTY.DisplayFormat.FormatString = "{0:n0}";
            this.gridColumn_INPUT_QTY.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn_INPUT_QTY.FieldName = "INPUT_QTY";
            this.gridColumn_INPUT_QTY.Name = "gridColumn_INPUT_QTY";
            this.gridColumn_INPUT_QTY.Visible = true;
            this.gridColumn_INPUT_QTY.VisibleIndex = 3;
            this.gridColumn_INPUT_QTY.Width = 105;
            // 
            // gridColumn_START_TIME
            // 
            this.gridColumn_START_TIME.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_START_TIME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_START_TIME.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_START_TIME.Caption = "시작시간";
            this.gridColumn_START_TIME.ColumnEdit = this.repositoryItemDateEdit_INPUT_TIME;
            this.gridColumn_START_TIME.FieldName = "START_TIME";
            this.gridColumn_START_TIME.Name = "gridColumn_START_TIME";
            this.gridColumn_START_TIME.Visible = true;
            this.gridColumn_START_TIME.VisibleIndex = 4;
            this.gridColumn_START_TIME.Width = 105;
            // 
            // repositoryItemDateEdit_INPUT_TIME
            // 
            this.repositoryItemDateEdit_INPUT_TIME.AutoHeight = false;
            this.repositoryItemDateEdit_INPUT_TIME.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit_INPUT_TIME.CalendarTimeEditing = DevExpress.Utils.DefaultBoolean.True;
            this.repositoryItemDateEdit_INPUT_TIME.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit_INPUT_TIME.DisplayFormat.FormatString = "t";
            this.repositoryItemDateEdit_INPUT_TIME.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.repositoryItemDateEdit_INPUT_TIME.EditFormat.FormatString = "t";
            this.repositoryItemDateEdit_INPUT_TIME.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.repositoryItemDateEdit_INPUT_TIME.MaskSettings.Set("mask", "t");
            this.repositoryItemDateEdit_INPUT_TIME.Name = "repositoryItemDateEdit_INPUT_TIME";
            // 
            // gridColumn_END_TIME
            // 
            this.gridColumn_END_TIME.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_END_TIME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_END_TIME.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_END_TIME.Caption = "종료시간";
            this.gridColumn_END_TIME.ColumnEdit = this.repositoryItemDateEdit_INPUT_TIME;
            this.gridColumn_END_TIME.FieldName = "END_TIME";
            this.gridColumn_END_TIME.Name = "gridColumn_END_TIME";
            this.gridColumn_END_TIME.Visible = true;
            this.gridColumn_END_TIME.VisibleIndex = 5;
            this.gridColumn_END_TIME.Width = 108;
            // 
            // gridColumn_INCAR_NO
            // 
            this.gridColumn_INCAR_NO.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_INCAR_NO.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_INCAR_NO.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_INCAR_NO.Caption = "차량번호";
            this.gridColumn_INCAR_NO.FieldName = "INCAR_NO";
            this.gridColumn_INCAR_NO.Name = "gridColumn_INCAR_NO";
            this.gridColumn_INCAR_NO.Visible = true;
            this.gridColumn_INCAR_NO.VisibleIndex = 6;
            this.gridColumn_INCAR_NO.Width = 154;
            // 
            // gridColumn_DRIVER_NAME
            // 
            this.gridColumn_DRIVER_NAME.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_DRIVER_NAME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_DRIVER_NAME.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_DRIVER_NAME.Caption = "거래처명";
            this.gridColumn_DRIVER_NAME.FieldName = "DRIVER_NAME";
            this.gridColumn_DRIVER_NAME.Name = "gridColumn_DRIVER_NAME";
            this.gridColumn_DRIVER_NAME.Visible = true;
            this.gridColumn_DRIVER_NAME.VisibleIndex = 7;
            this.gridColumn_DRIVER_NAME.Width = 196;
            // 
            // gridColumn_REMARK
            // 
            this.gridColumn_REMARK.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_REMARK.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_REMARK.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_REMARK.Caption = "특이사항";
            this.gridColumn_REMARK.FieldName = "REMARK";
            this.gridColumn_REMARK.Name = "gridColumn_REMARK";
            this.gridColumn_REMARK.Visible = true;
            this.gridColumn_REMARK.VisibleIndex = 8;
            this.gridColumn_REMARK.Width = 153;
            // 
            // repositoryItemLookUpEdit_EMPLOYEE_NO
            // 
            this.repositoryItemLookUpEdit_EMPLOYEE_NO.AutoHeight = false;
            this.repositoryItemLookUpEdit_EMPLOYEE_NO.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit_EMPLOYEE_NO.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "EMPLOYEE_NO"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "NAME")});
            this.repositoryItemLookUpEdit_EMPLOYEE_NO.Name = "repositoryItemLookUpEdit_EMPLOYEE_NO";
            this.repositoryItemLookUpEdit_EMPLOYEE_NO.ReadOnly = true;
            // 
            // dateEdit_workDate
            // 
            this.dateEdit_workDate.EditValue = null;
            this.dateEdit_workDate.Location = new System.Drawing.Point(104, 36);
            this.dateEdit_workDate.Name = "dateEdit_workDate";
            this.dateEdit_workDate.Properties.AllowFocused = false;
            this.dateEdit_workDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit_workDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit_workDate.Size = new System.Drawing.Size(128, 24);
            this.dateEdit_workDate.TabIndex = 4;
            this.dateEdit_workDate.EditValueChanged += new System.EventHandler(this.dateEdit_workDate_EditValueChanged);
            // 
            // btn_search
            // 
            this.btn_search.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_search.ImageOptions.Image")));
            this.btn_search.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_search.ImageOptions.ImageToTextIndent = 10;
            this.btn_search.Location = new System.Drawing.Point(237, 27);
            this.btn_search.Margin = new System.Windows.Forms.Padding(4);
            this.btn_search.Name = "btn_search";
            this.btn_search.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_search.Size = new System.Drawing.Size(111, 42);
            this.btn_search.TabIndex = 7;
            this.btn_search.Text = "조 회(F5)";
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // btn_report
            // 
            this.btn_report.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_report.ImageOptions.Image")));
            this.btn_report.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_report.ImageOptions.ImageToTextIndent = 10;
            this.btn_report.Location = new System.Drawing.Point(1531, 28);
            this.btn_report.Name = "btn_report";
            this.btn_report.Size = new System.Drawing.Size(109, 40);
            this.btn_report.TabIndex = 19;
            this.btn_report.Text = "일지 출력";
            this.btn_report.Click += new System.EventHandler(this.btn_report_Click);
            // 
            // memoEdit_Remark
            // 
            this.memoEdit_Remark.Location = new System.Drawing.Point(8, 811);
            this.memoEdit_Remark.Name = "memoEdit_Remark";
            this.memoEdit_Remark.Size = new System.Drawing.Size(1037, 81);
            this.memoEdit_Remark.TabIndex = 24;
            // 
            // btn_loadData
            // 
            this.btn_loadData.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_loadData.ImageOptions.Image")));
            this.btn_loadData.Location = new System.Drawing.Point(1059, 28);
            this.btn_loadData.Name = "btn_loadData";
            this.btn_loadData.Size = new System.Drawing.Size(126, 40);
            this.btn_loadData.TabIndex = 25;
            this.btn_loadData.Text = "하차내역 불러오기";
            this.btn_loadData.Click += new System.EventHandler(this.btn_loadData_Click);
            // 
            // btn_delete
            // 
            this.btn_delete.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_delete.ImageOptions.Image")));
            this.btn_delete.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_delete.ImageOptions.ImageToTextIndent = 10;
            this.btn_delete.Location = new System.Drawing.Point(1300, 27);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(111, 42);
            this.btn_delete.TabIndex = 26;
            this.btn_delete.Text = "삭제";
            this.btn_delete.ToolTip = "삭제(Ctrl + D)";
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // Root
            // 
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup_binInput});
            this.Root.Name = "Root";
            this.Root.OptionsPrint.AppearanceItem.BorderColor = System.Drawing.Color.Black;
            this.Root.OptionsPrint.AppearanceItem.Options.UseBorderColor = true;
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.Root.Size = new System.Drawing.Size(1648, 900);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup_binInput
            // 
            this.layoutControlGroup_binInput.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlGroup_binInput.CaptionImageOptions.Image")));
            this.layoutControlGroup_binInput.CustomizationFormText = "포장 작업지시";
            this.layoutControlGroup_binInput.GroupStyle = DevExpress.Utils.GroupStyle.Card;
            this.layoutControlGroup_binInput.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem_workdate,
            this.layoutControlItem7,
            this.emptySpaceItem1,
            this.layoutControlItem14,
            this.layoutControlItem13,
            this.layoutControlItem8,
            this.layoutControlItem9,
            this.layoutControlGroup1,
            this.layoutControlGroup2,
            this.layoutControlItem3,
            this.layoutControlItem4});
            this.layoutControlGroup_binInput.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup_binInput.Name = "layoutControlGroup_binInput";
            this.layoutControlGroup_binInput.Padding = new DevExpress.XtraLayout.Utils.Padding(1, 1, 1, 1);
            this.layoutControlGroup_binInput.Size = new System.Drawing.Size(1646, 898);
            this.layoutControlGroup_binInput.Text = "주원료 하차일지";
            // 
            // layoutControlItem_workdate
            // 
            this.layoutControlItem_workdate.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem_workdate.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem_workdate.Control = this.dateEdit_workDate;
            this.layoutControlItem_workdate.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem_workdate.CustomizationFormText = "작업일자";
            this.layoutControlItem_workdate.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlItem_workdate.ImageOptions.Image")));
            this.layoutControlItem_workdate.ImageOptions.ImageToTextDistance = 10;
            this.layoutControlItem_workdate.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem_workdate.MaxSize = new System.Drawing.Size(230, 40);
            this.layoutControlItem_workdate.MinSize = new System.Drawing.Size(230, 40);
            this.layoutControlItem_workdate.Name = "layoutControlItem_workdate";
            this.layoutControlItem_workdate.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 3, 3);
            this.layoutControlItem_workdate.Size = new System.Drawing.Size(230, 46);
            this.layoutControlItem_workdate.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem_workdate.Text = "하차일자";
            this.layoutControlItem_workdate.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem_workdate.TextSize = new System.Drawing.Size(74, 16);
            this.layoutControlItem_workdate.TextToControlDistance = 15;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.btn_search;
            this.layoutControlItem7.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem7.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem7.Location = new System.Drawing.Point(230, 0);
            this.layoutControlItem7.MaxSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem7.MinSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem7.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem7.Text = "layoutControlItem3";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(345, 0);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(706, 46);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem14
            // 
            this.layoutControlItem14.Control = this.btn_report;
            this.layoutControlItem14.Location = new System.Drawing.Point(1523, 0);
            this.layoutControlItem14.MaxSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem14.MinSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem14.Name = "layoutControlItem14";
            this.layoutControlItem14.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlItem14.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem14.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem14.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem14.TextVisible = false;
            // 
            // layoutControlItem13
            // 
            this.layoutControlItem13.Control = this.btn_save;
            this.layoutControlItem13.Location = new System.Drawing.Point(1408, 0);
            this.layoutControlItem13.MaxSize = new System.Drawing.Size(125, 46);
            this.layoutControlItem13.MinSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlItem13.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem13.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem13.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem13.TextVisible = false;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.btn_rowDel;
            this.layoutControlItem8.Location = new System.Drawing.Point(1233, 0);
            this.layoutControlItem8.MaxSize = new System.Drawing.Size(60, 46);
            this.layoutControlItem8.MinSize = new System.Drawing.Size(60, 46);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 23, 3, 3);
            this.layoutControlItem8.Size = new System.Drawing.Size(60, 46);
            this.layoutControlItem8.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem8.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem8.TextVisible = false;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.btn_rowAdd;
            this.layoutControlItem9.Location = new System.Drawing.Point(1193, 0);
            this.layoutControlItem9.MaxSize = new System.Drawing.Size(40, 46);
            this.layoutControlItem9.MinSize = new System.Drawing.Size(40, 46);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Size = new System.Drawing.Size(40, 46);
            this.layoutControlItem9.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem9.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem9.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 763);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Size = new System.Drawing.Size(1638, 107);
            this.layoutControlGroup1.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Text = "2. 특이사항";
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.memoEdit_Remark;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.MaxSize = new System.Drawing.Size(1041, 85);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(1041, 85);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(1636, 85);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.Tag = "LINE";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 46);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup2.Size = new System.Drawing.Size(1638, 717);
            this.layoutControlGroup2.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup2.Text = "1. 주원료 작업 현황";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1636, 695);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btn_loadData;
            this.layoutControlItem3.Location = new System.Drawing.Point(1051, 0);
            this.layoutControlItem3.MinSize = new System.Drawing.Size(142, 28);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 13, 3, 3);
            this.layoutControlItem3.Size = new System.Drawing.Size(142, 46);
            this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btn_delete;
            this.layoutControlItem4.Location = new System.Drawing.Point(1293, 0);
            this.layoutControlItem4.MaxSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem4.MinSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // splashScreenManager
            // 
            this.splashScreenManager.ClosingDelay = 500;
            // 
            // frm_MainIngredReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1648, 900);
            this.Controls.Add(this.layoutControl);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("frm_MainIngredReport.IconOptions.Image")));
            this.Name = "frm_MainIngredReport";
            this.Text = "주원료 하차일지";
            this.Load += new System.EventHandler(this.frm_MainIngredReport_Load);
            this.Shown += new System.EventHandler(this.frm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit_RESOURCE_NO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit_INPUT_TIME.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit_INPUT_TIME)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit_EMPLOYEE_NO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit_Remark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup_binInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem_workdate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraEditors.SimpleButton btn_rowDel;
        private DevExpress.XtraEditors.SimpleButton btn_rowAdd;
        private DevExpress.XtraEditors.SimpleButton btn_save;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_WORK_SEQ;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_RESOURCE_NO;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit_INPUT_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_LOCATION;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_START_TIME;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit_RESOURCE_NO;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_END_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_INCAR_NO;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit_EMPLOYEE_NO;
        private DevExpress.XtraEditors.DateEdit dateEdit_workDate;
        private DevExpress.XtraEditors.SimpleButton btn_search;
        private DevExpress.XtraEditors.SimpleButton btn_report;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup_binInput;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem_workdate;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem14;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem13;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
        private DevExpress.XtraEditors.MemoEdit memoEdit_Remark;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_DRIVER_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_REMARK;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager;
        private DevExpress.XtraEditors.SimpleButton btn_loadData;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_INPUT_QTY;
        private DevExpress.XtraEditors.SimpleButton btn_delete;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
    }
}