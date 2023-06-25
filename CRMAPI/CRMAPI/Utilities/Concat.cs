namespace CRMAPI.Utilities
{
    public static class Concat
    {
        public static List<string> textConcat { get; } = new List<string>();
        public static string textFinal { get; set; }

        public static string ConcatString(string item)
        {
            textConcat.Add(item);
            string concat = string.Join(",", textConcat);
            textFinal = concat;
            return concat;
        }

        public static void Add(string item)
        {
            textConcat.Add(item);
        }

        public static void Clean()
        {
            textConcat.Clear();
        }
    }
}
