namespace Tusur.Practices.Server.Models.Response
{
    public class GetProfilesResponse
    {
        public Guid ContractId { get; set; }
        public IEnumerable<GetProfileStudentDate> StudentDates { get; set; }
        public ushort Status { get; set; }
    }

    public class GetProfileStudentDate
    {
        public Guid StudentId { get; set; }
        public Guid PracticeDateId { get; set; }
    }
}
