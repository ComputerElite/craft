namespace craft.Users;

public class CraftPermission
{
    public string user { get; set; }
    public CraftPermissionType type { get; set; }
    public string path { get; set; }
    public bool isSharedFilePermission { get; set; }
}