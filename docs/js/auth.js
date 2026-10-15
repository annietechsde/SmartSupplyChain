// =========================================
// SmartSupplyChain - Authentication
// =========================================

document.addEventListener("DOMContentLoaded", () => {

    const loginForm = document.getElementById("loginForm");

    // This script is also loaded on other pages,
    // so do nothing if the login form does not exist.
    if (!loginForm) {
        return;
    }

    loginForm.addEventListener("submit", async (event) => {

        event.preventDefault();

        const email = document.getElementById("email").value.trim();
        const password = document.getElementById("password").value;
        const loginMessage = document.getElementById("loginMessage");

        loginMessage.textContent = "";
        loginMessage.className = "form-message";

        try {

            const result = await apiRequest("/Auth/login", {
                method: "POST",
                body: JSON.stringify({
                    email: email,
                    password: password
                })
            });

            if (!result) {
                return;
            }

            // Save JWT token
            if (result.token) {
                saveToken(result.token);
            }

            // Save user information
            const user = {
                id: result.userId,
                name: `${result.firstName || ""} ${result.lastName || ""}`.trim(),
                email: result.email,
                role: result.role
            };

            saveUser(user);

            loginMessage.textContent = "Login successful!";
            loginMessage.classList.add("success");

            // Open dashboard
            setTimeout(() => {
                window.location.href = "dashboard.html";
            }, 500);

        } catch (error) {

            console.error("Login error:", error);

            loginMessage.textContent =
                error.message || "Invalid email or password.";

            loginMessage.classList.add("error");
        }

    });

});