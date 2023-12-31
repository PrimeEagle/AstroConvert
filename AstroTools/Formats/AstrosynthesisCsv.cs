﻿using System.Collections.Generic;
using System.Linq;
using FileHelpers;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace AstroTools.Formats
{
    [DelimitedRecord(",")]
    [IgnoreFirst(5)]
    [IgnoreLast(2)]
    public class AstrosynthesisCsv : IAstroFormat
    {
        [FieldTrim(TrimMode.Both)]
        public string BodyType { get; set; }
        [FieldTrim(TrimMode.Both)]
        public string SystemId { get; set; }
        [FieldTrim(TrimMode.Both)]
        public string Name { get; set; }
        public double? X { get; set; }
        public double? Y { get; set; }
        public double? Z { get; set; }
        public double? Mass { get; set; }
        public double? Radius { get; set; }
        public double? Luminosity { get; set; }
        [FieldTrim(TrimMode.Both)]
        public string SpectralClass { get; set; }
        [FieldTrim(TrimMode.Both)]
        public string Color { get; set; }
        [FieldTrim(TrimMode.Both)] 
        public string Note { get; set; }

        [FieldHidden]
        public string HenryDraperId { get; set; }

        [FieldHidden]
        public string HarvardRevisedId { get; set; }
       
        [FieldHidden]
        public string HipparcosId { get; set; }

        [FieldHidden]
        public string HygId { get; set; }

        [FieldHidden]
        public string CodId { get; set; }

        [FieldHidden]
        public string BdId { get; set; }

        [FieldHidden]
        public string CpdId { get; set; }

        [FieldHidden]
        public string GlieseId { get; set; }

        [FieldHidden]
        public string BayerFlamsteedId { get; set; }

        [FieldHidden]
        public string BayerId { get; set; }

        [FieldHidden]
        public string FlamsteedId { get; set; }

        [FieldHidden]
        public string Constellation { get; set; }

        [FieldHidden]
        public double? ColorIndex { get; set; }

        [FieldHidden]
        public string CompanionPrimaryStarId { get; init; }

        public void UpdateNote()
        {
            var codId = string.IsNullOrEmpty(this.CodId) ? string.Empty : "C-" + this.CodId;
            var bdId = string.IsNullOrEmpty(this.BdId) ? string.Empty : "BD+" + this.BdId;

            this.Note = $"HD = {this.HenryDraperId}; HR = {this.HarvardRevisedId}; HIP = {this.HipparcosId}; CoD = {codId}; " +
                        $"BD = {bdId}; CPD = {this.CpdId}; Gliese = {this.GlieseId}; BF = {this.BayerFlamsteedId}; B = {this.BayerId}; " +
                        $"F = {this.FlamsteedId}; HYG = {this.HygId}; System = {this.SystemId}; " +
                        $"ColorIndex = {this.ColorIndex.ToString()}; Constellation = {this.Constellation}";
        }

		public AstrosynthesisCsv Convert()
		{
			AstrosynthesisCsv result = null;

            var hdId = Utility.GetTagValue(this.Note, "HD Cat number ");
            if (string.IsNullOrEmpty(hdId)) hdId = Utility.GetTagValue(this.Name, "HDEC ");
            if (string.IsNullOrEmpty(hdId)) hdId = Utility.GetTagValue(this.Name, "HDE ");
            if (string.IsNullOrEmpty(hdId)) hdId = Utility.GetTagValue(this.Name, "HD ");

            var hrId = Utility.GetTagValue(this.Note, "HR Cat number ");
            var hipId = Utility.GetTagValue(this.Note, "HIP");
            if (string.IsNullOrEmpty(hipId)) hipId = Utility.GetTagValue(this.Name, "HIP");

            var codId = Utility.GetTagValue(this.Note, "CoD C-");
            if (string.IsNullOrEmpty(codId)) codId = Utility.GetTagValue(this.Name, "CoD C-");

            var bdId = Utility.GetTagValue(this.Note, "BD B+");
            if (string.IsNullOrEmpty(bdId)) bdId = Utility.GetTagValue(this.Name, "BD B+");

            var cpdId = Utility.GetTagValue(this.Note, "CPD");
            if (string.IsNullOrEmpty(cpdId)) cpdId = Utility.GetTagValue(this.Name, "CPD");

            var glieseId = Utility.GetTagValue(this.Note, "Gliese");
            if (string.IsNullOrEmpty(glieseId)) glieseId = Utility.GetTagValue(this.Name, "Gliese");
            if (string.IsNullOrEmpty(glieseId)) glieseId = Utility.GetTagValue(this.Name, "Gl");

            this.HenryDraperId = hdId;
            this.HarvardRevisedId = hrId;
            this.HipparcosId = hipId;
            this.CodId = codId;
            this.BdId = bdId;
            this.CpdId = cpdId;
            this.GlieseId = glieseId;

            result = this;
            result.UpdateNote();

            return result;
		}

        public bool? AlternateAddCondition(AlternateConditionData data)
        {
            return data.PartOfMultiple;
        }

        IEnumerable<IAstroFormat> IAstroFormat.ReadFile(FileType ft)
        {
            var inputEngineAstro = new FileHelperEngine<AstrosynthesisCsv>();
            
            return inputEngineAstro.ReadFile($"{ft.Filename}").ToList<IAstroFormat>();
        }
    }
}
