function AddCompanyDetails() {
    window.location.href = '/CompanyDetails/EditIndex';
}
function GoBack() {
    window.location.href = '/CompanyDetails/Index';
}
function updatecompany() {
    if ($('#com_company_name').val() == '') {
        alert("Please enter company name!");
        return false;
    } else
        if ($('#com_company_name2').val() == '') {
            alert("Please enter company name 2!");
            return false;
        } else
            if ($('#com_owner_name').val() == '') {
                alert("Please owner name!");
                return false;
            } else
                if ($('#com_address').val() == '') {
                    alert("Please enter address!");
                    return false;
                }
                else
                    if ($('#com_email').val() == '') {
                        alert("Please enter email id!");
                        return false;
                    }
    var data = {
        com_id: $('#com_id').val(),
        com_company_name: $('#com_company_name').val(),
        com_company_name2: $('#com_company_name2').val(),
        com_owner_name: $('#com_owner_name').val(),
        com_address: $('#com_address').val(),
        com_gst_no: $('#com_gst_no').val(),
        com_email: $('#com_email').val(),
        com_contact: $('#com_contact').val(),
        com_website: $('#com_website').val(),
        com_bank_name: $('#com_bank_name').val(),
        com_branch: $('#com_branch').val(),
        com_acc_no: $('#com_acc_no').val(),
        com_ifsc: $('#com_ifsc').val()
    };
    $.ajax({
        type: 'POST',
        url: '/CompanyDetails/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            window.location.reload();
        },
        error: function (xhr, status, error) {
            console.log(error);
            window.location.reload();
        }
    });
}
function updateLogo(logo) {
    if ($('#com_company_logo').val() == '' & logo == 'logo') {
        alert("Please select logo!");
        return false;
    }
    if ($('#com_company_logo2').val() == '' & logo == 'logo2') {
        alert("Please select logo!");
        return false;
    }

    var formData = new FormData();
    formData.append('com_company_logo', $('#com_company_logo')[0].files[0]);
    formData.append('com_company_logo2', $('#com_company_logo2')[0].files[0]);
    formData.append('com_contact', $('#com_contact').text());
    formData.append('type', logo);
    $.ajax({
        type: 'POST',
        url: '/CompanyDetails/Update', // Use the form's action attribute
        data: formData, // Serialize the form data
        contentType: false,
        processData: false,
        success: function (response) {
            window.location.reload();
        },
        error: function (xhr, status, error) {
            console.log(error);
            window.location.reload();
        }
    });
}
function validateGST(gstNumber) {
    var input = gstNumber.value;
    const gstPattern = /^([0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}[Z]{1}[0-9A-Z]{1})$/;
    if (!gstPattern.test(input)) {
        alert('Please enter valid GST number!');
        gstNumber.value = '';
        return false;
    }
}
function validateWebsite(url) {
    var input = url.value;
    const urlPattern = /^(https?:\/\/)?(www\.)?([a-zA-Z0-9-]+\.){1,}[a-zA-Z]{2,}(:[0-9]{1,})?(\/[^\s]*)?$/;

    // Check if the provided URL matches the pattern
    if (urlPattern.test(input)) {
        alert('Please enter valid GST number!');
        url.value = '';
        return false;
    }
}