# 🚗 Ride Sharing Dispatch System

A backend-focused **Ride Sharing Dispatch System** inspired by platforms like Uber.  
This project demonstrates **Clean Architecture**, **Domain-Driven Design (DDD)**, and **Entity Framework Core** using **ASP.NET Core (.NET 9)**.

---

## 📌 Project Overview

The Ride Sharing Dispatch System handles:

- User authentication and roles (Rider, Driver)
- Trip creation and lifecycle management
- Driver availability and assignment
- Dispatch logic based on proximity and vehicle type
- Persistent storage using a relational database

The system is designed to be **scalable**, **testable**, and **maintainable**, with a strong separation of concerns across layers.

---

## 🏗️ Architecture

The project follows **Clean Architecture** principles:

RideSharingDispatch
│
├── RideSharingDispatch.API # Controllers, DI, Startup
├── RideSharingDispatch.Application # Services, Interfaces, Business Logic
├── RideSharingDispatch.Domain # Entities, Enums, Core Rules
├── RideSharingDispatch.Infrastructure # EF Core, DbContext, Repositories


### Layer Responsibilities

| Layer | Responsibility |
|-----|---------------|
| **API** | HTTP endpoints, dependency injection, app configuration |
| **Application** | Business rules, service logic, interfaces |
| **Domain** | Core entities, enums, invariants |
| **Infrastructure** | Database access, EF Core, repository implementations |

---

## 🧠 Key Concepts Used

- Clean Architecture
- Repository Pattern
- Service Layer Pattern
- Dependency Injection
- Entity Framework Core (Code-First)
- Asynchronous Programming (`async/await`)

---

## 🧰 Tech Stack

- **.NET 9**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server**
- **C#**
- **LINQ**

---

## 🗂️ Domain Model (Core Entities)

- **User**
  - Email, PasswordHash, Role
- **Driver**
  - Online status, location, vehicle type
- **Trip**
  - Pickup & destination coordinates
  - Status lifecycle (Requested → Completed)
- **Enums**
  - `TripStatus`
  - `UserRole`
  - `VehicleType`

---

## 🔄 Trip Lifecycle

Requested
↓
Accepted
↓
Arrived
↓
InProgress
↓
Completed


Trips can also be **Cancelled** at valid stages.

---

## 🔐 Authentication (Current Scope)

- Login via email and password
- Password hashing using `PasswordHasher<TUser>`
- Role-based access planned (Rider / Driver)

---

## 📦 Dependency Injection

All dependencies are registered in `Program.cs`:

- `DbContext`
- Repositories (`IUserRepository`, `ITripRepository`, etc.)
- Services (`ITripService`, `IUserService`)

ASP.NET Core automatically resolves dependencies at runtime.

---

## 🛠️ Database & Migrations

- **EF Core Code-First**
- Migrations stored in the **Infrastructure** layer
- Database created and updated using:

```bash
dotnet ef database update
```

Running the Project

Clone the repository

Configure the database connection string in appsettings.json

Apply migrations:

```bash
dotnet ef database update
```

Run the API:

```bash
dotnet run
```