namespace craft.Users;

public class User
{
    public string? uuid { get; set; }
    public string? username { get; set; }
    public string? password { get; set; }
    public bool isPublicLinkUser { get; set; }

    public static User GuestUser = new User()
    {
        uuid = "guest",
        username = "Guest",
        password = "",
    };
}