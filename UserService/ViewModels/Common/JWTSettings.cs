namespace UserService.ViewModels.Common
{
    public class JWTSettings
    {
        public string Issuer { get; set; } = null!;
        public string Key { get; set; } = null!;
        public string ExprationInMin { get; set; } = null!;
        public string Audience { get; set; } = null!;
    }
}
