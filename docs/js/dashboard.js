// =========================================
// SmartSupplyChain - Dashboard
// =========================================

document.addEventListener("DOMContentLoaded", async () => {

    // Make sure the user is logged in
    requireLogin();

    const welcomeMessage = document.getElementById("welcomeMessage");

    // Get logged-in user
    const user = getUser();

    if (!user) {
        return;
    }

    // Display logged-in user's name and role
    if (welcomeMessage) {

        const displayName =
            user.name ||
            user.email ||
            "User";

        welcomeMessage.textContent =
            `Welcome, ${displayName}! (${user.role || "User"})`;
    }

    // Apply role-based dashboard UI
    applyRoleBasedUI(user.role);

    // Load dashboard statistics
    await loadDashboardStatistics(user.role);
});


// =========================================
// Role-Based UI
// =========================================

function applyRoleBasedUI(role) {

    const normalizedRole = (role || "").toLowerCase();

    // Admin can access everything
    if (normalizedRole === "admin") {
        return;
    }

    // Customer restrictions
    if (normalizedRole === "customer") {

        // Hide Customers
        hideLinksByText([
            "Customers",
            "View Customers",
            "Manage Customers"
        ]);

        // Hide Shipments
        hideLinksByText([
            "Shipments",
            "View Shipments",
            "Manage Shipments"
        ]);

        // Hide Reports
        hideLinksByText([
            "Reports",
            "View Reports"
        ]);

        // Keep these available for Customer:
        // Orders
        // View Orders
        // Manage Orders
        // Tracking
        // Track Shipment
    }
}


// =========================================
// Hide Links / Buttons by Text
// =========================================

function hideLinksByText(texts) {

    const elements = document.querySelectorAll("a, button");

    elements.forEach(element => {

        const elementText =
            element.textContent.trim().toLowerCase();

        const shouldHide = texts.some(
            text => elementText === text.toLowerCase()
        );

        if (shouldHide) {
            element.style.display = "none";
        }
    });
}


// =========================================
// Load Dashboard Statistics
// =========================================

async function loadDashboardStatistics(role) {

    const customerCount =
        document.getElementById("customerCount");

    const orderCount =
        document.getElementById("orderCount");

    const shipmentCount =
        document.getElementById("shipmentCount");

    const normalizedRole =
        (role || "").toLowerCase();


    // =========================================
    // Admin Statistics
    // =========================================

    if (normalizedRole === "admin") {

        try {

            if (customerCount) {

                const customers =
                    await apiRequest("/Customers");

                if (Array.isArray(customers)) {
                    customerCount.textContent =
                        customers.length;
                } else {
                    customerCount.textContent = "0";
                }
            }

        } catch (error) {

            console.error(
                "Error loading customers:",
                error
            );

            if (customerCount) {
                customerCount.textContent = "-";
            }
        }


        try {

            if (orderCount) {

                const orders =
                    await apiRequest("/Orders");

                if (Array.isArray(orders)) {
                    orderCount.textContent =
                        orders.length;
                } else {
                    orderCount.textContent = "0";
                }
            }

        } catch (error) {

            console.error(
                "Error loading orders:",
                error
            );

            if (orderCount) {
                orderCount.textContent = "-";
            }
        }


        try {

            if (shipmentCount) {

                const shipments =
                    await apiRequest("/Shipments");

                if (Array.isArray(shipments)) {
                    shipmentCount.textContent =
                        shipments.length;
                } else {
                    shipmentCount.textContent = "0";
                }
            }

        } catch (error) {

            console.error(
                "Error loading shipments:",
                error
            );

            if (shipmentCount) {
                shipmentCount.textContent = "-";
            }
        }

        return;
    }


    // =========================================
    // Customer Statistics
    // =========================================

    if (normalizedRole === "customer") {

        // Customer should not see all customers
        if (customerCount) {
            customerCount.textContent = "-";
        }

        // Load orders
        try {

            if (orderCount) {

                const orders =
                    await apiRequest("/Orders");

                if (Array.isArray(orders)) {
                    orderCount.textContent =
                        orders.length;
                } else {
                    orderCount.textContent = "0";
                }
            }

        } catch (error) {

            console.error(
                "Error loading customer orders:",
                error
            );

            if (orderCount) {
                orderCount.textContent = "-";
            }
        }

        // Customer should not see all shipments
        if (shipmentCount) {
            shipmentCount.textContent = "-";
        }

        return;
    }
}