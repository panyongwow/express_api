using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
///上游平台相关类
/// </summary>
public class DBRun_Service
{
	public DBRun_Service()
	{
	}

    public static int getnum(string wherestr)
    {
        DB RunDB = DB.GetInstance(Sys.DB_Driver);

        if (Sys.DB_Driver == Sys.enum_DB_Driver.SQL2000)
        {
            if (wherestr.Length > 0) wherestr = " where " + wherestr;
            return (int)RunDB.ExecScalar("select count(0) from service " + wherestr);
        }
        else if (Sys.DB_Driver == Sys.enum_DB_Driver.Oracle)
        {
        }
        return 0;
    }

    /// <summary>获得上游平台</summary>
    /// <param name="minrow"></param>
    /// <param name="maxrow"></param>
    /// <param name="wherestr"></param>
    /// <returns></returns>
    public static DataTable list(int minrow, int maxrow, string wherestr)
    {
        DB RunDB = DB.GetInstance(Sys.DB_Driver);

        if (Sys.DB_Driver == Sys.enum_DB_Driver.SQL2000)
        {
            if (wherestr.Length > 0) wherestr = " where " + wherestr;
            return RunDB.ExecDataTable("select * from " +
                                       " ( " +
                                       " select ROW_NUMBER() over (order by id) as counts,  ID,name,remark from service " + wherestr +
                                       " ) a where counts>=" + minrow + " and counts<=" + maxrow);
        }
        else if (Sys.DB_Driver == Sys.enum_DB_Driver.Oracle)
        {
        }
        return null;
    }

    /// <summary>添加上游平台</summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static int add(string name,string remark)
    {
        DB RunDB = DB.GetInstance(Sys.DB_Driver);
        if (Sys.DB_Driver == Sys.enum_DB_Driver.SQL2000)
        {
            if (Sys.CNulltoInt0(RunDB.ExecScalar("select count(0) from service where name ='" + name + "' and isdel=0")) > 0)
            {
                return -100;
            }

            return Sys.CNulltoInt0(RunDB.ExecNoQuery("insert into service(name,remark) values('" + name + "','" + remark + "'); " +
                                                     " select SCOPE_IDENTITY()"));
        }
        else if (Sys.DB_Driver == Sys.enum_DB_Driver.Oracle)
        {
        }
        return 0;
    }


    /// <summary>根据ID获得上游平台详情</summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static DataTable getbyid(int id)
    {
        DB RunDB = DB.GetInstance(Sys.DB_Driver);
        if (Sys.DB_Driver == Sys.enum_DB_Driver.SQL2000)
        {
            return RunDB.ExecDataTable("select name,remark from service where id=" + id + " and isdel=0");
        }
        else if (Sys.DB_Driver == Sys.enum_DB_Driver.Oracle)
        {
        }
        return null;
    }

    /// <summary>修改上游平台</summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static int modify(int id, string name,string remark)
    {
        DB RunDB = DB.GetInstance(Sys.DB_Driver);
        if (Sys.DB_Driver == Sys.enum_DB_Driver.SQL2000)
        {
            if (Sys.CNulltoInt0(RunDB.ExecScalar("select count(0) from service where name ='" + name + "' and id<>" + id + " and isdel=0")) > 0)
            {
                return -100;   //已存在同名上游平台
            }
            if (RunDB.ExecNoQuery("update ownstore set name='" + name + "',remark='"+remark+"' where id=" + id + " and isdel=0") > 0)
            {
                return 0;
            }
            else
            {
                return -102;   //该上游平台不存在
            }
        }
        else if (Sys.DB_Driver == Sys.enum_DB_Driver.Oracle)
        {
        }
        return 0;
    }

    /// <summary>删除上游平台</summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static int del(string idstr)
    {
        DB RunDB = DB.GetInstance(Sys.DB_Driver);
        if (Sys.DB_Driver == Sys.enum_DB_Driver.SQL2000)
        {
            RunDB.ExecNoQuery("update service set isdel=1 where id in (" + idstr + ")");
            return 0;
        }
        else if (Sys.DB_Driver == Sys.enum_DB_Driver.Oracle)
        {
        }
        return 0;
    }
}