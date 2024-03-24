document.addEventListener('DOMContentLoaded', function () {
    var myLabel = document.querySelector('#Patients_filter label');
    myLabel.textContent = 'test';
});



$(document).ready(function () {
   
    //handle modal
    $('body').delegate('.js-render-modal', 'click', function () {
        var btn = $(this);
        var modal = $('#Modal');

        modal.find('#ModalLabel').text(btn.data('title'));

        $.get({
            url: btn.data('url'),
            success: function (form) {
                modal.find('.modal-body').html(form);
                $.validator.unobtrusive.parse(modal);
            },
            error: function () {
                showErrorMessage();
            }
        });

        modal.modal('show');
    });


    //datatables on server side
    dataTable = $('#Patients').DataTable({
        serverSide: true,
        processing: true,
        stateSave: true,
        fnAdjustColumnSizing: true,
        language: {
            processing: '<div class="d-flex justify-content-center text-primary align-items-center dt-spinner"> <div class="spinner-border" role = "status" > <span class="visually-hidden"> Loading...</span></div > <span class="text-muted ps-2"> Loading...</span></div >'
        },
        search: {
            return: true
        },
        ajax: {
            url: '/Patients/GetPatients',
            type: 'POST'
        },
        order: [[0, 'asc']],
        columnDefs: [{
            targets: [0],
            visible: false,
            searchable: false
        }],
        columns: [
            { "data": "id", "name": "Id", "className": "d-none" },
            { "data": "fullName", "name": "FullName" },
            { "data": "age", "name": "Age" },
            {
                "name": "Email",
                "render": function (data, type, row) {
                    if (row.email == null)
                        return `<span class="badge rounded-pill text-bg-light NotAvailable-badge">No Email yet</span>`;
                    
                    return `<span class="badge rounded-pill text-bg-danger email-badge">${row.email}</span>`;
                },
                "orderable": false
            },
            {
                "name": "LastDiagnosis",
                "orderable": false,
                "render": function (data, type, row) {
                    if (row.lastDiagnosis == null)
                        return `<span class="badge rounded-pill text-bg-light NotAvailable-badge">No Tests yet</span>`;
                    if (row.lastDiagnosis == "Healthy")
                        return `<span class="badge rounded-pill text-bg-success healthy-badge">${row.lastDiagnosis}</span>`;
                    return `<span class="badge rounded-pill text-bg-danger parkinson-badge">${row.lastDiagnosis}</span>`;
                }

            },
            {
                "className": 'text-end',
                "orderable": false,
                "render": function (data, type, row) {
                    return `<!-- Example single danger button -->
                                            <div class="btn-group">
                                              <button type="button" class="btn btn-primary dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
                                                Action
                                              </button>
                                              <ul class="dropdown-menu">
                                                <li><a class="dropdown-item" href="/Tests/Create?patientId=${row.id}">Take a test</a></li>
                                                <li><a class="dropdown-item" href="/Tests/Index?patientId=${row.id}">All Tests</a></li>
                                                <li><a class="dropdown-item js-render-modal" href="javascript:;" data-title='Edit Personal Info' data-url='/Patients/Edit/${row.id}'>Edit personal info</a></li>
                                                <li><hr class="dropdown-divider"></li>
                                                <li><a class="dropdown-item js-delete" href="javascript:;" data-url='/Patients/Delete/${row.id}'  data-message="Are you sure you want to delete this paient?">Delete Paitent</a></li>
                                              </ul>
                                            </div>`;
                }
            }

        ]



    });





});