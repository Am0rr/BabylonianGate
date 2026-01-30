# 🛡️ BabylonianGate

**BabylonianGate** is a robust RESTful API designed for a **Weapon Warehouse Management System**.
The system facilitates the management of personnel, weaponry, and ammunition, ensuring strict inventory control and comprehensive audit logging of all operations.

## 🚀 Key Features

- **Clean Architecture**: Strict separation of concerns (Domain, Application, Infrastructure, API).
- **Always Valid Domain Model**: Entities enforce invariants via static factory methods, preventing invalid state.
- **Robust Error Handling**: Global Exception Middleware transforms exceptions into standardized HTTP responses.
- **Automatic Validation**: Request validation pipeline using **FluentValidation**.
- **Audit Logging**: Automatic tracking of critical operations (Issue, Return, Maintenance) via `OperationLog`.
- **Unit Testing**: High test coverage using **xUnit**, **Moq**, and **FluentAssertions**.

## 🛠️ Tech Stack

- **Platform**: .NET 10 (ASP.NET Core Web API)
- **Database**: PostgreSQL / Entity Framework Core
- **Validation**: FluentValidation
- **Testing**: xUnit, Moq, FluentAssertions
- **Documentation**: Swagger / OpenAPI
- **Containerization**: Docker & Docker Compose
- **Principles**: SOLID, DDD (Domain-Driven Design), CQRS (Lite), Repository Pattern.

## 🏗️ Project Structure

The solution follows the Clean Architecture principles:

- **BG.Domain**: The core. Contains enterprise logic, entities (`Soldier`, `Weapon`, `AmmoCrate`), and interfaces. **No external dependencies.**
- **BG.App**: Business logic. Contains Services (`SoldierService`, etc.), DTOs, and Validators.
- **BG.Infra**: Data access. EF Core implementation, migrations, and repository logic.
- **BG.Api**: Entry point. Controllers, Middleware configuration, and DI setup.
- **BG.Tests**: Unit tests ensuring business logic reliability and domain integrity.

## ⚡ Getting Started

### Prerequisites

- Docker & Docker Compose **OR**
- .NET 8 SDK & PostgreSQL

### Run with Docker (Recommended)

```
# 1. Clone the repository
git clone [https://github.com/Am0rr/BabylonianGate.git](https://github.com/Am0rr/BabylonianGate.git)

# 2. Navigate to directory
cd BabylonianGate

# 3. Run application
docker-compose up -d --build
```

### Run Tests

```
dotnet test
```

## 📝 Development Status

The project is in active development, focusing on high code quality and architectural standards.

- [x] **Core Architecture**: Domain, Application, Infrastructure layers setup.
- [x] **Database**: EF Core context & PostgreSQL connection.
- [x] **Domain Logic**: "Always Valid" entities & Domain Services.
- [x] **API Layer**: Controllers, Middleware, DTOs.
- [x] **Validation**: FluentValidation integration.
- [x] **Unit Testing**: Full coverage for Domain Entities & Application Services (98 tests).
- [ ] **Frontend Client**.

---

Developed by [Am0rr](https://github.com/Am0rr)
