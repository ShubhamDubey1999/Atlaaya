'use strict';
function Login() {
    let user = $("#LoginForm").serializeObject();
    $.ajax({
        url: "/Login/Login",
        type: "POST",
        async: true,
        data: user,
        success: (resp) => {
            location.href = "/Home/Index"
        },
        error: (err) => {
            toastr.error(err.responseText);
        }
    })
}
function Register() {
    let isValid = true;
    if (isNullOrFalsy($("#RegUsername").val())) {
        isValid = false;
        $("#RegUsername").css("border", "2px solid red");
    } else {
        $("#RegUsername").css("border", "");
    }
    if (isNullOrFalsy($("#RegEmail").val())) {
        isValid = false;
        $("#RegEmail").css("border", "2px solid red");
    } else {
        $("#RegEmail").css("border", "");
    }
    if (isNullOrFalsy($("#RegPassword").val())) {
        isValid = false;
        $("#RegPassword").css("border", "2px solid red");
    } else {
        $("#RegPassword").css("border", "");
    }
    if (isNullOrFalsy($("#RegCnfPassword").val())) {
        isValid = false;
        $("#RegCnfPassword").css("border", "2px solid red");
    } else {
        $("#RegCnfPassword").css("border", "");
    }
    if ($("#RegCnfPassword").val() != $("#RegPassword").val()) {
        toastr.error("Password not match");
        isValid = false;
    }
    if (isValid) {
        let user = $("#RegisterForm").serializeObject();
        $.ajax({
            url: "/Login/Register",
            type: "POST",
            async: true,
            data: user,
            success: (resp) => {
                toastr.success(resp);
                location.reload();
            },
            error: (err) => {
                toastr.error(err.responseText);
            }
        })
    }
}
function EnquireNow() {
    let isValid = true;
    if (isNullOrFalsy($("#EnqFirstName").val())) {
        isValid = false;
        $("#EnqFirstName").css("border", "2px solid red");
    } else {
        $("#EnqFirstName").css("border", "");
    }

    let email = $("#EnqEmail").val();
    if (isNullOrFalsy(email) || !validateEmail(email)) {
        isValid = false;
        $("#EnqEmail").css("border", "2px solid red");
    } else {
        $("#EnqEmail").css("border", "");
    }

    let budget = $("#EnqBudget").val();
    if (isNullOrFalsy(budget) || !validateBudget(budget)) {
        isValid = false;
        $("#EnqBudget").css("border", "2px solid red");
    } else {
        $("#EnqBudget").css("border", "");
    }
    if (isValid) {
        var enquire = $("#EnquireForm").serializeObject();
        $.ajax({
            url: "/Login/Enquire",
            type: "POST",
            async: true,
            data: enquire,
            success: (resp) => {
                if (resp) {
                    toastr.success("We will contact you soon!!!");
                    $("#EnquireForm").modal('hide');
                }
            },
            error: (err) => {
                let error = "";
                $.each(err.responseJSON, function (index, value) {
                    error += value + "<br>";
                });
                toastr.error(error);
            }
        })
    }
}
function validateBudget(budget) {
    const budgetValue = parseFloat(budget);
    return !isNaN(budgetValue) && budgetValue >= 1;
}
function validateEmail(email) {
    const emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    return emailPattern.test(email);
}
$(document).ready(function () {
    $('#RegisterForm').on("keypress", function (e) {
        if (e.which === 13) {
            e.preventDefault();
            $("#btnRegister").click();
        }
    });

    $('#LoginForm').on("keypress", function (e) {
        if (e.which === 13) {
            e.preventDefault();
            $("#btnLogin").click();
        }
    });

    $('#EnquireForm').on("keypress", function (e) {
        if (e.which === 13) {
            e.preventDefault();
            $("#btnEnquire").click();
        }
    });
});
function isNullOrFalsy(value) {
    return !value;
}
function GetProjectDetails(Id) {
    location.href = "../Home/ProjectDetail?ProjectId=" + Id;
}
function AddTestimonial() {
    $.ajax({
        url: "../Testimonials/AddTestimonial",
        type: "get",
        async: true,
        success: (resp) => {
            $("#TestimonialModal").html(`
                <div class="modal fade" 
                    id="TestimonialModalCenter" 
                    tabindex="-1" 
                    role="dialog" 
                    aria-labelledby="TestimonialModalCenterTitle" 
                    aria-hidden="true">                    
                    <div class="modal-dialog modal-dialog-centered" role="document">
                        <div class="modal-content container">
                            ${resp}
                        </div>
                    </div>
                </div>
            `);
            $("#TestimonialModalCenter").modal('show');
        },
        error: (err) => {
            toastr.error(err.responseText);
        }
    });
}

function SubmitTestimonial() {
    let isValid = true;

}