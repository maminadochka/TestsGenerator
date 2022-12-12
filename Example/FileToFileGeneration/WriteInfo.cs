namespace Example.FileToFileGeneration
{
    public partial class FileToFileGenerator
    {
        private class WriteInfo
        {
            public string FileName { get; set; }

            public string GeneratedTestClass { get; set; }


            public WriteInfo(string fileName, string generatedTestClass)
            {
                FileName = fileName;
                GeneratedTestClass = generatedTestClass;
            }

            public WriteInfo()
            {
                FileName = string.Empty;
                GeneratedTestClass = string.Empty;
            }
        }
    }
}
