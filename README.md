# Radency_TestAssignment

![ASP.NET](https://img.shields.io/badge/ASP.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-512BD4?style=for-the-badge&logo=entity-framework&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)

Property Rental Management System built with ASP.NET Core MVC (.NET 10), Entity Framework Core and SQL Server.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server or SQL Server Express
- [LibMan CLI](https://learn.microsoft.com/aspnet/core/client-side/libman/libman-cli):
```bash
dotnet tool install -g Microsoft.Web.LibraryManager.Cli
```

## Usage

1. Clone the repository:
```bash
git clone https://github.com/nikita-stepanov04/Radency_TestAssignment
```

2. Navigate to the project directory:
```bash
cd Radency_TestAssignment/Radency_TestAssignment.Web
```

3. Open the configuration file (`appsettings.json`) and set the following values:
```json
{
    "ConnectionStrings": {
        "DbConnection": "Server=<host>\\<instance>;Database=Radency_TestAssignmentDB;User Id=<string>;Password=<string>;TrustServerCertificate=True"
    },
    "Serilog": {
        "MinimumLevel": {
          "Default": "Information",
          "Override": {
            "Microsoft.AspNetCore": "Warning",
            "LuckyPennySoftware.AutoMapper.License": "Fatal",
            "Quartz": "Warning"
          }
        }
    },
    "Pagination": {
        "ItemsPerPage": 10
    }
}
```

   | Setting | Description |
   |---|---|
   | `ConnectionStrings:DbConnection` | SQL Server connection string. For Windows authentication use `Trusted_Connection=True` instead of `User Id` and `Password`. |
   | `Pagination:ItemsPerPage` | Number of rows per page in all paged lists (properties, units, applications). Must be a positive integer. |

4. Restore NuGet packages:
```bash
dotnet restore
```

5. Restore client-side libraries:
```bash
libman restore
```

6. Run the application:
```bash
dotnet run
```

The application URL is printed in the console on startup (by default `http://localhost:5000`).

On the first start the application automatically creates the database, applies the migrations and seeds it with roles and unit types.