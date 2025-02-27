using MarketPlaceWeb.Base;
using MarketPlaceWeb.Driver;
using MarketPlaceWeb.Locators;
using MarketPlaceWeb.Pages;
using MarketPlaceWeb.Pages.SRP;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;

namespace MarketPlaceWeb.Test.ClearSearchTests
{
    [TestFixture]
    class ClearSearchTests : Page
    {
        SRPMain srp;
        string srpURL;
        string facetValue;
        string facetValueYearMin;
        string facetValueYearMax;
        string baseURL;
        AzureConfig azureConfig;
        LocalConfig localConfig;
        dynamic testDataFile;

        [OneTimeSetUp]
        public void FixtureInit()
        {
            azureConfig = new AzureConfig();
            localConfig = new LocalConfig();
            baseURL = (azureConfig.isAzureEnabled) ? azureConfig.uri.ToString() : localConfig.uri.ToString();
            language = (azureConfig.isAzureEnabled) ? azureConfig.language : localConfig.language;
            viewport = (azureConfig.isAzureEnabled) ? azureConfig.GetViewport() : localConfig.GetViewport();
            facetValue = viewport != Viewport.XS ? (language.ToString() == "EN") ? "Any" : "Tout" : string.Empty;
            facetValueYearMin = "Min";
            facetValueYearMax = "Max";
            testDataFile = GetTestDataFile(baseURL);
            srpVariant = (azureConfig.isAzureEnabled) ? azureConfig.srpVariant : (viewport.ToString() == "XS") ?
                GetTestData(testDataFile, "optimizelyCookies.srpRedesign.variantXS") :
                GetTestData(testDataFile, "optimizelyCookies.srpRedesign.variantDT");
        }

        [SetUp]
        public void Init()
        {
            driver = (azureConfig.isAzureEnabled) ? azureConfig.GetDriverFromAzure() : localConfig.GetDriverFromLocal();
            srp = new SRPMain(driver, viewport, language);
        }

        [TearDown]
        public void CleanUp()
        {
            ResultState resultState = TestContext.CurrentContext.Result.Outcome;
            if (resultState == ResultState.Error || resultState == ResultState.Failure)
            {
                TakeScreenshot(TestContext.CurrentContext.Test.Name);
                if (!string.IsNullOrEmpty(localConfig.config) && !localConfig.config.ToLower().Contains("local")) BrowserStackExtensions.MarkBSFailedStatus(driver);
            }
            driver.Quit();
        }

