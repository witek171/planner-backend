# Planner API

A universal backend API designed for reservation systems, staff management, and client scheduling.

---
## About the Project

This project provides a comprehensive and flexible backend solution designed to power scheduling and management applications for a wide variety of service-based businesses. Its universal architecture makes it suitable for organizations like:

- Sports clubs: managing class schedules, booking courts or facilities, and assigning trainers.

- Medical clinics: scheduling patient appointments, managing doctor availability, and handling patient records.

- Wellness centers: booking spa treatments, yoga classes, or consultations.

- Any business that relies on managing appointments, staff availability, and client reservations.

The API is built to handle complex relationships between companies (or locations), staff members, available services (events), and clients (participants), providing a robust foundation for a custom-tailored front-end application.

---
## Features

The system currently supports the following core functionalities:

### Authentication & Authorization:

- Staff registration and secure login (JWT-based).
  
- Multi-company context switching (staff choose one of their assigned companies).

- Role-based access control (RBAC) for staff roles (e.g., Manager, Reception employee, Trainer).
  
- Company-scoped data isolation.

### Company & Staff Management:

- CRUD operations for companies/locations, including hierarchy management (e.g., main office and receptions).

- CRUD operations for staff members, assignment to companies, and specialization management.

- Defining staff availability schedules.

- Validation of time ranges, including mandatory breaks between events, to prevent overlapping schedules, reservations, and staff availability conflicts.

### Reservation & Event Management:

- Defining event templates (EventTypes) and creating specific instances in the schedule (EventSchedules).

- Creating reservations and assigning multiple participants.

- Managing the payment status of reservations.

- CRUD operations for participant (client) data.

---
## Roadmap

- Full implementation of Email/SMS notifications (based on the Notifications table).

- Expansion of the internal messaging module (based on the Messages table).

- Integration with an online payment gateway.

- A dedicated client-facing panel for managing reservations.

- Reporting and statistics module.

---
## Technology & Libraries

- **.NET 8**: The latest LTS version of Microsoft's cross-platform framework.

- **ASP.NET Core**: For building the RESTful API.

- **ADO.NET**: For direct database communication and executing raw SQL queries.

- **Swashbuckle (Swagger)**: For API documentation generation.

- **TimePeriodLibrary.NET**: Used for handling time-period logic, such as in availability schedules.

- **JSON Web Token**: For stateless authentication and authorization.
  
- **AutoMapper**: For object-to-object mapping between DTOs and domain entities.
  
- **BCrypt.Net**: For secure password hashing using the BCrypt algorithm.

- **Microsoft SQL Server**: Relational database management system used for persistent data storage.

---
## Database Schema

<p align="center">
  <img width="3264" height="2944" alt="Image" src="https://github.com/user-attachments/assets/dc9c561e-06fe-4c51-bc43-a17a9d9c6059" />
</p>

---
## How to Run Locally

### Prerequisites:

 - [Docker Desktop](https://www.docker.com/products/docker-desktop/) installed and running.

### Quick start:

1. **Clone the repository:**
   ```bash
   git clone <repository-url>
   cd <repository-folder>
   ```

2. **Run Docker Compose:**
   ```bash
   docker-compose up -d --build
   ```

4. **Open Swagger:**
   ```bash
   http://localhost:5000/swagger
   ```

### Connection string (SQL Server):

Use this connection string to connect to the database from your local machine:

```bash
Server=localhost,1433;Database=PlannerDB;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;MultipleActiveResultSets=True;
```

JDBC equivalent:

```bash
jdbc:sqlserver://localhost:1433;databaseName=PlannerDB;user=sa;password=YourStrong@Password123;trustServerCertificate=true;multipleActiveResultSets=true
```

> Note: From inside Docker containers, `localhost` means **the container itself**.  
> Other containers should use the service name `sqlserver` instead of `localhost`.

### Demo login credentials

Use the following credentials to log in to the demo environment:

| Role | Email | Password |
|------|-------|----------|
| Manager | `m@m.pl` | `demo` |
| Trainer | `t@t.pl` | `demo` |
| Reception Employee | `r@r.pl` | `demo` |

> demo users are automatically created during application startup.

 ### What happens automatically:
- SQL Server starts up (port 1433).
- Database is created and populated with sample data.
- RSA keys are generated.
- API is ready to use (port 5000).

 ### Stop services:
```bash
docker-compose down
```

