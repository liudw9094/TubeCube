using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected string GetMediaString(IEnumerable<MediaContext> mArray, int count,
                                    int picWidth = 80, int picHeight = 60)
    {
        string rt = "";
        if(mArray == null)
            return rt;
        mArray = mArray.Take(count);
        rt += "<table class = 'mediaArray' width='100%'>\n";
        int i = 0;
        foreach (var m in mArray)
        {
            if (i % 5 == 0)
            {
                if (i != 0)
                    rt += "</tr>\n";
                rt += "<tr>\n";
            }
            string link = "v" + m.media.id.ToString();
            rt += "<td  align='center'>\n";
            rt += "<div>\n";
            rt += "<a href='"+link+"'><img src='user/ThumbnailLoader.ashx?tp=MediaThbnl&id=" +
                    m.media.id.ToString() + "' width='" + picWidth +
                    "' height='" + picHeight +"'/></a>\n";
            rt += "</div>\n";
            rt += "<div>\n";
            rt +=  "<a href='"+link+"'>" + Server.HtmlEncode(m.media.tittle) + "</a>\n";
            rt += "</div>\n";
            rt += "</td>\n";
            ++i;
        }
        for (int j = 0; j < i % 5; ++j)
            rt += "<td></td>\n";
        rt += "</tr>\n";
        rt += "</table>\n";
        return rt;
    }
    protected string GetHotMediaPlayer(int Width = 300, int Height = 200)
    {
        String szMediaID = MediaContext.GetTopHot(1).First().media.id.ToString();
        string rt = "";
        if (szMediaID == null)
            szMediaID = "";
        String vfile = "vml" + szMediaID;
        int nHeight = Height, nWidth = Width;
        int nIsAutoPlay = 0, nIsContinue = 0;
        nIsAutoPlay = 0;
        nIsContinue = 0;
        String szParam = "vcastr_file=" + vfile + "&IsAutoPlay=" +
                          nIsAutoPlay + "&IsContinue=" + nIsContinue + "&LogoText=TubeCube";

        rt += ("<object classid='clsid:D27CDB6E-AE6D-11cf-96B8-444553540000'" +
                                "codebase='http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0'" +
                                " width='" + nWidth + "px' height='" + nHeight + "px' >");
        rt += ("<param name='movie' value='user/flvplayer.swf' />");
        rt += ("<param name='quality' value='high' />");
        rt += ("<param name='allowFullScreen' value='true' />");
        rt += ("<param name='FlashVars'");
        rt += ("value='" + szParam + "' />");
        rt += ("<embed src='user/flvplayer.swf' allowfullscreen='true'");
        rt += ("flashvars='" + szParam + "'");
        rt += ("quality='high' pluginspage='http://www.macromedia.com/go/getflashplayer'");
        rt += ("type='application/x-shockwave-flash' width='" + nWidth + "' height='" + nHeight + "' />");
        rt += ("</object>");
        return rt;
    }
}