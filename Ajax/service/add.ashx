<%@ WebHandler Language="C#" Class="add" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Collections;
using System.Web.Script.Serialization;
using System.Data;

/// <summary>
/// 添加上游平台
/// </summary>
public class add : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");
        string name = Sys.CNull(context.Request.Form["name"]).Replace("'", "''");                          //平台名称
        string remark = Sys.CNull(context.Request.Form["remark"]).Replace("'", "''");                          //平台名称

        returntitle rt = new returntitle();
        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
        int id = 0;

        try
        {
            id = DBRun_Service.add(name, remark);
            if (id > 0)
            {
                rt.id = id;
                context.Response.Write(jsonSerializer.Serialize(rt));
            }
            else
            {
                context.Response.Write(jsonSerializer.Serialize(new error(id)));
            }
        }
        catch (DB_Exception DBex)
        {
            context.Response.Write(jsonSerializer.Serialize(new error(-99, DBex.Message)));
        }
        catch (Exception ex)
        {
            context.Response.Write(jsonSerializer.Serialize(new error(-99, ex.Message)));
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
        public int id { get; set; }  //成功返回上游平台ID
        public returntitle()
        {
            this.status = "OK";
        }
    }
}