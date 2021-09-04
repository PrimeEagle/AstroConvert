using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileHelpers;

namespace AstroTools
{
    [DelimitedRecord(",")]
    [IgnoreFirst(1)]
    public class Hyg3 : IAstroFormat
    {
        [FieldTrim(TrimMode.Both)]
        public string Id { get; set; }
        [FieldTrim(TrimMode.Both)]
        public string HipparcosId { get; set; }
        [FieldTrim(TrimMode.Both)]
        public string HenryDraperId { get; set; }
        [FieldTrim(TrimMode.Both)]
        public string HarvardRevisedId { get; set; }
        [FieldTrim(TrimMode.Both)]
        public string GlieseId { get; set; }
        [FieldTrim(TrimMode.Both)]
        public string BayerFlamsteedId { get; set; }
        [FieldTrim(TrimMode.Both)]
        public string Name { get; set; }
        public double? RightAscension { get; set; }
        public double? Declination { get; set; }
        public double? DistanceParsecs { get; set; }
        public double? ProperMotionRightAscension { get; set; }
        public double? ProperMotionDeclination { get; set; }
        public double? RadialVelocity { get; set; }
        public double? Magnitude { get; set; }
        public double? AbsoluteMagnitude { get; set; }
        [FieldTrim(TrimMode.Both)]
        public string Spectrum { get; set; }
        public double? ColorIndex { get; set; }
        public double? X { get; set; }
        public double? Y { get; set; }
        public double? Z { get; set; }
        public double? VelocityX { get; set; }
        public double? VelocityY { get; set; }
        public double? VelocityZ { get; set; }
        public double? RightAscensionRadians { get; set; }
        public double? DeclinationRadians { get; set; }
        public double? ProperMotionRightAscensionRadians { get; set; }
        public double? ProperMotionDeclinationRadians { get; set; }
        [FieldTrim(TrimMode.Both)]
        public string BayerId { get; set; }
        [FieldTrim(TrimMode.Both)]
        public string FlamsteedId { get; set; }
        [FieldTrim(TrimMode.Both)]
        public string Constellation { get; set; }
        [FieldTrim(TrimMode.Both)]
        public string CompanionStarId { get; set; }
        [FieldTrim(TrimMode.Both)]
        public string CompanionPrimaryStarId { get; set; }
        [FieldTrim(TrimMode.Both)]
        public string MultistarCatalogId { get; set; }
        public double? Luminosity { get; set; }
        [FieldTrim(TrimMode.Both)]
        public string VariableStarId { get; set; }
        public double? VariableMagnitudeMin { get; set; }
        public double? VariableMagnitudeMax { get; set; }
        [FieldHidden]
        public string SystemId { get; set; }

        public Astrosynthesis Convert()
        {
            Astrosynthesis result = null;

            string hdId = this.HenryDraperId;
            string hrId = this.HarvardRevisedId;

            result = new Astrosynthesis();

            double? radius = null;
            double? mass = null;

            if (this.ColorIndex.HasValue)
            {
                double temperature;
                double tempSol = Math.Pow(10, (14.551 - 0.656) / 3.684);

                if (this.ColorIndex > -0.0413)
                {
                    temperature = Math.Pow(10, (14.551 - this.ColorIndex.Value) / 3.684);
                }
                else
                {
                    temperature = Math.Pow(10, (4.945 - Math.Sqrt(1.087353 + 2.906977 * (this.ColorIndex.Value))));
                }

                radius = Math.Pow(tempSol / temperature, 2) * Math.Sqrt(this.Luminosity.Value);
                mass = Math.Pow(radius.Value, (1 / 0.8));
            }

            string bodyType;

            if (!string.IsNullOrEmpty(this.MultistarCatalogId)) bodyType = "Multiple";
            else if (!string.IsNullOrEmpty(this.Spectrum) && this.Spectrum.ToUpper().StartsWith("D")) bodyType = "White Dwarf";
            else bodyType = "Star";

            result.BodyType = bodyType;
            result.Color = null;
            result.Luminosity = this.Luminosity;
            result.Mass = mass;
            result.SystemId = this.MultistarCatalogId;
            result.Name = this.Name;
            result.Constellation = this.Constellation;
            result.Radius = radius;
            result.SpectralClass = this.Spectrum;
            result.X = this.X * 3.262;
            result.Y = this.Y * 3.262;
            result.Z = this.Z * 3.262;
            result.HygId = this.Id;
            result.BayerFlamsteedId = this.BayerFlamsteedId;
            result.BayerId = this.BayerId;
            result.FlamsteedId = this.FlamsteedId;
            result.GlieseId = this.GlieseId;
            result.HenryDraperId = this.HenryDraperId;
            result.HipparcosId = this.HipparcosId;
            result.HarvardRevisedId = this.HarvardRevisedId;
            result.HygId = this.Id;
            result.ColorIndex = this.ColorIndex;
            result.UpdateNote();

            return result;
        }

        public bool? AlternateAddCondition(AlternateConditionData data)
        {
            return !string.IsNullOrEmpty(data.InputItem.CompanionPrimaryStarId);
        }
    }
}
