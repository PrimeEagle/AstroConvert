using AstroTools.Formats;

namespace AstroTools
{
    public class AlternateConditionData
    {
        public IAstroFormat InputItem { get; set; }
        public AstrosynthesisCsv ConvertedItem { get; set; }
        public bool PartOfMultiple { get; set;  }
    }
}
