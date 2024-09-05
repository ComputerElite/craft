using craft.Users;

namespace craft.FileProvider;

public class MountPointFileProvider : IFileProvider
{
    public string? MountPoint { get; set; } = "";
    private User? user { get; set; } = null;
    private PermissionManager permissionManager { get; set; }
    private FileProviderManager fileProviderManager { get; set; }
    
    public MountPointFileProvider(User user, PermissionManager permissionManager,FileProviderManager fileProviderManager)
    {
        this.user = user;
        this.permissionManager = permissionManager;
        this.fileProviderManager = fileProviderManager;
    }
    
    public CraftFile? GetReadStream(string path)
    {
        return null;
    }

    public CraftFile? GetWriteStream(string path)
    {
        return null;
    }

    public List<CraftFile> GetFilesOfDirectory(string path)
    {
        // Will only return mount points so path does not have to be respected
        if (this.user == null) return new List<CraftFile>();
        // Get user permissions
        List<CraftPermission> userPermissions = permissionManager.GetPermissionsForUser(user);
        // Get all root level permissions
        List<CraftFile> files = new List<CraftFile>();
        foreach(CraftPermission permission in userPermissions)
        {
            if (permission.path == "/")
            {
                // All mount points are allowed, get them all
                return fileProviderManager.fileSystemFileProviders.ConvertAll<CraftFile>(x =>
                    new CraftFile
                    {
                        path = x.MountPoint ?? "",
                        isDirectory = true,
                        isFileProviderRoot = true
                    }
                );
            }
            string mountPoint = permission.path.Split("/")[1];
            if (files.Any(x => x.path == mountPoint)) continue; // already exists
            files.Add(new CraftFile
            {
                path = "/" + mountPoint + "/",
                isDirectory = true,
                isFileProviderRoot = true
            });
        }
        return files;
    }

    public CraftFile? GetFile(string path)
    {
        return null;
    }
}