namespace Tusur.Practices.Server.Models.Response
{
    public class GetApplicationResponseModel
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid PracticeDateId { get; set; }
        public Guid? OrganizationId { get; set; }
        public ushort Status { get; set; }
    }
}
