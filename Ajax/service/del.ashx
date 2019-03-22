<%@ WebHandler Language="C#" Class="del" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Collections;
using System.Web.Script.Serialization;
using System.Data;

/// <summary>
/// 批量删除上游平台
/// </summary>
public class del : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");

        string idstr = Sys.CNull(context.Request.Form["id"]);
        returntitle rt = new returntitle();
        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

        int status = 0;
        try
        {
            status = DBRun_Service.del(idstr);
            if (status == 0)
            {
                context.Response.Write(jsonSerializer.Serialize(rt));
            }
            else  //若返回的status小于0，则表明发生了错误
            {
                context.Response.Write(jsonSerializer.Serialize(new error(status)));
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
        public returntitle()
        {
            this.status = "OK";
        }
    }
}