// =========================================
// SmartSupplyChain - Shipments
// =========================================

document.addEventListener("DOMContentLoaded", () => {

    requireLogin();

    const shipmentForm = document.getElementById("shipmentForm");

    if (shipmentForm) {
        shipmentForm.addEventListener("submit", createShipment);
    }

    loadShipments();
});


// =========================================
// Create Shipment
// =========================================

async function createShipment(event) {

    event.preventDefault();

    const message = document.getElementById("shipmentMessage");

    const shipmentData = {
        orderId: Number(
            document.getElementById("orderId").value
        ),

        warehouseId: Number(
            document.getElementById("warehouseId").value
        ),

        origin:
            document.getElementById("origin").value.trim(),

        destination:
            document.getElementById("destination").value.trim(),

        estimatedDeliveryDate:
            document.getElementById("estimatedDeliveryDate").value,

        weight: Number(
            document.getElementById("weight").value
        )
    };

    message.textContent = "";
    message.className = "form-message";

    try {

        await apiRequest("/Shipments", {
            method: "POST",
            body: JSON.stringify(shipmentData)
        });

        message.textContent =
            "Shipment created successfully.";

        message.classList.add("success");

        document.getElementById("shipmentForm").reset();

        await loadShipments();

    } catch (error) {

        console.error("Create shipment error:", error);

        message.textContent =
            error.message || "Failed to create shipment.";

        message.classList.add("error");
    }
}


// =========================================
// Load Shipments
// =========================================

async function loadShipments() {

    const tableBody =
        document.getElementById("shipmentTableBody");

    if (!tableBody) {
        return;
    }

    tableBody.innerHTML = `
        <tr>
            <td colspan="8">
                Loading shipments...
            </td>
        </tr>
    `;

    try {

        const shipments =
            await apiRequest("/Shipments");

        tableBody.innerHTML = "";

        if (!Array.isArray(shipments) || shipments.length === 0) {

            tableBody.innerHTML = `
                <tr>
                    <td colspan="8">
                        No shipments found.
                    </td>
                </tr>
            `;

            return;
        }

        shipments.forEach(shipment => {

            const row = document.createElement("tr");

            row.innerHTML = `
                <td>${shipment.id ?? "-"}</td>

                <td>${shipment.orderId ?? "-"}</td>

                <td>${shipment.warehouseId ?? "-"}</td>

                <td>${shipment.origin ?? "-"}</td>

                <td>${shipment.destination ?? "-"}</td>

                <td>${shipment.status ?? "-"}</td>

                <td>
                    ${
                        shipment.estimatedDeliveryDate
                            ? new Date(
                                shipment.estimatedDeliveryDate
                              ).toLocaleDateString()
                            : "-"
                    }
                </td>

                <td>
                    <button
                        class="btn btn-danger"
                        onclick="deleteShipment(${shipment.id})">
                        Delete
                    </button>
                </td>
            `;

            tableBody.appendChild(row);
        });

    } catch (error) {

        console.error(
            "Load shipments error:",
            error
        );

        tableBody.innerHTML = `
            <tr>
                <td colspan="8">
                    Failed to load shipments.
                </td>
            </tr>
        `;
    }
}


// =========================================
// Delete Shipment
// =========================================

async function deleteShipment(shipmentId) {

    const confirmed = confirm(
        "Are you sure you want to delete this shipment?"
    );

    if (!confirmed) {
        return;
    }

    try {

        await apiRequest(`/Shipments/${shipmentId}`, {
            method: "DELETE"
        });

        await loadShipments();

    } catch (error) {

        console.error(
            "Delete shipment error:",
            error
        );

        alert(
            error.message ||
            "Failed to delete shipment."
        );
    }
}