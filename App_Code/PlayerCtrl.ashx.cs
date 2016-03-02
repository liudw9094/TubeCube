using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

public class PlayerCtrl : IHttpHandler, IReadOnlySessionState
{
    protected bool IsAuthorizationLegal()
    {
        return false;
    }
    public void ProcessRequest(HttpContext context)
    {
        String szMediaID = (String)context.Request.Params["mID"];
        if (szMediaID == null)
            szMediaID = "";
        String vfile = "vml" + szMediaID;
        int nHeight = 480, nWidth = 640;
        int nIsAutoPlay = 0, nIsContinue = 0;
        Int32.TryParse(context.Request.Params["ht"], out nHeight);
        Int32.TryParse(context.Request.Params["wd"], out nWidth);
        Int32.TryParse(context.Request.Params["ap"], out nIsAutoPlay);
        Int32.TryParse(context.Request.Params["ct"], out nIsContinue);
        String szParam = "vcastr_file=" + vfile + "&IsAutoPlay=" +
                          nIsAutoPlay + "&IsContinue=" + nIsContinue + "&LogoText=TubeCube";
        context.Response.ContentType = "text/html";
        context.Response.Write("<object classid='clsid:D27CDB6E-AE6D-11cf-96B8-444553540000'" +
                                "codebase='http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0'"+
                                " width='"+nWidth+"px' height='"+nHeight+"px' >");
        context.Response.Write("<param name='movie' value='user/flvplayer.swf' />");
        context.Response.Write("<param name='quality' value='high' />");
        context.Response.Write("<param name='allowFullScreen' value='true' />");
        context.Response.Write("<param name='FlashVars'");
        context.Response.Write("value='"+szParam+"' />");
        context.Response.Write("<embed src='user/flvplayer.swf' allowfullscreen='true'");
        context.Response.Write("flashvars='"+szParam+"'");
        context.Response.Write("quality='high' pluginspage='http://www.macromedia.com/go/getflashplayer'");
        context.Response.Write("type='application/x-shockwave-flash' width='" + nWidth + "' height='" + nHeight + "' />");
        context.Response.Write("</object>");
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}