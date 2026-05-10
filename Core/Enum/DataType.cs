namespace Core.Enum
{
    /// <summary>
    /// 시스템에서 사용할 Data Type을 정의합니다.
    /// </summary>
    /// <remarks>
    /// 2023-02-01 최초생성 : 오인봉
    /// 변경내역
    /// 
    /// </remarks>
    public enum DataType
    {
        /// <summary>
        /// 기본(String) 형식
        /// </summary>
        Default,
        /// <summary>
        /// Check 형식
        /// </summary>
        CheckEdit,
        /// <summary>
        /// CheckedComboBoxEdit 형식
        /// </summary>
        CheckedComboBox,
        /// <summary>
        /// ComboBoxEdit 형식
        /// </summary>
        ComboBox,
        /// <summary>
        /// 날짜형식 yyyy-MM-dd
        /// </summary>
        Date,
        /// <summary>
        /// 날짜형식 yyyy-MM-dd HH:mm:ss
        /// </summary>
        DateTime,
        /// <summary>
        /// 시간형식 HH:mm:ss
        /// </summary>
        Time,
        /// <summary>
        /// 시간형식 HH:mm
        /// </summary>
        TimeHM,
        /// <summary>
        /// 시간형식 HH:mm:ss.fff
        /// </summary>
        TimeHMMS,
        /// <summary>
        /// 숫자형식
        /// </summary>
        Number,
        /// <summary>
        /// 통화 형식
        /// </summary>
        Currency,
        /// <summary>
        /// 퍼센트 형식
        /// </summary>
        Percentage,
        /// <summary>
        /// 팝업 컨트롤 형식
        /// </summary>
        Popup,
        /// <summary>
        /// Button 형식
        /// </summary>
        Button,
        /// <summary>
        /// Button Edit 형식
        /// </summary>
        ButtonEdit,
        /// <summary>
        /// LinkButtonEdit 형식
        /// </summary>
        LinkButton,
        /// <summary>
        /// Color 선택 형식
        /// </summary>
        ColorSelect,
        /// <summary>
        /// 그림 형식(클릭 후 보임)
        /// </summary>
        Image,
        /// <summary>
        /// 그림형식(미리보기)
        /// </summary>
        Picture,
        /// <summary>
        /// 파일 경로 형식(FileOpenDialog 사용)
        /// </summary>
        File,
        /// <summary>
        /// Memo 형식(MemoEx Edit 사용, 아이콘 없음)
        /// </summary>
        Memo,
        /// <summary>
        /// Memo 형식(MemoEx Edit 사용, 아이콘 표시)
        /// </summary>
        MemoEx,
        /// <summary>
        /// 비밀번호 형식(******)
        /// </summary>
        Password,
        /// <summary>
        /// ProgessBar 형태
        /// </summary>
        Progress,
        /// <summary>
        /// Marquee Progress 형태
        /// </summary>
        MarqueeProgress,
        /// <summary>
        /// 다국어 입력 형태
        /// </summary>
        Language
    }
}
