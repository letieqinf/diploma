namespace Tusur.Practices.Server.Models.Request
{
    public class CreateSupervisorRequestModel
    {
        public Guid TeacherId { get; set; }
        public Guid GroupId { get; set; }
        public Guid PracticeDateId { get; set; }
    }
}
