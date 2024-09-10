namespace craft.Users;

public enum CraftPermissionType
{
    Read = 0,
    ReadWrite = 1, // writing should always be reading as well
    Administrator = 2,
}