using System.Text.Json;
using ComputerUtils.Logging;
using craft.FileProvider;

namespace craft.Users;

public class PermissionManager
{
    public List<CraftPermission> permissions { get; set; }
    
    public PermissionManager()
    {
        permissions = new List<CraftPermission>();
        permissions = Config.Config.Instance.Permissions;
    }

    public bool HasPermission(string filePath, User user, CraftPermissionType type)
    {
        Logger.Log(permissions.Count + " permissions found");
        CraftPermission? foundPermission = this.permissions.Where(p => filePath.StartsWith(p.path) && p.user == user.uuid && p.type >= type).MaxBy(x => x.path.Length);
        return foundPermission != null;
    }
}