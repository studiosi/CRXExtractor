using CommandLine;
using System;
using System.IO;
using System.IO.Compression;

namespace CRXExtractor
{
    class Program
    {
        static int Main(string[] args)
        {

            CRXExtractorOptions options = new CRXExtractorOptions();

            ParserResult<CRXExtractorOptions> result = Parser.Default
                .ParseArguments<CRXExtractorOptions>(args)
                .WithParsed(parsed => options = parsed);

            Console.WriteLine(options.FilePath);


            // Extract information from file
            CRXFile crx;
            try
            {
                crx = CRXFile.FromFile(options.FilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 1;
            }

            // Create output directory
            try
            {
                Directory.CreateDirectory(options.OutputPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 1;
            }

            // Save all the files
            try
            {
                string currentPath = Path.Combine(options.OutputPath, "pubkey.key");
                File.WriteAllBytes(currentPath, crx.PubKey);
                currentPath = Path.Combine(options.OutputPath, "signature.sig");
                File.WriteAllBytes(currentPath, crx.Signature);
                currentPath = Path.Combine(options.OutputPath, "temp.zip");
                File.WriteAllBytes(currentPath, crx.ZipContents);
                string extractPath = Path.Combine(options.OutputPath, @"contents\");
                ZipFile.ExtractToDirectory(currentPath, extractPath);
                File.Delete(currentPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 1;
            }

            return 0;

        }
    }
}
