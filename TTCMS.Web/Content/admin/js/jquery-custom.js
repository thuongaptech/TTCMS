
function Ajax_pagination() {
    $(document).on("click", "#ajax_paging .pagination  a", function () {
        var url = $(this).attr("href");
        $.get(url).done(function (html) {
            $("#TableContainer").html(html);
        });
        return false;
    });
};
function LoadFunction(URLs) {
    var url = URLs + "?show=" + $('#ShowId').val() + "&&page=" + $('#Page').val() + "&&search=" + $('#Search').val();
    $.get(url).done(function(html) {
        $("#TableContainer").html(html);
    });
};
function SearchAjax(URLs) {
    var url = URLs + "?show=" + $('#ShowId').val() + "&&search=" + $('#Search').val();
    $.get(url).done(function (html) {
        $("#TableContainer").html(html);
    });
};
function LoadClickAjaxModal()
{
    $('[data-toggle="modal"]').click(function () {
        var url = $(this).attr("href");
        $('#modal-show').load(url, function (result) {
            $('#modal-form').modal({
                show: true,
                backdrop: 'static',
                keyboard: false
            });
        });
    });
};
function CompleteAjaxRequest(result) {
    var returnObj = eval('(' + result.responseText + ')');
    $("#AjaxAlert").html("");
    if (returnObj.RedirectUrl != null && returnObj.Success == true) {
        $('#modal-form').modal('hide');
        $('#TableContainer').load(returnObj.RedirectUrl, function (resulturl) {
            AjaxAlert(returnObj.Code, returnObj.Msg);
        });
    }
    else {
        AjaxAlert(returnObj.Code, returnObj.Msg);
    }
};
function LoadClickAjaxModalJson() {
    $('[data-ajax="modal-json"]').click(function () {
        $("#AjaxAlert").html("");
        var url = $(this).attr("href");
        $.ajax({
            type: "GET",
            url: url,
            async: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                if (result.RedirectUrl == null) {
                    if (result.Success == true) {
                        $("#modal-show").html(result.PartialViewHtml);
                        $('#modal-form').modal({
                            show: true,
                            backdrop: 'static',
                            keyboard: false
                        });
                    }
                    else {
                        AjaxAlert(result.Code, result.Msg);
                    }
                }
                else {
                    $('#TableContainer').load(result.RedirectUrl, function (result) {
                        AjaxAlert(result.Code, result.Msg);
                    });
                }
            },
            error: function (e) {
                alert(e.Message);
            }
        });
    });
};
function LoadButonClickAjaxModalJson() {
    $('[data-ajax="modal-json"]').click(function () {
        $("#AjaxAlert").html("");
        var url = $(this).attr("data-href");
        $.ajax({
            type: "GET",
            url: url,
            async: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                if (result.RedirectUrl == null) {
                    if (result.Success == true) {
                        $("#modal-show").html(result.PartialViewHtml);
                        $('#modal-form').modal({
                            show: true,
                            backdrop: 'static',
                            keyboard: false
                        });
                    }
                    else {
                        AjaxAlert(result.Code, result.Msg);
                    }
                }
                else {
                    $('#TableContainer').load(result.RedirectUrl, function (result) {
                        AjaxAlert(result.Code, result.Msg);
                    });
                }
            },
            error: function (e) {
                alert(e.Message);
            }
        });
    });
};
function onSuccess(result) {
    AjaxAlert(result.Code, result.Msg);
    if (result.Code == "alert-success") {
        $("#item_" + result.Id).remove();
    }
};
function AjaxAlert(Code, Msg) {
    $.get("/TT_Admin/Base/AjaxAlert?code=" + Code + "&&msg=" + Msg).done(function (html) {
        $("#AjaxAlert").html(html);
    });
    
};
function show_confirm(obj) {
    var r = confirm(obj.attr('data-alert'));
    if (r == true)
        window.location = obj.attr('href');
}
function BrowseServer() {
    var finder = new CKFinder();
    //finder.basePath = '../';
    finder.selectActionFunction = SetFileField;
    finder.popup();
}
function SetFileField(fileUrl) {
    $("#Img_Thumbnail").val(fileUrl);
    document.getElementById('Image').src = fileUrl;
}
