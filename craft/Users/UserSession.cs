namespace craft.Users;

public class UserSession
{
    public string session { get; set; }
    public DateTime lastAccess { get; set; }
    public string origin { get; set; }
    public DateTime creationDate { get; set; }
}