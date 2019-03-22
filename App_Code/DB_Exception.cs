using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///DB_Exception 的摘要说明
/// </summary>
public class DB_Exception : System.Exception
{
    private string pMessge;

    public String Message
    {
        get { return pMessge; }
    }

    public DB_Exception(String Message)
    {
        this.pMessge = Message;
    }
}