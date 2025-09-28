# Recruitment Management System - ASP.NET Core Web API

This is a **Recruitment Management System** built using **ASP.NET Core Web API (.NET 6)**, **Entity Framework Core**, and **SQL Server**. The API allows for managing job postings, applicants, and recruitment workflows.

---

## 🔧 Tech Stack

- **Backend Framework**: ASP.NET Core Web API (.NET 6)
- **IDE**: Visual Studio 2022
- **Database**: SQL Server
- **ORM**: Entity Framework Core
- **API Testing**: Swagger (OpenAPI)

---

## 🚀 Getting Started

### ✅ Prerequisites

Make sure you have the following installed:

- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)
- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- Git

---

### 📥 Clone the Repository

```bash
git clone https://github.com/sujit628/RecruitmentManagementSystem.git
cd RecruitmentManagementSystem

⚙️ Setup Configuration

Open the project in Visual Studio 2022.

Open appsettings.json and configure your SQL Server connection string:
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=RecruitmentDB;Trusted_Connection=True;"
}

*** NuGet Packages Used ***
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
Swashbuckle.AspNetCore (for Swagger)

🗃️ Apply Database Migrations
Using the Package Manager Console in Visual Studio:
  Add-Migration FixCascadePaths
  Update-Database

📁 Project Structure
RecruitmentManagementSystem/
│
├── Controllers/          # API controllers
├── Models/               # Data models
├── Data/                 # DbContext and Migrations
├── DTOs/                 # Data transfer objects
├── Services/             # Business logic layer (optional)
├── appsettings.json      # App configuration
└── Program.cs            # Entry point

