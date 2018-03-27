ckFramework.FileUploadService = (function (fileUploadService) {
    var FileElementId = "";

    var SubmitFile = function (paramSelectFile)
    {
        ckFramework.ModalHelper.OpenWait();
        var form = $('#' + paramSelectFile.FileFormId);
        form.attr('action', paramSelectFile.SubmitAction);
        form.attr("encoding", "multipart/form-data");
        form.attr("enctype", "multipart/form-data");
        form.submit();

        BlockResubmit();
        return false;
    }

    var UploadFinish = function(result)
    {
        $('#' + FileElementId).val('');
        ckFramework.ModalHelper.CloseWait();
        if (result == "1")
        {
            ckFramework.ModalHelper.Alert(decodeURI(ckFramework.ClientMessage.GetMessage().FileUploadSuccess));
        }
        else {
            ckFramework.ModalHelper.Alert(decodeURI(result));
        }
    }

    function BlockResubmit() {
        var downloadTimer = window.setInterval(function () {
            var token = ckFramework.CookieService.GetCookie("ImportResultMsg");
            if (token) {
                window.clearInterval(downloadTimer);
                ckFramework.CookieService.DeleteCookie("ImportResultMsg");
                UploadFinish(token);
                fileUploadService.CallBack(token);
            }
        }, 500);
    }

    fileUploadService.FileSelected = function(paramSelectFile)
    {
        var fileName = $('#' + paramSelectFile.FileElementId).val();
        if(fileName)
        {
            FileElementId = paramSelectFile.FileElementId;
            SubmitFile(paramSelectFile)
        }
    }

    fileUploadService.ExportFile = function(srcUrl)
    {
        var iframe = document.createElement("iframe");
        iframe.src = srcUrl;
        iframe.style.display = "none";
        document.body.appendChild(iframe);
    }

    fileUploadService.CallBack = function (result) { }

    return fileUploadService;
}(ckFramework.FileUploadService || {}));