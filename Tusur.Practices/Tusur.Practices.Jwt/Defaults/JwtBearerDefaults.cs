namespace Tusur.Practices.Jwt.Defaults
{
    public static class JwtBearerDefaults
    {
        public const string Issuer = "Tusur.Practices.Server.User";
        public const string Audience = "Tusur.Practices.Server";
        public const string TokenSecret = "asdoiAHHDSAsjdf4QX3s12tuas122ur7";
        public const int TokenExpiresInMinutes = 15;
        public const int RefreshTokenExpiresInDays = 14;
    }
}
