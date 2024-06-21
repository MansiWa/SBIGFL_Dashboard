function AddBankOperation() {
    window.location.href = '/BankOperation/EditIndex';
}
function GoBack() {
    window.location.href = '/BankOperation/Index';
} function ClrData() {
    $('#r_file').val('');
    $('#r_name').val('');
    $('#r_date').val('');
    $('#r_remark').val('');
    $('#r_type').val('');
    $('#fileContent').text('');

}

function submitfile() {


    if ($('#r_name').val() == '') {
        alert("Please enter name!");
        return false;
    }
    else if ($('#r_type').val() == '') {
        alert("Please select report type!");
        return false;
    }
    else if ($('#r_date').val() == '') {
        alert("Please select date!");
        return false;
    }

    var formData = new FormData();
    formData.append('r_file', $('#r_file')[0].files[0]);
    formData.append('r_name', $('#r_name').val());
    formData.append('r_date', $('#r_date').val());
    formData.append('r_type', $('#r_type').val());
    formData.append('r_remark', $('#r_remark').val());

    $.ajax({
        type: 'POST',
        url: '/UploadReport/Insert',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {

            window.location.reload();
        },
        error: function (xhr, status, error) {
            console.log(error);
            //window.location.reload();
        }
    });

}

function viewFile() {

    var fileInput = document.getElementById('r_file');

    var file = fileInput.files[0];
    if (file) {
        var allowedExtensions = [".xls", ".xlsx", ".xlsm", ".csv"];

        if (allowedExtensions.some(ext => file.name.toLowerCase().endsWith(ext))) {
            var fileReader = new FileReader();
            fileReader.onload = function (event) {
                var fileContentElement = document.getElementById('fileContent');
                fileContentElement.innerHTML = '';

                if (file.name.toLowerCase().endsWith('.csv')) {
                    // For CSV files, you can display the content in a textarea
                    var textarea = document.createElement('textarea');
                    textarea.textContent = event.target.result;
                    textarea.rows = 10; // Adjust the number of rows as needed
                    textarea.cols = 50; // Adjust the number of columns as needed
                    fileContentElement.appendChild(textarea);
                } else {
                    // For Excel files, you can use SheetJS to parse and display the content
                    var workbook = XLSX.read(event.target.result, { type: 'binary' });
                    var sheetName = workbook.SheetNames[0]; // Assuming only one sheet
                    var worksheet = workbook.Sheets[sheetName];
                    var htmlContent = XLSX.utils.sheet_to_html(worksheet);
                    fileContentElement.innerHTML = htmlContent;
                }
            };
            if (file.name.toLowerCase().endsWith('.csv')) {
                fileReader.readAsText(file);
            } else {
                fileReader.readAsBinaryString(file);
            }
        }
        else {
            alert('Invalid file extension. Supported extensions are: ' + allowedExtensions.join(', '));
            $('#fileContent').text('');
            $('#r_file').val('');

        }
    }
    else {
        alert('Please select a file before clicking "View File"!');
    }
}
function FileUploadimage() {
    $("input[id='imagefile']").click();
}

$("#imagefile").change(function () {
    readURL(this);
});
function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        var file = input.files[0];

        // File format validation
        var fileExtension = file.name.split('.').pop().toLowerCase();
        var allowedExtensions = ['xls', 'xlsx','xlsm', 'csv'];

        if (allowedExtensions.indexOf(fileExtension) === -1) {
            alert('Invalid file format. Please select a proper image file.');
            $('#r_file').val('');
            return false;
        }

        reader.onload = function (e) {
            $('#r_file').attr('src', e.target.result);
        };

        reader.readAsDataURL(file);
    }
}
