// Redirect to login page if not logged in
let currentUser = null;
async function ensureUserLoggedIn(actionIfLoggedIn = null)
{
    let response = await fetch('/api/users/whoami',
        {
            headers:{'Authorization': "Bearer "+localStorage.getItem('api_token')}
        }
    );
    console.log(response);
    if(response.status == 200)
    {
        currentUser = await response.json();
        document.getElementById("user").innerHTML = currentUser.user.userName;
        if(actionIfLoggedIn != null)
            actionIfLoggedIn();
    }
    else
    {
        window.location.href = '/login.html';
    }
}
ensureUserLoggedIn();
