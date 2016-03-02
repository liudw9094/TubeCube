using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Cinema : System.Web.UI.Page
{
    protected String m_szMediaID;
    protected bool m_bAuthoryization = true;
    protected bool m_bPrivate = false;
    protected String m_szMediaTittle = "对不起，无法播放该视频";
    protected bool m_bCaptchaError = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        m_szMediaID = Request.Params["mID"];
        foreach (string tp in Request.AcceptTypes)
        {
            if (tp.ToLower() == "text/html".ToLower())
            {
                Guid mediaID = Guid.Empty;
                if (m_szMediaID != null &&
                   Guid.TryParse(m_szMediaID, out mediaID) &&
                    mediaID != Guid.Empty)
                {
                    MediaContext media = MediaContext.FromGuid(mediaID);
                    if (media != null)
                    {
                        string captcha = (string)Request.Params["code"];
                        if (captcha != null)
                        {
                            if (CaptchaImage.IsCodeCorrect(captcha, Session, CaptchaImage.CaptchaType.mediaPwd))
                            {
                                AuthorizationStatue atStatue = AuthorizationStatue.GetCurrentAuthorization();
                                atStatue.CurrentAuthorizedMedia = mediaID;
                                atStatue.CurrentAuthorizedMediaPassword = (string)Request.Params["pwd"];
                            }
                            else
                                m_bCaptchaError = true;
                        }
                        UserContext user = UserStatue.GetCurrentUserContext();
                        if (!AuthorizationStatue.CheckMediaAuthoryization(media))
                        {
                            m_bAuthoryization = false;
                            if (media.media.authorization.Trim() == "private" &&
                                (user == null || (user != null && 
                                    user.Profile.id != media.media.ownerID) ))
                                m_bPrivate = true;
                        }
                        else
                            m_szMediaTittle = Server.HtmlEncode(media.media.tittle.Trim());
                        media.Dispose();
                    }
                }
                return;
            }
        }
        Response.Clear();
        Response.Redirect("user/flvplayer.swf?vcastr_file=vml" + m_szMediaID, true);
        Response.End();
        Response.Close();
    }
}