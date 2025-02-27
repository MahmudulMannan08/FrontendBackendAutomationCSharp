using MarketPlaceWeb.Base;
using MarketPlaceWeb.Base.SqlDatabase;
using MarketPlaceWeb.Pages;
using MarketPlaceWeb.Pages.SRP;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UIAutomationLibrary.Pages.Dealers;
using UIAutomationLibrary.Pages.Editorials;
using static UIAutomationLibrary.Locators.DealerPagesLocators;


namespace SEO.Tests.DealerPagesTests
{
    class DealerPagesTests : Page
    {
        AzureConfig azureConfig;
        LocalConfig localConfig;
        string dealersUrl;
        string baseURL;
        SRPMain srp;
        DealersMain dealers;
        EditorialMain editorials;
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
            dealers = new DealersMain(driver, viewport, language);
            srp = new SRPMain(driver, viewport, language);
            editorials = new EditorialMain(driver, viewport, language);
            dealersUrl = dealers.dealersURL(language, testDataFile);
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
        public void VerifyBestPricedAwardLogoIsDisplaying()
        {

            #region Variables
            string dealerID = GetTestData(testDataFile, "DealerData.dealerID.dealerWithAward");
            string dealerAwardsURL = (language.ToString() == "EN") ? GetTestData(testDataFile, "urlsEn.dealerAwards") : GetTestData(testDataFile, "urlsFr.dealerAwards");
            #endregion
            url = new Uri(baseURL + dealersUrl + dealerID);
            Open();
            Assert.Multiple(() =>
            {
                Assert.IsTrue(dealers.IsDealerPageWidgetDisplaying(Dealer.BestPricedDealerLogo), "Best Priced Award logo is not displaying");
                dealers.ClickDealersTextLink(Dealer.BestPricedDealerLogo);
                Assert.IsTrue(dealers.IsInCurrentUrl(dealerAwardsURL), "Best priced Award logo is not redirecting to dealer awards page");

            });


        }
        [Test]
        public void VerifyGoogleReviewsLinkIsWorking()
        {

            #region Variables
            string dealerID = GetTestData(testDataFile, "DealerData.dealerID.dealerWithAward");
            #endregion
            url = new Uri(baseURL + dealersUrl + dealerID);
            Open();
            dealers.ClickDealersTextLink(Dealer.GoogleReviews);
            driver.SwitchTo().Window(driver.WindowHandles[1]);
            Assert.IsTrue(dealers.IsInCurrentUrl("maps"), "Google see reviews link is not redirecting to Google maps page");


        }
        [Test]
        public void VerifyVistWebsiteLinkIsWorking()
        {

            #region Variables
            string dealerID = GetTestData(testDataFile, "DealerData.dealerID.dealerID3");
            string dealerWebsite = "https://www.audiwinnipeg.com/";

            #endregion
            url = new Uri(baseURL + dealersUrl + dealerID);
            Open();
            Assert.IsTrue(dealers.GetAltAttribute(Dealer.DealerVistWebsite, "rel").Equals("noopener nofollow"));
            dealers.ClickDealersTextLink(Dealer.DealerVistWebsite);
            driver.SwitchTo().Window(driver.WindowHandles[1]);
            Assert.IsTrue(dealers.IsInCurrentUrl(dealerWebsite), "Visit Website link is not redirecting to Dealers website");

        }
        [Test]
        public void VerifyAllTheWidgetsAreShowingOnTheDealerPage()
        {
            #region Variables
            string dealerID = baseURL.Contains("www.auto")?GetTestData(testDataFile, "DealerData.dealerID.dealerID1"): GetTestData(testDataFile, "DealerData.dealerID.dealerID2");

            #endregion
            url = new Uri(baseURL + dealersUrl + dealerID);
            Open();
            Assert.Multiple(() =>
            {
                Assert.IsTrue(dealers.IsDealerPageWidgetDisplaying(Dealer.Inventory), "Inventory widget is not displaying");
                Assert.IsTrue(dealers.IsDealerPageWidgetDisplaying(Dealer.ExploreRemoteServices), "Explore remote services widget is not displaying");
                Assert.IsTrue(dealers.IsDealerPageWidgetDisplaying(Dealer.VisitTheDealership), "Visit the delaership widget is not displaying");
                Assert.IsTrue(dealers.IsDealerPageWidgetDisplaying(Dealer.ReviewsNews), "ReviewsNews widget is not displaying");

                Assert.IsTrue(dealers.IsDealerPageWidgetDisplaying(Dealer.LeadForm), "Lead form is not displaying");
                Assert.IsTrue(dealers.IsDealerPageWidgetDisplaying(Dealer.DealerEdit), "Dealer edit is not displaying");
            });
            if (language.ToString() == "EN")
            { Assert.IsTrue(dealers.IsDealerPageWidgetDisplaying(Dealer.AutoTraderTV), "Autotrader widget is not displaying"); }
        }

