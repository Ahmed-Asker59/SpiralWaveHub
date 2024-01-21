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