function SetPlayer(divID, mediaID, width, height, funcErrorResponded) {
    var divObj = $("div#" + divID);
    divObj.html("Media loading...");
    var postdata = { "mID": mediaID,
        "wd": width,
        "ht": height,
        "ap":1
    };
    divObj.load("user/PlayerCtrl.ashx", postdata,
        function (response, status, xhr) {
            if (status != "success") {
                divObj.html("An error occured: <br/>" + xhr.status + " " + xhr.statusText);
                funcErrorResponded(response, status, xhr);
            }
        });
}