using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MarketPlaceWeb.Pages.VDP
{
    public class VDPXS : VDPAbstract
    {
        public VDPXS(IWebDriver driver, Viewport viewport, Language language) : base(driver, viewport, language) { }

        #region Leads
        public void ClickOnEmailBtn()
        {
            ClickElement(FindElement(By.CssSelector(VDPLocators.XSLocators.EmailButton.GetAttribute<DescriptionAttribute>().Description)));
            WaitForModalToOpen(By.CssSelector(VDPLocators.CommonLocators.VdpLeadModal.GetAttribute<DescriptionAttribute>().Description));
        }

        public override void OpenLeadAbs(EmailLeadForm.EmailLeads emailLeads)
        {
            string tabName = string.Empty;
            switch (emailLeads)
            {
                case EmailLeadForm.EmailLeads.BookAnAppointment:
                    ClickOnEmailBtn();
                    tabName = (language.ToString() == "EN") ? "BOOK AN APPOINTMENT" : "PRENDRE RENDEZ-VOUS";
                    IWebElement bookAnAppointmentTabElement = FindElements(By.CssSelector(VDPLocators.CommonLocators.LeadContainerTab.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault(t => t.Text == tabName);
                    ClickElement(bookAnAppointmentTabElement);
                    WaitForLeadTabToOpen(EmailLeadForm.EmailLeads.BookAnAppointment);
                    break;
                case EmailLeadForm.EmailLeads.GeneralInquiry:
                    ClickOnEmailBtn();
                    tabName = (language.ToString() == "EN") ? "GENERAL INQUIRY" : "REQUÊTE GÉNÉRALE";
                    IWebElement generalInqTabElement = FindElements(By.CssSelector(VDPLocators.CommonLocators.LeadContainerTab.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault(t => t.Text == tabName);
                    ClickElement(generalInqTabElement);
                    WaitForLeadTabToOpen(EmailLeadForm.EmailLeads.GeneralInquiry);
                    break;
                case EmailLeadForm.EmailLeads.MakeAnOffer:
                    ClickOnEmailBtn();
                    tabName = (language.ToString() == "EN") ? "MAKE AN OFFER" : "FAIRE UNE OFFRE";
                    IWebElement makeAnOfferTabElement = FindElements(By.CssSelector(VDPLocators.CommonLocators.LeadContainerTab.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault(t => t.Text == tabName);
                    ClickElement(makeAnOfferTabElement);
                    WaitForLeadTabToOpen(EmailLeadForm.EmailLeads.MakeAnOffer);
                    break;
                case EmailLeadForm.EmailLeads.ViewMapLead:
                    ClickOnEmailBtn();
                    ClickElement(FindElement(By.CssSelector(VDPLocators.XSLocators.ViewMapButton.GetAttribute<DescriptionAttribute>().Description)));
                    WaitForModalToOpen(By.CssSelector(VDPLocators.CommonLocators.ViewMapModal.GetAttribute<DescriptionAttribute>().Description));
                    break;
                case EmailLeadForm.EmailLeads.CSBadgeBookAnAppointment:
                    IWebElement bookAnAppointmentCSBadgeElement = FindElement(By.CssSelector(VDPLocators.CommonLocators.BookAnAppointmentCSBadge.GetAttribute<DescriptionAttribute>().Description));
                    ScrollTo(bookAnAppointmentCSBadgeElement);
                    ClickElement(bookAnAppointmentCSBadgeElement);
                    WaitForModalToOpen(By.CssSelector(VDPLocators.CommonLocators.VdpLeadModal.GetAttribute<DescriptionAttribute>().Description));
                    WaitForLeadTabToOpen(EmailLeadForm.EmailLeads.BookAnAppointment);
                    break;
                case EmailLeadForm.EmailLeads.CSBadgeHomeTestDrive:
                    IWebElement homeTestDriveCSBadgeElement = FindElement(By.CssSelector(VDPLocators.CommonLocators.HomeTestDriveCSBadge.GetAttribute<DescriptionAttribute>().Description));
                    ScrollTo(homeTestDriveCSBadgeElement);
                    ClickElement(homeTestDriveCSBadgeElement);
                    WaitForModalToOpen(By.CssSelector(VDPLocators.CommonLocators.VdpLeadModal.GetAttribute<DescriptionAttribute>().Description));
                    WaitForLeadTabToOpen(EmailLeadForm.EmailLeads.BookAnAppointment);
                    break;
                case EmailLeadForm.EmailLeads.CSBadgeDelivery:
                    IWebElement deliveryCSBadgeElement = FindElement(By.CssSelector(VDPLocators.CommonLocators.DeliveryCSBadge.GetAttribute<DescriptionAttribute>().Description));
                    ScrollTo(deliveryCSBadgeElement);
                    ClickElement(deliveryCSBadgeElement);
                    WaitForModalToOpen(By.CssSelector(VDPLocators.CommonLocators.VdpLeadModal.GetAttribute<DescriptionAttribute>().Description));
                    WaitForLeadTabToOpen(EmailLeadForm.EmailLeads.GeneralInquiry);
                    break;
            }
        }
        #endregion

        #region ReserveIt
        public override void CheckDepositTerms(bool toBeChecked = true)
        {
            IWebElement element = FindElement(By.CssSelector(VDPLocators.XSLocators.DepositTerms.GetAttribute<DescriptionAttribute>().Description + " input"));
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(VDPLocators.XSLocators.DepositTerms.GetAttribute<DescriptionAttribute>().Description + " span")));
            }
        }
        #endregion

        #region TryBeforeYouBuy
        public override void ClickTenDayMoneyBackGuaranteeText()
        {
            IWebElement element1 = FindElement(By.CssSelector(VDPLocators.CommonLocators.TenDayMoneyBackGuarantee
                  .GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element1);
            ClickElement(element1);
        }
        #endregion

        public override void SetPricingBreakdown(bool isExpand, bool isModal)
        {
            if (!isModal)
            {
                ScrollTo(FindElement(By.CssSelector(VDPLocators.CommonLocators.PricingBreakdownPnl.GetAttribute<DescriptionAttribute>().Description)));
            }
            TogglePricingBreakdown(isExpand, isModal);
        }

        public override PaymentLeadForm GetPaymentModalLeadFormContent()
        {
            var paymentLeadForm = new PaymentLeadForm
            {
                PricingBreakdown = new PricingBreakdown
                {
                    DealerPriceLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalDealerPriceLbl.GetAttribute<DescriptionAttribute>().Description))?.Text,
                    DealerPrice = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalDealerPriceValueLbl.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim(),
                    MSRPLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalMSRPLbl.GetAttribute<DescriptionAttribute>().Description))?.Text,
                    MSRP = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalMSRPValueLbl.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim(),
                    InterestChargeLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalInterestChargeLbl.GetAttribute<DescriptionAttribute>().Description))?.Text,
                    InterestCharge = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalInterestChargeValueLbl.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim(),
                    DownPaymentLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalDownPaymentLbl.GetAttribute<DescriptionAttribute>().Description))?.Text,
                    DownPayment = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalDownPaymentValueLbl.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim(),
                    TradeInLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalTradeInLbl.GetAttribute<DescriptionAttribute>().Description))?.Text,
                    TradeIn = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalTradeInValueLbl.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim(),
                    TotalObligationLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalTotalObligationLbl.GetAttribute<DescriptionAttribute>().Description))?.Text,
                    TotalObligation = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalTotalObligationValueLbl.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim(),
                    YourEstimate = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalYourEstimate.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim(),
                    YourEstimateFrequency = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalYourEstimateFequencyLbl.GetAttribute<DescriptionAttribute>().Description))?.Text,
                    FinanceTerm = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalFinanceTermLbl.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim(),
                    FederalLuxuryTaxLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalLuxuryTaxLbl.GetAttribute<DescriptionAttribute>().Description))?.Text,
                    FederalLuxuryTax = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalLuxuryTaxValueLbl.GetAttribute<DescriptionAttribute>().Description))?.Text.Trim()
                }
            };
            paymentLeadForm.NameLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalNameLbl.GetAttribute<DescriptionAttribute>().Description))?.Text;
            ScrollTo(FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalEmailLbl.GetAttribute<DescriptionAttribute>().Description)));
            paymentLeadForm.SummaryDisclaimer = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalSummaryDisclaimer.GetAttribute<DescriptionAttribute>().Description)).Text;
            paymentLeadForm.EmailLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalEmailLbl.GetAttribute<DescriptionAttribute>().Description))?.Text;
            paymentLeadForm.PhoneNumberLabel = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalPhoneNumberLbl.GetAttribute<DescriptionAttribute>().Description))?.Text;
            paymentLeadForm.GetPriceAlert = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalGetPriceAlert.GetAttribute<DescriptionAttribute>().Description))?.Text;
            paymentLeadForm.SubmitButton = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalSubmitBtn.GetAttribute<DescriptionAttribute>().Description))?.Text;
            paymentLeadForm.FormDisclaimer = FindElement(By.CssSelector(VDPLocators.CommonLocators.ModalFormDisclaimer.GetAttribute<DescriptionAttribute>().Description))?.Text;
            return paymentLeadForm;
        }

        public override void ClickVideoPlayIconInGallery()
        {
            while (!IsElementVisible(By.CssSelector(VDPLocators.XSLocators.TapToViewIcon.GetAttribute<DescriptionAttribute>().Description)))
            {
                ClickElement(FindElement(By.XPath(VDPLocators.XSLocators.Nextarrow.GetAttribute<DescriptionAttribute>().Description)));

            }

            ClickElement(FindElement(By.CssSelector(VDPLocators.XSLocators.TapToViewIcon.GetAttribute<DescriptionAttribute>().Description)));
            IWebElement iframeYoutube = FindElement(By.CssSelector(VDPLocators.CommonLocators.YoutubeIframe.GetAttribute<DescriptionAttribute>().Description));
            driver.SwitchTo().Frame(iframeYoutube);
            ClickElement(FindElement(By.CssSelector(VDPLocators.CommonLocators.MoviePlayer.GetAttribute<DescriptionAttribute>().Description)));
            driver.SwitchTo().DefaultContent();
        }

        public override string VerifyYoutubeVideoIsPlaying()
        {
            IWebElement iframeYoutube = FindElement(By.CssSelector(VDPLocators.CommonLocators.YoutubeIframe.GetAttribute<DescriptionAttribute>().Description));
            driver.SwitchTo().Frame(iframeYoutube);
            string youtubeClassAttrribute = FindElement(By.CssSelector(VDPLocators.CommonLocators.MoviePlayer.GetAttribute<DescriptionAttribute>().Description)).GetAttribute("class");
            driver.SwitchTo().DefaultContent();
            return youtubeClassAttrribute;
        }

        #region TryBeforeYouBuy
        public override bool IsTbybButtonEnabled(bool isEnabled = true)
        {
            IWebElement element = FindElement(By.CssSelector(VDPLocators.XSLocators.TryBeforeYouBuyButton.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            if (!isEnabled)
            {
                return element.GetAttribute("class").Contains("disabled");
            }
            return IsElementEnabled(element);
        }

        public override void ClickTbybButton()
        {
            IWebElement element = FindElement(By.CssSelector(VDPLocators.XSLocators.TryBeforeYouBuyButton.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            ClickElement(element);
            WaitForPageLoad(90);
        }
        #endregion

        public override bool IsViewMapDisplayed()
        {
            throw new NotImplementedException("XS viewport doesn't have View map button");
        }

        public override void ClickOnViewMapButton()
        {
            throw new NotImplementedException("XS viewport doesn't have View map button");
        }

        public override bool? CheckPaymentCalculatorOnVDPWidget()
        {
            By MIWidget = By.CssSelector(VDPLocators.CommonLocators.PaymentCalcWidget.GetAttribute<DescriptionAttribute>().Description);
            By MIWidgetXs = By.CssSelector(VDPLocators.CommonLocators.PaymentCalcWidgetNewXS.GetAttribute<DescriptionAttribute>().Description);
           
                return (IsElementVisible(MIWidget)
                    || IsElementVisible(MIWidgetXs));           
        }

        internal override bool? CheckPaymentCalculatorOnVDPLeadForms()
        {
            ClickOnEmailBtn();
            return !IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.PaymentcalcLeadFormCTA.GetAttribute<DescriptionAttribute>().Description));
        }

        internal override bool? CheckPaymentCalculatorOnVDPPills()
        {
            return (IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.PaymentCalcPill.GetAttribute<DescriptionAttribute>().Description))
                && IsElementVisible(By.CssSelector(VDPLocators.CommonLocators.PaymenctCalcCTAXSHero.GetAttribute<DescriptionAttribute>().Description)));
        }

        public override bool VerifyMIQueryParams(Viewport viewport, Uri testURL)
        {
            string[] hrefs = new string[2]; // Array to store href values
                              
            hrefs[0] = GetElementAttribute(By.CssSelector(VDPLocators.CommonLocators.PaymentCalcPill.GetAttribute<DescriptionAttribute>().Description), "href");
            hrefs[1] = GetElementAttribute(By.CssSelector(VDPLocators.CommonLocators.PaymentCalcWidgetButton.GetAttribute<DescriptionAttribute>().Description), "href");
            
            // Expected parameter values based on the test URL
            NameValueCollection expectedParams = HttpUtility.ParseQueryString(testURL.Query);
            string expectedPlatform = "Mobile";
            string expectedActiveUpsells = expectedParams["ursrc"];
            if (string.IsNullOrEmpty(expectedActiveUpsells) && !string.IsNullOrEmpty(expectedParams["orup"]))
            {
                expectedActiveUpsells = "organic";

            }
            string testURLPath = testURL.AbsolutePath;
            string adidDealerIdPattern = @"\/(\d{1,}_\d{1,})_(\d{1,})\/";
            Match match = Regex.Match(testURLPath, adidDealerIdPattern);
            if (!match.Success)
            {
                adidDealerIdPattern = @"/(\d{1,}_\w+)_([\w\d]+)/";

                match = Regex.Match(testURLPath, adidDealerIdPattern);

                if (!match.Success)
                    return false;
            }


            string adidDealerId = match.Groups[0].Value;
            string expectedAdid = match.Groups[1].Value;
            string expectedDealerId = match.Groups[2].Value.ToUpper();


            foreach (string href in hrefs)
            {
                // Parse href to extract query parameters
                NameValueCollection queryParams = HttpUtility.ParseQueryString(new Uri(href).Query);

                // Get the values of the required parameters
                string platform = queryParams["platform"];
                string activeUpsells = queryParams["activeUpsells"];
                string dealerId = queryParams["dealerId"];
                string adid = queryParams["adid"];

                // Check if all required parameters exist and have the expected values
                bool isValid = !string.IsNullOrEmpty(platform) &&
                               !string.IsNullOrEmpty(activeUpsells) &&
                               !string.IsNullOrEmpty(dealerId) &&
                               !string.IsNullOrEmpty(adid) &&
                               platform == expectedPlatform &&
                               activeUpsells == expectedActiveUpsells &&
                               dealerId == expectedDealerId &&
                               adid == expectedAdid;

                if (!isValid)
                {
                    return false;
                }
            }

            return true;
        }


    }
}