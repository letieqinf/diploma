namespace Tusur.Practices.Server.Models.Request
{
    public class CreateApplicationRequestModel
    {
        public Guid? OrganizationId { get; set; } = null;
        public string? OrganizationName { get; set; } = null;
        public string? OrganizationAddress { get; set; } = null;
        public Guid PracticeDateId { get; set; }
        public bool IsDraft { get; set; }
    }
}
