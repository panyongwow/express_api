using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient; 

/// <summary>
///DB 的摘要说明
/// </summary>
public class DB
{
    private static DB UniqueInstance;
    private Sys.enum_DB_Driver pDBType;

    private IDbConnection pCon;
    private IDbCommand pCmd;
    private IDataReader pDR;
    //private IDataAdapter pDA;
    private SqlDataAdapter pDA;

    private IDbConnection pCon_log;
    private IDbCommand pCmd_log;

    public static DB GetInstance(Sys.enum_DB_Driver DBType)
    {
        if (UniqueInstance == null) UniqueInstance = new DB(DBType);
        return UniqueInstance;
    }

    private DB(Sys.enum_DB_Driver DBType)
    {
        this.pDBType = DBType;

        if (DBType == Sys.enum_DB_Driver.SQL2000)
        {
            this.pCon = new SqlConnection();
            this.pCmd = new SqlCommand();
            this.pDA = new SqlDataAdapter();
            this.pCon_log = new SqlConnection();
            this.pCmd_log = new SqlCommand();
        }
        else if (DBType == Sys.enum_DB_Driver.Oracle)
        {
        }
        this.pCmd.Connection = this.pCon;
        this.pCmd.CommandType = CommandType.Text;
        this.pCmd_log.Connection = this.pCon_log;
        this.pCmd_log.CommandType = CommandType.Text;
   }

