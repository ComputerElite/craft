using craft.DB;
using craft.FileProvider;
using craft.Server;
using craft.Users;

namespace craft;

public class Initializor
{
    private WebServer _server;
    private DBClient _dbClient;
    private FileProviderManager _fileProviderManager;
    private UserManager _userManager;
    private PermissionManager _permissionManager;
    
    public Initializor()
    {
        Config.Config.LoadConfig();
        _userManager = new UserManager();
        _permissionManager = new PermissionManager();
        _fileProviderManager = new FileProviderManager(_permissionManager);
        _server = new WebServer(_fileProviderManager, _userManager);
        _dbClient = new DBClient();
    }
    
    public void Start()
    {
        _server.Start();
    }
}