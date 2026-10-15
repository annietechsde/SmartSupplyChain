// =========================================
// SmartSupplyChain - Customers
// =========================================

document.addEventListener("DOMContentLoaded", () => {

    requireLogin();

    const customerForm = document.getElementById("customerForm");

    if (customerForm) {
        customerForm.addEventListener("submit", createCustomer);
    }

    loadCustomers();
});


// =========================================
// Create Customer
// =========================================

async function createCustomer(event) {

    event.preventDefault();

    const message = document.getElementById("customerMessage");

    const firstName =
        document.getElementById("firstName").value.trim();

    const lastName =
        document.getElementById("lastName").value.trim();

    const email =
        document.getElementById("email").value.trim();

    const phone =
        document.getElementById("phone").value.trim();

    const address =
        document.getElementById("address").value.trim();

    const customerData = {
        name: `${firstName} ${lastName}`.trim(),
        email: email,
        phoneNumber: phone,
        address: address
    };

    message.textContent = "";
    message.className = "form-message";

    try {

        await apiRequest("/Customers", {
            method: "POST",
            body: JSON.stringify(customerData)
        });

        message.textContent =
            "Customer created successfully.";

        message.classList.add("success");

        document.getElementById("customerForm").reset();

        await loadCustomers();

    } catch (error) {

        console.error("Create customer error:", error);

        message.textContent =
            error.message || "Failed to create customer.";

        message.classList.add("error");
    }
}


// =========================================
// Load Customers
// =========================================

async function loadCustomers() {

    const tableBody =
        document.getElementById("customerTableBody");

    if (!tableBody) {
        return;
    }

    tableBody.innerHTML = `
        <tr>
            <td colspan="6">Loading customers...</td>
        </tr>
    `;

    try {

        const customers =
            await apiRequest("/Customers");

        tableBody.innerHTML = "";

        if (!Array.isArray(customers) ||
            customers.length === 0) {

            tableBody.innerHTML = `
                <tr>
                    <td colspan="6">
                        No customers found.
                    </td>
                </tr>
            `;

            return;
        }

        customers.forEach(customer => {

            const row =
                document.createElement("tr");

            row.innerHTML = `
                <td>${customer.id ?? "-"}</td>

                <td>${customer.name ?? "-"}</td>

                <td>${customer.email ?? "-"}</td>

                <td>${customer.phoneNumber ?? "-"}</td>

                <td>${customer.address ?? "-"}</td>

                <td>
                    <button
                        class="btn btn-danger"
                        onclick="deleteCustomer(${customer.id})">
                        Delete
                    </button>
                </td>
            `;

            tableBody.appendChild(row);
        });

    } catch (error) {

        console.error(
            "Load customers error:",
            error
        );

        tableBody.innerHTML = `
            <tr>
                <td colspan="6">
                    Failed to load customers.
                </td>
            </tr>
        `;
    }
}


// =========================================
// Delete Customer
// =========================================

async function deleteCustomer(customerId) {

    const confirmed = confirm(
        "Are you sure you want to delete this customer?"
    );

    if (!confirmed) {
        return;
    }

    try {

        await apiRequest(`/Customers/${customerId}`, {
            method: "DELETE"
        });

        await loadCustomers();

    } catch (error) {

        console.error(
            "Delete customer error:",
            error
        );

        alert(
            error.message ||
            "Failed to delete customer."
        );
    }
}