        [Test, Property("TestCaseId", "5285")]
        public void VerifyClearSearchForMake()
        {
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int numberOfFound = srp.GetTotalNumberOfFound();
            srp.SelectFirstOptionFromMakeModelTypeSubType(SRPLocators.Facets.MakeChild);
            string selectedValue = srp.GetSelectedValueOfFacet(SRPLocators.Facets.MakeChild);
            int numberOfFoundForMake = srp.GetTotalNumberOfFound();
            Assert.IsTrue(numberOfFound > numberOfFoundForMake, "The number of found result for the make " + selectedValue + " does not change after applying facet.");
            Assert.IsTrue(srp.GetTitleText().Contains(selectedValue), "The title text does not contain make " + selectedValue);
            srp.ClearFacet(SRPLocators.Facets.MakeChild);
            Assert.IsTrue(srp.GetTotalNumberOfFound() > numberOfFoundForMake, "The number of found result after clearing the make " + selectedValue + " is not lesser than the number of found result after applying the same make.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.MakeChild), "The facet value is not reset after clearing the make " + selectedValue);
            Assert.IsTrue(!srp.GetTitleText().Contains(selectedValue), "The title text contains make " + selectedValue);
        }

        [Test, Property("TestCaseId", "5286")]
        public void VerifyClearSearchForModel()
        {
            #region Variables
            string make = GetTestData(testDataFile, "5291.make");
            #endregion

            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int numberOfFound = srp.GetTotalNumberOfFound();
            srp.SelectMakeFacet(make);
            srp.SelectFirstOptionFromMakeModelTypeSubType(SRPLocators.Facets.ModelChild);
            string selectedValue = srp.GetSelectedValueOfFacet(SRPLocators.Facets.ModelChild);
            int numberOfFoundForModel = srp.GetTotalNumberOfFound();
            Assert.IsTrue(numberOfFound > numberOfFoundForModel, "The number of found result for the model " + selectedValue + " does not change after applying facet.");
            Assert.IsTrue(srp.GetTitleText().Contains(selectedValue), "The title text does not contain make " + selectedValue);
            srp.ClearFacet(SRPLocators.Facets.ModelChild);
            Assert.IsTrue(srp.GetTotalNumberOfFound() > numberOfFoundForModel, "The number of found result after clearing the model " + selectedValue + " is not lesser than the number of found result after applying the same model.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.ModelChild), "The facet value is not reset after clearing the model " + selectedValue);
            Assert.IsTrue(!srp.GetTitleText().Contains(selectedValue), "The title text contains model " + selectedValue);
        }

        [Test, Property("TestCaseId", "5288")]
        public void VerifyClearSearchForBuyingOptionsFacet()
        {
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            srp.SelectBuyingOptionsSingle(SRPLocators.BuyingOptions.HomeDelivery, true);
            int numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the Buying Options filter " + SRPLocators.BuyingOptions.BuyOnline + " does not change after applying the facet.");
            Assert.IsTrue(srp.IsHomeDeliveryOn(), "The Home Delivery toggle button is not on after selecting Home Delivery from Buying Options facet.");
            srp.ClearFacet(SRPLocators.Facets.AtHomeServices);
            Assert.IsTrue(!srp.IsBuyingOptionsChecked(SRPLocators.BuyingOptions.BuyOnline), "The Home Delivery option is not cleared from facet Buying Options.");
            Assert.IsTrue(!srp.IsHomeDeliveryOn(), "The Home Delivery toggle button is still on after clearing Buying Options facet.");
            Assert.AreEqual(totalNumberOfFound, srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing Buying Options facet.");
        }

        [Test, Property("TestCaseId", "5291")]
        public void VerifyClearSearchForTrimFacet()
        {
            #region Variables
            string make = GetTestData(testDataFile, "5291.make");
            string model = GetTestData(testDataFile, "5291.model");
            string trim1 = GetTestData(testDataFile, "5291.trim.trim1");
            string trim2 = GetTestData(testDataFile, "5291.trim.trim2");
            string trim3 = GetTestData(testDataFile, "5291.trim.trim3");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.SelectMakeFacet(make);
            srp.SelectModelFacet(model);
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            List<string> listOfTrim = new List<string>() { trim1, trim2, trim3 };
            srp.SelectCheckboxMultiple(SRPLocators.Facets.TrimChild, listOfTrim, true);
            int numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the Trim facets " + listOfTrim + " does not change after applying the facet.");
            srp.ClearFacet(SRPLocators.Facets.TrimChild);
            Assert.AreEqual(totalNumberOfFound, srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing Trim facet.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.TrimChild), "The facet value is not reset after clearing the Trim " + listOfTrim);
        }

        [Test, Property("TestCaseId", "5292")]
        public void VerifyClearSearchForYearFacet()
        {
            #region Variables
            string minYear = GetTestData(testDataFile, "5292.minYear");
            string maxYear = GetTestData(testDataFile, "5292.maxYear");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            srp.SelectYearFacet(SRPLocators.Year.Both, minYear, maxYear);
            int numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the Year facet does not change after applying the facet.");
            srp.ClearFacet(SRPLocators.Facets.YearChild);
            Assert.AreEqual(totalNumberOfFound, srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing Year facet.");
        }

        [Test, Property("TestCaseId", "5287")]
        public void VerifyClearSearchForPrice()  //Old SRP - Failing on Small viewport due to https://trader.atlassian.net/browse/CONS-2302, disabled clicking on Price tab within EnterMinMaxFacet as workaround
        {
            #region Variables
            string minPrice = GetTestData(testDataFile, "5287.minPrice");
            string maxPrice = GetTestData(testDataFile, "5287.maxPrice");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int numberOfFound = srp.GetTotalNumberOfFound();
            srp.EnterMinMaxFacet(SRPLocators.Facets.PricePaymentsParent, minPrice, maxPrice);
            int numberOfFoundForPrice = srp.GetTotalNumberOfFound();
            Assert.IsTrue(numberOfFound > numberOfFoundForPrice, "The number of found result for the Price range " + minPrice + ", " + maxPrice + " does not change after applying facet.");
            srp.ClearFacet(SRPLocators.Facets.PricePaymentsParent);
            Assert.IsTrue(srp.GetTotalNumberOfFound() > numberOfFoundForPrice, "The number of found result after clearing the Price facet is not lesser than the number of found result after applying the same facet.");
        }

        [Test, Property("TestCaseId", "5290")]
        public void VerifyClearSearchForPayments()  //Old SRP - Failing on Small viewport due to https://trader.atlassian.net/browse/CONS-2302  //New SRP - Failing on Small/XS viewport due to https://trader.atlassian.net/browse/CONS-2097
        {
            #region Variables
            string minPayment = GetTestData(testDataFile, "5290.minPayment");
            string maxPayment = GetTestData(testDataFile, "5290.maxPayment");
            string downPayment = GetTestData(testDataFile, "5290.downPayment");
            string tradeInValue = GetTestData(testDataFile, "5290.tradeInValue");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int numberOfFound = srp.GetTotalNumberOfFound();
            srp.SelectPaymentsFacet(minPayment, maxPayment, SRPLocators.PaymentFrequency.Biweekly, SRPLocators.Term._48, downPayment, tradeInValue);
            int numberOfFoundForPayments = srp.GetTotalNumberOfFound();
            Assert.IsTrue(numberOfFound > numberOfFoundForPayments, "The number of found result for the Payment " + minPayment + ", " + maxPayment + ", " + SRPLocators.PaymentFrequency.Biweekly + ", " + SRPLocators.Term._48 + ", " + downPayment + ", " + tradeInValue + " does not change after applying the facet.");
            srp.ClearFacet(SRPLocators.Facets.PricePaymentsParent);
            Assert.IsTrue(srp.GetTotalNumberOfFound() > numberOfFoundForPayments, "The number of found result after clearing the Payments facet is not lesser than the number of found result after applying the same facet.");
        }

        [Test, Property("TestCaseId", "5293")]
        public void VerifyClearSearchForBodyTypeFacet()
        {
            #region Variables
            string bodyTypeCoupeEn = GetTestData(testDataFile, "5293.bodyTypeEn.bodyType1");
            string bodyTypeOtherEn = GetTestData(testDataFile, "5293.bodyTypeEn.bodyType2");
            string bodyTypeCoupeFr = GetTestData(testDataFile, "5293.bodyTypeFr.bodyType1");
            string bodyTypeOtherFr = GetTestData(testDataFile, "5293.bodyTypeFr.bodyType2");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            List<string> listOfBodyType = (language.ToString() == "EN") ? new List<string>() { bodyTypeCoupeEn, bodyTypeOtherEn } : new List<string>() { bodyTypeCoupeFr, bodyTypeOtherFr };
            srp.SelectCheckboxMultiple(SRPLocators.Facets.BodyTypeChild, listOfBodyType, true);
            int numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the Body Type facet does not change after applying the facet.");
            srp.ClearFacet(SRPLocators.Facets.BodyTypeChild);
            Assert.AreEqual(totalNumberOfFound, srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing Body Type facet.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.BodyTypeChild), "The facet value is not reset after clearing the Body Type.");
        }

        [Test, Property("TestCaseId", "5294")]
        public void VerifyClearSearchForExteriorColourFacet()
        {
            #region Variables
            string exteriorColour1En = GetTestData(testDataFile, "5294.exteriorColourEn.colour1");
            string exteriorColour2En = GetTestData(testDataFile, "5294.exteriorColourEn.colour2");
            string exteriorColour3En = GetTestData(testDataFile, "5294.exteriorColourEn.colour3");
            string exteriorColour1Fr = GetTestData(testDataFile, "5294.exteriorColourFr.colour1");
            string exteriorColour2Fr = GetTestData(testDataFile, "5294.exteriorColourFr.colour2");
            string exteriorColour3Fr = GetTestData(testDataFile, "5294.exteriorColourFr.colour3");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            List<string> listOfColours = (language.ToString() == "EN") ? new List<string>() { exteriorColour1En, exteriorColour2En, exteriorColour3En } : new List<string>() { exteriorColour1Fr, exteriorColour2Fr, exteriorColour3Fr };
            srp.SelectCheckboxMultiple(SRPLocators.Facets.ColourChild, listOfColours, true);
            int numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the Exterior Colour facet does not change after applying the facet.");
            srp.ClearFacet(SRPLocators.Facets.ColourChild);
            Assert.IsTrue(numberOfFoundWithFacet < srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing Exterior Colour facet.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.ColourChild), "The facet value is not reset after clearing the Exterior Colour");
        }

        [Test, Property("TestCaseId", "5295")]
        public void VerifyClearSearchForMileageFacet()
        {
            #region Variables
            string minMileage = GetTestData(testDataFile, "5295.minMileage");
            string maxMileage = GetTestData(testDataFile, "5295.maxMileage");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            srp.EnterMinMaxFacet(SRPLocators.Facets.KilometersChild, minMileage, maxMileage);
            int numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the Mileage facet does not change after applying the facet.");
            srp.ClearFacet(SRPLocators.Facets.KilometersChild);
            Assert.IsTrue(numberOfFoundWithFacet < srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing Mileage facet.");
        }

        [Test, Property("TestCaseId", "5296")]
        public void VerifyClearSearchForDrivetrainFacet()
        {
            #region Variables
            string drivetrain1En = GetTestData(testDataFile, "5296.drivetrainEn.drivetrain1");
            string drivetrain2En = GetTestData(testDataFile, "5296.drivetrainEn.drivetrain2");
            string drivetrain1Fr = GetTestData(testDataFile, "5296.drivetrainFr.drivetrain1");
            string drivetrain2Fr = GetTestData(testDataFile, "5296.drivetrainFr.drivetrain2");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            List<string> listOfDrivetrain = (language.ToString() == "EN") ? new List<string>() { drivetrain1En, drivetrain2En } : new List<string>() { drivetrain1Fr, drivetrain2Fr };
            srp.SelectCheckboxMultiple(SRPLocators.Facets.DrivetrainChild, listOfDrivetrain, true);
            int numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the Drivetrain facet does not change after applying the facet.");
            srp.ClearFacet(SRPLocators.Facets.DrivetrainChild);
            Assert.AreEqual(totalNumberOfFound, srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing Drivetrain facet.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.DrivetrainChild), "The facet value is not reset after clearing the Drivetrain.");
        }

        [Test, Property("TestCaseId", "5297")]
        public void VerifyClearSearchForFuelTypeFacet()
        {
            #region Variables
            string fuelType1En = GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1");
            string fuelType2En = GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2");
            string fuelType1Fr = GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2Fr = GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            List<string> listOfFuelType = (language.ToString() == "EN") ? new List<string>() { fuelType1En, fuelType2En } : new List<string>() { fuelType1Fr, fuelType2Fr };
            srp.SelectCheckboxMultiple(SRPLocators.Facets.FuelTypeChild, listOfFuelType, true);
            int numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the Fuel Type facet does not change after applying the facet.");
            srp.ClearFacet(SRPLocators.Facets.FuelTypeChild);
            Assert.AreEqual(totalNumberOfFound, srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing Fuel Type facet.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.FuelTypeChild), "The facet value is not reset after clearing the Fuel Type.");
        }

