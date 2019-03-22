using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Data;
using System.Linq.Expressions;

/*
                   _ooOoo_
                  o8888888o
                  88" . "88
                  (| -_- |)
                  O\  =  /O
               ____/`---'\____
             .'  \\|     |//  `.
            /  \\|||  :  |||//  \
           /  _||||| -:- |||||-  \
           |   | \\\  -  /// |   |
           | \_|  ''\---/''  |   |
           \  .-\__  `-`  ___/-. /
         ___`. .'  /--.--\  `. . __
      ."" '<  `.___\_<|>_/___.'  >'"".
     | | :  `- \`.;`\ _ /`;.`/ - ` : | |
     \  \ `-.   \_ __\ /__ _/   .-` /  /
======`-.____`-.___\_____/___.-`____.-'======

佛祖保佑 永无BUG
*/

/// <summary>
///系统类
/// </summary>
public static class Sys
{
    public static string ConStr = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DBConStr"].ToString();
    public static enum_DB_Driver DB_Driver;

    public enum enum_DB_Driver
    {
        SQL2000 = 1,
        Oracle = 3,
        Other = 0
    }
    public static string CNull(object value)
    {
        if (value == null || Convert.IsDBNull(value)) return string.Empty;
        else return value.ToString().Replace("'", "''");
    }

    public static float CNulltoFloat0(object value)
    {
        if (value == null || Convert.IsDBNull(value) || value == "") return 0;
        else return float.Parse(value.ToString());
    }

    public static int CNulltoInt0(object value)
    {
        if (value == null || Convert.IsDBNull(value)) return 0;
        else return int.Parse(value.ToString());
    }

    public static int CNulltoInt(object value, Int32 i)
    {
        if (value == null || Convert.IsDBNull(value)) return i;
        else return int.Parse(value.ToString());
    }

    public static decimal CNulltoDecimal(object value, decimal i)
    {
        if (value == null || Convert.IsDBNull(value)) return i;
        else return decimal.Parse(value.ToString());
    }

    public static decimal CNulltoDecimal0(object value)
    {
        if (value == null || Convert.IsDBNull(value)) return 0;
        else if (value.ToString().Length == 0) return 0;
        else return Decimal.Parse(value.ToString());
    }

    public static bool CNulltoBoolean(object value)
    {
        if (value == null || Convert.IsDBNull(value)) return false;
        else if (value.ToString().Length == 0) return false;
        else return Boolean.Parse(value.ToString());
    }

     //得到md5加密值
    public static string md5(string str)
    {
        System.Text.StringBuilder md5_str = new System.Text.StringBuilder();
        str += "xjiolgieowhf";
        byte[] byte_str = System.Text.Encoding.ASCII.GetBytes(str);
        byte[] byte_md5;
        System.Security.Cryptography.MD5 MD5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

        byte_md5 = MD5.ComputeHash(byte_str);

        foreach (byte i in byte_md5)
        {
            md5_str.Append(i.ToString("x2"));
        }
        return md5_str.ToString();
    }
}
