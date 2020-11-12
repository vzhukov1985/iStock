$('#plSettingsModal').on('show.bs.modal', function () {
    $.ajax({
        type: 'GET',
        url: "/" + controllerName + "/settings",
        datatype: "json",
        success: function (response) {
            $('#inpSupplierName').val(response.supplierName);
            $('#inpPricelistName').val(response.name);
            $('#inpPreorderInDays').val(response.preorderInDays);
            $('#inpMinStockAvail').val(response.minStockAvail);
            $('#chkIsFavorite').prop('checked', response.isFavorite);
            $('#currency').html(response.exchangeRateCurrency);
            if (response.isAutoExchangeRate) {
                $('#exchangeRateAuto').prop('checked', true);
                $('#exchangeRateVal').prop('disabled', true);
            }
            else {
                $('#exchangeRateCustom').prop('checked', true);
                $('#exchangeRateVal').removeAttr('disabled');
            }
            $('#exchangeRateVal').val(response.exchangeRate);
            $('#plSettingsModal').modal('show');
        }
    });
})

$('input[type=radio][name=exchangeRateType]').change(function () {
    if (this.value == 'Auto') {
        $('#exchangeRateVal').prop('disabled', true);
        $.ajax({
            type: 'GET',
            url: "/" + controllerName + "/getAutoExchangeRate",
            success: function (response) {
                $('#exchangeRateVal').val(response);
            }
        });
    }
    else {
        $('#exchangeRateVal').removeAttr('disabled');
    }
})

$('#btnSaveSettings').on('click', function () {
    $.ajax({
        type: 'POST',
        url: "/" + controllerName + "/settings",
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify({
            supplierName: $('#inpSupplierName').val(),
            name: $('#inpPricelistName').val(),
            preorderInDays: Number($('#inpPreorderInDays').val()),
            minStockAvail: Number($('#inpMinStockAvail').val()),
            isFavorite: $('#chkIsFavorite').is(':checked'),
            isCustomExchangeRate: $('#exchangeRateCustom').is(':checked'),
            CustomExchangeRate: $('#exchangeRateVal').val()
        }),
        success: function () {
            if (typeof updatePricelistHeader !== "undefined") { updatePricelistHeader(); }
            table.replaceData();
            $('#plSettingsModal').modal('hide');
        }
    });
})