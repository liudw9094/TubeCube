using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Xml.XPath;
/// <summary>
///MediaFormatFactory 的摘要说明
/// </summary>
public class MediaFormatFactory
{
	public MediaFormatFactory()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    //文件路径

    public const string ffmpegtool = "~/bin/ffmpeg.exe";
    public const string mencodertool = "~/bin/MPlayer-1.0rc2/mencoder.exe";
    public const string flvtool = "~/bin/flvtool2-1.0.6/flvtool2.exe";          //flv标记工具

    public static string upFile = "~/App_Data/uploads/media_unconverted" + "/";   //上传文件夹
    public static string imgFile = "~/App_Data/uploads/media_Thumbnail" + "/";    //图片文件夹
    public static string playFile = "~/App_Data/uploads/media" + "/";             //flv文件夹
    public static string xmlFile = "~/App_Data/uploads/media_xml" + "/";          //xml文件夹

    public static string sizeOfImg = "240x180";                     //图片的宽与高
    public static string widthOfFile = "640";                       //flv文件的宽度
    public static string heightOfFile = "480";                      //flv文件的高度

    //public static string ffmpegtool = ConfigurationManager.AppSettings["ffmpeg"];
    //public static string mencodertool = ConfigurationManager.AppSettings["mencoder"];
    //public static string upFile = ConfigurationManager.AppSettings["upfile"] + "/";
    //public static string imgFile = ConfigurationManager.AppSettings["imgfile"] + "/";
    //public static string playFile = ConfigurationManager.AppSettings["playfile"] + "/";
    //文件图片大小
    //public static string sizeOfImg = ConfigurationManager.AppSettings["CatchFlvImgSize"];
    //文件大小
    //public static string widthOfFile = ConfigurationManager.AppSettings["widthSize"];
    //public static string heightOfFile = ConfigurationManager.AppSettings["heightSize"];

    //   // //获取文件的名字
    //private System.Timers.Timer myTimer = new System.Timers.Timer(3000);//记时器
    public static string flvName = "";
    public static string imgName = "";
    public static string flvXml = "";
    public static int pId = 0;


    public static string GetFileName(string fileName)
    {
        int i = fileName.LastIndexOf("\\") + 1;
        string Name = fileName.Substring(i);
        return Name;
    }
    //获取文件扩展名
    public static string GetExtension(string fileName)
    {
        int i = fileName.LastIndexOf(".") + 1;
        string Name = fileName.Substring(i);
        return Name;
    }
    public static string GetFileTittle(string fileName)
    {
        string Name = GetFileName(fileName);
        int i = Name.LastIndexOf(".");
        Name = Name.Substring(0, i);
        return Name;
    }
    //
    #region //运行FFMpeg的视频解码，(这里是绝对路径)
    /// <summary>
    /// 转换文件并保存在指定文件夹下面(这里是绝对路径)
    /// </summary>
    /// <param name="fileName">上传视频文件的路径（原文件）</param>
    /// <param name="playFile">转换后的文件的路径（网络播放文件）</param>
    /// <param name="imgFile">从视频文件中抓取的图片路径</param>
    /// <returns>成功:返回图片虚拟地址;   失败:返回空字符串</returns>
    public void ToFlvAbsolute(string fileName, string playFile, string imgFile)
    {
        //取得ffmpeg.exe的路径,路径配置在Web.Config中,如:<add   key="ffmpeg"   value="E:\aspx1\ffmpeg.exe"   />   
        string ffmpeg = HttpContext.Current.Server.MapPath(MediaFormatFactory.ffmpegtool);
        if ((!System.IO.File.Exists(ffmpeg)) || (!System.IO.File.Exists(fileName)))
        {
            return;
        }

        //获得图片和(.flv)文件相对路径/最后存储到数据库的路径,如:/Web/User1/00001.jpg   

        string flv_file = System.IO.Path.ChangeExtension(playFile, ".flv");


        //截图的尺寸大小,配置在Web.Config中,如:<add   key="CatchFlvImgSize"   value="240x180"   />   
        string FlvImgSize = MediaFormatFactory.sizeOfImg;

        System.Diagnostics.ProcessStartInfo FilestartInfo = new System.Diagnostics.ProcessStartInfo(ffmpeg);

        FilestartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

        FilestartInfo.Arguments = " -i " + fileName + " -ab 56 -ar 22050 -b 500 -r 15 -s " + widthOfFile + "x" + heightOfFile + " " + flv_file;
        //ImgstartInfo.Arguments = "   -i   " + fileName + "   -y   -f   image2   -t   0.05   -s   " + FlvImgSize + "   " + flv_img;

        try
        {
            //转换
            System.Diagnostics.Process.Start(FilestartInfo);
            //截图
            CatchImg(fileName, imgFile, 2.0);
            //System.Diagnostics.Process.Start(ImgstartInfo);
        }
        catch (Exception e)
        {
            MyLog.ErrorLog(e.ToString());
        }
    }
    #endregion

