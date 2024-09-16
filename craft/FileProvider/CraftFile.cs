using System.ComponentModel.DataAnnotations;
using ComputerUtils.VarUtils;

namespace craft.FileProvider;

public class CraftFile
{
    [Key]
    public string uuid { get; set; }
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
    public string? fileType
    {
        get
        {
            if (isDirectory) return CraftFileType.Directory;
            return CraftFileType.fileTypeLoopup.TryGetValue(extension, out var value) ? value : CraftFileType.Unknown;
        }
    }
    public string? displayName;

    public string containedIn
    {
        get
        {
            // get parent directory
            return FileProviderManager.GetParent(path);
        }
    }
}

public class CraftFileType
{
    public const string Image = "image";
    public const string Audio = "audio";
    public const string Video = "video";
    public const string DiskImage = "diskImage";
    public const string TextFile = "textFile";
    public const string Archive = "archive";
    public const string Document = "document";
    public const string Website = "website";
    public const string Markdown = "markdown";
    public const string Directory = "directory";
    public const string? Unknown = null;

    public static readonly Dictionary<string, string?> fileTypeLoopup = new Dictionary<string, string?>
    {
        { "png", Image },
        { "jpg", Image },
        { "jpeg", Image },
        { "gif", Image },
        { "bmp", Image },
        { "tiff", Image },
        { "webp", Image },
        { "mp3", Audio },
        { "ogg", Audio },
        { "wav", Audio },
        { "flac", Audio },
        { "mp4", Video },
        { "webm", Video },
        { "mkv", Video },
        { "mov", Video },
        { "iso", DiskImage },
        { "img", DiskImage },
        { "txt", TextFile },
        { "log", TextFile },
        { "md", Markdown },
        { "zip", Archive },
        { "rar", Archive },
        { "7z", Archive },
        { "pdf", Document },
        { "html", Website },
        { "css", Website },
        { "js", Website }
    };

}