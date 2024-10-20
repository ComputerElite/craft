namespace craft.FileProvider;

public interface IFileProvider
{
    public string? mountPoint { get; set; }
    public CraftFile? GetReadStream(string path);
    public CraftFile? GetWriteStream(string path);
    public List<CraftFile> GetFilesOfDirectory(string path);
    public CraftFile? GetFile(string path);
}