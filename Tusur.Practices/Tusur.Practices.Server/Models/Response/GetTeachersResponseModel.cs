namespace Tusur.Practices.Server.Models.Response
{
    public class GetTeachersResponseModel
    {
        public Guid Id { get; set; }
        public Guid TeacherId { get; set; }
        public Guid DepartmentId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string? Patronymic { get; set; }
        public string Email { get; set; }
    }
}
