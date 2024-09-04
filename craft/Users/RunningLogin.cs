namespace craft.Users;

public class RunningLogin
{
    public string userUuid { get; set; }
    public string nonce { get; set; }
    public string challengeId { get; set; }
}