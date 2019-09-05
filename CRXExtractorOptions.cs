using CommandLine;

namespace CRXExtractor
{
    class CRXExtractorOptions
    {
        [Option('f', Required = true, HelpText = "Path to the CRX file to extract.")]
        public string FilePath { get; set; }
        [Option('o', Required = true, HelpText = "Output folder for the extracted contents.")]
        public string OutputPath { get; set; }
    }
}
