using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Search : System.Web.UI.Page
{
    protected const int picWidth = 160, picHeight = 120, countPerPage = 20;
    protected int pageCount = 0, pageIndex = 1;
    protected string keyWords = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        keyWords = Request.Params["keys"];
    }
    protected string GetSearchResults()
    {
        keyWords = Request.Params["keys"];
        if (!int.TryParse(Request.Params["page"], out pageIndex))
            pageIndex = 1;
        if (pageIndex <= 0)
            pageIndex = 1;
        if (keyWords == null)
            keyWords = "";
        return GetSearchResults(keyWords, pageIndex, 18);
    }
    protected string GetSearchResults(string keywords, int page = 1, int countPerPage = 20)
    {
        string rt = "";
        var medias = MediaContext.Search(keywords, out pageCount, page, countPerPage);
        if (medias == null || medias.Count() <= 0)
        {
            return "<b>没有找到匹配的内容</b>";
        }
        rt += "<table class = 'mediaArray' width='100%'>\n";
        int i = 0;
        foreach(var m in medias)
        {
            if (i % 6 == 0)
            {
                if (i != 0)
                    rt += "</tr>\n";
                rt += "<td width='" + picWidth + "'></td>\n";
            }
            string link = "v" + m.media.id.ToString();
            rt += "<td  align='center'>\n";
            rt += "<div>\n";
            rt += "<a href='" + link + "'><img src='user/ThumbnailLoader.ashx?tp=MediaThbnl&id=" +
                    m.media.id.ToString() + "' width='" + picWidth +
                    "' height='" + picHeight + "'/></a>\n";
            rt += "</div>\n";
            rt += "<div>\n";
            rt += "<a href='" + link + "'>" + Server.HtmlEncode(m.media.tittle) + "</a>\n";
            rt += "</div>\n";
            rt += "</td>\n";
            ++i;
        }
        if(i % 6 != 0)
            for (int j = 0; j < 6 - i % 6; ++j)
                rt += "<td width='" + picWidth+"'></td>\n";
        rt += "</tr>\n";
        rt += "</table>";
        return rt;
    }
}