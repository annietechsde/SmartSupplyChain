// =========================================
// SmartSupplyChain - Orders
// =========================================

document.addEventListener("DOMContentLoaded", () => {

    requireLogin();

    const orderForm = document.getElementById("orderForm");

    if (orderForm) {
        orderForm.addEventListener("submit", createOrder);
    }

    loadOrders();
});


// =========================================
// Create Order
// =========================================

async function createOrder(event) {

    event.preventDefault();

    const message = document.getElementById("orderMessage");

    const orderData = {
        customerId: Number(
            document.getElementById("customerId").value
        ),

        expectedDeliveryDate:
            document.getElementById("expectedDeliveryDate").value,

        items: [
            {
                productName:
                    document.getElementById("productName").value.trim(),

                productCode:
                    document.getElementById("productCode").value.trim(),

                quantity: Number(
                    document.getElementById("quantity").value
                ),

                unitPrice: Number(
                    document.getElementById("unitPrice").value
                )
            }
        ]
    };

    message.textContent = "";
    message.className = "form-message";

    try {

        await apiRequest("/Orders", {
            method: "POST",
            body: JSON.stringify(orderData)
        });

        message.textContent = "Order created successfully.";
        message.classList.add("success");

        document.getElementById("orderForm").reset();

        await loadOrders();

    } catch (error) {

        console.error("Create order error:", error);

        message.textContent =
            error.message || "Failed to create order.";

        message.classList.add("error");
    }
}


// =========================================
// Load Orders
// =========================================

async function loadOrders() {

    const tableBody = document.getElementById("orderTableBody");

    if (!tableBody) {
        return;
    }

    tableBody.innerHTML = `
        <tr>
            <td colspan="5">Loading orders...</td>
        </tr>
    `;

    try {

        const orders = await apiRequest("/Orders");

        tableBody.innerHTML = "";

        if (!Array.isArray(orders) || orders.length === 0) {

            tableBody.innerHTML = `
                <tr>
                    <td colspan="5">
                        No orders found.
                    </td>
                </tr>
            `;

            return;
        }

        orders.forEach(order => {

            const row = document.createElement("tr");

            const itemsText = Array.isArray(order.items)
                ? order.items.map(item =>
                    `${item.productName} (${item.quantity} × ₹${item.unitPrice})`
                  ).join("<br>")
                : "-";

            row.innerHTML = `
                <td>${order.id ?? "-"}</td>

                <td>${order.customerId ?? "-"}</td>

                <td>
                    ${order.expectedDeliveryDate
                        ? new Date(
                            order.expectedDeliveryDate
                          ).toLocaleDateString()
                        : "-"}
                </td>

                <td>${itemsText}</td>

                <td>
                    <button
                        class="btn btn-danger"
                        onclick="deleteOrder(${order.id})">
                        Delete
                    </button>
                </td>
            `;

            tableBody.appendChild(row);
        });

    } catch (error) {

        console.error("Load orders error:", error);

        tableBody.innerHTML = `
            <tr>
                <td colspan="5">
                    Failed to load orders.
                </td>
            </tr>
        `;
    }
}


// =========================================
// Delete Order
// =========================================

async function deleteOrder(orderId) {

    const confirmed = confirm(
        "Are you sure you want to delete this order?"
    );

    if (!confirmed) {
        return;
    }

    try {

        await apiRequest(`/Orders/${orderId}`, {
            method: "DELETE"
        });

        await loadOrders();

    } catch (error) {

        console.error("Delete order error:", error);

        alert(
            error.message || "Failed to delete order."
        );
    }
}