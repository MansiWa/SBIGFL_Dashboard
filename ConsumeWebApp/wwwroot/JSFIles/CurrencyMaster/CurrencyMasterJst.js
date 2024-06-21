function AddCurrency() {
     
    $("#createModal").modal("show");
    //$("#co_id").val('');
    $("#cm_currencyname").val('');
    $("#cm_currencysymbol").val('');
    $("#cm_currency_format").val('');
    $("#cm_currencycode").val('');
    $("#cm_isactive").val('');
    $('#btnsubmit').prop('disabled', false);
}
function CancelData() {
    $("#createModal").modal("hide");
    window.location.reload()
}
function submitCurrency() {
    if ($('#cm_currencyname').val() == '') {
        alert("Please enter currency name!");
        return false;
    } else
    if ($('#cm_currencysymbol').val() == '') {
        alert("Please enter currency symbol!");
        return false;
    } else
    if ($('#cm_currency_format').val() == '') {
        alert("Please select currency format!");
        return false;
    } else
    if ($('#cm_currencycode').val() == '') {
        alert("Please enter currency code!");
        return false;
    }

    var isActive = $('#cm_isactive').is(':checked') ? 1 : 0;
    var data = {
        cm_id: $('#cm_id').val(),
        cm_currencyname: $('#cm_currencyname').val(),
        cm_currencysymbol: $('#cm_currencysymbol').val(),
        cm_currency_format: $('#cm_currency_format').val(),
        cm_isactive: isActive,
        cm_currencycode: $('#cm_currencycode').val(),
    };
    $.ajax({
        type: 'POST',
        url: '/CurrencyMaster/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            // Handle success
            // console.log(response);
            $("#createModal").modal("hide");
            //alert(response);
            window.location.reload();
        },
        error: function (xhr, status, error) {
            // Handle errors
             console.error(error);
        }
    });
}
function editLinkClick(cm_id, cm_currencycode, cm_currencyname, cm_currencysymbol, cm_currency_format, cm_isactive) {
    $("#createModal").modal("show");
    $('#cm_id').val(cm_id);
    $("#cm_currencyname").val(cm_currencyname);
    $("#cm_currencysymbol").val(cm_currencysymbol);
    $("#cm_currency_format option:selected").text(cm_currency_format);
    //$("#cm_currency_format").val(cm_currency_format);
    $("#cm_currencycode").val(cm_currencycode);
    if (cm_isactive === "Active") {
        $('#cm_isactive').prop('checked', true);
    } else {
        $('#cm_isactive').prop('checked', false);
    }
}
function GoToRate(cm_id, cm_currencyname) {
    window.location.href = '/CurrencyRateMaster?CurrencyId=' + cm_id + '&CurrencyName=' + cm_currencyname;
}
function ToggleSwitch(cm_id, cm_currencycode, cm_currencyname, cm_currencysymbol, cm_currency_format, cm_isactive) {
    var data = {
        cm_id: cm_id,
        cm_currencycode: cm_currencycode,
        cm_currencyname: cm_currencyname,
        cm_currencysymbol: cm_currencysymbol,
        cm_currency_format: cm_currency_format,
        cm_isactive: cm_isactive
    };
    $.ajax({
        type: 'POST',
        url: '/CurrencyMaster/Create', // Use the correct URL or endpoint
        data: data,
        success: function (response) {
            // Handle success
            window.location.reload(); // Reload the page on success
        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error(error);
        }
    });
}
function Excel() {
    var currentUrl = window.location.href;
    var searchParams = new URL(currentUrl).searchParams;
    var baseUrl = window.location.origin + '/CurrencyMaster';
    var statusValue = searchParams.get('status');
    statusValue = statusValue || '1'; // Set a default value if 'status' is null
    var newUrl = baseUrl + '/Excel' + (statusValue ? '?status=' + statusValue : '');
    window.location.href = newUrl;
}

function Pdf() {
    var currentUrl = window.location.href;
    var searchParams = new URL(currentUrl).searchParams;
    var baseUrl = window.location.origin + '/CurrencyMaster';
    var statusValue = searchParams.get('status');
    statusValue = statusValue || '1'; // Set a default value if 'status' is null
    var newUrl = baseUrl + '/Pdf' + (statusValue ? '?status=' + statusValue : '');
    window.location.href = newUrl;
}
function performAction() {
    var status = ($('#toggleStatus').is(':checked')) ? '1' : '0';
    window.location.href = "/CurrencyMaster/Index?status=" + status;
}

function trimInput() {
    var code = document.getElementById("cm_currencycode");
    code.value = code.value.trim();

    var name = document.getElementById("cm_currencyname");
    name.value = name.value.trim();

    var format = document.getElementById("cm_currency_format");
    format.value = format.value.trim();

    var symbol = document.getElementById("cm_currencysymbol");
    symbol.value = symbol.value.trim();

}