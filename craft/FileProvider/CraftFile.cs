using ComputerUtils.VarUtils;

namespace craft.FileProvider;

public class CraftFile
{
    public Stream? requestedStream;
    
    public string name { get; set; }
    public string path { get; set; }
    public string realPath;

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

    public long size { get; set; }

    public string sizeString
    {
        get
        {
            return SizeConverter.ByteSizeToString(size);
        }
    }
    public DateTime lastModified { get; set; }
    public DateTime created { get; set; }
    public bool isDirectory { get; set; }
    public bool isFileProviderRoot { get; set; }
}