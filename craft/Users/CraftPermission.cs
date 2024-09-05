namespace craft.Users;

public class CraftPermission
{
    public string userUuid { get; set; }
    public CraftPermissionType type { get; set; }
    public string path { get; set; }
    public bool isSharedFilePermission { get; set; }

    public override string ToString()
    {
        return $"User: {userUuid}, Type: {type}, Path: {path}";
    }
}