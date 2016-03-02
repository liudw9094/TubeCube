using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
/// <summary>
///AuthorizationStatue 的摘要说明
/// </summary>
public class AuthorizationStatue
{

    protected Guid m_curAuthFolder = Guid.Empty;
    protected string m_szCurAuthFolderPwd;
    protected Guid m_curAuthMedia = Guid.Empty;
    protected string m_szCurAuthMediaPwd;
    public Guid CurrentAuthorizedFolder
    {
        get { return m_curAuthFolder; }
        set { m_curAuthFolder = value; }
    }
    public String CurrentAuthorizedFolderPassword
    {
        get { return m_szCurAuthFolderPwd; }
        set { m_szCurAuthFolderPwd = value; }
    }
    public Guid CurrentAuthorizedMedia
    {
        get { return m_curAuthMedia; }
        set { m_curAuthMedia = value; }
    }
    public String CurrentAuthorizedMediaPassword
    {
        get { return m_szCurAuthMediaPwd; }
        set { m_szCurAuthMediaPwd = value; }
    }
	protected AuthorizationStatue()
	{
	}
    public static AuthorizationStatue GetCurrentAuthorization()
    {
        HttpSessionState Session = HttpContext.Current.Session;
        AuthorizationStatue rt = null;
        rt = (AuthorizationStatue)Session["Authorization"];
        if (!(rt is AuthorizationStatue))
        {
            rt = new AuthorizationStatue();
            Session["Authorization"] = rt;
        }
        return rt;
    }
    public static bool CheckMediaAuthoryization(MediaContext media)
    {
        UserContext userCon = UserStatue.GetCurrentUserContext();
        AuthorizationStatue atStatue = AuthorizationStatue.GetCurrentAuthorization();
        if (media != null)
        {
            if (media.media.authorization.Trim() == "public" ||
                (userCon != null && media.media.ownerID == userCon.Profile.id))
                return true;
            userMediaFolder mf = media.media.userMediaFolder;
            if (media.media.authorization.Trim() == "protected"  &&
                atStatue.CurrentAuthorizedMedia == media.media.id &&
                media.media.protectingPwd == atStatue.CurrentAuthorizedMediaPassword)
            {
                if (mf != null)
                {
                    if (mf.authorization.Trim() == "public" ||
                        (mf.authorization.Trim() == "protected" &&
                        atStatue.CurrentAuthorizedFolder == mf.id &&
                        mf.protectedPwd == atStatue.CurrentAuthorizedFolderPassword))
                        return true;
                }
                else
                    return true;
            }
        }
        return false;
    }
}