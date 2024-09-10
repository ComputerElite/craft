using System.ComponentModel.DataAnnotations;

namespace craft.Users;

public class CraftUser
{

    [Key]
    public string uuid { get; set; }
    public string? username { get; set; }
    public string? password { get; set; }
    public bool isPublicLinkUser { get; set; } = false;
    public bool TwoFactorEnabled { get; set; } = false;
    
    public static CraftUser GuestCraftUser = new CraftUser()
    {
        uuid = "guest",
        username = "guest",
        password = "test",
    };
    public static CraftUser DefaultAdminUser = new CraftUser()
    {
        uuid = "root",
        username = "root",
        password = "root"
    };
}