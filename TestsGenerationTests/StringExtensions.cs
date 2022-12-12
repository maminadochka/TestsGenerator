namespace TestsGenerationTests
{
    public static class StringExtensions    
    {
        public static List<string> ExtractWords(this string str)
        {
            return str.Split(new char[] { ' ', '\n', '\r' })
                      .Where(item => !item.Equals(string.Empty))
                      .ToList();
        }
    }
}
