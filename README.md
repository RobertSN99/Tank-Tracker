# Tank Tracker

**Tank Tracker** is a web application designed to manage a collection of tanks. Built with **ASP.NET Core**, **React (Vite)**, and **SQL Server**, it allows users to explore, add, edit, and rate tanks. The project is currently under development.

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
  Active sessions are tracked and visible to authorized roles.

- **Rating System (Upcoming)**  
  Logged-in users will be able to rate tanks from 1 to 5 stars.

## Tech Stack

- **Backend:** ASP.NET Core Web API (.NET 8), Identity Framework
- **Frontend:** React (with Vite)
- **Database:** SQL Server
- **Authentication:** Cookie-based (HttpOnly)

## Project Status

This project is a **work in progress**:
- [x] Authentication system (register, login, logout)
- [x] Role-based access control
- [x] Session tracking
- [ ] Tank endpoints
- [ ] Frontend integration for tanks
- [ ] Rating functionality

## Setup Instructions

*To be added once the project reaches MVP status.*

## Roadmap

- [ ] Implement tank CRUD endpoints in the backend
- [ ] Integrate frontend with tank endpoints
- [ ] Develop rating functionality for tanks
- [ ] Enhance UI/UX for better user experience
- [ ] Write comprehensive tests for both frontend and backend
