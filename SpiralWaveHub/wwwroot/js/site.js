var dataTable;
function showSuccessMessage(message = "Saved Successfully!") {
    Swal.fire({
        icon: "success",
        title: "Success",
        text: message,
        confirmButtonColor: "#009ef7"
       
    });
}

function showErrorMessage(message = "Something Went Wrong!") {
    Swal.fire({
        icon: "error",
        title: "Error",
        text: message,
        confirmButtonColor: "#dc3545"
    });
}

function onModalSuccess() {
    showSuccessMessage();
    $('#Modal').modal('hide');
    dataTable.draw();
}

function deletePatient() {
    showSuccessMessage("Patient has been deleted successfully!");
    dataTable.draw();
}

function deleteTest(btn, result) {
    showSuccessMessage("Test has been deleted successfully!");
    btn.parents('.col-12').fadeOut();

    if (result === 0) {
        btn.parents('.row').addClass("d-none");
        $(".alert").removeClass("d-none");
    }
}

$(document).ready(function () {
    $('body').delegate('.js-delete', 'click', function () {
        var btn = $(this);

      
        bootbox.confirm({
            message: btn.data('message'),
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-danger'
                },
                cancel: {
                    label: 'No',
                    className: 'no-button'
                }
            },
            callback: function (result) {
                if (result) {
                    $.post({
                        url: btn.data('url'),
                        data: {
                            '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                        },
                        success: function (result) {
                            if (btn.hasClass("test"))
                                deleteTest(btn, result);
                            else
                                deletePatient();
                            
                        },
                        error: function () {
                            showErrorMessage();
                        }
                    });


                }
            }
        });
    });
});