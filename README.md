# Hotel Management System

## Project Overview
A multi-layered .NET 8.0 hotel management system with SQL Server and MongoDB integration. The system handles hotel reservations, room management, payments, and activity logging.

## Architecture
- **N-Tier Architecture**
  - Web API Layer (ASP.NET Core)
  - Business Logic Layer
  - Data Access Layer
  - Entity Layer

## Technologies Used
- .NET 8.0
- SQL Server (Primary Database)
- MongoDB (Logging Database)
- Dapper (SQL ORM)
- Swagger/OpenAPI
- ASP.NET Core Web API

## Key Features
1. **Reservation Management**
   - Create/Modify/Cancel reservations
   - Check-in/Check-out processing
   - Automatic cancellation fee calculation
   - Date-based availability checking

2. **Room Management**
   - Multiple room types support
   - Occupancy tracking
   - Revenue reporting
   - Availability checking

3. **Payment Processing**
   - Multiple payment methods
   - Payment tracking per reservation
   - Total calculation per booking

4. **Activity Logging**
   - MongoDB-based audit trail
   - User action tracking
   - System event logging

## Project Structure
```bash 
SolucionHotel/
├── WebApi/ # API Layer
├── Negocio/ # Business Logic Layer
├── AccesoDatos/ # Data Access Layer
├── Entidades/ # Entity Layer
└── SP/ # SQL Scripts & Stored Procedures
```

## Database Configuration
### SQL Server
1. Run the following scripts in order:
   - `script_creacion.sql` (Database creation)
   - `script_usuariobd.sql` (User setup)
   - `pa_*.sql` (Stored procedures)

### MongoDB
Configure connection string in `appsettings.json`:


## API Endpoints
- `/api/Reservacion` - Reservation management
- `/api/Habitacion` - Room management
- `/api/Pago` - Payment processing
- `/api/Bitacora` - Activity logging

## Getting Started
1. Clone the repository
2. Configure database connections in `appsettings.json`
3. Run database scripts
4. Build and run the solution

## Logging
- MongoDB-based audit trail
- Activity tracking
- Error logging
- User action monitoring
