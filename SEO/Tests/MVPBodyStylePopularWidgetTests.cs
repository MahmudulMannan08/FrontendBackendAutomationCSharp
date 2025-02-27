using MarketPlaceWeb.Base;
using MarketPlaceWeb.Pages.HP;
using MarketPlaceWeb.Pages;
using NUnit.Framework.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIAutomationLibrary.Pages.HubPages;
using static MarketPlaceWeb.Locators.SRPLocators;
using MarketPlaceWeb.Locators;
using OpenQA.Selenium;

namespace SEO.Tests
{
    class MVPBodyStylePopularWidgetTests: Page
    {
        AzureConfig azureConfig;
        LocalConfig localConfig;

        string srpUrl;
        string baseURL;
        SRPMain srp;
        HPMain hp;
        HubPagesMain hubpages;
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


        }
        [SetUp]
        public void Init()
        {
            driver = (azureConfig.isAzureEnabled) ? azureConfig.GetDriverFromAzure() : localConfig.GetDriverFromLocal();
            srp = new SRPMain(driver, viewport, language);
            hubpages = new HubPagesMain(driver, viewport, language);
            hp = new HPMain(driver, viewport, language);

        }

        [TearDown]
        public void CleanUp()
        {
            ResultState resultState = TestContext.CurrentContext.Result.Outcome;
            if (resultState == ResultState.Error || resultState == ResultState.Failure)
            {
                TakeScreenshot(TestContext.CurrentContext.Test.Name);
            }
            driver.Quit();
        
        }

        [Test]
        public void VerifyPopularWidgetDispalyOnSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");
            string eTitleParam2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.sedan") : GetTestData(testDataFile, "BodyTypesFr.sedan");

            #endregion
            url = new Uri(srpUrl);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            string[] expectedModels = GetTestData(testDataFile, "PopularCarsTitleEn.PopularCarsModels").Split(',');

