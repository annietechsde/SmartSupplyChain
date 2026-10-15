# SmartSupplyChain

SmartSupplyChain is a web-based logistics and supply chain management system developed using ASP.NET Core Web API, Entity Framework Core, and SQL Server.

The main purpose of this project is to manage supply chain activities through a single platform. It includes modules for customers, orders, shipments, warehouses, delivery agents, shipment tracking, payments, user authentication, and reports.

## Project Objectives

- To make logistics and supply chain management easier.
- To manage customer, order, and shipment information in one place.
- To provide secure access to the application.
- To maintain shipment and delivery records.
- To store application data in SQL Server.
- To connect the frontend with the backend through REST APIs.
- To test the application and improve its reliability.

## Technologies Used

**Backend**
- C#
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- Repository Pattern
- Unit of Work Pattern
- JWT Authentication
- REST APIs

**Database**
- Microsoft SQL Server
- SQL Server Management Studio (SSMS)

**Frontend**
- HTML5
- CSS3
- JavaScript

**Testing and API Documentation**
- Swagger / OpenAPI
- xUnit
- Moq

**Development Tools**
- Visual Studio Code
- Git
- GitHub

## System Architecture

The project follows a layered architecture. Each layer has a specific responsibility, which makes the application easier to manage and maintain.

```text
Frontend (HTML, CSS, JavaScript)
              |
              v
    ASP.NET Core Web API
              |
              v
         Controllers
              |
              v
    Application / Services
              |
              v
         Repositories
              |
              v
     Entity Framework Core
              |
              v
         SQL Server
```

### Architecture Components

- **Frontend:** Provides the interface through which users interact with the application.
- **Controllers:** Handle incoming API requests.
- **Application / Services:** Manage the main application logic.
- **Repositories:** Handle data access operations.
- **Entity Framework Core:** Connects the application with the database.
- **SQL Server:** Stores the application data.

## Main Features

### 1. User Authentication and Authorization
- Allows users to log in.
- Uses JWT authentication to secure protected API endpoints.
- Supports role-based access control.

### 2. Customer Management
- Manages customer information.
- Maintains customer address details.

### 3. Order Management
- Creates and manages orders.
- Maintains order items and order details.
- Connects orders with customers and delivery addresses.

### 4. Shipment Management
- Manages shipments related to orders.
- Maintains shipment information and status.

### 5. Warehouse Management
- Maintains warehouse information for logistics operations.

### 6. Delivery Management
- Manages delivery agent information.
- Maintains delivery assignments.

### 7. Shipment Tracking
- Maintains tracking information to monitor shipment progress.

### 8. Payment Management
- Maintains payment records related to orders.

### 9. Reports
- Provides a reports module for viewing supply chain information.

### 10. API Documentation
- Uses Swagger to view and test API endpoints.

### 11. Automated Testing
- Uses xUnit and Moq to test application functionality.

## Project Structure

The project is divided into different folders and projects to keep the code organized.

```text
SmartSupplyChain/
|
|-- src/
|   |-- SmartSupplyChain.API/
|   |   |-- Controllers/
|   |   |-- Middleware/
|   |   |-- Extensions/
|   |
|   |-- SmartSupplyChain.Core/
|   |-- SmartSupplyChain.Application/
|   |-- SmartSupplyChain.Infrastructure/
|
|-- tests/
|   |-- SmartSupplyChain.Tests/
|
|-- frontend/
|   |-- js/
|   |-- css/
|   |-- login.html
|
|-- SmartSupplyChain.sln
|-- README.md
```

## Prerequisites

The following software is required to run the project:

- .NET 8 SDK
- Microsoft SQL Server
- SQL Server Management Studio (SSMS)
- Visual Studio Code
- A modern web browser

## Database Configuration

The project uses Microsoft SQL Server with the database name `SmartSupplyChainDb`.

Before running the application:

1. Make sure the SQL Server service is running.
2. Configure the database connection string in the application settings.
3. Ensure that the database and required tables are available.
4. Apply the existing Entity Framework Core migrations if required.

Database passwords and JWT secrets should be kept private and should not be uploaded to a public repository.

## How to Run the Project

### 1. Open the Project

Open the SmartSupplyChain project folder in Visual Studio Code.

### 2. Restore Dependencies

Open the terminal in the project root and run:

```bash
dotnet restore
```

### 3. Build the Project

```bash
dotnet build
```

### 4. Run the Backend

Run the following command from the project root:

```bash
dotnet run --project ./src/SmartSupplyChain.API/
```

The API uses the URL configured in the application settings. In the current local setup, the base URL is:

`http://localhost:5000`

### 5. Run the Frontend

1. Open the `frontend` folder in Visual Studio Code.
2. Start the frontend using a local development server, such as Five Server.
3. Open the local URL shown by the server.
4. Log in using a valid application account.

The backend API must be running for the frontend to access application data.

## API Documentation

Swagger is used to view and test the available API endpoints.

Open the following URL after starting the backend:

`http://localhost:5000/swagger`

The API base URL is:

`http://localhost:5000`

These URLs may change if the application configuration is updated.

## Authentication and Authorization

The application uses JWT tokens for authentication and protects selected API endpoints.

To access a protected endpoint:

1. Log in using a valid account.
2. Obtain the access token returned by the API.
3. Add the token to the request's Authorization header.

```text
Authorization: Bearer <your-access-token>
```

Access to certain features depends on the user's assigned role.

## Testing

The project includes automated tests using xUnit and Moq.

To run the tests, execute:

```bash
dotnet test
```

To build the solution, execute:

```bash
dotnet build
```

## Future Enhancements

The project can be improved in the future by adding:

- Real-time shipment location tracking.
- Email and SMS notifications.
- Advanced reports and analytics.
- Improved inventory management.
- Integration with external logistics services.
- Cloud deployment.
- Additional integration tests.

## Conclusion

SmartSupplyChain provides a single platform for managing important logistics and supply chain activities. The project uses ASP.NET Core Web API, Entity Framework Core, SQL Server, and a web-based frontend to organize data and support order and shipment management.

It also provides an opportunity to apply concepts such as layered architecture, REST APIs, authentication, database management, and automated testing in a practical software project.