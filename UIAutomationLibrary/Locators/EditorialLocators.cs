using System;
using System.ComponentModel;

namespace UIAutomationLibrary.Locators
{
    public class EditorialLocators
    {
        internal class DescriptionFrenchAttribute : Attribute
        {
            public string DescriptionFrench { get; set; }

            public DescriptionFrenchAttribute(string value)
            {
                DescriptionFrench = value;
            }
        }
        public enum SecondaryNav
        {
            [Description("All Expert Reviews & Advice"), DescriptionFrench("Tous les avis d'experts et conseils")]
            EditorialLink,
            [Description("#home_menu")]
            EditorialHomeLink,
            [Description("#advice_menu")]
            AdviceLink,
            [Description("#expert_menu")]
            ExpertReviewsLink,
            [Description("#news_menu")]
            Newslink,
            [Description("#cool_menu")]
            CoolStuffLink,
            [Description("#video_menu")]
            VideosLink,
            [Description("#searchValue")]
            SearchLink,
        }
        public enum EditorialWidgets
        {
            [Description("div.hero-image")]
            HeroArticle,
            [Description("#module-b-1 div.primary")]
            ReviewsMainArticle,
            [Description("#module-a-1 div.primary"), DescriptionFrench("#module-b-1  div.primary")]
            CoolStuffMainArticle,
            [Description("#module-b-0 div.primary")]
            ComparisonPageMainArticle,
            [Description("#module-b-1 div.primary")]
            ExpertReviewsMainArticle,
            [Description("#secondary-header-menu .router-link-active")]
            SecondaryNavActiveLink,
            [Description("#secondary-header-menu")]
            SecondaryHeaderMenu,
            [Description("#tertiary-header-menu .active")]
            TertiaryHeaderMenu,
            [Description("#make-select")]
            FindReviewsMake,
            [Description("#model-select")]
            FindReviewsModel,
            [Description("#make-model-search")]
            FindReviewsSearchBtn,
            [Description("#foundResults")]
            FindReviewsResults,
            [Description(".see-all a")]
            SeeAllLinks,
            [Description("#searchLoadResults")]
            LoadMoreBtn,
            [Description("link[rel='canonical")]
            CanonicalUrl,


        }
        public enum TertiaryHeaderLinks
        {
            [Description("Expert Reviews"), DescriptionFrench("Avis d'expert")]
            ExpertReviews,
            [Description("New Car Previews"), DescriptionFrench("Avant-première")]
            NewCarPreviews,
            [Description("Car Comparisons"), DescriptionFrench("Comparaison")]
            CarComparisons,
            [Description("Used Car Reviews"), DescriptionFrench("Essai routier - usagé")]
            UsedCarReviews,
            [Description("Car Product Reviews"), DescriptionFrench("Évaluation d'accessoire")]
            CarProductReviews,
            [Description("Fun Stuff"), DescriptionFrench("Insolite")]
            FunStuff,
            [Description("Car Tech"), DescriptionFrench("Technologie automobile")]
            CarTech,
            [Description("Pop Culture"), DescriptionFrench("Culture Pop")]
            PopCulture,
            [Description("Adventure"), DescriptionFrench("Aventure")]
            Adventure,
            [Description("Opinions"), DescriptionFrench("Opinions")]
            Opinions,
            [Description("Car Selling Tips"), DescriptionFrench("Astuces pour vendre")]
            CarSellingTips,
            [Description("Car Buying Tips"), DescriptionFrench("Astuces pour acheter")]
            CarBuyingTips,
            [Description("Owners Tips"), DescriptionFrench("Astuces d'entretien")]
            OwnersTips,
        }
        public enum ArticlePage
        {
            [Description("div.module-article-summary")]
            ArticleSummary,
            [Description(".hero-image-video")]
            FeaturedImage,
            [Description("div.hero-header  h1")]
            ArticleH1,
            [Description(".category_tag")]
            ArticleCategoryTag,
            [Description("div.author-date  .author")]
            AuthorName,
            [Description(" div.author-date .datetime")]
            PublishedDate,
            [Description(".module-pros-cons")]
            ProsCons,
            [Description(".module-trader-score")]
            AutoTraderScores,
            [Description(".module-related-articles")]
            RelatedArticle,
            [Description("div.module-social-icons")]
            SocialIcons,
            [Description(".competitors")]
            Competitors,
            [Description(".specification")]
            Specifications,
            [Description(".author-bio-section")]
            AuthorWidget,
            [Description(".module-you-may-like")]
            YouMayAlsoLikeWidget,
            [Description("div.module-specification  h2")]
            ComparisonData

        }
        public enum AwardsPage
        {
            [Description("#awards-finalists-title")]
            AwardsPageH1Title,
        }
        public enum AutotraderTV
        {
            [Description("#btn_watchnow")]
            WatchNowBtn,
            [Description(".hubAreaVideoWidget_title")]
            HeroVideoTitle,
            [Description(".hubAreaVideoPlayWidget_title")]
            VideoPageH1,
            [Description(".heading-see-all")]
            SeeALLCollectionLink,
            [Description("div.heading > h1")]
            CollectionPageH1,
            [Description("#carousel1 img")]
            BuyingVehiclePlaylistLink,
            [Description("div.heading > h1")]
            PlaylistPageH1,
            [Description("#carousel1 .card-title")]
             BuyingAVehicleTitle,   
        }
       
    }
}
