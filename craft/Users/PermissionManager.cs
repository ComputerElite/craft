using System.Text.Json;
using ComputerUtils.Logging;
using craft.DB;
using craft.FileProvider;

namespace craft.Users;

public class PermissionManager
{
    
    public PermissionManager()
    {
        
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
        if(FileProviderManager.IsRoot(path) && type == CraftPermissionType.Read)
        {
            Logger.Log("Root path, no permission needed");
            return true;
        }

        using (CraftDbContext c = new())
        {
            CraftPermission? foundPermission = c.permissions.Where(p => path.StartsWith(p.path) && p.userUuid == craftUser.uuid && p.type >= type).OrderByDescending(x => x.path.Length).FirstOrDefault();
            
            return foundPermission != null;
        }
    }
    
    
    /// <summary>
    /// Checks if a user has a permission
    /// </summary>
    /// <param name="path">path to check for</param>
    /// <param name="craftUser">user to check for</param>
    /// <param name="type">check if permission is greater than type</param>
    /// <returns></returns>
    public bool HasPermission(CraftUser craftUser, CraftPermissionType type)
    {
        using (CraftDbContext c = new())
        {
            CraftPermission? foundPermission = c.permissions.FirstOrDefault(p => p.userUuid == craftUser.uuid && p.type == type);
            return foundPermission != null;
        }
    }

    /// <summary>
    /// Gets all permissions for a user that are greater than read
    /// </summary>
    /// <param name="craftUser"></param>
    /// <returns></returns>
    public List<CraftPermission> GetPermissionsForUser(CraftUser craftUser)
    {
        using (CraftDbContext c = new())
        {
            return c.permissions.Where(x => x.userUuid == craftUser.uuid && x.type > CraftPermissionType.Read).ToList();
        }
    }
}