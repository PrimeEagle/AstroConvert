using System;
using System.Collections.Generic;
using System.Text;

namespace AstroTools
{
    public class AlternateConditionData
    {
        public IAstroFormat InputItem { get; set; }
        public AstrosynthesisCsv ConvertedItem { get; set; }
        public bool PartOfMultiple { get; set;  }
    }
}
