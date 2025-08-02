# 📚 Полная документация School API

## 🚀 Обзор

School API - это RESTful API для системы управления образовательным процессом с интеграцией Elasticsearch для полнотекстового поиска.

**Базовый URL:** `http://localhost:3000`

**Swagger UI:** `http://localhost:3000/swagger`

## 🔐 Аутентификация

API использует JWT (JSON Web Tokens) для аутентификации. Добавьте заголовок:
```
Authorization: Bearer YOUR_JWT_TOKEN
```

## 📊 Роли пользователей

- **Student** - студент
- **Teacher** - преподаватель  
- **Admin** - администратор

## 🔍 Elasticsearch & Kibana

- **Elasticsearch:** `http://localhost:9200`
- **Kibana:** `http://localhost:5601`

---

## 📋 API Endpoints

### 🔐 Аутентификация

#### POST /api/auth/login
**Вход в систему**

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "password123"
}
```

**Ответ:**
```json
{
  "success": true,
  "message": "Успешная аутентификация",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
      "id": 1,
      "email": "user@example.com",
      "firstName": "Иван",
      "lastName": "Иванов",
      "role": "Student"
    }
  }
}
```

#### POST /api/auth/register
**Регистрация нового пользователя**

```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "newuser@example.com",
  "password": "password123",
  "firstName": "Петр",
  "lastName": "Петров",
  "roleId": 1
}
```

#### GET /api/auth/profile
**Получить профиль текущего пользователя**

```http
GET /api/auth/profile
Authorization: Bearer YOUR_JWT_TOKEN
```

#### POST /api/auth/change-password
**Изменить пароль**

```http
POST /api/auth/change-password
Authorization: Bearer YOUR_JWT_TOKEN
Content-Type: application/json

{
  "currentPassword": "oldpassword",
  "newPassword": "newpassword"
}
```

---

### 📚 Книги

#### GET /api/book
**Получить все книги**

```http
GET /api/book
Authorization: Bearer YOUR_JWT_TOKEN
```

#### GET /api/book/{id}
**Получить книгу по ID**

```http
GET /api/book/1
Authorization: Bearer YOUR_JWT_TOKEN
```

#### GET /api/book/course/{courseId}
**Получить книги курса**

```http
GET /api/book/course/1
Authorization: Bearer YOUR_JWT_TOKEN
```

#### POST /api/book
**Создать новую книгу** *(Teacher, Admin)*

```http
POST /api/book
Authorization: Bearer YOUR_JWT_TOKEN
Content-Type: application/json

{
  "title": "Программирование на C#",
  "author": "Джон Смит",
  "description": "Учебник по программированию",
  "isbn": "978-0-123456-47-2",
  "publicationYear": 2023,
  "publisher": "Издательство",
  "pages": 450,
  "language": "Русский",
  "courseIds": [1, 2]
}
```

#### PUT /api/book/{id}
**Обновить книгу** *(Teacher, Admin)*

```http
PUT /api/book/1
Authorization: Bearer YOUR_JWT_TOKEN
Content-Type: application/json

