using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

/// <summary>
///Captcha 的摘要说明
/// </summary>
public partial class Captcha : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        object oDstct = context.Request.Params["dstct"];
        int nDstct = 0;
        if (oDstct != null)
            nDstct = Convert.ToInt32(oDstct);
        string code = CaptchaImage.CreateCode(4);
        CaptchaImage.SetDistinction(context.Session, code, nDstct);
        CaptchaImage.ImageResponse(code, context.Response);
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}