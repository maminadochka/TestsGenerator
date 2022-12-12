using System.Threading.Tasks.Dataflow;
using TestsGeneration.Generators;
using TestsGeneration.Parsers;

namespace Example.FileToFileGeneration
{
    public partial class FileToFileGenerator 
    {
        private IParser _parser;
        private ITestGenerator _testGenerator; 
        private FileToFileGeneratorOptions _options; 
        private string _currResultDirectory;


        public FileToFileGenerator(IParser parser, ITestGenerator testGenerator, FileToFileGeneratorOptions options)
        {
            _parser = parser;
            _testGenerator = testGenerator;
            _options = options;
            _currResultDirectory = string.Empty;
        }


        public async Task GenerateTestsAsync(IEnumerable<string> files, string resultDirectory)
        {
            _currResultDirectory = resultDirectory;
            if (!Directory.Exists(resultDirectory))
                Directory.CreateDirectory(resultDirectory);

           
            var inputBuffer = new BufferBlock<string>(); 

            var readBlock = new TransformBlock<string, string>( 
                ReadFromFile,
                new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = _options.MaxRead }); 

            var processBlock = new TransformManyBlock<string, WriteInfo>(
                Process,
                new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = _options.MaxProcess }); 

            var outputBuffer = new BufferBlock<WriteInfo>(); 

            var writeBlock = new ActionBlock<WriteInfo>(
                WriteToFile,
                new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = _options.MaxWrite }); 

            DataflowLinkOptions _dataflowLinkOptions = new() { PropagateCompletion = true }; //уведомление об ошибке(окончании работы) соседние

            readBlock.LinkTo(inputBuffer, _dataflowLinkOptions);
            inputBuffer.LinkTo(processBlock, _dataflowLinkOptions);
            processBlock.LinkTo(outputBuffer, _dataflowLinkOptions);
            outputBuffer.LinkTo(writeBlock, _dataflowLinkOptions);

            foreach (var file in files) 
                readBlock.Post(file);

            readBlock.Complete();  
            await writeBlock.Completion; 
        }

        private async Task<string> ReadFromFile(string path) 
        {
            return await File.ReadAllTextAsync(path);
        }

        private List<WriteInfo> Process(string code)
        {
            var classes = _parser.Parse(code); 
            List<WriteInfo> result = new();

            foreach (var c in classes)
            {
                result.Add(new WriteInfo()
                {
                    FileName = string.Concat(_currResultDirectory, @"\", GenerateProperNamespace(c.Namespace), ".", c.Name, "Tests.cs"),
                    GeneratedTestClass = _testGenerator.Generate(c) 
                });
            }

            return result;
        }

        private async Task WriteToFile(WriteInfo writeInfo)
        {
            await File.WriteAllTextAsync(writeInfo.FileName, writeInfo.GeneratedTestClass);
        }

        private static string GenerateProperNamespace(string nameSpace) 
        {
            if (string.IsNullOrEmpty(nameSpace))
                return "Tests";
            else
                return nameSpace + ".Tests";
        }
    }
}
