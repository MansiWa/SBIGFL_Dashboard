function AddUserMaster() {
    $("#createModal").modal("show");
    $("#AddModel").show();
    $("#EditModel").hide();
    $("#um_staffname1").hide();
    $("#um_staffname").show();
    $("#um_rolename").val('');
    $("#um_user_name").val('');
    $("#um_user_name1").hide();
    $("#um_isactive").val('');
    $('#btnsubmit').prop('disabled', false);

}
function CancelData() {
    $("#createModal").modal("hide");
    window.location.reload()
}
function submitUserMaster() {
    if ($('#um_staffname').val() == '') {
        alert("Please select Staff name !");
        return false;
    }
    else if ($('#um_user_name').val() == '') {
        alert("Please enter username!");
        return false;
    }
    else if ($('#um_rolename').val() == '') {
        alert("Please select role !");
        return false;
    }

    var isActive = $('#um_isactive').is(':checked') ? 1 : 0;

    if ($('#um_staffname1').val() == '') {
        var data = {
            um_id: $('#um_id').val(),
            um_staffid: $('#um_staffname').val(),
            um_staffname: $('#um_staffname').find(":selected").text(),
            um_user_name: $('#um_user_name').val(),
            um_roleid: $('#um_rolename').val(),
            um_rolename: $('#um_rolename').find(":selected").text(),
            um_password: $('#um_password').val(),
            um_isactive: isActive,
        };
    }
    else {
        var data = {
            um_id: $('#um_id').val(),
            um_staffid: $('#um_staffid').val(),
            um_staffname: $('#um_staffname1').val(),
            um_user_name: $('#um_user_name1').val(),
            um_roleid: $('#um_rolename').val(),
            um_rolename: $('#um_rolename').find(":selected").text(),
            um_password: $('#um_password').val(),
            um_isactive: isActive,
        };
    }

    $.ajax({
        type: 'POST',
        url: '/UserMaster/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            $("#createModal").modal("hide");
            window.location.reload();
        },
        error: function (xhr, status, error) {
        }
    });
}
function editLinkClick(um_id, um_staffid, um_staffname, um_user_name, um_roleid, um_rolename, um_isactive) {


    $("#createModal").modal("show");
    $("#EditModel").show();
    $("#AddModel").hide();
    $("#um_staffname").hide();
    $("#um_staffname1").show();
    $("#um_user_name").hide();
    $("#um_user_name1").show();

    //if($("#um_staffname1").show==true)
    //{
    $('#um_id').val(um_id);
    $("#um_user_name").val(um_user_name);
    $("#um_user_name1").val(um_user_name);
    $("#um_staffid").val(um_staffid);
    $("#um_staffname1").val(um_staffname);
    $("#um_staffname").val(um_staffname);
    $("#um_rolename").val(um_rolename);

    //}

    var isActive = $('#um_isactive').val(um_isactive);

    if (um_isactive === "Active") {
        $('#um_isactive').prop('checked', true);
    } else {
        $('#um_isactive').prop('checked', false);
    }
    var role = um_rolename;
    var roleDropdown = document.getElementById("um_rolename");
    for (var i = 0; i < roleDropdown.options.length; i++) {
        if (roleDropdown.options[i].value === um_roleid) {
            roleDropdown.selectedIndex = i;
            break; // Exit the loop once the match is found
        }
    }

    $("#um_roleid").val(um_roleid);

}
function ToggleSwitch(um_id, um_staffid, um_staffname, um_user_name, um_roleid, um_rolename, status) {
    var data = {

        um_id: um_id,
        um_staffid: um_staffid,
        um_staffname: um_staffname,
        um_user_name: um_user_name,
        um_roleid: um_roleid,
        um_rolename: um_rolename,
        um_isactive: status
    };
    $.ajax({
        type: 'POST',
        url: '/UserMaster/Create', // Use the correct URL or endpoint
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
    var baseUrl = window.location.origin + '/UserMaster';
    var statusValue = searchParams.get('status');
    statusValue = statusValue || '1'; // Set a default value if 'status' is null
    var newUrl = baseUrl + '/Excel' + (statusValue ? '?status=' + statusValue : '');
    window.location.href = newUrl;
}

function Pdf() {
    var currentUrl = window.location.href;
    var searchParams = new URL(currentUrl).searchParams;
    var baseUrl = window.location.origin + '/UserMaster';
    var statusValue = searchParams.get('status');
    statusValue = statusValue || '1'; // Set a default value if 'status' is null
    var newUrl = baseUrl + '/Pdf' + (statusValue ? '?status=' + statusValue : '');
    window.location.href = newUrl;
}
function performAction() {
    var status = ($('#toggleStatus').is(':checked')) ? '1' : '0';
    window.location.href = "/UserMaster/Index?status=" + status;
}
function trimInput() {
    var username = document.getElementById("um_user_name");
    username.value = username.value.trim();

}

function username() {
    var data = {
        um_staffid: $('#um_staffname').val(),
        um_staffname: $('#um_staffname').find(":selected").text(),

    };
    $.ajax({
        type: 'POST',
        url: '/UserMaster/Username', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            if (response != null) {
                $("#um_user_name").val(response);
            }
        },
        error: function (xhr, status, error) {
        }
    });
}