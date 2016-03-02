<%@ Page Title="" Language="C#" MasterPageFile="~/frames/mainframe.master" AutoEventWireup="true" CodeFile="UploadMedia.aspx.cs" Inherits="UploadMedia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="css/Upload.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript" src="js/Upload_HTML5.js"></script>
    <script type="text/javascript" language="javascript">
    <%if(uploadStatue == null)
    { %>
        $(document).ready(
            function () {
                SetUploader("Upload",2000000000,false,[<%=GetFormatList()%>]);
            });
    <%}
    else
    { %>
        $(document).ready(
            function () {
                SetUploader("Upload",2000000000,true,[<%=GetFormatList()%>]);
            });
    <%} %>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="upload_tittle">上传支持<%=GetFormats() %>格式。</div>
    <div class="upload_msg"><%=uploadStatue == null ? "" : uploadStatue%></div>
    <div id="Upload"></div>
</asp:Content>

