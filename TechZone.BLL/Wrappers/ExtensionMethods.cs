namespace TechZone.BLL.Wrappers
{
    public static class ExtensionMethods
    {
        public static bool IsValidUpdate<T>(T value)
        {
            if (value == null)
                return false;

            if(value is string str)
                return !string.IsNullOrWhiteSpace(str) && str.ToLower() != "string";

            return true;
        }
    }
}
