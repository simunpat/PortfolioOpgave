# Portfolio Assignment - Database Programming

This is a .NET Core Web API project developed as part of the Database Programming course. The application demonstrates the implementation of a RESTful API with database integration using Entity Framework Core.

## Features

- RESTful API endpoints for data operations
- Entity Framework Core for database access
- Repository pattern implementation
- Dependency Injection
- Data Transfer Objects (DTOs)
- Database migrations

## Technologies Used

- .NET Core
- Entity Framework Core
- C#
- SQL Database

## Getting Started

### Prerequisites

- .NET SDK
- A SQL database (configured in appsettings.json)

### Installation

1. Clone the repository
   ```
   git clone https://github.com/YOUR_USERNAME/PortfolioOpgave.git
   ```

2. Navigate to the project directory
   ```
   cd PortfolioOpgave
   ```

3. Restore dependencies
   ```
   dotnet restore
   ```

4. Update the database
   ```
   dotnet ef database update
   ```

5. Run the application
   ```
   dotnet run
   ```

### Test User Credentials

You can use the following credentials to test the API:

```json
{
  "email": "john@example.com",
  "password": "Password123"
}
```

## Project Structure

- **Controllers/**: API endpoints
- **Models/**: Database entity models
- **DTOs/**: Data Transfer Objects
- **Repositories/**: Data access layer
- **Services/**: Business logic
- **Interfaces/**: Contracts for repositories and services
- **Data/**: Database context and configuration
- **Migrations/**: Database migration files

## License

This project is for educational purposes.

## Author

Símun Pætur á Torkilsheyggi