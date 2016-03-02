function refresh_captcha(imgID, dstct) {
    var imgSrc = $("#" + imgID);
    var src = imgSrc.attr("src");
    imgSrc.attr("src", "user/Captcha.ashx?dstct=" + dstct + "&ts=" + (new Date()).getTime());
}