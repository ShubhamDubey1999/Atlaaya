'use strict';
$(document).ready(function () {
    $('#myTable').DataTable();
});
function AdminCreateUser(id) {
    $.ajax({
        url: "/User/Create?Id=" + id,
        type: "Get",
        async: true,
        success: (res) => {
            console.log(res)
            $("#createUserDiv").html(res);
            $("#CreateUserModalCenter").modal('show');
        },
        error: (err) => {
            toastr.error(err.responseText);
        }
    })
}