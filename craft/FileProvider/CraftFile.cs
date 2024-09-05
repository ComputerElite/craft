using ComputerUtils.VarUtils;

namespace craft.FileProvider;

public class CraftFile
{
    public Stream? requestedStream;

    public string name
    {
        get
        {
            if(displayName != null) return displayName;
            if(path.EndsWith("/")) return Path.GetFileName(path.Substring(0, path.Length - 1));
            return Path.GetFileName(path);
        }
    }
    public string path { get; set; }
    public string realPath;

    /// <summary>
    /// Extension without dot
    /// </summary>
    public string extension
    {
        get
        {
            int indexOfDot = name.LastIndexOf('.');
            if (indexOfDot == -1)
            {
                return "";
            }
            return name.Substring(indexOfDot + 1);
        }
    }

    public long? size { get; set; }

    public string? sizeString
    {
        get
        {
            if (!size.HasValue) return null;
            return SizeConverter.ByteSizeToString(size.Value);
        }
    }
    public DateTime? lastModified { get; set; }
    public DateTime? created { get; set; }
    public bool isDirectory { get; set; }
    public bool isFileProviderRoot { get; set; }
    public string? displayName;
}