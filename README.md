# Backend.School

A comprehensive school management system backend built with ASP.NET Core Web API.

## Features

- **User Management**: Authentication, authorization, and user profiles
- **Course Management**: Create, update, and manage courses
- **Lesson Management**: Organize lessons within courses
- **Assignment System**: Create assignments and track submissions
- **Book Management**: Library system for educational resources
- **Role-based Access Control**: Different roles for students, teachers, and administrators

## Technology Stack

- **Framework**: ASP.NET Core 8.0
- **Database**: SQLite (with Entity Framework Core)
- **Authentication**: JWT (JSON Web Tokens)
- **Architecture**: Clean Architecture with Repository Pattern

## Project Structure

```
WebApplication3/
├── Controllers/          # API Controllers
├── Data/                # Database context and migrations
├── Models/              # Entities, DTOs, and responses
├── Services/            # Business logic services
└── Program.cs          # Application entry point
```

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository
```bash
git clone https://github.com/yourusername/backend.school.git
cd backend.school
```

2. Restore dependencies
```bash
dotnet restore
```

3. Run database migrations
```bash
dotnet ef database update
```

4. Run the application
```bash
dotnet run
```

The API will be available at `https://localhost:7000` (or the configured port).

## API Endpoints

### Authentication
- `POST /api/auth/login` - User login
- `POST /api/auth/register` - User registration
- `POST /api/auth/change-password` - Change password

### Users
- `GET /api/users/profile` - Get user profile
- `GET /api/users` - Get all users (Admin only)

### Courses
- `GET /api/courses` - Get all courses
- `POST /api/courses` - Create new course
- `GET /api/courses/{id}` - Get course by ID
- `PUT /api/courses/{id}` - Update course
- `DELETE /api/courses/{id}` - Delete course

### Lessons
- `GET /api/lessons` - Get all lessons
- `POST /api/lessons` - Create new lesson
- `GET /api/lessons/{id}` - Get lesson by ID
- `PUT /api/lessons/{id}` - Update lesson
- `DELETE /api/lessons/{id}` - Delete lesson

### Assignments
- `GET /api/assignments` - Get all assignments
- `POST /api/assignments` - Create new assignment
- `GET /api/assignments/{id}` - Get assignment by ID
- `PUT /api/assignments/{id}` - Update assignment
- `DELETE /api/assignments/{id}` - Delete assignment

### Submissions
- `GET /api/submissions` - Get all submissions
- `POST /api/submissions` - Submit assignment
- `GET /api/submissions/{id}` - Get submission by ID
- `PUT /api/submissions/{id}/grade` - Grade submission

### Books
- `GET /api/books` - Get all books
- `POST /api/books` - Add new book
- `GET /api/books/{id}` - Get book by ID
- `PUT /api/books/{id}` - Update book
- `DELETE /api/books/{id}` - Delete book

## Database Schema

The application uses Entity Framework Core with SQLite database. Key entities include:

- **User**: User accounts with roles
- **Course**: Educational courses
- **Lesson**: Lessons within courses
- **Assignment**: Assignments for students
- **AssignmentSubmission**: Student submissions
- **Book**: Library resources
- **Role**: User roles and permissions

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details. 