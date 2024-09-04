namespace craft.Users;

public class UserSession
{
    public string userUuid { get; set; }
    public string sessionId { get; set; }
    public DateTime lastAccess { get; set; }
    public string origin { get; set; }
    public DateTime creationDate { get; set; }
    public DateTime validUnti { get; set; }
}