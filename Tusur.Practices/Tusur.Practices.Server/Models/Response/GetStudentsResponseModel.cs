namespace Tusur.Practices.Server.Models.Response
{
    public class GetStudentsResponseModel
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string? Patronymic { get; set; }
    }
}
