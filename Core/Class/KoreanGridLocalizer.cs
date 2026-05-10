using DevExpress.XtraGrid.Localization;

public class KoreanGridLocalizer : GridLocalizer
{
    public override string GetLocalizedString(GridStringId id)
    {
        switch (id)
        {
            case GridStringId.MenuColumnSortAscending:
                return "오름차순 정렬";

            case GridStringId.MenuColumnSortDescending:
                return "내림차순 정렬";

            case GridStringId.MenuColumnClearSorting:
                return "정렬 지우기";

            case GridStringId.MenuGroupPanelShow:
                return "그룹 패널 표시";

            case GridStringId.MenuGroupPanelHide:
                return "그룹 패널 숨기기";

            case GridStringId.MenuColumnBestFit:
                return "최적 너비";

            case GridStringId.MenuColumnClearAllSorting:
                return "정렬 전체 초기화";

            case GridStringId.MenuColumnBestFitAllColumns:
                return "전체 열 최적 너비";

            case GridStringId.MenuColumnFilter:
                return "이 열 필터";

            case GridStringId.MenuFooterSum:
                return "합계";

            case GridStringId.MenuFooterMin:
                return "최소값";

            case GridStringId.MenuFooterMax:
                return "최대값";

            case GridStringId.MenuFooterCount:
                return "개수";

            case GridStringId.MenuFooterNone:
                return "없음";

            case GridStringId.MenuColumnRemoveColumn:
                return "열 숨기기";

            case GridStringId.MenuColumnColumnCustomization:
                return "숨김열 다시보기";
        }

        return base.GetLocalizedString(id);
    }
}
