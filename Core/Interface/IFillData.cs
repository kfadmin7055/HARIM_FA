using System.Data;

namespace Core.Interface
{
    /// <summary>
    /// DataSource의 데이터를 채우는 기능을 정의하는 Interface입니다.
    /// </summary>
    public interface IFillData
    {
        /// <summary>
        /// DataSource의 데이터를 채웁니다.
        /// </summary>
        void FillData();

        /// <summary>
        /// DataSource의 데이터를 채웁니다.
        /// </summary>
        /// <param name="ds">DataSource</param>
        void FillData(DataSet ds);

        /// <summary>
        /// DataSource의 데이터를 채웁니다.
        /// </summary>
        /// <param name="ds">DataSource</param>
        /// <param name="tableName">Table 명</param>
        void FillData(DataSet ds, string tableName);
    }
}
