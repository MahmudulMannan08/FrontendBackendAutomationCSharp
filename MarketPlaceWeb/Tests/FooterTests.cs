using MarketPlaceWeb.Base;
using MarketPlaceWeb.Pages.HP;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;

namespace MarketPlaceWeb.Test
{
    class FooterTests : Page
    {
        string baseURL;
        AzureConfig azureConfig;
        LocalConfig localConfig;
        HPMain hp;

        [OneTimeSetUp]
        public void FixtureInit()
        {
            azureConfig = new AzureConfig();
            localConfig = new LocalConfig();
            baseURL = (azureConfig.isAzureEnabled) ? azureConfig.uri.ToString() : localConfig.uri.ToString();
            language = (azureConfig.isAzureEnabled) ? azureConfig.language : localConfig.language;
            viewport = (azureConfig.isAzureEnabled) ? azureConfig.GetViewport() : localConfig.GetViewport();
        }

        [SetUp]
        public void Init()
        {
            driver = (azureConfig.isAzureEnabled) ? azureConfig.GetDriverFromAzure() : localConfig.GetDriverFromLocal();
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
        [Test, Property("TestCaseId", "8981")]
        public void VerifyFooterLinks()
        {
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            url = new Uri(baseURL);
            Open();
            var footerLinkElements = hp.GetFooterLinkElements();
            var isValidLink = true;
            var footerLinks = new Dictionary<string, string>();
            Assert.Multiple(() =>
            {
                foreach (var footerLinkElement in footerLinkElements)
                {
                    Assert.True(hp.IsElementEnabled(footerLinkElement) && footerLinkElement.Displayed, $"Footer link {footerLinkElement.Text} is not enabled or displayed");
                    footerLinks.Add(footerLinkElement.Text, footerLinkElement.GetAttribute("href"));
                }
            });
            Assert.Multiple(() =>
            {
                foreach (var footerLink in footerLinks)
                {
                    try
                    {
                        url = new Uri(footerLink.Value);
                        Open();
                        isValidLink = (language.ToString() == "EN" ? driver.Title.Contains("Oops") : driver.Title.Contains("Oups")) || driver.Title.Contains("404") || driver.Title.Contains("Page not found") ? false : true;
                    }
                    catch (Exception)
                    {
                        isValidLink = false;
                    }
                    TestContext.WriteLine($"Footer link: {footerLink.Key}, WebPage URL: {driver.Url}, Webpage title: {driver.Title}, isValidLink : {isValidLink} ");
                    Assert.True(isValidLink, $"Footer link {footerLink.Key} is broken");
                    isValidLink = true;
                }
            });

        }
    }
}