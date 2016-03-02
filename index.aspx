<%@ Page Language="C#" Title="Welcome to Tube Cube" MasterPageFile="~/frames/mainframe.master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>
<%@ Register TagPrefix="User" TagName="LoginCtrl" Src="~/ctrl/LoginCtrl.ascx" %>

<%--!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div style = " ">
    <table width = "980px" align="center">
        <tr>
            <td width="780px">
                <table width ="100%">
                    <tr>
                        <td width="300px">
                            <table width="100%">
                                <tr height="25px">
                                    <td>最热视频： 
                                        <strong><%=Server.HtmlEncode(
                                                MediaContext.GetTopHot(1).First().
                                                media.tittle)
                                        %></strong>
                                    </td>
                                </tr>
                                <tr height="200px">
                                    <td >
                                        <%=GetHotMediaPlayer()%>
                                    </td>
                                </tr>
                                <tr height = "50px">
                                    <td>
                                        TODO:第一热门视频最热评论
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width = "480px">
                            <%-- 推荐视频表 --%>
                            <%=GetMediaString(MediaContext.GetTopHot(15),15)%>
                        </td>
                    </tr>
                    <tr height = "150px">
                        <td colspan = "2">
                            TODO:根据喜好推荐的视频
                        </td>
                    </tr>
                    <tr height = "300px">
                        <td colspan = "2">
                            TODO：个人空间动态
                        </td>
                    </tr>
                    <tr height = "400px">
                        <td colspan = "2">
                            TODO：分类推荐
                        </td>
                    </tr>
                </table>
            </td>
            <td width="200px" valign = "top">
                <table width = "100%" height = "100%">
                    <tr>
                        <td height = "120px">
                            <%--TODO：用户信息及基本通知等选项--%>
                            <User:LoginCtrl ID="loginctrl" runat="server" />
                            <script language="javascript" type="text/javascript">
                                lgevent_dosuccess = function () {
                                    window.location.reload(0);
                                }
                            </script>
                        </td>
                    </tr>
                    <tr height = "200px">
                        <td height = "500px">
                            TODO:热门排行以及广告
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr height = "200px">
            <td colspan = "2">
                TODO：合作及推广
            </td>
        </tr>
    </table>
    </div>
    <%--MainFrames.WriteBottomFrame(Context);--%>
</asp:Content>
