<%@ Page Language="C#" MasterPageFile="~/frames/mainframe.master" AutoEventWireup="true" CodeFile="Cinema.aspx.cs" Inherits="Cinema" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title><%=m_szMediaTittle%></title>
    <script type="text/javascript" language="javascript" src="js/player.js"></script>
    <script language ="javascript" type="text/javascript" src="js/captacha.js"></script>
    <script type="text/javascript"  src="js/jquery-md5lib.js"></script>
    <script type="text/javascript" language="javascript">
        var refMedia = "<%=m_szMediaID%>";
        var bIsPrivate = <%=m_bPrivate.ToString().ToLower()%>;
        var bIsAuthoryization = <%=m_bAuthoryization.ToString().ToLower()%>
        $(document).ready(
            function () {
                if(bIsAuthoryization == false){
                    if(bIsPrivate == true){
                        var msg = $("div#msg");
                        msg.html("对不起，您没有查看该视频的权限。");
                        msg.dialog({ modal: true, title: "权限错误", resizable: false,
                                closeOnEscape: false, buttons: [
                                    {
                                        text: "OK",
                                        click: function () { $(this).dialog("close");}
                                    } ] });
                     }
                     else
                     {
                        $("form#cinema").bind("submit", function (event) {
                                $("#mediaPwd").val($.md5($("#mediaPwd").val()));
                            });
                     }
                }
                else{
                    SetPlayer("p_presentation", refMedia, 640, 480);
                }
            });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="msg" style="z-index:0;display:none;"></div>
    <table width="100%">
        <tr><td align="center">
            <div id="p_presentation" style="height:480px; width:640px;">
                <%if (m_bAuthoryization == false)
                  {

                      if (m_bPrivate == true)
                      { %>
                        <img src="user/image/badRequest.png" alt="权限错误" />
                <%    }
                      else
                      {%>
                      <div class="notify">
                        <form id="cinema" name="cinema" method="post">
                          <div class="notify_text">对不起，身份验证失败，该视频已加密。</div>
                          <div class="item">
                                <label class="lbl">请输入密码：</label>
                                <input type="password" id="mediaPwd" name="pwd" />
                          </div>
                          <div class="item">
                                <label class="lbl">验证码：</label>
                                <input type="text" id="mediaCaptcha" name="code" />
                                <%if (m_bCaptchaError)
                                  {%>
                                <span class="error">验证码错误！</span>
                                <%} %>
                          </div>
                          <div class="item indent">
                              <div class="athCode">
                                  <a href="javascript:void();"
                                  onclick="javascript:refresh_captcha('captchaImage_mp',<%=CaptchaImage.CaptchaType.mediaPwd%>)">
                                      <img width="98" height="25" id="captchaImage_mp" name="captchaImage_mp"
                                              alt="验证码" src="user/Captcha.ashx?dstct=<%=CaptchaImage.CaptchaType.mediaPwd%>"/>
                                      看不清？换一个
                                  </a>
                              </div>
                          </div>
                          <div class="item"><input type="submit" id="btOK" value="确定" /></div>
                        </form>
                      </div>
                      <%
                      }
                  } %>
            </div>
        </td></tr>
        <tr><td align="center">
            //TODO: Comments
        </td></tr>
    </table>
</asp:Content>

