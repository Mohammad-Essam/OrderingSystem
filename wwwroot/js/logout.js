function logout()
        {
            event.preventDefault();
            fetch("api/users/logout",{
                method:"DELETE",
                headers:{"Authorization":`${localStorage.getItem("api_token")}`}},
            ).then(data => {
                toastr.success("Logged Out Successfully");
            }).catch(err => {
                toastr.error("Error Logging Out");
            })
            localStorage.removeItem("api_token");
            window.location.href = "login.html";
        }