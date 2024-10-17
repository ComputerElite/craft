using ComputerUtils.Logging;
using craft.DB;
using craft.FileProvider;
using craft.Users;

namespace craft.Indexing;

public class FileIndexer
{
    private FileProviderManager _fileProviderManager;

    public FileIndexer(FileProviderManager fileProviderManager)
    {
        this._fileProviderManager =fileProviderManager;
    }
    public void IndexDirectory(string path)
    {
        IFileProvider? fp = _fileProviderManager.GetFileProvider(path, CraftUser.DefaultAdminUser);
        if(fp == null) return;
        try
        {
            
            List<CraftFile> files = fp.GetFilesOfDirectory(path);
            Logger.Log("Indexing directory " + path);
            using (CraftDbContext c = new())
            {
                c.files.AddRange(files.ConvertAll(x => { 
                    x.uuid = Guid.NewGuid().ToString();
                    return x;
                }));
                c.SaveChanges();
            }
            foreach (CraftFile craftFile in files)
            {
                if (!craftFile.isDirectory || craftFile.name == "..") continue;
                IndexDirectory(craftFile.path);
            }
        }
        catch (Exception e)
        {
            Logger.Log("Couldn't index directory " + path + ": " + e);
        }
    }

    public void ReindexEverything()
    {
        IndexDirectory("/");
        RecalculateAllDirectorySizes();
    }

    public void RecalculateAllDirectorySizes()
    {
        using (CraftDbContext c = new())
        {
            foreach (CraftFile directory in c.files.Where(x =>x.isDirectory).AsEnumerable())
            {
                directory.size = c.files.Where(x => x.path.StartsWith(directory.path) && !x.isDirectory)
                    .Sum(x => x.size);
                Logger.Log("Size of directory is " + directory.sizeString);
                
            }

            c.SaveChanges();
        }
    }
}