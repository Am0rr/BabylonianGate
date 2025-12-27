# 🛡️ BabylonianGate

**BabylonianGate** is a RESTful API designed for a **Weapon Warehouse Management System**.
The system facilitates the management of personnel, weaponry, and ammunition, while logging all inventory issuance and reception operations.

## 🚀 Tech Stack

- **Platform**: .NET 8 (ASP.NET Core Web API)
- **Architecture**: Clean Architecture (Domain, Application, Infrastructure, API)
- **Database**: PostgreSQL / Entity Framework Core
- **Containerization**: Docker & Docker Compose
- **Principles**: SOLID, Dependency Injection, Repository Pattern, CQRS (via Services/DTOs)

## 🏗️ Project Structure

The solution follows the Clean Architecture principles to ensure separation of concerns:

- **BG.Domain**: The core of the system. Contains enterprise logic, entities (\`Soldier\`, \`Weapon\`, \`AmmoCrate\`), and repository interfaces. No external dependencies.
- **BG.App**: Application business logic. Contains DTOs and Service interfaces (\`IWeaponService\`, \`ISoldierService\`).
- **BG.Infra**: Infrastructure layer. Implementation of data access (EF Core DbContext), migrations, and repositories.
- **BG.Api**: The entry point. API Controllers and Dependency Injection configuration.

## 🛠️ Getting Started

```
# 1. Clone the repository

git clone https://github.com/Am0rr/BabylonianGate.git

# 2. Navigate to the project directory

cd BabylonianGate

# 3. Run with Docker Compose (Database + API)

docker-compose up -d --build
```

## 📝 Development Status

The project is currently under active development.

- [x] Domain models and Core Architecture
- [x] EF Core and PostgreSQL configuration
- [ ] Business Logic Services implementation
- [ ] Unit & Integration Testing

---

_Developed by [Am0rr](https://github.com/Am0rr)_
