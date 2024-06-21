setTimeout(function () {
    $('.alert').alert('close');
}, 7000);

//document.addEventListener('DOMContentLoaded', function () {
//    $('.column100').on('mouseover', function () {
//        var table1 = $(this).parent().parent().parent();
//        var table2 = $(this).parent().parent();
//        var verTable = $(table1).data('vertable') + "";
//        var column = $(this).data('column') + "";
//        $(table2).find("." + column).addClass('hov-column-' + verTable);
//        $(table1).find(".row100.head ." + column).addClass('hov-column-head-' + verTable);
//    });
//    $('.column100').on('mouseout', function () {
//        var table1 = $(this).parent().parent().parent();
//        var table2 = $(this).parent().parent();
//        var verTable = $(table1).data('vertable') + "";
//        var column = $(this).data('column') + "";
//        $(table2).find("." + column).removeClass('hov-column-' + verTable);
//        $(table1).find(".row100.head ." + column).removeClass('hov-column-head-' + verTable);
//    });
//}
//);

//document.addEventListener('DOMContentLoaded', function () {
//    // Array of table class names
//    const tableClasses = ['.table100.ver1', '.table100.ver2','.table100.ver3', '.table100.ver4', '.table100.ver5', '.table100.ver6'];

//    // Array of colors corresponding to each table
//    const colors = [
//        { column: 'rgba(177, 255, 193, 1)', row: 'rgba(177, 255, 193, 1)' }, // for ver1
//        { column: 'rgba(255, 227, 132, 0.8)', row: 'rgba(255, 227, 132, 0.8)' }, // for ver2
//        { column: 'rgba(210, 254, 255, 1)', row: 'rgba(210, 254, 255, 1)' }, // for ver3
//        { column: 'rgba(212, 227, 141, 0.96)', row: 'rgba(212, 227, 141, 0.96)' }, // for ver4
//        { column: 'rgba(230, 203, 246, 0.96)', row: 'rgba(230, 203, 246, 0.96)' },  // for ver5
//        { column: 'rgba(255, 200, 200, 1)', row: 'rgba(255, 200, 200, 1)' }  // for ver6
//    ];
//    const hoverColors = [
//        'rgba(122, 246, 117, 1)', // for ver1
//        'rgba(255, 193, 76, 0.99)', // for ver2
//        'rgba(65, 235, 255, 0.81)', // for ver3
//        'rgba(255, 209, 94, 0.96)', // for ver4
//        'rgba(217, 167, 246, 0.96)',  // for ver5
//        'rgba(255, 150, 150, 1)'  // for ver6
//    ];

//    // Loop through each table class
//    tableClasses.forEach((tableClass, index) => {
//        const table = document.querySelector(tableClass);

//        if (table) {
//            table.addEventListener('mouseover', function (event) {
//                const target = event.target;

//                // Check if the hovered element is a table cell
//                if (target.tagName.toLowerCase() === 'td') {
//                    const cellIndex = target.cellIndex + 1; // Add 1 to match the column index
//                    const rowIndex = target.parentNode.rowIndex;

//                    // Apply styles to the hovered column
//                    const columns = document.querySelectorAll(`${tableClass} .column100.column${cellIndex}`);
//                    columns.forEach(column => {
//                        column.style.backgroundColor = colors[index].column;
//                    });

//                    // Apply styles to the hovered row
//                    const row = target.parentNode;
//                    row.style.backgroundColor = colors[index].row;

//                    // Apply styles to the hovered cell
//                    target.style.backgroundColor = hoverColors[index];
//                    target.style.color = 'black';
//                }
//            });

//            table.addEventListener('mouseout', function (event) {
//                const target = event.target;

//                // Check if the hovered element is a table cell
//                if (target.tagName.toLowerCase() === 'td') {
//                    // Reset styles of the hovered column
//                    const columns = document.querySelectorAll(`${tableClass} .column100`);
//                    columns.forEach(column => {
//                        column.style.backgroundColor = '';
//                    });

//                    // Reset styles of the hovered row
//                    const row = target.parentNode;
//                    row.style.backgroundColor = '';

//                    // Reset styles of the hovered cell
//                    target.style.backgroundColor = '';
//                    target.style.color = '';
//                }
//            });
//        }
//    });
//});
