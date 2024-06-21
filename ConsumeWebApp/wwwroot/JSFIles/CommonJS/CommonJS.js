function CancelData() {
    $("#createModal").modal("hide");
    window.location.reload()
}

function validateInput(inputElement) {
    // Regular expression to allow only characters and special characters, but not numbers
    var regex = /^[a-zA-Z0-9\s!#$%^&*()_,@\-./:;'"?<>|[\]{\}~`\\]*$/;

    var inputValue = inputElement.value;

    if (!regex.test(inputValue)) {
        alert("Invalid characters. Please use only characters, special characters and numbers.");
        inputElement.value = '';
        return false;
    }
}
function checkemail(email) {
    if (email.value != '') {
        var inputValue = email.value;
        var regex = /^[a-zA-Z0-9\.\_]+\@{1}[a-zA-Z0-9]+\.\w{2,4}$/;
        if (!regex.test(inputValue)) {
            alert('Invalid Email Id!');
            //$("#com_email").val('');
            email.value = '';
            return false;
        }
    }
}


function validateMobile(mobilenumber) {

    var input = mobilenumber.value;
    //var mobile_number = $("#com_contact").val();

    if (input != '') {
        //var mob_regex = /^(?:(?:\+|0{0,2})91(\s*[\-]\s*)?|[0]?)?[789]\d{9}$/;
        var mob_regex = /^[6-9][0-9]{9}$/;
        if (mob_regex.test(input)) {
            return true;
        } else {
            alert('Please enter valid contact no!');
            mobilenumber.value = '';
            return false;

        }
    }
}

function isValidNumberOfStaff(staffNumber) {
    // Regular expression for a positive integer
    var staffNumberRegex = /^[1-9]\d*$/; // Matches a positive integer

    var inputValue = staffNumber.value;

    if (!staffNumberRegex.test(inputValue)) {
        alert("Invalid number of staff. Please enter a positive integer.");
        staffNumber.value = '';
        return false;
    }
}
function isValidNumber(CNumber) {
    // Regular expression for a positive integer
    //var NumberRegex = /^$|^[0-9]\d*$/
    var NumberRegex = /^$|^[0-9]*\.?[0-9]+$/;
    ///^[0-9]+(\.[0-9]+)?$/;
    var inputValue = CNumber.value;

    if (!NumberRegex.test(inputValue)) {
        alert("Invalid number. please enter a positive integer.");
        CNumber.value = '';
        return false;
    }
}


function validateInputNumber(textBox) {

    var textBoxValue = parseFloat(textBox.value);


    if (textBoxValue < 0) {
        alert(" Please enter a non - negative value.!");
        textBox.value = 0;
        return false;

    }
    


}
//function notSpecialcharacters(inputValue) {

//    var regex = /^[a-zA-Z0-9\s]+$/;
//    if (!regex.test(inputValue)) {
//        alert('Please enter valid name !');

//        return false;
//    }

//}
function letterOnly(event) {

    var charCode = event.keyCode;
    if (event.ctrlKey || event.altKey) {

        event.preventDefault();
    }
    else {
        var key = event.keyCode;
        if (!((key == 8) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90))) {
            event.preventDefault();
        }
    }
}
function isNumber(event) {
    var num = event.keyCode;
    if ((num > 95 && num < 106) || (num > 36 && num < 41) || num == 9) {
        return;
    }
    if (event.shiftKey || event.ctrlKey || event.altKey) {
        event.preventDefault();
    } else if (num != 46 && num != 8) {
        if (isNaN(parseInt(String.fromCharCode(event.which)))) {
            event.preventDefault();
        }
    }
}
function validateUpi(upi) {
    // UPI pattern validation
    //const upiPattern = /^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/;
    var upiPattern = /^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+$/;
    var inputValue = upi.value;
    // Check if the UPI matches the pattern
    if (!upiPattern.test(inputValue)) {
        alert('Invalid Upi Id!');
        upi.value = '';
        return false;
    }
}
//function DecimalNumber(event) {
//    var num = event.keyCode;

//    if (
//        (num >= 48 && num <= 57) ||        // Numbers
//        (num >= 96 && num <= 105) ||       // Numpad numbers
//        num == 8 ||                         // Backspace
//        num == 9 ||                         // Tab
//        num == 37 || num == 39 ||           // Left and right arrows
//        num == 46 ||                        // Delete
//        (num == 190 && event.target.value.indexOf('.') === -1) // Allow decimal point if not already present
//    ) {
//        return;  // Allow the key
//    }

