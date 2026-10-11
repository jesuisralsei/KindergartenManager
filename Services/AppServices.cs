namespace KindergartenCRUD.Services
{
    /// <summary>
    /// Tiny service locator so every page can reach the same DatabaseService
    /// instance without wiring up a full DI container for a small app.
    /// </summary>
    public static class AppServices
    {
        public static readonly DatabaseService Database = new();
    }
}
