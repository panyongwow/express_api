<%@ WebHandler Language="C#" Class="list" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Collections;
using System.Web.Script.Serialization;
using System.Data;

/// <summary>
/// 浏览上游平台记录
/// </summary>
public class list : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");
        int nowpage = Sys.CNulltoInt(context.Request.Params["nowpage"],1);
        int apagenum = Sys.CNulltoInt(context.Request.Params["apagenum"],20);
        string name = Sys.CNull(context.Request.Params["name"]).Replace("'", "''");                          //搜索的平台名称
        string wherestr = " isdel=0 ";

        if (name.Length > 0) wherestr += " and name like '%" + name + "%' ";

        returntitle rt = new returntitle();
        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
        DataTable dt;

        int maxrow = 0;
        int minrow = 0;

        try
        {
            //总数量，分页使用
            rt.totalcount = DBRun_Service.getnum(wherestr);
            if (nowpage < 1) nowpage = 1;
            if (nowpage > (int)Math.Ceiling((double)rt.totalcount / apagenum))
            {
                nowpage = (int)Math.Ceiling((double)rt.totalcount / apagenum);
            }
            minrow = (nowpage - 1) * apagenum + 1;   //本页的第一行
            maxrow = nowpage * apagenum;           //本页的最后一行

            dt = DBRun_Service.list(minrow, maxrow, wherestr);
            rt.status = "OK";
            rt.nowpage = nowpage;
            foreach (DataRow dr in dt.Rows)
            {
                rt.details.Add(new returndetail
                {
                    id = Sys.CNulltoInt0(dr["id"]),
                    name =Sys.CNull(dr["name"]),
                    remark =Sys.CNull(dr["remark"])
                });
                rt.count++;
            }
            //执行序列化
            context.Response.Write(jsonSerializer.Serialize(rt));
        }
        catch (DB_Exception DBex)
        {
            context.Response.Write(jsonSerializer.Serialize(new error(-99,DBex.Message)));
        }
        catch (Exception ex)
        {
            context.Response.Write(jsonSerializer.Serialize(new error(-99,ex.Message)));
        }
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
    private class returntitle
    {
        public string status { get; set; }
        public int count { get; set; }
        public int totalcount { get; set; }   //总数量，分页使用
        public int nowpage { get; set; }  //返回当前页数，该字段主要是为了处理用户传入一个负页数或者是很大的当前页数，在这种情况下，返回当前的实际页数（一般就是第一页或最后一页）
        public List<returndetail> details { get; set; }

        public returntitle()
        {
            this.details = new List<returndetail>();
        }
    }

    private class returndetail
    {
        public int id { get; set; }
        public string name { get; set; }                  //平台名称
        public string remark { get; set; }                //备注
    } 
}