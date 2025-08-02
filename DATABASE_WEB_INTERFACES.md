# Веб-интерфейсы для работы с базой данных

В проекте настроены несколько веб-интерфейсов для работы с базой данных SQLite.

## Доступные интерфейсы

### 1. SQLite Web (Рекомендуется для SQLite)
- **URL**: http://localhost:8080
- **Описание**: Специализированный веб-интерфейс для работы с SQLite базами данных
- **Возможности**:
  - Просмотр структуры базы данных
  - Выполнение SQL запросов
  - Просмотр и редактирование данных
  - Экспорт данных
  - Создание и управление таблицами

### 2. Adminer
- **URL**: http://localhost:8081
- **Описание**: Универсальный веб-интерфейс для различных баз данных
- **Возможности**:
  - Поддержка множества СУБД
  - Простой и быстрый интерфейс
  - Выполнение SQL запросов
  - Управление структурой базы

### 3. DBeaver Community (CloudBeaver)
- **URL**: http://localhost:8978
- **Описание**: Мощный веб-интерфейс для работы с базами данных
- **Возможности**:
  - Полнофункциональный SQL редактор
  - Визуализация схемы базы данных
  - Поддержка множества СУБД
  - Экспорт/импорт данных
  - Создание диаграмм ER

## Запуск

```bash
# Запуск всех сервисов
docker-compose up -d

# Запуск только веб-интерфейсов для базы данных
docker-compose up -d sqlite-web adminer dbeaver
```

## Использование SQLite Web

1. Откройте http://localhost:8080
2. База данных `WebApplication3.db` будет автоматически загружена
3. Используйте интерфейс для:
   - Просмотра таблиц
   - Выполнения SQL запросов
   - Редактирования данных

### Примеры SQL запросов

```sql
-- Просмотр всех пользователей
SELECT * FROM Users;

-- Просмотр курсов с количеством уроков
SELECT c.Name, c.Description, COUNT(l.Id) as LessonCount
FROM Courses c
LEFT JOIN Lessons l ON c.Id = l.CourseId
GROUP BY c.Id, c.Name, c.Description;

-- Просмотр заданий с курсами
SELECT a.Title, a.Description, c.Name as CourseName
FROM Assignments a
JOIN Courses c ON a.CourseId = c.Id;

-- Просмотр книг
SELECT Title, Author, ISBN, CreatedAt
FROM Books
ORDER BY CreatedAt DESC;
```

## Использование Adminer

1. Откройте http://localhost:8081
2. В настройках подключения:
   - **Система**: SQLite
   - **Сервер**: (оставьте пустым)
   - **Пользователь**: (оставьте пустым)
   - **Пароль**: (оставьте пустым)
   - **База данных**: выберите файл `WebApplication3.db`

## Использование DBeaver Community

1. Откройте http://localhost:8978
2. Создайте новое подключение:
   - **Тип**: SQLite
   - **Путь к файлу**: `/data/WebApplication3.db`
3. Используйте мощный SQL редактор для работы с данными

## Структура базы данных

### Основные таблицы:

- **Users** - пользователи системы
- **Roles** - роли пользователей
- **Courses** - курсы
- **Lessons** - уроки
- **Assignments** - задания
- **Books** - книги
- **CourseEnrollments** - записи на курсы
- **AssignmentSubmissions** - сдачи заданий

### Связи:

- Users → Roles (многие к одному)
- Lessons → Courses (многие к одному)
- Assignments → Courses (многие к одному)
- CourseEnrollments → Users, Courses (многие к одному)
- AssignmentSubmissions → Users, Assignments (многие к одному)

## Безопасность

⚠️ **Важно**: Эти интерфейсы предназначены только для разработки!

- Не используйте в продакшене
- Ограничьте доступ к портам в продакшене
- Используйте VPN или firewall для защиты в продакшене

## Полезные команды

```bash
# Просмотр логов SQLite Web
docker-compose logs sqlite-web

# Перезапуск SQLite Web
docker-compose restart sqlite-web

# Остановка всех веб-интерфейсов
docker-compose stop sqlite-web adminer dbeaver

# Просмотр всех запущенных контейнеров
docker-compose ps
```

## Альтернативные инструменты

Если нужны дополнительные возможности:

1. **SQLite Studio** - десктопное приложение
2. **DB Browser for SQLite** - популярный GUI клиент
3. **DataGrip** - профессиональная IDE от JetBrains
4. **VS Code + SQLite Extension** - расширение для VS Code

## Troubleshooting

### Проблема: Не могу подключиться к SQLite Web
**Решение**: Проверьте, что файл базы данных существует:
```bash
ls -la WebApplication3/WebApplication3.db
```

### Проблема: База данных пустая
**Решение**: Запустите миграции:
```bash
cd WebApplication3
dotnet ef database update
```

### Проблема: Порт занят
**Решение**: Измените порты в docker-compose.yml:
```yaml
ports:
  - "8082:8080"  # Вместо 8080:8080
``` 