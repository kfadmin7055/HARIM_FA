using Core;
using Core.Class;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.Extension
{
    public static class KGridView
    {
        /// <summary>
        /// 그리드 컨트롤 바인드 하기 델리게이트
        /// </summary>
        /// <param name="pGridControl">그리드 컨트롤</param>
        /// <param name="gridView">그리드 뷰</param>
        /// <param name="pDataSource">데이타 소스</param>
        private delegate void BindGridControlDelegate(GridView gridView, GridControl gridControl, object pDataSource, bool pAutoColumnFit = false, bool pFocusIndex = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Caption">그리드 컬럼명</param>
        /// <param name="Name">Data 컬럼명</param>
        /// <param name="ColumnEdit">Data Type</param>
        /// <param name="Width">Width</param>
        /// <param name="Visible">Visible</param>
        /// <param name="AllowFilter"></param>
        /// <returns></returns>
        public static void ColumnEdit(this GridView gridView, string Name, string Caption, XColumnEdit ColumnEdit = XColumnEdit.TEXT, int iWidth = 95, int maxLenth = 50, bool Visible = true, bool ReadOnly = false, bool AllowEdit = true, bool Requied = false, DataTable dt = null, bool bSummary = false, string SummaryType = "")
        {
            //RepoColumnEdit repoColumnEdit = null;

            RepoColumnEdit repositoryItem;

            GridColumn column = new GridColumn();

            switch (ColumnEdit)
            {
                case XColumnEdit.TEXT:
                    repositoryItem = new RepoColumnEditText();
                    break;
                case XColumnEdit.NUM:
                    repositoryItem = new RepoColumnEditNum();
                    break;
                case XColumnEdit.CURRENCY:
                    repositoryItem = new RepoColumnEditCURRENCY();
                    break;
                case XColumnEdit.CHECK:
                    repositoryItem = new RepoColumnEditCheck();
                    break;
                case XColumnEdit.DATE:
                    repositoryItem = new RepoColumnEditDate();
                    break;
                case XColumnEdit.DATEMONTH:
                    repositoryItem = new RepoColumnEditDateMonth();
                    column.DisplayFormat.FormatString = "yyyy-MM";
                    column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    break;
                case XColumnEdit.DATETIME:
                    repositoryItem = new RepoColumnEditDateTime();
                    break;
                case XColumnEdit.COMBO:
                    repositoryItem = new RepoColumnEditComboBox();
                    break;
                case XColumnEdit.BUTTON:
                    repositoryItem = new RepoColumnEditButton();
                    break;
                default:
                    repositoryItem = new RepoColumnEdit();
                    break;
            }

            column.AppearanceHeader.Options.UseTextOptions = true;
            column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            column.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            column.Caption = Caption.ToUpper();
            column.FieldName = Name.ToUpper();
            column.Name = Name.ToUpper();
            column.ColumnEdit = repositoryItem.Repository(dt, iWidth);



            if (bSummary)
            {
                if (SummaryType == "%")
                    SummaryType = "입금비율 : {0:0.##} %";
                else
                    SummaryType = "{0:#,##0}";

                column.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
                    new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, Name, SummaryType )});
            }
            // column.ImageOptions.Image = //global::KF_CLASS.Properties.Resources.editdatasource_16x16;

            gridView.OptionsView.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            gridView.OptionsBehavior.AutoExpandAllGroups = true;
            gridView.OptionsView.ShowGroupedColumns = true;
            gridView.OptionsBehavior.AutoPopulateColumns = true;
            gridView.OptionsMenu.ShowGroupSummaryEditorItem = true;

            GridGroupSummaryItem item = new GridGroupSummaryItem();
            item.FieldName = "AMOUNT";
            item.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            item.ShowInGroupColumnFooter = gridView.Columns["AMOUNT"];
            item.DisplayFormat = "{0:#,##0}";
            gridView.GroupSummary.Add(item);

            //column.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;

            column.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;

            column.OptionsFilter.AllowFilter = false;

            column.OptionsColumn.ReadOnly = ReadOnly;

            column.OptionsColumn.AllowEdit = AllowEdit;

            column.Visible = Visible;
            // column.VisibleIndex = 4;

            column.MinWidth =  (iWidth * 0.8).ToString().Int32Parse();
            column.Width = iWidth;
            column.MaxWidth = (iWidth * 1.2).ToString().Int32Parse();

            if (Requied)
            {
                // 필수항목 임시 이미지
                column.ImageOptions.Image = global::Core.Properties.Resources.about_16x16;
            }

            gridView.Columns.AddRange(new GridColumn[] { column });
        }

        public static void AddRow(this GridView gridView)
        {
            try
            {
                gridView.AddNewRow();
                gridView.UpdateCurrentRow();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDevexpressGrid", "GridViewAddRow", ex);
                ShowMessageBox.XtraShowError("행을 추가하는 도중 에러가 발생했습니다");
            }
        }

        public static void GridEndEdit(this GridView pGridView)
        {
            if (pGridView == null)
                return;

            if (pGridView.IsEditing)
            {
                pGridView.GetFocusedDataRow().EndEdit();
            }

            pGridView.ClearColumnErrors();
            pGridView.CloseEditor();
            pGridView.UpdateCurrentRow();
        }

        /// <summary>
        /// 그리드 컨트롤 바인드 하기
        /// </summary>
        /// <param name="gridControl">그리드 컨트롤</param>
        /// <param name="gridView">그리드 뷰</param>
        /// <param name="pDataSource">데이타 소스</param>
        /// <param name="pAutoColumnFit">자동컬럼크기조정 Flag</param>
        /// <param name="pFocusIndex">그리드포커스위치 고정 Flag</param>
        public static void BindGridControl(this GridView gridView, GridControl gridControl, object pDataSource, bool pAutoColumnFit = false, bool pFocusIndex = false)
        {
            try
            {
                if (gridControl.InvokeRequired)
                {
                    BindGridControlDelegate pBindGridControlDelegate = new BindGridControlDelegate(BindGridControl);

                    gridControl.Invoke(pBindGridControlDelegate, gridView, pDataSource);
                }
                else
                {
                    try
                    {
                        var topRowIndex = gridView.TopRowIndex;
                        var focusedRowHandle = gridView.FocusedRowHandle;

                        gridView.ShowLoadingPanel();
                        gridControl.DataSource = null;
                        gridControl.DataSource = pDataSource;
                        gridView.HideLoadingPanel();

                        if (pAutoColumnFit)
                        {
                            gridView.BestFitColumns();
                        }

                        if (pFocusIndex)
                        {
                            gridView.FocusedRowHandle = focusedRowHandle;
                            gridView.TopRowIndex = topRowIndex;
                        }

                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 그리드 컬럼 픽스
        /// </summary>
        /// <param name="gridView"></param>
        /// <param name="fixedColumnNames"></param>
        public static void SetFixCol(this GridView gridView, string[] fixedColumnNames)
        {
            foreach (GridColumn col in gridView.Columns)
            {
                if (fixedColumnNames.Contains(col.FieldName))
                    col.Fixed = FixedStyle.Left;
                else
                    col.Fixed = FixedStyle.None;
            }
        }

        /// <summary>
        /// 클립보드 데이터를 GridView에 붙여넣되,
        /// Key 값이 일치하는 행이 있으면 수정, 없으면 행 추가
        /// </summary>
        /// <param name="gridView">대상 GridView</param>
        /// <param name="keyColumnFieldName">Key 컬럼의 FieldName (예: "CODE")</param>
        public static void ExcelPasteClipboard(this GridView gridView, string keyColumnFieldName)
        {
            string clipboardText = Clipboard.GetText();
            if (string.IsNullOrWhiteSpace(clipboardText))
                return;

            string[] lines = clipboardText
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0)
                return;

            DataTable dt = gridView.GridControl.DataSource as DataTable;
            if (dt == null)
                return;

            for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
            {
                string[] values = lines[lineIndex].Split('\t');
                if (values.Length == 0)
                    continue;

                string key = values[0].Trim();
                if (string.IsNullOrEmpty(key))
                    continue;

                // 기존 행 찾기
                DataRow row = null;
                foreach (DataRow r in dt.Rows)
                {
                    object keyVal = r[keyColumnFieldName];
                    if (keyVal != null && keyVal.ToString() == key)
                    {
                        row = r;
                        break;
                    }
                }

                // 행이 없으면 새로 추가
                if (row == null)
                {
                    row = dt.NewRow();
                    dt.Rows.Add(row);
                }

                // 값 채우기 (VisibleColumns를 직접 순회)
                for (int i = 0; i < values.Length && i < gridView.VisibleColumns.Count; i++)
                {
                    GridColumn col = gridView.VisibleColumns[i];
                    string fieldName = col.FieldName;

                    if (!dt.Columns.Contains(fieldName))
                        continue;

                    if (dt.Columns[fieldName].ReadOnly)
                        continue;

                    row[fieldName] = values[i];
                }
            }
        }
    }
}
