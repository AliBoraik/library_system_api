namespace Library.Domain.Constants;

public static class AdminConstants
{
    // Admin User Information
    public const string SystemAdminUserName = "SystemAdmin";
    public const string SystemAdminEmail = "admin@system.com";

    // Default Password (for seeding purposes, if applicable)
    public const string DefaultAdminPassword = "Adminadmin@123";

    // Admin IDs
    public static readonly Guid SystemAdminId = Guid.Parse("00000000-0000-0000-0000-000000000001");
}