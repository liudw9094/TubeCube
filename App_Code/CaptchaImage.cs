using System;
using System.Web;
using System.ComponentModel;
using System.Drawing;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;

/// <summary>
///Captcha 的摘要说明
/// </summary>
public static class CaptchaImage
{
    public static class CaptchaType
    {
        public const int unknown = 0,
                            registry = 1,
                            login = 2,
                            mediaPwd = 3,
                            mediaFolderPwd = 4,
                            mediaComment = 5;
    }
    static CaptchaImage()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    public static string GetCurrentCode(HttpSessionState Session, int nDistinction = CaptchaType.unknown)
    {
        if (Session == null)
            return null;
        Dictionary<int, string> lstCode = (Dictionary<int, string>)Session["CapatchaCodeList"];
        if (lstCode == null)
            return null;
        lock (lstCode)
        {
            if (!((object)lstCode is Dictionary<int, string>))
                return null;
            if (!lstCode.ContainsKey(nDistinction))
                return null;
            return lstCode[nDistinction];
        }
    }
    public static void DeleteCurrentCode(HttpSessionState Session, int nDistinction = CaptchaType.unknown)
    {
        if (Session == null)
            return;
        Dictionary<int, string> lstCode = (Dictionary<int, string>)Session["CapatchaCodeList"];
        if (lstCode == null)
            return;
        lock (lstCode)
        {
            if (!((object)lstCode is Dictionary<int, string>))
                return;
            lstCode.Remove(nDistinction);
        }
    }
    public static bool IsCodeCorrect(string Code, HttpSessionState Session,
                                    int nDistinction = CaptchaType.unknown)
    {
        if (Code == null)
            return false;
        string correctCode = GetCurrentCode(Session, nDistinction);
        if (correctCode == null)
            return false;
        return Code.ToLower().Equals(correctCode.ToLower());
    }
    // 设定区别验证
    public static void SetDistinction(HttpSessionState Session, string Code, int nDistinction = 0)
    {
        if (Session == null)
            return;
        Dictionary<int, string> lstCode = (Dictionary<int, string>)Session["CapatchaCodeList"];
        lock (Session)
        {
            if (lstCode == null || !((object)lstCode is Dictionary<int, string>))
            {
                Session["CapatchaCodeList"] = new Dictionary<int, string>();
                lstCode = (Dictionary<int, string>)Session["CapatchaCodeList"];
            }
        }
        lock (lstCode)
        {
            if (lstCode.ContainsKey(nDistinction))
                lstCode[nDistinction] = Code;
            else
                lstCode.Add(nDistinction, Code);
        }
    }
    /*产生验证码*/
    public static string CreateCode(int codeLength)
    {

        string so = "2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,m," +
                    "n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G," +
                    "H,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";
        string[] strArr = so.Split(',');
        string code = "";
        Random rand = new Random();
        for (int i = 0; i < codeLength; i++)
        {
            code += strArr[rand.Next(0, strArr.Length)];
        }
        return code;
    }

    /*产生验证图片*/
    public static void ImageResponse(string code, HttpResponse Response)
    {
        if (Response == null)
            return;
        Bitmap image = new Bitmap(code.Length * 17, 25);
        Graphics g = Graphics.FromImage(image);
        WebColorConverter ww = new WebColorConverter();
        g.Clear((Color)ww.ConvertFromString("#FAE264"));

        Random random = new Random();
        //画图片的背景噪音线
        for (int i = 0; i < 10; i++)
        {
            int x1 = random.Next(image.Width);
            int x2 = random.Next(image.Width);
            int y1 = random.Next(image.Height);
            int y2 = random.Next(image.Height);

            g.DrawLine(new Pen(Color.LightGray), x1, y1, x2, y2);
        }
        Font font = new Font("Arial", 15, FontStyle.Bold | FontStyle.Italic);
        System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(
         new Rectangle(0, 0, image.Width, image.Height),
                 Color.FromArgb(
                random.Next(177),
                random.Next(255),
                random.Next(255)),
                Color.FromArgb(
                random.Next(177),
                random.Next(255),
                random.Next(255)), 1.2f, true);
        g.DrawString(code, font, brush, 0, 0);

        //画图片的前景噪音点
        for (int i = 0; i < 300; i++)
        {
            int x = random.Next(image.Width);
            int y = random.Next(image.Height);
            image.SetPixel(x, y,
                Color.FromArgb(
                random.Next(255),
                random.Next(255),
                random.Next(255),
                random.Next(255)));
        }
        //画图片的前景噪音线
        for (int i = 0; i < 5; i++)
        {
            int x1 = random.Next(image.Width);
            int x2 = random.Next(image.Width);
            int y1 = random.Next(image.Height);
            int y2 = random.Next(image.Height);

            g.DrawLine(new Pen(
                Color.FromArgb(
                random.Next(255),
                random.Next(255),
                random.Next(255),
                random.Next(255))),
                x1, y1, x2, y2);
        }
        //画图片的边框线
        g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
        //输出图像
        Response.Clear();
        Response.ClearContent();
        Response.ContentType = "image/gif";
        Response.BinaryWrite(ms.ToArray());
        Response.End();
        ms.Dispose();
        g.Dispose();
        image.Dispose();
    }
}