    public int ExecNoQuery(String SQL)
    {
        //try
        //{
        //    if (this.pDR != null)
        //    {
        //        if (!this.pDR.IsClosed) this.pDR.Close();
        //    }
        //    writeSQL(SQL);
        //    ConOpen();
        //    this.pCmd.CommandText = SQL;
        //    return this.pCmd.ExecuteNonQuery();

        //}
        //catch (Exception ex)
        //{
        //    if (this.pCon.State == ConnectionState.Open) this.pCon.Close();
        //    throw new DB_Exception(ex.Message + " SQLStr:" + SQL);
        //}
        using (SqlConnection connection = getConnection())
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                LogHelper.WriteLog_Info(typeof(SqlCommand), "ExecNoQuery:" + SQL);
                cmd.Connection = connection;
                cmd.CommandText = SQL;
                int result=0;
                try
                {
                    result = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog_Error(typeof(SqlCommand), ex);
                    throw ex;

                }
                finally
                {
                    connection.Close();
                }
                return result;
            }
        }
    }

    public int ExecNoQuery1(String SQL)
    {
        writeSQL(SQL);
        int count = 0;
        using (SqlConnection connection = new SqlConnection(Sys.ConStr))
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = SQL;
                cmd.Connection = connection;

                cmd.CommandText = SQL;
                count = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DB_Exception(ex.Message + " SQLStr:" + SQL);
            }
        }
        return count;
    }

    public IDataReader ExecDataReader(String SQL)
    {
        try
        {
            if (this.pDR != null)
            {
                if (!this.pDR.IsClosed) this.pDR.Close();
            }
            writeSQL(SQL);

            ConOpen();

            this.pCmd.CommandText = SQL;
            pDR = pCmd.ExecuteReader();
            return pDR;
        }
        catch (Exception ex)
        {
            if (this.pDR != null)
            {
                if (!this.pDR.IsClosed) this.pDR.Close();
            }
           
            if (this.pCon.State == ConnectionState.Open) this.pCon.Close();
            throw new DB_Exception(ex.Message + " SQLStr:" + SQL);
        }
    }

    public object ExecScalar(string SQL)
    {
        //try
        //{
        //    if (this.pDR != null)
        //    {
        //        if (!this.pDR.IsClosed) this.pDR.Close();
        //    }
        //    writeSQL(SQL);

        //    ConOpen();
        //    this.pCmd.CommandText = SQL;
        //    return this.pCmd.ExecuteScalar();
        //}
        //catch (Exception ex)
        //{
        //    if (this.pCon.State == ConnectionState.Open) this.pCon.Close();
        //    throw new DB_Exception(ex.Message + " SQLStr:" + SQL);
        //}

        using (SqlConnection connection = getConnection())
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                LogHelper.WriteLog_Info(typeof(SqlCommand), "ExecScalar:" + SQL);
                cmd.Connection = connection;
                cmd.CommandText = SQL;
                object result;
                try
                {
                    result = cmd.ExecuteScalar();
                    if (result == null)
                    {
                        LogHelper.WriteLog_Info(typeof(SqlCommand), "ExecScalarResult:null");
                    }
                    else
                    {
                        LogHelper.WriteLog_Info(typeof(SqlCommand), "ExecScalarResult:" + result.ToString());
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog_Error(typeof(SqlCommand), ex);
                    throw ex;

                }
                finally
                {
                    connection.Close();
                }
                return result;
            }
        }
    }

    public DataTable ExecDataTable(String SQL, string TableName)
    {
       // writeSQL(SQL);

        using (SqlConnection connection = getConnection())
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                LogHelper.WriteLog_Info(typeof(SqlCommand), "ExecTable:" + SQL);
                cmd.Connection = connection;
                cmd.CommandText = SQL;
                DataTable returnDataTable = new DataTable(TableName);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {


                    try
                    {
                        DataSet returnDataSet = new DataSet(TableName);

                        //(pDA as SqlDataAdapter).SelectCommand = cmd as SqlCommand;
                        //(pDA as SqlDataAdapter).Fill(returnDataTable);

                        da.SelectCommand = cmd as SqlCommand;
                        da.Fill(returnDataTable);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteLog_Error(typeof(SqlCommand), ex);
                        throw ex;

                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                return returnDataTable;

            }
        }
    }

    public DataTable ExecDataTable(String SQL)
    {
        // writeSQL(SQL);

        using (SqlConnection connection = getConnection())
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                LogHelper.WriteLog_Info(typeof(SqlCommand), "ExecTable:" + SQL);
                cmd.Connection = connection;
                cmd.CommandText = SQL;
                DataTable returnDataTable = new DataTable("a");
                try
                {
                    DataSet returnDataSet = new DataSet("a");

                    (pDA as SqlDataAdapter).SelectCommand = cmd as SqlCommand;
                    (pDA as SqlDataAdapter).Fill(returnDataTable);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog_Error(typeof(SqlCommand), ex);
                    throw ex;

                }
                finally
                {
                    connection.Close();
                }
                return returnDataTable;

            }
        }
    }

    public DataTable ExecDataTable_Share(String SQL, string TableName)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(Sys.ConStr))
            {
                DataTable returnDataTable = new DataTable(TableName);
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = SQL;
                    cmd.Connection = connection;

                    DataSet returnDataSet = new DataSet(TableName);

                    (pDA as SqlDataAdapter).SelectCommand = cmd as SqlCommand;
                    (pDA as SqlDataAdapter).Fill(returnDataTable);
                }
                catch { }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
                return returnDataTable;

            }
        }
        catch (Exception ex)
        {
            if (this.pCon.State == ConnectionState.Open) this.pCon.Close();
            throw new DB_Exception(ex.Message + " DB_Search.ExecDataTable_SQLStr:" + SQL);
        }
    }
    //使用共享连接来获取数据，使用在打印阶段，需要获取之前填充在临时表里的数据，所以需采用共享连接
    public DataTable ExecDataTable_LastCon(String SQL,string TableName)
    {
        try
        {
            if (this.pDR != null)
            {
                if (!this.pDR.IsClosed) this.pDR.Close();
            }
            writeSQL(SQL);

            ConOpen();
            pCmd.CommandText = SQL;

            DataSet returnDataSet=new DataSet(TableName);
            DataTable returnDataTable = new DataTable(TableName);
            if (this.pDBType == Sys.enum_DB_Driver.SQL2000)
            {
                (pDA as SqlDataAdapter).SelectCommand = pCmd as SqlCommand;
                (pDA as SqlDataAdapter).Fill(returnDataTable);
            }
            else if (this.pDBType == Sys.enum_DB_Driver.Oracle)
            {
            }
            return returnDataTable;
        }
        catch (Exception ex)
        {
            if (this.pCon.State == ConnectionState.Open) this.pCon.Close();
            throw new DB_Exception(ex.Message + " SQLStr:" + SQL);
        }
    }

    /// <summary>
    /// 打开数据库连接
    /// </summary>
    protected static SqlConnection getConnection()
    {
        try
        {
            SqlConnection conn = new SqlConnection(Sys.ConStr);
            conn.Open();
            return conn;
        }
        catch (SqlException e)
        {

            throw e;
        }
    }

    public string ConOpen()
    {
        try
        {
            if (this.pCon.State != ConnectionState.Open)
            {
                this.pCon.ConnectionString = Sys.ConStr;
                this.pCon.Open();
            }
            return "OK";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public void ConClose()
    {
        if (this.pCon.State == ConnectionState.Open) this.pCon.Close();
    }

    private string Con_logOpen()
    {
        try
        {
            if (this.pCon_log.State != ConnectionState.Open)
            {
                this.pCon_log.ConnectionString = Sys.ConStr;
                this.pCon_log.Open();
            }
            return "OK";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public void Con_logClose()
    {
        if (this.pCon_log.State == ConnectionState.Open) this.pCon_log.Close();
    }

    /// <summary>
    /// 将执行的SQL语句写入SQL日志表
    /// </summary>
    /// <param name="SQL"></param>
    private void writeSQL(string SQL)
    {
        try
        {
            //SQL = SQL.Replace("'", "''");
            //Con_logOpen();
            //this.pCmd_log.CommandText ="insert into SQL_Log(SQL,times,source) values('"+SQL+"',getdate(),0)";
            //this.pCmd_log.ExecuteNonQuery();

        }
        catch (Exception ex)
        {
            if (this.pCon_log.State == ConnectionState.Open) this.pCon_log.Close();
        }
    }
}