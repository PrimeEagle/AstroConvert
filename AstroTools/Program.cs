using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                new FileType($"{path}hygdata_v3.csv", FileFormat.Hyg3),
                //new FileType($"{path}Space 100 ly Fileset Package\\Space1 20ly AstroSyn CSV.csv", FileFormat.Astrosynthesis),
                //new FileType($"{path}Space 100 ly Fileset Package\\Space2 20-60ly AstroSyn CSV.csv", FileFormat.Astrosynthesis),
                //new FileType($"{path}Space 100 ly Fileset Package\\Space3 60-78ly AstroSyn CSV.csv", FileFormat.Astrosynthesis),
                //new FileType($"{path}Space 100 ly Fileset Package\\Space4 78-90ly AstroSyn CSV.csv", FileFormat.Astrosynthesis),
                //new FileType($"{path}Space 100 ly Fileset Package\\Space5 90-100ly AstroSyn CSV.csv", FileFormat.Astrosynthesis),
                //new FileType($"{path}kepner_stardata\\kepner_50lyr_stars.csv", FileFormat.Astrosynthesis),
                //new FileType($"{path}kepner_stardata\\kepner_100lyr_stars.csv", FileFormat.Astrosynthesis),
                //new FileType($"{path}kepner_stardata\\kepner_1000lyr_stars.csv", FileFormat.Astrosynthesis),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_5ly.csv", FileFormat.Astrosynthesis),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_10ly.csv", FileFormat.Astrosynthesis),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_15ly.csv", FileFormat.Astrosynthesis),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_20ly.csv", FileFormat.Astrosynthesis),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_25ly.csv", FileFormat.Astrosynthesis),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_50ly.csv", FileFormat.Astrosynthesis),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_100ly.csv", FileFormat.Astrosynthesis),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_200ly.csv", FileFormat.Astrosynthesis),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_300ly.csv", FileFormat.Astrosynthesis),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_400ly.csv", FileFormat.Astrosynthesis),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_500ly.csv", FileFormat.Astrosynthesis),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_1000ly.csv", FileFormat.Astrosynthesis),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_5000ly.csv", FileFormat.Astrosynthesis),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_10000ly.csv", FileFormat.Astrosynthesis),
                //new FileType($"{path}hip_astro_charts_1000ly\\hip_astro_charts_beyond10000ly.csv", FileFormat.Astrosynthesis)
            };


            string outputFilename = "complete.csv";
            var outputEngine = new FileHelperEngine<Astrosynthesis>();
            AstrosynthesisStore outputStore = new AstrosynthesisStore();

            Catalog.Init();

            foreach (var file in files)
            {
                Console.WriteLine($"Reading file '{Path.GetFileName(file.Filename)}'...");
                ProcessFile(file, outputStore);
            }

            outputStore.RemoveAll(x => x.Name.ToLower() == "sol" || x.Name.ToLower().Contains(" sol "));

            outputEngine.WriteFile($"{path}{outputFilename}", outputStore.ToList());

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

                Astrosynthesis result = input.Convert();
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
            switch (ft.Format)
            {
                case FileFormat.Astrosynthesis:
                default:
                    var inputEngineAstro = new FileHelperEngine<Astrosynthesis>();
                    return inputEngineAstro.ReadFile($"{ft.Filename}").ToList<IAstroFormat>();
                case FileFormat.Hyg3:
                    var inputEngineHyg3 = new FileHelperEngine<Hyg3>();
                    var inputListHyg3 = inputEngineHyg3.ReadFile($"{ft.Filename}").ToList<Hyg3>();
                    var multiStars = inputListHyg3.Where(x => !string.IsNullOrEmpty(x.MultistarCatalogId)).OrderBy(x => x.MultistarCatalogId).ThenBy(x => x.GlieseId);
                    var singleStars = inputListHyg3.Where(x => string.IsNullOrEmpty(x.MultistarCatalogId));
                    
                    var result = (multiStars ?? Enumerable.Empty<IAstroFormat>()).Concat(singleStars ?? Enumerable.Empty<IAstroFormat>());
                    
                    return result;
            }
        }
    }
}
