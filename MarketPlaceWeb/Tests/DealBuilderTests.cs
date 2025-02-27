using MarketPlaceWeb.Base;
using MarketPlaceWeb.Pages;
using MarketPlaceWeb.Pages.Shared;
using MarketPlaceWeb.Pages.VDP;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketPlaceWeb.Test
{
    [TestFixture]
    class DealBuilderTests : Page
    {
        SRPMain srp;
        SharedMain shared;
        string baseURL;
        AzureConfig azureConfig;
        LocalConfig localConfig;
        dynamic testDataFile;
        string testcaseId;
        VDPMain vdp;

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
            vdp = new VDPMain(driver, viewport, language);
            shared = new SharedMain(driver, viewport, language);
            testcaseId = (string)TestContext.CurrentContext.Test.Properties.Get("TestCaseId");

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

        [Test, Property("TestCaseId", "56235")]
        public void VerifyBuyOnline_PaymentCalculatorWidgetOnVDPWhenDealerHasDigitalRetailsEnabled()
        {
            var testDataPL = new List<string> { GetTestData(testDataFile, $"{testcaseId}.payCalcPL") };
            var testDataTS = new List<string> { GetTestData(testDataFile, $"{testcaseId}.payCalcTS") };
            var testDataOrganic = new List<string> { GetTestData(testDataFile, $"{testcaseId}.payCalcOrganic") };
            var testDataBoost = new List<string> { GetTestData(testDataFile, $"{testcaseId}.payCalcBoost") };
            var testDataBoostHL = new List<string> { GetTestData(testDataFile, $"{testcaseId}.payCalcBoostHL") };
            var testDataPPL = new List<string> { GetTestData(testDataFile, $"{testcaseId}.payCalcPPL") };
            var testDataXPL = new List<string> { GetTestData(testDataFile, $"{testcaseId}.payCalcXPL") };

            Assert.Multiple(() =>
            {
                foreach (var testURL in testDataPL.Concat(testDataTS).Concat(testDataOrganic).Concat(testDataBoost).Concat(testDataBoostHL).Concat(testDataPPL).Concat(testDataXPL))
                {
                    if (viewport.Equals("XS") && testURL.Contains("ursrc=pl"))
                    {
                        continue; // Skip iteration for XS viewport and ursrc=pl
                    }
                    url = new Uri(baseURL + testURL);                                        
                    Open();                    
                    Assert.IsTrue(vdp.CheckPaymentCalculatorOnVDPWidget(), "The payment calculator widget is not displayed ");
                    Assert.IsTrue(vdp.CheckPaymentCalculatorOnVDPPills(), "The payment calculator pills is not displayed ");
                    Assert.IsTrue(vdp.VerifyICOWidgetrIsNotDisplayed(), "The ICO widget is displayed for Deal builder");                    
                    Assert.IsTrue(vdp.VerifyMIQueryParams(viewport, url), "The MI Query params are not validated, please investigate ");
                    Assert.IsTrue(vdp.CheckPaymentCalculatorOnVDPLeadForms(), "The payment calculator CTA on lead form is not displayed or clickable or It is displayed in XS viewport ");

                }
            });
        }

        [Test, Property("TestCaseId", "56233")]
        public void VerifyReserveITWidgetRankingWhenDealBuilderEnabled()
        {
            var testURL = GetTestData(testDataFile, $"{testcaseId}.payCalc");
            url = new Uri(baseURL + testURL);
            Open();
            Assert.IsTrue(vdp.VerifyReserveItWidgetRankingWhenDealBuilderIsEnabled(), "The reserve it order seem incorrect when Deal Builder is enabled");
        }

        [Test, Property("TestCaseId", "61579")]
        public void VerifyPaymentCalculatorPillsOnTheSRP()
        {
            var testURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.srpUrl");
            url = new Uri(baseURL + testURL);
            Open();
            Assert.IsTrue(srp.VerifyPaymentCalculatorPillsOrderOnSRP(), "The payment calculator pill is not displayed or is not available in the first position");


        }
    }
}
