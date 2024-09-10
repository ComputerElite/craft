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

    /// <summary>
    /// Checks if a user has permissions to access a path
    /// </summary>
    /// <param name="path">path to check for</param>
    /// <param name="craftUser">user to check for</param>
    /// <param name="type">check if permission is greater than type</param>
    /// <returns></returns>
    public bool HasPermission(string path, CraftUser craftUser, CraftPermissionType type)
    {
        permissions.ForEach(x => Logger.Log(x.ToString()));
        if(FileProviderManager.IsRoot(path) && type == CraftPermissionType.Read)
        {
            return true;
        }
        CraftPermission? foundPermission = this.permissions.Where(p => path.StartsWith(p.path) && p.userUuid == craftUser.uuid && p.type >= type).MaxBy(x => x.path.Length);
        return foundPermission != null;
    }

    /// <summary>
    /// Gets all permissions for a user that are greater than read
    /// </summary>
    /// <param name="craftUser"></param>
    /// <returns></returns>
    public List<CraftPermission> GetPermissionsForUser(CraftUser craftUser)
    {
        return this.permissions.Where(x => x.userUuid == craftUser.uuid&&x.type > CraftPermissionType.Read).ToList();
    }
}