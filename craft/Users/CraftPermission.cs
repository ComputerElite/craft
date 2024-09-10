using System.ComponentModel.DataAnnotations;

namespace craft.Users;

public class CraftPermission
{

    [Key]
    public string permissionUuid { get; set; }
    public string userUuid { get; set; }
    public CraftPermissionType type { get; set; }
    public string path { get; set; }
    public bool isSharedFilePermission { get; set; }

    public static CraftPermission DefaultAdminPermission = new CraftPermission()
    {
        userUuid = CraftUser.DefaultAdminUser.uuid,
        type = CraftPermissionType.Administrator,
        path = "/",
        permissionUuid = Guid.NewGuid().ToString()
    };

    public override string ToString()
    {
        return $"User: {userUuid}, Type: {type}, Path: {path}";
    }
}