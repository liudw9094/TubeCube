using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
///DBConnPool 的摘要说明
/// </summary>
public static class DBConnPool
{
    const int m_nNormalCount = 1;
    const int m_nMaxCount = 100;
    //const string m_nConnString = @"Data Source=.\SQLEXPRESS;AttachDbFilename=D:\aas\WebSites\TubeCube\App_Data\dbs.mdf;Integrated Security=True;User Instance=True";
    const string m_nConnString = @"Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\dbs.mdf;Integrated Security=True;User Instance=True";

    static Queue<SqlConnection> m_Conn = new Queue<SqlConnection>();
	static DBConnPool()
	{
        if(m_nNormalCount > m_nMaxCount)
            throw new Exception();
        lock (m_Conn)
        {
            AllClear();
            NewConnections();
        }
	}
    public static SqlConnection GetConnection()
    {
        lock (m_Conn)
        {
            if (m_Conn.Count <= 0)
            {
                AllClear();
                NewConnections();
            }
            return m_Conn.Dequeue();
        }
    }
    public static void BackConnection(SqlConnection connection)
    {
        if (connection == null)
            return;
        lock (m_Conn)
        {
            if (m_Conn.Count >= m_nMaxCount)
            {
                connection.Close();
                connection.Dispose();
                return;
            }
            m_Conn.Enqueue(connection);
        }
    }
    static void NewConnections()
    {   
        lock (m_Conn)
        {
            for (int i = 0; i < m_nNormalCount; ++i)
            {
                SqlConnection tmpCon = new SqlConnection(m_nConnString);
                tmpCon.Open();
                m_Conn.Enqueue(tmpCon);
            }
        }
    }
    static void AllClear()
    {
        lock (m_Conn)
        {
            if(m_Conn.Count <= 0)
            {
                m_Conn.Clear();
                return;
            }
            SqlConnection tmpConnection;
            while((tmpConnection=m_Conn.Dequeue()) != null)
            {
                tmpConnection.Close();
                tmpConnection.Dispose();
            }
            m_Conn.Clear();
        }
    }
}