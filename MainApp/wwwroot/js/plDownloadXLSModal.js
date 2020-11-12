$('#downloadXLSModal').on('show.bs.modal', function () {
    xlsIncludeExcluded = getCookie("XLSIncludeExcluded");
    xlsIncludeUnavailable = getCookie("XLSIncludeUnavailable");
    xlsIncludeUnverified = getCookie("XLSIncludeUnverified");

    if (xlsIncludeUnavailable == 'true') {
        $('#chkIncludeUnavailable').prop('checked', true);
    }
    else {
        $('#chkIncludeUnavailable').removeAttr('checked');
    }
    if (xlsIncludeExcluded == 'true') {
        $('#chkIncludeExcluded').prop('checked', true);
    }
    else {
        $('#chkIncludeExcluded').removeAttr('checked');
    }
    if (xlsIncludeUnverified == 'true') {
        $('#chkIncludeUnverified').prop('checked', true);
    }
    else {
        $('#chkIncludeUnverified').removeAttr('checked');
    }
})

$('#btnDownloadXLSGenerate').on('click', function () {
    xlsIncludeExcluded = $('#chkIncludeExcluded').is(':checked');
    xlsIncludeUnavailable = $('#chkIncludeUnavailable').is(':checked');
    xlsIncludeUnverified = $('#chkIncludeUnverified').is(':checked');
    setCookies();

    $('#btnDownloadXLSGenerate').html("Формируется...");
    $('#btnDownloadXLSGenerate').prop('disabled', true);

    $.ajax({
        type: "POST",
        url: "/" + controllerName + "/generateXLS",
        data: JSON.stringify({ includeExcluded: xlsIncludeExcluded, includeUnavailable: xlsIncludeUnavailable, includeUnverified: xlsIncludeUnverified }),
        contentType: "application/json;charset=utf-8",
        success: function (response) {
            $('#btnDownloadXLSGenerate').removeAttr('disabled');
            $('#btnDownloadXLSGenerate').html("Сформировать");
            $('#downloadXLSModal').modal('hide');
            window.location = controllerName + "/getXLS";
        },
        error: function (errormessage) {
            alert("Неизвестная ошибка " + errormessage);
            $('#btnDownloadXLSGenerate').removeAttr('disabled');
            $('#btnDownloadXLSGenerate').html("Сформировать");
            $('#downloadXLSModal').modal('hide');
        }
    });
})

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function setCookies() {
    document.cookie = "XLSIncludeExcluded=" + xlsIncludeExcluded + "; expires=Fri, 31 Dec 9999 23:59:59 GMT";
    document.cookie = "XLSIncludeUnavailable=" + xlsIncludeUnavailable + "; expires=Fri, 31 Dec 9999 23:59:59 GMT";
    document.cookie = "XLSIncludeUnverified=" + xlsIncludeUnverified + "; expires=Fri, 31 Dec 9999 23:59:59 GMT";
}