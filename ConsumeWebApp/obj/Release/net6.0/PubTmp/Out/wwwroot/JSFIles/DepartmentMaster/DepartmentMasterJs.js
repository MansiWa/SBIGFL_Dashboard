function AddDepartment() {
    $("#createModal").modal("show");
    //$("#d_id").val('');
    $("#d_department_name").val('');
    $("#d_department_code").val('');
    $('#btnsubmit').prop('disabled', false);
} 
function GoToDes(d_id, d_department_name) {
    window.location.href = '/DesignationMaster?DepartmentId=' + d_id + '&DepartmentName=' + d_department_name;
}
function ClearData() {
    $('#d_id').val(''),
        $('#d_department_name').val(''),
        $('#d_department_code').val('')
}
function CancelData() {
    $("#createModal").modal("hide");
    window.location.reload()
}
function submitDepartment() {
    if ($('#d_department_code').val() == '') {
        alert("Please enter code!");
        return false;
    } else if ($('#d_department_name').val() == '') {
        alert("Please enter name!");
        return false;
    }
    var isActive = $('#d_isactive').is(':checked') ? 1 : 0;
    var data = {
        d_id: $('#d_id').val(),
        d_department_name: $('#d_department_name').val(),
        d_department_code: $('#d_department_code').val(),
        d_isactive: isActive,
    };
    $.ajax({
        type: 'POST',
        url: '/DepartmentMaster/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            $("#createModal").modal("hide");
            window.location.reload();
        },
        error: function (xhr, status, error) {
        }
    });
}
function editLinkClick(d_id, d_department_code, d_department_name, d_isactive) {

    $("#createModal").modal("show");
    $('#d_id').val(d_id);
    $("#d_department_name").val(d_department_name);
    $("#d_department_code").val(d_department_code);
    if (d_isactive === "Active") {
        $('#d_isactive').prop('checked', true);
    } else {
        $('#d_isactive').prop('checked', false);
    }
}

function ToggleSwitch(d_id, d_department_code, d_department_name, d_isactive) {
    var data = {

        d_id: d_id,
        d_department_code: d_department_code,
        d_department_name: d_department_name,
        d_isactive: d_isactive
    };
    $.ajax({
        type: 'POST',
        url: '/DepartmentMaster/Create', // Use the correct URL or endpoint
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
    var baseUrl = window.location.origin + '/DepartmentMaster';
    var statusValue = searchParams.get('status');
    statusValue = statusValue || '1'; // Set a default value if 'status' is null
    var newUrl = baseUrl + '/Excel' + (statusValue ? '?status=' + statusValue : '');
    window.location.href = newUrl;
}

function Pdf() {
    var currentUrl = window.location.href;
    var searchParams = new URL(currentUrl).searchParams;
    var baseUrl = window.location.origin + '/DepartmentMaster';
    var statusValue = searchParams.get('status');
    statusValue = statusValue || '1'; // Set a default value if 'status' is null
    var newUrl = baseUrl + '/Pdf' + (statusValue ? '?status=' + statusValue : '');
    window.location.href = newUrl;
}
function performAction() {
    var status = ($('#toggleStatus').is(':checked')) ? '1' : '0';
    window.location.href = "/DepartmentMaster/Index?status=" + status;
}
function trimInput() {
    var code = document.getElementById("d_department_code");
    code.value = code.value.trim();

    var code = document.getElementById("d_department_name");
    code.value = code.value.trim();

}
