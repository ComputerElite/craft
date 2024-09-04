using craft.Server.ApiTypes;

namespace craft.Users;

public class UserManager
{
    public List<RunningLogin> runningLogins = new List<RunningLogin>();
    public UserManager()
    {
        
    }
    public User? GetUserBySession(string session)
    {
        return User.GuestUser;
    }
    public User? GetUserByUUID(string uuid)
    {
        return User.GuestUser;
    }
    public User? GetUserByUsername(string username)
    {
        if (username != User.GuestUser.username) return null;
        return User.GuestUser;
    }

    public LoginResponse InitiateLogin(string username)
    {
        User? u = GetUserByUsername(username);
        if(u == null)
        {
            return new LoginResponse { error = "Invalid username or password" };
        }
        RunningLogin rl = new RunningLogin
        {
            userUuid = u.uuid,
            nonce = CryptographicsHelper.GetRandomString(),
            challengeId = Guid.NewGuid().ToString()
        };
        // remove previous login attempts for this user
        runningLogins.RemoveAll(x => x.userUuid == u.uuid);
        // add current login attempt
        runningLogins.Add(rl);
        return new LoginResponse()
        {
            nonce = rl.nonce.ToString(),
            challengeId = rl.challengeId,
            success = true
        };
    }

    public LoginResponse Login(LoginRequest request)
    {
        RunningLogin? rl = runningLogins.FirstOrDefault(x => x.challengeId == request.challengeId);
        if(rl == null)
        {
            return new LoginResponse { error = "Challenge with this id not found" };
        }
        runningLogins.Remove(rl);
        User? u = GetUserByUUID(rl.userUuid);
        if(u == null)
        {
            return new LoginResponse { error = "User associated with challenge not found" };
        }
        
        // hash password
        string passwordHash = CryptographicsHelper.GetHash(u.password + rl.nonce + request.cnonce);
        if (passwordHash.ToLower() != request.passwordHash.ToLower())
        {
            return new LoginResponse { error = "Password incorrect" };
        }
        
        if (u.TwoFactorEnabled)
        {
            return new LoginResponse { success = true, requires2fa = true };
        }
        
        // ToDo, create session for user

        return new LoginResponse()
        {
            success = true
        };
    }
}