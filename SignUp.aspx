<%@ Page Title="注册/登陆" Language="C#" MasterPageFile="~/frames/mainframe.master" AutoEventWireup="true" CodeFile="SignUp.aspx.cs" Inherits="SignUp" %>
<%@ Reference VirtualPath="~/user/Captcha.ashx" %>
<%@ Register TagPrefix="User" TagName="LoginCtrl" Src="~/ctrl/LoginCtrl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript"  src="js/jquery-md5lib.js"></script>
    <script language ="javascript" type="text/javascript" src="js/captacha.js"></script>
    <script type="text/javascript">
        function checkRepasswd()
        {
            return $("#passwd_rg").val() == $("#repasswd_rg").val();
        }
        $(document).ready( //页面加载执行
		    function () {
		        //$("a").click(function(){ alert("Hello world!");}
		        //alert($('#Button1').value);
		        //$('#Button1').value = 'Save';
		        //$('#stats').load('index_baidu.html');
		        /**/
		        $("#email_rg").bind("keypress", function (event) {
		            if (event.keyCode == 13) //Enter
		            {
		                $("#user_name_rg").focus();
		                $("#user_name_rg").select();
		            }
		        })
		        $("#user_name_rg").bind("keypress", function (event) {
		            if (event.keyCode == 13) //Enter
		            {
		                $("#passwd_rg").focus();
		                $("#passwd_rg").select();
		            }
		        })
                $("#passwd_rg").bind("keypress", function (event) {
		            if (event.keyCode == 13) //Enter
		            {
		                $("#repasswd_rg").focus();
		                $("#repasswd_rg").select();
		            }
		        })
                $("#repasswd_rg").bind("keypress", function (event) {
		            if (event.keyCode == 13) //Enter
		            {
		                $("#captcha_rg").focus();
		                $("#captcha_rg").select();
		            }
		        })
                $("#captcha_rg").bind("keypress", function (event) {
		            if (event.keyCode == 13) //Enter
		            {
		                $("#agree_rg").focus();
		            }
		        })
		        $("#agree_rg").bind("keypress", function (event) {
		            if (event.keyCode == 13) //Enter
		            {
		                $("#btReg").click();
		            }
		        })
		        $("#btReg").bind("click", function (event) {
                    $("#btReg").attr("disabled",true);
		            $("div#rgmsg").html('<span style="color:#000000;">正在注册，请稍后。。。</span>');
                    if(!checkRepasswd())
                    {
                        $("div#rgmsg").html('<span style="color:#FF0000;">两次密码输入不相同！</span>');
                        $("#passwd_rg").focus();
		                $("#passwd_rg").select();
                        $("#btReg").attr("disabled",false);
                        return;
                    }
		            var postdata = {
                        Email:$("#email_rg").val(),
		                NickName: $("#user_name_rg").val(),
		                Passwd: $.md5($("#passwd_rg").val()),
		                Captcha: $("#captcha_rg").val()
		            };
		            var postjson = JSON.stringify(postdata);
		            $.ajax({
		                //要用post方式
		                type: "POST",
		                url: "user/SignUpAjax.asmx/Registry",
		                dataType: "json",  //dataType: "json",
		                data: postjson,
		                //方法所在页面和方法名Data.aspx                         
		                contentType: "application/json;",
		                success: function (data) {
		                    //返回的数据用data.d获取内容  json                             
		                    //alert("data:" + data.d);
		                    if (data.d == 0) {
		                        $("div#rgmsg").html('<span>注册成功!</span>');
		                        lgevent_dosuccess();
		                    }
                            else
                            {
                                refresh_captcha('captchaImage_rg',<%=CaptchaImage.CaptchaType.registry%>);
                                $("#btReg").attr("disabled",false);
                                if (data.d == -4){
		                                $("div#rgmsg").html('<span style="color:#FF0000;">验证码错误!</span>');
                                        $("#captcha_rg").focus();
		                                $("#captcha_rg").select();
                                }
		                        else if (data.d == -1) {
		                                $("div#rgmsg").html('<span style="color:#FF0000;">Email已存在!</span>');
                                        $("#email_rg").focus();
		                                $("#email_rg").select();
		                        }
                                else if (data.d == -2) {
		                                $("div#rgmsg").html('<span style="color:#FF0000;">用户名已存在!</span>');
                                        $("#email_rg").focus();
		                                $("#email_rg").select();
		                        }
		                        else
		                                $("div#rgmsg").html('<span style="color:#FF0000;">出现未知错误，请与管理员联系!</span>');

                            }
                        },
		                error: function (err) {
                            refresh_captcha('captchaImage_rg',<%=CaptchaImage.CaptchaType.login%>);
		                    $("div#rgmsg").html('<span style="color:#FF0000;">异步错误：' + err.d +
                                                '，请与管理员联系!</span>');
                            $("#btReg").attr("disabled",false);
		                }
		            });
		            //禁用按钮的提交     
		            return false;
		        });
		    }
        );
         
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script language ="javascript" type="text/javascript" src="js/captacha.js"></script>
    <table width="980px" align="center">
        <tr>
            <td width="580px" valign = "top">
                <form id="form_signup" name="form_signup" method="post" >
                    <div class="regform">
		                <div class="caption"><h3 class="title">新用户注册</h3></div>
		                <div class="regtip">请填写以下信息，全部为必填</div>
		                <div class="items">
                            <div class="item">
                                <label class="lbl">电子邮箱：</label>
                                  <input name="email_rg" id="email_rg" type="text"
                                      class="" tabindex="1" value=""/>
                                  <div id="email-S"></div>
                            </div>
                            <div class="item">
                                <label class="lbl">昵称：</label>
                                <input name="user_name_rg" id="user_name_rg"
                                    class="t" tabindex="2" type="text" value="" />
                                <div id="username-S"></div>
                            </div>
			                <div class="item">
                                <label class="lbl">密码：</label>
                                <input name="passwd_rg" id="passwd_rg" tabindex="3"
                                    class="" type="password"/>
                                <div id="passwd-S"></div>
                            </div>
			                <div class="item">
                                <label class="lbl">确认密码：</label>
                                <input type="password" name="repasswd_rg" id="repasswd_rg" tabindex="4" class="" />
                                <div id="repasswd-S"></div>
                            </div>
			                <div class="item">
                                <label class="lbl">验证码：</label>
                                <input name="captcha_rg" id="captcha_rg" type="text" tabindex="5" class="" />
                                <div id="captcha-S"></div>
                            </div>
			                <div class="item indent">
                                <div class="regcode">
                                    <a href="javascript:refresh_captcha('captchaImage_rg',<%=CaptchaImage.CaptchaType.registry%>)">
                                        <img width="98" height="25" id="captchaImage_rg" name="captchaImage_rg"
                                                alt="验证码" src="user/Captcha.ashx?dstct=<%=CaptchaImage.CaptchaType.registry%>"/>
                                        看不清？换一个
                                    </a>
                                </div>
                            </div>
			                <div class="item indent">
                                <input class="chk" type="checkbox" name="agree_rg" id="agree_rg" value="checkbox" checked="checked"
                                tabindex="6"/>我已阅读并接受<a href="/pub/youku/service/agreement.shtml" target="_blank">注册协议</a>和<a href="/pub/youku/service/copyright.shtml" target="_blank">版权声明</a>
                            </div>
			                <div class="item indent">
                                <div class="regnote">电子邮箱及昵称注册后不能修改，请仔细核对。</div>
                            </div>
                            <div class="item indent">
                                <div class="msg" id = "rgmsg"></div>
                            </div>
			                <div class="item indent">
                                <button name="btReg" id="btReg" type="button" tabindex="7">注　册</button>
                            </div>
			                <div class="item indent">
                                <div class="gopec">不了解Tube Cube？请访问<a href="/pec.aspx" target="_blank">产品体验中心</a></div>
                            </div>
		                </div>
                    </div>
                </form>
            </td>
            <td width="400px" valign = "top">
                <div>
                    <User:LoginCtrl ID="loginctrl" runat="server" />
                    <script language ="javascript" type="text/javascript">
                        lgevent_dosuccess = function () {
                            window.location.href = "<%=m_szBackUrl%>";
                        }
                    </script>
                </div>
                <div>
                    <%=BackforwardControl()%>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>

