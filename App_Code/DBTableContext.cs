using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
///TableContext 的摘要说明
/// </summary>
public class DBTableContext : IDisposable
{
    protected DataClassesDataContext dataContext = null;
    protected SqlConnection connection = null;
	public DBTableContext()
	{
        try
        {
            //connection = DBConnPool.GetConnection();
            dataContext = new DataClassesDataContext();
        }
        catch (Exception e)
        {
            Dispose();
            throw (e);
        }
	}
    public void Dispose()
    {
        if (dataContext != null)
            dataContext.Dispose();
        //if (connection != null)
            //DBConnPool.BackConnection(connection);
        GC.SuppressFinalize(this);
    }
    ~DBTableContext()
    {
        Dispose();
    }
}