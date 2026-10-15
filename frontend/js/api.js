// =========================================
// SmartSupplyChain - API Helper
// =========================================

// Base URL of ASP.NET Core Web API
const API_BASE_URL = "http://localhost:5000/api";


// =========================================
// Token Management
// =========================================

function saveToken(token) {
    localStorage.setItem("token", token);
}

function getToken() {
    return localStorage.getItem("token");
}

function removeToken() {
    localStorage.removeItem("token");
}


// =========================================
// User Management
// =========================================

function saveUser(user) {
    localStorage.setItem("user", JSON.stringify(user));
}

function getUser() {
    const user = localStorage.getItem("user");

    if (!user) {
        return null;
    }

    try {
        return JSON.parse(user);
    } catch (error) {
        return null;
    }
}

function removeUser() {
    localStorage.removeItem("user");
}


// =========================================
// Authentication Check
// =========================================

function isLoggedIn() {
    return getToken() !== null;
}


// =========================================
// API Request Helper
// =========================================

async function apiRequest(endpoint, options = {}) {

    const token = getToken();

    const headers = {
        "Content-Type": "application/json",
        ...(options.headers || {})
    };

    // Add JWT token if available
    if (token) {
        headers["Authorization"] = `Bearer ${token}`;
    }

    try {

        const response = await fetch(
            `${API_BASE_URL}${endpoint}`,
            {
                ...options,
                headers: headers
            }
        );

        let data = null;

        const contentType = response.headers.get("content-type");

        if (contentType && contentType.includes("application/json")) {
            data = await response.json();
        } else {
            data = await response.text();
        }

        // Token is invalid or expired
        if (response.status === 401) {

            removeToken();
            removeUser();

            window.location.href = "login.html";

            return null;
        }

        if (!response.ok) {

            const message =
                typeof data === "object" && data?.message
                    ? data.message
                    : typeof data === "string" && data
                        ? data
                        : "Something went wrong.";

            throw new Error(message);
        }

        return data;

    } catch (error) {

        console.error("API Error:", error);

        throw error;
    }
}


// =========================================
// Logout
// =========================================

function logout() {

    removeToken();
    removeUser();

    window.location.href = "login.html";
}


// =========================================
// Protect Pages
// =========================================

function requireLogin() {

    if (!isLoggedIn()) {
        window.location.href = "login.html";
    }
}


// =========================================
// Logout Button
// =========================================

document.addEventListener("DOMContentLoaded", () => {

    const logoutButton = document.getElementById("logoutButton");

    if (logoutButton) {

        logoutButton.addEventListener("click", (event) => {

            event.preventDefault();

            logout();
        });
    }

});