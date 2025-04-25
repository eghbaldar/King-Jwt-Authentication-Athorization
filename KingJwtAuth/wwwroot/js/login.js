async function login() {
    const userAgent = navigator.userAgent;
    const loginData = {
        Username: $("#txtUsername").val(),
        Password: $("#txtPassword").val(),
        UserAgent: userAgent
    };
    const response = await fetch('/auth/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(loginData),
    });
    if (response.ok) {
        const data = await response.json();
        const token = data.token;  // Assume the token is returned in the response

        // Store the token in localStorage
        // we can use storing token in localstorage beside cookie to use the pages who are protected by JWT-Bearer like /home/AllRolesBearer
        // localStorage.setItem("authToken", token);
        window.location.href = '/home/ProtectedPage';
    } else {
        alert('Login failed! Please check your credentials.');
    }
}