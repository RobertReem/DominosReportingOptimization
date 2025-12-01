# Domino's Reporting Optimization API

A simple ASP.NET Core API that demonstrates SQL query optimization for reporting systems. Built to explore the performance difference between poorly written and optimized database queries.

## What This Does

This project simulates a basic reporting platform with sales data. It has four main endpoints that let you query store performance, product sales, and revenue metrics. The key feature is comparing two versions of the same report—one written inefficiently and one optimized.

## Why It Matters

When you have thousands of orders in a database, how you write your SQL queries makes a huge difference. I built this to show the real performance impact:
- The unoptimized version makes multiple database calls
- The optimized version does it all in one query

## Tech Stack

- ASP.NET Core 9.0
- C#
- SQL Server
- Entity Framework Core
- Swagger for API testing

## Getting Started

### Prerequisites
- .NET 9 SDK
- SQL Server (or Docker)
- Git

### Setup

Clone the repo:
```zsh
git clone https://github.com/RobertReem/DominosReportingOptimization.git
cd DominosReportingOptimization
```

Start SQL Server (if using Docker):
```zsh
docker run --platform linux/arm64 -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Password@123" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/azure-sql-edge
```

Build and run:
```zsh
dotnet build
dotnet run
```

Visit Swagger UI:
```
http://localhost:5272/swagger
```

## API Endpoints

### Sales Reports
- `GET /api/reports/sales-optimized?startDate=2025-09-01&endDate=2025-11-30` - Fast version with JOINs
- `GET /api/reports/sales-unoptimized?startDate=2025-09-01&endDate=2025-11-30` - Slow version for comparison

### Store Performance
- `GET /api/reports/store-performance` - Metrics for all stores

### Product Analytics
- `GET /api/reports/top-products?topCount=10` - Top 10 best-selling products

## Database

The project uses 4 tables:
- **Stores** - 5 store locations
- **Orders** - 500 sample orders
- **Products** - 10 products (pizzas, wings, etc)
- **OrderItems** - Line items for each order

Sample data is automatically seeded when you run the app.

## What I Learned

Working on this project helped me understand:
- How to write efficient LINQ queries
- The importance of JOINs vs. loading data separately
- Using Entity Framework Core effectively
- Building a simple REST API

## Next Steps

If I were to continue this project, I'd add:
- Query performance logging to compare response times
- Caching for frequently requested reports
- Database indexes on commonly filtered columns
- More complex report filtering options