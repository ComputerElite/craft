using craft.Users;

namespace craft.FileProvider;

/// <summary>
/// Resolves paths to a specific FileProvider and checks permissions of the User before sending a file
/// </summary>
public class FileProviderManager
{
    private List<FileSystemFileProvider> _fileSystemFileProviders;
    private PermissionManager _permissionManager;
    
    public FileProviderManager(PermissionManager permissionManager)
    {
        _fileSystemFileProviders = new List<FileSystemFileProvider>();
        _fileSystemFileProviders.AddRange(Config.Config.Instance.FileSystemFileProviders);
        _permissionManager = permissionManager;
    }

    public IFileProvider? GetFileProvider(string path)
    {
        if(!path.Contains("/")) return null;
        string providerMountPoint = "/" + path.Split("/")[1] + "/";
        IFileProvider? fp = _fileSystemFileProviders.Find(fp => fp.MountPoint == providerMountPoint);
        return fp;
    }

    public bool CheckPermission(CraftFile craftFile, User user, CraftPermissionType permissionType)
    {
        return _permissionManager.HasPermission(craftFile.path, user, permissionType);
    }
    public bool CheckPermission(string path, User user, CraftPermissionType permissionType)
    {
        return _permissionManager.HasPermission(path, user, permissionType);
    }
}