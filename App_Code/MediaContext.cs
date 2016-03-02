using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;

/// <summary>
///MediaContext 的摘要说明
/// </summary>
public class MediaContext : DBTableContext
{
    protected media _media = null;
    public media media { get{return _media;} }
	protected MediaContext(): base()
	{
	}
    public MediaContext(Guid mID):base()
    {
        try
        {
            _media = dataContext.media.Where(m => m.id == mID).Single();
            if (_media == null)
                throw new Exception("Invalid ID.");
        }
        catch(Exception e)
        {
            Dispose();
            throw (e);
        }
    }
    public MediaContext(media mediaRef)
        : base()
    {
        if (mediaRef == null)
            throw (new NullReferenceException());
        _media = mediaRef;
    }
    public static MediaContext FromGuid(Guid mID)
    {
        try
        {
            MediaContext mediaContext = new MediaContext(mID);
            return mediaContext;
        }
        catch(Exception e)
        {
            MyLog.ErrorLog(e.ToString());
            return null;
        }
    }
    public static IEnumerable<MediaContext> GetTopHot(int count)
    {
        DataClassesDataContext dataContext = null;
        IEnumerable<MediaContext> rt = null ;
        try
        {
            dataContext = new DataClassesDataContext();
            {

                try
                {
                    IEnumerable<media> _mediaArray = from tmpMeida in dataContext.media
                                                     orderby tmpMeida.watchedTimes descending, tmpMeida.uploadedTime descending
                                                     select tmpMeida;

                    rt = new EnumerableQuery<MediaContext>(from tmpMeidaContext
                                                           in _mediaArray.Take(count)
                                                           select new MediaContext(tmpMeidaContext));
                }
                catch (Exception e)
                {
                    throw (e);
                }
            }
        }
        catch (Exception e)
        {
            if(dataContext!= null && dataContext.Connection != null)
                dataContext.Connection.Close();
            throw (e);
        }
        finally
        {
        }
        return rt;
    }
    public static IEnumerable<MediaContext> Search(string szKeywords, out int rtPages,
                                            int page = 1, int countPerPage = 20)
    {
        rtPages = 0;
        if (page <= 0)
            return null;
        DataClassesDataContext dataContext = null;
        IEnumerable<MediaContext> rt = null;
        string[] split = { " " };
        string[] keyWords = szKeywords.Split(split, StringSplitOptions.RemoveEmptyEntries);
        try
        {
            dataContext = new DataClassesDataContext();
            try
            {
                IEnumerable<media> _mediaArray = null;
                if (keyWords != null && keyWords.Count() > 0)
                {
                    foreach (string keyword in keyWords)
                    {
                        IEnumerable<media> tms = from tmpMeida in dataContext.media
                                                 where tmpMeida.tittle.ToLower().Contains(keyword.ToLower())
                                                 select tmpMeida;
                        if (_mediaArray == null)
                            _mediaArray = tms;
                        else
                            _mediaArray = _mediaArray.Union(tms);
                    }
                }
                else
                    _mediaArray = from tmpMeida in dataContext.media select tmpMeida;
                if (_mediaArray == null)
                    return null;
                _mediaArray = from tmpMeida in _mediaArray 
                            orderby tmpMeida.watchedTimes descending,
                            tmpMeida.uploadedTime descending
                              select tmpMeida;

                if (_mediaArray == null)
                    return null;
                rtPages = _mediaArray.Count() % countPerPage == 0 ?
                    _mediaArray.Count() / countPerPage :
                    _mediaArray.Count() / countPerPage + 1;
                if (countPerPage * (page - 1) > _mediaArray.Count())
                    return null;
                IEnumerable<media> rt1 = new EnumerableQuery<media>(from tmpMeidaContext
                                                       in _mediaArray.Take(
                                                             Math.Min(countPerPage * page, _mediaArray.Count()))
                                                 select tmpMeidaContext);
                IEnumerable<media> rt2 = new EnumerableQuery<media>(from tmpMeidaContext
                                                     in _mediaArray.Take(countPerPage * (page - 1))
                                                     select tmpMeidaContext);
                rt1 = rt1.Except(rt2);
                rt = new EnumerableQuery<MediaContext>(from tmpMeida
                                                       in rt1
                                                       select new MediaContext(tmpMeida));

            }
            catch (Exception e)
            {
                throw (e);
            }
        }
        catch (Exception e)
        {
            throw (e);
        }
        finally
        {
        }
        return rt;
    }
    public string GetFlvAbsolutePath()
    {
        String szFile = "~/user/flv/badRequest.flv";
        if (_media == null || media.id == Guid.Empty)
            goto end;
        string szTmpFile = "~/App_Data/uploads/media/" + media.id.ToString() + ".flv";
        if (File.Exists(HttpContext.Current.Server.MapPath(szTmpFile)))
            szFile = szTmpFile;
    end:
        return HttpContext.Current.Server.MapPath(szFile);
    }
    public static MediaContext UploadFile(string fileName, Stream fStream)
    {
        UserContext user = UserStatue.GetCurrentUserContext();
        if (user == null || fileName == null)
            return null;
        
        MediaContext rt = null;
        try
        {
            rt = new MediaContext();
            rt._media = rt.dataContext.NewMedia(user.Profile.id, fileName).Single();
            if (rt._media == null)
                return null;
            double fLastTime = double.NaN;
            string imgPath = MediaFormatFactory.StreamConvert2Flv(fileName, rt._media.id, fStream, out fLastTime);
            if (imgPath == null || !System.IO.File.Exists(imgPath) || double.IsNaN(fLastTime))
                return null;
            //var media = dataContext.media.Where(u => u.id == rt._media.id).Single();
            rt._media.Converting = false;
            rt._media.lasttimeSeconds = fLastTime;
            rt._media.thumbnail = GetThumbnailFromFile(imgPath);
            string szTittle = MediaFormatFactory.GetFileTittle(fileName);
            szTittle = szTittle.Substring(0,Math.Min(32,szTittle.Length));
            rt.media.tittle = szTittle;
            File.Delete(imgPath);
            rt.dataContext.SubmitChanges();
        }
        catch(Exception e)
        {
            if (rt != null)
                rt.Dispose();
            MyLog.ErrorLog(e.ToString());
            if(rt._media != null)
            return null;
        }
        return rt;
    }
    private static Binary GetThumbnailFromFile(string path)
    {
        Binary rt = null;
        FileStream imgFs = new FileStream(path, FileMode.Open);
        using (imgFs)
        {
            byte[] imgBytes = new byte[imgFs.Length];
            imgFs.Read(imgBytes, 0, (int)imgFs.Length);
            rt = new System.Data.Linq.Binary(imgBytes);
        }
        imgFs.Close();
        imgFs.Dispose();
        return rt;
    }
}