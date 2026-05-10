namespace HARIM_FA_DOSING
{
    partial class frm_BulkReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_BulkReport));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn_OUT_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_CUST_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_VEHICLENO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_PART_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_P_Q = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_LOCATION = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_START_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_END_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_REMARK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit_RESOURCE_NO = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemLookUpEdit_EMPLOYEE_NO = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemDateEdit_INPUT_TIME = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.dateEdit_workDate = new DevExpress.XtraEditors.DateEdit();
            this.btn_search = new DevExpress.XtraEditors.SimpleButton();
            this.btn_excelExport = new DevExpress.XtraEditors.SimpleButton();
            this.cboPlant_Code = new DevExpress.XtraEditors.LookUpEdit();
            this.cboL_Code = new DevExpress.XtraEditors.LookUpEdit();
            this.cboProcessKey = new DevExpress.XtraEditors.LookUpEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup_binInput = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem_workdate = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem14 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem13 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit_RESOURCE_NO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit_EMPLOYEE_NO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit_INPUT_TIME)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit_INPUT_TIME.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPlant_Code.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboL_Code.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProcessKey.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup_binInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem_workdate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.gridControl);
            this.layoutControl.Controls.Add(this.dateEdit_workDate);
            this.layoutControl.Controls.Add(this.btn_search);
            this.layoutControl.Controls.Add(this.btn_excelExport);
            this.layoutControl.Controls.Add(this.cboPlant_Code);
            this.layoutControl.Controls.Add(this.cboL_Code);
            this.layoutControl.Controls.Add(this.cboProcessKey);
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
            this.layoutControl.TabIndex = 16;
            this.layoutControl.Text = "layoutControl1";
            // 
            // gridControl
            // 
            this.gridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gridControl.Location = new System.Drawing.Point(6, 72);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gridControl.Name = "gridControl";
            this.gridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit_RESOURCE_NO,
            this.repositoryItemLookUpEdit_EMPLOYEE_NO,
            this.repositoryItemDateEdit_INPUT_TIME});
            this.gridControl.Size = new System.Drawing.Size(1636, 822);
            this.gridControl.TabIndex = 5;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            this.gridControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridControl_KeyDown);
            // 
            // gridView
            // 
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn_OUT_TYPE,
            this.gridColumn_CUST_NAME,
            this.gridColumn_VEHICLENO,
            this.gridColumn_PART_NAME,
            this.gridColumn_P_Q,
            this.gridColumn_LOCATION,
            this.gridColumn_START_TIME,
            this.gridColumn_END_TIME,
            this.gridColumn_REMARK});
            this.gridView.DetailHeight = 404;
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            this.gridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridView.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always;
            this.gridView.OptionsFind.FindNullPrompt = "검색할 문자열을 입력해 주세요...";
            this.gridView.OptionsFind.ShowFindButton = false;
            this.gridView.OptionsView.ColumnAutoWidth = false;
            this.gridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.gridView.OptionsView.ShowGroupPanel = false;
            this.gridView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView_CustomDrawRowIndicator);
            // 
            // gridColumn_OUT_TYPE
            // 
            this.gridColumn_OUT_TYPE.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_OUT_TYPE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_OUT_TYPE.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_OUT_TYPE.Caption = "상차종류";
            this.gridColumn_OUT_TYPE.FieldName = "OUT_TYPE";
            this.gridColumn_OUT_TYPE.Name = "gridColumn_OUT_TYPE";
            this.gridColumn_OUT_TYPE.Visible = true;
            this.gridColumn_OUT_TYPE.VisibleIndex = 0;
            this.gridColumn_OUT_TYPE.Width = 78;
            // 
            // gridColumn_CUST_NAME
            // 
            this.gridColumn_CUST_NAME.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_CUST_NAME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_CUST_NAME.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_CUST_NAME.Caption = "거래처명";
            this.gridColumn_CUST_NAME.FieldName = "CUST_NAME";
            this.gridColumn_CUST_NAME.Name = "gridColumn_CUST_NAME";
            this.gridColumn_CUST_NAME.Visible = true;
            this.gridColumn_CUST_NAME.VisibleIndex = 1;
            this.gridColumn_CUST_NAME.Width = 191;
            // 
            // gridColumn_VEHICLENO
            // 
            this.gridColumn_VEHICLENO.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_VEHICLENO.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_VEHICLENO.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_VEHICLENO.Caption = "차량번호";
            this.gridColumn_VEHICLENO.FieldName = "VEHICLENO";
            this.gridColumn_VEHICLENO.Name = "gridColumn_VEHICLENO";
            this.gridColumn_VEHICLENO.Visible = true;
            this.gridColumn_VEHICLENO.VisibleIndex = 2;
            this.gridColumn_VEHICLENO.Width = 123;
            // 
            // gridColumn_PART_NAME
            // 
            this.gridColumn_PART_NAME.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_PART_NAME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_PART_NAME.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_PART_NAME.Caption = "상차제품명";
            this.gridColumn_PART_NAME.FieldName = "PART_NAME";
            this.gridColumn_PART_NAME.Name = "gridColumn_PART_NAME";
            this.gridColumn_PART_NAME.Visible = true;
            this.gridColumn_PART_NAME.VisibleIndex = 3;
            this.gridColumn_PART_NAME.Width = 219;
            // 
            // gridColumn_P_Q
            // 
            this.gridColumn_P_Q.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_P_Q.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_P_Q.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_P_Q.Caption = "상차량 (KG)";
            this.gridColumn_P_Q.DisplayFormat.FormatString = "{0:n0} KG";
            this.gridColumn_P_Q.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn_P_Q.FieldName = "P_Q";
            this.gridColumn_P_Q.Name = "gridColumn_P_Q";
            this.gridColumn_P_Q.Visible = true;
            this.gridColumn_P_Q.VisibleIndex = 4;
            this.gridColumn_P_Q.Width = 96;
            // 
            // gridColumn_LOCATION
            // 
            this.gridColumn_LOCATION.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_LOCATION.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_LOCATION.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_LOCATION.Caption = "인출빈";
            this.gridColumn_LOCATION.FieldName = "LOCATION";
            this.gridColumn_LOCATION.Name = "gridColumn_LOCATION";
            this.gridColumn_LOCATION.Visible = true;
            this.gridColumn_LOCATION.VisibleIndex = 5;
            this.gridColumn_LOCATION.Width = 67;
            // 
            // gridColumn_START_TIME
            // 
            this.gridColumn_START_TIME.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_START_TIME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_START_TIME.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_START_TIME.Caption = "상차시작시간";
            this.gridColumn_START_TIME.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.gridColumn_START_TIME.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn_START_TIME.FieldName = "START_TIME";
            this.gridColumn_START_TIME.Name = "gridColumn_START_TIME";
            this.gridColumn_START_TIME.Visible = true;
            this.gridColumn_START_TIME.VisibleIndex = 6;
            this.gridColumn_START_TIME.Width = 176;
            // 
            // gridColumn_END_TIME
            // 
            this.gridColumn_END_TIME.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_END_TIME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_END_TIME.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_END_TIME.Caption = "상차완료시간";
            this.gridColumn_END_TIME.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.gridColumn_END_TIME.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn_END_TIME.FieldName = "END_TIME";
            this.gridColumn_END_TIME.Name = "gridColumn_END_TIME";
            this.gridColumn_END_TIME.Visible = true;
            this.gridColumn_END_TIME.VisibleIndex = 7;
            this.gridColumn_END_TIME.Width = 171;
            // 
            // gridColumn_REMARK
            // 
            this.gridColumn_REMARK.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_REMARK.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_REMARK.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_REMARK.Caption = "배송메모";
            this.gridColumn_REMARK.FieldName = "REMARK";
            this.gridColumn_REMARK.Name = "gridColumn_REMARK";
            this.gridColumn_REMARK.Visible = true;
            this.gridColumn_REMARK.VisibleIndex = 8;
            this.gridColumn_REMARK.Width = 153;
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
            // dateEdit_workDate
            // 
            this.dateEdit_workDate.EditValue = null;
            this.dateEdit_workDate.Location = new System.Drawing.Point(671, 35);
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
            this.btn_search.Location = new System.Drawing.Point(804, 26);
            this.btn_search.Margin = new System.Windows.Forms.Padding(4);
            this.btn_search.Name = "btn_search";
            this.btn_search.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_search.Size = new System.Drawing.Size(111, 42);
            this.btn_search.TabIndex = 7;
            this.btn_search.Text = "조 회(F5)";
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // btn_excelExport
            // 
            this.btn_excelExport.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_excelExport.ImageOptions.Image")));
            this.btn_excelExport.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_excelExport.ImageOptions.ImageToTextIndent = 10;
            this.btn_excelExport.Location = new System.Drawing.Point(939, 26);
            this.btn_excelExport.Name = "btn_excelExport";
            this.btn_excelExport.Size = new System.Drawing.Size(111, 42);
            this.btn_excelExport.TabIndex = 19;
            this.btn_excelExport.Text = "엑셀 내보내기";
            this.btn_excelExport.Click += new System.EventHandler(this.btn_excelExport_Click);
            // 
            // cboPlant_Code
            // 
            this.cboPlant_Code.Location = new System.Drawing.Point(47, 35);
            this.cboPlant_Code.Name = "cboPlant_Code";
            this.cboPlant_Code.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboPlant_Code.Size = new System.Drawing.Size(215, 24);
            this.cboPlant_Code.TabIndex = 23;
            this.cboPlant_Code.EditValueChanged += new System.EventHandler(this.cboPlant_Code_EditValueChanged);
            // 
            // cboL_Code
            // 
            this.cboL_Code.Location = new System.Drawing.Point(459, 35);
            this.cboL_Code.Name = "cboL_Code";
            this.cboL_Code.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboL_Code.Size = new System.Drawing.Size(111, 24);
            this.cboL_Code.TabIndex = 23;
            this.cboL_Code.EditValueChanged += new System.EventHandler(this.cboL_Code_EditValueChanged);
            // 
            // cboProcessKey
            // 
            this.cboProcessKey.Location = new System.Drawing.Point(315, 35);
            this.cboProcessKey.Name = "cboProcessKey";
            this.cboProcessKey.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboProcessKey.Size = new System.Drawing.Size(91, 24);
            this.cboProcessKey.TabIndex = 23;
            this.cboProcessKey.EditValueChanged += new System.EventHandler(this.cboProcessKey_EditValueChanged);
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
            this.layoutControlItem1,
            this.layoutControlItem14,
            this.layoutControlItem13,
            this.layoutControlItem4,
            this.layoutControlItem6});
            this.layoutControlGroup_binInput.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup_binInput.Name = "layoutControlGroup_binInput";
            this.layoutControlGroup_binInput.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup_binInput.Size = new System.Drawing.Size(1646, 898);
            this.layoutControlGroup_binInput.Text = "벌크 상차일지";
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
            this.layoutControlItem_workdate.Location = new System.Drawing.Point(568, 0);
            this.layoutControlItem_workdate.MaxSize = new System.Drawing.Size(230, 40);
            this.layoutControlItem_workdate.MinSize = new System.Drawing.Size(230, 40);
            this.layoutControlItem_workdate.Name = "layoutControlItem_workdate";
            this.layoutControlItem_workdate.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 3, 3);
            this.layoutControlItem_workdate.Size = new System.Drawing.Size(230, 46);
            this.layoutControlItem_workdate.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem_workdate.Text = "상차일자";
            this.layoutControlItem_workdate.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem_workdate.TextSize = new System.Drawing.Size(74, 16);
            this.layoutControlItem_workdate.TextToControlDistance = 15;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.btn_search;
            this.layoutControlItem7.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem7.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem7.Location = new System.Drawing.Point(798, 0);
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
            this.emptySpaceItem1.Location = new System.Drawing.Point(1048, 0);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(592, 46);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 46);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1640, 826);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem14
            // 
            this.layoutControlItem14.Control = this.btn_excelExport;
            this.layoutControlItem14.Location = new System.Drawing.Point(913, 0);
            this.layoutControlItem14.MaxSize = new System.Drawing.Size(135, 46);
            this.layoutControlItem14.MinSize = new System.Drawing.Size(135, 46);
            this.layoutControlItem14.Name = "layoutControlItem14";
            this.layoutControlItem14.Size = new System.Drawing.Size(135, 46);
            this.layoutControlItem14.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem14.Spacing = new DevExpress.XtraLayout.Utils.Padding(20, 0, 0, 0);
            this.layoutControlItem14.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem14.TextVisible = false;
            // 
            // layoutControlItem13
            // 
            this.layoutControlItem13.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem13.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem13.Control = this.cboPlant_Code;
            this.layoutControlItem13.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem13.CustomizationFormText = "플랜트";
            this.layoutControlItem13.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem13.MaxSize = new System.Drawing.Size(260, 46);
            this.layoutControlItem13.MinSize = new System.Drawing.Size(260, 46);
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Size = new System.Drawing.Size(260, 46);
            this.layoutControlItem13.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem13.Text = "플랜트";
            this.layoutControlItem13.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem13.TextSize = new System.Drawing.Size(36, 15);
            this.layoutControlItem13.TextToControlDistance = 5;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem4.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem4.Control = this.cboL_Code;
            this.layoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem4.CustomizationFormText = "라인";
            this.layoutControlItem4.Location = new System.Drawing.Point(404, 0);
            this.layoutControlItem4.MaxSize = new System.Drawing.Size(164, 46);
            this.layoutControlItem4.MinSize = new System.Drawing.Size(164, 46);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(164, 46);
            this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem4.Spacing = new DevExpress.XtraLayout.Utils.Padding(20, 0, 0, 0);
            this.layoutControlItem4.Text = "라인";
            this.layoutControlItem4.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem4.TextSize = new System.Drawing.Size(24, 15);
            this.layoutControlItem4.TextToControlDistance = 5;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem6.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem6.Control = this.cboProcessKey;
            this.layoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem6.CustomizationFormText = "공정";
            this.layoutControlItem6.Location = new System.Drawing.Point(260, 0);
            this.layoutControlItem6.MaxSize = new System.Drawing.Size(144, 46);
            this.layoutControlItem6.MinSize = new System.Drawing.Size(144, 46);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(144, 46);
            this.layoutControlItem6.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem6.Spacing = new DevExpress.XtraLayout.Utils.Padding(20, 0, 0, 0);
            this.layoutControlItem6.Text = "공정";
            this.layoutControlItem6.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem6.TextSize = new System.Drawing.Size(24, 15);
            this.layoutControlItem6.TextToControlDistance = 5;
            // 
            // frm_BulkReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1648, 900);
            this.Controls.Add(this.layoutControl);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("frm_BulkReport.IconOptions.Image")));
            this.Name = "frm_BulkReport";
            this.Text = "벌크 상차일지";
            this.Load += new System.EventHandler(this.frm_BulkReport_Load);
            this.Shown += new System.EventHandler(this.frm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit_RESOURCE_NO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit_EMPLOYEE_NO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit_INPUT_TIME.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit_INPUT_TIME)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPlant_Code.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboL_Code.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProcessKey.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup_binInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem_workdate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_CUST_NAME;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit_RESOURCE_NO;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_VEHICLENO;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_PART_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_P_Q;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit_INPUT_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_LOCATION;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_START_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_END_TIME;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_REMARK;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit_EMPLOYEE_NO;
        private DevExpress.XtraEditors.DateEdit dateEdit_workDate;
        private DevExpress.XtraEditors.SimpleButton btn_search;
        private DevExpress.XtraEditors.SimpleButton btn_excelExport;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup_binInput;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem_workdate;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem14;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_OUT_TYPE;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.LookUpEdit cboPlant_Code;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem13;
        private DevExpress.XtraEditors.LookUpEdit cboL_Code;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.LookUpEdit cboProcessKey;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
    }
}