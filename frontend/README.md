# Fullstack Auth App

A full-stack authentication application built with React, C# (ASP.NET Core), PostgreSQL, and Docker.

## Tech Stack

- **Frontend:** React, React Router, Axios, Plain CSS
- **Backend:** C# ASP.NET Core Web API
- **Database:** PostgreSQL
- **Containerisation:** Docker & Docker Compose

## Architecture
```
React Frontend
  → Nginx (reverse proxy)
    → ASP.NET Core API
      → Service Layer (business logic)
        → Repository Layer (data access)
          → PostgreSQL
```

### Backend Layers
- **Controllers** — handle HTTP requests and responses
- **Services** — contain all business logic
- **Repositories** — handle all database operations
- **DTOs** — define the shape of data sent over the wire
- **Models** — represent database entities

## Features

- User registration with First Name, Last Name, Email and Password
- Secure login with JWT authentication
- Password hashing with BCrypt
- Protected routes on both frontend and backend
- Auto database migrations on startup
- Unit tests with xUnit and Moq
- Integration tests with xUnit
- Build scripts for Windows and Linux/Mac

## Prerequisites

- Docker Desktop

That's it. Docker handles everything else.

## How to Run

**Option 1 - Using the build script (recommended):**

Windows:
```bash
build.bat
```

Linux/Mac:
```bash
chmod +x build.sh
./build.sh
```

**Option 2 - Manual:**
```bash
docker-compose up --build
```

**3. Open your browser:**
```
http://localhost
```

## API Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | /api/auth/register | Register a new user | No |
| POST | /api/auth/login | Login and receive JWT token | No |
| GET | /api/user/me | Get logged in user details | Yes |

## Running Tests



**All tests (unit + integration):**
```bash
cd backend.Tests
dotnet test
```

This will run all 13 tests:
- 7 unit tests (AuthService and UserService)
- 6 integration tests (API endpoints end to end)

## Assessment Requirements Coverage

| Requirement | Status |
|-------------|--------|
| Register page with First Name, Last Name, Email, Password | ✅ |
| Login page with Email and Password | ✅ |
| Protected User Details page | ✅ |
| C# ASP.NET Core API | ✅ |
| PostgreSQL database | ✅ |
| Backend running in Docker | ✅ |
| PostgreSQL running in same Docker setup | ✅ |
| Unit tests | ✅ |
| Integration tests | ✅ |
| Build script | ✅ |
| Source control (GitHub) | ✅ |
| Comprehensive README | ✅ |


## Bonus Points

| Bonus Item | Status | Details |
|------------|--------|---------|
| Integration tests | ✅ | 6 integration tests covering all API endpoints end to end |
| Build script | ✅ | `build.bat` for Windows, `build.sh` for Linux/Mac |


## Project Structure
```
fullstack-auth-app/
├── frontend/                  # React application
│   ├── src/
│   │   ├── api/               # HTTP calls to backend
│   │   ├── context/           # Auth context (global state)
│   │   ├── components/        # Reusable components
│   │   └── pages/             # Register, Login, UserDetails
│   ├── nginx.conf             # Nginx reverse proxy config
│   └── Dockerfile
├── backend/                   # ASP.NET Core API
│   ├── Controllers/
│   ├── Services/
│   ├── Repositories/
│   ├── Models/
│   ├── DTOs/
│   ├── Data/
│   └── Dockerfile
├── backend.Tests/             # Unit and integration tests
├── build.bat                  # Build script for Windows
├── build.sh                   # Build script for Linux/Mac
├── docker-compose.yml
└── README.md
```