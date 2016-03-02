using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;

/// <summary>
///MyUtils 的摘要说明
/// </summary>
public static class MyLog
{
    public static void ErrorLog(string errorMsg)
    {
        //ErrorLog(errorMsg, DateTime.Now.ToString("yyyyMMdd_hhmmss_") + new Random().Next(0x3e8, 0x270f) + ".log",
        //        HttpContext.Current.Request.ApplicationPath + "/App_Data/logs/");
        ErrorLog(errorMsg, DateTime.Now.ToString("yyyyMMdd") + ".log",
                HttpContext.Current.Request.ApplicationPath + "/App_Data/logs/");
    }
    public static void ErrorLog(string errorMsg, string fileName, string savePath)
    {
        string str = "Undetected Source Page.";
        HttpContext current = HttpContext.Current;
        if (HttpContext.Current.Request.ServerVariables["HTTP_REFERER"] != null)
        {
            str = current.Request.ServerVariables["HTTP_REFERER"].ToString();
        }
        string path = current.Server.MapPath(savePath);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        FileStream file_s = File.Open(path + fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
        StreamWriter writer = new StreamWriter(file_s, Encoding.Unicode, 10240);
        StringBuilder builder = new StringBuilder();
        builder.Append("\r\n\r\n");
        builder.Append("Local Time:\t\t\t" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
        builder.Append("\r\n\r\n");
        builder.Append("Request Source:\t\t\t" + str);
        builder.Append("\r\n\r\n");
        builder.Append("IP:\t\t\t" + GetClientIP());
        builder.Append("\r\n\r\n");
        builder.Append("Error Page:\t\t\t" + current.Request.Url.ToString());
        builder.Append("\r\n\r\n");
        builder.Append("Content:");
        builder.Append("\r\n\r\n");
        builder.Append(errorMsg);
        builder.Append("\r\n\r\n");
        builder.Append("===========================================================");
        builder.Append("\r\n\r\n");
        writer.Write(builder);
        writer.Flush();
        writer.Close();
        file_s.Close();
    }
    
    public static string GetClientIP()
    {
        HttpContext current = HttpContext.Current;
        string str = "";
        if (current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
        {
            return current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
        }
        if ((str == "") && (current.Request.ServerVariables["REMOTE_ADDR"] != null))
        {
            return current.Request.ServerVariables["REMOTE_ADDR"].ToString();
        }
        return current.Request.UserHostAddress;
    }
}