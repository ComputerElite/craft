using ComputerUtils.Logging;
using craft.Users;

namespace craft.FileProvider;

/// <summary>
/// Resolves paths to a specific FileProvider and checks permissions of the User before sending a file
/// </summary>
public class FileProviderManager
{
    public List<FileSystemFileProvider> fileSystemFileProviders;
    private PermissionManager _permissionManager;
    
    public FileProviderManager(PermissionManager permissionManager)
    {
        fileSystemFileProviders = new List<FileSystemFileProvider>();
        fileSystemFileProviders.AddRange(Config.Config.Instance.FileSystemFileProviders);
        _permissionManager = permissionManager;
    }

    public IFileProvider? GetFileProvider(string path, CraftUser craftUser)
    {
        Logger.Log("Trying to get file provider for " + path);
        if(!path.Contains("/")) return null;
        string[] pathParts = path.Split("/");
        if (pathParts.Length < 2)
        {
            return null;
        }
        if (IsRoot(path))
        {
            // MountPoint provider
            Logger.Log("Providing MountPoint provider");
            return new MountPointFileProvider(craftUser, _permissionManager, this);
        }
        string providerMountPoint = "/" + pathParts[1] + "/";
        Logger.Log("MountPoint: " + providerMountPoint);
        IFileProvider? fp = fileSystemFileProviders.Find(fp => fp.MountPoint == providerMountPoint);
        Logger.Log("Found provider: " + (fp != null));
        return fp;
    }

    public static bool IsRoot(string path)
    {
        return path == "/";
    }

    public bool CheckPermission(CraftFile craftFile, CraftUser craftUser, CraftPermissionType permissionType)
    {
        return _permissionManager.HasPermission(craftFile.path, craftUser, permissionType);
    }
    public bool CheckPermission(string path, CraftUser craftUser, CraftPermissionType permissionType)
    {
        return _permissionManager.HasPermission(path, craftUser, permissionType);
    }

    public static string GetParent(string path)
    {
        return path.Substring(0, path.Substring(0, path.Length - 1).LastIndexOf("/") + 1);
    }
}