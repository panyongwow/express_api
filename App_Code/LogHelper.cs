using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///LogHelper 的摘要说明
/// </summary>
/// 
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
public class LogHelper
{
    //记录错误日志
    public static void WriteLog_Error(Type t, Exception ex)
    {
        log4net.ILog log = log4net.LogManager.GetLogger(t);
        log.Error("Error", ex);
    }

    //记录错误日志
    public static void WriteLog_Error(Type t, string msg)
    {
        log4net.ILog log = log4net.LogManager.GetLogger(t);
        log.Error(msg);

    }

    //记录一般信息
    public static void WriteLog_Info(Type t, string msg)
    {
        log4net.ILog log = log4net.LogManager.GetLogger(t);
        log.Info(msg);


    }


}