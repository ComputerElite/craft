using ComputerUtils.Logging;
using craft.DB;
using craft.FileProvider;
using craft.Server;
using craft.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace craft;

public class Initializor
{
    private WebServer _server;
    private CraftDbContext _dbClient;
    private FileProviderManager _fileProviderManager;
    private UserManager _userManager;
    private PermissionManager _permissionManager;
    
    public Initializor()
    {
        Logger.displayLogInConsole = true;
        Config.Config.LoadConfig();
        Logger.Log(Config.Config.Instance.dbConnectionString);
        _userManager = new UserManager();
        _permissionManager = new PermissionManager();
        _fileProviderManager = new FileProviderManager(_permissionManager);
        _server = new WebServer(_fileProviderManager, _userManager);
        _dbClient = new CraftDbContext();
    }
    
    public void Start()
    {
        _userManager.CreateDefaultUserIfNotExists();
        _server.Start();
    }
}