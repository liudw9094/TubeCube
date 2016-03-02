using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;


public class ThumbnailLoader : IHttpHandler, IReadOnlySessionState
{

    public void ProcessRequest(HttpContext context)
    {
        string szType = context.Request.Params["tp"];
        string szID = context.Request.Params["id"];
        Guid mid = Guid.Empty;
        MediaContext mc = null;
        switch (szType)
        {
            case "UserProf":
                break;
            case "MediaThbnl":
                if(!Guid.TryParse(szID, out mid))
                    goto default;
                mc = MediaContext.FromGuid(mid);
                if(!AuthorizationStatue.CheckMediaAuthoryization(mc))
                    goto default;
                if (mc.media.thumbnail == null)
                {
                }
                else
                {
                    context.Response.ContentType = "img/jpg";
                    context.Response.BinaryWrite(mc.media.thumbnail.ToArray());
                }
                break;
            case "MediaFolderThbnl":
                break;
            default:
                context.Response.ContentType = "text/plain";
                context.Response.Write("Hello World");
                break;
        }
        
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}