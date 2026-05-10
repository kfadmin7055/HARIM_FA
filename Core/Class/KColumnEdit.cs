using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Class
{
    public enum XColumnEdit
    {
        /// <summary>
        /// 문자열
        /// </summary>
        TEXT,
        /// <summary>
        /// 숫자형
        /// </summary>
        NUM,
        /// <summary>
        /// 통화형
        /// </summary>
        CURRENCY,
        /// <summary>
        /// CHECK BOX
        /// </summary>
        CHECK,
        /// <summary>
        /// 달력(연월일)
        /// </summary>
        DATE,
        /// <summary>
        /// 달력(연월)
        /// </summary>
        DATEMONTH,
        /// <summary>
        /// 시간
        /// </summary>
        DATETIME,
        /// <summary>
        /// 콤보리스트
        /// </summary>
        COMBO,
        /// <summary>
        /// 버튼
        /// </summary>
        BUTTON
    }

    public class RepoColumnEdit
    {
        public virtual RepositoryItem Repository(DataTable dt, int iWidth = 200)
        {
            RepositoryItem repositoryItem = new RepositoryItem();

            return repositoryItem;
        }
    }

    /// <summary>
    /// 문자열
    /// </summary>
    public class RepoColumnEditText : RepoColumnEdit
    {
        public override RepositoryItem Repository(DataTable dt, int iWidth = 200)
        {
            RepositoryItemTextEdit repositoryItem = new RepositoryItemTextEdit();

            return repositoryItem;
        }
    }

    /// <summary>
    /// 숫자형 ##0.##
    /// </summary>
    public class RepoColumnEditNum : RepoColumnEdit
    {
        public override RepositoryItem Repository(DataTable dt, int iWidth = 200)
        {
            RepositoryItemTextEdit repositoryItem = new RepositoryItemTextEdit();

            repositoryItem.AutoHeight = false;
            repositoryItem.DisplayFormat.FormatString = "##0.##";
            repositoryItem.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            repositoryItem.Mask.EditMask = "##0.##";
            repositoryItem.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;

            return repositoryItem;
        }
    }

    /// <summary>
    /// 통화
    /// </summary>
    public class RepoColumnEditCURRENCY : RepoColumnEdit
    {
        public override RepositoryItem Repository(DataTable dt, int iWidth = 200)
        {
            RepositoryItemTextEdit repositoryItem = new RepositoryItemTextEdit();

            //repositoryItem.AutoHeight = false;
            //repositoryItem.DisplayFormat.FormatString = "#,##0";
            //repositoryItem.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            //repositoryItem.EditFormat.FormatString = "#,##0";
            //repositoryItem.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            //repositoryItem.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            //repositoryItem.MaskSettings.Set("MaskManagerSignature", "allowNull=False");
            //repositoryItem.MaskSettings.Set("mask", "#,##0");
            //repositoryItem.Name = "repoCurrency";
            //repositoryItem.UseMaskAsDisplayFormat = true;

            repositoryItem.AutoHeight = false;
            //repositoryItem.DisplayFormat.FormatString = "#,##0";
            //repositoryItem.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            //repositoryItem.EditFormat.FormatString = "#,##0";
            //repositoryItem.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            repositoryItem.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            repositoryItem.MaskSettings.Set("MaskManagerSignature", "allowNull=False");
            repositoryItem.MaskSettings.Set("mask", "c");
            repositoryItem.Name = "repoCurrency";
            repositoryItem.UseMaskAsDisplayFormat = true;

            return repositoryItem;
        }
    }

    /// <summary>
    /// 체크박스
    /// </summary>
    public class RepoColumnEditCheck : RepoColumnEdit
    {
        public override RepositoryItem Repository(DataTable dt, int iWidth = 200)
        {
            RepositoryItemCheckEdit repositoryItem = new RepositoryItemCheckEdit();

            repositoryItem.AutoHeight = false;
            repositoryItem.DisplayValueGrayed = "N";
            repositoryItem.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            repositoryItem.ValueChecked = "Y";
            repositoryItem.ValueUnchecked = "N";

            return repositoryItem;
        }
    }

    /// <summary>
    /// 달력 일자
    /// </summary>
    public class RepoColumnEditDate : RepoColumnEdit
    {
        public override RepositoryItem Repository(DataTable dt, int iWidth = 200)
        {
            RepositoryItemDateEdit repositoryItem = new RepositoryItemDateEdit();

            repositoryItem.AutoHeight = false;
            repositoryItem.MaskSettings.Set("mask", "yyyy-MM-dd");
            repositoryItem.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});

            return repositoryItem;
        }
    }

    /// <summary>
    /// 달력 연월
    /// </summary>
    public class RepoColumnEditDateMonth : RepoColumnEdit
    {
        public override RepositoryItem Repository(DataTable dt, int iWidth = 200)
        {
            RepositoryItemDateEdit repositoryItem = new RepositoryItemDateEdit();

            repositoryItem.AutoHeight = false;
            //repositoryItem.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            //new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            //repositoryItem.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            //new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            
            repositoryItem.DisplayFormat.FormatString = "yyyy-MM";
            repositoryItem.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            repositoryItem.EditFormat.FormatString = "yyyy-MM";
            repositoryItem.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            repositoryItem.MaskSettings.Set("mask", "yyyy-MM");
            //repositoryItem.UseMaskAsDisplayFormat = true;
            repositoryItem.Name = "repoDateMonth";
            //repositoryItem.UseMaskAsDisplayFormat = true;
            //repositoryItem.VistaCalendarInitialViewStyle = DevExpress.XtraEditors.VistaCalendarInitialViewStyle.YearView;
            repositoryItem.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearView;

            return repositoryItem;
        }
    }

    /// <summary>
    /// 달력 시간
    /// </summary>
    public class RepoColumnEditDateTime : RepoColumnEdit
    {
        public override RepositoryItem Repository(DataTable dt, int iWidth = 200)
        {
            RepositoryItemDateTimeOffsetEdit repositoryItem = new RepositoryItemDateTimeOffsetEdit();

            repositoryItem.AutoHeight = false;
            //repositoryItem.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            //new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            repositoryItem.DisplayFormat.FormatString = "HH:mm:ss";
            repositoryItem.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            repositoryItem.EditFormat.FormatString = "HH:mm:ss";
            repositoryItem.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            repositoryItem.MaskSettings.Set("mask", "HH:mm:ss");
            repositoryItem.Name = "repoTime_ST";

            return repositoryItem;
        }
    }

    /// <summary>
    /// 콤보박스
    /// </summary>
    public class RepoColumnEditComboBox : RepoColumnEdit
    {
        public override RepositoryItem Repository(DataTable dt, int iWidth = 200)
        {
            RepositoryItemLookUpEdit repositoryItem = new RepositoryItemLookUpEdit
            {

                //resources.ApplyResources(this.repositoryItem, "repositoryItem");
                //            repositoryItem.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                //new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("repositoryItem.Buttons"))))});
                //            repositoryItem.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
                //new DevExpress.XtraEditors.Controls.LookUpColumnInfo(resources.GetString("repositoryItem.Columns"), resources.GetString("repositoryItem.Columns1"))});

                TextEditStyle = TextEditStyles.Standard,
                SearchMode = SearchMode.AutoFilter,
            };
            repositoryItem.TextEditStyle = TextEditStyles.DisableTextEditor;

            repositoryItem.AutoHeight = false;
            //repositoryItem.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            //new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            repositoryItem.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "CODE"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "NAME")});
            repositoryItem.DisplayMember = "NAME";
            repositoryItem.DropDownRows = 10;
            repositoryItem.PopupWidth = iWidth;
            
            repositoryItem.ValueMember = "CODE";
            repositoryItem.DataSource = dt;
            repositoryItem.ShowHeader = true;
            

            repositoryItem.BestFit();
            //repositoryItem.SearchMode = SearchMode.AutoComplete;

            repositoryItem.NullText = "";
            repositoryItem.NullValuePrompt = "";

            if (repositoryItem.Columns[repositoryItem.ValueMember] != null)
                repositoryItem.Columns[repositoryItem.ValueMember].Visible = false;

            return repositoryItem;
        }
    }

    ///// <summary>
    ///// 라디오 버튼
    ///// </summary>
    //class RepoColumnEditRadioButton : RepoColumnEdit
    //{
    //    public override RepositoryItem Repository()
    //    {
    //        RepositoryItemRadioGroup repositoryItem = new RepositoryItemRadioGroup();

    //        return repositoryItem;
    //    }
    //}

    /// <summary>
    /// 버튼
    /// </summary>
    public class RepoColumnEditButton : RepoColumnEdit
    {
        public override RepositoryItem Repository(DataTable dt, int iWidth = 200)
        {
            RepositoryItemButtonEdit repositoryItem = new RepositoryItemButtonEdit();

            repositoryItem.AutoHeight = false;
            repositoryItem.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});

            return repositoryItem;
        }
    }
}
