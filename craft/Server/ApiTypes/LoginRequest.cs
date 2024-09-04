namespace craft.Server.ApiTypes;

public class LoginRequest
{
    public string? username { get; set; }
    public string challengeId { get; set; }
    public string? passwordHash { get; set; }
    public string? cnonce { get; set; }
}

public class LoginResponse
{
    public bool? requires2fa { get; set; }
    public string? nonce { get; set; }
    public string? session { get; set; }
    public string? error { get; set; }
    public bool success { get; set; } = false;
    public string? challengeId { get; set; }
}