        [Test]
        public void VerifyDealerEditLinkRedirectingTo1Source()
        {
            #region Variables
            string dealerID = GetTestData(testDataFile, "DealerData.dealerID.dealerID2");

            #endregion
            url = new Uri(baseURL + dealersUrl + dealerID);
            Open();
            dealers.ClickDealersTextLink(Dealer.DealerEditLink);
            driver.SwitchTo().Window(driver.WindowHandles[1]);
            Assert.IsTrue(dealers.IsInCurrentUrl("1Source"), "Visit Website link is not redirecting to Dealers website");

        }

        [Test]
        public void VerifyVideoPlaysOnClickingPlayButton()
        {
            #region Variables
            string dealerID = GetTestData(testDataFile, "DealerData.dealerID.dealerID1");

            #endregion
            url = new Uri(baseURL + dealersUrl + dealerID);
            Open();
            if (language.ToString() == "EN")
            {
               
                dealers.ClickDealersTextLink(Dealer.YTPlayButton);
                Assert.IsTrue(dealers.WaitUntilAttributeValueEquals(Dealer.YTPlayButtonHiddden, "class", "hidden"));
            }


        }
        [Test]
        public void VerifyReviewsRedirectToArticlePages()
        {
            #region Variables
            string dealerID = GetTestData(testDataFile, "DealerData.dealerID.dealerID1");

            #endregion
            url = new Uri(baseURL + dealersUrl + dealerID);
            Open();
            dealers.ClickDealersTextLink(Dealer.MainReview);
            Assert.IsTrue(editorials.IsArticleSummaryAvailable(), "Article page is not working");
        }
        [Test]
        public void VerifyInventoryListingRedirectToVDPPage()
        {
            #region Variables
            string dealerID = GetTestData(testDataFile, "DealerData.dealerID.dealerID2");

            #endregion
            url = new Uri(baseURL + dealersUrl + dealerID);
            Open();
            String listingTitle = dealers.GetInventoryListingTitle(0,Dealer.InventoryListing);
            Console.WriteLine(listingTitle);
            dealers.ClickDealersTextLink(Dealer.InventoryListing);
            dealers.WaitForPageLoad("/a/");
            Assert.IsTrue(dealers.GetH1TagText(Dealer.H1Tag).Contains(listingTitle), "HeroCard Title of the Vehicle din't match with SRP Listing Title");

        }
        [Test]
        public void VerifyTermsOfUseLinkIsWorking()
        {
            #region Variables
            string dealerID = GetTestData(testDataFile, "DealerData.dealerID.dealerID2");
            string H1 = (language.ToString() == "EN") ? "Privacy":"Renseignements personnels et non-responsabilité";
            #endregion
            url = new Uri(baseURL + dealersUrl + dealerID);
            Open();
            dealers.ClickDealersTextLink(Dealer.TermsOfUseLink);
            driver.SwitchTo().Window(driver.WindowHandles[1]);

            dealers.WaitForPageLoad("privacy-statement");
            Assert.IsTrue(dealers.GetH1TagText(Dealer.H1Tag).Contains(H1), "Privacy page is not working");
        }

        [Test]
        public void VerifyNonDealerUrlIsWorking()
        {
            #region Variables
            string dealerID = GetTestData(testDataFile, "DealerData.dealerID.nonDealer");
            string h1 = "Calmont";
            #endregion
            url = new Uri(baseURL + dealersUrl + dealerID);
            Open();
            WaitForPageLoad(20);
            Assert.IsTrue(dealers.GetH1TagText(Dealer.NonDealerH1).Contains(h1), "Non dealer page is not working");

        }

