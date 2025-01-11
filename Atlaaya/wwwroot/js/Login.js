'use strict';
function Login() {
    debugger;
    let user = $("#LoginForm").serializeObject();
    $.ajax({
        url: "/Login/Login",
        type: "POST",
        async: true,
        data: user,
        success: (resp) => {
            location.href  = "/Home/Index"
        },
        error: (err) => {
            toastr.error(err.responseText);
        }
    })
}
function Register() {
    $.ajax({
        url: "/Login/Register",
        type: "POST",
        async: true,
        success: (resp) => {
            toastr.success(resp);
            location.reload();
        },
        error: (err) => {
            toastr.error(err.responseText);
        }
    })
}