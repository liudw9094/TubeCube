using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
using System.Data.SqlClient;

/// <summary>
///UserStatue 的摘要说明
/// </summary>
public class UserStatue
{
	public UserStatue()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    public static void Logoff()
    {
        HttpSessionState Session = HttpContext.Current.Session;
        Session.Remove("UserContext");
    }
    public static UserContext GetCurrentUserContext()
    {
        HttpSessionState Session = HttpContext.Current.Session;
        object usr = Session["UserContext"];
        if (Session != null && usr != null && usr is UserContext)
            return (UserContext)usr;
        return null;
    }
    public static bool IsLogined()
    {
        if (GetCurrentUserContext() != null)
            return true;
        return false;
    }
    public static bool Login(string ID, string Passwd)
    {
        HttpSessionState Session = HttpContext.Current.Session;
        Logoff();
        UserContext up = UserContext.Login(ID, Passwd);
        if (up == null)
            return false;
        Session["UserContext"] = up;
        Session.Timeout = 60;
        return true;
    }
    /*
     * return value
     * 0: success
     * -1: email existed
     * -2: user existed
     * -3: unknown error
     */ 
    public static int Registry(String Email, String UserName, String Passwd)
    {
        HttpSessionState Session = HttpContext.Current.Session;
        UserContext.RegBackError err;
        Logoff();
        UserContext up = UserContext.Registry(Email, UserName, Passwd, out err);
        if (up == null)
        {
            if (err.BadEmail)
                return -1;
            if (err.BadUsername)
                return -2;
            return -3;
        }
        Session["UserContext"] = up;
        Session.Timeout = 60;
        return 0;
    }
}