using MarketPlaceWeb.Base;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using UIAutomationLibrary.Locators;

namespace UIAutomationLibrary.Pages.InstantCashOffer
{
    public abstract class InstantCashOfferAbstract : Page
    {
        protected InstantCashOfferAbstract(IWebDriver driver, Viewport viewport, Language language)
        {
            base.driver = driver;
            base.viewport = viewport;
            base.language = language;
        }

        public string GetIcoAttribute(Enum enumValue, IcoLeadForm icoLeadForm)
        {
            switch (icoLeadForm.icoLeadFormType)
            {
                case IcoLeadForm.IcoLeadFormType.TradeIn:
                    return enumValue.GetAttribute<DescriptionTradeInAttribute>().DescriptionTradeIn;
                case IcoLeadForm.IcoLeadFormType.BuyingCenter:
                    return enumValue.GetAttribute<DescriptionBuyingCenterAttribute>().DescriptionBuyingCenter;
                case IcoLeadForm.IcoLeadFormType.DWW:
                    return enumValue.GetAttribute<DescriptionDWWAttribute>().DescriptionDWW;
                default:
                    return enumValue.GetAttribute<DescriptionAttribute>().Description;
            }
        }

        public void WaitForIcoModalToOpen(bool isStatusOpen = true)
        {
            if (!isStatusOpen)
            {
                WaitForElementNotVisible(By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoModal.GetAttribute<DescriptionAttribute>().Description));
            }
            else
            {
                WaitForElementVisible(By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoModal.GetAttribute<DescriptionAttribute>().Description));
                WaitForIcoModalProgress();
            }
        }

        public void WaitForIcoModalProgress(int timeOut = 90)
        {
            try
            {
                WaitForElementNotVisible(By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoModalProgress.GetAttribute<DescriptionAttribute>().Description), timeOut);
            }
            catch (Exception) { }
        }

        public bool IsIcoWidgetAvailable()
        {
            try
            {
                return IsElementVisible(By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoWidget.GetAttribute<DescriptionAttribute>().Description));
            }
            catch (Exception)
            {
                RefreshPage();
                return IsElementVisible(By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoWidget.GetAttribute<DescriptionAttribute>().Description));
            }
        }

        public bool IsDWWIcoOfferAvailable()
        {
            Wait(10);
            By icoOfferDealerLocator = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoOfferParticipatingDealers.GetAttribute<DescriptionAttribute>().Description);
            WaitForElementVisible(icoOfferDealerLocator);
           
            ScrollTo(FindElement(icoOfferDealerLocator)); 
            try
            {
                return IsElementAvailable(By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoOfferParticipatingDealers.GetAttribute<DescriptionAttribute>().Description));
            }
            catch (Exception) {}
            return false;
        }


        public void ClickInstantCashOfferButton(IcoLeadForm icoLeadForm)
        {
            IWebElement element = FindElement(By.CssSelector(GetIcoAttribute(InstantCashOfferLocators.CommonLocators.IcoButton, icoLeadForm)));
            ScrollTo(element,5);
            ClickElementJS(element);
            WaitForIcoModalToOpen();
        }

        public void SelectIcoAnswer(By locator, bool answer, IcoLeadForm icoLeadForm = null,bool scrollIntoView = false, int waitTimeInSecond = 1, string locatorValue = null)
        {
            if(icoLeadForm.icoLeadFormType == IcoLeadForm.IcoLeadFormType.DWW)
            {
                IWebElement element = FindElement(locator);
                if (scrollIntoView) { ScrollTo(element); }
                ClickElementJS(locatorValue);
            }
            else
            {
                IWebElement answerElement = FindElements(locator)[answer == true ? 0 : 1];
                if (scrollIntoView) { ScrollTo(answerElement, waitTimeInSecond); }
                ClickElement(answerElement);
                              
            }            
        }

        public void CheckIcoTerms(IcoLeadForm icoLeadForm, string locatorValue = null, bool toBeChecked = true, bool scrollIntoView = false)
        {
            switch (icoLeadForm.icoLeadFormType)
            {
                case IcoLeadForm.IcoLeadFormType.BuyingCenter:
                case IcoLeadForm.IcoLeadFormType.TradeIn:
                    var element = FindElement(By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoTermsChkbox.GetAttribute<DescriptionAttribute>().Description));
                    if (scrollIntoView) { ScrollTo(element); }
                    if (IsCheckboxChecked(element) != toBeChecked)
                    {
                        ClickElement(FindElement(By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoTermsChkbox.GetAttribute<DescriptionAttribute>().Description + " label span")));
                    }
                    break;
                case IcoLeadForm.IcoLeadFormType.DWW:
                    element = FindElement(By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoTermsChkbox.GetAttribute<DescriptionAttribute>().Description));
                    if (scrollIntoView) { ScrollTo(element); }
                    ClickElementJS(locatorValue);
                    break;
            }           
        }

        public void EnterTextWithLoadOnIcoLead(By locator, string text, bool scrollIntoView = false, int timeOut = 30)
        {
            if (scrollIntoView) { ScrollTo(FindElement(locator),3); }
            EnterText(locator, text);
            WaitForIcoModalProgress(timeOut);
        }

        public abstract void ClickNextBtnOnIcoLead(By locator, string locatorValue = null, bool scrollIntoView = false, int timeOut = 30);
        
        public abstract void EnterYmmtDetails(IcoLeadForm icoLeadForm);
        
        public abstract void EnterVinDetails(IcoLeadForm icoLeadForm);

        public abstract void EnterIncorrectVinDetails();

        public abstract void ClickEnterYourVehicleMakeAndModelBtn();
        public abstract void EnterVehicleDetails(IcoLeadForm icoLeadForm);

        public bool IsHighMileageOldVehiclePageDisplayed()
        {
            try
            {
                WaitForElementVisible(By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoHighMileageOldVehicle.GetAttribute<DescriptionAttribute>().Description));
                return IsElementVisible(By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoHighMileageOldVehicle.GetAttribute<DescriptionAttribute>().Description));
            }
            catch (Exception) { return false; }
        }


        public bool VerifyElementsOnHighMileageOldVehiclePage()
        {

            try
            {

                WaitForElementVisible(By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoHighMileageOldVehicle.GetAttribute<DescriptionAttribute>().Description));

                return IsElementVisible(By.XPath(InstantCashOfferLocators.CommonLocators.CertifyYourCarImage.GetAttribute<DescriptionAttribute>().Description))

                && IsElementVisible(By.XPath(InstantCashOfferLocators.CommonLocators.GetCarPhotosImage.GetAttribute<DescriptionAttribute>().Description))

                && IsElementVisible(By.XPath(InstantCashOfferLocators.CommonLocators.HowToCleanCarImage.GetAttribute<DescriptionAttribute>().Description))

                && IsElementVisible(By.XPath(InstantCashOfferLocators.CommonLocators.VehicleResaleImage.GetAttribute<DescriptionAttribute>().Description))

                && IsElementVisible(By.CssSelector(InstantCashOfferLocators.CommonLocators.PostFreeAd.GetAttribute<DescriptionAttribute>().Description));

            }
            catch (Exception) { return false; }
        }


        public abstract void EnterVehicleConditions(IcoLeadForm icoLeadForm);

        public abstract void EnterCustomerDetails(IcoLeadForm icoLeadForm);
        public abstract void EnterVinDetailsDWW(IcoLeadForm icoLeadForm);

        public abstract void EnterVehicleDetailsDWW(IcoLeadForm icoLeadForm);

        public abstract void EnterVehicleConditionsDWW(IcoLeadForm icoLeadForm);

        public abstract void EnterCustomerDetailsDWW(IcoLeadForm icoLeadForm);

        public bool IsVinTrimEnabled()
        {
            IWebElement element = FindElement(By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoVinTrimDropdown.GetAttribute<DescriptionAttribute>().Description));
            try
            {
                WaitUntil(() => element.Enabled);
                return true;
            }
            catch (Exception) { return false; }
        }

        public void SubmitPhoneCodeVerificationByText(IcoLeadForm icoLeadForm)
        {
            By phoneCodeMethodBtnLocators = null;
            By submitCodeBtnLocator = null;
            By phoneVerifyLocator0 = null;
            By phoneVerifyLocator1 = null;
            By phoneVerifyLocator2 = null;
            By phoneVerifyLocator3 = null;

            switch (icoLeadForm.icoLeadFormType)
            {
                case IcoLeadForm.IcoLeadFormType.BuyingCenter:
                case IcoLeadForm.IcoLeadFormType.TradeIn:
                    phoneCodeMethodBtnLocators = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoCodeMethodBtns.GetAttribute<DescriptionAttribute>().Description);
                    submitCodeBtnLocator = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoSubmitCodeBtn.GetAttribute<DescriptionAttribute>().Description);
                    phoneVerifyLocator0 = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoPhoneVerificationCode0.GetAttribute<DescriptionAttribute>().Description);
                    phoneVerifyLocator1 = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoPhoneVerificationCode1.GetAttribute<DescriptionAttribute>().Description);
                    phoneVerifyLocator2 = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoPhoneVerificationCode2.GetAttribute<DescriptionAttribute>().Description);
                    phoneVerifyLocator3 = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoPhoneVerificationCode3.GetAttribute<DescriptionAttribute>().Description);
                    break;

                case IcoLeadForm.IcoLeadFormType.DWW:
                    phoneCodeMethodBtnLocators = By.XPath(InstantCashOfferLocators.DWWLocators.IcoCodeMethodBtns.GetAttribute<DescriptionAttribute>().Description);
                    submitCodeBtnLocator = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoNextBtn.GetAttribute<DescriptionAttribute>().Description);
                    phoneVerifyLocator0 = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoPhoneVerificationCode0.GetAttribute<DescriptionAttribute>().Description);
                    //phoneVerifyLocator1 = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoPhoneVerificationCode1.GetAttribute<DescriptionAttribute>().Description);
                    //phoneVerifyLocator2 = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoPhoneVerificationCode2.GetAttribute<DescriptionAttribute>().Description);
                    //phoneVerifyLocator3 = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoPhoneVerificationCode3.GetAttribute<DescriptionAttribute>().Description);

                    break;
            }
            ClickElement(FindElements(phoneCodeMethodBtnLocators).FirstOrDefault(x => x.Text.Contains(icoLeadForm.CodeVerificationMethodName)));
            WaitForIcoModalProgress();

            if (icoLeadForm.icoLeadFormType == IcoLeadForm.IcoLeadFormType.DWW)
            {
                WaitForElementVisible(By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoNextBtn.GetAttribute<DescriptionAttribute>().Description));
                EnterTextWithDelay(phoneVerifyLocator0, icoLeadForm.PhoneCode);
            }
            else
            {
                EnterTextWithDelay(phoneVerifyLocator0, icoLeadForm.PhoneCode[0].ToString());
                EnterTextWithDelay(phoneVerifyLocator1, icoLeadForm.PhoneCode[1].ToString());
                EnterTextWithDelay(phoneVerifyLocator2, icoLeadForm.PhoneCode[2].ToString());
                EnterTextWithDelay(phoneVerifyLocator3, icoLeadForm.PhoneCode[3].ToString());

            }

            switch (icoLeadForm.icoLeadFormType)
            {
                case IcoLeadForm.IcoLeadFormType.BuyingCenter:
                case IcoLeadForm.IcoLeadFormType.TradeIn:
                    ClickNextBtnOnIcoLead(submitCodeBtnLocator, timeOut: 90);
                    break;
                case IcoLeadForm.IcoLeadFormType.DWW:
                    ClickNextBtnOnIcoLead(submitCodeBtnLocator, InstantCashOfferLocators.DWWLocators.IcoNextBtn.GetAttribute<DescriptionAttribute>().Description, timeOut: 90);
                    break;
            }
        }

        public bool IsIcoSuccessPageDisplayed()
        {
            return IsElementVisible(By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoOfferSection.GetAttribute<DescriptionAttribute>().Description));
        }

        public int GetParticipatingDealerCount()
        {
            return FindElements(By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoOfferParticipatingDealers.GetAttribute<DescriptionAttribute>().Description)).Count;
        }

        public void CloseIcoSuccessModal()
        {
            IWebElement icoSuccessCloseBtnElement = FindElement(By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoSuccessCloseBtn.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(icoSuccessCloseBtnElement);
            ClickElement(icoSuccessCloseBtnElement);
        }

        public bool IsIcoModalClosed()
        {
            try
            {
                WaitForIcoModalToOpen(false);
                return true;
            }
            catch (Exception) { return false; }
        }

        public bool IsIcoOfferDisplayedOnVdp()
        {
            return IsElementVisible(By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoOfferTitle.GetAttribute<DescriptionAttribute>().Description));
        }

        public bool IsIcoWidgetButtonDisplayed(bool isDisplayed, string buttonText = null)
        {
            By IcoButtonLocator = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoButton.GetAttribute<DescriptionTradeInAttribute>().DescriptionTradeIn);
            if (!isDisplayed)
            {
                if (IsElementVisible(IcoButtonLocator))
                {
                    return !GetElementText(FindElement(IcoButtonLocator)).ToLower().Contains(buttonText.ToLower());
                }
                return !IsElementVisible(IcoButtonLocator);
            }
            else
            {
                if (IsElementVisible(IcoButtonLocator))
                {
                    return GetElementText(FindElement(IcoButtonLocator)).ToLower().Contains(buttonText.ToLower());
                }
                return false;
            }
        }

        public bool IsValidVin(IcoLeadForm icoLeadForm)
        {
            var isValidVin = IsElementAvailable(By.CssSelector(".pr-priceless-vehicle")) || IsElementAvailable(By.CssSelector(".pr-vin-has-active-offer")) ? false : true;
            return isValidVin;
        }

        public bool IsVinServerSideErrorDisplayed()
        {
            return IsElementVisible(By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoVinErrorServerSide.GetAttribute<DescriptionAttribute>().Description));
        }      
    }

    public class IcoLeadForm
    {
        public IcoLeadFormType icoLeadFormType { get; set; }
        public string Vin { get; set; }
        public string Year { get; set; }
        public string Make { get; set; }
        public string Kilometers { get; set; }
        public string PostalCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Color { get; set; }
        public string FrontTireCondition { get; set; }
        public string RearTireCondition { get; set; }
        public string CodeVerificationMethodName { get; set; }
        public string PhoneCode { get; set; }
        public Dictionary<IcoQuestionnaire, bool> IcoQuestionnaireAnswers { get; set; }
        public enum IcoQuestionnaire
        {
            IsOriginalOwner,
            IsMakingPayment,
            IsVehicleReplacement,
            WasInAccident,
            HasCleanHistory,
            HasDamage,
            HasMechanicalIssue,
            HasWarningLight,
            HasModification,
            HasOdor,
            TireCondition,
            HasOtherIssue
        }
        public enum IcoLeadFormType
        {
            TradeIn,
            BuyingCenter,
            DWW
        }
    }

    public class Vin
    {
        public string VinNumber { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public string Manufacturer => VinNumber.Substring(1, 2).ToUpperInvariant();
    }

    public class VinGenerator : Page
    {
        private static readonly IList<string> excludedVinManufacturers = new[]
        {
            "HD"
        };

        private const int minVinYear = 2010;
        private const int errorRetriesMax = 3;
        private const int trialCountMax = 100;

        private static readonly Regex VinRegex = new Regex(@"name=""vin""\sclass=""input""\svalue=""(?<vin>.+?)"".*?<div class=""description""><p><b>VIN Description:</b>\s*?(?<year>\d{4})(?<description>.*?)</p></div>",
            RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Singleline);

        private static Vin GetVin()
        {
            using (WebClient client = new WebClient())
            {
                string html = client.DownloadString("https://vingenerator.org");

                var match = VinRegex.Match(html);
                if (!match.Success)
                {
                    throw new Exception("Unable to parse VIN from vingenerator.org.");
                }

                return new Vin
                {
                    VinNumber = match.Groups["vin"].Value,
                    Year = int.Parse(match.Groups["year"].Value),
                    Description = match.Groups["description"].Value
                };
            }
        }

        public Vin GetRandomVin()
        {
            int errorRetries = 0;

            while (true)
            {
                try
                {
                    for (int trialCount = 0; trialCount < trialCountMax; trialCount++)
                    {
                        var vin = GetVin();
                        if (vin.Year >= minVinYear && !excludedVinManufacturers.Contains(vin.Manufacturer))
                        {
                            return vin;
                        }
                    }
                    throw new Exception("Unable to generate random VIN");
                }
                catch (Exception)
                {
                    if (errorRetries++ >= errorRetriesMax)
                    {
                        throw new Exception("Unable to generate random VIN");
                    }
                    Wait(1);
                }
            }
        }
    }
}
