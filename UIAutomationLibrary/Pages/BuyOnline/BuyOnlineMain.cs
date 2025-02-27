using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Pages.BuyOnline
{
    public class BuyOnlineMain : Page
    {
        BuyOnlineAbstract buyOnline;

        public BuyOnlineMain(IWebDriver driver, Viewport viewport, Language language)
        {
            base.driver = driver;
            base.viewport = viewport;
            base.language = language;
            switch (viewport)
            {
                case Viewport.Large:
                    buyOnline = new BuyOnlineLarge(driver, viewport, language);
                    break;
                case Viewport.XS:
                    buyOnline = new BuyOnlineXS(driver, viewport, language);
                    break;
                case Viewport.Small:
                    buyOnline = new BuyOnlineSmall(driver, viewport, language);
                    break;
            }
        }

        public bool IsBuyOnlinePageDisplayed(string buyOnlineUrl)
        {
            return buyOnline.IsBuyOnlinePageDisplayed(buyOnlineUrl);
        }

        public string GetBuyYourNextCarTitle()
        {
            return buyOnline.GetBuyYourNextCarTitle();
        }

        public bool IsSearchAllVehiclesButtonAvailable()
        {
            return buyOnline.IsSearchAllVehiclesButtonAvailable();
        }

        public string GetGetNotifiedTitle()
        {
            return buyOnline.GetGetNotifiedTitle();
        }

        public bool IsGetOnTheListLinkAvailable()
        {
            return buyOnline.IsGetOnTheListLinkAvailable();
        }

        public bool IsGetNotifiedButtonAvailable()
        {
            return buyOnline.IsGetNotifiedButtonAvailable();
        }

        public void ClickGetNotifiedBtn()
        {
            buyOnline.ClickGetNotifiedBtn();
        }

        public void ClickGetOnTheListLink()
        {
            buyOnline.ClickGetOnTheListLink();
        }

        public void EnterEmailOnGetNotified(string email)
        {
            buyOnline.EnterEmailOnGetNotified(email);
        }

        public void EnterPostalCodeOnGetNotified(string postalCode)
        {
            buyOnline.EnterPostalCodeOnGetNotified(postalCode);
        }

        public void CheckTermsOnGetNotified(bool toBeChecked = true)
        {
            buyOnline.CheckTermsOnGetNotified(toBeChecked);
        }

        public bool IsGetNotifiedBtnEnabled(bool isEnabled = true)
        {
            return buyOnline.IsGetNotifiedBtnEnabled(isEnabled);
        }

        public void ClickGetNotifiedBtnOnModal()
        {
            buyOnline.ClickGetNotifiedBtnOnModal();
        }

        public void ClickFaqQuestion1(bool isExpanded = true)
        {
            buyOnline.ClickFaqQuestion1(isExpanded);
        }

        public bool IsQuestion1AnswerDisplayed()
        {
            return buyOnline.IsQuestion1AnswerDisplayed();
        }

        public string GetSuccessMessageDisplayedOnModal()
        {
            return buyOnline.GetSuccessMessageDisplayedOnModal();
        }

        public bool IsCloseBtnDisplayedOnModal()
        {
            return buyOnline.IsCloseBtnDisplayedOnModal();
        }

        public void ClickCloseBtnOnModal()
        {
            buyOnline.ClickCloseBtnOnModal();
        }

        public bool IsPrivacyLinkOnGetNotifiedDisplayed()
        {
            return buyOnline.IsPrivacyLinkOnGetNotifiedDisplayed();
        }

        public void ClickPrivacyLinkOnGetNotified()
        {
            buyOnline.ClickPrivacyLinkOnGetNotified();
        }

        public bool IsRedirectToPrivacyFromGetNotified(string privacyUrl)
        {
            return buyOnline.IsRedirectToPrivacyFromGetNotified(privacyUrl);
        }

        public bool IsPrivacySectionDisplayed()
        {
            return buyOnline.IsPrivacySectionDisplayed();
        }

        public bool IsTermsLinkOnGetNotifiedDisplayed()
        {
            return buyOnline.IsTermsLinkOnGetNotifiedDisplayed();
        }

        public void ClickTermsLinkOnGetNotified()
        {
            buyOnline.ClickTermsLinkOnGetNotified();
        }

        public bool IsRedirectToTermsFromGetNotified(string termsUrl)
        {
            return buyOnline.IsRedirectToTermsFromGetNotified(termsUrl);
        }

        public bool IsTermsSectionDisplayed()
        {
            return buyOnline.IsTermsSectionDisplayed();
        }

        public string GetJustATapAwayTitle()
        {
            return buyOnline.GetJustATapAwayTitle();
        }

        public string GetBuyingProcessSubtitle()
        {
            return buyOnline.GetBuyingProcessSubtitle();
        }

        public string GetCustomizePurchaseSubtitle()
        {
            return buyOnline.GetCustomizePurchaseSubtitle();
        }

        public string GetApplyCreditSubtitle()
        {
            return buyOnline.GetApplyCreditSubtitle();
        }

        public string GetReserveVehicleSubtitle()
        {
            return buyOnline.GetReserveVehicleSubtitle();
        }

        public string GetTradeInVehicleSubtitle()
        {
            return buyOnline.GetTradeInVehicleSubtitle();
        }

        public bool IsMoreAboutTradeInLinkAvailable()
        {
            return buyOnline.IsMoreAboutTradeInLinkAvailable();
        }

        public string GetShowMeAllVehiclesTitle()
        {
            return buyOnline.GetShowMeAllVehiclesTitle();
        }

        public bool IsMakeDropdownAvailable()
        {
            return buyOnline.IsMakeDropdownAvailable();
        }

        public bool IsLocationTextboxAvailable()
        {
            return buyOnline.IsLocationTextboxAvailable();
        }

        public bool IsShowMeVehiclesButtonAvailable()
        {
            return buyOnline.IsShowMeVehiclesButtonAvailable();
        }

        public bool IsTrendingDealsAvailable()
        {
            return buyOnline.IsTrendingDealsAvailable();
        }

        public string GetOnlineSellerPlusTitle()
        {
            return buyOnline.GetOnlineSellerPlusTitle();
        }

        public bool IsFindOutMoreLinkAvailable()
        {
            return buyOnline.IsFindOutMoreLinkAvailable();
        }

        public string GetBestCarDealsTitle()
        {
            return buyOnline.GetBestCarDealsTitle();
        }

        public string GetPriceGuidanceSubtitle()
        {
            return buyOnline.GetPriceGuidanceSubtitle();
        }

        public string GetLiveChatCarouselTitle()
        {
            return buyOnline.GetLiveChatCarouselTitle();
        }

        public string GetHomeTestDriveCarouselTitle()
        {
            return buyOnline.GetHomeTestDriveCarouselTitle();
        }

        public string GetVirtualAppraisalCarouselTitle()
        {
            return buyOnline.GetVirtualAppraisalCarouselTitle();
        }

        public string GetOnlineReservationCarouselTitle()
        {
            return buyOnline.GetOnlineReservationCarouselTitle();
        }

        public string GetLocalDeliveryCarouselTitle()
        {
            return buyOnline.GetLocalDeliveryCarouselTitle();
        }

        public string GetICOTitle()
        {
            return buyOnline.GetICOTitle();
        }

        public string GetFaqTitle()
        {
            return buyOnline.GetFaqTitle();
        }

        public bool IsFaqAccordionDisplayed()
        {
            return buyOnline.IsFaqAccordionDisplayed();
        }

        public bool IsGreatPriceBadgeAvailable()
        {
            return buyOnline.IsGreatPriceBadgeAvailable();
        }

        public bool IsGoodPriceBadgeAvailable()
        {
            return buyOnline.IsGoodPriceBadgeAvailable();
        }

        public bool IsFairPriceBadgeAvailable()
        {
            return buyOnline.IsFairPriceBadgeAvailable();
        }

        public bool IsAbovePriceBadgeAvailable()
        {
            return buyOnline.IsAbovePriceBadgeAvailable();
        }

        public bool IsMoreDetailsButtonAvailable()
        {
            return buyOnline.IsMoreDetailsButtonAvailable();
        }

        public void ClickMoreDetailsBtnOnBuyOnline()
        {
            buyOnline.ClickMoreDetailsBtnOnBuyOnline();
        }

        public bool IsFaqSectionAvailable()
        {
            return buyOnline.IsFaqSectionAvailable();
        }

        public void ClickSearchAllVehiclesBtn()
        {
            buyOnline.ClickSearchAllVehiclesBtn();
        }

        public void SelectMakeOnBuyOnline(string makeValue)
        {
            buyOnline.SelectMakeOnBuyOnline(makeValue);
        }

        public void SelectModelOnBuyOnline(string modelValue)
        {
            buyOnline.SelectModelOnBuyOnline(modelValue);
        }

        public void EnterPostalCodeOnBuyOnline(string postalCode)
        {
            buyOnline.EnterPostalCodeOnBuyOnline(postalCode);
        }

        public int GetShowMeVehiclesCount()
        {
            return buyOnline.GetShowMeVehiclesCount();
        }

        public void ClickShowMeVehiclesBtn(bool blankInputTest = false)
        {
            buyOnline.ClickShowMeVehiclesBtn(blankInputTest);
        }

        public bool IsPostalCodeWarningMsgDisplayed()
        {
            return buyOnline.IsPostalCodeWarningMsgDisplayed();
        }

        public List<string> GetMakeModelOnTrendingDeals()
        {
            return buyOnline.GetMakeModelOnTrendingDeals();
        }

        public void ClickTrendingDealsBadge1()
        {
            buyOnline.ClickTrendingDealsBadge1();
        }
    }
}
