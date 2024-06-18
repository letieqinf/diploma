namespace Tusur.Practices.Server.Models.Response
{
    public class LoginResponseModel
    {
        public string AccessToken { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
