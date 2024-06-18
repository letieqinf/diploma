namespace Tusur.Practices.Server.Models.Request
{
    public class SubmitContractRequestModel
    {
        public Guid ContractId { get; set; }
        public IEnumerable<SubmitContractStudentDate> StudentDates { get; set; }
    }

    public class SubmitContractStudentDate
    {
        public Guid StudentId { get; set; }
        public Guid PracticeDateId { get; set; }
    }
}
