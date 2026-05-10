using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Oracle.ManagedDataAccess.Client;

namespace Core.Class
{
    public class clsDBManager : IDisposable
    {
        // SQL 핸들 클래스 
        private OracleConnection dbconnection = null;
        private OracleCommand dbCommand = null;
        private OracleTransaction dbTransaction = null;
        public OracleConnectionStringBuilder myCSB = new OracleConnectionStringBuilder();
        public OracleDataAdapter apt = new OracleDataAdapter();

        private string ConnectString;

        private string StrRtnVal;  //상태값을 문자열로 저장  

        private bool IsConnect;

        public OracleConnection getConnection()
        {
            return dbconnection;
        }

        public bool m_IsConnect
        {
            get { return IsConnect; }
            set { IsConnect = value; }
        }

        public OracleTransaction getTransaction()
        {
            return dbTransaction;
        }

        public clsDBManager()
        {

        }

        /// <summary>
        /// 트렌잭션 시작
        /// </summary>
        public void BeginTransaction()
        {
            try
            {
                if (dbconnection.State == ConnectionState.Closed)
                {
                    this.Open();
                }

                if (this.dbTransaction == null)
                {
                    try
                    {
                        dbTransaction = dbconnection.BeginTransaction();
                    }
                    catch (OracleException ex)
                    {
                        clsLog.logSave("clsDBManager", "BeginTransaction()", ex);

                        if (ex.Message.ToString().IndexOf("Network error") != -1)
                        {
                            IsConnect = false;

                            this.Close();
                            this.Open();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDBManager", "BeginTransaction()", ex);
            }
        }

        /// <summary>
        /// 프로시저 실행
        /// </summary>
        /// <param name="argSQL">프로시저 SQL구문</param>
        /// <param name="errMsg">프로시저 반환메시지</param>
        /// <returns>프로시저 처리 카운트</returns>
        public int SQLrunProc(string argSQL, ref string errMsg)
        {
            if (dbconnection.State == ConnectionState.Closed)
            {
                this.Open();
            }

            if (!argSQL.Contains("EXEC") || !argSQL.Contains("BEGIN"))
            {
                argSQL = argSQL.Replace("''", "null");
            }

            Console.WriteLine(argSQL + "\n;");
            clsLog.logSave(argSQL + "\n;", 1);

            this.dbCommand = new OracleCommand(argSQL, dbconnection);

            dbCommand.Transaction = dbTransaction;

            try
            {
                dbCommand.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
                clsLog.logSave("clsDBManager", "SQLrun() |" + argSQL, ex);

                if (ex.Message.ToString().IndexOf("Network error") != -1)
                {
                    this.Close();

                    IsConnect = false;
                    this.Open();
                }
                errMsg = ex.Message;
                return -1;
            }
            catch (Exception exx)
            {

                clsLog.logSave("clsDBManager", "SQLrun(Exception) |" + argSQL, exx);
                errMsg = exx.Message;
                return -1;
            }

            return 1;
        }

        /// <summary>
        /// SQL구문 실행
        /// </summary>
        /// <param name="argSQL">SQL구문</param>
        /// <returns>SQL구문처리 카운트</returns>
        public int SQLrun(string argSQL)
        {
            if (dbconnection.State == ConnectionState.Closed)
            {
                this.Open();
            }

            if (!argSQL.Contains("EXEC") || !argSQL.Contains("BEGIN"))
            {
                argSQL = argSQL.Replace("''", "null");
            }

            Console.WriteLine(argSQL + "\n;");
            clsLog.logSave(argSQL + "\n;", 1);

            try
            {
                this.dbCommand = new OracleCommand(argSQL, dbconnection);

                dbCommand.Transaction = dbTransaction;

                return dbCommand.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
                clsLog.logSave("clsDBManager", "SQLrun() |" + argSQL, ex);

                if (ex.Message.ToString().IndexOf("Network error") != -1)
                {
                    this.Close();

                    IsConnect = false;
                    this.Open();
                }
            }
            catch (Exception exx)
            {

                clsLog.logSave("clsDBManager", "SQLrun(Exception) |" + argSQL, exx);

            }
            return -1;

        }

        /// <summary>
        /// 트랜잭션 커밋
        /// </summary>
        public void Commit()
        {
            try
            {
                if (dbTransaction != null)
                {
                    dbTransaction.Commit();
                    dbTransaction = null;
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDBManager", "Commit()", ex);
            }
        }

        /// <summary>
        /// 트랜잭션 롤백
        /// </summary>
        public void Rollback()
        {
            try
            {
                if (dbTransaction != null)
                {
                    dbTransaction.Rollback();

                    dbTransaction = null;
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDBManager", "Rollback()", ex);
            }
        }


        public clsDBManager(string ConString)
        {
            this.ConnectString = ConString;
        }

        /// <summary>
        /// DB 연결
        /// </summary>
        /// <returns>DB연결 유무</returns>
        public Boolean Open()
        {
            try
            {
                dbconnection = new OracleConnection();

                dbconnection.ConnectionString = ConnectString;

                // timeout 은 기본15초로 맵핑되어 있다. 

                if (dbconnection.State != ConnectionState.Open)
                {
                    dbconnection.Open();
                    this.IsConnect = true;

                    return true;
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDBManager", "Open()", ex);
                this.IsConnect = false;

                return false;
            }

            return true;
        }

        /// <summary>
        /// DB연결 초기화
        /// </summary>
        public void Dispose()
        {
            try
            {
                GC.SuppressFinalize(this);
                this.Close();
                this.dbconnection = null;
                this.dbCommand = null;
                this.dbTransaction = null;
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDBManager", "Dispose()", ex);
            }
        }

        /// <summary>
        /// DB연결 닫기
        /// </summary>
        public void Close()
        {
            if (dbconnection.State != ConnectionState.Closed)
            {
                try
                {
                    dbconnection.Close();
                }
                catch (Exception ex)
                {
                    clsLog.logSave("clsDBManager", "Close()", ex);
                }
            }
        }

        /// <summary>
        /// DB상태 반환
        /// </summary>
        /// <returns>Broken, Closed, Connecting, Executing, Fetching, Open 반환</returns>
        public string getState()
        {
            try
            {
                if (dbconnection != null)
                {
                    switch (dbconnection.State)
                    {

                        case ConnectionState.Broken:
                            StrRtnVal = "Broken";
                            break;

                        case ConnectionState.Closed:
                            StrRtnVal = "Closed";
                            break;
                        case ConnectionState.Connecting:
                            StrRtnVal = "Connecting";
                            break;
                        case ConnectionState.Executing:
                            StrRtnVal = "Executing";
                            break;
                        case ConnectionState.Fetching:
                            StrRtnVal = "Fetching";
                            break;
                        case ConnectionState.Open:
                            StrRtnVal = "Open";
                            break;
                    }
                }
                else
                {
                    StrRtnVal = "Not Connecting";
                }
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDBManager", "getState()", ex);
                StrRtnVal = "Error";
            }

            return StrRtnVal;

        }

        /// <summary>
        /// DataSet Row Count 가져오기
        /// </summary>
        /// <param name="argSet">데이터셋</param>
        /// <returns>Row Count 반환</returns>
        public int getRowCnt(DataSet argSet)
        {

            if (argSet == null)
            {
                return 0;
            }

            try
            {
                return argSet.Tables[0].Rows.Count;
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDBManager", "getRowCnt", ex);
                return -1;

            }
        }

        /// <summary>
        /// DataSet 데이터 읽기
        /// </summary>
        /// <param name="argSet">데이터셋</param>
        /// <param name="argColName">컬러명</param>
        /// <param name="argidx">로우 인덱스</param>
        /// <returns>DataSet 값 반환</returns>
        public string getData(DataSet argSet, string argColName, int argidx)
        {
            if (argSet.Tables[0].Rows.Count <= 0)
            {
                return null;
            }

            try
            {
                return argSet.Tables[0].Rows[argidx][argColName].ToString();
            }
            catch (OracleException ex)
            {
                clsLog.logSave("clsDBManager", "getData()", ex);
                return null;
            }
            catch (Exception exx)
            {
                clsLog.logSave("clsDBManager", "getData(Exception)", exx);
                return null;

            }

        }

        public OracleDataReader ExecuteReader(string argQuery, List<OracleParameter> param = null)
        {
            OracleDataReader reader = null;

            try
            {
                // DB 연결 확인
                if (dbconnection.State == ConnectionState.Closed)
                {
                    this.Open();
                    clsLog.logSave("ExecuteReader db reconnect...", 1);
                }

                if (string.IsNullOrEmpty(argQuery))
                    return null;

                Console.WriteLine(argQuery + "\n;");
                clsLog.logSave(argQuery + "\n;", 1);

                // 명령 생성
                this.dbCommand = new OracleCommand(argQuery, dbconnection);
                this.dbCommand.Transaction = dbTransaction;
                this.dbCommand.BindByName = true;

                // 파라미터 추가
                if (param != null)
                {
                    foreach (var p in param)
                    {
                        this.dbCommand.Parameters.Add(p);
                    }
                }

                // CommandBehavior.Default or SequentialAccess (BLOB 용)
                reader = this.dbCommand.ExecuteReader(CommandBehavior.SequentialAccess);

                return reader;
            }
            catch (OracleException ex)
            {
                clsLog.logSave("clsDBManager", "ExecuteReader(OracleException)", ex);

                if (ex.Message.Contains("Network error"))
                {
                    this.Close();
                    IsConnect = false;
                    this.Open();
                    clsLog.logSave("재연결시도(Network error)", 1);
                }

                return null;
            }
            catch (Exception exx)
            {
                clsLog.logSave("clsDBManager", "ExecuteReader(Exception)", exx);

                this.Close();
                IsConnect = false;
                this.Open();

                return null;
            }
        }

        /// <summary>
        /// INSERT, UPDATE, DELETE 용 ExecuteNonQuery
        /// </summary>
        public int ExecuteNonQuery(string argQuery, List<OracleParameter> param = null)
        {
            int result = 0;

            try
            {
                // 연결이 끊겨 있으면 다시 연결
                if (dbconnection.State == ConnectionState.Closed)
                {
                    this.Open();
                    clsLog.logSave("ExecuteNonQuery db reconnect...", 1);
                }

                if (string.IsNullOrEmpty(argQuery))
                    return 0;

                if (!argQuery.Contains("EXEC"))
                {
                    argQuery = argQuery.Replace("''", "null");
                }

                Console.WriteLine(argQuery + "\n;");

                clsLog.logSave(argQuery + "\n;", 1);

                this.dbCommand = new OracleCommand(argQuery, dbconnection);
                this.dbCommand.Transaction = dbTransaction;
                this.dbCommand.BindByName = true;     // Oracle에서는 필수

                // 파라미터가 있으면 추가
                if (param != null)
                {
                    foreach (var p in param)
                    {
                        this.dbCommand.Parameters.Add(p);
                    }
                }

                result = this.dbCommand.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
                clsLog.logSave("clsDBManager", "ExecuteNonQuery()", ex);

                if (ex.Message.ToString().IndexOf("Network error") != -1)
                {
                    this.Close();
                    IsConnect = false;

                    this.Open();
                    clsLog.logSave("재연결시도(Network error)", 1);
                }
            }
            catch (Exception exx)
            {
                this.Close();
                IsConnect = false;

                this.Open();
                clsLog.logSave("clsDBManager", "ExecuteNonQuery(Exception) |" + argQuery, exx);
            }

            return result;
        }


        /// <summary>
        /// 데이터셋 가져오기
        /// </summary>
        /// <param name="argQuery">SQL구문</param>
        /// <param name="table">DataSet 테이블</param>
        /// <returns>SQL구문에 대한 데이터셋 반환</returns>
        public DataSet ExecutDataset(String argQuery, String table = "")
        {

            if (dbconnection.State == ConnectionState.Closed)
            {
                this.Open();

                clsLog.logSave("ExecutDataset db reconnect...", 1);
            }

            if (argQuery == null || argQuery == "") return null;

            if (!argQuery.Contains("EXEC"))
            {
                argQuery = argQuery.Replace("''", "null");
            }

            Console.WriteLine(argQuery + "\n;");

            //if (!argQuery.Contains("SELECT"))
            clsLog.logSave(argQuery + "\n;", 1);

            this.dbCommand = new OracleCommand(argQuery, dbconnection);
            this.dbCommand.Transaction = dbTransaction;
            //this.dbCommand.CommandTimeout = 0;

            /*            this.dbCommand.Connection = dbconnection;
                        this.dbCommand.CommandText = argQuery;
                        this.dbCommand.CommandType = CommandType.Text;*/

            OracleDataAdapter adapter = new OracleDataAdapter();
            adapter.SelectCommand = dbCommand;

            DataSet dataSet = new DataSet();

            try
            {
                if (table != "")
                {

                    adapter.Fill(dataSet, table);
                }
                else
                {
                    adapter.Fill(dataSet);

                }
            }
            catch (OracleException ex)
            {

                clsLog.logSave("clsDBManager", "ExecutDataset()", ex);

                if (ex.Message.ToString().IndexOf("Network error") != -1)
                {
                    this.Close();

                    IsConnect = false;

                    this.Open();

                    clsLog.logSave("재연결시도(Network error)", 1);
                }


            }
            catch (Exception exx)
            {
                this.Close();

                IsConnect = false;

                this.Open();
                clsLog.logSave("clsDBManager", "ExecutDataset(Exception) |" + argQuery, exx);

            }

            return dataSet;

        }

        /// <summary>
        /// 데이터셋 가져오기
        /// </summary>
        /// <param name="argQuery">SQL구문</param>
        /// <param name="argDataSet">데이터셋</param>
        /// <param name="table">테이블</param>
        /// <returns>SQL구문 데이터셋 반환</returns>
        public DataSet ExecutDataset2(String argQuery, DataSet argDataSet, String table = "")
        {

            if (dbconnection.State == ConnectionState.Closed)
            {
                Open();
            }

            if (argQuery == null || argQuery == "") return null;

            argQuery = argQuery.Replace("''", "null");

            this.dbCommand = new OracleCommand();
            this.dbCommand.Connection = dbconnection;
            this.dbCommand.CommandText = argQuery;
            this.dbCommand.CommandType = CommandType.Text;

            OracleDataAdapter adapter = new OracleDataAdapter();
            adapter.SelectCommand = dbCommand;

            try
            {
                if (table != "")
                {

                    adapter.Fill(argDataSet, table);
                }
                else
                {
                    adapter.Fill(argDataSet);
                }


            }
            catch (OracleException ex)
            {

                clsLog.logSave("clsDBManager", "ExecutDataset()", ex);

                if (ex.Message.ToString().IndexOf("Network error") != -1)
                {
                    this.Close();

                    IsConnect = false;


                    this.Open();

                    clsLog.logSave("재연결시도(Network error)", 1);
                }


            }
            catch (Exception exx)
            {
                clsLog.logSave("clsDBManager", "ExecutDataset(Exception) |" + argQuery, exx);

            }

            return argDataSet;

        }

        public object ExecuteScalar(string query)
        {
            if (dbconnection.State == ConnectionState.Closed)
            {
                Open();
            }

            query = query.Replace("''", "null");
            this.dbCommand = new OracleCommand(query, dbconnection);

            dbCommand.Transaction = dbTransaction;

            try
            {
                object result = dbCommand.ExecuteScalar();
                if (result == DBNull.Value)
                {
                    return null; // 쿼리 결과가 NULL인 경우
                }
                return result;
            }
            catch (OracleException ex)
            {
                clsLog.logSave("clsDBManager", "ExecuteScalar() |" + query, ex);

                if (ex.Message.ToString().IndexOf("Network error") != -1)
                {
                    this.Close();
                    IsConnect = false;
                    this.Open();
                }
                return null;
            }
            catch (Exception exx)
            {
                clsLog.logSave("clsDBManager", "ExecuteScalar(Exception) |" + query, exx);
                return null;
            }
        }

        /// <summary>
        /// DataTable 가져오기
        /// </summary>
        /// <param name="argQuery">SQL구문</param>
        /// <param name="table">테이블명</param>
        /// <returns>처리행수 반환</returns>
        public int ExecutDataTable(String argQuery, DataTable table)
        {
            int rownum = 0;

            if (argQuery == null || argQuery == "") return -1;

            if (dbconnection.State == ConnectionState.Closed)
            {
                Open();
            }

            try
            {

                table.Clear();

                argQuery = argQuery.Replace("''", "null");

                this.dbCommand = new OracleCommand();
                this.dbCommand.Connection = dbconnection;
                this.dbCommand.CommandText = argQuery;
                this.dbCommand.CommandType = CommandType.Text;

                OracleDataAdapter adapter = new OracleDataAdapter();

                rownum = adapter.Fill(table);

            }
            catch (OracleException ex)
            {

                clsLog.logSave("clsDBManager", "ExecutDataTable(SqlException) |" + argQuery, ex);

            }
            catch (Exception exx)
            {

                clsLog.logSave("clsDBManager", "ExecutDataTable(Exception) |" + argQuery, exx);

                return -1;
            }

            return rownum;

        }

        public byte[] ReadFile(string sPath)
        {
            byte[] data = null;
            try
            {
                FileInfo fInfo = new FileInfo(sPath);
                long numBytes = fInfo.Length;
                FileStream fStream = new FileStream(sPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryReader br = new BinaryReader(fStream);
                data = br.ReadBytes((int)numBytes);
                fStream.Close();
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDBManager", MethodBase.GetCurrentMethod().Name, ex);
            }

            return data;
        }

        public int Insert_Blob(string qry, byte[] blob_sign)
        {
            try
            {
                OracleParameter blobParameter = new OracleParameter();
                blobParameter.OracleDbType = OracleDbType.Blob;
                blobParameter.ParameterName = "SIGN";
                blobParameter.Value = blob_sign;
                this.dbCommand = new OracleCommand(qry, dbconnection);
                dbCommand.Parameters.Add(blobParameter);
            }
            catch (Exception ex)
            {
                clsLog.logSave("clsDBManager", MethodBase.GetCurrentMethod().Name, ex);
            }
            return dbCommand.ExecuteNonQuery();
        }


    }
}
