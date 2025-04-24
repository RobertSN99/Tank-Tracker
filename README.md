# Tank Tracker

**Tank Tracker** is a web application designed to manage a collection of tanks from World of Tanks game. Built with **ASP.NET Core**, **React (Vite)**, and **SQL Server**, it allows users to explore, add, edit, and rate tanks. The project is currently under development.

---

## Features

- **View Tanks**  
  Anyone can browse the list of tanks and view their details without needing to log in.

- **Authentication & Authorization**

  - Users can register and log in using a cookie-based authentication system.
  - Role-based access control is implemented using ASP.NET Core Identity and policy-based authorization.

- **Tank Management (Restricted Access)**

  - **Moderators** can add, edit, or delete tanks.
  - **Admins** can do everything moderators can, plus manage users (including promoting/demoting moderators).

- **Session Tracking**

  - Active sessions are tracked and visible to authorized roles.

- **Rating System**

  - **Admins** and **Moderators** will be able to rate tanks from 1 to 5 stars, with steps of 0.5.

- **Frontend theme toggle**

  - Users can select between dark theme and light theme

---

## Tech Stack

- **Backend:** ASP.NET Core Web API (.NET 8), Identity Framework
- **Frontend:** React (with Vite)
- **Database:** SQL Server
- **Authentication:** Cookie-based (HttpOnly)

---

## Roadmap

- [x] Implement authentication & authorization system
- [x] Implement session tracking
- [x] Implement tank CRUD endpoints in the backend
- [x] Integrate frontend with tank endpoints
- [x] Develop rating functionality for tanks
- [ ] Get tanks data directly from WarGaming API
- [ ] Expand auth system by using OAuth 2.0
- [ ] Implement frontend user management
- [ ] Enhance UI/UX for better user experience
- [ ] Write comprehensive tests for both frontend and backend

---

## Setup Instructions

Follow the steps below to set up the project locally:

### 1. Clone the Repository

```bash
git clone https://github.com/RobertSN99/Tank-Tracker.git
cd Tank-Tracker
```

---

### 2. Set up the Backend (ASP.NET Core)

Navigate to the server project directory:

```bash
cd Server
```

Restore dependencies:

```bash
dotnet restore
```

Ensure you have SQL Server installed and running. Create a new database (e.g., `TankTrackerDb`), then update the `appsettings.json` file:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=TankTrackerDb;Trusted_Connection=True;TrustServerCertificate=True",
}
```

Apply database migrations:

```bash
dotnet ef database update
```

Run the backend server:

```bash
dotnet run
```

---

### 3. Set up the Frontend (React + Vite)

```bash
cd ../Client
```

Install the dependencies:

```bash
npm install
```

Start the Vite development server:

```bash
npm run dev
```
