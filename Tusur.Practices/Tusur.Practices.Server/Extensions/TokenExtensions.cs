namespace Tusur.Practices.Server.Extensions
{
    public static class TokenExtensions
    {
        public static string GetTokenFromHeaders(this HttpRequest request)
        {
            var header = request.Headers.Authorization.ToString();
            if (header is null)
                throw new ArgumentException();

            var jwt = header.Split(' ')[1];
            return jwt;
        }
    }
}
