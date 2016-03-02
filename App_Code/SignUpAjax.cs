using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Data;
using System.Data.SqlClient;
/// <summary>
///Login 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
[System.Web.Script.Services.ScriptService]
public class SignUpAjax : System.Web.Services.WebService {

    public SignUpAjax () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    [WebMethod(EnableSession=true)]
    public bool IsLogined()
    {
        if (Session != null && Session["user"] != null)
            return true;
        return false;
    }
    /*
     *  Return Value:
     *      0: Success
     *     -1: Bad password or id
     *     -2: Bad captcha
     *     -3: Exception detected
     */
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public int Login(string ID, string Passwd, string Captcha)
    {
        if (!CaptchaImage.IsCodeCorrect(Captcha, Session, CaptchaImage.CaptchaType.login))
            return -2;
        CaptchaImage.DeleteCurrentCode(Session, CaptchaImage.CaptchaType.login);
        if (ID == null || ID.Equals("") || Passwd == null || Passwd.Equals(""))
            return -1;
        try
        {
            if (!UserStatue.Login(ID,Passwd))
                return -1;
            return 0;
        }
        catch (Exception e)
        {
            MyLog.ErrorLog(e.ToString());
            return -3;
            //throw e;
        }
        finally
        {
        }
    }
    /*
     *  Return Value:
     *      0: Success
     *     -1: Bad nickname
     *     -2: Bad email
     *     -3: unknown error
     *     -4: Bad captcha
     */
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public int Registry(string Email, string NickName, string Passwd, string Captcha)
    {
        if (Email == null || Email.Equals(""))
            return -1;
        if(NickName == null || NickName.Equals(""))
            return -2;
        // TODO: 添加注册代码
        //SqlConnection tmpCon = DBConnPool.GetConnection();
        //SqlCommand cmd = null;
        if (!CaptchaImage.IsCodeCorrect(Captcha, Session, CaptchaImage.CaptchaType.registry))
        {
            CaptchaImage.DeleteCurrentCode(Session, CaptchaImage.CaptchaType.registry);
            return -4;
        }
        CaptchaImage.DeleteCurrentCode(Session, CaptchaImage.CaptchaType.registry);
        try
        {
            return UserStatue.Registry(Email, NickName, Passwd);
        }
        catch (Exception e)
        {
            MyLog.ErrorLog(e.ToString());
            return -3;
            //throw e;
        }
        finally
        {
        }
        /*
        try
        {
            cmd = new SqlCommand(@"EXECUTE [dbo].[NewUser]", tmpCon);
            //cmd = tmpCon.CreateCommand();
            //cmd.CommandText = @"select password from [users] where id = @id or email = @id";
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@nickname", NickName);
            cmd.Parameters.AddWithValue("@email", Email);
            cmd.Parameters.AddWithValue("@password", Passwd);
            return (int)cmd.ExecuteScalar();
        }
        catch (Exception e)
        {
            MyLog.ErrorLog(e.ToString());
            return -5;
            //throw e;
        }
        finally
        {
            if (cmd != null)
                cmd.Dispose();
            DBConnPool.BackConnection(tmpCon);
        }
         */
    }
}
