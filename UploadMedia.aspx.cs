using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.IO;

public partial class UploadMedia : System.Web.UI.Page
{
    protected string[] formats = { "flv", "wmv", "avi", "rm", "rmvb", "mkv" };
    protected string uploadStatue = null;
    protected bool IsLegalFormat(string fileName)
    {
        for (int i = 0; i < formats.Length; ++i)
        {
            if (MediaFormatFactory.GetExtension(fileName).
                    ToLower().Equals(formats[i].ToLower()))
                return true;
        }
        return false;
    }
    protected string GetFormatList()
    {
        string rt = "";
        for (int i = 0; i < formats.Length; ++i)
        {
            if (i != 0)
                rt += ",";
            rt += "\"" + formats[i] + "\"";
        }
        return rt;
    }
    protected string GetFormats()
    {
        string rt = "";
        for (int i = 0; i < formats.Length; ++i)
        {
            if (i != 0)
                rt += ",";
            rt += " " + formats[i];
        }
        return rt;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        bool IsLoged = UserStatue.IsLogined();
        string szUploadFile = Request.Headers["X_FILENAME"];
        string tempUploadFolderPath = Server.MapPath("~/uploads/media");
        if (szUploadFile != null)
        {
            if (!IsLoged || !IsLegalFormat(szUploadFile))
            {
                goto uploadErr;
            }
            Stream str = Request.InputStream;
            MediaContext media = MediaContext.UploadFile(szUploadFile, str);
            Response.ContentType = "text/plain";
            if (media != null)
                Response.Write("File uploaded successfully.");
            else
                Response.Write("File uploaded failed.");
            Response.Flush();
            Response.End();
            Response.Close();
            return;
        }
        if(Request.Files.Count > 0)
        {
            if(!IsLoged)
                goto uploadErr;
            for (int i = 0; i < Request.Files.Count; ++i)
            {
                if (Request.Files.GetKey(i) == "X_FILENAME[]")
                {
                    Stream upFS = Request.Files[i].InputStream;
                    szUploadFile = Request.Files[i].FileName;
                    MediaContext media = MediaContext.UploadFile(szUploadFile, upFS);
                    if (media != null)
                    {
                        media.Dispose();
                        uploadStatue = "文件上传成功！";
                    }
                    else
                        uploadStatue = "文件上传失败！";
                    uploadStatue = Server.HtmlEncode(uploadStatue);
                }
            }
        }

        if (!IsLoged)
        {
            Response.Redirect("SignUp.aspx?refUrl="+Server.HtmlEncode(Request.RawUrl));
            Response.End();
            Response.Close();
            return;
        }
        return;
uploadErr:
        Response.StatusCode = 403;
        Response.ContentType = "text/plain";
        Response.Write("Request Forbidden: Anonymous Uploading.");
        Response.End();
        Response.Close();
    }
}