using System.ComponentModel.DataAnnotations;

namespace craft.FileProvider;

/// <summary>
/// Provides Files directly from the FileSystem
/// </summary>
public class FileSystemFileProvider : IFileProvider
{
    [Key]
    public string? id { get; set; }
    public string? rootPath { get; set; }
    public string? mountPoint { get; set; }
    string GetRealPath(string path)
    {
        
        return Path.Join(rootPath, path.Replace(mountPoint, ""));
    }


    /// <summary>
    /// Returns a Stream to read a file from the FileSystem
    /// </summary>
    /// <returns>Stream which reads the file</returns>
    public CraftFile? GetReadStream(string path)
    {
        CraftFile? f = GetFile(path);
        if (f == null) return null;
        f.requestedStream = File.OpenRead(f.realPath);
        return f;
    }

    /// <summary>
    /// Returns a Stream to write to a file from the FileSystem
    /// </summary>
    /// <returns>Stream which writes to the file</returns>
    public CraftFile? GetWriteStream(string path)
    {
        CraftFile? f = GetFile(path);
        if (f != null) return null;
        f = new CraftFile();
        f.realPath = GetRealPath(path);
        if(!Directory.Exists(Path.GetDirectoryName(f.realPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(f.realPath));
        }
        f.requestedStream = File.OpenWrite(f.realPath);
        return f;
    }

    /// <summary>
    /// Returns a List of everything within a directory
    /// </summary>
    /// <returns>List of CRAFT Files</returns>
    public List<CraftFile> GetFilesOfDirectory(string path)
    {
        string realPath = GetRealPath(path);
        if(!Directory.Exists(realPath)) return new List<CraftFile>();
        DirectoryInfo info = new DirectoryInfo(realPath);
        List<CraftFile> files = new List<CraftFile>();
        files.Add(new CraftFile
        {
            path = FileProviderManager.GetParent(path),
            isDirectory = true,
            displayName = ".."
        });
        foreach (DirectoryInfo directory in info.GetDirectories())
        {
            CraftFile craftFile = new CraftFile();
            craftFile.path = Path.Join(path, directory.Name) +"/";
            craftFile.realPath = directory.FullName;
            craftFile.size = 0;
            craftFile.lastModified = directory.LastWriteTime.ToUniversalTime();
            craftFile.created = directory.CreationTime.ToUniversalTime();
            craftFile.isDirectory = true;
            craftFile.isFileProviderRoot = false;
            files.Add(craftFile);
        }
        foreach (var file in info.GetFiles())
        {
            CraftFile craftFile = new CraftFile();
            craftFile.path = Path.Join(path, file.Name);
            craftFile.realPath = file.FullName;
            craftFile.size = file.Length;
            craftFile.lastModified = file.LastWriteTime.ToUniversalTime();
            craftFile.created = file.CreationTime.ToUniversalTime();
            craftFile.isDirectory = false;
            craftFile.isFileProviderRoot = false;
            files.Add(craftFile);
        }

        return files;
    }

    /// <summary>
    /// Returns a List of everything within a directory
    /// </summary>
    /// <returns>List of CRAFT Files</returns>
    public CraftFile? GetFile(string path)
    {
        string realPath = GetRealPath(path);
        if (!File.Exists(realPath) && !Directory.Exists(realPath)) return null;
        FileInfo info = new FileInfo(realPath);
        CraftFile file = new CraftFile();
        file.realPath = realPath;
        file.path = path;
        file.size = info.Length;
        file.lastModified = info.LastWriteTime.ToUniversalTime();
        file.created = info.CreationTime.ToUniversalTime();
        file.isDirectory = info.Attributes.HasFlag(FileAttributes.Directory);
        file.isFileProviderRoot = path == "/";
        return file;
    }
}