//    // Prevent input if shift, ctrl, or alt key is pressed
//    if (event.shiftKey || event.ctrlKey || event.altKey) {
//        event.preventDefault();
//    } else {
//        event.preventDefault();
//    }
//}
function DecimalNumber(event) {
    var key = event.key;

    if (
        (key >= '0' && key <= '9') ||           // Numbers
        key == 'Backspace' ||                   // Backspace
        key == 'Tab' ||                         // Tab
        key == 'ArrowLeft' || key == 'ArrowRight' || // Left and right arrows
        key == 'Delete' ||                      // Delete
        (key == '.' && event.target.value.indexOf('.') === -1) // Allow decimal point if not already present
    ) {
        return;  // Allow the key
    }

    // Prevent input if shift, ctrl, or alt key is pressed
    if (event.shiftKey || event.ctrlKey || event.altKey) {
        event.preventDefault();
    } else {
        event.preventDefault();
    }
}
function validateBirthdate(dateString) {
    var dateParts = dateString.split('-');
    var day = parseInt(dateParts[2]);
    var month = parseInt(dateParts[1]) - 1; // Months are zero-based
    var year = parseInt(dateParts[0]);

    var inputDate = new Date(year, month, day);
    var today = new Date();


    // Check if the birthdate is a future date

    // Calculate the age based on the birthdate
    var age = today.getFullYear() - inputDate.getFullYear();
    var monthDiff = today.getMonth() - inputDate.getMonth();
    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < inputDate.getDate())) {
        age--;
    }

    if (age < 18) {
        alert("Please select valid date!");
        $("#c_dob").val('');
        $("#st_dob").val('');
    }

    return true;
}
function addCommas(input) {
    var value = input.value.replace(/\D/g, ''); // Remove non-numeric characters
    var parts = [];
    while (value.length > 0) {
        parts.unshift(value.slice(-3));
        value = value.slice(0, -3);
    }
    //Txt_opening_balance.Text = parts.join(',');
    input.value = parts.join(',');
}
function numberWithCommas(number) {
    return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

function parseCurrency(currencyText) {
    var locale = $('#cm_currency_format').val();
    //var formattedChar = new Intl.NumberFormat(locale).format(1111.1).charAt(1);

    // Replace symbols and separators specific to the given currency format
    //var cleanedValue = parseFloat(currencyText.replace(new RegExp('[^\d' + formattedChar + ']', 'g'), '').replace(formattedChar, '.'));

    //return isNaN(cleanedValue) ? null : cleanedValue;
    //var cleanedValue = parseFloat(currencyText.replace(/[^\d.,]|,(?=[^,]*$)|\.(?=[^.]*$)/g, '').replace(',', '.'));
    var cleanedValue = currencyText.replaceAll(/[^\d.,]/g, '');

    // Replace comma with dot as the decimal separator
    cleanedValue = cleanedValue.replaceAll(',', '');

    // Parse the cleaned string as a float
    var parsedValue = parseFloat(cleanedValue);
    //var cleanedValue = parseFloat(normalizedValue);
    return isNaN(parsedValue) ? null : parsedValue;
}
var locales = [
    { code: 'en-US', style: 'currency', currency: 'USD' },   // United States Dollar
    { code: 'en-GB', style: 'currency', currency: 'GBP' },   // British Pound Sterling
    { code: 'en-IN', style: 'currency', currency: 'INR' },   // Indian Rupee
    { code: 'es-ES', style: 'currency', currency: 'EUR' },   // Euro (Spain)
    // Add more locales as needed
];
function formatAsCurrency(digits) {
    var locale = $('#cm_currency_format').val();
    var usCurrency = new Intl.NumberFormat(locale);
    var usFormatted = usCurrency.format(digits);
    var selectedLocale = locales.find(function (item) {
        return item.code === locale;
    });
    var seFormatted = digits.toLocaleString(locale, { style: 'currency', currency: selectedLocale.currency });
    return seFormatted;
}

function formatNumberWithSuffix(value, culture) {
    var suffixes = ["", "K", "M", "B", "T"];

    var magnitude = Math.floor(Math.log10(Math.abs(value)));

    if (magnitude >= 3) {
        var suffixIndex = Math.floor((magnitude - 1) / 3);
        var scaledValue = value / Math.pow(10, suffixIndex * 3);
        return scaledValue.toLocaleString(culture, { maximumFractionDigits: 2 }) + ' ' + suffixes[suffixIndex];
    } else {
        return value.toLocaleString(culture, { maximumFractionDigits: 2 });
    }
}
function validateNumchar(event) {
    var key = event.keyCode;
    var keyk = event.key;

    if (event.ctrlKey || event.altKey) {
        event.preventDefault();
    } else {
        if (!((key == 8) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90) || keyk === "0")) {
            if (event.shiftKey ||
                (key < 48 && key != 32 && key != 48 && key != 96) || // Disallow characters before '0' and '0' from numpad
                (key > 57 && key < 65) ||
                (key > 90 && key < 97) ||
                key > 122) {
                event.preventDefault();
            }
        }
    }
}


function getmonth(date) {
    const d = new Date(date);
    var month = d.getMonth();
    if (month == "0")  month = "10";
    else if (month == "1") month = "11";
    else if (month == "2") month = "12";
    else if (month == "3") month = "1";
    else if (month == "4") month = "2";
    else if (month == "5") month = "3";
    else if (month == "6") month = "4";
    else if (month == "7") month = "5";
    else if (month == "8") month = "6";
    else if (month == "9") month = "7";
    else if (month == "10") month = "8";
    else if (month == "11") month = "9";

}