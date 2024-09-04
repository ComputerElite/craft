namespace craft.Users;

public class Challenge
{
    public string userUuid { get; set; }
    public string nonce { get; set; }
    public string challengeId { get; set; }
    public ChallengeType type { get; set; }
}

public enum ChallengeType
{
    Password,
    TOTP
}