{
  "title": "Обновленное название",
  "author": "Новый автор",
  "description": "Обновленное описание",
  "isbn": "978-0-123456-47-3",
  "publicationYear": 2024,
  "publisher": "Новое издательство",
  "pages": 500,
  "language": "Русский",
  "courseIds": [1, 3]
}
```

#### DELETE /api/book/{id}
**Удалить книгу** *(Teacher, Admin)*

```http
DELETE /api/book/1
Authorization: Bearer YOUR_JWT_TOKEN
```

#### POST /api/book/{bookId}/course/{courseId}
**Добавить книгу к курсу** *(Teacher, Admin)*

```http
POST /api/book/1/course/2
Authorization: Bearer YOUR_JWT_TOKEN
```

#### DELETE /api/book/{bookId}/course/{courseId}
**Удалить книгу из курса** *(Teacher, Admin)*

```http
DELETE /api/book/1/course/2
Authorization: Bearer YOUR_JWT_TOKEN
```

---

### 🔍 Поиск (Elasticsearch)

#### GET /api/search/global?query={query}
**Глобальный поиск по всем индексам**

```http
GET /api/search/global?query=программирование
Authorization: Bearer YOUR_JWT_TOKEN
```

**Ответ:**
```json
{
  "query": "программирование",
  "total_results": 5,
  "results": {
    "books": [
      {
        "id": 1,
        "title": "Программирование на C#",
        "author": "Джон Смит",
        "type": "book"
      }
    ],
    "courses": [
      {
        "id": 1,
        "name": "Основы программирования",
        "description": "Курс по программированию",
        "type": "course"
      }
    ]
  }
}
```

#### GET /api/search/{indexName}?query={query}
**Поиск по конкретному индексу**

```http
GET /api/search/books?query=python
Authorization: Bearer YOUR_JWT_TOKEN
```

**Доступные индексы:**
- `users` - пользователи
- `courses` - курсы
- `books` - книги
- `lessons` - уроки
- `assignments` - задания

#### GET /api/search/status
**Проверка статуса Elasticsearch** *(без авторизации)*

```http
GET /api/search/status
```

**Ответ:**
```json
{
  "status": "connected",
  "indices": {
    "users": false,
    "courses": false,
    "books": true,
    "lessons": false,
    "assignments": false
  }
}
```

#### POST /api/search/setup
**Создание индексов Elasticsearch** *(Admin)*

```http
POST /api/search/setup
Authorization: Bearer YOUR_JWT_TOKEN
```

---

### 🎓 Курсы

#### GET /api/course
**Получить все курсы**

#### GET /api/course/{id}
**Получить курс по ID**

#### POST /api/course
**Создать новый курс** *(Teacher, Admin)*

#### PUT /api/course/{id}
**Обновить курс** *(Teacher, Admin)*

#### DELETE /api/course/{id}
**Удалить курс** *(Admin)*

---

### 📖 Уроки

#### GET /api/lesson
**Получить все уроки**

#### GET /api/lesson/{id}
**Получить урок по ID**

#### GET /api/lesson/course/{courseId}
**Получить уроки курса**

#### POST /api/lesson
**Создать новый урок** *(Teacher, Admin)*

#### PUT /api/lesson/{id}
**Обновить урок** *(Teacher, Admin)*

#### DELETE /api/lesson/{id}
**Удалить урок** *(Teacher, Admin)*

---

### 📝 Задания

#### GET /api/assignment
**Получить все задания**

#### GET /api/assignment/{id}
**Получить задание по ID**

#### GET /api/assignment/course/{courseId}
**Получить задания курса**

#### POST /api/assignment
**Создать новое задание** *(Teacher, Admin)*

#### PUT /api/assignment/{id}
**Обновить задание** *(Teacher, Admin)*

#### DELETE /api/assignment/{id}
**Удалить задание** *(Teacher, Admin)*

---

### 📤 Отправки заданий

#### GET /api/submission
**Получить все отправки**

#### GET /api/submission/{id}
**Получить отправку по ID**

#### GET /api/submission/assignment/{assignmentId}
**Получить отправки задания**

#### GET /api/submission/student/{studentId}
**Получить отправки студента**

#### POST /api/submission
**Создать новую отправку** *(Student)*

#### PUT /api/submission/{id}/grade
**Оценить отправку** *(Teacher, Admin)*

---

### 👥 Пользователи

#### GET /api/user
**Получить всех пользователей** *(Admin)*

#### GET /api/user/{id}
**Получить пользователя по ID** *(Admin)*

#### PUT /api/user/{id}
**Обновить пользователя** *(Admin)*

#### DELETE /api/user/{id}
**Удалить пользователя** *(Admin)*

---

### 🎭 Роли

#### GET /api/role
**Получить все роли**

#### GET /api/role/{id}
**Получить роль по ID**

---

### 👨‍💼 Администрирование

#### GET /api/admin/stats
**Получить статистику системы** *(Admin)*

#### POST /api/admin/seed
**Заполнить тестовыми данными** *(Admin)*

---

## 🔧 Коды ответов

| Код | Описание |
|-----|----------|
| 200 | Успешный запрос |
| 201 | Создано |
| 400 | Неверный запрос |
| 401 | Не авторизован |
| 403 | Недостаточно прав |
| 404 | Не найдено |
| 409 | Конфликт (например, пользователь уже существует) |
| 500 | Ошибка сервера |

---

## 📊 Примеры использования

### Получение токена
```bash
curl -X POST http://localhost:3000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@school.com",
    "password": "admin123"
  }'
```

### Создание книги с автоматической индексацией
```bash
curl -X POST http://localhost:3000/api/book \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Новая книга",
    "author": "Автор",
    "description": "Описание книги",
    "isbn": "978-0-123456-47-4",
    "publicationYear": 2024,
    "publisher": "Издательство",
    "pages": 300,
    "language": "Русский",
    "courseIds": [1]
  }'
```

### Поиск по Elasticsearch
```bash
curl "http://localhost:3000/api/search/global?query=программирование" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

---

## 🎯 Особенности

### 🔍 Elasticsearch интеграция
- Автоматическая индексация при создании/обновлении/удалении книг
- Полнотекстовый поиск по всем сущностям
- Нечеткий поиск с поддержкой опечаток
- Глобальный поиск по всем индексам

### 🔐 Безопасность
- JWT аутентификация
- Ролевая авторизация
- Валидация входных данных
- Защита от SQL-инъекций

### 📈 Производительность
- Кэширование запросов
- Оптимизированные запросы к БД
- Асинхронная обработка
- Индексация в Elasticsearch

---

## 🛠️ Разработка

### Локальная разработка
```bash
# Запуск всех сервисов
docker-compose up -d

# Проверка статуса
curl http://localhost:3000/api/search/status

# Swagger UI
open http://localhost:3000/swagger

# Kibana
open http://localhost:5601
```

### Тестирование
```bash
# Запуск тестов
./test_elasticsearch.sh
```

---

## 📞 Поддержка

- **Email:** support@school.com
- **Документация:** `http://localhost:3000/swagger`
- **Elasticsearch:** `http://localhost:9200`
- **Kibana:** `http://localhost:5601` 