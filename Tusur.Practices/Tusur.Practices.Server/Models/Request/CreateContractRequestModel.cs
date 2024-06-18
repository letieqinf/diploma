using Tusur.Practices.Server.Models.Response;

namespace Tusur.Practices.Server.Models.Request
{
    public class CreateContractRequestModel
    {
        public Guid OrganizationId { get; set; }
        public List<StudentDate> StudentDates { get; set; }
        public bool IsDraft { get; set; }
    }

    public class StudentDate
    {
        public Guid StudentId { get; set; }
        public Guid PracticeDateId { get; set; }
    }
}
