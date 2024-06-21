function AddDesignation() {

    const urlParams = new URLSearchParams(window.location.search);

    // Extract the values
    const DepartmentId = urlParams.get("DepartmentId");
    const DepartmentName = urlParams.get("DepartmentName");

    $("#createModal").modal("show");
    //$("#co_id").val('');
    $("#de_department_id").val(DepartmentId);
    $("#de_designation_name").val('');
    $("#de_designation_code").val('');
    $("#de_isactive").val('');
    $("#de_department_name").val(DepartmentName)

    $('#btnsubmit').prop('disabled', false);

}
function CancelData() {
    $("#createModal").modal("hide");
    window.location.reload()
}
function submitDesignation() {

    if ($('#de_designation_code').val() == '') {
        alert("Please enter code!");
        return false;
    } else
        if ($('#de_designation_name').val() == '') {
            alert("Please enter name!");
            return false;
        }


    var isActive = $('#de_isactive').is(':checked') ? 1 : 0;

    var data = {
        de_id: $('#de_id').val(),
        de_isactive: isActive,
        de_department_id: $('#de_department_id').val(),
        de_designation_name: $('#de_designation_name').val(),
        de_designation_code: $('#de_designation_code').val(),
        de_department_name: $('#de_department_name').val()

    };
    $.ajax({
        type: 'POST',
        url: '/DesignationMaster/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            $("#createModal").modal("hide");
            window.location.reload();
        },

        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}
function editLinkClick(de_id, de_department_id, de_department_name, de_designation_code, de_designation_name, de_isactive) {
    $("#createModal").modal("show");
    $('#de_id').val(de_id);
    $("#de_department_id").val(de_department_id);
    $("#de_designation_name").val(de_designation_name);
    $("#de_department_name").val(de_department_name);
    $("#de_designation_code").val(de_designation_code);

    var isActive = $('#de_isactive').val(de_isactive);

    if (de_isactive === "Active") {
        $('#de_isactive').prop('checked', true);
    } else {
        $('#de_isactive').prop('checked', false);
    }

}
function ToggleSwitch(de_id, de_department_id, de_department_name, de_designation_code, de_designation_name, de_isactive) {
    var data = {
        de_id: de_id,
        de_designation_code: de_designation_code,
        de_department_id: de_department_id,
        de_department_name: de_department_name,
        de_designation_name: de_designation_name,
        de_isactive: de_isactive
    };
    $.ajax({
        type: 'POST',
        url: '/DesignationMaster/Create', // Use the correct URL or endpoint
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
    const DepartmentId = searchParams.get("DepartmentId");
    const DepartmentName = searchParams.get("DepartmentName");
    var baseUrl = window.location.origin + '/DesignationMaster';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Excel' + (statusValue ? '?status=' + statusValue : '') + ('&DepartmentId=' + DepartmentId) + ('&DepartmentName=' + DepartmentName);

    window.location.href = newUrl;


}

function Pdf() {
    var currentUrl = window.location.href;
    var searchParams = new URL(currentUrl).searchParams;
    const DepartmentId = searchParams.get("DepartmentId");
    const DepartmentName = searchParams.get("DepartmentName");
    var baseUrl = window.location.origin + '/DesignationMaster';
    var statusValue = searchParams.get('status');

    statusValue = statusValue || '1'; // Set a default value if 'status' is null

    var newUrl = baseUrl + '/Pdf' + (statusValue ? '?status=' + statusValue : '') + ('&DepartmentId=' + DepartmentId) + ('&DepartmentName=' + DepartmentName);

    window.location.href = newUrl;
}
function performAction() {
    const urlParams = new URLSearchParams(window.location.search);
    // Extract the values
    const DepartmentId = urlParams.get("DepartmentId");
    const DepartmentName = urlParams.get("DepartmentName");
    var status = ($('#toggleStatus').is(':checked')) ? '1' : '0';
    window.location.href = "/DesignationMaster/Index?status=" + status + "&DepartmentId=" + DepartmentId + "&DepartmentName=" + DepartmentName;

}
function trimInput() {
    var code = document.getElementById("de_designation_code");
    code.value = code.value.trim();

    var code = document.getElementById("de_designation_name");
    code.value = code.value.trim();

}