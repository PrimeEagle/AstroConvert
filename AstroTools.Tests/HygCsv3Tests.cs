namespace AstroTools.Tests
{
    using AstroTools;
    using System;
    using NUnit.Framework;
    using NSubstitute;

    [TestFixture]
    public class HygCsv3Tests
    {
        private HygCsv3 _testClass;

        [SetUp]
        public void SetUp()
        {
            _testClass = new HygCsv3();
        }

        [Test]
        public void CanConstruct()
        {
            var instance = new HygCsv3();
            Assert.That(instance, Is.Not.Null);
        }

        [Test]
        public void CanCallConvert()
        {
            var result = _testClass.Convert();
            Assert.Fail("Create or modify test");
        }

        [Test]
        public void CanCallAlternateAddCondition()
        {
            var data = new AlternateConditionData { InputItem = Substitute.For<IAstroFormat>(), ConvertedItem = new AstrosynthesisCsv(), PartOfMultiple = true };
            var result = _testClass.AlternateAddCondition(data);
            Assert.Fail("Create or modify test");
        }

        [Test]
        public void CannotCallAlternateAddConditionWithNullData()
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.AlternateAddCondition(default(AlternateConditionData)));
        }

        [Test]
        public void CanCallReadFile()
        {
            var ft = new FileType("TestValue1927880521", InputFileFormat.HygDsoCsv2);
            var result = ((IAstroFormat)_testClass).ReadFile(ft);
            Assert.Fail("Create or modify test");
        }

        [Test]
        public void CannotCallReadFileWithNullFt()
        {
            Assert.Throws<ArgumentNullException>(() => ((IAstroFormat)_testClass).ReadFile(default(FileType)));
        }

        [Test]
        public void CanSetAndGetId()
        {
            var testValue = "TestValue1850436438";
            _testClass.Id = testValue;
            Assert.That(_testClass.Id, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetHipparcosId()
        {
            var testValue = "TestValue1896721219";
            _testClass.HipparcosId = testValue;
            Assert.That(_testClass.HipparcosId, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetHenryDraperId()
        {
            var testValue = "TestValue1733179806";
            _testClass.HenryDraperId = testValue;
            Assert.That(_testClass.HenryDraperId, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetHarvardRevisedId()
        {
            var testValue = "TestValue1387604587";
            _testClass.HarvardRevisedId = testValue;
            Assert.That(_testClass.HarvardRevisedId, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetGlieseId()
        {
            var testValue = "TestValue1885966288";
            _testClass.GlieseId = testValue;
            Assert.That(_testClass.GlieseId, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetBayerFlamsteedId()
        {
            var testValue = "TestValue1002457101";
            _testClass.BayerFlamsteedId = testValue;
            Assert.That(_testClass.BayerFlamsteedId, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetName()
        {
            var testValue = "TestValue2089278015";
            _testClass.Name = testValue;
            Assert.That(_testClass.Name, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetRightAscension()
        {
            var testValue = 1978014678.75;
            _testClass.RightAscension = testValue;
            Assert.That(_testClass.RightAscension, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetDeclination()
        {
            var testValue = 1135097200.71;
            _testClass.Declination = testValue;
            Assert.That(_testClass.Declination, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetDistanceParsecs()
        {
            var testValue = 1477566051.39;
            _testClass.DistanceParsecs = testValue;
            Assert.That(_testClass.DistanceParsecs, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetProperMotionRightAscension()
        {
            var testValue = 13140075.959999999;
            _testClass.ProperMotionRightAscension = testValue;
            Assert.That(_testClass.ProperMotionRightAscension, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetProperMotionDeclination()
        {
            var testValue = 1527378300.36;
            _testClass.ProperMotionDeclination = testValue;
            Assert.That(_testClass.ProperMotionDeclination, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetRadialVelocity()
        {
            var testValue = 12673120.68;
            _testClass.RadialVelocity = testValue;
            Assert.That(_testClass.RadialVelocity, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetMagnitude()
        {
            var testValue = 496094843.96999997;
            _testClass.Magnitude = testValue;
            Assert.That(_testClass.Magnitude, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetAbsoluteMagnitude()
        {
            var testValue = 718356241.35;
            _testClass.AbsoluteMagnitude = testValue;
            Assert.That(_testClass.AbsoluteMagnitude, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetSpectrum()
        {
            var testValue = "TestValue386551408";
            _testClass.Spectrum = testValue;
            Assert.That(_testClass.Spectrum, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetColorIndex()
        {
            var testValue = 1230584100.57;
            _testClass.ColorIndex = testValue;
            Assert.That(_testClass.ColorIndex, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetX()
        {
            var testValue = 1260164027.43;
            _testClass.X = testValue;
            Assert.That(_testClass.X, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetY()
        {
            var testValue = 410482543.68;
            _testClass.Y = testValue;
            Assert.That(_testClass.Y, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetZ()
        {
            var testValue = 160280573.31;
            _testClass.Z = testValue;
            Assert.That(_testClass.Z, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetVelocityX()
        {
            var testValue = 1525450583.25;
            _testClass.VelocityX = testValue;
            Assert.That(_testClass.VelocityX, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetVelocityY()
        {
            var testValue = 638565682.59;
            _testClass.VelocityY = testValue;
            Assert.That(_testClass.VelocityY, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetVelocityZ()
        {
            var testValue = 237810007.71;
            _testClass.VelocityZ = testValue;
            Assert.That(_testClass.VelocityZ, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetRightAscensionRadians()
        {
            var testValue = 1924912807.29;
            _testClass.RightAscensionRadians = testValue;
            Assert.That(_testClass.RightAscensionRadians, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetDeclinationRadians()
        {
            var testValue = 572057374.68;
            _testClass.DeclinationRadians = testValue;
            Assert.That(_testClass.DeclinationRadians, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetProperMotionRightAscensionRadians()
        {
            var testValue = 1216740221.73;
            _testClass.ProperMotionRightAscensionRadians = testValue;
            Assert.That(_testClass.ProperMotionRightAscensionRadians, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetProperMotionDeclinationRadians()
        {
            var testValue = 797355161.46;
            _testClass.ProperMotionDeclinationRadians = testValue;
            Assert.That(_testClass.ProperMotionDeclinationRadians, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetBayerId()
        {
            var testValue = "TestValue323440502";
            _testClass.BayerId = testValue;
            Assert.That(_testClass.BayerId, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetFlamsteedId()
        {
            var testValue = "TestValue1679295073";
            _testClass.FlamsteedId = testValue;
            Assert.That(_testClass.FlamsteedId, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetConstellation()
        {
            var testValue = "TestValue380754507";
            _testClass.Constellation = testValue;
            Assert.That(_testClass.Constellation, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetCompanionStarId()
        {
            var testValue = "TestValue1463654197";
            _testClass.CompanionStarId = testValue;
            Assert.That(_testClass.CompanionStarId, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetCompanionPrimaryStarId()
        {
            var testValue = "TestValue492089554";
            _testClass.CompanionPrimaryStarId = testValue;
            Assert.That(_testClass.CompanionPrimaryStarId, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetMultistarCatalogId()
        {
            var testValue = "TestValue1299962515";
            _testClass.MultistarCatalogId = testValue;
            Assert.That(_testClass.MultistarCatalogId, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetLuminosity()
        {
            var testValue = 1632120162.75;
            _testClass.Luminosity = testValue;
            Assert.That(_testClass.Luminosity, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetVariableStarId()
        {
            var testValue = "TestValue1106682369";
            _testClass.VariableStarId = testValue;
            Assert.That(_testClass.VariableStarId, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetVariableMagnitudeMin()
        {
            var testValue = 694603061.46;
            _testClass.VariableMagnitudeMin = testValue;
            Assert.That(_testClass.VariableMagnitudeMin, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetVariableMagnitudeMax()
        {
            var testValue = 1431185655.24;
            _testClass.VariableMagnitudeMax = testValue;
            Assert.That(_testClass.VariableMagnitudeMax, Is.EqualTo(testValue));
        }

        [Test]
        public void CanSetAndGetSystemId()
        {
            var testValue = "TestValue1105001075";
            _testClass.SystemId = testValue;
            Assert.That(_testClass.SystemId, Is.EqualTo(testValue));
        }
    }
}