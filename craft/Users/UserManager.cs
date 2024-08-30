namespace craft.Users;

public class UserManager
{
    public User? GetUserBySession(string session)
    {
        return User.GuestUser;
    }
}