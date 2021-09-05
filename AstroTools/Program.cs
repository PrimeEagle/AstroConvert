using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using FileHelpers;

namespace AstroTools
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime startTime = DateTime.Now;

            string path = "D:\\Dropbox\\Writing\\Supplements\\Empyrean\\_AstroSynthesis Supplements\\";
            List<FileType> files = new List<FileType>
            {
                //new FileType($"{path}hygdata_v3.csv", InputFileFormat.HygCsv3),
                new FileType($"{path}Space 100 ly Fileset Package\\Space1 20ly AstroSyn CSV.csv", InputFileFormat.AstrosynthesisCsv),
                //new FileType($"{path}Space 100 ly Fileset Package\\Space2 20-60ly AstroSyn CSV.csv", InputFileFormat.AstrosynthesisCsv),
                //new FileType($"{path}Space 100 ly Fileset Package\\Space3 60-78ly AstroSyn CSV.csv", InputFileFormat.AstrosynthesisCsv),
                //new FileType($"{path}Space 100 ly Fileset Package\\Space4 78-90ly AstroSyn CSV.csv", InputFileFormat.AstrosynthesisCsv),
                //new FileType($"{path}Space 100 ly Fileset Package\\Space5 90-100ly AstroSyn CSV.csv", InputFileFormat.AstrosynthesisCsv),
                //new FileType($"{path}kepner_stardata\\kepner_50lyr_stars.csv", InputFileFormat.AstrosynthesisCsv),
                //new FileType($"{path}kepner_stardata\\kepner_100lyr_stars.csv", InputFileFormat.AstrosynthesisCsv),
                //new FileType($"{path}kepner_stardata\\kepner_1000lyr_stars.csv", InputFileFormat.AstrosynthesisCsv),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_5ly.csv", InputFileFormat.AstrosynthesisCsv),
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

            string outputFilename = $"{path}complete.csv";
            OutputFileFormat outputFormat = OutputFileFormat.AstrosynthesisCsv;

            AstrosynthesisStore outputStore = new AstrosynthesisStore();

            Catalog.Init();

            foreach (var file in files)
            {
                Console.WriteLine($"Reading file '{Path.GetFileName(file.Filename)}'...");
                ProcessFile(file, outputStore);
            }

            outputStore.RemoveAll(x => x.Name.ToLower() == "sol" || x.Name.ToLower().Contains(" sol "));

            WriteFile(outputFormat, outputFilename, outputStore);

            DateTime endTime = DateTime.Now;

            TimeSpan ts = endTime - startTime;
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
            IEnumerable<IAstroFormat> inputList;

            inputList = ReadFile(ft);

            int count = 0;
            int parse = 0;
            int total = inputList.Count();
            string lastSystemId = string.Empty;

            Console.Write($"     Parsing {parse} records of {total}...          ");
            foreach (var input in inputList)
            {
                bool partOfMultiple = (input.SystemId == lastSystemId);

                AstrosynthesisCsv result = input.Convert();
                parse++;
                if (parse % 100 == 0) Console.Write($"\r     Parsing {parse} records of {total} ({count} added)...          ");

                AlternateConditionData data = new AlternateConditionData();
                data.InputItem = input;
                data.ConvertedItem = result;
                data.PartOfMultiple = partOfMultiple;

                bool added = outputStore.Add(result, input.AlternateAddCondition(data));

                if (added)
                    count++;
                else
                    Catalog.Add(CatalogType.Eliminated, $"HD = {result.HenryDraperId}, HR = {result.HarvardRevisedId}, HIP = {result.HipparcosId}, COD = {result.CodId}, BD = {result.BdId}, CPD = {result.CpdId}, Gliese = {result.GlieseId}, Name = {result.Name}");

                lastSystemId = input.SystemId;
            }
            Console.WriteLine($"\r     {count} records out of {inputList.Count()} added.             ");
        }

        private static IEnumerable<IAstroFormat> ReadFile(FileType ft)
        {
            string className = $"{ typeof(Program).Namespace}.{ ft.Format.ToString()}";

            IAstroFormat fileFormat = (IAstroFormat)Activator.CreateInstance(Assembly.GetExecutingAssembly().FullName, className).Unwrap();
            
            return fileFormat.ReadFile(ft);
        }

        private static void WriteFile(OutputFileFormat outputFormat, string filename, AstrosynthesisStore store)
        {
            switch(outputFormat)
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
                                            new XElement("RandomSeed", Utility.RandomDigits(9))
                                        )
                                    )
                                );
                    break;
            }
        }
    }
}
