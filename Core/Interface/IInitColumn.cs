using Core.Class;
using Core.Enum;

namespace Core.Interface
{
    /// <summary>
    /// Column Initialize 기능을 정의하는 Interface입니다.
    /// </summary>
    public interface IInitColumn
    {
        /// <summary>
        /// Grid Column을 초기화합니다.
        /// </summary>
        /// <param name="fieldName">Column Field 명</param>
        /// <param name="caption">Column Header Text</param>
        void InitColumn(string fieldName, string caption);

        /// <summary>
        /// Grid Column을 초기화합니다.
        /// </summary>
        /// <param name="fieldName">Column Field 명</param>
        /// <param name="caption">Column Header Text</param>
        /// <param name="width">Column 너비</param>
        void InitColumn(string fieldName, string caption, int width);

        /// <summary>
        /// Grid Column을 초기화합니다.
        /// </summary>
        /// <param name="fieldName">Column Field 명</param>
        /// <param name="caption">Column Header Text</param>
        /// <param name="width">Column 너비</param>
        /// <param name="maxLength">Data의 최대 길이, 0이면 설정 안 함</param>
        /// <param name="allowEdit">Column Cell 수정 여부</param>
        /// <param name="visible">Column 숨김/보임 여부</param>
        void InitColumn(string fieldName, string caption, int width, int maxLength, bool allowEdit, bool visible);

        /// <summary>
        /// Grid Column을 초기화합니다.
        /// </summary>
        /// <param name="fieldName">Column Field 명</param>
        /// <param name="caption">Column Header Text</param>
        /// <param name="width">Column 너비</param>
        /// <param name="maxLength">Data의 최대 길이, 0이면 설정 안 함</param>
        /// <param name="allowEdit">Column Cell 수정 여부</param>
        /// <param name="visible">Column 숨김/보임 여부</param>
        /// <param name="dataType">Column DataType</param>
        void InitColumn(string fieldName, string caption, int width, int maxLength, bool allowEdit, bool visible, DataType dataType);

        /// <summary>
        /// Grid Column을 초기화합니다.
        /// </summary>
        /// <param name="fieldName">Column Field 명</param>
        /// <param name="caption">Column Header Text</param>
        /// <param name="width">Column 너비</param>
        /// <param name="maxLength">Data의 최대 길이, 0이면 설정 안 함</param>
        /// <param name="allowEdit">Column Cell 수정 여부</param>
        /// <param name="visible">Column 숨김/보임 여부</param>
        /// <param name="dataType">Column DataType</param>
        /// <param name="horzAlign">Column Cell 정렬</param>
        void InitColumn(string fieldName, string caption, int width, int maxLength, bool allowEdit, bool visible, DataType dataType, HorzAlign horzAlign);

        /// <summary>
        /// Grid Column을 초기화합니다.
        /// </summary>
        /// <param name="fieldName">Column Field 명</param>
        /// <param name="caption">Column Header Text</param>
        /// <param name="width">Column 너비</param>
        /// <param name="maxLength">Data의 최대 길이, 0이면 설정 안 함</param>
        /// <param name="decimalPlace">소숫점 길이 수, 기본값 0</param>
        /// <param name="allowEdit">Column Cell 수정 여부</param>
        /// <param name="visible">Column 숨김/보임 여부</param>
        /// <param name="dataType">Column DataType</param>
        /// <param name="horzAlign">Column Cell 정렬</param>
        void InitColumn(string fieldName, string caption, int width, int maxLength, int decimalPlace, bool allowEdit, bool visible, DataType dataType, HorzAlign horzAlign);
    }
}
