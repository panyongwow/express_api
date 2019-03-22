using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///error 的摘要说明
/// </summary>
public class error
{
    private string _errordetail;
    private string _errorattr;
    public string status
    {
        get { return "ERROR"; }
        set { }
    }
    public int errorcode { get; set; }

    public string errorattr
    {
        get
        {
            return Sys.CNull(this._errorattr);
        }
        set { this._errorattr = value; }
    }

    public string errordetail
    {
        get
        {
            string value;
            switch (errorcode)
            {
                case -1:
                    value = "用户名或密码错误";
                    break;
                case -2:
                    value = "传入ID过多";
                    break;
                case -10:
                    value = "部分参数未指定";
                    break;
                case -11:
                    value = "部分参数为空";
                    break;
                case -99:
                    value = this._errordetail;
                    break;
                case -100:
                    value = "添加的上游平台名称已存在！";
                    break;
                case -102:
                    value = "该上游平台不存在，可能已被删除！";
                    break;
                default:
                    value = "未知错误，错误码：" + errorcode.ToString();
                    break;
            }
            return value;
        }
        set { this._errordetail = value; }
    }

    public error()
    {
    }

    public error(int errorcode)
    {
        this.errorcode = errorcode;
    }

    public error(int errorcode, string errordetail)
    {
        this.errorcode = errorcode;
        this.errordetail = errordetail;
    }
}