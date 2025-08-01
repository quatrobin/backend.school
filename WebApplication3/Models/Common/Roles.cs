namespace WebApplication3.Models.Common;

public static class Roles
{
    public const string Student = "Студент";
    public const string Teacher = "Преподаватель";
    public const string Admin = "Администратор";
    
    public static readonly string[] AllRoles = { Student, Teacher, Admin };
} 