var __upLoad__fileTypes = [];
var __upLoad__maxFileSize = 300000;
var __upLoad__traditionalMode = false;
function __upLoad_right(mainStr, lngLen) {
    // alert(mainStr.length) 
    if (mainStr.length - lngLen >= 0 && mainStr.length >= 0 && mainStr.length - lngLen <= mainStr.length) {
        return mainStr.substring(mainStr.length - lngLen, mainStr.length)
    }
    else { return null }
}
function __upLoad_isLegalType(file) {
    // File type asure
    var postfix = file.name.substring(file.name.lastIndexOf(".") + 1).toUpperCase();
    var badType = false;
    if (__upLoad__fileTypes != null && __upLoad__fileTypes.length > 0) {
        badType = true;
        for (var i = 0, ft; ft = __upLoad__fileTypes[i]; i++) {
            if (ft.toUpperCase() == postfix)
                badType = false;
        }
    }
    return !badType;
}
function $__upLoad__ID(id) {
    return document.getElementById(id);
}
//
// __upLoad__Output information
function __upLoad__Output(msg) {
    var m = $__upLoad__ID("upLoad__page_message_D73GTYmMk");
    m.innerHTML = m.innerHTML + msg;
}
function SetUploader(divID, maxFileSize,traditionalMode,fileTypes) {
    if (traditionalMode == null)
        traditionalMode = false;
    __upLoad__traditionalMode = traditionalMode;
    if (maxFileSize == null)
        maxFileSize = 300000;
    if (fileTypes == null)
        fileTypes = __upLoad__fileTypes;
    var divObj = $("div#" + divID);
    divObj.html("上传页面载入中...");
    var postdata = {
        "fatherDivID": divID,
        "maxFileSize":maxFileSize,
        "traditonalMode": traditionalMode,
        "fileTypes":fileTypes
    };
    divObj.load("user/Upload.aspx", postdata,
        function (response, status, xhr) {
            if (status != "success") {
                divObj.html("An error occured: <br/>" + xhr.status + " " + xhr.statusText);
                funcErrorResponded(response, status, xhr);
            }
            else {
                __upLoad__fileTypes = fileTypes;
                __upLoad__maxFileSize = maxFileSize;
                $__upLoad__ID("upLoad__page_submitbutton_D73GTYmMk").style.display = "none";
                if (traditionalMode == true) {
                    var fileselect = $__upLoad__ID("X_FILENAME");
                    fileselect.onchange=__upLoad__FileSelectHandler_t;
                    return;
                }
                // is XHR2 available?
                var xhr = null;
                if (window.XMLHttpRequest) {// code for all new browsers
                    xhr = new XMLHttpRequest();
                }
                else if (window.ActiveXObject) {// code for IE5 and IE6
                    xhr = new ActiveXObject("Microsoft.XMLHTTP");
                }
                if (xhr != null && xhr.upload) {
                    var fileselect = $__upLoad__ID("upLoad__page_fileselect_D73GTYmMk"),
                    filedrag = $__upLoad__ID("upLoad__page_filedrag_D73GTYmMk"),
                    submitbutton = $__upLoad__ID("upLoad__page_submitbutton_D73GTYmMk");
                    // file select
                    fileselect.addEventListener("change", __upLoad__FileSelectHandler, false);
                    // file drop
                    filedrag.addEventListener("dragover", __upLoad__FileDragHover, false);
                    filedrag.addEventListener("dragleave", __upLoad__FileDragHover, false);
                    filedrag.addEventListener("drop", __upLoad__FileSelectHandler, false);
                    filedrag.style.display = "block";
                    // remove submit button
                    submitbutton.style.display = "none";
                }
                else {
                    alert("浏览器无法支持当前上传模式，即将进入基本模式。");
                    SetUploader(divID, maxFileSize, true, fileTypes);
                }
            }
        });

}
// file drag hover
function __upLoad__FileDragHover(e) {
    e.stopPropagation();
    e.preventDefault();
    e.target.className = (e.type == "dragover" ? "hover" : "");
}
/*
// file selection
function __upLoad__FileSelectHandler(e) {
    // cancel event and hover styling
    __upLoad__FileDragHover(e);
    // fetch FileList object
    var files = e.target.files || e.dataTransfer.files;
    // process all File objects
    for (var i = 0, f; f = files[i]; i++) {
        __upLoad__ParseFile(f);
    }
}
*/
function __upLoad__ParseFile(file) {
    var szOut = "<p>文件信息: <strong>" + file.name +
          "</strong> 类别: <strong>" + file.type +
          "</strong> 大小: <strong>" + file.size +
          "</strong> bytes";
     // File type asure
    if (!__upLoad_isLegalType(file)) {
        szOut = szOut + "<strong> 文件类型不正确！</strong>";
    }
    if (file.size > __upLoad__maxFileSize)
        szOut = szOut + "<strong> 文件体积过大！</strong>"
    szOut = szOut + "</p>";
    __upLoad__Output(szOut);
    //if (file.type.indexOf("image") == 0) {
        //var reader = new FileReader();
        //reader.onload = function (e) {
        //}
        //reader.readAsDataURL(file);
    //}
}
function __upLoad__FileSelectHandler(e) {
    // cancel event and hover styling
    __upLoad__FileDragHover(e);
    // fetch FileList object
    var files = e.target.files || e.dataTransfer.files;
    // process all File objects
    for (var i = 0, f; f = files[i]; i++) {
        __upLoad__ParseFile(f);
        __upLoad__UploadFile(f);
    }
}
function __upLoad__FileSelectHandler_t(e) {
    $__upLoad__ID("upLoad__page_submitbutton_D73GTYmMk").style.display = "none";
    var showSubmit = true;
    // fetch FileList object
    var files;
    if (e == null)
        files = $__upLoad__ID("X_FILENAME").value;
    if (files == null) {
        if (e.target != null && e.target.files != null)
            files = e.target.files;
        if (e.dataTransfer != null && e.dataTransfer.files != null)
            files = e.dataTransfer.files;
        if (files == null) {
            alert("无法获得文件信息！");
            return;
        }
    }
    // show File informations
    var m = $__upLoad__ID("upLoad__page_message_D73GTYmMk");
    m.innerHTML = "";
    if (files != $__upLoad__ID("X_FILENAME").value) {
        for (var i = 0, f; f = files[i]; i++) {
            var szOut = "<p>文件信息: <strong>" + f.name +
          "</strong> 类别: <strong>" + f.type +
          "</strong> 大小: <strong>" + f.size +
          "</strong> bytes";
            // File type asure
            if (!__upLoad_isLegalType(f)) {
                szOut = szOut + "<strong> 文件类型不正确，请重新选择上传文件！</strong></p>";
                __upLoad__Output(szOut);
                showSubmit = false;
                continue;
            }
            // File size asure
            if (f.size > __upLoad__maxFileSize) {
                szOut = szOut + "<strong> 文件体积过大，请重新选择上传文件！</strong>"
                showSubmit = false;
            }
            szOut = szOut + "</p>";
            __upLoad__Output(szOut);
        }
    }
    else {
        var postfix = files.substring(files.lastIndexOf(".") + 1).toUpperCase();
        
        var szOut = "<p>文件信息: <strong>" + files +"</strong>";
        // File type asure
        var badType = false;
        if (__upLoad__fileTypes != null && __upLoad__fileTypes.length > 0) {
            badType = true;
            for (var i = 0, ft; ft = __upLoad__fileTypes[i]; i++) {
                if (ft.toUpperCase() == postfix)
                    badType = false;
            }
        }
        if (badType) {
            szOut = szOut + "<strong> 文件类型不正确，请重新选择上传文件！</strong></p>";
            showSubmit = false;
        }
        szOut = szOut + "</p>";
        __upLoad__Output(szOut);
    }
    if (showSubmit)
        $__upLoad__ID("upLoad__page_submitbutton_D73GTYmMk").style.display = "block";
}
function __upLoad__UploadFile(file) {
    var xhr = new XMLHttpRequest();
    // create progress bar
    var o = $__upLoad__ID("upLoad__page_progress_D73GTYmMk");
    var progress = o.appendChild(document.createElement("p"));
    //file.type == "image/jpeg"
    if (xhr.upload && file.size <= __upLoad__maxFileSize && __upLoad_isLegalType(file)) {
        // progress bar
        var cdText = document.createTextNode("文件：" + file.name);
        progress.appendChild(cdText);
        var szOriginText = progress.innerHTML;
        var div1 = document.createElement("div");
        var aCancle = document.createElement("a");
        div1.appendChild(aCancle);
        progress.appendChild(div1);
        aCancle.innerHTML = "取消上传";
        aCancle.addEventListener("click", function (e) { xhr.abort(); }, false);
        aCancle.setAttribute("href", "javascript:void();");
        var cdText = null;
        xhr.upload.addEventListener("progress", function (e) {
            var pc = parseInt(100 - (e.loaded * 100 / e.total));
            if (e.loaded == e.total)
                progress.innerHTML = szOriginText + "<br/>正在处理上传文件，请稍候。";
            else {
                if (cdText == null) {
                    cdText = document.createElement("div");
                    progress.appendChild(cdText);
                    progress.appendChild(cdText)
                }
                if(e.loaded ==0)
                    cdText.innerHTML = "等待上传队列...";
                else
                    cdText.innerHTML = "已上传：" + (100 - pc) + "%";
            }
            progress.style.backgroundPosition = pc + "% 0";
        }, false);
        xhr.onreadystatechange = function (e) {
            if (xhr.readyState == 4) {
                progress.className = (xhr.status == 200 ? "success" : "failed");
                progress.innerHTML = szOriginText + "<br/>上传" + (xhr.status == 200 ? "成功！" : "失败！");
                if (xhr.status != 200) {
                    __upLoad__Output("传输文件<strong>" + file.name + "</strong>失败！" );
                }
            }
        }
        //xhr.open("post", $__upLoad__ID("upLoad__page_form_D73GTYmMk").action, true);
        xhr.open("post", document.URL, true);
        xhr.setRequestHeader("X_FILENAME", file.name);
        xhr.send(file);
    }
    else {
        progress.appendChild(document.createTextNode("文件：" + file.name + "\n上传已被终止！"));
        progress.className = "failed";
    }
}