        [Test]
        public void VerifyNonDealerPageListingsNavigateToVDP()
        {
            #region Variables
            string dealerID = GetTestData(testDataFile, "DealerData.dealerID.nonDealer");
           
            #endregion
            url = new Uri(baseURL + dealersUrl + dealerID);
            Open();
            WaitForPageLoad(20);
            String listingTitle = dealers.GetInventoryListingTitle(0, Dealer.NonDealerInventoryListings);
            dealers.ClickOnNonDealerInventoryListings(0);
            dealers.WaitForPageLoad(20);
            Assert.IsTrue(dealers.GetH1TagText(Dealer.H1Tag).Contains(listingTitle), "Listing Title of the Vehicle didn't match with VDP page");
        }
        [Test]
        public void VerifyNonDealerPageViewInventoryLinkNavigateToSRP()
        {
            #region Variables
            string dealerID = GetTestData(testDataFile, "DealerData.dealerID.nonDealer");
            string h1 = (language.ToString() == "EN") ? "New & Used Cars for sale in Edmonton" : "Autos neuves et d'occasion à vendre - Edmonton";
            #endregion
            url = new Uri(baseURL + dealersUrl + dealerID);
            Open();
            WaitForPageLoad(20);
            dealers.ClickDealersTextLink(Dealer.NonDealerViewInventoryLink);
            driver.SwitchTo().Window(driver.WindowHandles[1]);
            WaitForPageLoad(20);
            Assert.IsTrue(dealers.GetH1TagText(Dealer.H1Tag).Contains(h1), "View Inventory button is not navigated to SRP page");

        }

        [Test]
        public void VerifyNonDealerPageFindOutHowLinkNavigateToDealerSignupPage()
        {
            #region Variables
            string dealerID = GetTestData(testDataFile, "DealerData.dealerID.nonDealer");
            string h1 = (language.ToString() == "EN") ? "Automotive Dealership Digital Marketing Solutions":"Solutions de marketing numérique pour commerçants automobiles";
            #endregion
            url = new Uri(baseURL + dealersUrl + dealerID);
            Open();
            WaitForPageLoad(20);
            dealers.ClickOnFindOutHowLink(0);
            driver.SwitchTo().Window(driver.WindowHandles[1]);
            WaitForPageLoad(20);
            Assert.IsTrue(dealers.GetH1TagText(Dealer.H1Tag).Contains(h1), "Find out How button is not linked to Dealer signup page");
        }

        [Test]
        public void GeneralInquiryLead()
        {
            if (baseURL.Contains("beta"))
                {
                #region Variables
                string dealerInventoryGILeadConsumerTopicId = GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.dealerInventoryGILeadConsumerTopicId");
                string dealerInventoryGILeadDealerTopicId = GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.dealerInventoryGILeadDealerTopicId");
                string dealerInventoryGILead1STopicId = GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.dealerInventoryGILead1STopicId");
                string dealerInventoryPageUrl = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureLeads.dealerInventoryPageEn.dealerInventoryPageUrl") : GetTestData(testDataFile, "commonTestData.featureLeads.dealerInventoryPageFr.dealerInventoryPageUrl");

                DateTime EmailCreatedUtc;
                StatisticDatabase statisticDatabase;
                #endregion
                url = new Uri(baseURL + dealerInventoryPageUrl);

                statisticDatabase = new StatisticDatabase(driver, GetTestData(testDataFile, "connectionStrings.azrmdbqa1Statistic"));
                EmailCreatedUtc = DateTime.UtcNow;
                Open();

                DealerInventoryLeadForm dealerInventoryLead = new DealerInventoryLeadForm
                {
                    LeadType = DealerInventoryLeadForm.DealerInventoryLeads.GeneralInquiry,
                    Name = "MP DIPGeneralInqLeadTest",
                    Email = "DIPGeneralInq_" + Extensions.GenerateRandomString(6) + "@trader.ca",
                    PhoneNumber = GetTestData(testDataFile, "commonTestData.featureLeads.phoneNumber"),
                    Message = "This is dealer inventory page general inquiry lead from SEO Automation"
                };

                srp.SubmitLeadForm(dealerInventoryLead);
                Assert.True(srp.IsDipLeadFeedbackMsgDisplayed(), "Dealer Inventory Page Lead submission message is not displayed for lead: " + Enum.GetName(typeof(DealerInventoryLeadForm.DealerInventoryLeads), (int)dealerInventoryLead.LeadType));

                srp.ClickOKBtnDipLead();

                var expecteddealerInventoryGILeadTopicIdList = new List<string> { dealerInventoryGILeadConsumerTopicId, dealerInventoryGILeadDealerTopicId, dealerInventoryGILead1STopicId };
                var actualdealerInventoryGILeadTopicIdList = statisticDatabase.GetDealerInventoryLeadTopicId(dealerInventoryLead.Email, EmailCreatedUtc, 3);
                Assert.True(Enumerable.SequenceEqual(expecteddealerInventoryGILeadTopicIdList.OrderBy(x => x), actualdealerInventoryGILeadTopicIdList.OrderBy(x => x)), "Expected number of / matching topic ID emails not sent. Topic ID list of sent email(s): " + string.Join(", ", actualdealerInventoryGILeadTopicIdList.ToList()));  //Dealer Inventory Page - General Inquiry Lead Email(3 emails - Consumer, Dealer, ADF) Verification by Topic ID
            }

        }
    }
}
