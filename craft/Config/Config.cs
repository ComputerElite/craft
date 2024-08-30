using System.Text.Json;
using craft.FileProvider;
using craft.Users;

namespace craft.Config;

public class Config
{
    public List<FileSystemFileProvider> FileSystemFileProviders { get; set; } = new();
    public List<User> Users { get; set; } = new();
    public List<CraftPermission> Permissions { get; set; } = new();

    public static Config? Instance;

    private static readonly string _configPath = "config.json";
    public static void LoadConfig()
    {
        if(!File.Exists(_configPath))
        {
            Instance = new Config();
            SaveConfig();
            return;
        }
        Instance = JsonSerializer.Deserialize<Config>(File.ReadAllText(_configPath));
    }

    public static void SaveConfig()
    {
        File.WriteAllText(_configPath, JsonSerializer.Serialize(Instance));
    }
}