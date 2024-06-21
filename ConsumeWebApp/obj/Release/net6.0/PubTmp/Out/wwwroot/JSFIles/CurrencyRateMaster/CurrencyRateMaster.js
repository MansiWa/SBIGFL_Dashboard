function AddCurrencyRate() {
     
    const urlParams = new URLSearchParams(window.location.search);

    // Extract the values
    const CurrencyId = urlParams.get("CurrencyId");
    const CurrencyName = urlParams.get("CurrencyName");

    $("#createModal").modal("show");
    //$("#co_id").val('');
    $("#cr_currencyid").val(CurrencyId);
    $("#cr_currency_name").val(CurrencyName);
    $("#cr_currencyrate").val('');
    $("#cr_fromdate").val('');
    $("#cr_todate").val('');
    $('#cr_isactive').prop('checked', true);
    $('#btnsubmit').prop('disabled', false);
}
function CancelData() {
    $("#createModal").modal("hide");
    window.location.reload()

    $("#createModal12").modal("hide");
    window.location.reload()
}
function validateFloatInput(inputElement) {
    // Get a reference to the input element

    var inputValue = inputElement.value;
    // Use a regular expression to allow only numbers and one decimal point
    const validInput = inputValue.match(/^\d*(\.\d{0,2})?$/);

    // If the input is not valid, display an alert and clear the input value
    if (!validInput) {
        alert("Please enter a valid float number.");
        cr_currencyrate.value = "";
    }

}
function submitCurrencyRate() {
    if ($('#cr_currencyrate').val() == '') {
        alert("Please enter currency rate!");
        return false;
    } else
        if ($('#cr_fromdate').val() == '') {
            alert("Please enter from date!");
            return false;
        } 

    var isActive = $('#cr_isactive').is(':checked') ? 1 : 0;

    var data = {
        cr_id: $('#cr_id').val(),
        cr_currencyid: $('#cr_currencyid').val(),
        cr_currencyrate: $('#cr_currencyrate').val(),
        cr_currency_name: $('#cr_currency_name').val(),
        cr_fromdate: $('#cr_fromdate').val(),
        cr_isactive: isActive,
        cr_todate: $('#cr_todate').val(),
    };
    $.ajax({
        type: 'POST',
        url: '/CurrencyRateMaster/Create', // Use the form's action attribute
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
function updateCurrencyRate() {
    if ($('#cr_currency_name1').val() == '-- Select --') {
        alert("Please select currency!");
        return false;
    }

    var isActive = $('#cr_isactive1').is(':checked') ? 1 : 0;

    var data = {
        cr_id: $('#cr_id1').val(),
        cr_currencyid: $('#cr_currencyid1').val(),
        cr_currencyrate: $('#cr_currencyrate1').val(),
        cr_currency_name: $('#cr_currency_name1').val(),
        cr_isactive: isActive,
        cr_fromdate: $('#cr_fromdate1').val(),
        cr_todate: $('#cr_todate1').val()
    };
    $.ajax({
        type: 'POST',
        url: '/CurrencyRateMaster/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {

            window.location.reload();
        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error(error);
        }
    });
}
function editLinkClick(cr_id, cr_currencyrate, cr_currencyid, cr_currency_name, cr_fromdate, cr_isactive) {
    $("#createModal12").modal("show");
    $('#cr_id1').val(cr_id);
    $("#cr_currencyrate1").val(cr_currencyrate);
    $("#cr_currency_name1").val(cr_currency_name);
    $('#cr_currencyid1').val(cr_currencyid)
    var rawDate = cr_fromdate; // Assuming rawDate is "21-10-2023"
    var parts = rawDate.split('-'); // Split the date string by hyphens
    if (parts.length === 3) {
        var formattedDate = parts[2] + '-' + parts[1] + '-' + parts[0];
        $('#cr_fromdate1').val(formattedDate);
    }
    
    if (cr_isactive === "Active") {
        $('#cr_isactive1').prop('checked', true);
    } else {
        $('#cr_isactive1').prop('checked', false);
    }

}
function ToggleSwitch(cr_id, cr_currency_name, cr_currencyid, cr_currencyrate, cr_fromdate, cr_todate, cr_isactive) {
    var data = {
        cr_id: cr_id,
        cr_currency_name: cr_currency_name, cr_currencyid: cr_currencyid,
        cr_currencyrate: cr_currencyrate, cr_fromdate: cr_fromdate,
        cr_todate: cr_todate, cr_isactive: cr_isactive
    };
    $.ajax({
        type: 'POST',
        url: '/CurrencyRateMaster/Create', // Use the correct URL or endpoint
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
    const CurrencyId = searchParams.get("CurrencyId");
    const CurrencyName = searchParams.get("CurrencyName");
    var baseUrl = window.location.origin + '/CurrencyRateMaster';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Excel' + (statusValue ? '?status=' + statusValue : '') + ('&CurrencyId=' + CurrencyId) + ('&CurrencyName=' + CurrencyName);

    window.location.href = newUrl;


}

function Pdf() {
    var currentUrl = window.location.href;
    var searchParams = new URL(currentUrl).searchParams;
    const CurrencyId = searchParams.get("CurrencyId");
    const CurrencyName = searchParams.get("CurrencyName");
    var baseUrl = window.location.origin + '/CurrencyRateMaster';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Pdf' + (statusValue ? '?status=' + statusValue : '') + ('&CurrencyId=' + CurrencyId) + ('&CurrencyName=' + CurrencyName);

    window.location.href = newUrl;
}
function performAction() {
    const urlParams = new URLSearchParams(window.location.search);
    // Extract the values
    const CurrencyId = urlParams.get("CurrencyId");
    const CurrencyName = urlParams.get("CurrencyName");
    var status = ($('#toggleStatus').is(':checked')) ? '1' : '0';
    window.location.href = "/CurrencyRateMaster/Index?status=" + status + "&CurrencyId=" + CurrencyId + "&CurrencyName=" + CurrencyName;

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