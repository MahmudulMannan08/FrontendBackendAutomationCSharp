using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIAutomationLibrary.Locators
{
    public class HubPagesLocators
    {
        internal class DescriptionFrenchAttribute : Attribute
        {
            public string DescriptionFrench { get; set; }

            public DescriptionFrenchAttribute(string value)
            {
                DescriptionFrench = value;
            }
        }
        internal class DescriptionMakeAttribute : Attribute
        {
            public string DescriptionMake { get; set; }

            public DescriptionMakeAttribute(string value)
            {
                DescriptionMake = value;
            }
        }
        internal class DescriptionMakeModelAttribute : Attribute
        {
            public string DescriptionMakeModel { get; set; }

            public DescriptionMakeModelAttribute(string value)
            {
                DescriptionMakeModel = value;
            }
        } 
        public enum ResearchPage
        {
            [Description("#news-reviews-dropdown")]
            ReviewsAdviceLink,

            [Description("Research Car Options"), DescriptionFrench("Rechercher véhicule")]
            VehicleResearchLink,
            [Description(".navbar-menu")]
            HamburgurMenu,
            [Description("link[rel='canonical")]
            CanonicalUrl,
            [Description("Home"), DescriptionFrench("Accueil")]
            HomeBreadCrumb,
            [Description("#hub-make-dropdown"), DescriptionMake("#make-make-dropdown"), DescriptionMakeModel("#mm-make-dropdown")]
            MakeDropDown,
            [Description("#hub-model-dropdown"), DescriptionMake("#make-model-dropdown"), DescriptionMakeModel("#mm-model-dropdown")]
            ModelDropDown,
            [Description("#hub-year-dropdown"), DescriptionMake("#make-year-dropdown"), DescriptionMakeModel("#mm-year-dropdown")]
            YearDropDown,
            [Description("#hub-search-button"), DescriptionMake("#make-search-button"), DescriptionMakeModel("#mm-search-button")]
            SearchBtn,
            [Description("#hubHome-make-compare-1")]
            Make1CompareTool,
            [Description("#hubHome-model-compare-1")]
            Model1CompareTool,
            [Description("#hubHome-year-compare-1")]
            Year1CompareTool,
            [Description("#hubHome-make-compare-2")]
            Make2CompareTool,
            [Description("#hubHome-model-compare-2")]
            Model2CompareTool,
            [Description("#hubHome-year-compare-2")]
            Year2CompareTool,
            [Description("#hubHome-compare-button")]
            CompareBtn,
            [Description("h1")]
            H1Tag,

            [Description("Ford")]
            ResearchByMakeLink,
            [Description(".hubHome__research__toggle__more")]
            MoreMakesLink,
            [DescriptionMake("vehicles_latest")]
            LatestAndUpcomingVehicles,
            [Description("vehicles_Coupes"), DescriptionMake("vehicles_Coupe")]
            Coupes,

            [Description("vehicles_SUVs"), DescriptionMake("vehicles_SUV")]
            SUVs,

            [Description("vehicles_Hatchbacks"), DescriptionMake("vehicles_Hatchback")]
            Hatchbacks,

            [Description("vehicles_Minivans"), DescriptionMake("vehicles_Minivan")]
            Minivans,

            [Description("vehicles_Sedans"), DescriptionMake("vehicles_Sedan")]
            Sedans,
            [Description("vehicles_Trucks"), DescriptionMake("vehicles_Truck")]
            Trucks,
            [Description("vehicles_Wagons")]
            Wagons,
            [DescriptionMake("vehicles_Convertible")]
            Convertibles,
            [Description(".hubHome__vehicles__carouselItemImage__image")]
            CarouselImages,
            [Description(".hubHome__vehicles__viewDetailsButton"), DescriptionMake(".hubMake__vehicles__viewDetailsButton")]
            ViewDetailsBtn,
            [Description(".hubAreaReviewAndNews__button")]
            ViewAllArticlesBtn,
            [Description(".hubAreaReviewAndNews__carouselItem__image")]
            ArticlesCarouselImages,


        }
        public enum MakePage
        {
            [Description(".hero__makeAndModelText")]
            H1,
            [Description(".hubMake__models__link")]
            MakeModelLink,
            [Description(".hubAreaInventory__viewDetailsButton.National")]
            ViewAllDealsBtn,

        }
        public enum YearMakeModelPage
        {
            [Description(".hero-scores")]
            HeroScores,
            [Description(".hub-hero-section")]
            HeroSection,
            [Description("#photos")]
            PhotosSection,
            [Description(".hub-key-specs")]
            KeySpecsSection,
            [Description(".hub-social-icons-nonmobile")]
            SocialIcons,
            [Description("#buyersGuide")]
            BuyersGuide,
            [Description("#trimTable")]
            TrimComparison,
            [Description("#videoWidgetHubPages")]
            VideoWidget,
            [Description("#reviewsandNews")]
            ReviewsNewsWidget,
            [Description("#traderScores")]
            ATScoresWidget,
            [Description("#OwnerScores")]
            OwnerScoresWidget,
            [Description("#recallInformation")]
            RecallInfoWidget,
            [Description("#faqHubPages")]
            QnAWidget,
            [Description("#Inventory")]
            InventoryWidget,
            [Description("#similarVehicles")]
            SimilarVehiclesWidget,
            [Description(".selectAnotherVehicle")]
            VehicleResearchSection,
            [Description(".hubAreaOwnerScores__gauge.gauge-markup.gauage-type-2")]
            HeroATScore,
            [Description("span.badgeScore")]
            ATBadgeScore,
            [Description(".traderReadMore")]
            ATScoreReadMoreLink,
            [Description(".hubAreaInventory__make")]
            InventoryTitles,
            [Description("#hub_inv_Calgary")]
            CalgaryTab,
            [Description(".hubAreaInventory__location")]
            InventoryCity,
            [Description("[data-gt_type = reviews_news_article_link]")]
            ReviewsNewsArticles,
            [Description("div.hubAreaReviewAndNews__carouselItemInfo span:nth-child(2)")]
            ArticleDates,
            [Description(".hubAreaReviewAndNews__reviewTitle")]
            ArticleTitles,
            [Description("#inventoryTitle")]
            InventoryWidgetTitle

        }
        public enum MakeModelPage
        {
            [Description("#recallInformation a")]
            TransportCanadaLink,
            [Description("#OwnerScores a")]
            ViewAllOwnerReviews,
            [Description("#buyersGuide")]
            ModelOverview,
            [Description("#Photos")]
            PhotosSection,
            [Description("#ReviewsAndNews")]
            ReviewsNewsWidget,
            [Description("#OwnerScores")]
            OwnerScoresWidget,
            [Description("#recallInformation")]
            RecallInfoWidget,
            [Description("#Inventory")]
            InventoryWidget,
            [Description(".selectAnotherVehicle")]
            VehicleResearchSection,
            [Description(".explore__links")]
            ExploreLinks,
            [Description(".explore__link")]
            ExploreLink,
            [Description(".notH1")]
            OwnerReviewsPageHeader,
            [Description("#titleText")]
            SrpHeader,
            [Description(".close-button")]
            CloseCookies,

        }

        public enum StickyNavs
        {
            [Description(".hub-nav a")]
            StickyNavsCount,
            [Description("#nav-item-make")]
            ResearchmakeStickyTab,
            [Description("#nav-item-compare")]
            CompareStickyTab,
            [Description("#nav-item-suvs")]
            SUVsStickyTab,
            [Description("#nav-item-sedans")]
            SedansStickyTab,
            [Description("#nav-item-coupes")]
            CoupesStickyTab,
            [Description("#nav-item-hatchbacks")]
            HatchbacksStickyTab,
            [Description("#nav-item-trucks")]
            TrucksStickyTab,
            [Description("#nav-item-wagons")]
            WagonsStickyTab,
            [Description("#nav-item-minivans")]
            MinivansStickyTab,
            [Description("#nav-item-articles")]
            ArticlesStickyTab,
            [Description("#nav-item-latest")]
            LatestVehiclesStickyTab,
            [Description("#nav-item-SUV")]
            SUVsStickyTabMakePage,
            [Description("#nav-item-Sedan")]
            SedansStickyTabMakePage,
            [Description("#nav-item-Coupe")]
            CoupesStickyTabMakePage,
            [Description("#nav-item-Truck")]
            TrucksStickyTabMakePage,
            [Description("#nav-item-Hatchback")]
            HatchbacksStickyTabMakePage,
            [Description("#nav-item-Minivan")]
            MinivansStickyTabMakePage,
            [Description("#nav-item-Convertible")]
            ConvertiblesTabMakePage,
            [Description("#nav-item-reviewsAndNews")]
            ArticlesStickyTabMakePage,
            [Description("#nav-item-inventory")]
            InventoryStickyTabMakePage,
         

        }

        public enum ComparisonPage
        {
            [Description(".compareCarsOverview__vehicleTitle")]
            FirstVehicleTitle,

        }
    }
}
