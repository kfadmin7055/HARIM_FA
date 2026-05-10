using DevExpress.XtraGrid.Views.Base;
using System;

namespace HARIM_FA_DOSING
{
    partial class frm_Scale
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Scale));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.txtName = new DevExpress.XtraEditors.TextEdit();
            this.btnPLCDownLoad = new DevExpress.XtraEditors.SimpleButton();
            this.btnPLCUpload = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn_CHK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCHK = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.PLANT_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridscboPlant = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.repositoryItemSearchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.PROCESS_KEY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridcboPROCESS_KEY = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn_L_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridcboL_CODE = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn_SCALE_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_SCALE_NO = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_SCALE_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_MAX_Q = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_ER_Q = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_W_WAIT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_W_REQ = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_W_STP = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_R_WAIT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_R_REQ = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_R_STP = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_MAX_HZ = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_IN_SCALE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCboIN_SCALE = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn_PLC_ADDRESS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_STD = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_SDD = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_I_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridsCboPLC_ADDRESS = new DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btn_save = new DevExpress.XtraEditors.SimpleButton();
            this.btn_delete = new DevExpress.XtraEditors.SimpleButton();
            this.btn_rowDel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_rowAdd = new DevExpress.XtraEditors.SimpleButton();
            this.btn_search = new DevExpress.XtraEditors.SimpleButton();
            this.cboProcessKey = new DevExpress.XtraEditors.LookUpEdit();
            this.cboPlant_Code = new DevExpress.XtraEditors.LookUpEdit();
            this.cboL_Code = new DevExpress.XtraEditors.LookUpEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem_plcRead = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem_plcSend = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem12 = new DevExpress.XtraLayout.LayoutControlItem();
            this.splashScreenManager = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::HARIM_FA_DOSING.WaitForm), true, false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCHK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridscboPlant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboPROCESS_KEY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboL_CODE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCboIN_SCALE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridsCboPLC_ADDRESS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProcessKey.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPlant_Code.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboL_Code.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem_plcRead)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem_plcSend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.txtName);
            this.layoutControl.Controls.Add(this.btnPLCDownLoad);
            this.layoutControl.Controls.Add(this.btnPLCUpload);
            this.layoutControl.Controls.Add(this.gridControl);
            this.layoutControl.Controls.Add(this.btn_save);
            this.layoutControl.Controls.Add(this.btn_delete);
            this.layoutControl.Controls.Add(this.btn_rowDel);
            this.layoutControl.Controls.Add(this.btn_rowAdd);
            this.layoutControl.Controls.Add(this.btn_search);
            this.layoutControl.Controls.Add(this.cboProcessKey);
            this.layoutControl.Controls.Add(this.cboPlant_Code);
            this.layoutControl.Controls.Add(this.cboL_Code);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(2578, 264, 879, 681);
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(1760, 620);
            this.layoutControl.TabIndex = 1;
            this.layoutControl.Text = "layoutControl1";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(589, 37);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(77, 24);
            this.txtName.StyleController = this.layoutControl;
            this.txtName.TabIndex = 22;
            // 
            // btnPLCDownLoad
            // 
            this.btnPLCDownLoad.AllowFocus = false;
            this.btnPLCDownLoad.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnPLCDownLoad.ImageOptions.Image")));
            this.btnPLCDownLoad.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnPLCDownLoad.ImageOptions.ImageToTextIndent = 10;
            this.btnPLCDownLoad.Location = new System.Drawing.Point(1095, 28);
            this.btnPLCDownLoad.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPLCDownLoad.Name = "btnPLCDownLoad";
            this.btnPLCDownLoad.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnPLCDownLoad.Size = new System.Drawing.Size(114, 42);
            this.btnPLCDownLoad.StyleController = this.layoutControl;
            this.btnPLCDownLoad.TabIndex = 12;
            this.btnPLCDownLoad.Text = "PLC 읽어오기";
            this.btnPLCDownLoad.Click += new System.EventHandler(this.btnPLCDownLoad_Click);
            // 
            // btnPLCUpload
            // 
            this.btnPLCUpload.AllowFocus = false;
            this.btnPLCUpload.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnPLCUpload.ImageOptions.Image")));
            this.btnPLCUpload.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnPLCUpload.ImageOptions.ImageToTextIndent = 10;
            this.btnPLCUpload.Location = new System.Drawing.Point(977, 28);
            this.btnPLCUpload.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPLCUpload.Name = "btnPLCUpload";
            this.btnPLCUpload.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnPLCUpload.Size = new System.Drawing.Size(114, 42);
            this.btnPLCUpload.StyleController = this.layoutControl;
            this.btnPLCUpload.TabIndex = 11;
            this.btnPLCUpload.Text = "PLC에 전송";
            this.btnPLCUpload.Click += new System.EventHandler(this.btnPLCUpload_Click);
            // 
            // gridControl
            // 
            this.gridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.gridControl.Location = new System.Drawing.Point(7, 73);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gridControl.Name = "gridControl";
            this.gridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.gridCHK,
            this.gridscboPlant,
            this.gridcboPROCESS_KEY,
            this.gridcboL_CODE,
            this.gridsCboPLC_ADDRESS,
            this.gridCboIN_SCALE});
            this.gridControl.Size = new System.Drawing.Size(1746, 540);
            this.gridControl.TabIndex = 5;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            this.gridControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridControl_KeyDown);
            // 
            // gridView
            // 
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn_CHK,
            this.PLANT_CODE,
            this.PROCESS_KEY,
            this.gridColumn_L_CODE,
            this.gridColumn_SCALE_CODE,
            this.gridColumn_SCALE_NO,
            this.gridColumn_SCALE_NAME,
            this.gridColumn_MAX_Q,
            this.gridColumn_ER_Q,
            this.gridColumn_W_WAIT,
            this.gridColumn_W_REQ,
            this.gridColumn_W_STP,
            this.gridColumn_R_WAIT,
            this.gridColumn_R_REQ,
            this.gridColumn_R_STP,
            this.gridColumn_MAX_HZ,
            this.gridColumn_IN_SCALE,
            this.gridColumn_PLC_ADDRESS,
            this.gridColumn_STD,
            this.gridColumn_SDD,
            this.gridColumn_I_TIME});
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
            this.gridView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView_CustomDrawRowIndicator);
            this.gridView.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gridView_ShowingEditor);
            this.gridView.ColumnPositionChanged += new System.EventHandler(this.gridView_ColumnPositionChanged);
            this.gridView.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView_CellValueChanged);
            this.gridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridView_MouseDown);
            // 
            // gridColumn_CHK
            // 
            this.gridColumn_CHK.Caption = "선택";
            this.gridColumn_CHK.ColumnEdit = this.gridCHK;
            this.gridColumn_CHK.FieldName = "CHK";
            this.gridColumn_CHK.Name = "gridColumn_CHK";
            this.gridColumn_CHK.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_CHK.Visible = true;
            this.gridColumn_CHK.VisibleIndex = 0;
            this.gridColumn_CHK.Width = 35;
            // 
            // gridCHK
            // 
            this.gridCHK.AutoHeight = false;
            this.gridCHK.Name = "gridCHK";
            // 
            // PLANT_CODE
            // 
            this.PLANT_CODE.Caption = "플랜트";
            this.PLANT_CODE.ColumnEdit = this.gridscboPlant;
            this.PLANT_CODE.FieldName = "PLANT_CODE";
            this.PLANT_CODE.MaxWidth = 160;
            this.PLANT_CODE.MinWidth = 160;
            this.PLANT_CODE.Name = "PLANT_CODE";
            this.PLANT_CODE.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.PLANT_CODE.Visible = true;
            this.PLANT_CODE.VisibleIndex = 1;
            this.PLANT_CODE.Width = 160;
            // 
            // gridscboPlant
            // 
            this.gridscboPlant.AutoHeight = false;
            this.gridscboPlant.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridscboPlant.Name = "gridscboPlant";
            this.gridscboPlant.NullText = "";
            this.gridscboPlant.PopupView = this.repositoryItemSearchLookUpEdit1View;
            // 
            // repositoryItemSearchLookUpEdit1View
            // 
            this.repositoryItemSearchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemSearchLookUpEdit1View.Name = "repositoryItemSearchLookUpEdit1View";
            this.repositoryItemSearchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemSearchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // PROCESS_KEY
            // 
            this.PROCESS_KEY.Caption = "공정";
            this.PROCESS_KEY.ColumnEdit = this.gridcboPROCESS_KEY;
            this.PROCESS_KEY.FieldName = "PROCESS_KEY";
            this.PROCESS_KEY.MaxWidth = 120;
            this.PROCESS_KEY.MinWidth = 120;
            this.PROCESS_KEY.Name = "PROCESS_KEY";
            this.PROCESS_KEY.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.PROCESS_KEY.Visible = true;
            this.PROCESS_KEY.VisibleIndex = 2;
            this.PROCESS_KEY.Width = 120;
            // 
            // gridcboPROCESS_KEY
            // 
            this.gridcboPROCESS_KEY.AutoHeight = false;
            this.gridcboPROCESS_KEY.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridcboPROCESS_KEY.Name = "gridcboPROCESS_KEY";
            this.gridcboPROCESS_KEY.NullText = "";
            this.gridcboPROCESS_KEY.EditValueChanged += new System.EventHandler(this.gridcboPROCESS_KEY_EditValueChanged);
            // 
            // gridColumn_L_CODE
            // 
            this.gridColumn_L_CODE.Caption = "라인";
            this.gridColumn_L_CODE.ColumnEdit = this.gridcboL_CODE;
            this.gridColumn_L_CODE.FieldName = "L_CODE";
            this.gridColumn_L_CODE.MaxWidth = 100;
            this.gridColumn_L_CODE.MinWidth = 100;
            this.gridColumn_L_CODE.Name = "gridColumn_L_CODE";
            this.gridColumn_L_CODE.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn_L_CODE.Visible = true;
            this.gridColumn_L_CODE.VisibleIndex = 3;
            this.gridColumn_L_CODE.Width = 100;
            // 
            // gridcboL_CODE
            // 
            this.gridcboL_CODE.AutoHeight = false;
            this.gridcboL_CODE.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridcboL_CODE.Name = "gridcboL_CODE";
            this.gridcboL_CODE.NullText = "";
            // 
            // gridColumn_SCALE_CODE
            // 
            this.gridColumn_SCALE_CODE.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_SCALE_CODE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_SCALE_CODE.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_SCALE_CODE.Caption = "스케일코드";
            this.gridColumn_SCALE_CODE.FieldName = "SCALE_CODE";
            this.gridColumn_SCALE_CODE.MinWidth = 16;
            this.gridColumn_SCALE_CODE.Name = "gridColumn_SCALE_CODE";
            this.gridColumn_SCALE_CODE.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_SCALE_CODE.Visible = true;
            this.gridColumn_SCALE_CODE.VisibleIndex = 4;
            this.gridColumn_SCALE_CODE.Width = 109;
            // 
            // gridColumn_SCALE_NO
            // 
            this.gridColumn_SCALE_NO.Caption = "스케일 번호";
            this.gridColumn_SCALE_NO.FieldName = "SCALE_NO";
            this.gridColumn_SCALE_NO.Name = "gridColumn_SCALE_NO";
            this.gridColumn_SCALE_NO.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_SCALE_NO.Visible = true;
            this.gridColumn_SCALE_NO.VisibleIndex = 5;
            // 
            // gridColumn_SCALE_NAME
            // 
            this.gridColumn_SCALE_NAME.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_SCALE_NAME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_SCALE_NAME.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_SCALE_NAME.Caption = "스케일명";
            this.gridColumn_SCALE_NAME.FieldName = "SCALE_NAME";
            this.gridColumn_SCALE_NAME.MinWidth = 16;
            this.gridColumn_SCALE_NAME.Name = "gridColumn_SCALE_NAME";
            this.gridColumn_SCALE_NAME.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_SCALE_NAME.Visible = true;
            this.gridColumn_SCALE_NAME.VisibleIndex = 6;
            this.gridColumn_SCALE_NAME.Width = 109;
            // 
            // gridColumn_MAX_Q
            // 
            this.gridColumn_MAX_Q.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_MAX_Q.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_MAX_Q.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_MAX_Q.Caption = "최대용량";
            this.gridColumn_MAX_Q.DisplayFormat.FormatString = "{0:n0}";
            this.gridColumn_MAX_Q.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn_MAX_Q.FieldName = "MAX_Q";
            this.gridColumn_MAX_Q.MinWidth = 16;
            this.gridColumn_MAX_Q.Name = "gridColumn_MAX_Q";
            this.gridColumn_MAX_Q.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_MAX_Q.Visible = true;
            this.gridColumn_MAX_Q.VisibleIndex = 7;
            this.gridColumn_MAX_Q.Width = 109;
            // 
            // gridColumn_ER_Q
            // 
            this.gridColumn_ER_Q.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_ER_Q.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_ER_Q.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_ER_Q.Caption = "잔량";
            this.gridColumn_ER_Q.DisplayFormat.FormatString = "{0:n0}";
            this.gridColumn_ER_Q.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn_ER_Q.FieldName = "ER_Q";
            this.gridColumn_ER_Q.MinWidth = 16;
            this.gridColumn_ER_Q.Name = "gridColumn_ER_Q";
            this.gridColumn_ER_Q.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_ER_Q.Visible = true;
            this.gridColumn_ER_Q.VisibleIndex = 8;
            this.gridColumn_ER_Q.Width = 109;
            // 
            // gridColumn_W_WAIT
            // 
            this.gridColumn_W_WAIT.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_W_WAIT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_W_WAIT.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_W_WAIT.Caption = "계량대기(초)";
            this.gridColumn_W_WAIT.DisplayFormat.FormatString = "{0:n0}";
            this.gridColumn_W_WAIT.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn_W_WAIT.FieldName = "W_WAIT";
            this.gridColumn_W_WAIT.MinWidth = 16;
            this.gridColumn_W_WAIT.Name = "gridColumn_W_WAIT";
            this.gridColumn_W_WAIT.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_W_WAIT.Visible = true;
            this.gridColumn_W_WAIT.VisibleIndex = 9;
            this.gridColumn_W_WAIT.Width = 109;
            // 
            // gridColumn_W_REQ
            // 
            this.gridColumn_W_REQ.Caption = "계량소요(초)";
            this.gridColumn_W_REQ.FieldName = "W_REQ";
            this.gridColumn_W_REQ.Name = "gridColumn_W_REQ";
            this.gridColumn_W_REQ.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_W_REQ.Visible = true;
            this.gridColumn_W_REQ.VisibleIndex = 10;
            this.gridColumn_W_REQ.Width = 80;
            // 
            // gridColumn_W_STP
            // 
            this.gridColumn_W_STP.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_W_STP.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_W_STP.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_W_STP.Caption = "계량안정(초)";
            this.gridColumn_W_STP.DisplayFormat.FormatString = "{0:n0}";
            this.gridColumn_W_STP.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn_W_STP.FieldName = "W_STP";
            this.gridColumn_W_STP.MinWidth = 16;
            this.gridColumn_W_STP.Name = "gridColumn_W_STP";
            this.gridColumn_W_STP.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_W_STP.Visible = true;
            this.gridColumn_W_STP.VisibleIndex = 11;
            this.gridColumn_W_STP.Width = 109;
            // 
            // gridColumn_R_WAIT
            // 
            this.gridColumn_R_WAIT.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_R_WAIT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_R_WAIT.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_R_WAIT.Caption = "방출대기(초)";
            this.gridColumn_R_WAIT.DisplayFormat.FormatString = "{0:n0}";
            this.gridColumn_R_WAIT.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn_R_WAIT.FieldName = "R_WAIT";
            this.gridColumn_R_WAIT.MinWidth = 16;
            this.gridColumn_R_WAIT.Name = "gridColumn_R_WAIT";
            this.gridColumn_R_WAIT.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_R_WAIT.Visible = true;
            this.gridColumn_R_WAIT.VisibleIndex = 12;
            this.gridColumn_R_WAIT.Width = 109;
            // 
            // gridColumn_R_REQ
            // 
            this.gridColumn_R_REQ.Caption = "방출소요(초)";
            this.gridColumn_R_REQ.FieldName = "R_REQ";
            this.gridColumn_R_REQ.Name = "gridColumn_R_REQ";
            this.gridColumn_R_REQ.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_R_REQ.Visible = true;
            this.gridColumn_R_REQ.VisibleIndex = 13;
            // 
            // gridColumn_R_STP
            // 
            this.gridColumn_R_STP.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_R_STP.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_R_STP.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_R_STP.Caption = "방출안정(초)";
            this.gridColumn_R_STP.DisplayFormat.FormatString = "{0:n0}";
            this.gridColumn_R_STP.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn_R_STP.FieldName = "R_STP";
            this.gridColumn_R_STP.MinWidth = 16;
            this.gridColumn_R_STP.Name = "gridColumn_R_STP";
            this.gridColumn_R_STP.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_R_STP.Visible = true;
            this.gridColumn_R_STP.VisibleIndex = 14;
            this.gridColumn_R_STP.Width = 109;
            // 
            // gridColumn_MAX_HZ
            // 
            this.gridColumn_MAX_HZ.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_MAX_HZ.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_MAX_HZ.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_MAX_HZ.Caption = "스케일최대(Hz)";
            this.gridColumn_MAX_HZ.DisplayFormat.FormatString = "{0:n0}";
            this.gridColumn_MAX_HZ.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn_MAX_HZ.FieldName = "MAX_HZ";
            this.gridColumn_MAX_HZ.MinWidth = 16;
            this.gridColumn_MAX_HZ.Name = "gridColumn_MAX_HZ";
            this.gridColumn_MAX_HZ.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_MAX_HZ.Visible = true;
            this.gridColumn_MAX_HZ.VisibleIndex = 15;
            this.gridColumn_MAX_HZ.Width = 109;
            // 
            // gridColumn_IN_SCALE
            // 
            this.gridColumn_IN_SCALE.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_IN_SCALE.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_IN_SCALE.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_IN_SCALE.Caption = "스케일비율(KG)";
            this.gridColumn_IN_SCALE.ColumnEdit = this.gridCboIN_SCALE;
            this.gridColumn_IN_SCALE.DisplayFormat.FormatString = "{0:n0}";
            this.gridColumn_IN_SCALE.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn_IN_SCALE.FieldName = "IN_SCALE";
            this.gridColumn_IN_SCALE.MinWidth = 16;
            this.gridColumn_IN_SCALE.Name = "gridColumn_IN_SCALE";
            this.gridColumn_IN_SCALE.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_IN_SCALE.Visible = true;
            this.gridColumn_IN_SCALE.VisibleIndex = 16;
            this.gridColumn_IN_SCALE.Width = 109;
            // 
            // gridCboIN_SCALE
            // 
            this.gridCboIN_SCALE.AutoHeight = false;
            this.gridCboIN_SCALE.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridCboIN_SCALE.Name = "gridCboIN_SCALE";
            // 
            // gridColumn_PLC_ADDRESS
            // 
            this.gridColumn_PLC_ADDRESS.Caption = "PLC 주소";
            this.gridColumn_PLC_ADDRESS.FieldName = "PLC_ADDRESS";
            this.gridColumn_PLC_ADDRESS.MinWidth = 100;
            this.gridColumn_PLC_ADDRESS.Name = "gridColumn_PLC_ADDRESS";
            this.gridColumn_PLC_ADDRESS.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_PLC_ADDRESS.Visible = true;
            this.gridColumn_PLC_ADDRESS.VisibleIndex = 17;
            this.gridColumn_PLC_ADDRESS.Width = 180;
            // 
            // gridColumn_STD
            // 
            this.gridColumn_STD.Caption = "시작 번호";
            this.gridColumn_STD.FieldName = "STD";
            this.gridColumn_STD.Name = "gridColumn_STD";
            this.gridColumn_STD.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_STD.Visible = true;
            this.gridColumn_STD.VisibleIndex = 18;
            // 
            // gridColumn_SDD
            // 
            this.gridColumn_SDD.Caption = "전송 개수";
            this.gridColumn_SDD.FieldName = "SDD";
            this.gridColumn_SDD.Name = "gridColumn_SDD";
            this.gridColumn_SDD.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_SDD.Visible = true;
            this.gridColumn_SDD.VisibleIndex = 19;
            // 
            // gridColumn_I_TIME
            // 
            this.gridColumn_I_TIME.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_I_TIME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_I_TIME.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_I_TIME.Caption = "수정일자";
            this.gridColumn_I_TIME.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.gridColumn_I_TIME.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn_I_TIME.FieldName = "I_TIME";
            this.gridColumn_I_TIME.MinWidth = 16;
            this.gridColumn_I_TIME.Name = "gridColumn_I_TIME";
            this.gridColumn_I_TIME.OptionsColumn.AllowEdit = false;
            this.gridColumn_I_TIME.OptionsColumn.AllowFocus = false;
            this.gridColumn_I_TIME.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn_I_TIME.OptionsColumn.ReadOnly = true;
            this.gridColumn_I_TIME.OptionsColumn.TabStop = false;
            this.gridColumn_I_TIME.Visible = true;
            this.gridColumn_I_TIME.VisibleIndex = 20;
            this.gridColumn_I_TIME.Width = 131;
            // 
            // gridsCboPLC_ADDRESS
            // 
            this.gridsCboPLC_ADDRESS.AutoHeight = false;
            this.gridsCboPLC_ADDRESS.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridsCboPLC_ADDRESS.Name = "gridsCboPLC_ADDRESS";
            this.gridsCboPLC_ADDRESS.PopupView = this.gridView1;
            this.gridsCboPLC_ADDRESS.EditValueChanged += new System.EventHandler(this.gridsCboPLC_ADDRESS_EditValueChanged);
            // 
            // gridView1
            // 
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // btn_save
            // 
            this.btn_save.AllowFocus = false;
            this.btn_save.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_save.ImageOptions.Image")));
            this.btn_save.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_save.ImageOptions.ImageToTextIndent = 10;
            this.btn_save.Location = new System.Drawing.Point(1526, 28);
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
            // btn_delete
            // 
            this.btn_delete.Appearance.ForeColor = System.Drawing.Color.Red;
            this.btn_delete.Appearance.Options.UseForeColor = true;
            this.btn_delete.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_delete.ImageOptions.Image")));
            this.btn_delete.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_delete.ImageOptions.ImageToTextIndent = 10;
            this.btn_delete.Location = new System.Drawing.Point(1642, 29);
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
            // btn_rowDel
            // 
            this.btn_rowDel.AllowFocus = false;
            this.btn_rowDel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_rowDel.ImageOptions.Image")));
            this.btn_rowDel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btn_rowDel.Location = new System.Drawing.Point(1467, 29);
            this.btn_rowDel.Margin = new System.Windows.Forms.Padding(0);
            this.btn_rowDel.Name = "btn_rowDel";
            this.btn_rowDel.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_rowDel.Size = new System.Drawing.Size(34, 40);
            this.btn_rowDel.StyleController = this.layoutControl;
            this.btn_rowDel.TabIndex = 20;
            this.btn_rowDel.ToolTip = "취소 (ESC)";
            this.btn_rowDel.Click += new System.EventHandler(this.btn_rowDel_Click);
            // 
            // btn_rowAdd
            // 
            this.btn_rowAdd.AllowFocus = false;
            this.btn_rowAdd.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_rowAdd.ImageOptions.Image")));
            this.btn_rowAdd.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btn_rowAdd.Location = new System.Drawing.Point(1427, 29);
            this.btn_rowAdd.Margin = new System.Windows.Forms.Padding(0);
            this.btn_rowAdd.Name = "btn_rowAdd";
            this.btn_rowAdd.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_rowAdd.Size = new System.Drawing.Size(34, 40);
            this.btn_rowAdd.StyleController = this.layoutControl;
            this.btn_rowAdd.TabIndex = 21;
            this.btn_rowAdd.ToolTip = "신규(F3)";
            this.btn_rowAdd.Click += new System.EventHandler(this.btn_rowAdd_Click);
            // 
            // btn_search
            // 
            this.btn_search.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_search.ImageOptions.ImageToTextIndent = 10;
            this.btn_search.Location = new System.Drawing.Point(671, 29);
            this.btn_search.Margin = new System.Windows.Forms.Padding(4);
            this.btn_search.Name = "btn_search";
            this.btn_search.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_search.Size = new System.Drawing.Size(109, 40);
            this.btn_search.StyleController = this.layoutControl;
            this.btn_search.TabIndex = 7;
            this.btn_search.Text = "조 회(F5)";
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // cboProcessKey
            // 
            this.cboProcessKey.Location = new System.Drawing.Point(277, 37);
            this.cboProcessKey.Name = "cboProcessKey";
            this.cboProcessKey.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboProcessKey.Size = new System.Drawing.Size(91, 24);
            this.cboProcessKey.StyleController = this.layoutControl;
            this.cboProcessKey.TabIndex = 23;
            this.cboProcessKey.EditValueChanged += new System.EventHandler(this.cboProcessKey_EditValueChanged);
            // 
            // cboPlant_Code
            // 
            this.cboPlant_Code.Location = new System.Drawing.Point(49, 37);
            this.cboPlant_Code.Name = "cboPlant_Code";
            this.cboPlant_Code.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboPlant_Code.Size = new System.Drawing.Size(175, 24);
            this.cboPlant_Code.StyleController = this.layoutControl;
            this.cboPlant_Code.TabIndex = 23;
            this.cboPlant_Code.EditValueChanged += new System.EventHandler(this.cboPlant_Code_EditValueChanged);
            // 
            // cboL_Code
            // 
            this.cboL_Code.Location = new System.Drawing.Point(421, 37);
            this.cboL_Code.Name = "cboL_Code";
            this.cboL_Code.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboL_Code.Size = new System.Drawing.Size(91, 24);
            this.cboL_Code.StyleController = this.layoutControl;
            this.cboL_Code.TabIndex = 23;
            this.cboL_Code.EditValueChanged += new System.EventHandler(this.cboL_Code_EditValueChanged);
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
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem3,
            this.emptySpaceItem2,
            this.layoutControlItem_plcRead,
            this.layoutControlItem_plcSend,
            this.layoutControlItem8,
            this.layoutControlItem10,
            this.layoutControlItem11,
            this.layoutControlItem5,
            this.emptySpaceItem1,
            this.layoutControlItem2,
            this.layoutControlItem4,
            this.layoutControlItem7,
            this.layoutControlItem12});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlGroup1.Size = new System.Drawing.Size(1760, 620);
            this.layoutControlGroup1.Text = "스케일정보관리";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 46);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Padding = new DevExpress.XtraLayout.Utils.Padding(1, 1, 1, 1);
            this.layoutControlItem1.Size = new System.Drawing.Size(1748, 542);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btn_save;
            this.layoutControlItem3.Location = new System.Drawing.Point(1518, 0);
            this.layoutControlItem3.MaxSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem3.MinSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(1205, 0);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(213, 46);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem_plcRead
            // 
            this.layoutControlItem_plcRead.Control = this.btnPLCDownLoad;
            this.layoutControlItem_plcRead.Location = new System.Drawing.Point(1087, 0);
            this.layoutControlItem_plcRead.MaxSize = new System.Drawing.Size(118, 46);
            this.layoutControlItem_plcRead.MinSize = new System.Drawing.Size(118, 46);
            this.layoutControlItem_plcRead.Name = "layoutControlItem_plcRead";
            this.layoutControlItem_plcRead.Size = new System.Drawing.Size(118, 46);
            this.layoutControlItem_plcRead.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem_plcRead.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem_plcRead.TextVisible = false;
            // 
            // layoutControlItem_plcSend
            // 
            this.layoutControlItem_plcSend.Control = this.btnPLCUpload;
            this.layoutControlItem_plcSend.Location = new System.Drawing.Point(969, 0);
            this.layoutControlItem_plcSend.MaxSize = new System.Drawing.Size(118, 46);
            this.layoutControlItem_plcSend.MinSize = new System.Drawing.Size(118, 46);
            this.layoutControlItem_plcSend.Name = "layoutControlItem_plcSend";
            this.layoutControlItem_plcSend.Size = new System.Drawing.Size(118, 46);
            this.layoutControlItem_plcSend.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem_plcSend.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem_plcSend.TextVisible = false;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.btn_delete;
            this.layoutControlItem8.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem8.CustomizationFormText = "layoutControlItem4";
            this.layoutControlItem8.Location = new System.Drawing.Point(1633, 0);
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
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this.btn_rowDel;
            this.layoutControlItem10.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem10.CustomizationFormText = "layoutControlItem10";
            this.layoutControlItem10.Location = new System.Drawing.Point(1458, 0);
            this.layoutControlItem10.MaxSize = new System.Drawing.Size(60, 46);
            this.layoutControlItem10.MinSize = new System.Drawing.Size(60, 46);
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
            this.layoutControlItem11.Location = new System.Drawing.Point(1418, 0);
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
            this.layoutControlItem5.Location = new System.Drawing.Point(662, 0);
            this.layoutControlItem5.MaxSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem5.MinSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlItem5.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem5.Text = "layoutControlItem3";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(777, 0);
            this.emptySpaceItem1.MinSize = new System.Drawing.Size(104, 24);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(192, 46);
            this.emptySpaceItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem2.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem2.Control = this.txtName;
            this.layoutControlItem2.Location = new System.Drawing.Point(508, 0);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(127, 28);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(154, 46);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.Spacing = new DevExpress.XtraLayout.Utils.Padding(20, 0, 0, 0);
            this.layoutControlItem2.Text = "스케일명";
            this.layoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(48, 15);
            this.layoutControlItem2.TextToControlDistance = 5;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem4.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem4.Control = this.cboProcessKey;
            this.layoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem4.CustomizationFormText = "공정";
            this.layoutControlItem4.Location = new System.Drawing.Point(220, 0);
            this.layoutControlItem4.MaxSize = new System.Drawing.Size(144, 46);
            this.layoutControlItem4.MinSize = new System.Drawing.Size(144, 46);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(144, 46);
            this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem4.Spacing = new DevExpress.XtraLayout.Utils.Padding(20, 0, 0, 0);
            this.layoutControlItem4.Text = "공정";
            this.layoutControlItem4.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem4.TextSize = new System.Drawing.Size(24, 15);
            this.layoutControlItem4.TextToControlDistance = 5;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem7.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem7.Control = this.cboL_Code;
            this.layoutControlItem7.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem7.CustomizationFormText = "라인";
            this.layoutControlItem7.Location = new System.Drawing.Point(364, 0);
            this.layoutControlItem7.MaxSize = new System.Drawing.Size(144, 46);
            this.layoutControlItem7.MinSize = new System.Drawing.Size(144, 46);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(144, 46);
            this.layoutControlItem7.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem7.Spacing = new DevExpress.XtraLayout.Utils.Padding(20, 0, 0, 0);
            this.layoutControlItem7.Text = "라인";
            this.layoutControlItem7.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem7.TextSize = new System.Drawing.Size(24, 15);
            this.layoutControlItem7.TextToControlDistance = 5;
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
            // splashScreenManager
            // 
            this.splashScreenManager.ClosingDelay = 500;
            // 
            // frm_Scale
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1760, 620);
            this.Controls.Add(this.layoutControl);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("frm_Scale.IconOptions.Image")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frm_Scale";
            this.Tag = "frm_Scale";
            this.Text = "스케일정보 관리";
            this.Load += new System.EventHandler(this.frm_Scale_Load);
            this.Shown += new System.EventHandler(this.frm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCHK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridscboPlant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboPROCESS_KEY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridcboL_CODE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCboIN_SCALE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridsCboPLC_ADDRESS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProcessKey.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPlant_Code.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboL_Code.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem_plcRead)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem_plcSend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_SCALE_CODE;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_SCALE_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_MAX_Q;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_ER_Q;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_W_WAIT;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_W_STP;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_R_STP;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_R_WAIT;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_MAX_HZ;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_IN_SCALE;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_I_TIME;
        private DevExpress.XtraEditors.SimpleButton btn_save;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraEditors.SimpleButton btnPLCDownLoad;
        private DevExpress.XtraEditors.SimpleButton btnPLCUpload;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem_plcRead;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem_plcSend;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager;
        private DevExpress.XtraEditors.SimpleButton btn_delete;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraEditors.SimpleButton btn_rowDel;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem10;
        private DevExpress.XtraEditors.SimpleButton btn_rowAdd;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem11;
        private DevExpress.XtraEditors.TextEdit txtName;
        private DevExpress.XtraEditors.SimpleButton btn_search;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_CHK;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit gridCHK;
        private DevExpress.XtraGrid.Columns.GridColumn PLANT_CODE;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit gridscboPlant;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemSearchLookUpEdit1View;
        private DevExpress.XtraGrid.Columns.GridColumn PROCESS_KEY;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_L_CODE;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit gridcboPROCESS_KEY;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit gridcboL_CODE;
        private DevExpress.XtraEditors.LookUpEdit cboProcessKey;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.LookUpEdit cboPlant_Code;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem12;
        private DevExpress.XtraEditors.LookUpEdit cboL_Code;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_PLC_ADDRESS;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_STD;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_SDD;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_W_REQ;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_R_REQ;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit gridsCboPLC_ADDRESS;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit gridCboIN_SCALE;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_SCALE_NO;
    }
}