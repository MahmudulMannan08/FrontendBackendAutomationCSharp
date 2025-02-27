using MarketPlaceWeb.Base;
using MarketPlaceWeb.Driver;
using MarketPlaceWeb.Locators;
using MarketPlaceWeb.Pages;
using MarketPlaceWeb.Pages.HP;
using MarketPlaceWeb.Pages.SRP;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace MarketPlaceWeb.Test.DirectLinkTests
{
    [TestFixture]
    public class DirectLinkTests : Page
    {
        SRPMain srp;
        HPMain hp;
        string directLinkURL;
        string baseURL;
        AzureConfig azureConfig;
        LocalConfig localConfig;
        dynamic testDataFile;

        [OneTimeSetUp]
        public void FixtureInit()
        {
            azureConfig = new AzureConfig();
            localConfig = new LocalConfig();
            hp = new HPMain(driver, viewport, language);
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

        [Test, Property("TestCaseId", "5381")]
        public void VerifyMakeModelTrimYearPriceViaBookmark()
        {
            #region Variables
            string make = GetTestData(testDataFile, "5381.make");
            string model = GetTestData(testDataFile, "5381.model");
            string trim = GetTestData(testDataFile, "5381.trim");
            string yearMin = GetTestData(testDataFile, "5381.yearMin");
            string yearMax = GetTestData(testDataFile, "5381.yearMax");
            string priceMin = GetTestData(testDataFile, "5381.priceMin");
            string priceMax = GetTestData(testDataFile, "5381.priceMax");
            #endregion
            directLinkURL = srp.directLinkURL(language, "MakeModelTrimYearPrice", testDataFile);
            url = new Uri(baseURL + directLinkURL);
            Open();

            srp.OpenFacetAbs(SRPLocators.Facets.MakeChild, SRPAbstract.FacetStatus.Close);
            Assert.AreEqual(make, srp.GetSelectedValueOfFacet(SRPLocators.Facets.MakeChild), "The Make facet value is not " + make + " for the url " + url);

            srp.OpenFacetAbs(SRPLocators.Facets.ModelChild, SRPAbstract.FacetStatus.Close);
            Assert.AreEqual(model, srp.GetSelectedValueOfFacet(SRPLocators.Facets.ModelChild), "The Model facet value is not " + model + " for the url " + url);

            srp.OpenFacetAbs(SRPLocators.Facets.TrimChild, SRPAbstract.FacetStatus.Close);
            Assert.AreEqual(trim, srp.GetSelectedValueOfFacet(SRPLocators.Facets.TrimChild), "The Trim facet value is not " + trim + " for the url " + url);

            Assert.AreEqual(yearMin, srp.GetYearMinSelectedText(), "The Year min facet value is not " + yearMin + " for the url " + url);
            Assert.AreEqual(yearMax, srp.GetYearMaxSelectedText(), "The Year max facet value is not " + yearMax + " for the url " + url);

            srp.OpenFacetAbs(SRPLocators.Facets.PricePaymentsParent, SRPAbstract.FacetStatus.Open);
            Assert.AreEqual(priceMin, Extensions.RemoveAllMatchingChar(srp.GetFacetElementMinText(SRPLocators.Facets.PricePaymentsParent), new List<char>() { '$', ',', ' ' }), "The Price min facet value is not " + priceMin + " for the url " + url);
            Assert.AreEqual(priceMax, Extensions.RemoveAllMatchingChar(srp.GetFacetElementMaxText(SRPLocators.Facets.PricePaymentsParent), new List<char>() { '$', ',', ' ' }), "The Price max facet value is not " + priceMax + " for the url " + url);
        }

        [Test, Property("TestCaseId", "5385")]
        public void VerifyLocDistncTrimBdTypExtClrDrvtrnFuelEngTransmisssionSeatDoorsViaBookmark()
        {
            #region Variables
            string location = GetTestData(testDataFile, "5385.location");
            string distance = (language.ToString() == "EN") ? GetTestData(testDataFile, "5385.distanceEn") : GetTestData(testDataFile, "5385.distanceFr");
            string bodyType = (language.ToString() == "EN") ? GetTestData(testDataFile, "5385.bodyTypeEn") : GetTestData(testDataFile, "5385.bodyTypeFr");
            string exteriorColour = (language.ToString() == "EN") ? GetTestData(testDataFile, "5385.exteriorColourEn") : GetTestData(testDataFile, "5385.exteriorColourFr");
            string drivetrain = (language.ToString() == "EN") ? GetTestData(testDataFile, "5385.drivetrainEn") : GetTestData(testDataFile, "5385.drivetrainFr");
            string fuelType = (language.ToString() == "EN") ? GetTestData(testDataFile, "5385.fuelTypeEn") : GetTestData(testDataFile, "5385.fuelTypeFr");
            string engine = (language.ToString() == "EN") ? GetTestData(testDataFile, "5385.engineEn") : GetTestData(testDataFile, "5385.engineFr");
            string transmission = (language.ToString() == "EN") ? GetTestData(testDataFile, "5385.transmissionEn") : GetTestData(testDataFile, "5385.transmissionFr");
            string seatingCapacity = (language.ToString() == "EN") ? GetTestData(testDataFile, "5385.seatingCapacityEn") : GetTestData(testDataFile, "5385.seatingCapacityFr");
            string doors = (language.ToString() == "EN") ? GetTestData(testDataFile, "5385.doorsEn") : GetTestData(testDataFile, "5385.doorsFr");
            #endregion
            directLinkURL = srp.directLinkURL(language, "LocDistncTrimBdTypExtClrDrvtrnFuelEngTransmisssionSeatDoors", testDataFile);
            url = new Uri(baseURL + directLinkURL);
            Open();
            
            srp.OpenFacetAbs(SRPLocators.Facets.CityPostalCodeChild, SRPAbstract.FacetStatus.Close);
            Assert.AreEqual(location, srp.GetSelectedValueOfFacet(SRPLocators.Facets.CityPostalCodeChild), "The City/Postal Code facet value is not " + location + " for the url " + url);
            
            srp.OpenFacetAbs(SRPLocators.Facets.Distance, SRPAbstract.FacetStatus.Close);
            Assert.AreEqual(distance, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Distance), "The Distance facet value is not " + distance + " for the url " + url);

            srp.OpenFacetAbs(SRPLocators.Facets.BodyTypeChild, SRPAbstract.FacetStatus.Close);
            Assert.AreEqual(bodyType, srp.GetSelectedValueOfFacet(SRPLocators.Facets.BodyTypeChild), "The Body Type facet value is not " + bodyType + " for the url " + url);

            srp.OpenFacetAbs(SRPLocators.Facets.ColourChild, SRPAbstract.FacetStatus.Close);
            Assert.AreEqual(exteriorColour, srp.GetSelectedValueOfFacet(SRPLocators.Facets.ColourChild), "The Exterior Colour facet value is not " + exteriorColour + " for the url " + url);

            srp.OpenFacetAbs(SRPLocators.Facets.DrivetrainChild, SRPAbstract.FacetStatus.Close);
            Assert.AreEqual(drivetrain, srp.GetSelectedValueOfFacet(SRPLocators.Facets.DrivetrainChild), "The Drivetrain facet value is not " + drivetrain + " for the url " + url);

            srp.OpenFacetAbs(SRPLocators.Facets.FuelTypeChild, SRPAbstract.FacetStatus.Close);
            Assert.AreEqual(fuelType, srp.GetSelectedValueOfFacet(SRPLocators.Facets.FuelTypeChild), "The Fuel Type facet value is not " + fuelType + " for the url " + url);

            srp.OpenFacetAbs(SRPLocators.Facets.EngineChild, SRPAbstract.FacetStatus.Close);
            Assert.AreEqual(engine, srp.GetSelectedValueOfFacet(SRPLocators.Facets.EngineChild), "The Engine facet value is not " + engine + " for the url " + url);

            srp.OpenFacetAbs(SRPLocators.Facets.TransmissionChild, SRPAbstract.FacetStatus.Close);
            Assert.AreEqual(transmission, srp.GetSelectedValueOfFacet(SRPLocators.Facets.TransmissionChild), "The Transmission facet value is not " + transmission + " for the url " + url);

            srp.OpenFacetAbs(SRPLocators.Facets.SeatingCapacityChild, SRPAbstract.FacetStatus.Close);
            Assert.AreEqual(seatingCapacity, srp.GetSelectedValueOfFacet(SRPLocators.Facets.SeatingCapacityChild), "The Seating Capacity facet value is not " + seatingCapacity + " for the url " + url);

            srp.OpenFacetAbs(SRPLocators.Facets.DoorsChild, SRPAbstract.FacetStatus.Close);
            Assert.AreEqual(doors, srp.GetSelectedValueOfFacet(SRPLocators.Facets.DoorsChild), "The Doors facet value is not " + doors + " for the url " + url);
        }

        [Test, Property("TestCaseId", "5386")]
        public void VerifyLengthWeightSleepsSlideOutsViaBookmark()
        {
            #region Variables
            string length = GetTestData(testDataFile, "5386.length");
            string weight = (language.ToString() == "EN") ? GetTestData(testDataFile, "5386.weightEn") : GetTestData(testDataFile, "5386.weightFr");
            string sleeps = GetTestData(testDataFile, "5386.sleeps");
            string slideOuts = GetTestData(testDataFile, "5386.slideOuts");
            #endregion
            directLinkURL = srp.directLinkURL(language, "LengthWeightSleepsSlideOuts", testDataFile);
            url = new Uri(baseURL + directLinkURL);
            Open();
            Assert.AreEqual(length, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Length), "The Length facet value is not " + length + " for the url " + url);
            Assert.AreEqual(weight, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Weight), "The Weight facet value is not " + weight + " for the url " + url);
            Assert.AreEqual(sleeps, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Sleeps), "The Sleeps facet value is not " + sleeps + " for the url " + url);
            Assert.AreEqual(slideOuts, srp.GetSelectedValueOfFacet(SRPLocators.Facets.SlideOuts), "The Slide Outs facet value is not " + slideOuts + " for the url " + url);
        }

        [Test, Property("TestCaseId", "5387")]
        public void VerifyHoursAndSubTypeViaBookmark()
        {
            #region Variables
            string subType = (language.ToString() == "EN") ? GetTestData(testDataFile, "5387.subTypeEn") : GetTestData(testDataFile, "5387.subTypeFr");
            string hours = (language.ToString() == "EN") ? GetTestData(testDataFile, "5387.hoursEn") : GetTestData(testDataFile, "5387.hoursFr");
            #endregion
            directLinkURL = srp.directLinkURL(language, "HoursAndSubType", testDataFile);
            url = new Uri(baseURL + directLinkURL);
            Open();
            Assert.AreEqual(subType, srp.GetSelectedValueOfFacet(SRPLocators.Facets.SubType), "The Sub Type facet value is not " + subType + " for the url " + url);
            Assert.AreEqual(hours, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Hours), "The Hours facet value is not " + hours + " for the url " + url);
        }

        [Test, Property("TestCaseId", "5388")]
        public void VerifyHorsepowerViaBookmark()
        {
            #region Variables
            string horsepower = (language.ToString() == "EN") ? GetTestData(testDataFile, "5388.horsepowerEn") : GetTestData(testDataFile, "5388.horsepowerFr");
            #endregion
            directLinkURL = srp.directLinkURL(language, "Horsepower", testDataFile);
            url = new Uri(baseURL + directLinkURL);
            Open();
            Assert.AreEqual(horsepower, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Horsepower), "The Horsepower facet value is not " + horsepower + " for the url " + url);
        }

        [Test, Property("TestCaseId", "5389")]
        public void VerifyEngineSizeViaBookmark()
        {
            #region Variables
            string engineSize = GetTestData(testDataFile, "5389.engineSize");
            #endregion
            directLinkURL = srp.directLinkURL(language, "EngineSize", testDataFile);
            url = new Uri(baseURL + directLinkURL);
            Open();
            Assert.AreEqual(engineSize, srp.GetSelectedValueOfFacet(SRPLocators.Facets.EngineSize), "The Engine Size facet value is not " + engineSize + " for the url " + url);
        }

        [Test, Property("TestCaseId", "8296")]
        public void VerifyHomePageHeaders()  //Need to click on all header links to verify the CTA is working / or atlease all header links are clickable (displayed and enabled)
        {
            #region Variables
            string headerLinkList = (language.ToString() == "EN") ? GetTestData(testDataFile, "8296.EN") : GetTestData(testDataFile, "8296.FR");
            dynamic deserializedObject = JsonConvert.DeserializeObject(headerLinkList);
            #endregion


            Assert.Multiple(() => {
                foreach (var item in deserializedObject)
                {
                    var headerLink = item.Value;
                    url = new Uri(baseURL + headerLink);
                    Open();
                    Assert.True(IsInCurrentUrl(Convert.ToString(headerLink)), $"'{headerLink}' page was not loaded.");
                }
            });
        }
    }
}