using craft.Users;

namespace craft.FileProvider;

public class MountPointFileProvider : IFileProvider
{
    public string? MountPoint { get; set; } = "";
    private CraftUser? CraftUser { get; set; } = null;
    private PermissionManager permissionManager { get; set; }
    private FileProviderManager fileProviderManager { get; set; }
    
    public MountPointFileProvider(CraftUser craftUser, PermissionManager permissionManager,FileProviderManager fileProviderManager)
    {
        this.CraftUser = craftUser;
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
        if (this.CraftUser == null) return new List<CraftFile>();
        // Get user permissions
        List<CraftPermission> userPermissions = permissionManager.GetPermissionsForUser(CraftUser);
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