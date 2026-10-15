// =========================================
// SmartSupplyChain - Tracking
// =========================================

document.addEventListener("DOMContentLoaded", () => {

    requireLogin();

    const trackingForm =
        document.getElementById("trackingForm");

    if (trackingForm) {
        trackingForm.addEventListener(
            "submit",
            trackShipment
        );
    }
});


// =========================================
// Track Shipment
// =========================================

async function trackShipment(event) {

    event.preventDefault();

    const trackingNumber =
        document.getElementById("trackingNumber")
            .value
            .trim();

    const message =
        document.getElementById("trackingMessage");

    const result =
        document.getElementById("trackingResult");

    message.textContent = "";
    message.className = "form-message";

    result.innerHTML = "";

    if (!trackingNumber) {

        message.textContent =
            "Please enter a tracking number.";

        message.classList.add("error");

        return;
    }

    try {

        const shipment =
            await apiRequest(
                `/Tracking/${encodeURIComponent(trackingNumber)}`
            );

        if (!shipment) {
            return;
        }

        message.textContent =
            "Shipment found.";

        message.classList.add("success");

        displayTrackingResult(shipment);

    } catch (error) {

        console.error(
            "Tracking error:",
            error
        );

        message.textContent =
            error.message ||
            "Shipment not found.";

        message.classList.add("error");
    }
}


// =========================================
// Display Tracking Result
// =========================================

function displayTrackingResult(shipment) {

    const result =
        document.getElementById("trackingResult");

    if (!result) {
        return;
    }

    result.innerHTML = `
        <div class="card">

            <h2>Shipment Details</h2>

            <p>
                <strong>Shipment ID:</strong>
                ${shipment.id ?? "-"}
            </p>

            <p>
                <strong>Tracking Number:</strong>
                ${shipment.trackingNumber ?? "-"}
            </p>

            <p>
                <strong>Order ID:</strong>
                ${shipment.orderId ?? "-"}
            </p>

            <p>
                <strong>Status:</strong>
                ${shipment.status ?? "-"}
            </p>

            <p>
                <strong>Origin:</strong>
                ${shipment.origin ?? "-"}
            </p>

            <p>
                <strong>Destination:</strong>
                ${shipment.destination ?? "-"}
            </p>

            <p>
                <strong>Estimated Delivery:</strong>
                ${
                    shipment.estimatedDeliveryDate
                        ? new Date(
                            shipment.estimatedDeliveryDate
                          ).toLocaleDateString()
                        : "-"
                }
            </p>

        </div>
    `;
}