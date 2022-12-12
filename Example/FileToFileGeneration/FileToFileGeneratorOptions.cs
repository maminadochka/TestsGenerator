namespace Example.FileToFileGeneration
{
    public class FileToFileGeneratorOptions
    {
        //лимит потоков
        public int MaxRead { get; init; }

        public int MaxProcess { get; init; }

        public int MaxWrite { get; init; }

        public static FileToFileGeneratorOptions Default
        {
            get { return _default; }
        }

        private static FileToFileGeneratorOptions _default = new FileToFileGeneratorOptions()
        {
            MaxRead = 5,
            MaxProcess = 5,
            MaxWrite = 5
        };


        public FileToFileGeneratorOptions()
        {
            MaxRead = 5;
            MaxProcess = 5;
            MaxWrite = 5;
        }
    }
}
