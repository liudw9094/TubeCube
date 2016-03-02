using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class SignUp : System.Web.UI.Page
{
    protected string m_szBackUrl = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        m_szBackUrl = Request.Params["refUrl"];
        if (m_szBackUrl == null || m_szBackUrl.Equals(""))
            m_szBackUrl = "index.aspx";
        if (Request.Params["logoff"] != null)
        {
            UserStatue.Logoff();
            Response.Redirect(m_szBackUrl);
        }
    }
    protected string BackforwardControl()
    {
        string rt = "";
        UserContext curUser = UserStatue.GetCurrentUserContext();
        if (curUser != null)
        {
            rt += "<a href=\"" + m_szBackUrl + "\">";
            rt += "点击此处返回之前页面";
            rt += "</a>";
        }
        return rt;
    }
}