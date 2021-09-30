using System.Collections.Generic;
using AstroTools.Formats;

namespace AstroTools
{
    public interface IAstroFormat
    {
        string CompanionPrimaryStarId { get; init; }
        string SystemId { get; set; }

        AstrosynthesisCsv Convert();
        bool? AlternateAddCondition(AlternateConditionData data);
        IEnumerable<IAstroFormat> ReadFile(FileType ft);
    }
}
