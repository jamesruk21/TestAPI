namespace Core.Constants
{
    public static class Messages
    {
        public const string MissingConfiguration = "No jsonFilePath entry in the appsettings, cannot load Person data";
        public const string SearchTermMandatory = "searchTerm is mandatory";
        public const string SearchTermTooLong = "searchTerm cannot be longer than 50 chars";
    }
}
