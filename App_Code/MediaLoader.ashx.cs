using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.IO;
public class MediaLoader : IHttpHandler,IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        String szFile = context.Server.MapPath("~/user/flv/badRequest.flv");
        String szMediaID = context.Request["mID"];
        if (szMediaID == null)
            goto end;
        Guid mediaID = Guid.Empty;;
        if (!Guid.TryParse(szMediaID, out mediaID) || mediaID == Guid.Empty)
            goto end;
        MediaContext media = MediaContext.FromGuid(mediaID);
        if (media != null)
        {
            if (AuthorizationStatue.CheckMediaAuthoryization(media))
            {
                szFile = media.GetFlvAbsolutePath();
                lock (media.media)
                {
                    ++media.media.watchedTimes;
                }
            }
            media.Dispose();
        }
end:
        FileInfo file = new FileInfo(szFile);
        context.Response.ContentType = "video/x-flv";
        context.Response.AddHeader("Content-Length ", file.Length.ToString());
        context.Response.WriteFile(file.FullName);
        context.Response.Flush();
        context.Response.End();
        context.Response.Close();

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}