# Domino's Reporting Optimization API

A simple ASP.NET Core API that demonstrates SQL query optimization for reporting systems. Built to explore the performance difference between poorly written and optimized database queries.

## What This Does

This project simulates a basic reporting platform with sales data. It has multiple endpoints that let you query store performance, product sales, and revenue metrics. The key feature is comparing unoptimized vs optimized versions of the same queries—showing real performance differences through stored procedures called from C#.

## Why It Matters

When you have thousands of orders in a database, how you write your SQL queries makes a huge difference. I built this to show the real performance impact:
- The unoptimized versions make multiple database calls or use inefficient patterns
- The optimized versions use JOINs, proper indexing, and window functions
- You can see the execution time difference for each comparison

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

### Reporting Endpoints (LINQ-based)
- `GET /api/reports/sales-optimized?startDate=2025-09-01&endDate=2025-11-30` - Fast version with JOINs
- `GET /api/reports/sales-unoptimized?startDate=2025-09-01&endDate=2025-11-30` - Slow version for comparison
- `GET /api/reports/store-performance` - Metrics for all stores
- `GET /api/reports/top-products?topCount=10` - Top 10 best-selling products

### Stored Procedure Endpoints (SQL-based optimization examples)

**Example 1: Orders with Stores (JOIN optimization)**
- `GET /api/storedprocedures/orders-unoptimized` - Simple SELECT without JOIN
- `GET /api/storedprocedures/orders-optimized` - Single query with JOIN and window functions

**Example 2: Product Sales (Index optimization)**
- `GET /api/storedprocedures/products-unoptimized` - Missing index on foreign key
- `GET /api/storedprocedures/products-optimized` - With index on OrderItems.ProductId

**Example 3: Store Rankings (Subquery vs Window Functions)**
- `GET /api/storedprocedures/stores-unoptimized` - Correlated subqueries (N+1 problem)
- `GET /api/storedprocedures/stores-optimized` - Window functions and CTE

## Database

The project uses 4 tables:
- **Stores** - 5 store locations
- **Orders** - 500 sample orders
- **Products** - 10 products (pizzas, wings, etc)
- **OrderItems** - Line items for each order

Sample data is automatically seeded when you run the app.

## Optimization Patterns Demonstrated

### Pattern 1: JOINs vs Separate Queries
- **Unoptimized:** Fetch orders, then make separate calls to get store info
- **Optimized:** Single query with INNER JOIN and window functions
- **Lesson:** Combining related data in one query is much faster than multiple round-trips

### Pattern 2: Missing Indexes
- **Unoptimized:** No index on foreign key = table scan required
- **Optimized:** Index on OrderItems.ProductId for efficient joins
- **Lesson:** Proper indexing on frequently joined columns improves performance significantly

### Pattern 3: Correlated Subqueries vs Window Functions
- **Unoptimized:** Correlated subqueries execute once per row (N+1 problem)
- **Optimized:** Window functions and CTE process data in single pass
- **Lesson:** Aggregate functions should be computed at the SQL level, not in application code

## What I Learned

Working on this project helped me understand:
- How to write efficient LINQ queries
- The importance of JOINs vs loading data separately
- Using Entity Framework Core effectively
- Building stored procedures and calling them from C#
- How indexes impact query performance
- Performance tracking and measurement
- Building a simple REST API

## Key Takeaways

1. **Measure First** - Use execution time metrics to understand performance
2. **Understand Your Data** - Know table sizes and relationships before optimizing
3. **Test Thoroughly** - Ensure optimized queries return the same data as unoptimized versions
4. **Use Proper Tools** - SQL Server execution plans and query analysis are essential
5. **It's About Trade-offs** - Optimization involves balancing complexity, readability, and performance

## Next Steps

If I were to continue this project, I'd add:
- Query performance logging to compare response times
- Caching for frequently requested reports
- Database indexes on commonly filtered columns
- More complex report filtering options
- Frontend dashboard to visualize the data

## ZSH SCRIPTS
## brew install prometheus
## prometheus --config.file=prometheus.yml

## Open browser and go to: localhost:9090

## http_request_duration_seconds_bucket
## http_requests_received_total{handler="/metrics"}

## make sure API is running, 
## ZSH ./load_test.zsh (this will hit the endpoints 50 times)


## This calculates the average request duration per endpoint over the last 5 minutes.
## rate(http_request_duration_seconds_sum[5m])
## rate(http_request_duration_seconds_count[5m])

## to see just the stored procedures endpoints:
## http_request_duration_seconds_bucket{handler=~"/api/storedprocedures.*"}