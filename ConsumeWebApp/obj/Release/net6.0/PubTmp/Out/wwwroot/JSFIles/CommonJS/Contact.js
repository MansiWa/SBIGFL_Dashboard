function validateMobileBycountry(mobilenumber) {

    var input = mobilenumber.value;
    var co_country_code = $("#co_country_code").val();//"+55";//

    if (input !== '' && co_country_code !== '') {
        var countryCodeDigits = co_country_code.replace(/\D/g, ''); // Extract digits from the country code

        // Define an object to map country codes to the expected length and patterns of mobile numbers
        var countryDetails = {
            '1': { length: 10, pattern: /^[2-9]\d{9}$/ },   // United States and Canada
            '44': { length: 10, pattern: /^[2-9]\d{9}$/ },  // United Kingdom
            '61': { length: 9, pattern: /^[2-9]\d{8}$/ },   // Australia
            '49': { length: 10, pattern: /^[1-9]\d{9}$/ },  // Germany
            '33': { length: 9, pattern: /^[1-9]\d{8}$/ },   // France
            '86': { length: 11, pattern: /^1[3456789]\d{9}$/ }, // China
            '91': { length: 10, pattern: /^[6-9]\d{9}$/ },  // India
            '81': { length: 10, pattern: /^[1-9]\d{9}$/ },  // Japan
            '55': { length: 9, pattern: /^[6-9]\d{8}$/ },   // Brazil
            '27': { length: 9, pattern: /^[6-9]\d{8}$/ }    // South Africa
            // Add more country codes, lengths, and patterns as needed
        };

        var countryDetailsForCode = countryDetails[countryCodeDigits];

        if (countryDetailsForCode &&
            input.length === countryDetailsForCode.length &&
            countryDetailsForCode.pattern.test(input)) {
            return true;
        } else {
            alert('Please enter a valid mobile number!');
            mobilenumber.value = '';
            return false;
        }
    }
}


function validatePhoneNumber(phoneNumber) {
    var co_country_code = $("#co_country_code").val();
    var input = phoneNumber.value;
    if (input !== '' && co_country_code !== '') {
        var countryDetails = {
            'US': { length: 10, pattern: /^[2-9]\d{9}$/ },    // United States and Canada
            'UK': { length: 10, pattern: /^[2-9]\d{9}$/ },    // United Kingdom
            'AU': { length: 9, pattern: /^[2-9]\d{8}$/ },     // Australia
            'DE': { length: 10, pattern: /^[1-9]\d{9}$/ },    // Germany
            'FR': { length: 9, pattern: /^[1-9]\d{8}$/ },     // France
            'CN': { length: 11, pattern: /^1[3456789]\d{9}$/ }, // China
            'IN': { length: 10, pattern: /^[6-9]\d{9}$/ },    // India
            'JP': { length: 10, pattern: /^[1-9]\d{9}$/ },    // Japan
            'BR': { length: 9, pattern: /^[6-9]\d{8}$/ },     // Brazil
            'ZA': { length: 9, pattern: /^[6-9]\d{8}$/ }      // South Africa

        };

        var countryDetailsForCode = countryDetails[co_country_code];

        if (countryDetailsForCode &&
            input.length === countryDetailsForCode.length &&
            countryDetailsForCode.pattern.test(input)) {
            return true;
        } else {
            alert('Please enter a valid mobile number for the selected country code!');
            mobilenumber.value = '';
            return false;
        }
    }
}

function validateGSTNumber(GSTNumber) {
    var countryCode = $("#co_country_code").val();
    var input = GSTNumber.value.toUpperCase();



    var patterns = {
        '1': /^\d{2}-\d{7}$/, // United States
        '44': /^(GB)?\d{3} \d{4} \d{2}(\d{3})?$/, // United Kingdom
        '61': /^\d{2}-\d{3}-\d{3}-\d{3}$/, // Australia
        '49': /^\d{2}-\d{9}$/, // Germany
        '33': /^\d{2} \d{3} \d{3} \d{3}$/,
        '91': /^\d{2}[A-Z]{5}\d{4}[A-Z]\d{1}[A-Z]\d{1}$/
        // Add more country patterns...
    };


    if (input !== '' && countryCode in patterns) {

        if (patterns[countryCode].test(input)) {

            return true;
        } else {
            alert('Please enter a valid GST number!');
            // Clear the input field if invalid
            // $("#GSTNumber").val('');
            return false;
        }
    }

}
//function validateGSTNumber(GSTNumber) {
//    var countryCode = $("#co_country_code").val();
//    var input = GSTNumber.value.toUpperCase();



//    var patterns = {
//        'US': /^\d{2}-\d{7}$/, // United States
//        'UK': /^(GB)?\d{3} \d{4} \d{2}(\d{3})?$/, // United Kingdom
//        'AU': /^\d{2}-\d{3}-\d{3}-\d{3}$/, // Australia
//        'DE': /^\d{2}-\d{9}$/, // Germany
//         'FR':/^\d{2} \d{3} \d{3} \d{3}$/,
//        'IN': /^\d{2}[A-Z]{5}\d{4}[A-Z]\d{1}[A-Z]\d{1}$/
//        // Add more country patterns...
//    };


//    if (input !== '' && countryCode in patterns) {

