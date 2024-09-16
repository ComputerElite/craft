using System.Text.Json;
using craft.FileProvider;
using craft.Users;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace craft.Config;

public class Config
{

    public static Config? Instance;
    public string dbConnectionString { get; set; } = "Host=localhost;Port=5432;Database=craft;Username=craft;Password=craft";
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