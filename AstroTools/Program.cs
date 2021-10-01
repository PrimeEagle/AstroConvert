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
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_10ly.csv", InputFileFormat.AstrosynthesisCsv),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_15ly.csv", InputFileFormat.AstrosynthesisCsv),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_20ly.csv", InputFileFormat.AstrosynthesisCsv),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_25ly.csv", InputFileFormat.AstrosynthesisCsv),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_50ly.csv", InputFileFormat.AstrosynthesisCsv),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_100ly.csv", InputFileFormat.AstrosynthesisCsv),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_200ly.csv", InputFileFormat.AstrosynthesisCsv),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_300ly.csv", InputFileFormat.AstrosynthesisCsv),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_400ly.csv", InputFileFormat.AstrosynthesisCsv),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_500ly.csv", InputFileFormat.AstrosynthesisCsv),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_1000ly.csv", InputFileFormat.AstrosynthesisCsv),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_5000ly.csv", InputFileFormat.AstrosynthesisCsv),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_10000ly.csv", InputFileFormat.AstrosynthesisCsv),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_beyond10000ly.csv", InputFileFormat.AstrosynthesisCsv)
            };

            var outputFilename = $"{path}complete.csv";
            const OutputFileFormat outputFormat = OutputFileFormat.AstrosynthesisCsv;

            var outputStore = new AstrosynthesisStore();

            Catalog.Init();

            Parallel.ForEach(files, file =>
            {
                Console.WriteLine($"Reading file '{Path.GetFileName(file.Filename)}'...");
                ProcessFile(file, outputStore);
            });

            outputStore.RemoveAll(x => x.Name.ToLower() == "sol" || x.Name.ToLower().Contains(" sol "));

            WriteFile(outputFormat, outputFilename, outputStore);

            var endTime = DateTime.Now;

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
            var inputList = ReadFile(ft);

            var count = 0;
            var parse = 0;
            var astroFormats = inputList as IAstroFormat[] ?? inputList.ToArray();
            var total = astroFormats.Count();
            var lastSystemId = string.Empty;

            Console.Write($"     Parsing {parse} records of {total}...          ");
            Parallel.ForEach(astroFormats, input =>
            {
                var partOfMultiple = (input.SystemId == lastSystemId);

                var result = input.Convert();
                Interlocked.Increment(ref parse);
                if (parse % 100 == 0)
                    Console.Write($"\r     Parsing {parse} records of {total} ({count} added)...          ");

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
            });

            Console.WriteLine($"\r     {count} records out of {astroFormats.Count()} added.             ");
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
