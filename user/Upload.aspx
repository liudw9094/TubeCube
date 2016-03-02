<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Upload.aspx.cs" Inherits="user_Upload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="upLoad__page_form_D73GTYmMk" method="post" enctype="multipart/form-data">
    <fieldset>
        <legend>上传</legend>
        
        <div>
            <%=(bTraditionalMode?"":"若无法上传文件，请使用") %>
            <a href="javascript:void();"
            onclick = "SetUploader('<%=FatherDiv%>', <%=nMaxFileSize%>, <%=(!bTraditionalMode).ToString().ToLower()%>);">
                <%=(bTraditionalMode?"炫丽模式":"基本模式") %>
            </a>
        </div>
        <%--<input type="hidden" id="upLoad__page_MAX_FILE_SIZE_D73GTYmMk" name="upLoad__page_MAX_FILE_SIZE_D73GTYmMk" value="<%=nMaxFileSize%>" />--%>
        <%if (!bTraditionalMode)
          { %>
            <div>
              <label for="upLoad__page_fileselect_D73GTYmMk">请选择需要上传的文件:</label>
              <input type="file" id="upLoad__page_fileselect_D73GTYmMk"
                name="upLoad__page_fileselect_D73GTYmMk[]"
                maxlength="<%=nMaxFileSize%>" multiple />
              <div id="upLoad__page_filedrag_D73GTYmMk">您可以将文件直接拖拽至该区域</div>
            </div>
        <%}
          else
          { %>
            <div>
              <label for="upFile">Files to upload:</label>
              <input type="file" id="X_FILENAME" name="X_FILENAME[]"
                maxlength="<%=nMaxFileSize%>" multiple />
            </div>
        <%} %>
            <div id="upLoad__page_submitbutton_D73GTYmMk" style="display:none;">
              <button type="submit">上传文件</button>
            </div>
            <table width = "100%">
                <tr>
                    <%if (!bTraditionalMode)
                      { %>
                    <td width = "250px" align = "right">
                        <div id="upLoad__page_progress_D73GTYmMk" align = "center"></div>
                    </td>
                    <%} %>
                    <td align="left" valign="top">
                        <div id="upLoad__page_message_D73GTYmMk"></div>
                    </td>
                </tr>
            </table>
            
    </fieldset>
    </form>
</body>
</html>
