using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class mainframe : System.Web.UI.MasterPage
{
    protected string szUserNickName = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        UserContext uc = UserStatue.GetCurrentUserContext();
        if (uc != null)
            szUserNickName = uc.Profile.nickname.Trim();
    }
}
