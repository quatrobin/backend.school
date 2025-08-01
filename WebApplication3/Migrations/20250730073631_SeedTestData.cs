using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication3.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Добавляем роли только если их нет
            migrationBuilder.Sql(@"
                INSERT OR IGNORE INTO ""Roles"" (""Id"", ""Name"", ""Description"")
                VALUES 
                    (1, 'Студент', 'Роль для студентов'),
                    (2, 'Преподаватель', 'Роль для преподавателей'),
                    (3, 'Администратор', 'Роль для администраторов')
            ");

            // Добавляем пользователей только если их нет
            migrationBuilder.Sql(@"
                INSERT OR IGNORE INTO ""Users"" (""Id"", ""Email"", ""FirstName"", ""LastName"", ""PasswordHash"", ""RoleId"", ""CreatedAt"")
                VALUES 
                    (1, 'admin@school.com', 'Админ', 'Админов', 'AQAAAAEAACcQAAAAELbXpWZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQ==', 3, datetime('now')),
                    (2, 'teacher1@school.com', 'Иван', 'Петров', 'AQAAAAEAACcQAAAAELbXpWZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQ==', 2, datetime('now')),
                    (3, 'teacher2@school.com', 'Мария', 'Сидорова', 'AQAAAAEAACcQAAAAELbXpWZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQ==', 2, datetime('now')),
                    (4, 'student1@school.com', 'Алексей', 'Иванов', 'AQAAAAEAACcQAAAAELbXpWZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQ==', 1, datetime('now')),
                    (5, 'student2@school.com', 'Елена', 'Козлова', 'AQAAAAEAACcQAAAAELbXpWZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQ==', 1, datetime('now')),
                    (6, 'student3@school.com', 'Дмитрий', 'Смирнов', 'AQAAAAEAACcQAAAAELbXpWZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQ==', 1, datetime('now')),
                    (7, 'student4@school.com', 'Анна', 'Попова', 'AQAAAAEAACcQAAAAELbXpWZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQ==', 1, datetime('now')),
                    (8, 'student5@school.com', 'Сергей', 'Васильев', 'AQAAAAEAACcQAAAAELbXpWZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQZQ==', 1, datetime('now'))
            ");

            // Добавляем курсы только если их нет
            migrationBuilder.Sql(@"
                INSERT OR IGNORE INTO ""Courses"" (""Id"", ""Name"", ""Description"", ""CreatedAt"", ""UpdatedAt"")
                VALUES 
                    (1, 'Математика', 'Основы математики для студентов', datetime('now'), datetime('now')),
                    (2, 'Физика', 'Введение в физику', datetime('now'), datetime('now')),
                    (3, 'История', 'История России', datetime('now'), datetime('now')),
                    (4, 'Литература', 'Русская литература XIX века', datetime('now'), datetime('now')),
                    (5, 'Химия', 'Общая химия', datetime('now'), datetime('now'))
            ");

            // Добавляем книги только если их нет
            migrationBuilder.Sql(@"
                INSERT OR IGNORE INTO ""Books"" (""Id"", ""Title"", ""Author"", ""Description"", ""ISBN"", ""PublicationYear"", ""Publisher"", ""Pages"", ""Language"", ""CreatedAt"", ""UpdatedAt"")
                VALUES 
                    (1, 'Алгебра и начала анализа', 'Колмогоров А.Н.', 'Учебник по алгебре для 10-11 классов', '978-5-09-019243-9', 2019, 'Просвещение', 384, 'Русский', datetime('now'), datetime('now')),
                    (2, 'Физика. 10 класс', 'Мякишев Г.Я.', 'Учебник по физике для 10 класса', '978-5-09-019244-6', 2020, 'Просвещение', 416, 'Русский', datetime('now'), datetime('now')),
                    (3, 'История России', 'Данилов А.А.', 'Учебник по истории России', '978-5-09-019245-3', 2021, 'Просвещение', 352, 'Русский', datetime('now'), datetime('now')),
                    (4, 'Литература. 10 класс', 'Лебедев Ю.В.', 'Учебник по литературе для 10 класса', '978-5-09-019246-0', 2020, 'Просвещение', 368, 'Русский', datetime('now'), datetime('now')),
                    (5, 'Химия. 10 класс', 'Рудзитис Г.Е.', 'Учебник по химии для 10 класса', '978-5-09-019247-7', 2021, 'Просвещение', 320, 'Русский', datetime('now'), datetime('now'))
            ");

            // Добавляем уроки только если их нет
            migrationBuilder.Sql(@"
                INSERT OR IGNORE INTO ""Lessons"" (""Id"", ""Title"", ""Description"", ""CourseId"", ""LessonDate"", ""DurationMinutes"", ""Materials"", ""CreatedAt"", ""UpdatedAt"")
                VALUES 
                    (1, 'Введение в алгебру', 'Основные понятия алгебры', 1, datetime('now', '-30 days'), 90, 'Презентация, задачи', datetime('now'), datetime('now')),
                    (2, 'Квадратные уравнения', 'Решение квадратных уравнений', 1, datetime('now', '-25 days'), 90, 'Учебник, задачи', datetime('now'), datetime('now')),
                    (3, 'Механика', 'Основы механики', 2, datetime('now', '-28 days'), 90, 'Лабораторная работа', datetime('now'), datetime('now')),
                    (4, 'Термодинамика', 'Законы термодинамики', 2, datetime('now', '-20 days'), 90, 'Презентация', datetime('now'), datetime('now')),
                    (5, 'Древняя Русь', 'История Древней Руси', 3, datetime('now', '-22 days'), 90, 'Карты, документы', datetime('now'), datetime('now')),
                    (6, 'Пушкин А.С.', 'Творчество Пушкина', 4, datetime('now', '-18 days'), 90, 'Тексты произведений', datetime('now'), datetime('now')),
                    (7, 'Органическая химия', 'Основы органической химии', 5, datetime('now', '-15 days'), 90, 'Лабораторная работа', datetime('now'), datetime('now'))
            ");

            // Добавляем задания только если их нет
            migrationBuilder.Sql(@"
                INSERT OR IGNORE INTO ""Assignments"" (""Id"", ""Title"", ""Description"", ""CourseId"", ""DueDate"", ""MaxScore"", ""CreatedAt"", ""UpdatedAt"")
                VALUES 
                    (1, 'Контрольная работа по алгебре', 'Решить 10 задач по алгебре', 1, datetime('now', '+7 days'), 100, datetime('now'), datetime('now')),
                    (2, 'Лабораторная работа по физике', 'Измерение ускорения свободного падения', 2, datetime('now', '+5 days'), 50, datetime('now'), datetime('now')),
                    (3, 'Реферат по истории', 'Написать реферат о Куликовской битве', 3, datetime('now', '+10 days'), 80, datetime('now'), datetime('now')),
                    (4, 'Анализ стихотворения', 'Анализ стихотворения Пушкина', 4, datetime('now', '+8 days'), 60, datetime('now'), datetime('now')),
                    (5, 'Практическая работа по химии', 'Определение pH растворов', 5, datetime('now', '+6 days'), 70, datetime('now'), datetime('now'))
            ");

            // Добавляем записи на курсы только если их нет
            migrationBuilder.Sql(@"
                INSERT OR IGNORE INTO ""CourseEnrollments"" (""Id"", ""StudentId"", ""CourseId"", ""EnrolledAt"", ""IsActive"")
                VALUES 
                    (1, 4, 1, datetime('now', '-30 days'), 1),
                    (2, 4, 2, datetime('now', '-30 days'), 1),
                    (3, 5, 1, datetime('now', '-30 days'), 1),
                    (4, 5, 3, datetime('now', '-30 days'), 1),
                    (5, 6, 2, datetime('now', '-30 days'), 1),
                    (6, 6, 4, datetime('now', '-30 days'), 1),
                    (7, 7, 1, datetime('now', '-30 days'), 1),
                    (8, 7, 5, datetime('now', '-30 days'), 1),
                    (9, 8, 3, datetime('now', '-30 days'), 1),
                    (10, 8, 4, datetime('now', '-30 days'), 1)
            ");

            // Добавляем сдачи заданий только если их нет
            migrationBuilder.Sql(@"
                INSERT OR IGNORE INTO ""AssignmentSubmissions"" (""Id"", ""AssignmentId"", ""StudentId"", ""Content"", ""FileUrl"", ""SubmittedAt"", ""Score"", ""Feedback"", ""GradedAt"", ""GradedBy"")
                VALUES 
                    (1, 1, 4, 'Решил все задачи. Ответы: 1) x=5, 2) x=-3, 3) x=2', NULL, datetime('now', '-2 days'), 85, 'Хорошая работа, но есть ошибки в задачах 3 и 7', datetime('now', '-1 days'), 2),
                    (2, 2, 6, 'Измерения проведены. g = 9.8 м/с²', 'files/lab_report.pdf', datetime('now', '-1 days'), 45, 'Отлично! Все измерения точные', datetime('now'), 2),
                    (3, 3, 8, 'Реферат о Куликовской битве готов', 'files/essay_history.pdf', datetime('now', '-3 days'), 75, 'Хороший реферат, но нужно больше источников', datetime('now', '-1 days'), 3),
                    (4, 4, 6, 'Анализ стихотворения ''Я помню чудное мгновенье''', 'files/poetry_analysis.pdf', datetime('now', '-1 days'), 55, 'Глубокий анализ, но можно добавить больше деталей', datetime('now'), 3),
                    (5, 5, 7, 'Практическая работа выполнена', 'files/chemistry_lab.pdf', datetime('now', '-2 days'), 65, 'Хорошая работа, все опыты проведены правильно', datetime('now', '-1 days'), 3)
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Удаляем данные в обратном порядке
            migrationBuilder.Sql("DELETE FROM \"AssignmentSubmissions\" WHERE \"Id\" IN (1, 2, 3, 4, 5)");
            migrationBuilder.Sql("DELETE FROM \"CourseEnrollments\" WHERE \"Id\" IN (1, 2, 3, 4, 5, 6, 7, 8, 9, 10)");
            migrationBuilder.Sql("DELETE FROM \"Assignments\" WHERE \"Id\" IN (1, 2, 3, 4, 5)");
            migrationBuilder.Sql("DELETE FROM \"Lessons\" WHERE \"Id\" IN (1, 2, 3, 4, 5, 6, 7)");
            migrationBuilder.Sql("DELETE FROM \"Books\" WHERE \"Id\" IN (1, 2, 3, 4, 5)");
            migrationBuilder.Sql("DELETE FROM \"Courses\" WHERE \"Id\" IN (1, 2, 3, 4, 5)");
            migrationBuilder.Sql("DELETE FROM \"Users\" WHERE \"Id\" IN (1, 2, 3, 4, 5, 6, 7, 8)");
            migrationBuilder.Sql("DELETE FROM \"Roles\" WHERE \"Id\" IN (1, 2, 3)");
        }
    }
}
