using System.ComponentModel;


namespace MarketPlaceWeb.Locators
{
    public class HPLocators
    {
        public enum SearchCriteria
        {
            [Description("#rfMakes")]
            Make,
            [Description("#rfModel")]
            Model,
            [Description("#rfPriceHigh")]
            MaxPrice,
            [Description(".searchCriteriaContainer #locationAddress")]
            Location,
            [Description(".mainSearchContainer #locationAddressV2")]
            LocationV2,
            [Description("#SearchButton")]
            SearchButton

        }
        public enum SEOWidgets
        {
            [Description(".bodyTypeItem")]
            BodyTypeSRPLink,
            [Description("#shoppingExperienceRow")]
            BrowseByBodyTypeWidget,
            [Description("#awardWinners")]
            AutoTraderAwardWinnersWidget,
            [Description("#editorialWidget")]
            EditorialArticleWidget,
            [Description(".editorial-logo")]
            EditorialArticleLogo,
            [Description("label[for='tab1']")]
            EditorialLatestTab,
            [Description("label[for='tab2']")]
            EditorialAdviceTab,
            [Description("label[for='tab3']")]
            EditorialReviewsTab,
            [Description("label[for='tab4']")]
            EditorialNewsTab,
            [Description("label[for='tab5']")]
            EditorialCoolStuffTab,
            [Description("#Latest .editorial-list-item .title")]
            EditorialLatestArticles,
            [Description("#Advice .editorial-list-item .title")]
            EditorialAdviceArticles,
            [Description("#Reviews .editorial-list-item .title")]
            EditorialReviewtArticles,
            [Description("#News .editorial-list-item .title")]
            EditorialNewsArticles,
            [Description("#CoolStuff .editorial-list-item .title")]
            EditorialCoolStuffArticles,
            [Description("#Latest .primary")]
            EditorialLatestLeftArticle,
            [Description(".editorial-title")]
            LatestNewsReviewsWidget,
            [Description("#Latest .viewall-others ")]
            EditorialLatestSeeAllLink,
            [Description("#Advice .viewall-others ")]
            EditorialAdviceSeeAllLink,
            [Description("#News .viewall-others ")]
            EditorialNewsSeeAllLink,
            [Description("#Reviews .viewall-others ")]
            EditorialReviewsSeeAllLink,
            [Description("#CoolStuff .viewall-others ")]
            EditorialCoolStuffSeeAllLink,
            [Description("#videoWidget")]
            EditorialVideoWidget,
            [Description(".video-logo")]
            AutoTraderTVLogo,
            [Description(".video-title")]
            UpNextHeader,
            [Description(".video-list a")]
            ATVideoLinks,
            [Description("#videoWidget div.primary a")]
            ATPrimaryVideoLinks,
            [Description("#videoWidget div.secondary span.title")]
            ATVtitle,
            [Description("#videoWidget a")]
            ATSeeAllLink,
            [Description("#newArrival")]
            NewArrivalWidget,
            [Description("#popularCarsRow")]
            MostPopularCarsWidget,
            [Description("#awardWinners a")]
            SeeAllAwardsLink,
            [Description(".category-learn-more")]
            LearMoreAwardsLink,
            [Description("div.title > div.make-model")]
            AwardsMMName,
            [Description(".category-button")]
            AwardsMMForSaleLink,
            [Description("#Latest a")]
            NewsReviewsArticleLink,
            [Description(".viewall-others")]
            NewsReviewsViewAllLink,
            [Description(".newArrival-explore")]
            NewArrivalsExploreLink,
            [Description("a.view-inventory-title")]
            NewArrivalsViewInventoryLink
        }
        public enum FooterLocators
        {
            [Description(".footer-links a")]
            FooterLinks
        }

        public enum SsoLoginModal
        {
            [Description("#loginframe")]
            LoginIframe,
            [Description("#signInName")]
            Email,
            [Description("#password")]
            Password,
            [Description("#forgotPassword")]
            ForgotYourPassword,
            [Description("button#next")]
            LoginBtn,
            [Description("a#createAccount")]
            RegisterHere,
            [Description("#FacebookExchange")]
            SignInWithFB,
            [Description("#GoogleExchange")]
            SignInWithGoogle,
            [Description("#AppleExchange")]
            SignInWithApple,
            [Description("input#email")]
            CreateYourAccountEmail,
            [Description("div.error.pageLevel")]
            ErrorInvalidUser,
            [Description("div.error.itemLevel p")]
            ErrorNoUserName,
            [Description("div.error.itemLevel p[role=\"alert\"]")]
            ErrorNoPassword,

            [Description("#ssoModal")]
            SsoModal
        }
    }
}
