using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using AstroTools.Formats;
using FileHelpers;
using VNet.Utility;

namespace AstroTools
{
    public class Program
    {
        private static object _sync = new object();

        public static void Main(string[] args)
        {
            var startTime = DateTime.Now;

            const string path = "D:\\Dropbox\\Writing\\Supplements\\Empyrean\\_AstroSynthesis Supplements\\";
            var files = new List<FileType>
            {
                //new FileType($"{path}hygdata_v3.csv", InputFileFormat.HygCsv3),
                new FileType($"{path}Space 100 ly Fileset Package\\Space1 20ly AstroSyn CSV.csv", InputFileFormat.AstrosynthesisCsv),
                new FileType($"{path}Space 100 ly Fileset Package\\Space2 20-60ly AstroSyn CSV.csv", InputFileFormat.AstrosynthesisCsv),
                new FileType($"{path}Space 100 ly Fileset Package\\Space3 60-78ly AstroSyn CSV.csv", InputFileFormat.AstrosynthesisCsv),
                new FileType($"{path}Space 100 ly Fileset Package\\Space4 78-90ly AstroSyn CSV.csv", InputFileFormat.AstrosynthesisCsv),
                new FileType($"{path}Space 100 ly Fileset Package\\Space5 90-100ly AstroSyn CSV.csv", InputFileFormat.AstrosynthesisCsv),
                new FileType($"{path}kepner_stardata\\kepner_50lyr_stars.csv", InputFileFormat.AstrosynthesisCsv),
                new FileType($"{path}kepner_stardata\\kepner_100lyr_stars.csv", InputFileFormat.AstrosynthesisCsv),
                new FileType($"{path}kepner_stardata\\kepner_1000lyr_stars.csv", InputFileFormat.AstrosynthesisCsv),
                new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_5ly.csv", InputFileFormat.AstrosynthesisCsv),
                new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_10ly.csv", InputFileFormat.AstrosynthesisCsv),
                new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_15ly.csv", InputFileFormat.AstrosynthesisCsv),
                new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_20ly.csv", InputFileFormat.AstrosynthesisCsv),
                new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_25ly.csv", InputFileFormat.AstrosynthesisCsv),
                new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_50ly.csv", InputFileFormat.AstrosynthesisCsv),
                new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_100ly.csv", InputFileFormat.AstrosynthesisCsv),
                new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_200ly.csv", InputFileFormat.AstrosynthesisCsv),
                new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_300ly.csv", InputFileFormat.AstrosynthesisCsv),
                new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_400ly.csv", InputFileFormat.AstrosynthesisCsv),
                new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_500ly.csv", InputFileFormat.AstrosynthesisCsv),
                new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_1000ly.csv", InputFileFormat.AstrosynthesisCsv),
                new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_5000ly.csv", InputFileFormat.AstrosynthesisCsv),
                new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_10000ly.csv", InputFileFormat.AstrosynthesisCsv),
                new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_beyond10000ly.csv", InputFileFormat.AstrosynthesisCsv)
            };

            var outputFilename = $"{path}complete.csv";
            const OutputFileFormat outputFormat = OutputFileFormat.AstrosynthesisCsv;

            var outputStore = new AstrosynthesisStore();

            Catalog.Init();

            var i = -1;

            Console.CursorVisible = false;

            Parallel.ForEach(files,
                new ParallelOptions { MaxDegreeOfParallelism = 12 },
                file =>
                {
                    Interlocked.Increment(ref i);
                    file.Number = i;
                    ProcessFile(file, outputStore);
                });

            outputStore.RemoveAll(x => x.Name.ToLower() == "sol" || x.Name.ToLower().Contains(" sol "));

            WriteFile(outputFormat, outputFilename, outputStore);

            var endTime = DateTime.Now;
            Console.SetCursorPosition(0, 30);
            var ts = endTime - startTime;
            foreach (var e in Catalog.Get(CatalogType.Eliminated))
            {
                Console.WriteLine(e);
            }
            Console.WriteLine($"{Catalog.Count(CatalogType.Eliminated)} duplicates eliminated.");
            Console.WriteLine($"Total processing time: {ts.Hours}:{ts.Minutes}:{ts.Seconds}");
            Console.WriteLine($"{outputStore.Count} total records saved to output file.");
        }

        private static void ProcessFile(FileType ft, AstrosynthesisStore outputStore)
        {
            //ShowMessage(ft, $"Reading file...");

            var inputList = ReadFile(ft);

            var count = 0;
            var parse = 0;
            var astroFormats = inputList as IAstroFormat[] ?? inputList.ToArray();
            var total = astroFormats.Count();
            var lastSystemId = string.Empty;
            //(ft, $"Parsing {parse} records of {total}...");
            foreach (var input in inputList)
            {
                var partOfMultiple = (input.SystemId == lastSystemId);

                var result = input.Convert();
                parse++;
                //if (parse % 100 == 0)
                ShowMessage(ft, $"({Math.Round((double)parse / (double)total * 100.0, 2)}%)");

                var data = new AlternateConditionData
                {
                    InputItem = input,
                    ConvertedItem = result,
                    PartOfMultiple = partOfMultiple
                };

                var added = outputStore.Add(result, input.AlternateAddCondition(data));

                if (added)
                    Interlocked.Increment(ref count);
                else
                    Catalog.Add(CatalogType.Eliminated,
                        $"HD = {result.HenryDraperId}, HR = {result.HarvardRevisedId}, HIP = {result.HipparcosId}, COD = {result.CodId}, BD = {result.BdId}, CPD = {result.CpdId}, Gliese = {result.GlieseId}, Name = {result.Name}");

                lastSystemId = input.SystemId;
            }
            //ShowMessage(ft, $"{count} records out of {astroFormats.Count()} added.");
        }

        private static void ShowMessage(FileType ft, string message)
        {
            lock (_sync)
            {
                message = $"{Path.GetFileName(ft.Filename.ToLower().Trim())} {message}".PadRight(100);
                Console.SetCursorPosition(0, ft.Number);
                Console.WriteLine(message);
            }
        }

        private static IEnumerable<IAstroFormat> ReadFile(FileType ft)
        {
            var className = $"{ typeof(Program).Namespace}.Formats.{ ft.Format.ToString()}";

            var fileFormat = (IAstroFormat)Activator.CreateInstance(Assembly.GetExecutingAssembly().FullName, className)?.Unwrap();

            return fileFormat?.ReadFile(ft);
        }

        private static void WriteFile(OutputFileFormat outputFormat, string filename, AstrosynthesisStore store)
        {
            switch (outputFormat)
            {
                case OutputFileFormat.AstrosynthesisCsv:
                default:
                    var outputEngine = new FileHelperEngine<AstrosynthesisCsv>();
                    outputEngine.WriteFile($"{filename}", store.ToList());

                    break;
                case OutputFileFormat.AstrosynthesisDsoXml:
                    var xml = new XElement("Bodies",
                                        new XAttribute("Count", store.Count),
                                    store.Select(body =>
                                        new XElement("Body",
                                            new XAttribute("ID", new Guid().ToString()),
                                            new XAttribute("Name", body.Name),
                                            new XAttribute("TypeID", "Deep Space Object"),
                                            new XElement("X", body.X),
                                            new XElement("Z", body.Z),
                                            new XElement("RandomSeed", StringUtility.CreateRandomDigits(9))
                                        )
                                    )
                                );
                    break;
            }
        }
    }
}
