using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.Linq;

/// <summary>
///UserBasicProfile 的摘要说明
/// </summary>

public class UserContext : DBTableContext
{
    protected users m_profile = null;
    public struct RegBackError
    {
        public bool BadUsername, BadEmail;
    }

    public users Profile { get { return m_profile; } }
	protected UserContext():base()
	{
	}
    public static UserContext Login(string ID, string Passwd)
    {
        UserContext rt = new UserContext();
        //SqlConnection connection = DBConnPool.GetConnection();
        DataClassesDataContext dataContext = null;
        try
        {
            dataContext = new DataClassesDataContext();//DataClassesDataContext(connection);
            IEnumerable<users> usersQuery =
                from tmpUsers in dataContext.users
                where tmpUsers.email == ID || tmpUsers.nickname==ID
                select tmpUsers;

            if (usersQuery == null || usersQuery.Count() <= 0)
                return null;
            rt.m_profile = (users)usersQuery.First();

            String pwd = rt.m_profile.password;
            if (pwd != null && pwd.Equals(Passwd))
                return rt;
            return null;
        }
        catch (Exception e)
        {
            MyLog.ErrorLog(e.ToString());
            return null;
        }
        finally
        {
            if (dataContext != null)
                dataContext.Dispose();
            //DBConnPool.BackConnection(connection);
        }
    }
    public static UserContext Registry(String Email, String NickName, String Passwd,
                                        out RegBackError RegBackError)
    {
        RegBackError = new RegBackError { BadEmail = false, BadUsername = false };
        UserContext rt = null;
        //SqlConnection connection = DBConnPool.GetConnection();
        DataClassesDataContext dataContext = null;
        try
        {
            dataContext = new DataClassesDataContext();//DataClassesDataContext(connection);
            ISingleResult<NewUserResult> result = dataContext.NewUser(NickName, Email, Passwd, null, null, null, 0);
            NewUserResult firstRt = result.First();
            if (firstRt == null)
                return null;
            switch(firstRt.Column1)
            {
                case 0:
                    dataContext.SubmitChanges();
                    rt =  Login(Email, Passwd);
                    break;
                case -1:
                    RegBackError.BadUsername = true;
                    break;
                case -2:
                    RegBackError.BadEmail = true;
                    break;
                default:
                    break;
            }
        }
        catch (Exception e)
        {
            MyLog.ErrorLog(e.ToString());
            RegBackError.BadEmail = false;
            RegBackError.BadUsername = false;
            return null;
        }
        finally
        {
            if (dataContext != null)
                dataContext.Dispose();
            //DBConnPool.BackConnection(connection);
        }
        return rt;
    }
    public bool Update()
    {
        return false;
    }
}