            Assert.IsTrue(srp.GetTotalWidgetModelCount() == 6, "Total number of model links displayed does not match as expected!!!");
            foreach ( string model in expectedModels ) {
                Assert.IsTrue(srp.IsPoluparWidgetModelDisplayed(model), String.Format("Model {0} is not displayed", model));
                Assert.AreEqual(srp.GetPopularWidgetModelLink(model), srpUrl + GetTestData(testDataFile, String.Format("PopularCarsUrlsEn.{0}", model.Replace(" ",""))), String.Format("href link for model {0} is invalid", model));
            }
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.Sedan, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
        }

        [Test]
        public void VerifyPopularWidgetForConvertibleSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");
            string eTitleParam2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.convertible") : GetTestData(testDataFile, "BodyTypesFr.convertible");

            #endregion
            url = new Uri(srpUrl);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            string[] expectedModels = GetTestData(testDataFile, "PopularCarsTitleEn.PopularCarsModels").Split(',');

            Assert.IsTrue(srp.GetTotalWidgetModelCount() == 6, "Total number of model links displayed does not match as expected!!!");
            foreach (string model in expectedModels)
            {
                Assert.IsTrue(srp.IsPoluparWidgetModelDisplayed(model), String.Format("Model {0} is not displayed", model));
                Assert.AreEqual(srp.GetPopularWidgetModelLink(model), srpUrl + GetTestData(testDataFile, String.Format("PopularCarsUrlsEn.{0}", model.Replace(" ", ""))), String.Format("href link for model {0} is invalid", model));
            }
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.Convertible, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
            Assert.IsTrue(srp.GetTotalWidgetModelCount() == 6, "Total number of model links displayed does not match as expected!!!");

            string[] expectedConvertibleModels = GetTestData(testDataFile, "PopularCarsTitleEn.PopularConvertiblesModels").Split(',');
            foreach (string model in expectedConvertibleModels)
            {
                Assert.IsTrue(srp.IsPoluparWidgetModelDisplayed(model), String.Format("Model {0} is not displayed", model));
                Assert.AreEqual(srp.GetPopularWidgetModelLink(model), srpUrl + GetTestData(testDataFile, String.Format("PopularConvertiblesUrlsEn.{0}", model.Replace(" ", ""))), String.Format("href link for model {0} is invalid", model));
            }
        }

        [Test]
        public void VerifyPopularWidgetForCoupeSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");
            string eTitleParam2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.coupe") : GetTestData(testDataFile, "BodyTypesFr.coupe");

            #endregion
            url = new Uri(srpUrl);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            string[] expectedModels = GetTestData(testDataFile, "PopularCarsTitleEn.PopularCarsModels").Split(',');

            Assert.IsTrue(srp.GetTotalWidgetModelCount() == 6, "Total number of model links displayed does not match as expected!!!");
            foreach (string model in expectedModels)
            {
                Assert.IsTrue(srp.IsPoluparWidgetModelDisplayed(model), String.Format("Model {0} is not displayed", model));
                Assert.AreEqual(srp.GetPopularWidgetModelLink(model), srpUrl + GetTestData(testDataFile, String.Format("PopularCarsUrlsEn.{0}", model.Replace(" ", ""))), String.Format("href link for model {0} is invalid", model));
            }
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.Coupe, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
            Assert.IsTrue(srp.GetTotalWidgetModelCount() == 6, "Total number of model links displayed does not match as expected!!!");

            string[] expectedCoupeModels = GetTestData(testDataFile, "PopularCarsTitleEn.PopularCoupeModels").Split(',');
            foreach (string model in expectedCoupeModels)
            {
                Assert.IsTrue(srp.IsPoluparWidgetModelDisplayed(model), String.Format("Model {0} is not displayed", model));
                Assert.AreEqual(srp.GetPopularWidgetModelLink(model), srpUrl + GetTestData(testDataFile, String.Format("PopularCoupeUrlsEn.{0}", model.Replace(" ", ""))), String.Format("href link for model {0} is invalid", model));
            }
        }

        [Test]
        public void VerifyPopularWidgetForHatchBackSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");
            string eTitleParam2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.hatchback") : GetTestData(testDataFile, "BodyTypesFr.hatchback");

            #endregion
            url = new Uri(srpUrl);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            string[] expectedModels = GetTestData(testDataFile, "PopularCarsTitleEn.PopularCarsModels").Split(',');

            Assert.IsTrue(srp.GetTotalWidgetModelCount() == 6, "Total number of model links displayed does not match as expected!!!");
            foreach (string model in expectedModels)
            {
                Assert.IsTrue(srp.IsPoluparWidgetModelDisplayed(model), String.Format("Model {0} is not displayed", model));
                Assert.AreEqual(srp.GetPopularWidgetModelLink(model), srpUrl + GetTestData(testDataFile, String.Format("PopularCarsUrlsEn.{0}", model.Replace(" ", ""))), String.Format("href link for model {0} is invalid", model));
            }
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.Hatchback, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
            Assert.IsTrue(srp.GetTotalWidgetModelCount() == 6, "Total number of model links displayed does not match as expected!!!");

            string[] expectedHatchbackModels = GetTestData(testDataFile, "PopularCarsTitleEn.PopularHatchbackModels").Split(',');
            foreach (string model in expectedHatchbackModels)
            {
                Assert.IsTrue(srp.IsPoluparWidgetModelDisplayed(model), String.Format("Model {0} is not displayed", model));
                Assert.AreEqual(srp.GetPopularWidgetModelLink(model), srpUrl + GetTestData(testDataFile, String.Format("PopularHatchabackUrlsEn.{0}", model.Replace(" ", ""))), String.Format("href link for model {0} is invalid", model));
            }
        }

        [Test]
        public void VerifyPopularWidgetForMinivanSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");
            string eTitleParam2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.minivan") : GetTestData(testDataFile, "BodyTypesFr.minivan");

            #endregion
            url = new Uri(srpUrl);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            string[] expectedModels = GetTestData(testDataFile, "PopularCarsTitleEn.PopularCarsModels").Split(',');

            Assert.IsTrue(srp.GetTotalWidgetModelCount() == 6, "Total number of model links displayed does not match as expected!!!");
            foreach (string model in expectedModels)
            {
                Assert.IsTrue(srp.IsPoluparWidgetModelDisplayed(model), String.Format("Model {0} is not displayed", model));
                Assert.AreEqual(srp.GetPopularWidgetModelLink(model), srpUrl + GetTestData(testDataFile, String.Format("PopularCarsUrlsEn.{0}", model.Replace(" ", ""))), String.Format("href link for model {0} is invalid", model));
            }
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.Minivan, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
            Assert.IsTrue(srp.GetTotalWidgetModelCount() == 6, "Total number of model links displayed does not match as expected!!!");

            string[] expectedMinivanModels = GetTestData(testDataFile, "PopularCarsTitleEn.PopularMinivanModels").Split(',');
            foreach (string model in expectedMinivanModels)
            {
                Assert.IsTrue(srp.IsPoluparWidgetModelDisplayed(model), String.Format("Model {0} is not displayed", model));
                Assert.AreEqual(srp.GetPopularWidgetModelLink(model), srpUrl + GetTestData(testDataFile, String.Format("PopularMinivanUrlsEn.{0}", model.Replace(" ", ""))), String.Format("href link for model {0} is invalid", model));
            }
        }

        [Test]
        public void VerifyPopularWidgetForSedanSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");
            string eTitleParam2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.sedan") : GetTestData(testDataFile, "BodyTypesFr.sedan");

            #endregion
            url = new Uri(srpUrl);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            string[] expectedModels = GetTestData(testDataFile, "PopularCarsTitleEn.PopularCarsModels").Split(',');

            Assert.IsTrue(srp.GetTotalWidgetModelCount() == 6, "Total number of model links displayed does not match as expected!!!");
            foreach (string model in expectedModels)
            {
                Assert.IsTrue(srp.IsPoluparWidgetModelDisplayed(model), String.Format("Model {0} is not displayed", model));
                Assert.AreEqual(srp.GetPopularWidgetModelLink(model), srpUrl + GetTestData(testDataFile, String.Format("PopularCarsUrlsEn.{0}", model.Replace(" ", ""))), String.Format("href link for model {0} is invalid", model));
            }
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.Sedan, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
            Assert.IsTrue(srp.GetTotalWidgetModelCount() == 6, "Total number of model links displayed does not match as expected!!!");

            string[] expectedTruckModels = GetTestData(testDataFile, "PopularCarsTitleEn.PopularSedanModels").Split(',');
            foreach (string model in expectedTruckModels)
            {
                Assert.IsTrue(srp.IsPoluparWidgetModelDisplayed(model), String.Format("Model {0} is not displayed", model));
                Assert.AreEqual(srp.GetPopularWidgetModelLink(model), srpUrl + GetTestData(testDataFile, String.Format("PopularSedanUrlsEn.{0}", model.Replace(" ", ""))), String.Format("href link for model {0} is invalid", model));
            }
        }

        [Test]
        public void VerifyPopularWidgetForSUVSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");
            string eTitleParam2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.suv") : GetTestData(testDataFile, "BodyTypesFr.suv");

            #endregion
            url = new Uri(srpUrl);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            string[] expectedModels = GetTestData(testDataFile, "PopularCarsTitleEn.PopularCarsModels").Split(',');

            Assert.IsTrue(srp.GetTotalWidgetModelCount() == 6, "Total number of model links displayed does not match as expected!!!");
            foreach (string model in expectedModels)
            {
                Assert.IsTrue(srp.IsPoluparWidgetModelDisplayed(model), String.Format("Model {0} is not displayed", model));
                Assert.AreEqual(srp.GetPopularWidgetModelLink(model), srpUrl + GetTestData(testDataFile, String.Format("PopularCarsUrlsEn.{0}", model.Replace(" ", ""))), String.Format("href link for model {0} is invalid", model));
            }
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.SUV, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
            Assert.IsTrue(srp.GetTotalWidgetModelCount() == 6, "Total number of model links displayed does not match as expected!!!");

            string[] expectedSUVModels = GetTestData(testDataFile, "PopularCarsTitleEn.PopularSUVModels").Split(',');
            foreach (string model in expectedSUVModels)
            {
                Assert.IsTrue(srp.IsPoluparWidgetModelDisplayed(model), String.Format("Model {0} is not displayed", model));
                Assert.AreEqual(srp.GetPopularWidgetModelLink(model), srpUrl + GetTestData(testDataFile, String.Format("PopularSUVUrlsEn.{0}", model.Replace(" ", ""))), String.Format("href link for model {0} is invalid", model));
            }
        }

        [Test]
        public void VerifyPopularWidgetForTruckSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");
            string eTitleParam2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.truck") : GetTestData(testDataFile, "BodyTypesFr.truck");

            #endregion
            url = new Uri(srpUrl);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            string[] expectedModels = GetTestData(testDataFile, "PopularCarsTitleEn.PopularCarsModels").Split(',');

            Assert.IsTrue(srp.GetTotalWidgetModelCount() == 6, "Total number of model links displayed does not match as expected!!!");
            foreach (string model in expectedModels)
            {
                Assert.IsTrue(srp.IsPoluparWidgetModelDisplayed(model), String.Format("Model {0} is not displayed", model));
                Assert.AreEqual(srp.GetPopularWidgetModelLink(model), srpUrl + GetTestData(testDataFile, String.Format("PopularCarsUrlsEn.{0}", model.Replace(" ", ""))), String.Format("href link for model {0} is invalid", model));
            }
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.Truck, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
            Assert.IsTrue(srp.GetTotalWidgetModelCount() == 6, "Total number of model links displayed does not match as expected!!!");

            string[] expectedTruckModels = GetTestData(testDataFile, "PopularCarsTitleEn.PopularTruckModels").Split(',');
            foreach (string model in expectedTruckModels)
            {
                Assert.IsTrue(srp.IsPoluparWidgetModelDisplayed(model), String.Format("Model {0} is not displayed", model));
                Assert.AreEqual(srp.GetPopularWidgetModelLink(model), srpUrl + GetTestData(testDataFile, String.Format("PopularTruckUrlsEn.{0}", model.Replace(" ", ""))), String.Format("href link for model {0} is invalid", model));
            }
        }

        [Test]
        public void VerifyPopularWidgetForWagonSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");
            string eTitleParam2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.wagon") : GetTestData(testDataFile, "BodyTypesFr.wagon");

            #endregion
            url = new Uri(srpUrl);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            string[] expectedModels = GetTestData(testDataFile, "PopularCarsTitleEn.PopularCarsModels").Split(',');

            Assert.IsTrue(srp.GetTotalWidgetModelCount() == 6, "Total number of model links displayed does not match as expected!!!");
            foreach (string model in expectedModels)
            {
                Assert.IsTrue(srp.IsPoluparWidgetModelDisplayed(model), String.Format("Model {0} is not displayed", model));
                Assert.AreEqual(srp.GetPopularWidgetModelLink(model), srpUrl + GetTestData(testDataFile, String.Format("PopularCarsUrlsEn.{0}", model.Replace(" ", ""))), String.Format("href link for model {0} is invalid", model));
            }
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.Wagon,true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
            Assert.IsTrue(srp.GetTotalWidgetModelCount() == 6, "Total number of model links displayed does not match as expected!!!");

            string[] expectedWagonModels = GetTestData(testDataFile, "PopularCarsTitleEn.PopularWagonModels").Split(',');
            foreach (string model in expectedWagonModels)
            {
                Assert.IsTrue(srp.IsPoluparWidgetModelDisplayed(model), String.Format("Model {0} is not displayed", model));
                Assert.AreEqual(srp.GetPopularWidgetModelLink(model), srpUrl + GetTestData(testDataFile, String.Format("PopularWagonUrlsEn.{0}", model.Replace(" ", ""))), String.Format("href link for model {0} is invalid", model));
            }
        }

        [Test]
        public void VerifyPopularWidgetMultipleBodyTypeSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string bodyTypeCoupeEn = GetTestData(testDataFile, "bodyTypeEn.bodyType1");
            string bodyTypeOtherEn = GetTestData(testDataFile, "bodyTypeEn.bodyType2");
            string bodyTypeCoupeFr = GetTestData(testDataFile, "bodyTypeFr.bodyType1");
            string bodyTypeOtherFr = GetTestData(testDataFile, "bodyTypeFr.bodyType2");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");


            #endregion
            url = new Uri(srpUrl);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            string[] expectedModels = GetTestData(testDataFile, "PopularCarsTitleEn.PopularCarsModels").Split(',');

            Assert.IsTrue(srp.GetTotalWidgetModelCount() == 6, "Total number of model links displayed does not match as expected!!!");
            foreach (string model in expectedModels)
            {
                Assert.IsTrue(srp.IsPoluparWidgetModelDisplayed(model), String.Format("Model {0} is not displayed", model));
                Assert.AreEqual(srp.GetPopularWidgetModelLink(model), srpUrl + GetTestData(testDataFile, String.Format("PopularCarsUrlsEn.{0}", model.Replace(" ", ""))), String.Format("href link for model {0} is invalid", model));
            }
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            List<string> listOfBodyType = (language.ToString() == "EN") ? new List<string>() { bodyTypeCoupeEn, bodyTypeOtherEn } : new List<string>() { bodyTypeCoupeFr, bodyTypeOtherFr };
            srp.SelectCheckboxMultiple(SRPLocators.Facets.BodyTypeChild, listOfBodyType, true);
            Assert.IsFalse(srp.IsPopularWidgetTitle());
        }

        [Test]
        public void VerifyPopularWidgetForSUVStaticUrlSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string body = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.suvSRP") : GetTestData(testDataFile, "urlsFr.suvSRP");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            //string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.suv") : GetTestData(testDataFile, "BodyTypesFr.suv");
            string eTitleParam1 = (language.ToString() == "EN") ? "SUVs" : GetTestData(testDataFile, "BodyTypesFr.suv");


            #endregion
            url = new Uri(baseURL + body);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            Assert.IsTrue(srp.GetTotalWidgetModelCount() == 6, "Total number of model links displayed does not match as expected!!!");
            
            string[] expectedSUVModels = GetTestData(testDataFile, "PopularCarsTitleEn.PopularSUVModels").Split(',');
            foreach (string model in expectedSUVModels)
            {
                Assert.IsTrue(srp.IsPoluparWidgetModelDisplayed(model), String.Format("Model {0} is not displayed", model));
                Assert.AreEqual(srp.GetPopularWidgetModelLink(model), srpUrl + GetTestData(testDataFile, String.Format("PopularSUVUrlsEn.{0}", model.Replace(" ", ""))), String.Format("href link for model {0} is invalid", model));
            }
        }

        [Test]
        public void VerifyPopularWidgetForSedanStaticUrlSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string body = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.sedanSRP") : GetTestData(testDataFile, "urlsFr.sedanSRP");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.sedan") : GetTestData(testDataFile, "BodyTypesFr.sedan");


            #endregion
            url = new Uri(baseURL + body);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            Assert.IsTrue(srp.GetTotalWidgetModelCount() == 6, "Total number of model links displayed does not match as expected!!!");

            string[] expectedSedanModels = GetTestData(testDataFile, "PopularCarsTitleEn.PopularSedanModels").Split(',');
            foreach (string model in expectedSedanModels)
            {
                Assert.IsTrue(srp.IsPoluparWidgetModelDisplayed(model), String.Format("Model {0} is not displayed", model));
                Assert.AreEqual(srp.GetPopularWidgetModelLink(model), srpUrl + GetTestData(testDataFile, String.Format("PopularSedanUrlsEn.{0}", model.Replace(" ", ""))), String.Format("href link for model {0} is invalid", model));
            }
        }

        [Test]
        public void VerifyPopularWidgetForConvertibleStaticUrlSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string body = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.convertibleSRP") : GetTestData(testDataFile, "urlsFr.convertibleSRP");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.convertible") : GetTestData(testDataFile, "BodyTypesFr.convertible");


            #endregion
            url = new Uri(baseURL + body);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            Assert.IsTrue(srp.GetTotalWidgetModelCount() == 6, "Total number of model links displayed does not match as expected!!!");

            string[] expectedSUVModels = GetTestData(testDataFile, "PopularCarsTitleEn.PopularConvertiblesModels").Split(',');
            foreach (string model in expectedSUVModels)
            {
                Assert.IsTrue(srp.IsPoluparWidgetModelDisplayed(model), String.Format("Model {0} is not displayed", model));
                Assert.AreEqual(srp.GetPopularWidgetModelLink(model), srpUrl + GetTestData(testDataFile, String.Format("PopularConvertiblesUrlsEn.{0}", model.Replace(" ", ""))), String.Format("href link for model {0} is invalid", model));
            }
        }
        [Test]
        public void VerifyPopularWidgetForCoupeStaticUrlSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string body = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.coupeSRP") : GetTestData(testDataFile, "urlsFr.coupeSRP");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.coupe") : GetTestData(testDataFile, "BodyTypesFr.coupe");


            #endregion
            url = new Uri(baseURL + body);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            Assert.IsTrue(srp.GetTotalWidgetModelCount() == 6, "Total number of model links displayed does not match as expected!!!");

            string[] expectedSUVModels = GetTestData(testDataFile, "PopularCarsTitleEn.PopularCoupeModels").Split(',');
            foreach (string model in expectedSUVModels)
            {
                Assert.IsTrue(srp.IsPoluparWidgetModelDisplayed(model), String.Format("Model {0} is not displayed", model));
                Assert.AreEqual(srp.GetPopularWidgetModelLink(model), srpUrl + GetTestData(testDataFile, String.Format("PopularCoupeUrlsEn.{0}", model.Replace(" ", ""))), String.Format("href link for model {0} is invalid", model));
            }
        }

        [Test]
        public void VerifyPopularWidgetForMinivanStaticUrlSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string body = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.minivanSRP") : GetTestData(testDataFile, "urlsFr.minivanSRP");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.minivan") : GetTestData(testDataFile, "BodyTypesFr.minivan");


            #endregion
            url = new Uri(baseURL + body);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            Assert.IsTrue(srp.GetTotalWidgetModelCount() == 6, "Total number of model links displayed does not match as expected!!!");

            string[] expectedSUVModels = GetTestData(testDataFile, "PopularCarsTitleEn.PopularMinivanModels").Split(',');
            foreach (string model in expectedSUVModels)
            {
                Assert.IsTrue(srp.IsPoluparWidgetModelDisplayed(model), String.Format("Model {0} is not displayed", model));
                Assert.AreEqual(srp.GetPopularWidgetModelLink(model), srpUrl + GetTestData(testDataFile, String.Format("PopularMinivanUrlsEn.{0}", model.Replace(" ", ""))), String.Format("href link for model {0} is invalid", model));
            }
        }

        [Test]
        public void VerifyPopularWidgetForTruckStaticUrlSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string body = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.truckSRP") : GetTestData(testDataFile, "urlsFr.truckSRP");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.truck") : GetTestData(testDataFile, "BodyTypesFr.truck");


            #endregion
            url = new Uri(baseURL + body);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            Assert.IsTrue(srp.GetTotalWidgetModelCount() == 6, "Total number of model links displayed does not match as expected!!!");

            string[] expectedSUVModels = GetTestData(testDataFile, "PopularCarsTitleEn.PopularTruckModels").Split(',');
            foreach (string model in expectedSUVModels)
            {
                Assert.IsTrue(srp.IsPoluparWidgetModelDisplayed(model), String.Format("Model {0} is not displayed", model));
                Assert.AreEqual(srp.GetPopularWidgetModelLink(model), srpUrl + GetTestData(testDataFile, String.Format("PopularTruckUrlsEn.{0}", model.Replace(" ", ""))), String.Format("href link for model {0} is invalid", model));
            }

        }

        [Test]
        public void VerifyPopularWidgetForHatchBackStaticUrlSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string body = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.hatchbackSRP") : GetTestData(testDataFile, "urlsFr.hatchbackSRP");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.hatchback") : GetTestData(testDataFile, "BodyTypesFr.hatchback");


            #endregion
            url = new Uri(baseURL + body);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            Assert.IsTrue(srp.GetTotalWidgetModelCount() == 6, "Total number of model links displayed does not match as expected!!!");

            string[] expectedSUVModels = GetTestData(testDataFile, "PopularCarsTitleEn.PopularHatchbackModels").Split(',');
            foreach (string model in expectedSUVModels)
            {
                Assert.IsTrue(srp.IsPoluparWidgetModelDisplayed(model), String.Format("Model {0} is not displayed", model));
                Assert.AreEqual(srp.GetPopularWidgetModelLink(model), srpUrl + GetTestData(testDataFile, String.Format("PopularHatchabackUrlsEn.{0}", model.Replace(" ", ""))), String.Format("href link for model {0} is invalid", model));
            }

        }

        [Test]
        public void VerifyPopularWidgetOnLocationChangeSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string city = GetTestData(testDataFile, "LocationLinks.city");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");
            string eTitleParam2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.sedan") : GetTestData(testDataFile, "BodyTypesFr.sedan");

            #endregion
            url = new Uri(srpUrl);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.Sedan, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
            srp.SelectLocationFacet("Kitchener");
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
        }

        [Test]
        public void VerifyPopularWidgetOnDistanceChangeSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string city = GetTestData(testDataFile, "LocationLinks.city");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");
            string eTitleParam2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.sedan") : GetTestData(testDataFile, "BodyTypesFr.sedan");

            #endregion
            url = new Uri(srpUrl);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.Sedan, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
            srp.SelectSearchRadiusFacet(SRPLocators.SearchRadius.Plus_1000);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, city), "Title text does not match as expected!!!");
        }

        [Test]
        public void VerifyPopularWidgetOnDeselectingUsedSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");
            string eTitleParam2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.sedan") : GetTestData(testDataFile, "BodyTypesFr.sedan");


            #endregion
            url = new Uri(srpUrl);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.Sedan, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
            srp.SelectConditionFacetSingle(SRPLocators.Condition.Used, false);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
        }

        [Test]
        public void VerifyPopularWidgetOnDeselectingNewSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");
            string eTitleParam2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.sedan") : GetTestData(testDataFile, "BodyTypesFr.sedan");
            

            #endregion
            url = new Uri(srpUrl);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.Sedan, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
            srp.SelectConditionFacetSingle(SRPLocators.Condition.New, false);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
        }

        [Test]
        public void VerifyPopularWidgetOnDeselectingNewAndUsedSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string condition1En = GetTestData(testDataFile, "commonTestData.conditionEn.condition1");
            string condition2En = GetTestData(testDataFile, "commonTestData.conditionEn.condition2");
            string condition3En = GetTestData(testDataFile, "commonTestData.conditionEn.condition3");
            string condition4En = GetTestData(testDataFile, "commonTestData.conditionEn.condition4");
            string condition1Fr = GetTestData(testDataFile, "commonTestData.conditionFr.condition1");
            string condition2Fr = GetTestData(testDataFile, "commonTestData.conditionFr.condition2");
            string condition3Fr = GetTestData(testDataFile, "commonTestData.conditionFr.condition3");
            string condition4Fr = GetTestData(testDataFile, "commonTestData.conditionFr.condition4");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");
            string eTitleParam2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.sedan") : GetTestData(testDataFile, "BodyTypesFr.sedan");

            List<string> conditionsToBeChecked = (language.ToString() == "EN") ? new List<string>() { condition1En, condition3En } : new List<string>() { condition1Fr, condition3Fr };
            List<string> conditionsToBeUnChecked = (language.ToString() == "EN") ? new List<string>() { condition2En, condition4En } : new List<string>() { condition2Fr, condition4Fr };
            

            #endregion
            url = new Uri(srpUrl);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.Sedan, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
            srp.SelectCheckboxMultiple(SRPLocators.Facets.ConditionParent, conditionsToBeChecked, true);
            srp.SelectCheckboxMultiple(SRPLocators.Facets.ConditionParent, conditionsToBeUnChecked, false);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");

        }

        [Test]
        public void VerifyPopularWidgetOnDeselectingDealerSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");
            string eTitleParam2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.sedan") : GetTestData(testDataFile, "BodyTypesFr.sedan");

            #endregion
            url = new Uri(srpUrl);

            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.Sedan, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
            srp.SelectSellerTypeFacet(SRPLocators.SellerType.Dealer, false);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");

        }

        [Test]
        public void VerifyPopularWidgetOnDeselectingPrivateSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");
            string eTitleParam2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.sedan") : GetTestData(testDataFile, "BodyTypesFr.sedan");

            #endregion
            url = new Uri(srpUrl);

            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.Sedan, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
            srp.SelectSellerTypeFacet(SRPLocators.SellerType.Private, false);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
        }

        [Test]
        public void VerifyPopularWidgetForMakeSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");
            string eTitleParam2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.sedan") : GetTestData(testDataFile, "BodyTypesFr.sedan");

            #endregion
            url = new Uri(srpUrl);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.Sedan, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
            srp.SelectFirstOptionFromMakeModelTypeSubType(SRPLocators.Facets.MakeChild);
           // srp.SelectMakeFacet("Honda");
            Assert.IsFalse(srp.IsPopularWidgetTitle());
        }

        [Test]
        public void VerifyPopularWidgetForMakeModelSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");
            string eTitleParam2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.sedan") : GetTestData(testDataFile, "BodyTypesFr.sedan");

            #endregion
            url = new Uri(srpUrl);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.Sedan, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
            srp.SelectFirstOptionFromMakeModelTypeSubType(SRPLocators.Facets.MakeChild);
            srp.SelectFirstOptionFromMakeModelTypeSubType(SRPLocators.Facets.ModelChild);
            Assert.IsFalse(srp.IsPopularWidgetTitle());
        }

        [Test]
        public void VerifyPopularWidgetForMakeModelTrimSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");
            string eTitleParam2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.sedan") : GetTestData(testDataFile, "BodyTypesFr.sedan");
            
            #endregion
            url = new Uri(srpUrl); 
            
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.Sedan, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
            srp.SelectFirstOptionFromMakeModelTypeSubType(SRPLocators.Facets.MakeChild);
            srp.SelectFirstOptionFromMakeModelTypeSubType(SRPLocators.Facets.ModelChild);
            srp.SelectFirstCheckboxFromFacet(SRPLocators.Facets.TrimChild);
            Assert.IsFalse(srp.IsPopularWidgetTitle());
        }

        [Test]
        public void VerifyPopularWidgetForYearSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");
            string eTitleParam2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.hatchback") : GetTestData(testDataFile, "BodyTypesFr.hatchback");


            #endregion
            url = new Uri(srpUrl);
            string year = GetTestData(testDataFile, "yearRangeData.year5");
           
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.Hatchback, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
            srp.SelectYearFacet(SRPLocators.Year.Both, year, year);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
        }

        [Test]
        public void VerifyPopularWidgetForYearRangeSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");
            string eTitleParam2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.sedan") : GetTestData(testDataFile, "BodyTypesFr.sedan");

            #endregion
            url = new Uri(srpUrl);
            string minYear = GetTestData(testDataFile, "yearRangeData.year5");
            string maxYear = GetTestData(testDataFile, "yearRangeData.year1");

            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.Sedan, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
            srp.SelectYearFacet(SRPLocators.Year.Both, minYear, maxYear);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
        }
        [Test]
        public void VerifyPopularWidgetForExteriorColorSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");
            string eTitleParam2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.sedan") : GetTestData(testDataFile, "BodyTypesFr.sedan");

            #endregion
            url = new Uri(srpUrl);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.Sedan, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
            srp.SelectExteriorColourFacetSingle(SRPLocators.ExteriorColour.Black, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
        }
        [Test]
        public void VerifyPopularWidgetForDriveTrainSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string location = GetTestData(testDataFile, "LocationLinks.defaultLocation");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");
            string eTitleParam2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.sedan") : GetTestData(testDataFile, "BodyTypesFr.sedan");

            #endregion
            url = new Uri(srpUrl);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.Sedan, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
            srp.SelectDrivetrainFacetSingle(SRPLocators.Drivetrain.RearWheelDrive, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");
        }

        [Test]
        public void VerifyPopularWidgetOnProvinceBodyStyleSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string province = GetTestData(testDataFile, "LocationLinks.province");
            string location = GetTestData(testDataFile, "LocationLinks.provinceTitle");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");
            string eTitleParam2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.sedan") : GetTestData(testDataFile, "BodyTypesFr.sedan");

            #endregion
            url = new Uri(srpUrl + province);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, location), "Title text does not match as expected!!!");
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.Sedan, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, location), "Title text does not match as expected!!!");

        }

        [Test]
        public void VerifyPopularWidgetOnCityProvinceBodyStyleSRP()
        {
            #region Variables
            srpUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.srp") : baseURL + GetTestData(testDataFile, "urlsFr.srp");
            string expectedTitle = (language.ToString() == "EN") ? GetTestData(testDataFile, "PopularCarsTitleEn.GeneralSRP") : GetTestData(testDataFile, "PopularCarsTitleFr.GeneralSRP");
            //how to do for EN and FR page optimally
            string city = GetTestData(testDataFile, "LocationLinks.city");
            string province = GetTestData(testDataFile, "LocationLinks.province");
            string eTitleParam1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.general") : GetTestData(testDataFile, "BodyTypesFr.general");
            string eTitleParam2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "BodyTypesEn.sedan") : GetTestData(testDataFile, "BodyTypesFr.sedan");
            #endregion
            url = new Uri(srpUrl + province + "/"+ city);
            Open();
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam1, city), "Title text does not match as expected!!!");
            srp.SelectLocationFacet(GetTestData(testDataFile, "LocationLinks.postalCode"));
            srp.SelectBodyTypeFacetSingle(SRPLocators.BodyType.Sedan, true);
            Assert.AreEqual(srp.GetPopularWidgetTitle(), String.Format(expectedTitle, eTitleParam2, city), "Title text does not match as expected!!!");
        }
    }
}
