using craft.Server.ApiTypes;

namespace craft.Users;

public class UserManager
{
    public List<Challenge> runningLogins = new ();
    public List<UserSession> sessions = new ();
    public UserManager()
    {
        
    }
    public User? GetUserBySession(string session)
    {
        UserSession? s = sessions.Find(x => x.sessionId == session);
        if(s == null)
        {
            return null;
        }
        return GetUserByUUID(s.userUuid);
    }
    public User? GetUserByUUID(string uuid)
    {
        if (uuid == User.GuestUser.uuid) return User.GuestUser;
        return null;
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
        Challenge rl = new Challenge
        {
            userUuid = u.uuid,
            nonce = CryptographicsHelper.GetRandomString(),
            challengeId = Guid.NewGuid().ToString(),
            type = ChallengeType.Password
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

    public UserSession CreateUserSession(User user, TimeSpan validFor)
    {
        return CreateUserSession(user, DateTime.UtcNow + validFor);
    }

    public UserSession CreateUserSession(User user, DateTime validUntil)
    {
        UserSession session = new UserSession
        {
            userUuid = user.uuid,
            creationDate = DateTime.UtcNow,
            lastAccess = DateTime.UtcNow,
            validUnti = validUntil,
            origin = null,
            sessionId = CryptographicsHelper.GetRandomString(100, 100)
        };
        sessions.Add(session);
        return session;
    }

    public LoginResponse Login(LoginRequest request)
    {
        Challenge? rl = runningLogins.FirstOrDefault(x => x.challengeId == request.challengeId && x.type == ChallengeType.Password);
        if(rl == null)
        {
            return new LoginResponse { error = "Password challenge with this id not found" };
        }
        runningLogins.Remove(rl);
        User? u = GetUserByUUID(rl.userUuid);
        if(u == null)
        {
            return new LoginResponse { error = "User associated with challenge not found" };
        }
        
        // hash password
        string passwordHash = CryptographicsHelper.GetHash(u.password + rl.nonce + request.cnonce);
        if (request.passwordHash == null)
        {
            return new LoginResponse { error = "No password hash specified" };
        }
        if (passwordHash.ToLower() != request.passwordHash.ToLower())
        {
            return new LoginResponse { error = "Password incorrect" };
        }
        
        if (u.TwoFactorEnabled)
        {
            Challenge newRl = new Challenge { challengeId = Guid.NewGuid().ToString(), userUuid = u.uuid , type = ChallengeType.TOTP};
            runningLogins.Add(newRl);
            return new LoginResponse { success = true, requires2fa = true, challengeId = newRl.challengeId};
        }

        UserSession session = CreateUserSession(u, new TimeSpan(0, 1, 0, 0)); // Sessions are valid for 1 hour

        return new LoginResponse()
        {
            success = true,
            sessionId = session.sessionId
        };
    }
}