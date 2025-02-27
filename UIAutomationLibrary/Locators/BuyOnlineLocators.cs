using System;
using System.ComponentModel;

namespace MarketPlaceWeb.Locators
{
    public class BuyOnlineLocators
    {
        public enum BuyOnline
        {
            [Description("#bannerTitle")]
            BuyYourNextCarTitle,
            [Description(".get-notify-title")]
            GetNotifiedTitle,
            [Description(".card-title-1")]
            JustATapAwayTitle,
            [Description(".card-title-2")]
            Subtitles,
            [Description("#bh-searchbox-heading-div div")]
            ShowMeAllVehiclesTitle,
            [Description(".online-seller-plus-title")]
            OnlineSellerPlusTitle,
            [Description(".best-deals-heading")]
            BestCarDealsTitle,
            [Description(".best-deals-card1-title")]
            PriceGuidanceSubtitle,
            [Description(".ico-title")]
            ICOTitle,
            [Description("#faq h1")]
            FAQTitle,
            [Description("#accordion")]
            FAQAccordion,
            [Description("#q1 a")]
            FAQQuestion1Btn,
            [Description("#a1")]
            FAQQuestion1Answer,
            [Description("#postalCodeWarning")]
            PostalCodeWarningMsg,
            [Description("button.banner-search-all-btn")]
            SearchAllVehiclesBtn,
            [Description(".get-notify-text a")]
            GetOnTheListLink,
            [Description(".get-notification-button button")]
            GetNotifiedBtn,
            [Description("#getNotifiedFlyoutOverlay")]
            GetNotifiedModal,
            [Description("#continueSearchFlyoutOverlay")]
            GetNotifiedSuccessModal,
            [Description("#email")]
            EmailGetNotified,
            [Description("#postalCode")]
            PostalCodeGetNotified,
            [Description("#privacyPolicy")]
            TermsCheckboxGetNotified,
            [Description("#get-notified-btn")]
            GetNotifiedBtnOnModal,
            [Description("#continueSearchFlyoutOverlay .section-title-description")]
            SuccessMsgOnModal,
            [Description("#closeFlyout")]
            CloseBtnOnModal,
            [Description("#getNotifiedInputForm a[href='/Cms/PrivacyPolicy#privacy']")]
            PrivacyLinkGetNotified,
            [Description("#privacy")]
            PrivacyHeaderPrivacyPage,
            [Description("#getNotifiedInputForm a[href='/Cms/PrivacyPolicy#terms']")]
            TermsLinkGetNotified,
            [Description("#terms")]
            TermsHeaderPrivacyPage,
            [Description("a[href='/ico/']")]
            MoreAboutTradeInLink,
            [Description("#rfMakes")]
            MakeDropdown,
            [Description("#rfModel")]
            ModelDropdown,
            [Description("#locationAddress")]
            LocationTextbox,
            [Description("#SearchButton")]
            ShowMeVehiclesBtn,
            [Description("#bh-searchbox-trending-div")]
            TrendingDealsDiv,
            [Description(".online-seller-plus-link")]
            FindOutMoreLink,
            [Description(".price-analysis-great-deal")]
            GreatPriceBadge,
            [Description(".price-analysis-good-deal")]
            GoodPriceBadge,
            [Description(".price-analysis-fair-deal")]
            FairPriceBadge,
            [Description(".price-analysis-above-deal")]
            AbovePriceBadge,
            [Description("#best-deals-carousel-2-lg .best-deals-carousel-2-card-title")]
            Carousels,
            [Description(".ico-title + button")]
            MoreDetailsBtn,
            [Description("#faq")]
            FaqDiv,
            [Description("#bh-searchbox-trending-badge1")]
            TrendingDealBadge1
        }

        public enum XSLocators
        {
            [Description("#best-deals-carousel-xs-exp img[alt='great deal']")]
            GreatPriceBadge,
            [Description("#best-deals-carousel-xs-exp img[alt='good deal']")]
            GoodPriceBadge,
            [Description("#best-deals-carousel-xs-exp img[alt='fair deal']")]
            FairPriceBadge,
            [Description("#best-deals-carousel-xs-exp img[alt='above deal']")]
            AbovePriceBadge,
            [Description("#best-deals-carousel-2-xs-exp .best-deals-carousel-2-card-title")]
            Carousels,
        }

        public enum SmallLocators
        {
            [Description("#best-deals-carousel-sm-exp img[alt='great deal']")]
            GreatPriceBadge,
            [Description("#best-deals-carousel-sm-exp img[alt='good deal']")]
            GoodPriceBadge,
            [Description("#best-deals-carousel-sm-exp img[alt='fair deal']")]
            FairPriceBadge,
            [Description("#best-deals-carousel-sm-exp img[alt='above deal']")]
            AbovePriceBadge,
            [Description("#best-deals-carousel-2-sm-exp .best-deals-carousel-2-card-title")]
            Carousels,
        }
    }
}
