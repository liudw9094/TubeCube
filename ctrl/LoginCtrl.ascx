<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LoginCtrl.ascx.cs" Inherits="LoginCtrl" %>

<script type="text/javascript"  src="js/jquery-md5lib.js"></script>
<script language ="javascript" type="text/javascript" src="js/captacha.js"></script>

<div id="dvLgin">
    <%if (!UserStatue.IsLogined())
      { %>
    <form id="form_login" name="form_login" method="post" >
        <div class="logform">
            <div class="caption"><h3 class="title">已经注册？请登录：</h3></div>
            <div class="items">
                <div class="item">
                    <label class="lbl">昵称/邮箱：</label>
                    <input class="txt" type="text" id="user_id_login" name="user_id_login" value="" />
                </div>
                <div class="item">
                    <label class="lbl">密码：</label>
                    <input class="txt" type="password" id="passwd_login" name="passwd_login" />
                </div>
                <div class="item">
                    <label class="lbl">验证码：</label>
                    <input class="txt" type="text" id="captcha_login" name="captcha_login" />
                </div>
                <div class="item">
                    <a href="javascript:refresh_captcha('captchaImage_login',<%=CaptchaImage.CaptchaType.login%>)">
                        <img width="98" height="25" id="captchaImage_login" name="captchaImage_login"
                                alt="验证码" src="user/Captcha.ashx?dstct=<%=CaptchaImage.CaptchaType.login%>"/>
                        看不清？换一个
                    </a>        
                </div>
                <div class="item indent">
                    <input class="chk" type="checkbox" checked="checked" id="remember_login" name="forever_login" /><label for="remember">记住我</label>
                </div>
                <div class="item indent">
                    <div class="msg" id = "lgnmsg"></div>
                    <button type="button" id="lgin" name="lgin" >登 录</button>
                    <!--span class="forgot_pwd"><a href="javascript:findpwd();">忘记密码</a></span-->
                </div>
            </div>
        </div>
    </form>
    <script type="text/javascript">
        function lgevent_dosuccess() {
        }
        $("#user_id_login").bind("keypress", function (event) {
		        if (event.keyCode == 13) //Enter
		        {
		            $("#passwd_login").focus();
		            $("#passwd_login").select();
		        }
		});
		$("#passwd_login").bind("keypress", function (event) {
		    if (event.keyCode == 13) //Enter
		    {
		        $("#captcha_login").focus();
		        $("#captcha_login").select();
		    }
		});
		$("#captcha_login").bind("keypress", function (event) {
		    if (event.keyCode == 13) //Enter
		    {
		        $("#lgin").click();
		    }
		});
		$("#lgin").bind("click", function (event) {
            $("#lgin").attr("disabled",true);
		    $("div#lgnmsg").html('<span style="color:#000000;">正在验证账号。。。</span>');
		    var postdata = {
		        ID: $("#user_id_login").val(),
		        Passwd: $.md5($("#passwd_login").val()),
		        Captcha: $("#captcha_login").val()
		    };
		    var postjson = JSON.stringify(postdata);
		    $.ajax({
		        //要用post方式
		        type: "POST",
		        url: "user/SignUpAjax.asmx/Login",
		        dataType: "json",  //dataType: "json",
		        data: postjson,
		        //方法所在页面和方法名Data.aspx                         
		        contentType: "application/json;",
		        success: function (data) {
		            //返回的数据用data.d获取内容  json                             
		            //alert("data:" + data.d);
		            if (data.d == 0) {
		                $("div#lgnmsg").html('<span>验证成功!</span>');
		                lgevent_dosuccess();
		            }
                    else
                    {
                        $("#lgin").attr("disabled",false);
                        if (data.d == -2){
		                        $("div#lgnmsg").html('<span style="color:#FF0000;">验证码错误!</span>');
                                $("#captcha_login").focus();
		                        $("#captcha_login").select();
                        }
		                else{
                            refresh_captcha('captchaImage_login',<%=CaptchaImage.CaptchaType.login%>);
                            if (data.d == -1) {
		                        $("div#lgnmsg").html('<span style="color:#FF0000;">用户名或密码错误!</span>');
                                $("#user_id_login").focus();
		                        $("#user_id_login").select();
		                    }
		                    else
		                        $("div#lgnmsg").html('<span style="color:#FF0000;">出现未知错误，请与管理员联系!</span>');
                        }
                    }
                },
		        error: function (err) {
                    refresh_captcha('captchaImage_login',<%=CaptchaImage.CaptchaType.login%>);
		            $("div#lgnmsg").html('<span style="color:#FF0000;">异步错误：' + err.d +
                                        '，请与管理员联系!</span>');
                    $("#lgin").attr("disabled",false);
		        }
		    });
		    //禁用按钮的提交     
		    return false;
		});
    </script>
    <%}
      else
      { %>
        <span>您好，<strong><%=Server.HtmlEncode(
            UserStatue.GetCurrentUserContext().Profile.nickname)%>
        </strong>。</span>
    <%} %>
</div>