    #region 截图
    public static string CatchImg(string fileName, string imgFile, double second)
    {
        //
        string ffmpeg = HttpContext.Current.Server.MapPath(MediaFormatFactory.ffmpegtool);
        //
        string flv_img = imgFile;
        //
        string FlvImgSize = MediaFormatFactory.sizeOfImg;
        //
        System.Diagnostics.ProcessStartInfo ImgstartInfo = new System.Diagnostics.ProcessStartInfo(ffmpeg);
        ImgstartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        //
        ImgstartInfo.Arguments = " -i \"" + fileName + "\" -y -f image2 -ss "+second+" -vframes 1 -s " + FlvImgSize + " \"" + flv_img +"\"";
        try
        {
            Process ps = System.Diagnostics.Process.Start(ImgstartInfo);
            ps.WaitForExit();
            ps.Dispose();
        }
        catch (Exception e)
        {
            MyLog.ErrorLog(e.ToString());
            return "";
        }
        //
        //catchFlvTool(fileName);
        if (System.IO.File.Exists(flv_img))
        {
            return flv_img;
        }

        return "";
    }
    #endregion
    #region //运行FFMpeg的视频解码，(这里是(虚拟)相对路径)
    /// <summary>
    /// 转换文件并保存在指定文件夹下面(这里是相对路径)
    /// </summary>
    /// <param name="fileName">上传视频文件的路径（原文件）</param>
    /// <param name="playFile">转换后的文件的路径（网络播放文件）</param>
    /// <param name="imgFile">从视频文件中抓取的图片路径</param>
    /// <returns>成功:返回图片虚拟地址;   失败:返回空字符串</returns>
    public void ToFlv(string fileName, string playFile, string imgFile, string xmlFile)
    {
        //取得ffmpeg.exe的路径,路径配置在Web.Config中,如:<add   key="ffmpeg"   value="E:\aspx1\ffmpeg.exe"   />   
        string ffmpeg = HttpContext.Current.Server.MapPath(MediaFormatFactory.ffmpegtool);
        if ((!System.IO.File.Exists(ffmpeg)) || (!System.IO.File.Exists(fileName)))
        {
            return;
        }

        //获得图片和(.flv)文件相对路径/最后存储到数据库的路径,如:/Web/User1/00001.jpg   
        string flv_img = System.IO.Path.ChangeExtension(HttpContext.Current.Server.MapPath(imgFile), ".jpg");
        string flv_file = System.IO.Path.ChangeExtension(HttpContext.Current.Server.MapPath(playFile), ".flv");


        //截图的尺寸大小,配置在Web.Config中,如:<add   key="CatchFlvImgSize"   value="240x180"   />   
        string FlvImgSize = MediaFormatFactory.sizeOfImg;

        System.Diagnostics.ProcessStartInfo FilestartInfo = new System.Diagnostics.ProcessStartInfo(ffmpeg);
       
        FilestartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        //此处组合成ffmpeg.exe文件需要的参数即可,此处命令在ffmpeg   0.4.9调试通过 
        //ffmpeg -i F:\01.wmv -ab 56 -ar 22050 -b 500 -r 15 -s 320x240 f:\test.flv
        FilestartInfo.Arguments = " -i " + fileName + " -ab 56 -ar 22050 -b 500 -r 15 -s " + widthOfFile + "x" + heightOfFile + " " + flv_file;

        try
        {
            System.Diagnostics.Process ps = new System.Diagnostics.Process();
            ps.StartInfo = FilestartInfo;
            ps.Start();
            ps.WaitForExit();
            if (CatchImg(flv_file, imgFile,2.0).Equals(""))
                throw new Exception("截图失败！");
            if (catchFlvTool(flv_file, xmlFile).Equals(""))
                throw new Exception("获取视频失败！");
            /*
            HttpContext.Current.Session.Add("ProcessID", ps.Id);
            HttpContext.Current.Session.Add("flv", flv_file);
            HttpContext.Current.Session.Add("img", imgFile);
            myTimer.Elapsed += new System.Timers.ElapsedEventHandler(myTimer_Test);
            myTimer.Enabled = true;
             * */
        }
        catch (Exception e)
        {
            MyLog.ErrorLog(e.ToString());
        }
    }
    #endregion

