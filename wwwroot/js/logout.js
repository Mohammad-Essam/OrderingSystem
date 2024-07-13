function logout()
        {
    event.preventDefault();

    async function logout() {
        try {
            const response = await fetch("api/users/logout", {
                method: "DELETE",
                headers: { "authorization": `Bearer ${localStorage.getItem("api_token")}` },
            });

            if (response.ok) {
                toastr.success("Logged Out Successfully");
            } else {
                toastr.error("Error Logging Out");
            }
        } catch (err) {
            toastr.error("Error Logging Out");
        } finally {
            localStorage.removeItem("api_token");
            window.location.href = "login.html";
        }
    }

    logout();

        }