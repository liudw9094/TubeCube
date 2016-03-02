using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.XPath;
/// <summary>
///Global 的摘要说明
/// </summary>
public partial class _Global : HttpApplication
{
	public _Global()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}


    void Application_Start(object sender, EventArgs e)
    {
        //在应用程序启动时运行的代码

    }

    void Application_End(object sender, EventArgs e)
    {
        //在应用程序关闭时运行的代码

    }

    void Application_Error(object sender, EventArgs e)
    {
        //在出现未处理的错误时运行的代码

    }

    void Session_Start(object sender, EventArgs e)
    {
        //在新会话启动时运行的代码

    }

    void Session_End(object sender, EventArgs e)
    {
        //在会话结束时运行的代码。 
        // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
        // InProc 时，才会引发 Session_End 事件。如果会话模式 
        //设置为 StateServer 或 SQLServer，则不会引发该事件。

    }
    // 作为重定向
    protected void Application_BeginRequest(object sender, EventArgs e)
    {
        //MyLog.ErrorLog(Request.Url.ToString());
        try
        {
            string path = Server.MapPath("~/App_Data/ReWriter.xml");
            XPathDocument myXPathDocument = new XPathDocument(path);
            XPathNavigator myXPathNavigator = myXPathDocument.CreateNavigator();
            XPathNodeIterator myXPathNodeIterator = myXPathNavigator.Select("//rule");
            System.Text.RegularExpressions.Regex oReg;
            string ReWriteUrl;
            while (myXPathNodeIterator.MoveNext())
            {
                //oReg=new Regex(oNode.SelectSingleNode("url/text()").Value);
                XPathNavigator nav2 = myXPathNodeIterator.Current.Clone();
                string oldString = "", newString = "";
                XPathNodeIterator it2 = nav2.Select("old");
                while (it2.MoveNext())
                {
                    oldString = it2.Current.Value;
                    if (oldString[0] == '~')
                    {
                        if(HttpContext.Current.Request.ApplicationPath.Equals("/"))
                            oldString = HttpContext.Current.Request.Url.Scheme + "://" +
                                HttpContext.Current.Request.Url.Authority  + oldString.Substring(1);
                        else
                            oldString = HttpContext.Current.Request.Url.Scheme + "://" +
                                HttpContext.Current.Request.Url.Authority +
                                HttpContext.Current.Request.ApplicationPath + oldString.Substring(1);
                    }
                    break;
                }
                it2 = nav2.Select("new");
                while (it2.MoveNext())
                {
                    newString = it2.Current.Value;
                    break;
                }
                if (oldString != "" && newString != "")
                {
                    //MyLog.ErrorLog(oldString);
                    oReg = new System.Text.RegularExpressions.Regex(oldString);
                    if (oReg.IsMatch(Request.Url.ToString()))
                    {
                        ReWriteUrl = oReg.Replace(Request.Url.ToString(), newString);
                        HttpContext.Current.RewritePath(ReWriteUrl);

                        //MyLog.ErrorLog(ReWriteUrl);
                        break;
                    }
                }
            }
        }
        catch (Exception er)
        {
            MyLog.ErrorLog(er.ToString());
        }
    }

}