    #region //运行mencoder的视频解码器转换(这里是(绝对路径))
    public static double ToFlv_M(string vFileName, string playFile, string imgFile, string xmlFile)
    {
        HttpServerUtility Server = HttpContext.Current.Server;
        string tool = Server.MapPath(MediaFormatFactory.mencodertool);
        //string mplaytool = HttpContext.Current.Server.MapPath(MediaFormatFactory.ffmpegtool);

        if ((!System.IO.File.Exists(tool)) || (!System.IO.File.Exists(vFileName)))
        {
            return double.NaN;
        }

        string flv_file = System.IO.Path.ChangeExtension(playFile, ".flv");


        //截图的尺寸大小,配置在Web.Config中,如:<add   key="CatchFlvImgSize"   value="240x180"   />   
        string FlvImgSize = MediaFormatFactory.sizeOfImg;

        System.Diagnostics.ProcessStartInfo FilestartInfo = new System.Diagnostics.ProcessStartInfo(tool);

        FilestartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        //FilestartInfo.Arguments = " \"" + vFileName + "\" -o \"" + flv_file + "\" -mc 0 -of lavf -oac mp3lame -lameopts abr:br=56 -ovc lavc -lavcopts vcodec=flv:vbitrate=1000:mbd=2:mv0:trell:v4mv:cbp:last_pred=1:dia=-1:cmp=0:vb_strategy=1:autoaspect -vf scale="+ widthOfFile + ":-3,expand=" + widthOfFile + ":" + heightOfFile + " -ofps 16 -srate 22050";
        FilestartInfo.Arguments = " \"" + vFileName + "\" -o \"" + flv_file + "\" -mc 0 -of lavf -oac mp3lame -lameopts abr:br=56 -ovc lavc -lavcopts vcodec=flv:vbitrate=1000:mbd=2:mv0:trell:v4mv:cbp:last_pred=1:dia=-1:cmp=0:vb_strategy=1:autoaspect -vf scale=" + widthOfFile + ":-3 -ofps 16 -srate 22050";
        
        try
        {
            System.Diagnostics.Process ps = new System.Diagnostics.Process();
            ps.StartInfo = FilestartInfo;
            ps.Start();
            ps.WaitForExit();
            File.Delete(vFileName); // 删除原上传文件
            // 获取视频信息
            if (catchFlvTool(flv_file, xmlFile).Equals(""))
                throw new Exception("获取视频失败！");
            XPathDocument myXPathDocument = new XPathDocument(xmlFile);
            XPathNavigator myXPathNavigator = myXPathDocument.CreateNavigator();
            XPathNodeIterator myXPathNodeIterator = myXPathNavigator.Select("//fileset");
            myXPathNodeIterator.MoveNext();
            XPathNavigator nav2 = myXPathNodeIterator.Current.Clone();
            XPathNodeIterator it2 = nav2.Select("flv");
            it2.MoveNext();
            XPathNavigator nav3 = it2.Current.Clone();
            XPathNodeIterator it3 = nav3.Select("lasttimestamp");
            it3.MoveNext();
            string szLastTimeStamp = it3.Current.Value;
            double fLastTimeStamp = double.NaN;
            double.TryParse(szLastTimeStamp, out fLastTimeStamp);
            if(double.IsNaN(fLastTimeStamp))
                return double.NaN;
            if (CatchImg(flv_file, imgFile, fLastTimeStamp / 2).Equals(""))
                throw new Exception("截图失败！");

            return fLastTimeStamp;
        }
        catch (Exception e)
        {
            MyLog.ErrorLog(e.ToString());
            return double.NaN;
        }
    }

