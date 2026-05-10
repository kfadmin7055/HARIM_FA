using System;

namespace HARIM_FA_DOSING
{
    partial class frm_IngredDayUseResult
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
            DevExpress.XtraPivotGrid.DataSourceColumnBinding dataSourceColumnBinding1 = new DevExpress.XtraPivotGrid.DataSourceColumnBinding();
            DevExpress.XtraPivotGrid.DataSourceColumnBinding dataSourceColumnBinding2 = new DevExpress.XtraPivotGrid.DataSourceColumnBinding();
            DevExpress.XtraPivotGrid.DataSourceColumnBinding dataSourceColumnBinding3 = new DevExpress.XtraPivotGrid.DataSourceColumnBinding();
            DevExpress.XtraPivotGrid.DataSourceColumnBinding dataSourceColumnBinding4 = new DevExpress.XtraPivotGrid.DataSourceColumnBinding();
            DevExpress.XtraPivotGrid.DataSourceColumnBinding dataSourceColumnBinding5 = new DevExpress.XtraPivotGrid.DataSourceColumnBinding();
            DevExpress.XtraPivotGrid.DataSourceColumnBinding dataSourceColumnBinding6 = new DevExpress.XtraPivotGrid.DataSourceColumnBinding();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_IngredDayUseResult));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.pivotGridControl = new DevExpress.XtraPivotGrid.PivotGridControl();
            this.pivotGridField_INGRED_LOT = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField_NAME = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField_SET_VAL = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField_P_Q = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField_DIV_VAL = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField_WORK_START_DATE = new DevExpress.XtraPivotGrid.PivotGridField();
            this.dateEdit_workEndDate = new DevExpress.XtraEditors.DateEdit();
            this.dateEdit_workStartDate = new DevExpress.XtraEditors.DateEdit();
            this.btn_search = new DevExpress.XtraEditors.SimpleButton();
            this.btn_excelExport = new DevExpress.XtraEditors.SimpleButton();
            this.cboPlant_Code = new DevExpress.XtraEditors.LookUpEdit();
            this.cboL_Code = new DevExpress.XtraEditors.LookUpEdit();
            this.cboProcess_Key = new DevExpress.XtraEditors.LookUpEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem_workdate = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.simpleLabelItem1 = new DevExpress.XtraLayout.SimpleLabelItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem13 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.gridColumn_INGRED_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_TYPE_DESC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_P_Q = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_UOM_DESC = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pivotGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workEndDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workStartDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workStartDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPlant_Code.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboL_Code.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProcess_Key.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem_workdate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.pivotGridControl);
            this.layoutControl.Controls.Add(this.dateEdit_workEndDate);
            this.layoutControl.Controls.Add(this.dateEdit_workStartDate);
            this.layoutControl.Controls.Add(this.btn_search);
            this.layoutControl.Controls.Add(this.btn_excelExport);
            this.layoutControl.Controls.Add(this.cboPlant_Code);
            this.layoutControl.Controls.Add(this.cboL_Code);
            this.layoutControl.Controls.Add(this.cboProcess_Key);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(1760, 620);
            this.layoutControl.TabIndex = 11;
            this.layoutControl.Text = "layoutControl1";
            // 
            // pivotGridControl
            // 
            this.pivotGridControl.Appearance.HeaderArea.Options.UseTextOptions = true;
            this.pivotGridControl.Appearance.HeaderArea.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pivotGridControl.Appearance.HeaderArea.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.pivotGridControl.Fields.AddRange(new DevExpress.XtraPivotGrid.PivotGridField[] {
            this.pivotGridField_INGRED_LOT,
            this.pivotGridField_NAME,
            this.pivotGridField_SET_VAL,
            this.pivotGridField_P_Q,
            this.pivotGridField_DIV_VAL,
            this.pivotGridField_WORK_START_DATE});
            this.pivotGridControl.Location = new System.Drawing.Point(6, 72);
            this.pivotGridControl.Name = "pivotGridControl";
            this.pivotGridControl.OptionsCustomization.CustomizationFormSearchBoxVisible = true;
            this.pivotGridControl.OptionsData.DataProcessingEngine = DevExpress.XtraPivotGrid.PivotDataProcessingEngine.Optimized;
            this.pivotGridControl.Size = new System.Drawing.Size(1748, 542);
            this.pivotGridControl.TabIndex = 9;
            this.pivotGridControl.CustomCellValue += new System.EventHandler<DevExpress.XtraPivotGrid.PivotCellValueEventArgs>(this.pivotGridControl1_CustomCellValue);
            this.pivotGridControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridControl_KeyDown);
            // 
            // pivotGridField_INGRED_LOT
            // 
            this.pivotGridField_INGRED_LOT.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.pivotGridField_INGRED_LOT.AreaIndex = 0;
            this.pivotGridField_INGRED_LOT.Caption = "원료코드";
            dataSourceColumnBinding1.ColumnName = "INGRED_LOT";
            this.pivotGridField_INGRED_LOT.DataBinding = dataSourceColumnBinding1;
            this.pivotGridField_INGRED_LOT.Name = "pivotGridField_INGRED_LOT";
            // 
            // pivotGridField_NAME
            // 
            this.pivotGridField_NAME.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.pivotGridField_NAME.AreaIndex = 1;
            this.pivotGridField_NAME.Caption = "사용원료";
            dataSourceColumnBinding2.ColumnName = "NAME";
            this.pivotGridField_NAME.DataBinding = dataSourceColumnBinding2;
            this.pivotGridField_NAME.Name = "pivotGridField_NAME";
            // 
            // pivotGridField_SET_VAL
            // 
            this.pivotGridField_SET_VAL.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.pivotGridField_SET_VAL.AreaIndex = 0;
            this.pivotGridField_SET_VAL.Caption = "설정값(KG)";
            this.pivotGridField_SET_VAL.CellFormat.FormatString = "{0:n3}";
            this.pivotGridField_SET_VAL.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dataSourceColumnBinding3.ColumnName = "SET_VAL";
            this.pivotGridField_SET_VAL.DataBinding = dataSourceColumnBinding3;
            this.pivotGridField_SET_VAL.Name = "pivotGridField_SET_VAL";
            // 
            // pivotGridField_P_Q
            // 
            this.pivotGridField_P_Q.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.pivotGridField_P_Q.AreaIndex = 1;
            this.pivotGridField_P_Q.Caption = "사용량 (KG)";
            this.pivotGridField_P_Q.CellFormat.FormatString = "{0:n3}";
            this.pivotGridField_P_Q.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dataSourceColumnBinding4.ColumnName = "P_Q";
            this.pivotGridField_P_Q.DataBinding = dataSourceColumnBinding4;
            this.pivotGridField_P_Q.Name = "pivotGridField_P_Q";
            // 
            // pivotGridField_DIV_VAL
            // 
            this.pivotGridField_DIV_VAL.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.pivotGridField_DIV_VAL.AreaIndex = 2;
            this.pivotGridField_DIV_VAL.Caption = "편차(KG)";
            this.pivotGridField_DIV_VAL.CellFormat.FormatString = "{0:n3}";
            this.pivotGridField_DIV_VAL.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dataSourceColumnBinding5.ColumnName = "DIV_VAL";
            this.pivotGridField_DIV_VAL.DataBinding = dataSourceColumnBinding5;
            this.pivotGridField_DIV_VAL.Name = "pivotGridField_DIV_VAL";
            // 
            // pivotGridField_WORK_START_DATE
            // 
            this.pivotGridField_WORK_START_DATE.Appearance.Cell.Options.UseTextOptions = true;
            this.pivotGridField_WORK_START_DATE.Appearance.Cell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pivotGridField_WORK_START_DATE.Appearance.Cell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.pivotGridField_WORK_START_DATE.Appearance.CellGrandTotal.Options.UseTextOptions = true;
            this.pivotGridField_WORK_START_DATE.Appearance.CellGrandTotal.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pivotGridField_WORK_START_DATE.Appearance.CellGrandTotal.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.pivotGridField_WORK_START_DATE.Appearance.Header.Options.UseTextOptions = true;
            this.pivotGridField_WORK_START_DATE.Appearance.Header.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pivotGridField_WORK_START_DATE.Appearance.Header.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.pivotGridField_WORK_START_DATE.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.pivotGridField_WORK_START_DATE.AreaIndex = 0;
            this.pivotGridField_WORK_START_DATE.Caption = "일자";
            dataSourceColumnBinding6.ColumnName = "WORK_START_DATE";
            this.pivotGridField_WORK_START_DATE.DataBinding = dataSourceColumnBinding6;
            this.pivotGridField_WORK_START_DATE.MinWidth = 80;
            this.pivotGridField_WORK_START_DATE.Name = "pivotGridField_WORK_START_DATE";
            // 
            // dateEdit_workEndDate
            // 
            this.dateEdit_workEndDate.EditValue = null;
            this.dateEdit_workEndDate.Location = new System.Drawing.Point(858, 35);
            this.dateEdit_workEndDate.Name = "dateEdit_workEndDate";
            this.dateEdit_workEndDate.Properties.AllowFocused = false;
            this.dateEdit_workEndDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit_workEndDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit_workEndDate.Size = new System.Drawing.Size(111, 24);
            this.dateEdit_workEndDate.StyleController = this.layoutControl;
            this.dateEdit_workEndDate.TabIndex = 8;
            this.dateEdit_workEndDate.EditValueChanged += new System.EventHandler(this.dateEdit_workEndDate_EditValueChanged);
            // 
            // dateEdit_workStartDate
            // 
            this.dateEdit_workStartDate.EditValue = null;
            this.dateEdit_workStartDate.Location = new System.Drawing.Point(713, 35);
            this.dateEdit_workStartDate.Name = "dateEdit_workStartDate";
            this.dateEdit_workStartDate.Properties.AllowFocused = false;
            this.dateEdit_workStartDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit_workStartDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit_workStartDate.Size = new System.Drawing.Size(126, 24);
            this.dateEdit_workStartDate.StyleController = this.layoutControl;
            this.dateEdit_workStartDate.TabIndex = 4;
            this.dateEdit_workStartDate.EditValueChanged += new System.EventHandler(this.dateEdit_workStartDate_EditValueChanged);
            // 
            // btn_search
            // 
            this.btn_search.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_search.ImageOptions.Image")));
            this.btn_search.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_search.ImageOptions.ImageToTextIndent = 10;
            this.btn_search.Location = new System.Drawing.Point(973, 26);
            this.btn_search.Margin = new System.Windows.Forms.Padding(4);
            this.btn_search.Name = "btn_search";
            this.btn_search.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_search.Size = new System.Drawing.Size(111, 42);
            this.btn_search.StyleController = this.layoutControl;
            this.btn_search.TabIndex = 7;
            this.btn_search.Text = "조 회(F5)";
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // btn_excelExport
            // 
            this.btn_excelExport.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_excelExport.ImageOptions.Image")));
            this.btn_excelExport.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_excelExport.ImageOptions.ImageToTextIndent = 10;
            this.btn_excelExport.Location = new System.Drawing.Point(1627, 26);
            this.btn_excelExport.Name = "btn_excelExport";
            this.btn_excelExport.Size = new System.Drawing.Size(127, 42);
            this.btn_excelExport.StyleController = this.layoutControl;
            this.btn_excelExport.TabIndex = 10;
            this.btn_excelExport.Text = "엑셀 내보내기";
            this.btn_excelExport.Click += new System.EventHandler(this.btn_excelExport_Click);
            // 
            // cboPlant_Code
            // 
            this.cboPlant_Code.Location = new System.Drawing.Point(47, 35);
            this.cboPlant_Code.Name = "cboPlant_Code";
            this.cboPlant_Code.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboPlant_Code.Size = new System.Drawing.Size(195, 24);
            this.cboPlant_Code.StyleController = this.layoutControl;
            this.cboPlant_Code.TabIndex = 23;
            this.cboPlant_Code.EditValueChanged += new System.EventHandler(this.cboPlant_Code_EditValueChanged);
            // 
            // cboL_Code
            // 
            this.cboL_Code.Location = new System.Drawing.Point(499, 35);
            this.cboL_Code.Name = "cboL_Code";
            this.cboL_Code.Properties.AllowFocused = false;
            this.cboL_Code.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboL_Code.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "코드", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("공정", "공정")});
            this.cboL_Code.Properties.NullText = "";
            this.cboL_Code.Properties.PopupSizeable = false;
            this.cboL_Code.Size = new System.Drawing.Size(122, 24);
            this.cboL_Code.StyleController = this.layoutControl;
            this.cboL_Code.TabIndex = 5;
            this.cboL_Code.EditValueChanged += new System.EventHandler(this.cboL_Code_EditValueChanged);
            // 
            // cboProcess_Key
            // 
            this.cboProcess_Key.Location = new System.Drawing.Point(295, 35);
            this.cboProcess_Key.Name = "cboProcess_Key";
            this.cboProcess_Key.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboProcess_Key.Size = new System.Drawing.Size(137, 24);
            this.cboProcess_Key.StyleController = this.layoutControl;
            this.cboProcess_Key.TabIndex = 23;
            this.cboProcess_Key.EditValueChanged += new System.EventHandler(this.cboProcess_Key_EditValueChanged);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup1});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.Root.Size = new System.Drawing.Size(1760, 620);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlGroup1.CaptionImageOptions.Image")));
            this.layoutControlGroup1.CustomizationFormText = "포장 작업지시";
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem_workdate,
            this.layoutControlItem4,
            this.layoutControlItem2,
            this.simpleLabelItem1,
            this.emptySpaceItem2,
            this.layoutControlItem1,
            this.layoutControlItem7,
            this.layoutControlItem13,
            this.layoutControlItem5,
            this.layoutControlItem6});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(1, 1, 1, 1);
            this.layoutControlGroup1.Size = new System.Drawing.Size(1760, 620);
            this.layoutControlGroup1.Text = "일자별 원료사용내역";
            // 
            // layoutControlItem_workdate
            // 
            this.layoutControlItem_workdate.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem_workdate.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem_workdate.Control = this.dateEdit_workStartDate;
            this.layoutControlItem_workdate.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem_workdate.CustomizationFormText = "작업일자";
            this.layoutControlItem_workdate.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlItem_workdate.ImageOptions.Image")));
            this.layoutControlItem_workdate.ImageOptions.ImageToTextDistance = 10;
            this.layoutControlItem_workdate.Location = new System.Drawing.Point(620, 0);
            this.layoutControlItem_workdate.MaxSize = new System.Drawing.Size(218, 46);
            this.layoutControlItem_workdate.MinSize = new System.Drawing.Size(218, 46);
            this.layoutControlItem_workdate.Name = "layoutControlItem_workdate";
            this.layoutControlItem_workdate.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 3, 3);
            this.layoutControlItem_workdate.Size = new System.Drawing.Size(218, 46);
            this.layoutControlItem_workdate.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem_workdate.Text = "작업일자";
            this.layoutControlItem_workdate.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem_workdate.TextSize = new System.Drawing.Size(74, 16);
            this.layoutControlItem_workdate.TextToControlDistance = 5;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.pivotGridControl;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 46);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(1752, 546);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem2.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem2.Control = this.dateEdit_workEndDate;
            this.layoutControlItem2.Location = new System.Drawing.Point(852, 0);
            this.layoutControlItem2.MaxSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // simpleLabelItem1
            // 
            this.simpleLabelItem1.AllowHotTrack = false;
            this.simpleLabelItem1.AppearanceItemCaption.Options.UseTextOptions = true;
            this.simpleLabelItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.simpleLabelItem1.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.simpleLabelItem1.Location = new System.Drawing.Point(838, 0);
            this.simpleLabelItem1.MaxSize = new System.Drawing.Size(14, 46);
            this.simpleLabelItem1.MinSize = new System.Drawing.Size(14, 46);
            this.simpleLabelItem1.Name = "simpleLabelItem1";
            this.simpleLabelItem1.Size = new System.Drawing.Size(14, 46);
            this.simpleLabelItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.simpleLabelItem1.Text = "~";
            this.simpleLabelItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.simpleLabelItem1.TextSize = new System.Drawing.Size(8, 15);
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(1082, 0);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(539, 46);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.btn_excelExport;
            this.layoutControlItem1.Location = new System.Drawing.Point(1621, 0);
            this.layoutControlItem1.MaxSize = new System.Drawing.Size(131, 46);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(131, 46);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(131, 46);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.btn_search;
            this.layoutControlItem7.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem7.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem7.Location = new System.Drawing.Point(967, 0);
            this.layoutControlItem7.MaxSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem7.MinSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem7.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem7.Text = "layoutControlItem3";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // layoutControlItem13
            // 
            this.layoutControlItem13.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem13.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem13.Control = this.cboPlant_Code;
            this.layoutControlItem13.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem13.CustomizationFormText = "플랜트";
            this.layoutControlItem13.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem13.MaxSize = new System.Drawing.Size(240, 46);
            this.layoutControlItem13.MinSize = new System.Drawing.Size(240, 46);
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Size = new System.Drawing.Size(240, 46);
            this.layoutControlItem13.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem13.Text = "플랜트";
            this.layoutControlItem13.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem13.TextSize = new System.Drawing.Size(36, 15);
            this.layoutControlItem13.TextToControlDistance = 5;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem5.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem5.Control = this.cboL_Code;
            this.layoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem5.CustomizationFormText = "진행상태";
            this.layoutControlItem5.Location = new System.Drawing.Point(430, 0);
            this.layoutControlItem5.MaxSize = new System.Drawing.Size(190, 46);
            this.layoutControlItem5.MinSize = new System.Drawing.Size(190, 46);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 3, 3, 3);
            this.layoutControlItem5.Size = new System.Drawing.Size(190, 46);
            this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem5.Spacing = new DevExpress.XtraLayout.Utils.Padding(20, 0, 0, 0);
            this.layoutControlItem5.Text = "라인";
            this.layoutControlItem5.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem5.TextSize = new System.Drawing.Size(24, 15);
            this.layoutControlItem5.TextToControlDistance = 11;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem6.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem6.Control = this.cboProcess_Key;
            this.layoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem6.CustomizationFormText = "공정";
            this.layoutControlItem6.Location = new System.Drawing.Point(240, 0);
            this.layoutControlItem6.MaxSize = new System.Drawing.Size(190, 46);
            this.layoutControlItem6.MinSize = new System.Drawing.Size(190, 46);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(190, 46);
            this.layoutControlItem6.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem6.Spacing = new DevExpress.XtraLayout.Utils.Padding(20, 0, 0, 0);
            this.layoutControlItem6.Text = "공정";
            this.layoutControlItem6.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem6.TextSize = new System.Drawing.Size(24, 15);
            this.layoutControlItem6.TextToControlDistance = 5;
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.Name = "sqlDataSource1";
            // 
            // gridColumn_INGRED_CODE
            // 
            this.gridColumn_INGRED_CODE.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_INGRED_CODE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_INGRED_CODE.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_INGRED_CODE.Caption = "원료코드";
            this.gridColumn_INGRED_CODE.FieldName = "INGRED_CODE";
            this.gridColumn_INGRED_CODE.Name = "gridColumn_INGRED_CODE";
            this.gridColumn_INGRED_CODE.OptionsColumn.ReadOnly = true;
            this.gridColumn_INGRED_CODE.Visible = true;
            this.gridColumn_INGRED_CODE.VisibleIndex = 0;
            this.gridColumn_INGRED_CODE.Width = 166;
            // 
            // gridColumn_NAME
            // 
            this.gridColumn_NAME.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_NAME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_NAME.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_NAME.Caption = "원료명";
            this.gridColumn_NAME.FieldName = "NAME";
            this.gridColumn_NAME.MinWidth = 16;
            this.gridColumn_NAME.Name = "gridColumn_NAME";
            this.gridColumn_NAME.OptionsColumn.ReadOnly = true;
            this.gridColumn_NAME.Visible = true;
            this.gridColumn_NAME.VisibleIndex = 1;
            this.gridColumn_NAME.Width = 296;
            // 
            // gridColumn_TYPE_DESC
            // 
            this.gridColumn_TYPE_DESC.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_TYPE_DESC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_TYPE_DESC.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_TYPE_DESC.Caption = "원료타입";
            this.gridColumn_TYPE_DESC.FieldName = "TYPE_DESC";
            this.gridColumn_TYPE_DESC.Name = "gridColumn_TYPE_DESC";
            this.gridColumn_TYPE_DESC.OptionsColumn.ReadOnly = true;
            this.gridColumn_TYPE_DESC.Visible = true;
            this.gridColumn_TYPE_DESC.VisibleIndex = 2;
            this.gridColumn_TYPE_DESC.Width = 130;
            // 
            // gridColumn_P_Q
            // 
            this.gridColumn_P_Q.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_P_Q.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_P_Q.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_P_Q.Caption = "원료사용량 (KG)";
            this.gridColumn_P_Q.DisplayFormat.FormatString = "{0:n3}";
            this.gridColumn_P_Q.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn_P_Q.FieldName = "P_Q";
            this.gridColumn_P_Q.Name = "gridColumn_P_Q";
            this.gridColumn_P_Q.OptionsColumn.ReadOnly = true;
            this.gridColumn_P_Q.Visible = true;
            this.gridColumn_P_Q.VisibleIndex = 3;
            this.gridColumn_P_Q.Width = 128;
            // 
            // gridColumn_UOM_DESC
            // 
            this.gridColumn_UOM_DESC.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_UOM_DESC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_UOM_DESC.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_UOM_DESC.Caption = "원료사용단위";
            this.gridColumn_UOM_DESC.FieldName = "UOM_DESC";
            this.gridColumn_UOM_DESC.Name = "gridColumn_UOM_DESC";
            this.gridColumn_UOM_DESC.OptionsColumn.ReadOnly = true;
            this.gridColumn_UOM_DESC.Width = 115;
            // 
            // frm_IngredDayUseResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1760, 620);
            this.Controls.Add(this.layoutControl);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("frm_IngredDayUseResult.IconOptions.Image")));
            this.Name = "frm_IngredDayUseResult";
            this.Text = "일자별 원료사용내역";
            this.Load += new System.EventHandler(this.frm_IngredDayUseResult_Load);
            this.Shown += new System.EventHandler(this.frm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pivotGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workEndDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workStartDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit_workStartDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPlant_Code.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboL_Code.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProcess_Key.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem_workdate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleLabelItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraEditors.DateEdit dateEdit_workEndDate;
        private DevExpress.XtraEditors.DateEdit dateEdit_workStartDate;
        private DevExpress.XtraEditors.SimpleButton btn_search;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem_workdate;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.SimpleLabelItem simpleLabelItem1;
        private DevExpress.XtraPivotGrid.PivotGridControl pivotGridControl;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_INGRED_CODE;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_TYPE_DESC;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_P_Q;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_UOM_DESC;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField_NAME;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField_P_Q;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField_WORK_START_DATE;
        private DevExpress.XtraEditors.SimpleButton btn_excelExport;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField_INGRED_LOT;
        private DevExpress.XtraEditors.LookUpEdit cboPlant_Code;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem13;
        private DevExpress.XtraEditors.LookUpEdit cboL_Code;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraEditors.LookUpEdit cboProcess_Key;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField_SET_VAL;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField_DIV_VAL;
    }
}