//        if (patterns[countryCode].test(input)) {

//            return true;
//        } else {
//            alert('Please enter a valid GST number!');
//            // Clear the input field if invalid
//            // $("#GSTNumber").val('');
//            return false;
//        }
//    }

//}
//function numberBycurrencyformat(number) {

//    var input = $('#'+number).text();
//    var CodeDigits = input.replace(/\D/g, '');
//    var cm_currency_format ="00.0.0.000" //$("#cm_currency_format").val();

//    if (!isNaN(CodeDigits) && cm_currency_format) {
//        var formattedNumber = parseFloat(CodeDigits).toLocaleString('en-IN', {
//            minimumFractionDigits: 0,
//            maximumFractionDigits: 0,
//            useGrouping: true
//        });

//        // Extract the placeholder character from the currency format
//        var placeholder = cm_currency_format.charAt(cm_currency_format.length - 4);

//        // Replace commas or periods based on the provided currency format
//        formattedNumber = formattedNumber.replace(/[.,]/g, placeholder);
//        $('#' + number).text(formattedNumber)
//        input = formattedNumber;
//        // If you want to return the formatted value
//        return formattedNumber;
//    }
//    return '';
//}
function checkMaxValue(inputElement, maxValue) {
    //if (parseInt(inputElement.value) > maxValue) {
    //    inputElement.value = maxValue; // Set the value to the maximum allowed
    //}
    let parsedValue = parseInt(inputElement.value);

    if (isNaN(parsedValue) && inputElement.value != '') {
        // If the value is not a valid number, set it to 0
        inputElement.value = 0;
    } else if (parsedValue >= maxValue) {
        // If the value is greater than the maximum allowed, set it to the maximum allowed
        inputElement.value = maxValue;
    }
}

function numberBycurrencyformat(elementId) {
    var input = $('#' + elementId).text();
    var cm_currency_format = "00.00.000" //$("#cm_currency_format").val();

    // Extract only numeric digits
    var digits = input.replace(/\D/g, '');

    // Pad with zeros to match the required format length
    while (digits.length < cm_currency_format.replace(/[^0-9]/g, '').length) {
        digits = '0' + digits;
    }

    // Format the number based on the specified format
    var formattedNumber = '';
    var formatIndex = 0;

    for (var i = 0; i < digits.length; i++) {
        if (cm_currency_format.charAt(formatIndex) === '0') {
            formattedNumber += digits.charAt(i);
        } else {
            formattedNumber += cm_currency_format.charAt(formatIndex);
            i--; // Don't advance the digits index for non-numeric placeholders
        }

        formatIndex++;
    }

    $('#' + elementId).text(formattedNumber)
    return formattedNumber;
}

function formatNumber(elementId) {
    var input = $('#' + elementId).text();
    var digits = input.toString().replace(/\D/g, '');
    var format = '00,00,000';
    var characterCount = format.length;
    // Trim leading zeros
    digits = digits.replace(/^0+/, '');

    // Pad with zeros to match the required format length
    //while (digits.length < format.replace(/[^0-9]/g, '').length) {
    //    digits = '0' + digits;
    //}

    // Format the number based on the specified format
    var formattedNumber = '';
    var fNumber = 0;
    var formatIndex = characterCount - 1;

    for (var i = digits.length; i > 0; i--) {
        if (format[formatIndex] === '0') {
            formattedNumber = (digits[i] * 10) + digits[i];
        } else {
            formattedNumber = (formattedNumber * 10) + format[formatIndex];
            // Don't advance the digits index for non-numeric placeholders
        }
        //i--;
        formatIndex--;
    }
    var usCurrency = new Intl.NumberFormat('en-US');
    var usFormatted = usCurrency.format(digits);
    $('#' + elementId).text(usFormatted)
    return formattedNumber;
}
function currencyf() {
    var name = new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' });
    string.Format(new CultureInfo("en-US"), "{0:C}", 1456.12155)      // $1,456.12
    // Currency format for Denmark
    string.Format(new CultureInfo("da-DK"), "{0:C}", 1456.12155)      // 1.456,12 kr.
    // Currency format for Great Britain
    string.Format(new CultureInfo("en-GB"), "{0:C}", 1456.12155)      // £1,456.12
    // Currency format for Denmark fo Japan
    string.Format(new CultureInfo("ja-JP"), "{0:C}", 1456.12155)
}
//function parseCurrency(currencyText, locale) {
//    // Replace symbols and separators specific to the en-GB currency format
//    var cleanedValue = parseFloat(currencyText.replace(new RegExp('[^\d' + new Intl.NumberFormat(locale).format(1111.1).charAt(1) + ']', 'g'), ''));
//    return isNaN(cleanedValue) ? null : cleanedValue;
//}


function parseDateString(dateString,id) {
    var dateParts = dateString.split('-');
    var year = parseInt(dateParts[0]);
    var month = parseInt(dateParts[1]) - 1; // Months are zero-based
    var day = parseInt(dateParts[2]);

    // Validate year to be four digits
    if (year < 1000 || year > 9999 || isNaN(year)) {
        alert("Invalid year format! Please enter a four-digit year.");
        $('#' + id).val('');
    }

    return new Date(year, month, day);
}