    /*
    /// <summary>
    /// 记时器功能，自动保存截图
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void myTimer_Test(object sender, System.Timers.ElapsedEventArgs e)
    {
        if (!object.Equals(null, HttpContext.Current.Session["ProcessID"]))
        {
            try
            {
                System.Diagnostics.Process prs = System.Diagnostics.Process.GetProcessById(int.Parse(HttpContext.Current.Session["ProcessID"].ToString()));
                if (prs.HasExited)
                {
                    CatchImg(HttpContext.Current.Session["flv"].ToString(), HttpContext.Current.Session["img"].ToString());
                    catchFlvTool(HttpContext.Current.Session["flv"].ToString());
                    myTimer.Enabled = false;
                    myTimer.Close();
                    myTimer.Dispose();
                    HttpContext.Current.Session.Abandon();
                }
            }
            catch (Exception err)
            {
                MyLog.ErrorLog(err.ToString());
                CatchImg(HttpContext.Current.Session["flv"].ToString(), HttpContext.Current.Session["img"].ToString());
                catchFlvTool(HttpContext.Current.Session["flv"].ToString());
                myTimer.Enabled = false;
                myTimer.Close();
                myTimer.Dispose();
                HttpContext.Current.Session.Abandon();
            }
        }
    }
    
    */
    #endregion

    public static string catchFlvTool(string fileName, string flv_xml)
    {
        HttpServerUtility Server = HttpContext.Current.Server;
        //
        string flvtools = Server.MapPath(MediaFormatFactory.flvtool);
        //
        //string flv_xml = fileName.Replace(".flv", ".xml").Replace(MediaFormatFactory.upFile.Replace("/", ""), MediaFormatFactory.xmlFile.Replace("/", ""));
        //
        System.Diagnostics.ProcessStartInfo ImgstartInfo = new System.Diagnostics.ProcessStartInfo(flvtools);
        ImgstartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        //
        ImgstartInfo.Arguments = " \"" + fileName + "\" -UPx \"" + fileName + "\"";
        ImgstartInfo.FileName = flvtools;
        ImgstartInfo.RedirectStandardOutput = true;
        ImgstartInfo.UseShellExecute = false;
        ImgstartInfo.CreateNoWindow = true;
        try
        {
            Process ps = new System.Diagnostics.Process();
            ps.StartInfo = ImgstartInfo;
            
            string outString = "";
            List<string> otStrings = new List<string>();
            ps.OutputDataReceived += (object sender, DataReceivedEventArgs e) => { otStrings.Add(e.Data); };
            ps.Start();
            ps.BeginOutputReadLine();
            ps.WaitForExit();
            File.WriteAllText(flv_xml, outString);
            File.WriteAllLines(flv_xml, otStrings);
            ps.Dispose();
        }
        catch(Exception e)
        {
            MyLog.ErrorLog(e.ToString());
            return "";
        }
        //
        if (System.IO.File.Exists(flv_xml))
        {
            return flv_xml;
        }

        return "";
    }
    public static string StreamConvert2Flv(String upFileName, Guid mediaID, Stream fStream, out double fLastTime)
    {
        HttpServerUtility Server = HttpContext.Current.Server;
        fLastTime = double.NaN;
        // 开始准备转换文件
        string upload_uncvted = Server.MapPath(upFile);
        string upload_cvted = Server.MapPath(playFile);
        string upload_thmbnl = Server.MapPath(imgFile);
        string upload_xml = Server.MapPath(xmlFile);
        upload_uncvted += mediaID.ToString() + "." + MediaFormatFactory.GetExtension(upFileName);
        upload_cvted += mediaID.ToString() + ".flv";
        upload_thmbnl += mediaID.ToString() + ".jpg";
        upload_xml += mediaID.ToString() + ".xml";
        FileStream fs = new FileStream(upload_uncvted, FileMode.Create);
        using (fs)
        {
            using (fStream)
            {
                fStream.CopyTo(fs);
                fs.Flush();
            }
            fStream.Close();
            fStream.Dispose();
        }
        fs.Close();
        fs.Dispose();
        double fTime = ToFlv_M(upload_uncvted, upload_cvted, upload_thmbnl, upload_xml);
        if (File.Exists(upload_thmbnl) && !double.IsNaN(fTime))
        {
            fLastTime = fTime;
            return upload_thmbnl;
        }
        return null;
    }
}
