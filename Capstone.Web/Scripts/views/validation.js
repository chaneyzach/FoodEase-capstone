
$(document).ready(function () {

    $("#registerForm").validate({

        //debug: false,
        rules: {
            FirstName: {
                required: true
            },
            LastName: {
                required: true
            },
            Email: {
                required: true,
                email: true
            },
            Username: {
                required: true,
                minlength: 6
            },
            Password: {
                required: true,
                minlength: 8
            },
            ConfirmPassword: {
                equalTo: "#Password"
            },
        },
        messages: {
            FirstName: {
                required: "First name is required"
            },
            LastName: {
                required: "Last name is required"
            },
            Email: {
                required: "Email address is required",
                email: "Please enter a valid email address"
            },
            Username: {
                required: "Username is required",
                minlength: "Username must be at least 6 characters long"
            },
            Password: {
                required: "Password is required",
                minlength: "Password must be at least 8 characters long"
            },
            ConfirmPassword: {
                equalTo: "Passwords do not match"
            }

        }
    });
    $("#loginForm").validate({

        //debug: false,
        rules: {            
            Username: {
                required: true
            },
            Password: {
                required: true
            }
        },
        messages: {            
            Username: {
                required: "Username is required",
            },
            Password: {
                required: "Password is required",
            }
        }
    });
});