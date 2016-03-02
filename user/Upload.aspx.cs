using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class user_Upload : System.Web.UI.Page
{
    protected string FatherDiv = "";
    protected bool bTraditionalMode = false;
    protected int nMaxFileSize = 300000;
    protected string[] fileTypes = {}; 
    protected void Page_Load(object sender, EventArgs e)
    {
        // 判断是否登陆
        if (!UserStatue.IsLogined())
            goto uploadErr;
        // 判断参数是否合法
        char[] splitChar = {','};
        FatherDiv = Request.Params["fatherDivID"];
        String szTM = Request.Params["traditonalMode"];
        String szMaxFileSize = Request.Params["maxFileSize"];
        String szFileType = Request.Params["fileTypes[]"];
        if (szFileType != null && szFileType != "")
            fileTypes = szFileType.Split(splitChar, StringSplitOptions.None);
        if (FatherDiv == null || FatherDiv.Equals("") ||
            szTM == null || szTM.Equals("") ||
            szMaxFileSize == null || szMaxFileSize.Equals(""))
            goto failed;
        if (!bool.TryParse(szTM, out bTraditionalMode))
            goto failed;
        if (!int.TryParse(szMaxFileSize, out nMaxFileSize))
            goto failed;
        return;
    failed:
        Response.StatusCode = 416;
        Response.Write("Bad Parameters.");
        Response.End();
        return;
    uploadErr:
        Response.StatusCode = 403;
        Response.ContentType = "text/plain";
        Response.Write("Request Forbidden: Anonymous Uploading.");
        Response.End();
        Response.Close();
        return;
    }
}