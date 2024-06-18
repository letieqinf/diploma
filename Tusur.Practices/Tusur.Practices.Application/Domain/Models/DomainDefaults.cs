namespace Tusur.Practices.Application.Domain.Models
{
    public class DomainDefaults
    {
        public static List<string> DefaultRoles { get; } = new()
        {
            "user", "student", "teacher", "secretary", "education"
        };

        public static string User { get; } = DefaultRoles[0];
        public static string Student { get => DefaultRoles[1]; }
        public static string Teacher { get => DefaultRoles[2]; }
        public static string Secretary { get => DefaultRoles[3]; }
        public static string Education { get => DefaultRoles[4]; }
    }
}
