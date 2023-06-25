namespace CRMAPI.Utilities
{
    public class GenerateRandomString
    {
        private static Random random = new Random();
        public static string RandomToken(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
                result[i] = chars[random.Next(chars.Length)];
            return new string(result);
        }
    }
}
