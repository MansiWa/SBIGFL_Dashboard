
function cleardatatable() {
    $('#datatable1').DataTable().clear().destroy();
}

function myFunction() {

    //debugger;
    //trt{
    //    $('#datatable1').DataTable().clear().destroy();
    //}
    //catch () { $('#dataTable').DataTable(); }
    
    if (!$.fn.DataTable.isDataTable('#dataTable1')) {

        var table = $('#dataTable1').DataTable({
            // "paging": true, // Enable default pagination
            "search": false,
            "responsive": true,
            lengthChange: false, // Hide default DataTables length control
            pageLength: 2,
            
            "columnDefs": [
                { targets: [0, 1, -1], searchable: false },
                { "targets": 'no-filter', "orderable": false },
                { targets: '_all', searchable: true, visible: true }
            ],
            // Add other DataTable options as needed
            "drawCallback": function (settings) {
                // Update custom pagination controls
                var paginationContainer = $('#custom-pagination');
                var pageInfo = this.api().page.info();
                var currentPage = pageInfo.page + 1;
                var totalPages = pageInfo.pages;
                var maxPagesToShow = 2; // Define the maximum number of pages to show

                var paginationHTML = '';
                paginationHTML += '<ul class="pagination">';
                paginationHTML += '<li class="page-item ' + (currentPage === 1 ? 'disabled' : '') + '"><a class="page-link" href="#" data-page="' + (currentPage - 1) + '">&laquo;</a></li>';

                var startPage = Math.max(1, currentPage - Math.floor(maxPagesToShow / 2));
                var endPage = Math.min(startPage + maxPagesToShow - 1, totalPages);

                if (startPage > 1) {
                    paginationHTML += '<li class="page-item"><a class="page-link" href="#" data-page="1">1</a></li>';
                    if (startPage > 2) {
                        paginationHTML += '<li class="page-item disabled"><a class="page-link">...</a></li>';
                    }
                }

                for (var i = startPage; i <= endPage; i++) {
                    paginationHTML += '<li class="page-item ' + (currentPage === i ? 'active' : '') + '"><a class="page-link" href="#" data-page="' + i + '">' + i + '</a></li>';
                }

                if (endPage < totalPages) {
                    if (endPage < totalPages - 1) {
                        paginationHTML += '<li class="page-item disabled"><a class="page-link">...</a></li>';
                    }
                    paginationHTML += '<li class="page-item"><a class="page-link" href="#" data-page="' + totalPages + '">' + totalPages + '</a></li>';
                }

                paginationHTML += '<li class="page-item ' + (currentPage === totalPages ? 'disabled' : '') + '"><a class="page-link" href="#" data-page="' + (currentPage + 1) + '">&raquo</a></li>';
                paginationHTML += '</ul>';

                paginationContainer.html(paginationHTML);

                // Attach click event handlers to pagination links
                paginationContainer.find('.page-link').click(function (e) {
                    e.preventDefault();
                    var page = $(this).data('page');
                    table.page(page - 1).draw(false);
                });


                $('#table_length_select').change(function () {
                    var selectedValue = parseInt($(this).val());
                    table.page.len(selectedValue).draw();
                    $('html, body').animate({ scrollTop: $('#dataTable1').offset().top }, 'fast');
                });


            }
        });

        //remove search

        //$('#table_id_filter').remove();
        //$('.dataTables_length').css("display", "none");






        // Custom search functionality

        $('#custom-search').on('keyup', function () {
            table.search(this.value).draw();
        });
        

    }
}


function initializeDataTable(tableSelector, paginationContainerId) {
    if (!($.fn.DataTable.isDataTable(tableSelector))) {

        var table = $(tableSelector).DataTable({
            "search": false,
            "responsive": true,
            lengthChange: false,
            pageLength: 2,
            "columnDefs": [
                { targets: [0, 1, -1], searchable: false },
                { "targets": 'no-filter', "orderable": false },
                { targets: '_all', searchable: true, visible: true }
            ],
            "drawCallback": function (settings) {
                var paginationContainer = $('#' + paginationContainerId);
                var pageInfo = this.api().page.info();
                var currentPage = pageInfo.page + 1;
                var totalPages = pageInfo.pages;
                var maxPagesToShow = 2;

                var paginationHTML = '';
                paginationHTML += '<ul class="pagination">';
                paginationHTML += '<li class="page-item ' + (currentPage === 1 ? 'disabled' : '') + '"><a class="page-link" href="#" data-page="' + (currentPage - 1) + '">&laquo;</a></li>';

                var startPage = Math.max(1, currentPage - Math.floor(maxPagesToShow / 2));
                var endPage = Math.min(startPage + maxPagesToShow - 1, totalPages);

                if (startPage > 1) {
                    paginationHTML += '<li class="page-item"><a class="page-link" href="#" data-page="1">1</a></li>';
                    if (startPage > 2) {
                        paginationHTML += '<li class="page-item disabled"><a class="page-link">...</a></li>';
                    }
                }

                for (var i = startPage; i <= endPage; i++) {
                    paginationHTML += '<li class="page-item ' + (currentPage === i ? 'active' : '') + '"><a class="page-link" href="#" data-page="' + i + '">' + i + '</a></li>';
                }

                if (endPage < totalPages) {
                    if (endPage < totalPages - 1) {
                        paginationHTML += '<li class="page-item disabled"><a class="page-link">...</a></li>';
                    }
                    paginationHTML += '<li class="page-item"><a class="page-link" href="#" data-page="' + totalPages + '">' + totalPages + '</a></li>';
                }

                paginationHTML += '<li class="page-item ' + (currentPage === totalPages ? 'disabled' : '') + '"><a class="page-link" href="#" data-page="' + (currentPage + 1) + '">&raquo</a></li>';
                paginationHTML += '</ul>';

                paginationContainer.html(paginationHTML);



                paginationContainer.find('.page-link').click(function (e) {
                    e.preventDefault();
                    var page = $(this).data('page');
                    table.page(page - 1).draw(false);
                });
                $('#table_length_select2').change(function () {
                    var selectedValue = parseInt($(this).val());
                    table.page.len(selectedValue).draw();
                    $('html, body').animate({ scrollTop: $('#dataTable2').offset().top }, 'fast');
                });

                $('#table_length_select3').change(function () {
                    var selectedValue = parseInt($(this).val());
                    table.page.len(selectedValue).draw();
                    $('html, body').animate({ scrollTop: $('#dataTable3').offset().top }, 'fast');
                });
                // ... (rest of the code)

            }
        });

        $('#custom-search2').on('keyup', function () {
            table.search(this.value).draw();
        });
        $('#custom-search3').on('keyup', function () {
            table.search(this.value).draw();
        });
    }
}


