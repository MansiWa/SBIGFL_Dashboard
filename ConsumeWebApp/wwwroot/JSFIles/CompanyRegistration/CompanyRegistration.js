function submitcompany(event) {
    event.preventDefault();
 if ($('#com_company_name').val() == '') {
     alert("Please enter company name!");
     return false;
 } else
     if ($('#CountryId').val() == '') {
     alert("Please select Country!");
     return false;
 } else
 if ($('#com_email').val() == '') {
     alert("Please enter emailId!");
     return false;
 }
 else
 if ($('#com_contact').val() == '') {
     alert("Please enter contact number!");
     return false;
 }
 else
 if ($('#com_staff_no').val() == '') {
     alert("Please enter staff number!");
     return false;
 }
 //checkemail($('#com_email').val());
 //validateMobile($('#com_contact').val());

 var data = {

     com_company_name: $('#com_company_name').val(),
     CountryId: $('#CountryId').val(),
     //CountryId: $('#CountryId').find(":selected").text(),
     com_email: $('#com_email').val(),
     com_contact: $('#com_contact').val(),
     com_staff_no: $('#com_staff_no').val()
 };
 $.ajax({
     type: 'POST',
     url: 'CompanyRegistration/Create', // Use the form's action attribute
     data: data, // Serialize the form data
     success: function (response) {
         if (response.outcomeDetail == "Company registred successfully!") {
             //alert("Company registred successfully!");
             //TempData["successMessage"] = response.outcomeDetail;
            // var name = response.name;
            // var comId = response.id;
            // var email = $('#com_email').val();
            // var country = $('#CountryId').find(":selected").text();
            // var country = $('#CountryId').val();
             //var url = '/Subscription?ComId=' + comId + '&com_name=' + name + '&com_email=' + email + '&country=' + country;
             var url = '/Login';

             window.location.href = url;
         }
         else {
             //alert("Error occured while processing!");
             window.location.reload();
             //TempData["errorMessage"] = "Error occured while processing!";
         }
        
     },
     error: function (xhr, status, error) {
         //TempData["errorMessage"] = error;
         //alert("Error occured while processing!");
         window.location.reload();
     }
 });
}
function submitsub(sm_id, package_name, sm_service, subAmount, subDiscount, final_Amount, Invoice, Quotation, Expence, cash_order, sub_duration) {

    var com_id = $('#com_id').val();
    var com_company_name = $('#com_company_name').val();
    var email = $('#com_email').val();
    var country = $('#country').val();
    var data = {
        com_id: com_id,
        sm_id: sm_id,
        com_company_name: com_company_name,
        package_name: package_name,
        sm_service: sm_service,
        subAmount: subAmount,
        subDiscount: subDiscount,
        final_Amount: final_Amount,
        Invoice: Invoice,
        Quotation: Quotation,
        Expence: Expence,
        cash_order: cash_order,
        sub_duration: sub_duration
    };
    $.ajax({
        type: 'POST',
        url: '/Subscription/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
            if (response.outcomeDetail == "Subscription inserted successfully!") {
                //var subId = response.sub_id;
                //var comId = response.com_id;
                //var com_name = response.com_name;
                //var com_email = email;
                //var countryname = country;
                //var final_Amt = final_Amount;
                //var url = '/PaymentGateway?ComId=' + comId + '&SubId=' + subId + '&com_company_name=' + com_name + '&com_company_email=' + com_email + '&final_Amt=' + final_Amt + '&country=' + countryname;
                var url = '/Login';
                window.location.href = url;
            }
            else {
                //alert("Error occured while processing!");
                window.location.reload();
                //TempData["errorMessage"] = "Error occured while processing!";
            }
            
        },
        error: function (xhr, status, error) {
            
            //alert("Error occured while processing!");
            window.location.reload();
        }
    });
}
function Submit() {
    var com_id = $('#com_id').val();
    var sub_id = $('#sub_id').val();
    var com_name = $('#com_company_name').val();
    if ($('#CountryId').val() == '') {
        alert("Please select Country!");
        return false;
    } else if ($('#Payment_Mode').val() == '') {
        alert("Please enter payment mode!");
        return false;
    } else if ($('#zipCode').val() == '') {
        alert("Please enter zip code!");
        return false;
    }
    //checkemail($('#com_email').val());Card
    var data = {
        com_id: com_id,
        sub_id: sub_id,
        com_company_name: com_name,
        com_email: $('#com_email').val(),
        Amount: $('#Amount').val(),
        Payment_Mode: $('#Payment_Mode').val(),
        Country: $('#CountryId').val()
    };
    $.ajax({
        type: 'POST',
        url: '/PaymentGateway/Create', // Use the form's action attribute
        data: data, // Serialize the form data
        success: function (response) {
           
            if (response.outcomeDetail =="Payment inserted successfully!") {
                var url = '/Home';
                window.location.href = url;
            }
             else {
            //alert("Error occured while processing!");
             window.location.reload();
            //TempData["errorMessage"] = "Error occured while processing!";
        }
        },
        error: function (xhr, status, error) {
            //alert("Error occured while processing!");
            window.location.reload();
        }
    });
}

function ValidateDetails() {
            if ($('#Contact_no').val() == '') {
                alert("Please enter username!");
                return false;
            } else
                if ($('#Contact_no').val() == '' && $('#com_password').val() == '') {
                    alert("Please enter username and password!");
                    return false;
                } else
                    if ($('#com_password').val() == '') {
                        alert("Please enter password!");
                        return false;
                    }

    //validateMobile($('#com_contact').val());
    var isChecked = $('#flexCheckChecked').prop('checked') ? 1 : 0;
    var data = {
        is_signIn: isChecked,
        Contact_no: $('#Contact_no').val(),
        com_password: $('#com_password').val(),
        com_code: $('#com_code').val(),
    };
    $.ajax({
        type: 'POST',
        url: 'Login/Create',
        data: data,
        success: function (response) {

            if (response == "Please enter valid credentials" || response.com_id == null || response == "Your subscription has been expired!") {
                alert(response);
                window.location.reload();
            } 
            else {
                var com_id = response.com_id;
                var url = 'DashBoard';//?com_id=' + com_id;// + '&Server_Key=' + name;
                window.location.href = url;
            }

        },
        error: function (xhr, status, error) {
            alert(error);
            window.location.reload();
            //TempData["errorMessage"] = error;
        }
    });
}
function forgot() {
    var data = {
        Contact_no: $('#username').val(),
    };
    $.ajax({
        type: 'Get',
        url: '/Login/ForgotPassword',
        data: data,
        success: function (response) {

            if (response == "Please enter valid credentials" || response == "Your subscription has been expired!" || response=="Invalid data!") {
                alert(response);
                window.location.reload();
            }
            else {
                alert("Password has been send to you email!")
                var url = '/Login';//?com_id=' + com_id;// + '&Server_Key=' + name;
                window.location.href = url;
            }

        },
        error: function (xhr, status, error) {
            alert("Please enter valid credentials!");
            window.location.reload();
            //TempData["errorMessage"] = error;
        }
    });
}



