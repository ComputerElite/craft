using craft.DB;
using craft.Server.ApiTypes;

namespace craft.Users;

public class UserManager
{
    public List<Challenge> runningLogins = new ();
    public List<CraftUserSession> sessions = new ();
    public UserManager()
    {
        
    }
    
    public CraftUser? GetUserBySession(string session)
    {
        
        using(CraftDbContext c = new())
        {
            CraftUserSession? s = c.sessions.FirstOrDefault(x => x.sessionId == session);
            if(s == null)
            {
                return null;
            }
            // set last access and check if session is still valid
            s.lastAccess = DateTime.UtcNow;
            if (s.validUnti < s.lastAccess)
            {
                // remove session if it expired
                c.sessions.Remove(s);
                c.SaveChanges();
                return null;
            }

            c.SaveChanges();
            return GetUserByUUID(s.userUuid);
        }
    }
    public CraftUser? GetUserByUUID(string uuid)
    {
        using(CraftDbContext c = new())
        {
            CraftUser? u = c.users.FirstOrDefault(x => x.uuid == uuid);
            if(u != null)
            {
                return u;
            }
        }

        return null;
    }
    public CraftUser? GetUserByUsername(string username)
    {
        using(CraftDbContext c = new())
        {
            CraftUser? u = c.users.FirstOrDefault(x => x.username == username);
            if(u != null)
            {
                return u;
            }
        }

        return null;
    }

    public LoginResponse InitiateLogin(string username)
    {
        CraftUser? u = GetUserByUsername(username);
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

    public CraftUserSession CreateUserSession(CraftUser craftUser, TimeSpan validFor)
    {
        return CreateUserSession(craftUser, DateTime.UtcNow + validFor);
    }

    public CraftUserSession CreateUserSession(CraftUser craftUser, DateTime validUntil)
    {
        CraftUserSession session = new CraftUserSession
        {
            userUuid = craftUser.uuid,
            creationDate = DateTime.UtcNow,
            lastAccess = DateTime.UtcNow,
            validUnti = validUntil,
            origin = null,
            sessionId = CryptographicsHelper.GetRandomString(100, 100)
        };
        using (CraftDbContext c = new())
        {
            c.sessions.Add(session);
            c.SaveChanges();
        }
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
        CraftUser? u = GetUserByUUID(rl.userUuid);
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

        CraftUserSession session = CreateUserSession(u, new TimeSpan(0, 1, 0, 0)); // Sessions are valid for 1 hour

        return new LoginResponse()
        {
            success = true,
            sessionId = session.sessionId
        };
    }

    public List<CraftUserSession> GetSessionsForUser(CraftUser craftUser)
    {
        using(CraftDbContext c = new())
        {
            return c.sessions.Where(x => x.userUuid == craftUser.uuid).ToList();
        }
    }

    public bool IsRootUserNeeded()
    {
        
        using (CraftDbContext c = new())
        {
            return c.users.Count(x => x.uuid != CraftUser.DefaultAdminUser.uuid) == 0;
        }
    }

    public void CreateDefaultUserIfNotExists()
    {
        if (!IsRootUserNeeded()) return;
        if(DoesDefaultAdminUserExist()) return;
        using (CraftDbContext c = new())
        {
            c.users.Add(CraftUser.DefaultAdminUser);
            c.permissions.Add(CraftPermission.DefaultAdminPermission);
            c.SaveChanges();
        }
    }

    private bool DoesDefaultAdminUserExist()
    {
        using (CraftDbContext c = new())
        {
            return c.users.Any(x => x.uuid == CraftUser.DefaultAdminUser.uuid);
        }
    }
}