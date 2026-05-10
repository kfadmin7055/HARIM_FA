namespace HARIM_FA_DOSING
{
    partial class frm_Pallet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Pallet));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.btn_rowAdd = new DevExpress.XtraEditors.SimpleButton();
            this.btn_rowDel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_reflash = new DevExpress.XtraEditors.SimpleButton();
            this.btn_delete = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn_PTMCD = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_PTMCDNM = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_WEIGHT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_DISPLAY_SEQ = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_USER_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn_I_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btn_save = new DevExpress.XtraEditors.SimpleButton();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.btn_rowAdd);
            this.layoutControl.Controls.Add(this.btn_rowDel);
            this.layoutControl.Controls.Add(this.btn_reflash);
            this.layoutControl.Controls.Add(this.btn_delete);
            this.layoutControl.Controls.Add(this.gridControl);
            this.layoutControl.Controls.Add(this.btn_save);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.Root = this.Root;
            this.layoutControl.Size = new System.Drawing.Size(1760, 620);
            this.layoutControl.TabIndex = 11;
            this.layoutControl.Text = "layoutControl1";
            // 
            // btn_rowAdd
            // 
            this.btn_rowAdd.AllowFocus = false;
            this.btn_rowAdd.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_rowAdd.ImageOptions.Image")));
            this.btn_rowAdd.Location = new System.Drawing.Point(1428, 26);
            this.btn_rowAdd.Margin = new System.Windows.Forms.Padding(0);
            this.btn_rowAdd.Name = "btn_rowAdd";
            this.btn_rowAdd.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_rowAdd.Size = new System.Drawing.Size(36, 42);
            this.btn_rowAdd.StyleController = this.layoutControl;
            this.btn_rowAdd.TabIndex = 21;
            this.btn_rowAdd.ToolTip = "신규(F3)";
            this.btn_rowAdd.Click += new System.EventHandler(this.btn_rowAdd_Click);
            // 
            // btn_rowDel
            // 
            this.btn_rowDel.AllowFocus = false;
            this.btn_rowDel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_rowDel.ImageOptions.Image")));
            this.btn_rowDel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
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
            // btn_reflash
            // 
            this.btn_reflash.AllowFocus = false;
            this.btn_reflash.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_reflash.ImageOptions.Image")));
            this.btn_reflash.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
            this.btn_reflash.Location = new System.Drawing.Point(1388, 26);
            this.btn_reflash.Margin = new System.Windows.Forms.Padding(0);
            this.btn_reflash.Name = "btn_reflash";
            this.btn_reflash.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_reflash.Size = new System.Drawing.Size(36, 42);
            this.btn_reflash.StyleController = this.layoutControl;
            this.btn_reflash.TabIndex = 19;
            this.btn_reflash.ToolTip = "새로고침(F5)";
            this.btn_reflash.Click += new System.EventHandler(this.btn_reflash_Click);
            // 
            // btn_delete
            // 
            this.btn_delete.Appearance.ForeColor = System.Drawing.Color.Red;
            this.btn_delete.Appearance.Options.UseForeColor = true;
            this.btn_delete.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_delete.ImageOptions.ImageToTextIndent = 10;
            this.btn_delete.Location = new System.Drawing.Point(1643, 26);
            this.btn_delete.Margin = new System.Windows.Forms.Padding(4);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_delete.Size = new System.Drawing.Size(111, 42);
            this.btn_delete.StyleController = this.layoutControl;
            this.btn_delete.TabIndex = 8;
            this.btn_delete.Text = "삭 제";
            this.btn_delete.ToolTip = "삭제(Ctrl + D)";
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // gridControl
            // 
            this.gridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gridControl.Location = new System.Drawing.Point(6, 72);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(1748, 542);
            this.gridControl.TabIndex = 5;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            this.gridControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridControl_KeyDown);
            // 
            // gridView
            // 
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn_PTMCD,
            this.gridColumn_PTMCDNM,
            this.gridColumn_WEIGHT,
            this.gridColumn_DISPLAY_SEQ,
            this.gridColumn_USER_ID,
            this.gridColumn_I_TIME});
            this.gridView.DetailHeight = 404;
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            this.gridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gridView.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always;
            this.gridView.OptionsFind.FindNullPrompt = "검색할 문자열을 입력해 주세요...";
            this.gridView.OptionsFind.ShowFindButton = false;
            this.gridView.OptionsView.ColumnAutoWidth = false;
            this.gridView.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.gridView.OptionsView.ShowFooter = true;
            this.gridView.OptionsView.ShowGroupPanel = false;
            this.gridView.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView_CustomDrawRowIndicator);
            // 
            // gridColumn_PTMCD
            // 
            this.gridColumn_PTMCD.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_PTMCD.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_PTMCD.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_PTMCD.Caption = "파렛트코드";
            this.gridColumn_PTMCD.FieldName = "PTMCD";
            this.gridColumn_PTMCD.Name = "gridColumn_PTMCD";
            this.gridColumn_PTMCD.Visible = true;
            this.gridColumn_PTMCD.VisibleIndex = 0;
            this.gridColumn_PTMCD.Width = 128;
            // 
            // gridColumn_PTMCDNM
            // 
            this.gridColumn_PTMCDNM.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_PTMCDNM.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_PTMCDNM.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_PTMCDNM.Caption = "파렛트명";
            this.gridColumn_PTMCDNM.FieldName = "PTMCDNM";
            this.gridColumn_PTMCDNM.Name = "gridColumn_PTMCDNM";
            this.gridColumn_PTMCDNM.Visible = true;
            this.gridColumn_PTMCDNM.VisibleIndex = 1;
            this.gridColumn_PTMCDNM.Width = 214;
            // 
            // gridColumn_WEIGHT
            // 
            this.gridColumn_WEIGHT.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_WEIGHT.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_WEIGHT.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_WEIGHT.Caption = "파레트무게(KG)";
            this.gridColumn_WEIGHT.DisplayFormat.FormatString = "{0:n3}";
            this.gridColumn_WEIGHT.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn_WEIGHT.FieldName = "WEIGHT";
            this.gridColumn_WEIGHT.Name = "gridColumn_WEIGHT";
            this.gridColumn_WEIGHT.Visible = true;
            this.gridColumn_WEIGHT.VisibleIndex = 2;
            this.gridColumn_WEIGHT.Width = 140;
            // 
            // gridColumn_DISPLAY_SEQ
            // 
            this.gridColumn_DISPLAY_SEQ.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_DISPLAY_SEQ.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_DISPLAY_SEQ.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_DISPLAY_SEQ.Caption = "표시순서";
            this.gridColumn_DISPLAY_SEQ.FieldName = "DISPLAY_SEQ";
            this.gridColumn_DISPLAY_SEQ.Name = "gridColumn_DISPLAY_SEQ";
            this.gridColumn_DISPLAY_SEQ.Visible = true;
            this.gridColumn_DISPLAY_SEQ.VisibleIndex = 3;
            this.gridColumn_DISPLAY_SEQ.Width = 84;
            // 
            // gridColumn_USER_ID
            // 
            this.gridColumn_USER_ID.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_USER_ID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_USER_ID.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_USER_ID.Caption = "입력사원";
            this.gridColumn_USER_ID.FieldName = "USER_ID";
            this.gridColumn_USER_ID.Name = "gridColumn_USER_ID";
            this.gridColumn_USER_ID.OptionsColumn.ReadOnly = true;
            this.gridColumn_USER_ID.Visible = true;
            this.gridColumn_USER_ID.VisibleIndex = 4;
            this.gridColumn_USER_ID.Width = 126;
            // 
            // gridColumn_I_TIME
            // 
            this.gridColumn_I_TIME.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn_I_TIME.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn_I_TIME.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn_I_TIME.Caption = "입력시간";
            this.gridColumn_I_TIME.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.gridColumn_I_TIME.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn_I_TIME.FieldName = "I_TIME";
            this.gridColumn_I_TIME.Name = "gridColumn_I_TIME";
            this.gridColumn_I_TIME.OptionsColumn.AllowFocus = false;
            this.gridColumn_I_TIME.OptionsColumn.ReadOnly = true;
            this.gridColumn_I_TIME.Visible = true;
            this.gridColumn_I_TIME.VisibleIndex = 5;
            this.gridColumn_I_TIME.Width = 279;
            // 
            // btn_save
            // 
            this.btn_save.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_save.ImageOptions.ImageToTextIndent = 10;
            this.btn_save.Location = new System.Drawing.Point(1528, 26);
            this.btn_save.Margin = new System.Windows.Forms.Padding(4);
            this.btn_save.Name = "btn_save";
            this.btn_save.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_save.Size = new System.Drawing.Size(111, 42);
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
            this.layoutControlItem4,
            this.layoutControlItem9,
            this.layoutControlItem10,
            this.layoutControlItem11});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(1, 1, 1, 1);
            this.layoutControlGroup1.Size = new System.Drawing.Size(1760, 620);
            this.layoutControlGroup1.Text = "파렛트정보관리";
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
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btn_save;
            this.layoutControlItem3.Location = new System.Drawing.Point(1522, 0);
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
            this.emptySpaceItem2.Location = new System.Drawing.Point(0, 0);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(1382, 46);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btn_delete;
            this.layoutControlItem4.Location = new System.Drawing.Point(1637, 0);
            this.layoutControlItem4.MaxSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem4.MinSize = new System.Drawing.Size(115, 46);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(115, 46);
            this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.btn_reflash;
            this.layoutControlItem9.Location = new System.Drawing.Point(1382, 0);
            this.layoutControlItem9.MaxSize = new System.Drawing.Size(40, 46);
            this.layoutControlItem9.MinSize = new System.Drawing.Size(40, 46);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Size = new System.Drawing.Size(40, 46);
            this.layoutControlItem9.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem9.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem9.TextVisible = false;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this.btn_rowDel;
            this.layoutControlItem10.Location = new System.Drawing.Point(1462, 0);
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
            this.layoutControlItem11.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("layoutControlItem11.ImageOptions.Image")));
            this.layoutControlItem11.Location = new System.Drawing.Point(1422, 0);
            this.layoutControlItem11.MaxSize = new System.Drawing.Size(40, 46);
            this.layoutControlItem11.MinSize = new System.Drawing.Size(40, 46);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Size = new System.Drawing.Size(40, 46);
            this.layoutControlItem11.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem11.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem11.TextVisible = false;
            // 
            // frm_Pallet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1760, 620);
            this.Controls.Add(this.layoutControl);
            this.Name = "frm_Pallet";
            this.Text = "파렛트정보관리";
            this.Load += new System.EventHandler(this.frm_Pallet_Load);
            this.Shown += new System.EventHandler(this.frm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraEditors.SimpleButton btn_rowAdd;
        private DevExpress.XtraEditors.SimpleButton btn_rowDel;
        private DevExpress.XtraEditors.SimpleButton btn_reflash;
        private DevExpress.XtraEditors.SimpleButton btn_delete;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_WEIGHT;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_I_TIME;
        private DevExpress.XtraEditors.SimpleButton btn_save;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem10;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_PTMCD;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_PTMCDNM;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_DISPLAY_SEQ;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn_USER_ID;
    }
}