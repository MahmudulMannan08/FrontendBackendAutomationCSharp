using MarketPlaceWeb.Base;
using MarketPlaceWeb.Base.SqlDatabase;
using MarketPlaceWeb.Driver;
using MarketPlaceWeb.Pages;
using MarketPlaceWeb.Pages.Shared;
using MarketPlaceWeb.Pages.SRP;
using MarketPlaceWeb.Pages.VDP;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MarketPlaceWeb.Test
{
    [TestFixture]
    public class SubmitLeadTests : Page
    {
        SRPMain srp;
        VDPMain vdp;
        string baseURL;
        AzureConfig azureConfig;
        LocalConfig localConfig;
        dynamic testDataFile;
        DateTime EmailCreatedUtc;
        StatisticDatabase statisticDatabase;

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

            statisticDatabase = new StatisticDatabase(driver, GetTestData(testDataFile, "connectionStrings.azrmdbqa1Statistic"));
            EmailCreatedUtc = DateTime.UtcNow;
            srpVariant = (azureConfig.isAzureEnabled) ? azureConfig.srpVariant : (viewport.ToString() == "XS") ?
                GetTestData(testDataFile, "optimizelyCookies.srpRedesign.variantXS") :
                GetTestData(testDataFile, "optimizelyCookies.srpRedesign.variantDT");
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
            statisticDatabase.Dispose();
            driver.Quit();
        }

        [Test, Property("TestCaseId", "5976")]
        public void VerifySubmitLeadForGeneralInquiry()
        {
            #region Variables
            string feedbackMessage1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureLeads.feedbackMessageEn1") : GetTestData(testDataFile, "commonTestData.featureLeads.feedbackMessageFr1");
            string feedbackMessage2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureLeads.feedbackMessageEn2") : GetTestData(testDataFile, "commonTestData.featureLeads.feedbackMessageFr2");
            string generalInquiryLeadConsumerTopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.generalInquiryLeadConsumerTopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.generalInquiryLeadConsumerTopicId");
            string generalInquiryLeadDealerTopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.generalInquiryLeadDealerTopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.generalInquiryLeadDealerTopicId");
            string generalInquiryLead_1S_TopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.generalInquiryLead_1S_TopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.generalInquiryLead_1S_TopicId");
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            dynamic vdpAds = JsonConvert.DeserializeObject(GetTestData(testDataFile, $"recursiveData.{testcaseId}.ads"));
            #endregion
            Assert.Multiple(() => 
            {
                foreach (var adItem in vdpAds)
                {
                    vdp.GoToVDP(baseURL, adItem.Value.ToString());
                    EmailLeadForm emailLeadForm = new EmailLeadForm
                    {
                        LeadForm = EmailLeadForm.EmailLeads.GeneralInquiry,
                        Name = "MP GeneralInquiryLeadTest",
                        Email = "GeneralInquiryLead_" + Extensions.GenerateRandomString(6) + "@trader.ca",
                        PhoneNumber = GetTestData(testDataFile, "commonTestData.featureLeads.phoneNumber"),
                        Message = "This is General Inquiry from Marketplace Web Automation"
                    };
                    vdp.SubmitLeadForm(emailLeadForm);
                    Assert.AreEqual(feedbackMessage1, vdp.GetEmailLeadFeedbackMessage1(), "Feedback message does not display after submitting the lead.");
                    Assert.AreEqual(feedbackMessage2, vdp.GetEmailLeadFeedbackMessage2(), "Feedback message does not display after submitting the lead.");
                    vdp.ClickOnOkBtnOnEmailLeadFeedbackDialog();
                    vdp.IsEmailLeadFormAvailable();

                    var expectedGeneralInquiryLeadTopicIdList = new List<string> { generalInquiryLeadConsumerTopicId, generalInquiryLeadDealerTopicId, generalInquiryLead_1S_TopicId };
                    var actualGeneralInquiryLeadTopicIdList = statisticDatabase.GetEmailTopicId(emailLeadForm.Email, EmailCreatedUtc, 3);  //Bug: Statistic DB is not creating record Log for ADF email for leads. Reference: https://trader.atlassian.net/browse/CONS-1558 [Bug fixed]
                    Assert.True(Enumerable.SequenceEqual(expectedGeneralInquiryLeadTopicIdList.OrderBy(x => x), actualGeneralInquiryLeadTopicIdList.OrderBy(x => x)), "Expected number of / matching topic ID emails not sent. Topic ID list of sent email(s): " + string.Join(", ", actualGeneralInquiryLeadTopicIdList.ToList()));  //General Inquiry Lead Email(3 emails - Consumer, Dealer, ADF) Verification by Topic ID
                }
            });
        }

        [Test, Property("TestCaseId", "6009")]
        public void VerifyMakeAnOfferLeadOnXS()
        {
            #region Variables
            string makeAnOfferLeadConsumerTopicId = GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.makeAnOfferLeadConsumerTopicId") ;
            string makeAnOfferLeadDealerTopicId =GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.makeAnOfferLeadDealerTopicId");
            string makeAnOfferLead1STopicId = GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.makeAnOfferLead1STopicId");
            string feedbackMessage1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureLeads.feedbackMessageEn1") : GetTestData(testDataFile, "commonTestData.featureLeads.feedbackMessageFr1");
            string feedbackMessage2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureLeads.feedbackMessageEn2") : GetTestData(testDataFile, "commonTestData.featureLeads.feedbackMessageFr2");
            string name = GetTestData(testDataFile, "6009.Name");
            string message = GetTestData(testDataFile, "6009.Message");
            string myOffer = GetTestData(testDataFile, "6009.MyOffer");
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            dynamic vdpUrls = JsonConvert.DeserializeObject(GetTestData(testDataFile, $"{testcaseId}.Urls"));
            #endregion
            if ((viewport.ToString() == "XS"))
            {
                Assert.Multiple(() =>
                {
                    foreach (var vdpUrlItem in vdpUrls)
                    {
                        url = new Uri(baseURL + vdpUrlItem.Value.ToString());
                        Open();
                        EmailLeadForm emailLeadForm = new EmailLeadForm
                        {
                            LeadForm = EmailLeadForm.EmailLeads.MakeAnOffer,
                            Name = name,
                            Email = "MakeAnOfferLead_" + Extensions.GenerateRandomString(6) + "@trader.ca",
                            PhoneNumber = GetTestData(testDataFile, "commonTestData.featureLeads.phoneNumber"),
                            Message = message,
                            MyOffer = (viewport.ToString() == "XS") ? myOffer : null
                        };

                        vdp.SubmitLeadForm(emailLeadForm);
                        Assert.AreEqual(feedbackMessage1, vdp.GetEmailLeadFeedbackMessage1(), "Feedback message does not display after submitting the lead.");
                        Assert.AreEqual(feedbackMessage2, vdp.GetEmailLeadFeedbackMessage2(), "Feedback message does not display after submitting the lead.");
                        vdp.ClickOnOkBtnOnEmailLeadFeedbackDialog();
                        Assert.True(vdp.IsEmailLeadFormAvailable(), "Lead form is not displayed.");

                        var expectedMakeAnOfferLeadTopicIdList = new List<string> { makeAnOfferLeadConsumerTopicId, makeAnOfferLeadDealerTopicId, makeAnOfferLead1STopicId };
                        var actualMakeAnOfferLeadTopicIdList = statisticDatabase.GetEmailTopicId(emailLeadForm.Email, EmailCreatedUtc, 3);
                        Assert.True(Enumerable.SequenceEqual(expectedMakeAnOfferLeadTopicIdList.OrderBy(x => x), actualMakeAnOfferLeadTopicIdList.OrderBy(x => x)), "Expected number of / matching topic ID emails not sent. Topic ID list of sent email(s): " + string.Join(", ", actualMakeAnOfferLeadTopicIdList.ToList()));  //Make An Offer Lead Email(3 emails - Consumer, Dealer, ADF) Verification by Topic ID
                }
                });

            }
        }

        [Test, Property("TestCaseId", "5842")]
        public void VerifyBookAnAppointment()
        {
            #region Variables
            string bookAnAppointmentLeadConsumerTopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.bookAnAppointmentLeadConsumerTopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.generalInquiryLeadConsumerTopicId");
            string bookAnAppointmentLeadDealerTopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.bookAnAppointmentLeadDealerTopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.generalInquiryLeadDealerTopicId");
            string bookAnAppointmentLead1STopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.bookAnAppointmentLead1STopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.generalInquiryLead_1S_TopicId");
            string feedbackMessage1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureLeads.feedbackMessageEn1") : GetTestData(testDataFile, "commonTestData.featureLeads.feedbackMessageFr1");
            string feedbackMessage2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureLeads.feedbackMessageEn2") : GetTestData(testDataFile, "commonTestData.featureLeads.feedbackMessageFr2");
            string name = GetTestData(testDataFile, "5842.Name");
            string message = GetTestData(testDataFile, "5842.Message");
            var vdpUrls = GetTestData(testDataFile, "5842.Urls");
            dynamic deserializedObject = JsonConvert.DeserializeObject(vdpUrls);

            #endregion

            Assert.Multiple(() =>
            {
                foreach (var item in deserializedObject)
                {
                    var vdpUrl = item.Value;
                    url = new Uri(baseURL + vdpUrl);
                    Open();

                    EmailLeadForm emailLeadForm = new EmailLeadForm
                    {
                        LeadForm = ((viewport.ToString() == "XS") ? EmailLeadForm.EmailLeads.BookAnAppointment : EmailLeadForm.EmailLeads.CSBadgeBookAnAppointment),
                        Name = ((viewport.ToString() == "XS") ? name:"MP GeneralInquiryLeadTest"),
                        Email = ((viewport.ToString() == "XS") ? "BookAnAppointmentLead_" + Extensions.GenerateRandomString(6) + "@trader.ca" : "GeneralInquiryLead_" + Extensions.GenerateRandomString(6) + "@trader.ca"),
                        PhoneNumber = GetTestData(testDataFile, "commonTestData.featureLeads.phoneNumber"),
                        PreferredDate = ((viewport.ToString() == "XS") ? DateTime.Now.AddDays(1).ToString("M/d/yyyy", CultureInfo.InvariantCulture): null),
                        TimeOfDay = ((viewport.ToString() == "XS") ? "1":null),
                        Message = ((viewport.ToString() == "XS") ? message : "This is General Inquiry from Marketplace Web Automation")                       
                    };

                    vdp.SubmitLeadForm(emailLeadForm);
                    Assert.AreEqual(feedbackMessage1, vdp.GetEmailLeadFeedbackMessage1(), "\"Thank You!\" message does not display after submitting the lead.");
                    Assert.AreEqual(feedbackMessage2, vdp.GetEmailLeadFeedbackMessage2(), "\"Your inquiry has been sent.\" message does not display after submitting the lead.");
                    vdp.ClickOnOkBtnOnEmailLeadFeedbackDialog();
                    Assert.True(vdp.IsEmailLeadFormAvailable(), "Lead form is not displayed.");

                    var expectedBookAnAppointmentLeadTopicIdList = new List<string> { bookAnAppointmentLeadConsumerTopicId, bookAnAppointmentLeadDealerTopicId, bookAnAppointmentLead1STopicId };
                    var actualBookAnAppointmentLeadTopicIdList = statisticDatabase.GetEmailTopicId(emailLeadForm.Email, EmailCreatedUtc, 3);
                    Assert.True(Enumerable.SequenceEqual(expectedBookAnAppointmentLeadTopicIdList.OrderBy(x => x), actualBookAnAppointmentLeadTopicIdList.OrderBy(x => x)), "Expected number of / matching topic ID emails not sent. Topic ID list of sent email(s): " + string.Join(", ", actualBookAnAppointmentLeadTopicIdList.ToList()));  //Book An Appointment Lead Email(3 emails - Consumer, Dealer, ADF) Verification by Topic ID
                }
            });
        }

        [Test, Property("TestCaseId", "8464")]
        public void ViewMapBAALead()
        {
            #region Variables
            string viewMapBAALeadConsumerTopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.viewMapBAALeadConsumerTopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.viewMapBAALeadConsumerTopicId");
            string viewMapBAALeadDealerTopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.viewMapBAALeadDealerTopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.viewMapBAALeadDealerTopicId");
            string viewMapBAALead1STopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.viewMapBAALead1STopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.viewMapBAALead1STopicId");
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            dynamic vdpAds = JsonConvert.DeserializeObject(GetTestData(testDataFile, $"recursiveData.{testcaseId}.ads"));
            #endregion
            Assert.Multiple(() => 
            {
                foreach (var adItem in vdpAds)
                {
                    vdp.GoToVDP(baseURL, adItem.Value.ToString());
                    EmailLeadForm emailLeadForm = new EmailLeadForm
                    {
                        LeadForm = EmailLeadForm.EmailLeads.ViewMapLead,
                        Name = "MP ViewMapLeadTest",
                        Email = "ViewMapLead_" + Extensions.GenerateRandomString(6) + "@trader.ca",
                        PhoneNumber = GetTestData(testDataFile, "commonTestData.featureLeads.phoneNumber"),
                        PreferredDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy", CultureInfo.InvariantCulture),
                        TimeOfDay = "1",
                        Message = "This is View Map BAA lead from Marketplace Web Automation"
                    };

                    vdp.SubmitLeadForm(emailLeadForm);
                    Assert.True(vdp.IsEmailLeadFeedbackMsgDisplayed(emailLeadForm), "Lead submission message is not displayed");

                    var expectedViewMapLeadTopicIdList = new List<string> { viewMapBAALeadConsumerTopicId, viewMapBAALeadDealerTopicId, viewMapBAALead1STopicId };
                    var actualViewMapLeadTopicIdList = statisticDatabase.GetEmailTopicId(emailLeadForm.Email, EmailCreatedUtc, 3);
                    Assert.True(Enumerable.SequenceEqual(expectedViewMapLeadTopicIdList.OrderBy(x => x), actualViewMapLeadTopicIdList.OrderBy(x => x)), "Expected number of / matching topic ID emails not sent. Topic ID list of sent email(s): " + string.Join(", ", actualViewMapLeadTopicIdList.ToList()));  //View Map BAA Lead Email(3 emails - Consumer, Dealer, ADF) Verification by Topic ID
                }
            });
        }

        [Test, Property("TestCaseId", "8451")]
        public void HomeTestDriveBAALead()
        {
            #region Variables
            string homeTestDriveLeadConsumerTopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.homeTestDriveLeadConsumerTopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.generalInquiryLeadConsumerTopicId");
            string homeTestDriveLeadDealerTopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.homeTestDriveLeadDealerTopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.generalInquiryLeadDealerTopicId");
            string homeTestDriveLead1STopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.homeTestDriveLead1STopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.generalInquiryLead_1S_TopicId");
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            dynamic vdpAds = JsonConvert.DeserializeObject(GetTestData(testDataFile, $"recursiveData.{testcaseId}.ads"));            
            #endregion
            Assert.Multiple(() =>
            {
                foreach (var adItem in vdpAds)
                {
                    vdp.GoToVDP(baseURL, adItem.Value.ToString());
                    EmailLeadForm emailLeadForm = new EmailLeadForm
                    {
                        LeadForm = EmailLeadForm.EmailLeads.CSBadgeHomeTestDrive,
                        Name = "MP HomeTestDriveLeadTest",
                        Email = "HomeTestDrive_" + Extensions.GenerateRandomString(6) + "@trader.ca",
                        PhoneNumber = GetTestData(testDataFile, "commonTestData.featureLeads.phoneNumber"),
                        PreferredDate = ((viewport.ToString() == "XS") ? DateTime.Now.AddDays(1).ToString("M/d/yyyy", CultureInfo.InvariantCulture) : null),
                        TimeOfDay = ((viewport.ToString() == "XS") ? "1" : null),
                        Message = "This is Home Test Drive BAA lead from Marketplace Web Automation"                                                                                              
                    };

                    vdp.SubmitLeadForm(emailLeadForm);
                    Assert.True(vdp.IsEmailLeadFeedbackMsgDisplayed(emailLeadForm), "Lead submission message is not displayed");

                    var expectedHomeTestDriveLeadTopicIdList = new List<string> { homeTestDriveLeadConsumerTopicId, homeTestDriveLeadDealerTopicId, homeTestDriveLead1STopicId };
                    var actualHomeTestDriveLeadTopicIdList = statisticDatabase.GetEmailTopicId(emailLeadForm.Email, EmailCreatedUtc, 3);
                    Assert.True(Enumerable.SequenceEqual(expectedHomeTestDriveLeadTopicIdList.OrderBy(x => x), actualHomeTestDriveLeadTopicIdList.OrderBy(x => x)), "Expected number of / matching topic ID emails not sent. Topic ID list of sent email(s): " + string.Join(", ", actualHomeTestDriveLeadTopicIdList.ToList()));  //Home Test Drive BAA Lead Email(3 emails - Consumer, Dealer, ADF) Verification by Topic ID
                }
            });
        }

        [Test, Property("TestCaseId", "8452")]
        public void GalleryLead()
        {
            #region Variables
            string galleryLeadConsumerTopicId = GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.galleryLeadConsumerTopicId");
            string galleryLeadDealerTopicId = GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.galleryLeadDealerTopicId");
            string galleryLead1STopicId = GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.galleryLead1STopicId");
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            dynamic vdpUrls = JsonConvert.DeserializeObject(GetTestData(testDataFile, $"recursiveData.{testcaseId}.Urls"));
            #endregion
            if (viewport.ToString() == "Large")
            {
                Assert.Multiple(() =>
                {
                    foreach (var vdpUrlItem in vdpUrls)
                    {
                        url = new Uri(baseURL + vdpUrlItem.Value.ToString());
                        Open();
                        EmailLeadForm emailLeadForm = new EmailLeadForm
                        {
                            LeadForm = EmailLeadForm.EmailLeads.GalleryLead,
                            Email = "Gallery_" + Extensions.GenerateRandomString(6) + "@trader.ca",
                            Message = "This is Gallery lead from Marketplace Web Automation"
                        };

                        vdp.SubmitLeadForm(emailLeadForm);
                        Assert.True(vdp.IsEmailLeadFeedbackMsgDisplayed(emailLeadForm), "Lead submission message is not displayed");

                        bool isPrivateAd = vdpUrlItem.Value.ToString().Contains("19-");
                        var expectedGalleryLeadTopicIdList = (isPrivateAd == true) ? new List<string> { galleryLeadConsumerTopicId, galleryLeadDealerTopicId } : new List<string> { galleryLeadConsumerTopicId, galleryLeadDealerTopicId, galleryLead1STopicId };
                        var actualGalleryLeadTopicIdList = statisticDatabase.GetEmailTopicId(emailLeadForm.Email, EmailCreatedUtc, (isPrivateAd == true) ? 2 : 3);
                        Assert.True(Enumerable.SequenceEqual(expectedGalleryLeadTopicIdList.OrderBy(x => x), actualGalleryLeadTopicIdList.OrderBy(x => x)), "Expected number of / matching topic ID emails not sent. Topic ID list of sent email(s): " + string.Join(", ", actualGalleryLeadTopicIdList.ToList()));  //Gallery Lead Email(2 emails - Consumer, Dealer) Verification by Topic ID
                    }
                });
            }
        }

        [Test, Property("TestCaseId", "8573")]
        [Ignore("CS BAdge is now covered as part of Test Verify Book and Appointment Desktop script")]
        public void CSBadgeBookAppointmentLead()
        {
            #region Variables
            string csBadgeBookApptLeadConsumerTopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.csBadgeBookApptLeadConsumerTopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.generalInquiryLeadConsumerTopicId");
            string csBadgeBookApptLeadDealerTopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.csBadgeBookApptLeadDealerTopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.generalInquiryLeadDealerTopicId");
            string csBadgeBookApptLead1STopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.csBadgeBookApptLead1STopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.generalInquiryLead_1S_TopicId");
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            dynamic vdpAds = JsonConvert.DeserializeObject(GetTestData(testDataFile, $"recursiveData.{testcaseId}.ads"));
            #endregion
            Assert.Multiple(() =>
            {
                foreach (var adItem in vdpAds)
                {
                    vdp.GoToVDP(baseURL, adItem.Value.ToString());
                    EmailLeadForm emailLeadForm = new EmailLeadForm
                    {
                        LeadForm = EmailLeadForm.EmailLeads.CSBadgeBookAnAppointment,
                        Name = "MP CSBadgeBookApptLeadTest",
                        Email = "CSBadgeBookAppt_" + Extensions.GenerateRandomString(6) + "@trader.ca",
                        PhoneNumber = GetTestData(testDataFile, "commonTestData.featureLeads.phoneNumber"),
                        PreferredDate = (viewport.ToString() == "XS")? DateTime.Now.AddDays(1).ToString("M/d/yyyy", CultureInfo.InvariantCulture):null,
                        TimeOfDay = (viewport.ToString() == "XS") ? "1":null,
                        Message = "This is CS Badge Book An Appointment lead from Marketplace Web Automation"
                    };

                    vdp.SubmitLeadForm(emailLeadForm);
                    Assert.True(vdp.IsEmailLeadFeedbackMsgDisplayed(emailLeadForm), "Lead submission message is not displayed");

                    var expectedHomeTestDriveLeadTopicIdList = new List<string> { csBadgeBookApptLeadConsumerTopicId, csBadgeBookApptLeadDealerTopicId, csBadgeBookApptLead1STopicId };
                    var actualHomeTestDriveLeadTopicIdList = statisticDatabase.GetEmailTopicId(emailLeadForm.Email, EmailCreatedUtc, 3);
                    Assert.True(Enumerable.SequenceEqual(expectedHomeTestDriveLeadTopicIdList.OrderBy(x => x), actualHomeTestDriveLeadTopicIdList.OrderBy(x => x)), "Expected number of / matching topic ID emails not sent. Topic ID list of sent email(s): " + string.Join(", ", actualHomeTestDriveLeadTopicIdList.ToList()));  //Home Test Drive BAA Lead Email(3 emails - Consumer, Dealer, ADF) Verification by Topic ID
                }
            });
        }

        [Test, Property("TestCaseId", "6061")]
        public void VerifyDeliveryLead()
        {
            #region Variables
            string deliveryLeadConsumerTopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.deliveryLeadConsumerTopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.deliveryLeadConsumerTopicId");
            string deliveryLeadDealerTopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.deliveryLeadDealerTopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.deliveryLeadDealerTopicId");
            string deliveryLead1STopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.deliveryLead1STopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.deliveryLead1STopicId");

            string feedbackMessage1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureLeads.feedbackMessageEn1") : GetTestData(testDataFile, "commonTestData.featureLeads.feedbackMessageFr1");
            string feedbackMessage2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureLeads.feedbackMessageEn2") : GetTestData(testDataFile, "commonTestData.featureLeads.feedbackMessageFr2");
            string name = GetTestData(testDataFile, "6061.Name");
            string message = (language.ToString() == "EN") ? GetTestData(testDataFile, "6061.MessageEN") : GetTestData(testDataFile, "6061.MessageFR");
            var vdpUrls = GetTestData(testDataFile, "6061.Urls");
            dynamic deserializedObject = JsonConvert.DeserializeObject(vdpUrls);

            #endregion
            
            Assert.Multiple(() =>
            {
                foreach (var item in deserializedObject)
                {
                    var vdpUrl = item.Value;
                    url = new Uri(baseURL + vdpUrl);
                    Open();

                    EmailLeadForm emailLeadForm = new EmailLeadForm
                    {
                        LeadForm = EmailLeadForm.EmailLeads.CSBadgeDelivery,
                        Name = name,
                        Email = "DeliveryLead_" + Extensions.GenerateRandomString(6) + "@trader.ca",
                        Message = message
                    };                   
                    vdp.SubmitLeadForm(emailLeadForm);
                    Assert.AreEqual(feedbackMessage1, vdp.GetEmailLeadFeedbackMessage1(), "\"Thank You!\" message does not display after submitting the lead.");
                    Assert.AreEqual(feedbackMessage2, vdp.GetEmailLeadFeedbackMessage2(), "\"Your inquiry has been sent.\" message does not display after submitting the lead.");
                    vdp.ClickOnOkBtnOnEmailLeadFeedbackDialog();
                    Assert.True(vdp.IsEmailLeadFormAvailable(), "Lead form is not displayed.");

                    var expectedDeliveryLeadTopicIdList = new List<string> { deliveryLeadConsumerTopicId, deliveryLeadDealerTopicId, deliveryLead1STopicId };
                    var actualDeliveryLeadTopicIdList = statisticDatabase.GetEmailTopicId(emailLeadForm.Email, EmailCreatedUtc, 3);
                    Assert.True(Enumerable.SequenceEqual(expectedDeliveryLeadTopicIdList.OrderBy(x => x), actualDeliveryLeadTopicIdList.OrderBy(x => x)), "Expected number of / matching topic ID emails not sent. Topic ID list of sent email(s): " + string.Join(", ", actualDeliveryLeadTopicIdList.ToList()));  //Delivery Lead Email(3 emails - Consumer, Dealer, ADF) Verification by Topic ID
                }
            });
        }

        [Test, Property("TestCaseId", "8576")]  //Bug: https://trader.atlassian.net/browse/SEO-4791
        public void DipGeneralInquiryLead()
        {
            #region Variables
            string dealerInventoryGILeadConsumerTopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.dealerInventoryGILeadConsumerTopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.dealerInventoryGILeadConsumerTopicId");
            string dealerInventoryGILeadDealerTopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.dealerInventoryGILeadDealerTopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.dealerInventoryGILeadDealerTopicId");
            string dealerInventoryGILead1STopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.dealerInventoryGILead1STopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.dealerInventoryGILead1STopicId");
            string dealerInventoryPageUrl = (language == Language.EN) ? GetTestData(testDataFile, "commonTestData.featureLeads.dealerInventoryPageEn.dealerInventoryPageUrl") : GetTestData(testDataFile, "commonTestData.featureLeads.dealerInventoryPageFr.dealerInventoryPageUrl");
            #endregion
            url = new Uri(baseURL + dealerInventoryPageUrl);
            Open();
            DealerInventoryLeadForm dealerInventoryLead = new DealerInventoryLeadForm
            {
                LeadType = DealerInventoryLeadForm.DealerInventoryLeads.GeneralInquiry,
                Name = "MP DIPGeneralInqLeadTest",
                Email = "DIPGeneralInq_" + Extensions.GenerateRandomString(6) + "@trader.ca",
                PhoneNumber = GetTestData(testDataFile, "commonTestData.featureLeads.phoneNumber"),
                Message = "This is dealer inventory page general inquiry lead from Marketplace Web Automation"
            };

            srp.SubmitLeadForm(dealerInventoryLead);
            Assert.True(srp.IsDipLeadFeedbackMsgDisplayed(), "Dealer Inventory Page Lead submission message is not displayed for lead: " + Enum.GetName(typeof(DealerInventoryLeadForm.DealerInventoryLeads), (int)dealerInventoryLead.LeadType));

            srp.ClickOKBtnDipLead();

            var expecteddealerInventoryGILeadTopicIdList = new List<string> { dealerInventoryGILeadConsumerTopicId, dealerInventoryGILeadDealerTopicId, dealerInventoryGILead1STopicId };
            var actualdealerInventoryGILeadTopicIdList = statisticDatabase.GetDealerInventoryLeadTopicId(dealerInventoryLead.Email, EmailCreatedUtc, 3);
            Assert.True(Enumerable.SequenceEqual(expecteddealerInventoryGILeadTopicIdList.OrderBy(x => x), actualdealerInventoryGILeadTopicIdList.OrderBy(x => x)), "Expected number of / matching topic ID emails not sent. Topic ID list of sent email(s): " + string.Join(", ", actualdealerInventoryGILeadTopicIdList.ToList()));  //Dealer Inventory Page - General Inquiry Lead Email(3 emails - Consumer, Dealer, ADF) Verification by Topic ID
        }

        //Disabled below test, until different lead types are re-introduced back to DIP lead form
        /*[Test, Property("TestCaseId", "8623")]
        public void DipServiceInquiryLead()
        {
            #region Variables
            string dealerInventorySILeadConsumerTopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.dealerInventorySILeadConsumerTopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.dealerInventorySILeadConsumerTopicId");
            string dealerInventorySILeadDealerTopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.dealerInventorySILeadDealerTopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.dealerInventorySILeadDealerTopicId");
            string dealerInventorySILead1STopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.dealerInventorySILead1STopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.dealerInventorySILead1STopicId");
            string dealerInventoryPageUrl = (language == Language.EN) ? GetTestData(testDataFile, "commonTestData.featureLeads.dealerInventoryPageEn.dealerInventoryPageUrl") : GetTestData(testDataFile, "commonTestData.featureLeads.dealerInventoryPageFr.dealerInventoryPageUrl");
            #endregion
            url = new Uri(baseURL + dealerInventoryPageUrl);
            Open();
            DealerInventoryLeadForm dealerInventoryLead = new DealerInventoryLeadForm
            {
                LeadType = DealerInventoryLeadForm.DealerInventoryLeads.ServiceInquiry,
                Name = "MP DIPServiceInqLeadTest",
                Email = "DIPServiceInq_" + Extensions.GenerateRandomString(6) + "@trader.ca",
                PhoneNumber = GetTestData(testDataFile, "commonTestData.featureLeads.phoneNumber"),
                Message = "This is dealer inventory page service inquiry lead from Marketplace Web Automation"
            };

            srp.SubmitLeadForm(dealerInventoryLead);
            Assert.True(srp.IsDipLeadFeedbackMsgDisplayed(), "Dealer Inventory Page Lead submission message is not displayed for lead: " + Enum.GetName(typeof(DealerInventoryLeadForm.DealerInventoryLeads), (int)dealerInventoryLead.LeadType));

            srp.ClickOKBtnDipLead();

            var expecteddealerInventorySILeadTopicIdList = new List<string> { dealerInventorySILeadConsumerTopicId, dealerInventorySILeadDealerTopicId, dealerInventorySILead1STopicId };
            var actualdealerInventorySILeadTopicIdList = statisticDatabase.GetDealerInventoryLeadTopicId(dealerInventoryLead.Email, EmailCreatedUtc, 3);
            Assert.True(Enumerable.SequenceEqual(expecteddealerInventorySILeadTopicIdList.OrderBy(x => x), actualdealerInventorySILeadTopicIdList.OrderBy(x => x)), "Expected number of / matching topic ID emails not sent. Topic ID list of sent email(s): " + string.Join(", ", actualdealerInventorySILeadTopicIdList.ToList()));  //Dealer Inventory Page - Service Inquiry Lead Email(3 emails - Consumer, Dealer, ADF) Verification by Topic ID
        }

        [Test, Property("TestCaseId", "8624")]
        public void DipNewVehicleInquiryLead()
        {
            #region Variables
            string dealerInventoryNVILeadConsumerTopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.dealerInventoryNVILeadConsumerTopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.dealerInventoryNVILeadConsumerTopicId");
            string dealerInventoryNVILeadDealerTopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.dealerInventoryNVILeadDealerTopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.dealerInventoryNVILeadDealerTopicId");
            string dealerInventoryNVILead1STopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.dealerInventoryNVILead1STopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.dealerInventoryNVILead1STopicId");
            string dealerInventoryPageUrl = (language == Language.EN) ? GetTestData(testDataFile, "commonTestData.featureLeads.dealerInventoryPageEn.dealerInventoryPageUrl") : GetTestData(testDataFile, "commonTestData.featureLeads.dealerInventoryPageFr.dealerInventoryPageUrl");
            #endregion
            url = new Uri(baseURL + dealerInventoryPageUrl);
            Open();
            DealerInventoryLeadForm dealerInventoryLead = new DealerInventoryLeadForm
            {
                LeadType = DealerInventoryLeadForm.DealerInventoryLeads.NewVehicleInquiry,
                Name = "MP DIPNewVehicleInqLeadTest",
                Email = "DIPNewVehicleInq_" + Extensions.GenerateRandomString(6) + "@trader.ca",
                PhoneNumber = GetTestData(testDataFile, "commonTestData.featureLeads.phoneNumber"),
                Message = "This is dealer inventory page new vehicle inquiry lead from Marketplace Web Automation"
            };

            srp.SubmitLeadForm(dealerInventoryLead);
            Assert.True(srp.IsDipLeadFeedbackMsgDisplayed(), "Dealer Inventory Page Lead submission message is not displayed for lead: " + Enum.GetName(typeof(DealerInventoryLeadForm.DealerInventoryLeads), (int)dealerInventoryLead.LeadType));

            srp.ClickOKBtnDipLead();

            var expecteddealerInventoryNVILeadTopicIdList = new List<string> { dealerInventoryNVILeadConsumerTopicId, dealerInventoryNVILeadDealerTopicId, dealerInventoryNVILead1STopicId };
            var actualdealerInventoryNVILeadTopicIdList = statisticDatabase.GetDealerInventoryLeadTopicId(dealerInventoryLead.Email, EmailCreatedUtc, 3);
            Assert.True(Enumerable.SequenceEqual(expecteddealerInventoryNVILeadTopicIdList.OrderBy(x => x), actualdealerInventoryNVILeadTopicIdList.OrderBy(x => x)), "Expected number of / matching topic ID emails not sent. Topic ID list of sent email(s): " + string.Join(", ", actualdealerInventoryNVILeadTopicIdList.ToList()));  //Dealer Inventory Page - New Vehicle Inquiry Lead Email(3 emails - Consumer, Dealer, ADF) Verification by Topic ID
        }

        [Test, Property("TestCaseId", "8625")]
        public void DipUsedVehicleInquiryLead()
        {
            #region Variables
            string dealerInventoryUVILeadConsumerTopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.dealerInventoryUVILeadConsumerTopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.dealerInventoryUVILeadConsumerTopicId");
            string dealerInventoryUVILeadDealerTopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.dealerInventoryUVILeadDealerTopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.dealerInventoryUVILeadDealerTopicId");
            string dealerInventoryUVILead1STopicId = (viewport.ToString() == "XS") ? GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdXS.dealerInventoryUVILead1STopicId") : GetTestData(testDataFile, "commonTestData.featureLeads.emailTopicId.topicIdLargeSmall.dealerInventoryUVILead1STopicId");
            string dealerInventoryPageUrl = (language == Language.EN) ? GetTestData(testDataFile, "commonTestData.featureLeads.dealerInventoryPageEn.dealerInventoryPageUrl") : GetTestData(testDataFile, "commonTestData.featureLeads.dealerInventoryPageFr.dealerInventoryPageUrl");
            #endregion
            url = new Uri(baseURL + dealerInventoryPageUrl);
            Open();
            DealerInventoryLeadForm dealerInventoryLead = new DealerInventoryLeadForm
            {
                LeadType = DealerInventoryLeadForm.DealerInventoryLeads.NewVehicleInquiry,
                Name = "MP DIPUsedVehicleInqLeadTest",
                Email = "DIPUsedVehicleInq_" + Extensions.GenerateRandomString(6) + "@trader.ca",
                PhoneNumber = GetTestData(testDataFile, "commonTestData.featureLeads.phoneNumber"),
                Message = "This is dealer inventory page used vehicle inquiry lead from Marketplace Web Automation"
            };

            srp.SubmitLeadForm(dealerInventoryLead);
            Assert.True(srp.IsDipLeadFeedbackMsgDisplayed(), "Dealer Inventory Page Lead submission message is not displayed for lead: " + Enum.GetName(typeof(DealerInventoryLeadForm.DealerInventoryLeads), (int)dealerInventoryLead.LeadType));

            srp.ClickOKBtnDipLead();

            //var expecteddealerInventoryUVILeadTopicIdList = new List<string> { dealerInventoryNVILeadConsumerTopicId, dealerInventoryNVILeadDealerTopicId, dealerInventoryNVILead1STopicId };
            //var actualdealerInventoryUVILeadTopicIdList = statisticDatabase.GetDealerInventoryLeadTopicId(dealerInventoryLead.Email, EmailCreatedUtc, 3);
            //Assert.True(Enumerable.SequenceEqual(expecteddealerInventoryUVILeadTopicIdList.OrderBy(x => x), actualdealerInventoryUVILeadTopicIdList.OrderBy(x => x)), "Expected number of / matching topic ID emails not sent. Topic ID list of sent email(s): " + string.Join(", ", actualdealerInventoryUVILeadTopicIdList.ToList()));  //Dealer Inventory Page - Used Vehicle Inquiry Lead Email(3 emails - Consumer, Dealer, ADF) Verification by Topic ID
        }

        */

        [Test, Property("TestCaseId", "61156")]
        public void VerifyNewLeadFormOnVDPOnDesktop()
        {
            #region Variables           
            string feedbackMessage1 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureLeads.feedbackMessageEn1") : GetTestData(testDataFile, "commonTestData.featureLeads.feedbackMessageFr1");
            string feedbackMessage2 = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureLeads.feedbackMessageEn2") : GetTestData(testDataFile, "commonTestData.featureLeads.feedbackMessageFr2");
            string termsAndCMessage = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureLeads.t&cEn") : GetTestData(testDataFile, "commonTestData.featureLeads.t&cFr");
            string phoneAssistedText = (language.ToString() == "EN") ? GetTestData(testDataFile, "commonTestData.featureLeads.phoneText") : GetTestData(testDataFile, "commonTestData.featureLeads.phoneTextFr");
            string name = GetTestData(testDataFile, "61156.Name");
            string message = GetTestData(testDataFile, "61156.Message");
            var vdpUrls = GetTestData(testDataFile, "61156.Urls");
            dynamic deserializedObject = JsonConvert.DeserializeObject(vdpUrls);

            #endregion
            if (!viewport.ToString().Equals("XS"))
            {                
                Assert.Multiple(() =>
                {
                    foreach (var item in deserializedObject)
                    {
                        var vdpUrl = item.Value;
                        url = new Uri(baseURL + vdpUrl);
                        Open();

                        EmailLeadForm emailLeadForm = new EmailLeadForm
                        {
                            LeadForm = EmailLeadForm.EmailLeads.GeneralInquiry,
                            Name = "MP GeneralInquiryLeadTest",
                            Email = "GeneralInquiryLead_" + Extensions.GenerateRandomString(6) + "@trader.ca",                           
                            Message = "This is General Inquiry from Marketplace Web Automation"
                        };


                        EmailLeadForm.DealerType DealerType = EmailLeadForm.DealerType.Traditional;
                        if (item.Name.Contains("Virtual"))
                        {
                            DealerType = EmailLeadForm.DealerType.Virtual;
                        }
                        else if (item.Name.Contains("Hybrid"))
                        {
                            DealerType = EmailLeadForm.DealerType.Hybrid;
                        }

                        Assert.IsTrue(vdp.CheckLeadForm(DealerType), "The New Lead form header and footer seems to be invalid");
                        Assert.IsTrue(vdp.VerifyDealerDetailsOnTheLeadForm(), "Dealer Details  section not validated");
                        Assert.AreEqual(termsAndCMessage, vdp.GetEmailLeadTermsAndConditionMessage(), "Terms and Condition messages didnt match");
                        Assert.AreEqual(phoneAssistedText, vdp.VerifyPhoneNumberAssitedTextOnTheLeadForm(), "Phone Number assited text is not valid");
                        vdp.SubmitLeadForm(emailLeadForm);
                        Assert.AreEqual(feedbackMessage1, vdp.GetEmailLeadFeedbackMessage1(), "Feedback message does not display after submitting the lead.");
                        Assert.AreEqual(feedbackMessage2, vdp.GetEmailLeadFeedbackMessage2(), "Feedback message does not display after submitting the lead.");
                        vdp.ClickOnOkBtnOnEmailLeadFeedbackDialog();
                        Assert.True(vdp.IsEmailLeadFormAvailable(), "Lead form is not displayed.");

                    }
                });
            }
        }
    }
}
