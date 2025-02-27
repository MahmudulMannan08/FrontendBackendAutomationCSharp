using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Pages.BuyOnline
{
    public abstract class BuyOnlineAbstract : Page
    {
        protected BuyOnlineAbstract(IWebDriver driver, Viewport viewport, Language language)
        {
            base.driver = driver;
            base.viewport = viewport;
            base.language = language;
        }

        public bool IsBuyOnlinePageDisplayed(string buyOnlineUrl)
        {
            if (!IsInCurrentUrl(buyOnlineUrl))
            {
                WaitForElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.SearchAllVehiclesBtn.GetAttribute<DescriptionAttribute>().Description), 60);
                return true;
            }
            return IsInCurrentUrl(buyOnlineUrl);
        }

        public string GetBuyYourNextCarTitle()
        {
            IWebElement element = FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.BuyYourNextCarTitle.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            return GetElementText(element);
        }

        public string GetGetNotifiedTitle()
        {
            IWebElement element = FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.GetNotifiedTitle.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            return GetElementText(element);
        }

        public string GetJustATapAwayTitle()
        {
            IWebElement element = FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.JustATapAwayTitle.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            return GetElementText(element);
        }

        public string GetBuyingProcessSubtitle()
        {
            IWebElement element = FindElements(By.CssSelector(BuyOnlineLocators.BuyOnline.Subtitles.GetAttribute<DescriptionAttribute>().Description))[0];
            ScrollTo(element);
            return GetElementText(element);
        }

        public string GetCustomizePurchaseSubtitle()
        {
            IWebElement element = FindElements(By.CssSelector(BuyOnlineLocators.BuyOnline.Subtitles.GetAttribute<DescriptionAttribute>().Description))[1];
            ScrollTo(element);
            return GetElementText(element);
        }

        public string GetApplyCreditSubtitle()
        {
            IWebElement element = FindElements(By.CssSelector(BuyOnlineLocators.BuyOnline.Subtitles.GetAttribute<DescriptionAttribute>().Description))[2];
            ScrollTo(element);
            return GetElementText(element);
        }

        public string GetReserveVehicleSubtitle()
        {
            IWebElement element = FindElements(By.CssSelector(BuyOnlineLocators.BuyOnline.Subtitles.GetAttribute<DescriptionAttribute>().Description))[3];
            ScrollTo(element);
            return GetElementText(element);
        }

        public string GetTradeInVehicleSubtitle()
        {
            IWebElement element = FindElements(By.CssSelector(BuyOnlineLocators.BuyOnline.Subtitles.GetAttribute<DescriptionAttribute>().Description))[4];
            ScrollTo(element);
            return GetElementText(element);
        }

        public string GetShowMeAllVehiclesTitle()
        {
            IWebElement element = FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.ShowMeAllVehiclesTitle.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            return GetElementText(element);
        }

        public string GetOnlineSellerPlusTitle()
        {
            IWebElement element = FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.OnlineSellerPlusTitle.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            return GetElementText(element);
        }

        public string GetBestCarDealsTitle()
        {
            IWebElement element = FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.BestCarDealsTitle.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            return GetElementText(element).Replace("\r\n", " ");
        }

        public string GetPriceGuidanceSubtitle()
        {
            IWebElement element = FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.PriceGuidanceSubtitle.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            return GetElementText(element);
        }

        public string GetICOTitle()
        {
            IWebElement element = FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.ICOTitle.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            return GetElementText(element);
        }

        public string GetFaqTitle()
        {
            IWebElement element = FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.FAQTitle.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            return GetElementText(element);
        }

        public bool IsFaqAccordionDisplayed()
        {
            return IsElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.FAQAccordion.GetAttribute<DescriptionAttribute>().Description));
        }

        public bool IsSearchAllVehiclesButtonAvailable()
        {
            return IsElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.SearchAllVehiclesBtn.GetAttribute<DescriptionAttribute>().Description));
        }

        public bool IsGetOnTheListLinkAvailable()
        {
            IWebElement element = FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.GetOnTheListLink.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            return IsElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.GetOnTheListLink.GetAttribute<DescriptionAttribute>().Description));
        }

        public bool IsGetNotifiedButtonAvailable()
        {
            return IsElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.GetNotifiedBtn.GetAttribute<DescriptionAttribute>().Description));
        }

        public void ClickGetNotifiedBtn()
        {
            ClickElement(FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.GetNotifiedBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.GetNotifiedModal.GetAttribute<DescriptionAttribute>().Description));
        }

        public void ClickGetOnTheListLink()
        {
            ClickElement(FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.GetOnTheListLink.GetAttribute<DescriptionAttribute>().Description)));
            WaitForElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.GetNotifiedModal.GetAttribute<DescriptionAttribute>().Description));
        }

        public void EnterEmailOnGetNotified(string email)
        {
            EnterText(By.CssSelector(BuyOnlineLocators.BuyOnline.EmailGetNotified.GetAttribute<DescriptionAttribute>().Description), email);
        }

        public void EnterPostalCodeOnGetNotified(string postalCode)
        {
            EnterText(By.CssSelector(BuyOnlineLocators.BuyOnline.PostalCodeGetNotified.GetAttribute<DescriptionAttribute>().Description), postalCode);
        }

        public void CheckTermsOnGetNotified(bool toBeChecked = true)
        {
            IWebElement element = FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.TermsCheckboxGetNotified.GetAttribute<DescriptionAttribute>().Description));
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.TermsCheckboxGetNotified.GetAttribute<DescriptionAttribute>().Description)));
            }
        }

        public bool IsGetNotifiedBtnEnabled(bool isEnabled = true)
        {
            if (!isEnabled)
            {
                return !IsElementEnabled(FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.GetNotifiedBtnOnModal.GetAttribute<DescriptionAttribute>().Description)));
            }
            else
            {
                return IsElementEnabled(FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.GetNotifiedBtnOnModal.GetAttribute<DescriptionAttribute>().Description)));
            }
        }

        public void ClickGetNotifiedBtnOnModal()
        {
            ClickElement(FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.GetNotifiedBtnOnModal.GetAttribute<DescriptionAttribute>().Description)));
        }

        public void ClickFaqQuestion1(bool isExpanded = true)
        {
            IWebElement element = FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.FAQQuestion1Btn.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(element);
            if (!isExpanded)
            {
                WaitUntil(() => element.GetAttribute("aria-expanded").ToLower().Equals("false"));  //Wait until Q1 is collapsed
            }
            else
            {
                WaitUntil(() => element.GetAttribute("aria-expanded").ToLower().Equals("true"));  //Wait until Q1 is expanded
            }
        }

        public bool IsQuestion1AnswerDisplayed()
        {
            return IsElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.FAQQuestion1Answer.GetAttribute<DescriptionAttribute>().Description));
        }

        public string GetSuccessMessageDisplayedOnModal()
        {
            return GetElementText(FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.SuccessMsgOnModal.GetAttribute<DescriptionAttribute>().Description)));
        }

        public bool IsCloseBtnDisplayedOnModal()
        {
            return IsElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.CloseBtnOnModal.GetAttribute<DescriptionAttribute>().Description));
        }

        public void ClickCloseBtnOnModal()
        {
            ClickElement(FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.CloseBtnOnModal.GetAttribute<DescriptionAttribute>().Description)));
            WaitForElementNotVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.GetNotifiedSuccessModal.GetAttribute<DescriptionAttribute>().Description));
        }

        public bool IsPrivacyLinkOnGetNotifiedDisplayed()
        {
            return IsElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.PrivacyLinkGetNotified.GetAttribute<DescriptionAttribute>().Description));
        }

        public void ClickPrivacyLinkOnGetNotified()
        {
            ClickElement(FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.PrivacyLinkGetNotified.GetAttribute<DescriptionAttribute>().Description)));
        }

        public bool IsRedirectToPrivacyFromGetNotified(string privacyUrl)
        {
            SwitchTabOrWindow(() => driver.Url.Contains(privacyUrl));
            return IsInCurrentUrl(privacyUrl);
        }

        public bool IsPrivacySectionDisplayed()
        {
            return IsElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.PrivacyHeaderPrivacyPage.GetAttribute<DescriptionAttribute>().Description));
        }

        public bool IsTermsLinkOnGetNotifiedDisplayed()
        {
            return IsElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.TermsLinkGetNotified.GetAttribute<DescriptionAttribute>().Description));
        }

        public void ClickTermsLinkOnGetNotified()
        {
            ClickElement(FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.TermsLinkGetNotified.GetAttribute<DescriptionAttribute>().Description)));
        }

        public bool IsRedirectToTermsFromGetNotified(string termsUrl)
        {
            SwitchTabOrWindow(() => driver.Url.Contains(termsUrl));
            return IsInCurrentUrl(termsUrl);
        }

        public bool IsTermsSectionDisplayed()
        {
            return IsElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.TermsHeaderPrivacyPage.GetAttribute<DescriptionAttribute>().Description));
        }

        public bool IsMoreAboutTradeInLinkAvailable()
        {
            return IsElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.MoreAboutTradeInLink.GetAttribute<DescriptionAttribute>().Description));
        }

        public bool IsMakeDropdownAvailable()
        {
            return IsElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.MakeDropdown.GetAttribute<DescriptionAttribute>().Description));
        }

        public bool IsLocationTextboxAvailable()
        {
            return IsElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.LocationTextbox.GetAttribute<DescriptionAttribute>().Description));
        }

        public bool IsShowMeVehiclesButtonAvailable()
        {
            return IsElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.ShowMeVehiclesBtn.GetAttribute<DescriptionAttribute>().Description));
        }

        public bool IsTrendingDealsAvailable()
        {
            return IsElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.TrendingDealsDiv.GetAttribute<DescriptionAttribute>().Description));
        }

        public bool IsFindOutMoreLinkAvailable()
        {
            return IsElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.FindOutMoreLink.GetAttribute<DescriptionAttribute>().Description));
        }

        public abstract bool IsGreatPriceBadgeAvailable();

        public abstract bool IsGoodPriceBadgeAvailable();

        public abstract bool IsFairPriceBadgeAvailable();

        public abstract bool IsAbovePriceBadgeAvailable();

        public abstract string GetLiveChatCarouselTitle();

        public abstract string GetHomeTestDriveCarouselTitle();

        public abstract string GetVirtualAppraisalCarouselTitle();

        public abstract string GetOnlineReservationCarouselTitle();

        public abstract string GetLocalDeliveryCarouselTitle();

        public abstract void WaitForSRPPageLoad(int timeOut = 60);

        public bool IsMoreDetailsButtonAvailable()
        {
            IWebElement element = FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.MoreDetailsBtn.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            return IsElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.MoreDetailsBtn.GetAttribute<DescriptionAttribute>().Description));
        }

        public void ClickMoreDetailsBtnOnBuyOnline()
        {
            ClickElement(FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.MoreDetailsBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForPageLoad(30);
        }

        public bool IsFaqSectionAvailable()
        {
            IWebElement element = FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.FaqDiv.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            return IsElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.FaqDiv.GetAttribute<DescriptionAttribute>().Description));
        }

        public void ClickSearchAllVehiclesBtn()
        {
            ClickElement(FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.SearchAllVehiclesBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.ShowMeVehiclesBtn.GetAttribute<DescriptionAttribute>().Description));
        }

        public void SelectMakeOnBuyOnline(string makeValue)
        {
            SelectByValue(By.CssSelector(BuyOnlineLocators.BuyOnline.MakeDropdown.GetAttribute<DescriptionAttribute>().Description), makeValue);
        }

        public void SelectModelOnBuyOnline(string modelValue)
        {
            SelectByValue(By.CssSelector(BuyOnlineLocators.BuyOnline.ModelDropdown.GetAttribute<DescriptionAttribute>().Description), modelValue);
        }

        public void EnterPostalCodeOnBuyOnline(string postalCode)
        {
            EnterText(By.CssSelector(BuyOnlineLocators.BuyOnline.LocationTextbox.GetAttribute<DescriptionAttribute>().Description), postalCode);
        }

        public int GetShowMeVehiclesCount()
        {
            try
            {
                return Int32.Parse(new string(GetElementText(FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.ShowMeVehiclesBtn.GetAttribute<DescriptionAttribute>().Description))).Where(char.IsDigit).ToArray()));
            }
            catch (FormatException e)
            {
                throw new Exception("Unable to parse Show Me Vehicles count: " + e.Message);
            }
        }

        public void ClickShowMeVehiclesBtn(bool blankInputTest = false)
        {
            ClickElement(FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.ShowMeVehiclesBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForPageLoad(90);

            if (!blankInputTest)
            {
                ReloadOnErrorDisplay(By.CssSelector(CommonLocators.ErrorPage.ErrorPage500.GetAttribute<System.ComponentModel.DescriptionAttribute>().Description));
                WaitForSRPPageLoad();
            }
        }

        public bool IsPostalCodeWarningMsgDisplayed()
        {
            return IsElementVisible(By.CssSelector(BuyOnlineLocators.BuyOnline.PostalCodeWarningMsg.GetAttribute<DescriptionAttribute>().Description));
        }

        public List<string> GetMakeModelOnTrendingDeals()
        {
            return GetElementText(FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.TrendingDealBadge1.GetAttribute<DescriptionAttribute>().Description))).Split(' ').ToList();
        }

        public void ClickTrendingDealsBadge1()
        {
            ClickElement(FindElement(By.CssSelector(BuyOnlineLocators.BuyOnline.TrendingDealBadge1.GetAttribute<DescriptionAttribute>().Description)));
            WaitForPageLoad(90);
        }
    }
}
