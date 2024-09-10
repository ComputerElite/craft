using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace craft.Users;

public class CraftUserSession
{
    public string userUuid { get; set; }
    [JsonIgnore]
    [Key]
    public string sessionId { get; set; }
    public DateTime lastAccess { get; set; }
    public string? origin { get; set; }
    public DateTime creationDate { get; set; }
    public DateTime validUnti { get; set; }
}