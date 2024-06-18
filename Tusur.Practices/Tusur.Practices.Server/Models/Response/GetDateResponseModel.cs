namespace Tusur.Practices.Server.Models.Response
{
    public class GetDateResponseModel : GetPracticeResponseModel
    {
        public DateOnly StartsAt { get; set; }
        public DateOnly EndsAt { get; set; }
    }
}
