﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AstroTools
{
    public interface IAstroFormat
    {
        string CompanionPrimaryStarId { get; set; }
        string SystemId { get; set; }

        Astrosynthesis Convert();
        bool? AlternateAddCondition(AlternateConditionData data);
    }
}
