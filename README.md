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

- **Framework**: ASP.NET Core 9.0
- **Database**: SQLite (with Entity Framework Core)
- **Authentication**: JWT (JSON Web Tokens)
- **Search Engine**: Elasticsearch 7.17.0
- **Logging**: Serilog with Elasticsearch integration
- **Architecture**: Clean Architecture with Repository Pattern
- **Containerization**: Docker & Docker Compose

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

- .NET 9.0 SDK
- Docker & Docker Compose
- Visual Studio 2022 or VS Code

### Quick Start with Docker

1. Clone the repository
```bash
git clone https://github.com/yourusername/backend.school.git
cd backend.school
```

2. Start all services with Docker Compose
```bash
docker-compose up -d
```

3. Access the services:
- **API**: http://localhost:3000
- **Swagger UI**: http://localhost:3000/swagger
- **Elasticsearch**: http://localhost:9200
- **Kibana**: http://localhost:5601
- **SQLite Web**: http://localhost:8080
- **Adminer**: http://localhost:8081
- **DBeaver Community**: http://localhost:8978

### Manual Installation

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
cd WebApplication3
dotnet ef database update
cd ..
```

4. Start Elasticsearch (optional)
```bash
docker-compose up -d elasticsearch kibana
```

5. Run the application
```bash
cd WebApplication3
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

### Search
- `GET /api/search?q={query}` - Search across all entities
- `GET /api/search/users?q={query}` - Search users
- `GET /api/search/courses?q={query}` - Search courses
- `GET /api/search/books?q={query}` - Search books

### Elasticsearch Management
- `GET /api/elasticsearch/health` - Check Elasticsearch health
- `GET /api/elasticsearch/info` - Get Elasticsearch info
- `POST /api/elasticsearch/test-logging` - Test logging

## Database Schema

The application uses Entity Framework Core with SQLite database. Key entities include:

- **User**: User accounts with roles
- **Course**: Educational courses
- **Lesson**: Lessons within courses
- **Assignment**: Assignments for students
- **AssignmentSubmission**: Student submissions
- **Book**: Library resources
- **Role**: User roles and permissions

## Web Interfaces

### Database Management
- **SQLite Web** (http://localhost:8080): Specialized interface for SQLite databases
- **Adminer** (http://localhost:8081): Universal database management interface
- **DBeaver Community** (http://localhost:8978): Professional database IDE

### Monitoring & Analytics
- **Kibana** (http://localhost:5601): Log analysis and visualization
- **Elasticsearch** (http://localhost:9200): Search engine and log storage

### Quick Start Scripts
```bash
# Start only database interfaces
./start-db-interfaces.sh

# Start all services
docker-compose up -d

# View logs
docker-compose logs -f
```

## Documentation

- [API Documentation](API_DOCUMENTATION.md)
- [Elasticsearch Setup](ELASTICSEARCH_SETUP.md)
- [Database Web Interfaces](DATABASE_WEB_INTERFACES.md)
- [Logging Examples](WebApplication3/LOGGING_EXAMPLES.md)

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details. 