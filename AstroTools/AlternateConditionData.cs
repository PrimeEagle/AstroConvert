using AstroTools.Formats;

namespace AstroTools
{
    public class AlternateConditionData
    {
        public IAstroFormat InputItem { get; init; }
        public AstrosynthesisCsv ConvertedItem { get; init; }
        public bool PartOfMultiple { get; init;  }
    }
}
