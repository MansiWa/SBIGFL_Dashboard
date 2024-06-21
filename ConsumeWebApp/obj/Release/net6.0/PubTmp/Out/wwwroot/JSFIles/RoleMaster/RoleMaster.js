function AddRole() {
    window.location.href = '/RoleMaster/EditIndex';
}
function GoBack() {
    window.location.href = '/RoleMaster/Index';
}
$(document).ready(function () {
    // Handle the click event for the flexCheckDefault checkbox
    $("#dataTable").on("click", ".flexCheckDefault", function () {
        // Find the parent row of the clicked checkbox
        var row = $(this).closest("tr");


        // Check or uncheck all checkboxes in the row based on the flexCheckDefault checkbox state
        $("input[type='checkbox']", row).prop("checked", $(this).prop("checked"));
    });
});
function submitRole() {
    /* event.preventDefault();*/


    if ($('#r_rolename').val() == '') {
        alert("Please enter role name!");
        return false;
    } else
        if ($('#r_description').val() == '') {
            alert("Please enter description!");
            return false;
        } else
            if ($('#r_module').val() == '') {
                alert("Please enter module!");
                return false;
            }
    var menuDataArray = [];


    // Iterate over each row in the table (assuming each row represents a menu)
    $("#dataTable tbody tr").each(function () {
        // Check if at least one checkbox is checked in the current row
        if ($(this).find("input[type='checkbox']:checked").length > 0) {
            var menuData = {
                a_menuid: $(this).find("#a_menuid").text(), // Adjust the index based on your table structure
                a_addaccess: $(this).find("#a_addaccess").is(':checked') ? 1 : 0,
                a_editaccess: $(this).find("#a_editaccess").is(':checked') ? 1 : 0,
                a_viewaccess: $(this).find("#a_viewaccess").is(':checked') ? 1 : 0,
                a_deleteaccess: $(this).find("#a_deleteaccess").is(':checked') ? 1 : 0,
                a_workflow: $(this).find("#a_workflow").is(':checked') ? 1 : 0,
            };


            menuDataArray.push(menuData);
        }
    });
    var isActive = $('#r_isactive').is(':checked') ? 1 : 0;
    var data = {
        r_id: $('#r_id').val(),
        r_rolename: $('#r_rolename').val(),
        r_description: $('#r_description').val(),
        r_module: $('#r_module').val(),
        Privilage: menuDataArray,
        r_isactive: isActive
    };
    $.ajax({
        type: 'POST',
        url: '/RoleMaster/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {

            window.location.href = "/RoleMaster/Index";
        },
        error: function (xhr, status, error) {
            // Handle errors
            // console.error(error);
        }
    });
}


/* For Edit Submit Button  */
function editLinkClick(r_id, r_rolename, r_description, r_module, r_isactive) {

    $.get("./RoleMaster/Edit",
        { r_id: r_id, r_rolename: r_rolename, r_description: r_description, r_module: r_module },
        function (data) {
            $('#r_id').text(r_id);
            //$('#r_rolename').text(r_rolename);
            $('#r_rolename').prop('readonly', true).val(r_rolename);
            $('#r_description').val(r_description);
            $('#r_module').val(r_module);
            if (r_isactive === "Active") {
                $('#r_isactive').prop('checked', true);
            } else {
                $('#r_isactive').prop('checked', false);
            }
            //window.location.href = '/RoleMaster/EditIndex';


        });


}


function ToggleSwitchCon(r_id, r_rolename, r_description, r_module, status) {
    var data = {


        r_id: r_id,
        r_rolename: r_rolename,
        r_description: r_description,
        r_module: r_module,
        r_isactive: status
    };


    $.ajax({
        type: 'POST',
        url: '/RoleMaster/Create', // Use the correct URL or endpoint
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


    var baseUrl = window.location.origin + '/RoleMaster';
    var statusValue = searchParams.get('status');


    statusValue = statusValue || '1'; // Set a default value if 'status' is null


    var newUrl = baseUrl + '/Excel' + (statusValue ? '?status=' + statusValue : '');


    window.location.href = newUrl;


}


function Pdf() {


    var currentUrl = window.location.href;


    var searchParams = new URL(currentUrl).searchParams;


    var baseUrl = window.location.origin + '/RoleMaster';
    var statusValue = searchParams.get('status');


    statusValue = statusValue || '1'; // Set a default value if 'status' is null


    var newUrl = baseUrl + '/Pdf' + (statusValue ? '?status=' + statusValue : '');


    window.location.href = newUrl;
}


function performAction() {
    var status = ($('#toggleStatus').is(':checked')) ? '1' : '0';
    window.location.href = "/RoleMaster/Index?status=" + status;


}