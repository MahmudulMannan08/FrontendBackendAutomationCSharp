using MarketPlaceWeb.Base;
using MarketPlaceWeb.Driver;
using MarketPlaceWeb.Locators;
using MarketPlaceWeb.Pages;
using MarketPlaceWeb.Pages.SRP;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;

namespace MarketPlaceWeb.Test.SearchFacetsTests
{
    [TestFixture]
    class SearchFacetsTests : Page
    {
        SRPMain srp;
        string srpURL;
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

        [Test, Property("TestCaseId", "5251")]
        public void VerifyMakeSelectionInSRP()
        {
            #region Variables
            string make = GetTestData(testDataFile, "5251.make");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.SelectMakeFacet(make);
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.MakeChild, make), "The facet value is not correct.");
            Assert.IsTrue(srp.GetTitleText().Contains(make), "The title text does not contain make " + make);
            Assert.IsTrue(srp.ContainsInListingTitle(make), "Not every listing contains make " + make);
        }

        [Test, Property("TestCaseId", "5252")]
        public void VerifyModelSelectionInSRP()
        {
            #region Variables
            string make = GetTestData(testDataFile, "5252.make");
            string model = GetTestData(testDataFile, "5252.model");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.SelectMakeFacet(make);
            srp.SelectModelFacet(model);
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.ModelChild, model), "The facet value is not correct.");
            Assert.IsTrue(srp.GetTitleText().Contains(make + " " + model), "The title text does not contain make " + make + " " + model);
            Assert.IsTrue(srp.ContainsInListingTitle(make), "Not every listing contains make " + make);
            Assert.IsTrue(srp.ContainsInListingTitle(model), "Not every listing contains model " + model);
        }

        [Test, Property("TestCaseId", "5253")]
        public void VerifyTrimSelectionInSRP()  //Failing due to bad data in Beta
        {
            #region Variables
            string make = GetTestData(testDataFile, "5253.make");
            string model = GetTestData(testDataFile, "5253.model");
            string trim = GetTestData(testDataFile, "5253.trim");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PVFiltered, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.SelectMakeFacet(make);
            srp.SelectModelFacet(model);
            srp.SelectTrimFacetSingle(trim, true);
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.TrimChild, trim), "The facet value is not correct.");
            if (viewport != Viewport.XS) { Assert.IsTrue(srp.ContainsInListingTitle(trim), "Not every listing contains trim " + trim); }  //trim is not included in SRP list title for XS  //All listing may or may not have exact match for Trim
            Assert.IsTrue(srp.ContainsInListingTitle(make), "Not every listing contains make " + make);
            Assert.IsTrue(srp.ContainsInListingTitle(model), "Not every listing contains model " + model);
        }

        [Test, Property("TestCaseId", "5254")]
        public void VerifyMaxYearOnlySelectionInSRP()
        {
            #region Variables
            string maxYear = GetTestData(testDataFile, "5254.maxYear");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.SelectYearFacet(SRPLocators.Year.MaxYear, "", maxYear);
            Assert.IsTrue(srp.GetTitleText().Contains(maxYear), "The title text does not contain max year " + maxYear);
            Assert.IsTrue(srp.ContainsInListingTitle(maxYear, "<"), "Not every year in listing is lesser or equal to maxYear " + maxYear);
        }

        [Test, Property("TestCaseId", "5255")]
        public void VerifyMinYearOnlySelectionInSRP()
        {
            #region Variables
            string minYear = GetTestData(testDataFile, "5255.minYear");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.SelectYearFacet(SRPLocators.Year.MinYear, minYear, "");
            Assert.IsTrue(srp.GetTitleText().Contains(minYear), "The title text does not contain make " + minYear);
            Assert.IsTrue(srp.ContainsInListingTitle(minYear, ">"), "Not every year in listing is greater or equal to minYear " + minYear);
        }

        [Test, Property("TestCaseId", "5256")]
        public void VerifyMinMaxYearSelectionInSRP()
        {
            #region Variables
            string minYear = GetTestData(testDataFile, "5256.minYear");
            string maxYear = GetTestData(testDataFile, "5256.maxYear");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.SelectYearFacet(SRPLocators.Year.Both, minYear, maxYear);
            string yearRange = (language.ToString() == "EN") ? $"{minYear} to {maxYear}" : $"{minYear} à {maxYear}";
            Assert.IsTrue(srp.GetTitleText().Contains(yearRange), "The title text does not contain make " + minYear + " - " + maxYear);
            Assert.IsTrue(srp.ContainsInListingTitle(minYear, "<>", maxYear), "Not every year in listing is between " + minYear + " - " + maxYear);
        }

        [Test, Property("TestCaseId", "5257")]
        public void VerifyMaxPriceOnlySelectionInSRP()  //Old SRP - Failing on Small viewport due to https://trader.atlassian.net/browse/CONS-2302, disabled clicking on Price tab within EnterMinMaxFacet as workaround
        {
            #region Variables
            string maxPrice = GetTestData(testDataFile, "5257.maxPrice");
            string price = (language.ToString() == "EN") ? GetTestData(testDataFile, "5257.priceEn") : GetTestData(testDataFile, "5257.priceFr");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.EnterMinMaxFacet(SRPLocators.Facets.PricePaymentsParent, "", maxPrice);
            Assert.IsTrue(srp.ListingPrice(maxPrice, "<"), "Not every price in listing is lesser or equal to " + maxPrice);
        }

        [Test, Property("TestCaseId", "5258")]
        public void VerifyMinPriceOnlySelectionInSRP()  //Old SRP - Failing on Small viewport due to https://trader.atlassian.net/browse/CONS-2302, disabled clicking on Price tab within EnterMinMaxFacet as workaround
        {
            #region Variables
            string minPrice = GetTestData(testDataFile, "5258.minPrice");
            string price = (language.ToString() == "EN") ? GetTestData(testDataFile, "5258.priceEn") : GetTestData(testDataFile, "5258.priceFr");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.EnterMinMaxFacet(SRPLocators.Facets.PricePaymentsParent, minPrice, "");
            Assert.IsTrue(srp.ListingPrice(minPrice, ">"), "Not every price in listing is greater or equal to " + minPrice);
        }

        [Test, Property("TestCaseId", "5259")]
        public void VerifyMinMaxPriceSelectionInSRP()  //Old SRP - Failing on Small viewport due to https://trader.atlassian.net/browse/CONS-2302, disabled clicking on Price tab within EnterMinMaxFacet as workaround
        {
            #region Variables
            string minPrice = GetTestData(testDataFile, "5259.minPrice");
            string maxPrice = GetTestData(testDataFile, "5259.maxPrice");
            string price = (language.ToString() == "EN") ? GetTestData(testDataFile, "5259.priceEn") : GetTestData(testDataFile, "5259.priceFr");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.EnterMinMaxFacet(SRPLocators.Facets.PricePaymentsParent, minPrice, maxPrice);
            Assert.IsTrue(srp.ListingPrice(minPrice, "<>", maxPrice), "Not every price in listing is between " + minPrice + " - " + maxPrice);
        }

        [Test, Property("TestCaseId", "5260")]
        public void VerifyPrivateSellerTypeSelectionInSRP()
        {
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.SelectSellerTypeFacet(SRPLocators.SellerType.Dealer, false);
            Assert.IsTrue(srp.IsEveryListingPrivate(), "Not every listing is Private.");
        }

        [Test, Property("TestCaseId", "5261")]
        public void VerifyDealerSellerTypeSelectionInSRP()
        {
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.SelectSellerTypeFacet(SRPLocators.SellerType.Private, false);
            Assert.IsTrue(srp.IsEveryListingDealer(), "Not every listing is Dealer.");
        }

        [Test, Property("TestCaseId", "5341")]
        public void VerifyDistanceSelectionInSRP()
        {
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.SelectSearchRadiusFacet(SRPLocators.SearchRadius.Plus_25);
            Assert.IsTrue(srp.IsProximityWithinRange(25), "Proximity distance is not less or equal to 25");
            srp.SelectSearchRadiusFacet(SRPLocators.SearchRadius.Plus_50);
            Assert.IsTrue(srp.IsProximityWithinRange(50), "Proximity distance is not less or equal to 50");
            srp.SelectSearchRadiusFacet(SRPLocators.SearchRadius.Plus_100);
            Assert.IsTrue(srp.IsProximityWithinRange(100), "Proximity distance is not less or equal to 100");
            srp.SelectSearchRadiusFacet(SRPLocators.SearchRadius.Plus_250);
            Assert.IsTrue(srp.IsProximityWithinRange(250), "Proximity distance is not less or equal to 250");
            srp.SelectSearchRadiusFacet(SRPLocators.SearchRadius.Plus_500);
            Assert.IsTrue(srp.IsProximityWithinRange(500), "Proximity distance is not less or equal to 500");
            srp.SelectSearchRadiusFacet(SRPLocators.SearchRadius.Plus_1000);
            Assert.IsTrue(srp.IsProximityWithinRange(1000), "Proximity distance is not less or equal to 1000");
        }

        [Test, Property("TestCaseId", "5344")]
        public void VerifyMileageSelection()
        {
            #region Variables
            string minKilometres = GetTestData(testDataFile, "5344.minKilometres");
            string maxKilometres = GetTestData(testDataFile, "5344.maxKilometres");
            string kms = (language.ToString() == "EN") ? GetTestData(testDataFile, "5344.kmsEn") : GetTestData(testDataFile, "5344.kmsFr");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.EnterMinMaxFacet(SRPLocators.Facets.KilometersChild, minKilometres, maxKilometres);
            Assert.IsTrue(srp.IsMileageWithinRange(minKilometres, "<>", maxKilometres), "Not every Mileage is not within a range " + minKilometres + " - " + maxKilometres);
        }

        [Test, Property("TestCaseId", "5345")]
        public void VerifyCPOSelection()
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
            List<string> conditionsToBeUnChecked = (language.ToString() == "EN") ? new List<string>() { condition1En, condition3En, condition4En } : new List<string>() { condition1Fr, condition3Fr, condition4Fr };
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.SelectMakeFacet("Honda");
            srp.SelectConditionFacetSingle(SRPLocators.Condition.CertifiedPreOwned, true);
            srp.SelectCheckboxMultiple(SRPLocators.Facets.ConditionParent, conditionsToBeUnChecked, false);
            Assert.IsTrue(srp.IsEveryListingCPO(), "Not every listing is Certified Pre-Owned. ");
        }

        [Test, Property("TestCaseId", "5346")]
        public void VerifyNewCarSelection()
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
            List<string> conditionsToBeUnChecked = (language.ToString() == "EN") ? new List<string>() { condition2En, condition3En, condition4En } : new List<string>() { condition2Fr, condition3Fr, condition4Fr };
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.SelectConditionFacetSingle(SRPLocators.Condition.New, true);
            srp.SelectCheckboxMultiple(SRPLocators.Facets.ConditionParent, conditionsToBeUnChecked, false);
            Assert.IsTrue(srp.IsEveryListingNew(), "Not every listing is NEW. ");
        }

        [Test, Property("TestCaseId", "5368")]
        public void VerifyExteriorColor()
        {
            #region Variables
            string color = (language.ToString() == "EN") ? GetTestData(testDataFile, "5368.exteriorColourEn") : GetTestData(testDataFile, "5368.exteriorColourFr");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.SelectExteriorColourFacetSingle(SRPLocators.ExteriorColour.Black, true);
            Assert.AreEqual(srp.GetTotalNumberOfFound(), srp.GetTotalNumberOfFoundInFacetOption(SRPLocators.Facets.ColourChild, color));
            Assert.AreEqual(color, srp.GetSelectedValueOfFacet(SRPLocators.Facets.ColourChild), "The facet value is not correct.");
        }

        [Test, Property("TestCaseId", "5369")]
        public void VerifyBodyType()
        {
            #region Variables
            string bodyType = (language.ToString() == "EN") ? GetTestData(testDataFile, "5369.bodyTypeEn") : GetTestData(testDataFile, "5369.bodyTypeFr");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.Sedan, true);
            Assert.AreEqual(srp.GetTotalNumberOfFound(), srp.GetTotalNumberOfFoundInFacetOption(SRPLocators.Facets.BodyTypeChild, bodyType));
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.BodyTypeChild, bodyType), "The facet value is not correct.");
        }

        [Test, Property("TestCaseId", "5370")]
        public void VerifyDrivetrain()
        {
            #region Variables
            string drivetrain = (language.ToString() == "EN") ? GetTestData(testDataFile, "5370.drivetrainEn") : GetTestData(testDataFile, "5370.drivetrainFr");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.SelectDrivetrainFacetSingle(SRPLocators.Drivetrain.RearWheelDrive, true);
            Assert.AreEqual(srp.GetTotalNumberOfFound(), srp.GetTotalNumberOfFoundInFacetOption(SRPLocators.Facets.DrivetrainChild, drivetrain));
            Assert.AreEqual(drivetrain, srp.GetSelectedValueOfFacet(SRPLocators.Facets.DrivetrainChild), "The facet value is not correct.");
        }

        [Test, Property("TestCaseId", "5372")]
        public void VerifyFuelType()
        {
            #region Variables
            string fuelType1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType1") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType1");
            string fuelType2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.fuelTypeEn.fuelType2") : GetTestData(testDataFile, "commonTestData.fuelTypeFr.fuelType2");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            List<string> listOfFuelType = new List<string>() { fuelType1, fuelType2 };
            srp.SelectCheckboxMultiple(SRPLocators.Facets.FuelTypeChild, listOfFuelType, true);
            Assert.AreEqual(srp.GetTotalNumberOfFound(), srp.GetTotalNumberOfFoundInFacetOption(SRPLocators.Facets.FuelTypeChild, fuelType1) + srp.GetTotalNumberOfFoundInFacetOption(SRPLocators.Facets.FuelTypeChild, fuelType2));
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.FuelTypeChild, fuelType1, fuelType2), "The facet value is not correct.");
        }

        [Test, Property("TestCaseId", "5374")]
        public void VerifyEngine()  //Failing on Small viewport due to issue https://trader.atlassian.net/browse/CONS-1940
        {
            #region Variables
            string engine1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "5374.engineEn.engine1") : GetTestData(testDataFile, "5374.engineFr.engine1");
            string engine2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "5374.engineEn.engine2") : GetTestData(testDataFile, "5374.engineFr.engine2");
            string engine3 = (language.ToString() == "EN") ? GetTestData(testDataFile, "5374.engineEn.engine3") : GetTestData(testDataFile, "5374.engineFr.engine3");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            List<string> listofEngine = new List<string>() { engine1, engine2, engine3 };
            srp.SelectCheckboxMultiple(SRPLocators.Facets.EngineChild, listofEngine, true);
            Assert.AreEqual(srp.GetTotalNumberOfFound(), srp.GetTotalNumberOfFoundInFacetOption(SRPLocators.Facets.EngineChild, engine1) + srp.GetTotalNumberOfFoundInFacetOption(SRPLocators.Facets.EngineChild, engine2) + srp.GetTotalNumberOfFoundInFacetOption(SRPLocators.Facets.EngineChild, engine3));
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.EngineChild, engine1, engine2, engine3), "The facet value is not correct.");
        }

        [Test, Property("TestCaseId", "5375")]
        public void VerifyTransmission()  //Failing on Small viewport due to issue https://trader.atlassian.net/browse/CONS-1940
        {
            #region Variables
            string transmission = (language.ToString() == "EN") ? GetTestData(testDataFile, "5375.transmissionEn") : GetTestData(testDataFile, "5375.transmissionFr");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.SelectTransmissionFacetSingle(SRPLocators.Transmission.Automatic, true);
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.TransmissionChild, transmission), "The facet value is not correct.");
            Assert.AreEqual(srp.GetTotalNumberOfFound(), srp.GetTotalNumberOfFoundInFacetOption(SRPLocators.Facets.TransmissionChild, transmission));
        }

        [Test, Property("TestCaseId", "5376")]
        public void VerifySeatingCapacity()  //Failing on Small viewport due to issue https://trader.atlassian.net/browse/CONS-1940
        {
            #region Variables
            string seatingCapacity1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "5376.seatingCapacityEn.seatingCapacity1") : GetTestData(testDataFile, "5376.seatingCapacityFr.seatingCapacity1");
            string seatingCapacity2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "5376.seatingCapacityEn.seatingCapacity2") : GetTestData(testDataFile, "5376.seatingCapacityFr.seatingCapacity2");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            List<string> listOfSeating = new List<string>() { seatingCapacity1, seatingCapacity2 };
            srp.SelectCheckboxMultiple(SRPLocators.Facets.SeatingCapacityChild, listOfSeating, true);
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            int totalNumberOfFoundInFacetOption1 = srp.GetTotalNumberOfFoundInFacetOption(SRPLocators.Facets.SeatingCapacityChild, seatingCapacity1);
            if (viewport != Viewport.Small)  //Fails on Small sometimes, due to facet jump issue on bottom facets, closing as workaround
            {
                srp.CloseFacet(SRPLocators.Facets.SeatingCapacityChild);
            }
            int totalNumberOfFoundInFacetOption2 = srp.GetTotalNumberOfFoundInFacetOption(SRPLocators.Facets.SeatingCapacityChild, seatingCapacity2);
            Assert.AreEqual(totalNumberOfFound, totalNumberOfFoundInFacetOption1 + totalNumberOfFoundInFacetOption2);
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.SeatingCapacityChild, seatingCapacity1, seatingCapacity2), "The facet value is not correct.");
        }

        [Test, Property("TestCaseId", "5377")]
        public void VerifyDoors()  //Failing on Small viewport due to issue https://trader.atlassian.net/browse/CONS-1940
        {
            #region Variables
            string doors = (language.ToString() == "EN") ? GetTestData(testDataFile, "5377.doorsEn") : GetTestData(testDataFile, "5377.doorsFr");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.PV, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.SelectDoorsFacetSingle(SRPLocators.Doors._2Door, true);
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.DoorsChild, doors), "The facet value is not correct.");
            Assert.AreEqual(srp.GetTotalNumberOfFound(), srp.GetTotalNumberOfFoundInFacetOption(SRPLocators.Facets.DoorsChild, doors));         
        }

        [Test, Property("TestCaseId", "5378")]
        public void VerifyRVLength()
        {
            #region Variables
            string minLength = GetTestData(testDataFile, "5378.minLength");
            string maxLength = GetTestData(testDataFile, "5378.maxLength");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.RVs, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.EnterMinMaxFacet(SRPLocators.Facets.Length, minLength, maxLength);
            Assert.AreNotEqual(0, srp.GetTotalNumberOfFound());
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.Length, minLength + " ft - " + maxLength + " ft"), "The facet value is not correct.");
        }

        [Test, Property("TestCaseId", "5379")]
        public void VerifyRVWeight()
        {
            #region Variables
            string minWeight = GetTestData(testDataFile, "5379.minWeight");
            string maxWeight = GetTestData(testDataFile, "5379.maxWeight");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.RVs, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.EnterMinMaxFacet(SRPLocators.Facets.Weight, "", maxWeight);
            Assert.AreEqual("< " + maxWeight + " lb", srp.GetSelectedValueOfFacet(SRPLocators.Facets.Weight), "The facet value is not " + maxWeight);
            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            srp.EnterMinMaxFacet(SRPLocators.Facets.Weight, minWeight, "");
            Assert.Less(srp.GetTotalNumberOfFound(), totalNumberOfFound);
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.Weight, minWeight + " lb - " + maxWeight + " lb"), "The facet value is not correct.");
        }

        [Test, Property("TestCaseId", "5380")]
        public void VerifyRVSleeps()
        {
            #region Variables
            string select1 = GetTestData(testDataFile, "5380.select1");
            string select2 = GetTestData(testDataFile, "5380.select2");
            string select3 = GetTestData(testDataFile, "5380.select3");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.RVs, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.SelectSleepsFacetSingle(SRPLocators.Sleeps._1, true);
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.Sleeps, select1), "The facet value is not correct.");
            Assert.AreEqual(srp.GetTotalNumberOfFound(), srp.GetTotalNumberOfFoundInFacetOption(SRPLocators.Facets.Sleeps, select1));

            srp.SelectSleepsFacetSingle(SRPLocators.Sleeps._3, true);
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.Sleeps, select1, select2), "The facet value is not correct.");
            Assert.AreEqual(srp.GetTotalNumberOfFound(), srp.GetTotalNumberOfFoundInFacetOption(SRPLocators.Facets.Sleeps, select1));

            srp.SelectSleepsFacetSingle(SRPLocators.Sleeps._8, true);
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.Sleeps, select1, select2, select3), "The facet value is not correct.");
            Assert.AreEqual(srp.GetTotalNumberOfFound(), srp.GetTotalNumberOfFoundInFacetOption(SRPLocators.Facets.Sleeps, select1));
        }

        [Test, Property("TestCaseId", "5382")]
        public void VerifyRVSlideOuts()
        {
            #region Variables
            string select1 = GetTestData(testDataFile, "5382.select1");
            string select2 = GetTestData(testDataFile, "5382.select2");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.RVs, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            List<string> listOfSeating = new List<string>() { select1, select2 };
            srp.SelectCheckboxMultiple(SRPLocators.Facets.SlideOuts, listOfSeating, true);
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.SlideOuts, select1, select2), "The facet value is not correct.");
            Assert.AreEqual(srp.GetTotalNumberOfFound(), srp.GetTotalNumberOfFoundInFacetOption(SRPLocators.Facets.SlideOuts, select1));
        }

        [Test, Property("TestCaseId", "5383")]
        public void VerifyBoatHoursSubtype()
        {
            #region Variables
            string selectSubtype = (language.ToString() == "EN") ? GetTestData(testDataFile, "5383.subTypeEn") : GetTestData(testDataFile, "5383.subTypeFr");
            string minHour = GetTestData(testDataFile, "5383.minHour");
            string maxHour = GetTestData(testDataFile, "5383.maxHour");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.Boats, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.SelectSubTypeFacetSingle(SRPLocators.SubType.Cruiser, true);
            srp.OpenFacetAbs(SRPLocators.Facets.SubType, SRPAbstract.FacetStatus.Open);
            srp.CloseFacet(SRPLocators.Facets.SubType); // Bug, Reopening Facets as a fix for now
            int totalNumberOfFoundInFacetOption = srp.GetTotalNumberOfFoundInFacetOption(SRPLocators.Facets.SubType, selectSubtype);
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.SubType, selectSubtype), "The facet value is not correct.");
            Assert.AreEqual(srp.GetTotalNumberOfFound(), totalNumberOfFoundInFacetOption);

            srp.EnterMinMaxFacet(SRPLocators.Facets.Hours, minHour, "");
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.Hours, "> " + minHour + " h"), "The facet value is not correct.");
            Assert.Less(srp.GetTotalNumberOfFound(), totalNumberOfFoundInFacetOption);

            int totalNumberOfFound = srp.GetTotalNumberOfFound();
            srp.EnterMinMaxFacet(SRPLocators.Facets.Hours, "", maxHour);
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.Hours, minHour + " h - " + maxHour + " h"), "The facet value is not correct.");
            Assert.LessOrEqual(srp.GetTotalNumberOfFound(), totalNumberOfFound); //Equal added due to lack of data
        }

        [Test, Property("TestCaseId", "5384")]
        public void VerifyWatercraftHorsepower()
        {
            #region Variables
            string maxHorsepower = GetTestData(testDataFile, "5384.maxHorsepower");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.Watercraft, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.EnterMinMaxFacet(SRPLocators.Facets.Horsepower, "", maxHorsepower);
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.Horsepower, "< " + maxHorsepower + " hp"), "The facet value is not correct.");
        }

        [Test, Property("TestCaseId", "5390")]
        public void VerifyBikesEngineSize()
        {
            #region Variables
            string minEngineSize = GetTestData(testDataFile, "5390.minEngineSize");
            string maxEngineSize = GetTestData(testDataFile, "5390.maxEngineSize");
            #endregion
            srpURL = srp.srpURL(language, SRPMain.Category.BikesAndATVs, testDataFile);
            url = new Uri(baseURL + srpURL);
            Open();
            srp.EnterMinMaxFacet(SRPLocators.Facets.EngineSize, minEngineSize, maxEngineSize);
            Assert.IsTrue(srp.ContainsInFacet(SRPLocators.Facets.EngineSize, minEngineSize + " cc - " + maxEngineSize + " cc"), "The facet value is not correct.");
        }
    }
}