        [Test, Property("TestCaseId", "5298")]
        public void VerifyClearSearchForEngineFacet()
        {
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            srp.SelectEngineFacetSingle(SRPLocators.Engine._4Cylinder, true);
            int numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the Engine facet does not change after applying the facet.");
            srp.ClearFacet(SRPLocators.Facets.EngineChild);
            Assert.AreEqual(totalNumberOfFound, srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing Engine facet.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.EngineChild), "The facet value is not reset after clearing the Engine.");
        }

        [Test, Property("TestCaseId", "5299")]
        public void VerifyClearSearchForTransmissionFacet()
        {
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            srp.SelectTransmissionFacetSingle(SRPLocators.Transmission.Manual, true);
            int numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the Transmission facet does not change after applying the facet.");
            srp.ClearFacet(SRPLocators.Facets.TransmissionChild);
            Assert.AreEqual(totalNumberOfFound, srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing Transmission facet.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.TransmissionChild), "The facet value is not reset after clearing the Transmission.");
        }

        [Test, Property("TestCaseId", "5300")]
        public void VerifyClearSearchForSeatingCapacityFacet()
        {
            #region Variables
            string seatingCapacity1En = GetTestData(testDataFile, "5300.seatingCapacityEn.seatingCapacity1");
            string seatingCapacity2En = GetTestData(testDataFile, "5300.seatingCapacityEn.seatingCapacity2");
            string seatingCapacity1Fr = GetTestData(testDataFile, "5300.seatingCapacityFr.seatingCapacity1");
            string seatingCapacity2Fr = GetTestData(testDataFile, "5300.seatingCapacityFr.seatingCapacity2");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            List<string> listOfSeatingCapacity = (language.ToString() == "EN") ? new List<string>() { seatingCapacity1En, seatingCapacity2En } : new List<string>() { seatingCapacity1Fr, seatingCapacity2Fr };
            srp.SelectCheckboxMultiple(SRPLocators.Facets.SeatingCapacityChild, listOfSeatingCapacity, true);
            int numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the Seating Capacity facet does not change after applying the facet.");
            srp.ClearFacet(SRPLocators.Facets.SeatingCapacityChild);
            Assert.AreEqual(totalNumberOfFound, srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing Seating Capacity facet.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.SeatingCapacityChild), "The facet value is not reset after clearing the Seating Capacity.");
        }

        [Test, Property("TestCaseId", "5301")]
        public void VerifyClearSearchForDoorsFacet()
        {
            #region Variables
            string doors1En = GetTestData(testDataFile, "5301.doorsEn.doors1");
            string doors2En = GetTestData(testDataFile, "5301.doorsEn.doors2");
            string doors1Fr = GetTestData(testDataFile, "5301.doorsFr.doors1");
            string doors2Fr = GetTestData(testDataFile, "5301.doorsFr.doors2");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            List<string> listOfDoors = (language.ToString() == "EN") ? new List<string>() { doors1En, doors2En } : new List<string>() { doors1Fr, doors2Fr };
            srp.SelectCheckboxMultiple(SRPLocators.Facets.DoorsChild, listOfDoors, true);
            int numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the Doors facet does not change after applying the facet.");
            srp.ClearFacet(SRPLocators.Facets.DoorsChild);
            Assert.IsTrue(numberOfFoundWithFacet < srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing Doors facet.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.DoorsChild), "The facet value is not reset after clearing the Doors.");
        }

        [Test, Property("TestCaseId", "5289")]
        public void VerifyClearAllSearch()  //Old SRP - Failing on Small viewport due to https://trader.atlassian.net/browse/CONS-2302
        {
            #region Variables
            string condition1En = GetTestData(testDataFile, "commonTestData.conditionEn.condition1");
            string condition2En = GetTestData(testDataFile, "commonTestData.conditionEn.condition2");
            string condition3En = GetTestData(testDataFile, "commonTestData.conditionEn.condition3");
            string condition4En = GetTestData(testDataFile, "commonTestData.conditionEn.condition4");
            string condition1Fr = GetTestData(testDataFile, "commonTestData.conditionFr.condition1");
            string condition2Fr = GetTestData(testDataFile, "commonTestData.conditionFr.condition2");
            string condition3Fr = GetTestData(testDataFile, "commonTestData.conditionFr.condition3");
            string condition4Fr = GetTestData(testDataFile, "commonTestData.conditionFr.condition4");
            List<string> conditionsToBeChecked = (language.ToString() == "EN") ? new List<string>() { condition1En, condition3En } : new List<string>() { condition1Fr, condition3Fr };
            List<string> conditionsToBeUnChecked = (language.ToString() == "EN") ? new List<string>() { condition2En, condition4En } : new List<string>() { condition2Fr, condition4Fr };
            string make = GetTestData(testDataFile, "5289.make");
            string model = GetTestData(testDataFile, "5289.model");
            string trim = GetTestData(testDataFile, "5289.trim");
            string minYear = GetTestData(testDataFile, "5289.minYear");
            string maxYear = GetTestData(testDataFile, "5289.maxYear");
            string minPrice = GetTestData(testDataFile, "5289.minPrice");
            string exteriorColour1En = GetTestData(testDataFile, "5289.exteriorColourEn.exteriorColour1");
            string exteriorColour2En = GetTestData(testDataFile, "5289.exteriorColourEn.exteriorColour2");
            string exteriorColour1Fr = GetTestData(testDataFile, "5289.exteriorColourFr.exteriorColour1");
            string exteriorColour2Fr = GetTestData(testDataFile, "5289.exteriorColourFr.exteriorColour2");
            List<string> exteriorColour = (language.ToString() == "EN") ? new List<string>() { exteriorColour1En, exteriorColour2En } : new List<string>() { exteriorColour1Fr, exteriorColour2Fr };
            string minKilometres = GetTestData(testDataFile, "5289.minKilometres");
            string distanceEn = GetTestData(testDataFile, "5289.distanceEn");
            string distanceFr = GetTestData(testDataFile, "5289.distanceFr");
            string display = GetTestData(testDataFile, "5289.display");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.SelectSearchRadiusFacet(SRPLocators.SearchRadius.Plus_1000);
            srp.SelectSort(SRPLocators.Sort.Year_New_To_Old);
            srp.SelectDisplay(SRPLocators.Display._100);
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            srp.SelectCheckboxMultiple(SRPLocators.Facets.ConditionParent, conditionsToBeChecked, true);
            srp.SelectCheckboxMultiple(SRPLocators.Facets.ConditionParent, conditionsToBeUnChecked, false);
            //srp.SelectSellerTypeFacet(SRPLocators.SellerType.Private, false);  //Removed this as unchecking Private dealer for New and CPO list has no effect on number of results
            srp.SelectMakeFacet(make);
            srp.SelectModelFacet(model);
            //srp.SelectContactlessServicesSingle(SRPLocators.ContactlessServices.LiveChat, true);  //Disabling this as this filters down number of result to very low
            srp.SelectTrimFacetSingle(trim, true);
            srp.SelectYearFacet(SRPLocators.Year.Both, minYear, maxYear);
            srp.EnterMinMaxFacet(SRPLocators.Facets.PricePaymentsParent, minPrice, "");  //Does not affect number of result for this test
            srp.SelectFirstCheckboxFromFacet(SRPLocators.Facets.BodyTypeChild);
            //srp.SelectCheckboxMultiple(SRPLocators.Facets.ExteriorColour, exteriorColour, true);  //Does not have the colors
            srp.EnterMinMaxFacet(SRPLocators.Facets.KilometersChild, minKilometres, "");
            //srp.SelectDrivetrainFacetSingle(SRPLocators.Drivetrain.FrontWheelDrive, true);  //Disabling these as no options available for test to continue at this point. Need to break VerifyClearAllSearch in smaller chunks for automation.
            //srp.SelectFuelTypeFacetSingle(SRPLocators.FuelType.Gasoline, true);
            //srp.SelectEngineFacetSingle(SRPLocators.Engine._4Cylinder, true);
            //srp.SelectTransmissionFacetSingle(SRPLocators.Transmission.Automatic, true);
            //srp.SelectSeatingCapacityFacetSingle(SRPLocators.SeatingCapacity._5Seats, true);
            //srp.SelectDoorsFacetSingle(SRPLocators.Doors._4PlusDoor, true);
            //srp.SelectOtherOptionsSingle(SRPLocators.OtherOptions.With_Free_CARFAX_Report, true);
            int totalNumberOfFoundWithFacets = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > totalNumberOfFoundWithFacets, $"Total number of found {totalNumberOfFound} is not greater than total number with facets {totalNumberOfFoundWithFacets}");

            srp.ClearAllFacets();
            int totalNumberOfFoundClear = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound == totalNumberOfFoundClear, $"Total number of found {totalNumberOfFound} is not equal to total number {totalNumberOfFoundClear} after clearing all facets");

            string distance = (language.ToString() == "EN") ? distanceEn : distanceFr;
            Assert.AreEqual(distance, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Distance), "The Distance facet is reset.");

            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.MakeChild), "The Make facet is not reset.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.ModelChild), "The Model facet is not reset.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.TrimChild), "The Trim facet is not reset.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.BodyTypeChild), "The Body Type facet is not reset.");
            //Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.ColourChild), "The Exterior Colour facet is not reset.");

            Dictionary<string, bool> conditionTypes = (language.ToString() == "EN") ? new Dictionary<string, bool> { { "New", true }, { "Used", true }, { "Certified pre-owned", true }, { "Damaged", false } } : new Dictionary<string, bool> { { "Neuf", true }, { "Occasion", true }, { "Véhicules Certifiés", true }, { "Endommagé", false } };
            Assert.IsTrue(srp.IsCheckboxStatusMultiple(SRPLocators.Facets.ConditionParent, conditionTypes));

            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.KilometersChild), "The Kilometers facet is not reset.");
            //Assert.AreEqual(facetValue, srp.GetFacetElementMinText(SRPLocators.Facets.KilometersChild), "The Kilometers Min facet is not reset.");
            //Assert.AreEqual(facetValue, srp.GetFacetElementMaxText(SRPLocators.Facets.KilometersChild), "The Kilometers Max facet is not reset.");

            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.YearChild), "The Year facet is not reset.");
            //Assert.IsTrue(srp.GetYearMinSelectedText().Equals(facetValue) || srp.GetYearMinSelectedText().Equals(facetValueYearMin), "The Year min facet is not reset.");
            //Assert.IsTrue(srp.GetYearMaxSelectedText().Equals(facetValue) || srp.GetYearMaxSelectedText().Equals(facetValueYearMax), "The Year max facet is not reset.");

            Dictionary<string, bool> sellerTypes = (language.ToString() == "EN") ? new Dictionary<string, bool> { { "Dealer", true }, { "Private", true } } : new Dictionary<string, bool> { { "Détaillant", true }, { "Particulier", true } };
            Assert.IsTrue(srp.IsCheckboxStatusMultiple(SRPLocators.Facets.SellerTypeParent, sellerTypes));

            //Dictionary<string, bool> buyingOptionTypes = (language.ToString() == "EN") ? new Dictionary<string, bool> { { "Live Chat", false }, { "Virtual Appraisal", false }, { "Home Test Drive", false }, { "Online Reservation", false }, { "Buy online", false }, { "Home delivery", false }, { "Money Back Guarantee", false }, { "Try Before You Buy", false } } : new Dictionary<string, bool> { { "Clavardage", false }, { "Évaluation virtuelle", false }, { "Essai routier à la maison", false }, { "Réservation en ligne", false }, { "Achat en ligne", false }, { "Livraison à domicile", false }, { "Garantie de remboursement", false }, { "Essayez avant d'acheter", false } };
            //Assert.IsTrue(srp.IsCheckboxStatusMultiple(SRPLocators.Facets.BuyingOptionsParent, buyingOptionTypes));

            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.PricePaymentsParent), "The Price & Payments facet is not reset.");
            //Assert.AreEqual(facetValue, srp.GetFacetElementMinText(SRPLocators.Facets.PricePaymentsParent), "The Price Min facet is not reset.");
            //Assert.AreEqual(facetValue, srp.GetFacetElementMaxText(SRPLocators.Facets.PricePaymentsParent), "The Price Max facet is not reset.");

            //Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.DrivetrainChild), "The Drivetrain facet is not reset.");
            //Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.FuelTypeChild), "The Fuel Type facet is not reset.");
            //Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.EngineChild), "The Engine facet is not reset.");
            //Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.TransmissionChild), "The Transmission facet is not reset.");

            //Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.SeatingCapacityChild), "The Seating Capacity facet is not reset.");
            //Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.DoorsChild), "The Door facet is not reset.");

            Dictionary<string, bool> otherOptionTypes = (language.ToString() == "EN") ? new Dictionary<string, bool> { { "With photos", true }, { "With price", true }, { "With free CARFAX Report", false } } : new Dictionary<string, bool> { { "Avec photos", true }, { "Avec prix", true }, { "Avec CARFAX gratuit", false } };
            Assert.IsTrue(srp.IsCheckboxStatusMultiple(SRPLocators.Facets.OtherOptionsParent, otherOptionTypes));

            if (viewport != Viewport.XS)
                Assert.AreEqual(display, srp.GetDisplay(), "Display is changed.");
        }

        [Test, Property("TestCaseId", "5323")]
        public void VerifyClearSearchForTypeFacet()  //Failing due to issue https://trader.atlassian.net/browse/CONS-2108
        {
            srpURL = srp.srpURL(language, SRPMain.Category.CommercialHeavyTrucks, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            srp.SelectFirstOptionFromMakeModelTypeSubType(SRPLocators.Facets.Type);
            string selectedValue = srp.GetSelectedValueOfFacet(SRPLocators.Facets.Type);
            int numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the Type facet does not change after applying the facet.");
            Assert.IsTrue(srp.GetTitleText().Contains(selectedValue), "The title text does not contain type " + selectedValue);
            srp.ClearFacet(SRPLocators.Facets.Type);
            Assert.AreEqual(totalNumberOfFound, srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing Type facet.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Type), "The facet value is not reset after clearing the Type.");
            Assert.IsTrue(!srp.GetTitleText().Contains(selectedValue), "The title text contains type " + selectedValue);
        }

        [Test, Property("TestCaseId", "5326")]
        public void VerifyClearSearchForLengthFacet()
        {
            #region Variables
            string minLength = GetTestData(testDataFile, "5326.minLength");
            string maxLength = GetTestData(testDataFile, "5326.maxLength");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.RVs, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            srp.EnterMinMaxFacet(SRPLocators.Facets.Length, minLength, maxLength);
            int numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the Length facet does not change after applying the facet.");
            srp.ClearFacet(SRPLocators.Facets.Length);
            Assert.IsTrue(numberOfFoundWithFacet < srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing Length facet.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Length), "The facet value is not reset after clearing the Length.");
        }

        [Test, Property("TestCaseId", "5327")]
        public void VerifyClearSearchForWeightFacet()
        {
            #region Variables
            string minWeight = GetTestData(testDataFile, "5327.minWeight");
            string maxWeight = GetTestData(testDataFile, "5327.maxWeight");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.RVs, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            srp.EnterMinMaxFacet(SRPLocators.Facets.Weight, minWeight, maxWeight);
            int numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the Weightfacet does not change after applying the facet.");
            srp.ClearFacet(SRPLocators.Facets.Weight);
            Assert.IsTrue(numberOfFoundWithFacet < srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing Weight facet.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Weight), "The facet value is not reset after clearing the Weight.");
        }

        [Test, Property("TestCaseId", "5325")]
        public void VerifyClearSearchForWheelbaseFacet()
        {
            #region Variables
            string minWheelbase = GetTestData(testDataFile, "5325.minWheelbase");
            string maxWheelbase = GetTestData(testDataFile, "5325.maxWheelbase");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.CommercialHeavyTrucks, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            srp.EnterMinMaxFacet(SRPLocators.Facets.Wheelbase, minWheelbase, maxWheelbase);
            int numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the Wheelbase facet does not change after applying the facet.");
            srp.ClearFacet(SRPLocators.Facets.Wheelbase);
            Assert.IsTrue(numberOfFoundWithFacet < srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing Wheelbase facet.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Wheelbase), "The facet value is not reset after clearing the Wheelbase.");
        }

        [Test, Property("TestCaseId", "5328")]
        public void VerifyClearSearchForSleepsFacet()
        {
            #region Variables
            string sleeps1 = GetTestData(testDataFile, "5328.sleeps.sleeps1");
            string sleeps2 = GetTestData(testDataFile, "5328.sleeps.sleeps2");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.RVs, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            List<string> listOfSleeps = new List<string>() { sleeps1, sleeps2 };
            srp.SelectCheckboxMultiple(SRPLocators.Facets.Sleeps, listOfSleeps, true);
            int numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the Sleeps facet does not change after applying the facet.");
            srp.ClearFacet(SRPLocators.Facets.Sleeps);
            Assert.IsTrue(numberOfFoundWithFacet < srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing Sleeps facet.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Sleeps), "The facet value is not reset after clearing the Sleeps.");
        }

        [Test, Property("TestCaseId", "5331")]
        public void VerifyClearSearchForSubTypeFacet()
        {
            srpURL = srp.srpURL(language, SRPMain.Category.Boats, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            srp.SelectFirstOptionFromMakeModelTypeSubType(SRPLocators.Facets.SubType);
            int numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the SubType facet does not change after applying the facet.");
            srp.ClearFacet(SRPLocators.Facets.SubType);
            Assert.AreEqual(totalNumberOfFound, srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing SubType facet.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.SubType), "The facet value is not reset after clearing the SubType.");
        }

        [Test, Property("TestCaseId", "5332")]
        public void VerifyClearSearchForHoursFacet()
        {
            #region Variables
            string minHours = GetTestData(testDataFile, "5332.minHours");
            string maxHours = GetTestData(testDataFile, "5332.maxHours");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.Boats, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            srp.EnterMinMaxFacet(SRPLocators.Facets.Hours, minHours, maxHours);
            int numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the Hours facet does not change after applying the facet.");
            srp.ClearFacet(SRPLocators.Facets.Hours);
            Assert.IsTrue(numberOfFoundWithFacet < srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing Hours facet.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Hours), "The facet value is not reset after clearing the Hours.");
        }

        [Test, Property("TestCaseId", "5333")]
        public void VerifyClearSearchForSubTypeAndHoursFacet()
        {
            #region Variables
            string minHours = GetTestData(testDataFile, "5333.minHours");
            string maxHours = GetTestData(testDataFile, "5333.maxHours");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.Boats, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            srp.SelectFirstOptionFromMakeModelTypeSubType(SRPLocators.Facets.SubType);
            srp.EnterMinMaxFacet(SRPLocators.Facets.Hours, minHours, maxHours);
            int numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the SubType and Hours facet does not change after applying the facet.");
            srp.ClearAllFacets();
            Assert.IsTrue(numberOfFoundWithFacet < srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing SubType and Hours facet.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.SubType), "The facet value is not reset after clearing the SubType.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Hours), "The facet value is not reset after clearing the Hours.");
        }

        [Test, Property("TestCaseId", "5329")]
        public void VerifyClearSearchForSlideOutsFacet()
        {
            #region Variables
            string slideOuts1 = GetTestData(testDataFile, "5329.slideOuts.slideOuts1");
            string slideOuts2 = GetTestData(testDataFile, "5329.slideOuts.slideOuts2");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.RVs, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            List<string> listOfSlideOuts = new List<string>() { slideOuts1, slideOuts2 };
            srp.SelectCheckboxMultiple(SRPLocators.Facets.SlideOuts, listOfSlideOuts, true);
            int numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the SlideOuts facet does not change after applying the facet.");
            srp.ClearFacet(SRPLocators.Facets.SlideOuts);
            Assert.IsTrue(numberOfFoundWithFacet < srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing SlideOuts facet.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.SlideOuts), "The facet value is not reset after clearing the SlideOuts.");
        }

        [Test, Property("TestCaseId", "5334")]
        public void VerifyClearSearchForHorsepowerFacet()
        {
            #region Variables
            string minHorsepower = GetTestData(testDataFile, "5334.minHorsepower");
            string maxHorsepower = GetTestData(testDataFile, "5334.maxHorsepower");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.Watercraft, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            srp.EnterMinMaxFacet(SRPLocators.Facets.Horsepower, minHorsepower, maxHorsepower);
            int numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the Horsepower facet does not change after applying the facet.");
            srp.ClearFacet(SRPLocators.Facets.Horsepower);
            Assert.IsTrue(numberOfFoundWithFacet < srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing Horsepower facet.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Horsepower), "The facet value is not reset after clearing the Horsepower.");
        }

        [Test, Property("TestCaseId", "5336")]
        public void VerifyClearSearchForEngineSizeFacet()
        {
            #region Variables
            string minEngineSize = GetTestData(testDataFile, "5336.minEngineSize");
            string maxEngineSize = GetTestData(testDataFile, "5336.maxEngineSize");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.BikesAndATVs, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            srp.EnterMinMaxFacet(SRPLocators.Facets.EngineSize, minEngineSize, maxEngineSize);
            int numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the EngineSize facet does not change after applying the facet.");
            srp.ClearFacet(SRPLocators.Facets.EngineSize);
            Assert.IsTrue(numberOfFoundWithFacet < srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing EngineSize facet.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.EngineSize), "The facet value is not reset after clearing the EngineSize.");
        }

        [Test, Property("TestCaseId", "5337")]
        public void VerifyClearAllSearchForEngineSizeFacet()
        {
            #region Variables
            int numberOfFoundWithFacet;
            string minEngineSize = GetTestData(testDataFile, "5337.minEngineSize");
            string maxEngineSize = GetTestData(testDataFile, "5337.maxEngineSize");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.BikesAndATVs, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            srp.EnterMinMaxFacet(SRPLocators.Facets.EngineSize, minEngineSize, maxEngineSize);
            numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the EngineSize facet does not change after applying the facet.");
            srp.ClearAllFacets();
            Assert.IsTrue(numberOfFoundWithFacet < srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing EngineSize facet.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.EngineSize), "The facet value is not reset after clearing the EngineSize.");
        }

        [Test, Property("TestCaseId", "5335")]
        public void VerifyClearAllSearchForHorsepowerFacet()
        {
            #region Variables
            string minHorsepower = GetTestData(testDataFile, "5335.minHorsepower");
            string maxHorsepower = GetTestData(testDataFile, "5335.maxHorsepower");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.Watercraft, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            srp.EnterMinMaxFacet(SRPLocators.Facets.Horsepower, minHorsepower, maxHorsepower);
            int numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the Horsepower facet does not change after applying the facet.");
            srp.ClearAllFacets();
            Assert.IsTrue(numberOfFoundWithFacet < srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing Horsepower facet.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Horsepower), "The facet value is not reset after clearing the Horsepower.");
        }

        [Test, Property("TestCaseId", "5330")]
        public void VerifyClearAllSearchForLengthWeightSleepsSlideOutsfacets()
        {
            #region Variables
            int numberOfFoundWithFacet;
            string minLength = GetTestData(testDataFile, "5330.minLength");
            string maxLength = GetTestData(testDataFile, "5330.maxLength");
            string minWeight = GetTestData(testDataFile, "5330.minWeight");
            string maxWeight = GetTestData(testDataFile, "5330.maxWeight");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.RVs, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            srp.EnterMinMaxFacet(SRPLocators.Facets.Length, minLength, maxLength);
            srp.EnterMinMaxFacet(SRPLocators.Facets.Weight, minWeight, maxWeight);
            srp.SelectFirstCheckboxFromFacet(SRPLocators.Facets.Sleeps);
            srp.SelectFirstCheckboxFromFacet(SRPLocators.Facets.SlideOuts);
            numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the All facet does not change after applying the facet.");
            srp.ClearAllFacets();
            Assert.IsTrue(numberOfFoundWithFacet < srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing All facet.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Length), "The facet value is not reset after clearing the Length.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Weight), "The facet value is not reset after clearing the Weight.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Sleeps), "The facet value is not reset after clearing the Sleeps.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.SlideOuts), "The facet value is not reset after clearing the SlideOuts.");
        }

        [Test, Property("TestCaseId", "5324")]
        public void VerifyClearAllSearchForWheelbaseAndTypeFacets()
        {
            #region Variables
            string minWheelbase = GetTestData(testDataFile, "5324.minWheelbase");
            string maxWheelbase = GetTestData(testDataFile, "5324.maxWheelbase");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.CommercialHeavyTrucks, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            srp.SelectFirstOptionFromMakeModelTypeSubType(SRPLocators.Facets.Type);
            srp.EnterMinMaxFacet(SRPLocators.Facets.Wheelbase, minWheelbase, maxWheelbase);
            int numberOfFoundWithFacet = srp.GetTotalNumberOfFound();
            Assert.IsTrue(totalNumberOfFound > numberOfFoundWithFacet, "The number of found result for the All facet does not change after applying the facet.");
            srp.ClearAllFacets();
            Assert.IsTrue(numberOfFoundWithFacet < srp.GetTotalNumberOfFound(), "The number of found result is not reset to initial value after clearing Type and Wheelbase facet.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Wheelbase), "The Wheelbase facet is not reset.");
            Assert.AreEqual(facetValue, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Type), "The Type facet is not reset.");
        }
    }
}