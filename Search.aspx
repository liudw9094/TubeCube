<%@ Page Language="C#" MasterPageFile="~/frames/mainframe.master" AutoEventWireup="true" CodeFile="Search.aspx.cs" Inherits="Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>搜索 <%=Server.HtmlEncode(keyWords)%></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width = "980px">
        <tr height="500px">
            <td>
                <%=GetSearchResults()%>
            </td>
        </tr>
        <tr align="center">
            <td>
            <%if (pageIndex > 1)
              {%>
              <a href ="Search.aspx?keys=<%=Server.HtmlEncode(keyWords)%>&page=<%=pageIndex - 1 %>">上一页</a>
            <%} %>
            <%=pageIndex%>/<%=pageCount%>
            <%if (pageIndex < pageCount)
              {%>
              <a href ="Search.aspx?keys=<%=Server.HtmlEncode(keyWords)%>&page=<%=pageIndex + 1 %>">下一页</a>
            <%} %>
            </td>
        </tr>
        <tr height="200px">
            <td>TODO:推荐视频</td>
        </tr>
    </table>
</asp:Content>
