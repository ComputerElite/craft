using System.Net;
using System.Text;
using System.Text.Json;
using ComputerUtils.Logging;
using ComputerUtils.Webserver;
using craft.FileProvider;
using craft.Server.ApiTypes;
using craft.Users;

namespace craft.Server;

public class WebServer
{
    private HttpServer _server;
    private FileProviderManager _fileProviderManager;
    private UserManager _userManager;
    
    public WebServer(FileProviderManager fileProviderManager, UserManager userManager)
    {
        _server = new HttpServer();
        _fileProviderManager = fileProviderManager;
        _userManager = userManager;
    }
    
    public User? GetUserBySession(ServerRequest request)
    {
        Cookie? sessionCookie = request.cookies["session"];
        if(sessionCookie == null) return null;
        string session = sessionCookie.Value;
        User? u = _userManager.GetUserBySession(session);
        return u;
    }
    
    public void DoGetFile(ServerRequest request, string path)
    {
        path = Path.GetFullPath(path);
        User? u = GetUserBySession(request);
        if (u == null)
        {
            ApiError.SendUnauthorized(request);
            return;
        }

        IFileProvider? fp = _fileProviderManager.GetFileProvider(path, u);
        if(fp == null)
        {
            ApiError.SendUnauthorized(request);
            return;
        }
        CraftFile? s = fp.GetReadStream(path);
        if(s == null || s.requestedStream == null)
        {
            ApiError.SendNotFound(request);
            return;
        }


        request.ForwardStream(s.requestedStream, s.requestedStream.Length, HttpServer.GetContentTpe("file." + s.extension),
            Encoding.Default, 200, true, new Dictionary<string, string>
            {
                {"Content-Disposition", "attachment; filename=\"" + s.name + "\""}
            });
    }
    
    public void DoGetFileMeta(ServerRequest request, string path)
    {
        path = Path.GetFullPath(path);
        User? u = GetUserBySession(request);
        if (u == null)
        {
            ApiError.SendUnauthorized(request);
            return;
        }

        IFileProvider? fp = _fileProviderManager.GetFileProvider(path, u);
        if(fp == null)
        {
            ApiError.SendUnauthorized(request);
            return;
        }
        CraftFile? s = fp.GetFile(path);
        if(s == null || s.requestedStream == null)
        {
            ApiError.SendNotFound(request);
            return;
        }


        request.ForwardStream(s.requestedStream, s.requestedStream.Length, HttpServer.GetContentTpe("file." + s.extension),
            Encoding.Default, 200, true, new Dictionary<string, string>
            {
                {"Content-Disposition", "attachment; filename=\"" + s.name + "\""}
            });
    }
    
    public void DoGetFiles(ServerRequest request, string path)
    {
        path = Path.GetFullPath(path);
        User? u = GetUserBySession(request);
        if (u == null)
        {
            ApiError.SendUnauthorized(request);
            return;
        }

        IFileProvider? fp = _fileProviderManager.GetFileProvider(path, u);
        if(fp == null)
        {
            ApiError.SendUnauthorized(request);
            return;
        }
        if (!_fileProviderManager.CheckPermission(path, u, CraftPermissionType.Read))
        {
            Logger.Log("User '" + u.username +  "' does not have permission to read " + path);
            ApiError.SendNotFound(request);
            return;
        }
        List<CraftFile> files = fp.GetFilesOfDirectory(path);
        request.SendString(JsonSerializer.Serialize(files), "application/json", 200);
    }
    
    public void Start()
    {
        _server.AddRoute("GET", "/api/v1/list_dir", request =>
        {
            string? path = request.queryString.Get("path");
            if(path == null)
            {
                ApiError.SendMissingKey("path", request);
                return true;
            }

            DoGetFiles(request, path);
            return true;
        });
        _server.AddRoute("GET", "/api/v1/file", request =>
        {
            string? path = request.queryString.Get("path");
            if(path == null)
            {
                ApiError.SendMissingKey("path", request);
                return true;
            }

            DoGetFile(request, path);
            return true;
        });
        _server.AddRoute("GET", "/api/v1/file_meta", request =>
        {
            string? path = request.queryString.Get("path");
            if(path == null)
            {
                ApiError.SendMissingKey("path", request);
                return true;
            }

            DoGetFileMeta(request, path);
            return true;
        });
        _server.AddRoute("POST", "/api/v1/user/login", request =>
        {
            LoginRequest? r = null;
            try
            {
                r = JsonSerializer.Deserialize<LoginRequest>(request.bodyString);
            } catch(Exception e)
            {
                ApiError.MalformedRequest(request);
            }
            if(r == null)
            {
                ApiError.MalformedRequest(request);
                return true;
            }
            LoginResponse response = _userManager.Login(r);
            request.SendString(JsonSerializer.Serialize(response), "application/json", 200);
            return true;
        });
        _server.AddRoute("POST", "/api/v1/user/start_login", request =>
        {
            LoginRequest? r = null;
            try
            {
                r = JsonSerializer.Deserialize<LoginRequest>(request.bodyString);
            } catch(Exception e)
            {
                ApiError.MalformedRequest(request);
            }
            if(r == null)
            {
                ApiError.MalformedRequest(request);
                return true;
            }
            LoginResponse response = _userManager.InitiateLogin(r.username);
            request.SendString(JsonSerializer.Serialize(response), "application/json", 200);
            return true;
        });
        _server.StartServer(8383);    
    }
}