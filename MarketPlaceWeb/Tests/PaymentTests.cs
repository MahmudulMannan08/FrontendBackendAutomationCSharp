using MarketPlaceWeb.Base;
using MarketPlaceWeb.Driver;
using MarketPlaceWeb.Pages;
using MarketPlaceWeb.Pages.VDP;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;

namespace MarketPlaceWeb.Test
{
    [TestFixture]
    public class PaymentTests : Page
    {
        SRPMain srp;
        VDPMain vdp;
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
            vdp = new VDPMain(driver, viewport, language);
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
        #region Payment1
        [Test, Property("TestCaseId", "7252")]
        public void VerifyPayment1CalculatorOldCar()
        {
            bool isDefaultValue = false, isFinanaceFeeEnable = false, isExpand = true;
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var testURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.OldCarUrl");
            url = new Uri(baseURL + testURL);
            Open();
            srp.ClickOnFirstOrganicListing();
            var paymentCalculator = new PaymentCalculator
            {
                PaymentConfiguration = new PaymentConfiguration
                {
                    Downpayment = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.downpayment"),
                    TradeIn = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.tradeIn"),
                    SalesTax = Convert.ToBoolean(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.salesTax"))
                }

            };
            vdp.FillPaymentConfiguration(paymentCalculator);
            vdp.SetPricingBreakdown(isExpand);
            var pricingBreakdown = vdp.GetPricingBreakdown(isFinanaceFeeEnable, isDefaultValue);
            var paymentBottomPanel = vdp.GetPaymentBottomPanel();
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.dealerPriceLabel"), pricingBreakdown.DealerPriceLabel, "Dealer price label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.dealerPrice"), pricingBreakdown.DealerPrice, "Dealer price value doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.interestChargeLabel"), pricingBreakdown.InterestChargeLabel, "Interest charge label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.salesTaxLabel"), pricingBreakdown.SalesTaxesLabel, "Sales tax label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.downPaymentLabel"), pricingBreakdown.DownPaymentLabel, "Down Payment label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.downPayment"), pricingBreakdown.DownPayment, "Down Payment value doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.tradeInLabel"), pricingBreakdown.TradeInLabel, "Trade-in label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.tradeIn"), pricingBreakdown.TradeIn, "Trade-in value doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.totalObligationLabel"), pricingBreakdown.TotalObligationLabel, "Total obligation label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.yourEstimateLabel"), pricingBreakdown.YourEstimateLabel, "Your estimate label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentBottomPanel.disclaimer"), paymentBottomPanel.Disclaimer, "Disclaimer doesn't match");
        }
        [Test, Property("TestCaseId", "11064")]
        public void VerifyPayment1LuxuryTax()
        {
            bool isExpand = true;
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var testURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.NewCarUrl");
            url = new Uri(baseURL + testURL);
            Open();

            var paymentCalculator = new PaymentCalculator
            {
                PaymentConfiguration = new PaymentConfiguration
                {
                    SalesTax = Convert.ToBoolean(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.salesTax"))
                }

            };
            vdp.FillPaymentConfiguration(paymentCalculator);
            vdp.SetPricingBreakdown(isExpand);
            var pricingBreakdown = vdp.GetPricingBreakdown();
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.dealerPriceLabel"), pricingBreakdown.DealerPriceLabel, "Dealer price label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.salesTaxLabel"), pricingBreakdown.SalesTaxesLabel, "Sales tax label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.FederalLuxuryTaxLabel"), pricingBreakdown.FederalLuxuryTaxLabel, "Federal luxury tax label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.FederalLuxuryTax"), pricingBreakdown.FederalLuxuryTax, "Federal luxury tax doesn't match");
        }
        #endregion

        #region Payment2
        [Test, Property("TestCaseId", "6846")]
        public void VerifyFinancePaymentCalculatorOldCar()
        {
            bool isDefaultValue = false, isFinanaceFeeEnable = true, isExpand = true;
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var testURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.OldCarUrl");
            url = new Uri(baseURL + testURL);
            Open();
            srp.ClickOnFirstOrganicListing();
            var paymentCalculator = new PaymentCalculator
            {
                PaymentType = PaymentCalculator.Type.Finance,
                PaymentConfiguration = new PaymentConfiguration
                {
                    PaymentFrequency = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.paymentFrequency"),
                    FinanceTerm = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.financeTerm"),
                    Downpayment = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.downpayment"),
                    TradeIn = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.tradeIn"),
                    SalesTax = Convert.ToBoolean(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.salesTax"))
                }

            };
            vdp.FillPaymentConfiguration(paymentCalculator);
            vdp.SetPricingBreakdown(isExpand);
            var pricingBreakdown = vdp.GetPricingBreakdown(isFinanaceFeeEnable, isDefaultValue);
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.dealerPriceLabel"), pricingBreakdown.DealerPriceLabel, "Dealer price label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.dealerPrice"), pricingBreakdown.DealerPrice, "Dealer price value doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.financeFeeLabel"), pricingBreakdown.FinanceFeeLabel, "Finance fee label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.financeFee"), pricingBreakdown.FinanceFee, "Finance fee value doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.interestChargeLabel"), pricingBreakdown.InterestChargeLabel, "Interest charge label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.salesTaxLabel"), pricingBreakdown.SalesTaxesLabel, "Sales tax label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.downPaymentLabel"), pricingBreakdown.DownPaymentLabel, "Down Payment label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.downPayment"), pricingBreakdown.DownPayment, "Down Payment value doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.tradeInLabel"), pricingBreakdown.TradeInLabel, "Trade-in label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.tradeIn"), pricingBreakdown.TradeIn, "Trade-in value doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.totalObligationLabel"), pricingBreakdown.TotalObligationLabel, "Total obligation label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.yourEstimateLabel"), pricingBreakdown.YourEstimateLabel, "Your estimate label doesn't match");
        }
        [Test, Property("TestCaseId", "6857")]
        public void VerifyDefaultFinancePaymentCalculatorOldCar()
        {
            bool isFinanaceFeeEnable = true, isExpand = true;
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var testURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.OldCarUrl");
            url = new Uri(baseURL + testURL);
            Open();
            vdp.SetPricingBreakdown(isExpand);
            var paymentConfiguration = vdp.GetPaymentConfiguation();
            var pricingBreakdown = vdp.GetPricingBreakdown(isFinanaceFeeEnable);
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.paymentFrequency"), paymentConfiguration.PaymentFrequency, "Payment frequency value doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.financeTerm"), paymentConfiguration.FinanceTerm, "Finance term value doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.downpayment"), paymentConfiguration.Downpayment, "Downpayment value doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.tradeIn"), paymentConfiguration.TradeIn, "Trade-in value doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.salesTax").ToLower(), paymentConfiguration.SalesTax.ToString().ToLower(), "Sales tax value doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.dealerPriceLabel"), pricingBreakdown.DealerPriceLabel, "Dealer price label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.dealerPrice"), pricingBreakdown.DealerPrice, "Dealer price value doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.financeFeeLabel"), pricingBreakdown.FinanceFeeLabel, "Finance fee label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.financeFee"), pricingBreakdown.FinanceFee, "Finance fee value doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.interestChargeLabel"), pricingBreakdown.InterestChargeLabel, "Interest charge label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.interestCharge"), pricingBreakdown.InterestCharge, "Interest charge doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.totalObligationLabel"), pricingBreakdown.TotalObligationLabel, "Total obligation label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.totalObligation"), pricingBreakdown.TotalObligation, "Total obligation doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.yourEstimateLabel"), pricingBreakdown.YourEstimateLabel, "Your estimate label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.yourEstimate"), pricingBreakdown.YourEstimate, "Your estimate doesn't match");

        }
        [Test, Property("TestCaseId", "6869")]
        public void VerifyFinanceNotNeededOldCar()
        {
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var testURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.OldCarUrl");
            url = new Uri(baseURL + testURL);
            Open();
            var paymentCalculator = new PaymentCalculator
            {
                PaymentType = PaymentCalculator.Type.Finance,
                PaymentConfiguration = new PaymentConfiguration
                {
                    PaymentFrequency = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.paymentFrequency"),
                    FinanceTerm = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.financeTerm"),
                    Downpayment = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.downpayment"),
                    TradeIn = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.tradeIn")
                }
            };
            vdp.FillPaymentConfiguration(paymentCalculator);
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.financingNotNeeded"), vdp.GetFinanceNotNeededMsg(), "Financing not needed message doesn't match when Downpayment and Trade-in sum is greater than Total obligation");


            var addTradeInOnly = new PaymentCalculator
            {
                PaymentType = PaymentCalculator.Type.Finance,
                PaymentConfiguration = new PaymentConfiguration
                {
                    Downpayment = "0",
                    TradeIn = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.tradeInHigherThanTotal")
                }
            };
            vdp.FillPaymentConfiguration(addTradeInOnly);
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.financingNotNeeded"), vdp.GetFinanceNotNeededMsg(), "Financing not needed message doesn't match when Trade-in is greater than Total obligation");


            var addDownpaymentOnly = new PaymentCalculator
            {
                PaymentType = PaymentCalculator.Type.Finance,
                PaymentConfiguration = new PaymentConfiguration
                {
                    Downpayment = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.downpaymentHigherThanTotal"),
                    TradeIn = "0"
                }
            };
            vdp.FillPaymentConfiguration(addDownpaymentOnly);
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.financingNotNeeded"), vdp.GetFinanceNotNeededMsg(), "Financing not needed message doesn't match when Downpayment is greater than Total obligation");

        }

        [Test, Property("TestCaseId", "6862")]
        public void VerifyDefaultFinancePaymentCalculatorNewCar()
        {
            bool isExpand = true;
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var testURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.NewCarUrl");
            url = new Uri(baseURL + testURL);
            Open();
            vdp.SetPricingBreakdown(isExpand);
            var paymentConfiguration = vdp.GetPaymentConfiguation();
            var pricingBreakdown = vdp.GetPricingBreakdown();
            var paymentBottomPanel = vdp.GetPaymentBottomPanel();
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.paymentFrequency"), paymentConfiguration.PaymentFrequency, "Payment frequency value doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.financeTerm"), paymentConfiguration.FinanceTerm, "Finance term value doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.downpayment"), paymentConfiguration.Downpayment, "Downpayment value doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.tradeIn"), paymentConfiguration.TradeIn, "Trade-in value doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.salesTax"), paymentConfiguration.SalesTax.ToString().ToLower(), "Sales tax value doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.msrpLabel"), pricingBreakdown.MSRPLabel, "Dealer price label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.msrp"), pricingBreakdown.MSRP, "Dealer price value doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.interestChargeLabel"), pricingBreakdown.InterestChargeLabel, "Interest charge label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.interestCharge"), pricingBreakdown.InterestCharge, "Interest charge doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.totalObligationLabel"), pricingBreakdown.TotalObligationLabel, "Total obligation label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.totalObligation"), pricingBreakdown.TotalObligation, "Total obligation doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.yourEstimateLabel"), pricingBreakdown.YourEstimateLabel, "Your estimate label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.yourEstimate"), pricingBreakdown.YourEstimate, "Your estimate doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentBottomPanel.inquireInfo"), paymentBottomPanel.InquireInfo, "Inquire info doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentBottomPanel.inquireNowButton"), paymentBottomPanel.InquireNow, "Inquire now button doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentBottomPanel.disclaimer"), paymentBottomPanel.Disclaimer, "Disclaimer doesn't match");

        }
        [Test, Property("TestCaseId", "6861")]
        public void VerifyFinanceTabNewCar()
        {
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var testURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.OldCarUrl");
            url = new Uri(baseURL + testURL);
            Open();
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.tabName"), vdp.GetPaymentTabName(), "Payment calculator tab name doesn't match");
        }
        [Test, Property("TestCaseId", "6892")]
        public void VerifyZeroFinanceFeeNewCar()
        {
            bool isFinanaceFeeEnable = true, isExpand = true;
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var testURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.NewCarUrl");
            url = new Uri(baseURL + testURL);
            Open();
            vdp.SetPricingBreakdown(isExpand);
            var pricingBreakdown = vdp.GetPricingBreakdown(isFinanaceFeeEnable);
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.msrpLabel"), pricingBreakdown.MSRPLabel, "MSRP label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.msrp"), pricingBreakdown.MSRP, "MSRP value doesn't match");
            Assert.Null(pricingBreakdown.FinanceFeeLabel, "Finance fee label is not hidden");
            Assert.Null(pricingBreakdown.FinanceFee, "Finance fee value is not hidden");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.interestChargeLabel"), pricingBreakdown.InterestChargeLabel, "Interest charge label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.interestCharge"), pricingBreakdown.InterestCharge, "Interest charge doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.totalObligationLabel"), pricingBreakdown.TotalObligationLabel, "Total obligation label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.totalObligation"), pricingBreakdown.TotalObligation, "Total obligation doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.yourEstimateLabel"), pricingBreakdown.YourEstimateLabel, "Your estimate label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.yourEstimate"), pricingBreakdown.YourEstimate, "Your estimate doesn't match");
        }
        [Test, Property("TestCaseId", "6849")]
        public void ClickOnInquireNowNewCar()
        {
            bool isExpand = true, isModal = true;
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var testURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.NewCarUrl");
            url = new Uri(baseURL + testURL);
            Open();
            var paymentCalculator = new PaymentCalculator
            {
                PaymentType = PaymentCalculator.Type.Finance,
                PaymentConfiguration = new PaymentConfiguration
                {
                    PaymentFrequency = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.paymentFrequency"),
                    FinanceTerm = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.financeTerm"),
                    Downpayment = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.downpayment"),
                    TradeIn = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.tradeIn")
                }
            };
            vdp.FillPaymentConfiguration(paymentCalculator);
            vdp.ClickOnPayment2Inquire();
            vdp.SetPricingBreakdown(isExpand, isModal);
            var paymentLeadForm = vdp.GetPaymentModalLeadFormContent();
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentLeadForm.pricingBreakdown.msrpLabel"), paymentLeadForm.PricingBreakdown.MSRPLabel, "MSRP label doesn't match in modal");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentLeadForm.pricingBreakdown.msrp"), paymentLeadForm.PricingBreakdown.MSRP, "MSRP value doesn't match in modal");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentLeadForm.pricingBreakdown.interestChargeLabel"), paymentLeadForm.PricingBreakdown.InterestChargeLabel, "Interest charge label doesn't match in modal");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentLeadForm.pricingBreakdown.interestCharge"), paymentLeadForm.PricingBreakdown.InterestCharge, "Interest charge doesn't match in modal");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentLeadForm.pricingBreakdown.totalObligationLabel"), paymentLeadForm.PricingBreakdown.TotalObligationLabel, "Total obligation label doesn't match in modal");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentLeadForm.pricingBreakdown.totalObligation"), paymentLeadForm.PricingBreakdown.TotalObligation, "Total obligation doesn't match in modal");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentLeadForm.pricingBreakdown.yourEstimate"), paymentLeadForm.PricingBreakdown.YourEstimate, "Your estimate doesn't match in modal");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentLeadForm.pricingBreakdown.yourEstimateFequency"), paymentLeadForm.PricingBreakdown.YourEstimateFrequency, "Your estimate frequency doesn't match in modal");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentLeadForm.pricingBreakdown.financeTerm"), paymentLeadForm.PricingBreakdown.FinanceTerm, "Finance term doesn't match in modal");
            Assert.IsTrue(paymentLeadForm.SummaryDisclaimer.Contains(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentLeadForm.summaryDisclaimer")), "Summary disclaimer doesn't match in modal");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentLeadForm.nameLabel"), paymentLeadForm.NameLabel, "Name label doesn't match in modal");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentLeadForm.emailLabel"), paymentLeadForm.EmailLabel, "Email label doesn't match in modal");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentLeadForm.phoneNumberLabel"), paymentLeadForm.PhoneNumberLabel, "Phone number label doesn't match in modal");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentLeadForm.getPriceAlert"), paymentLeadForm.GetPriceAlert, "Get price alert doesn't match in modal");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentLeadForm.submitButton"), paymentLeadForm.SubmitButton, "Submit button doesn't match in modal");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentLeadForm.formDisclaimer"), paymentLeadForm.FormDisclaimer, "Form disclaimer doesn't match in modal");

        }
        [Test, Property("TestCaseId", "6850")]
        public void SubmitInquireNowNewCar()
        {
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var testURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.NewCarUrl");
            url = new Uri(baseURL + testURL);
            Open();
            var emailLeadForm = new EmailLeadForm
            {
                Name = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentLeadForm.name"),
                Email = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentLeadForm.email"),
                PhoneNumber = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentLeadForm.phoneNumber"),
                Message = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentLeadForm.message"),
                GetPriceAlert = Convert.ToBoolean(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentLeadForm.getPriceAlertChkbox")
)
            };
            var paymentCalculator = new PaymentCalculator
            {
                PaymentType = PaymentCalculator.Type.Finance,
                PaymentConfiguration = new PaymentConfiguration
                {
                    PaymentFrequency = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.paymentFrequency"),
                    FinanceTerm = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.financeTerm"),
                    Downpayment = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.downpayment"),
                    TradeIn = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.tradeIn")
                }
            };
            vdp.FillPaymentConfiguration(paymentCalculator);
            vdp.ClickOnPayment2Inquire();
            vdp.SubmitPayment2Inquiry(emailLeadForm);
            var thankYouContent = vdp.GetPaymentModalThankYouContent();
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.thankYouModal.heading"), thankYouContent.Heading, "Heading doesn't match in payment thank you modal");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.thankYouModal.message"), thankYouContent.Message, "Message doesn't match in payment thank you modal");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.thankYouModal.finishButton"), thankYouContent.FinishButton, "Button text doesn't match in payment thank you modal");
            vdp.ClickOnPaymentModalThankYouFinishBtn();
        }

        [Test, Property("TestCaseId", "6893")]
        public void VerifyZeroFinanceFeeCPO()
        {
            bool isFinanaceFeeEnable = true, isExpand = true;
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var testURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.CPOUrl");
            url = new Uri(baseURL + testURL);
            Open();
            vdp.SetPricingBreakdown(isExpand);
            var pricingBreakdown = vdp.GetPricingBreakdown(isFinanaceFeeEnable);
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.dealerPriceLabel"), pricingBreakdown.DealerPriceLabel, "Dealer price label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.dealerPrice"), pricingBreakdown.DealerPrice, "Dealer price value doesn't match");
            Assert.Null(pricingBreakdown.FinanceFeeLabel, "Finance fee label is not hidden");
            Assert.Null(pricingBreakdown.FinanceFee, "Finance fee value is not hidden");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.interestChargeLabel"), pricingBreakdown.InterestChargeLabel, "Interest charge label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.interestCharge"), pricingBreakdown.InterestCharge, "Interest charge doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.totalObligationLabel"), pricingBreakdown.TotalObligationLabel, "Total obligation label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.totalObligation"), pricingBreakdown.TotalObligation, "Total obligation doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.yourEstimateLabel"), pricingBreakdown.YourEstimateLabel, "Your estimate label doesn't match");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.pricingBreakdown.yourEstimate"), pricingBreakdown.YourEstimate, "Your estimate doesn't match");
      }
        [Test, Property("TestCaseId", "11065")]
        public void VerifyLuxuryTax()
        {
            bool isExpand = true, isModal = true;
            var testcaseId = TestContext.CurrentContext.Test.Properties.Get("TestCaseId");
            var testURL = GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.NewCarUrl");
            url = new Uri(baseURL + testURL);
            Open();
            var paymentCalculator = new PaymentCalculator
            {
                PaymentType = PaymentCalculator.Type.Finance,
                PaymentConfiguration = new PaymentConfiguration
                {
                    SalesTax = Convert.ToBoolean(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentConfig.salesTax"))
                }
            };
            vdp.FillPaymentConfiguration(paymentCalculator);
            vdp.ClickOnPayment2Inquire();
            vdp.SetPricingBreakdown(isExpand, isModal);
            var paymentLeadForm = vdp.GetPaymentModalLeadFormContent();
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentLeadForm.pricingBreakdown.federalLuxuryTaxLabel"), paymentLeadForm.PricingBreakdown.FederalLuxuryTaxLabel, "Federal Tax label doesn't match in modal");
            Assert.AreEqual(GetTestData(testDataFile, $"{testcaseId}.{language.ToString()}.paymentLeadForm.pricingBreakdown.federalLuxuryTax"), paymentLeadForm.PricingBreakdown.FederalLuxuryTax, "Federal Tax value doesn't match in modal");
        }
        #endregion

    }
}