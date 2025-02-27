using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using MarketPlaceWeb.Base;
using MarketPlaceWeb.Pages;
using MarketPlaceWeb.Pages.HP;
using MarketPlaceWeb.Locators;

namespace SEO.Tests
{
    class BodyTypeSRPTests : Page
    {
        AzureConfig azureConfig;
        LocalConfig localConfig;
        SRPMain srp;
        HPMain hp;

        string baseURL;

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
        public void VeryfySUVBodystyleSRPNavigation()
        {

            #region Variables
            //string distance = "+100 km";
            string bodyType = (language.ToString() == "EN") ? "SUV" : "VUS";
            string CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.suvSRP") : baseURL + GetTestData(testDataFile, "urlsFr.suvSRP");
            string LocalSRPLink = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "LocalSRPLinksEn.suv.Barrie") : baseURL + GetTestData(testDataFile, "LocalSRPLinksFr.suv.Alma");
            #endregion
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(0);
            string Locallink = srp.GetLocalSRPLink();
            //Assert.AreEqual(distance, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Distance), "The Distance facet value is not " + distance + " for the url " + url);
            Assert.AreEqual(bodyType, srp.GetSelectedValueOfFacet(SRPLocators.Facets.BodyTypeChild), "The Body Type facet value is not " + bodyType + " for the url " + url);
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
            Assert.IsTrue(Locallink.Equals(LocalSRPLink), "Local srp link url is notcorrect" + Locallink);


        }
        [Test]
        public void VeryfyTruckBodystyleSRPNavigation()
        {

            #region Variables
            //string distance = "+100 km";
            string bodyType = (language.ToString() == "EN") ? "Truck" : "Camion";
            string CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.truckSRP") : baseURL + GetTestData(testDataFile, "urlsFr.truckSRP");
            string LocalSRPLink = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "LocalSRPLinksEn.truck.Barrie") : baseURL + GetTestData(testDataFile, "LocalSRPLinksFr.truck.Alma");
            #endregion
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(1);
            string Locallink = srp.GetLocalSRPLink();
            //Assert.AreEqual(distance, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Distance), "The Distance facet value is not " + distance + " for the url " + url);
            Assert.AreEqual(bodyType, srp.GetSelectedValueOfFacet(SRPLocators.Facets.BodyTypeChild), "The Body Type facet value is not " + bodyType + " for the url " + url);
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
            Assert.IsTrue(Locallink.Equals(LocalSRPLink), "Local srp link url is not correct" + Locallink);
        }
        [Test]
        public void VeryfySedanBodystyleSRPNavigation()
        {

            #region Variables
            //string distance = "+100 km";
            string bodyType = (language.ToString() == "EN") ? "Sedan" : "Berline";
            string CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.sedanSRP") : baseURL + GetTestData(testDataFile, "urlsFr.sedanSRP");
            string LocalSRPLink = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "LocalSRPLinksEn.sedan.Barrie") : baseURL + GetTestData(testDataFile, "LocalSRPLinksFr.sedan.Alma");
            #endregion
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(2);
            string Locallink = srp.GetLocalSRPLink();
            //Assert.AreEqual(distance, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Distance), "The Distance facet value is not " + distance + " for the url " + url);
            Assert.AreEqual(bodyType, srp.GetSelectedValueOfFacet(SRPLocators.Facets.BodyTypeChild), "The Body Type facet value is not " + bodyType + " for the url " + url);
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
            Assert.IsTrue(Locallink.Equals(LocalSRPLink), "Local srp link url is notcorrect" + Locallink);
        }
        [Test]
        public void VeryfyCoupeBodystyleSRPNavigation()
        {

            #region Variables
            //string distance = "+100 km";
            string bodyType = (language.ToString() == "EN") ? "Coupe" : "Coupé";
            string CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.coupeSRP") : baseURL + GetTestData(testDataFile, "urlsFr.coupeSRP");
            string LocalSRPLink = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "LocalSRPLinksEn.coupe.Barrie") : baseURL + GetTestData(testDataFile, "LocalSRPLinksFr.coupe.Alma");
            #endregion
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(3);
            string Locallink = srp.GetLocalSRPLink();
            //Assert.AreEqual(distance, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Distance), "The Distance facet value is not " + distance + " for the url " + url);
            Assert.AreEqual(bodyType, srp.GetSelectedValueOfFacet(SRPLocators.Facets.BodyTypeChild), "The Body Type facet value is not " + bodyType + " for the url " + url);
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
            Assert.IsTrue(Locallink.Equals(LocalSRPLink), "Local srp link url is notcorrect" + Locallink);
        }
        [Test]
        public void VeryfyMiniVanBodystyleSRPNavigation()
        {
            #region Variables
            //string distance = "+100 km";
            string bodyType = (language.ToString() == "EN") ? "Minivan" : "Minifourgonnette ou fourgonnette";
            string CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.minivanSRP") : baseURL + GetTestData(testDataFile, "urlsFr.minivanSRP");
            string LocalSRPLink = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "LocalSRPLinksEn.minivan.Barrie") : baseURL + GetTestData(testDataFile, "LocalSRPLinksFr.minivan.Alma");
            #endregion
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(4);
            string Locallink = srp.GetLocalSRPLink();
            //Assert.AreEqual(distance, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Distance), "The Distance facet value is not " + distance + " for the url " + url);
            Assert.AreEqual(bodyType, srp.GetSelectedValueOfFacet(SRPLocators.Facets.BodyTypeChild), "The Body Type facet value is not " + bodyType + " for the url " + url);
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
            Assert.IsTrue(Locallink.Equals(LocalSRPLink), "Local srp link url is notcorrect" + Locallink);
        }
        [Test]
        public void VeryfyHatchbackBodystyleSRPNavigation()
        {

            #region Variables
            //string distance = "+100 km";
            string bodyType = (language.ToString() == "EN") ? "Hatchback" : "Hatchback";
            string CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.hatchbackSRP") : baseURL + GetTestData(testDataFile, "urlsFr.hatchbackSRP");
            string LocalSRPLink = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "LocalSRPLinksEn.hatchback.Barrie") : baseURL + GetTestData(testDataFile, "LocalSRPLinksFr.hatchback.Alma");
            #endregion
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(5);
            string Locallink = srp.GetLocalSRPLink();
            //Assert.AreEqual(distance, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Distance), "The Distance facet value is not " + distance + " for the url " + url);
            Assert.AreEqual(bodyType, srp.GetSelectedValueOfFacet(SRPLocators.Facets.BodyTypeChild), "The Body Type facet value is not " + bodyType + " for the url " + url);
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
            Assert.IsTrue(Locallink.Equals(LocalSRPLink), "Local srp link url is notcorrect" + Locallink);
        }
        [Test]
        public void VeryfyConvertibleBodystyleSRPNavigation()
        {

            #region Variables
            //string distance = "+100 km";
            string bodyType = (language.ToString() == "EN") ? "Convertible" : "Décapotable ou cabriolet";
            string CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.convertibleSRP") : baseURL + GetTestData(testDataFile, "urlsFr.convertibleSRP");
            string LocalSRPLink = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "LocalSRPLinksEn.convertible.Barrie") : baseURL + GetTestData(testDataFile, "LocalSRPLinksFr.convertible.Alma");
            #endregion
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(6);
            string Locallink = srp.GetLocalSRPLink();
            //Assert.AreEqual(distance, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Distance), "The Distance facet value is not " + distance + " for the url " + url);
            Assert.AreEqual(bodyType, srp.GetSelectedValueOfFacet(SRPLocators.Facets.BodyTypeChild), "The Body Type facet value is not " + bodyType + " for the url " + url);
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
            Assert.IsTrue(Locallink.Equals(LocalSRPLink), "Local srp link url is notcorrect" + Locallink);
        }
        [Test]
        public void VeryfyWagonBodystyleSRPNavigation()
        {

            #region Variables
            //string distance = "+100 km";
            string bodyType = (language.ToString() == "EN") ? "Wagon" : "Familiale";
            string CanonicalUrl = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "urlsEn.wagonSRP") : baseURL + GetTestData(testDataFile, "urlsFr.wagonSRP");
            string LocalSRPLink = (language.ToString() == "EN") ? baseURL + GetTestData(testDataFile, "LocalSRPLinksEn.wagon.Barrie") : baseURL + GetTestData(testDataFile, "LocalSRPLinksFr.wagon.Alma");
            #endregion
            url = new Uri(baseURL);
            Open();
            hp.ClickOnBodyTypeLink(7);
            string Locallink = srp.GetLocalSRPLink();
            //Assert.AreEqual(distance, srp.GetSelectedValueOfFacet(SRPLocators.Facets.Distance), "The Distance facet value is not " + distance + " for the url " + url);
            Assert.AreEqual(bodyType, srp.GetSelectedValueOfFacet(SRPLocators.Facets.BodyTypeChild), "The Body Type facet value is not " + bodyType + " for the url " + url);
            Assert.IsTrue(IsInCurrentUrl(CanonicalUrl), "BrowserUrl doesn't match with Canonical Url");
            Assert.IsTrue(Locallink.Equals(LocalSRPLink), "Local srp link url is notcorrect" + Locallink);
        }
    }
}
