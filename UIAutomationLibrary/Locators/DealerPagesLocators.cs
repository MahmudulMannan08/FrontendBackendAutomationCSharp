using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIAutomationLibrary.Locators
{
    public class DealerPagesLocators
    {
        internal class DescriptionFrenchAttribute : Attribute
        {
            public string DescriptionFrench { get; set; }

            public DescriptionFrenchAttribute(string value)
            {
                DescriptionFrench = value;
            }
        }
        public enum Dealer
        {
            
            [Description(".dealer-award-badge")] 
            BestPricedDealerLogo,
            [Description("div.hero-second-div > div > a.reviews")]
            GoogleReviews,
            [Description(".visit-site")]
            DealerVistWebsite,
            [Description("div[class='service-container'])[2]")]
            ServiceOfferings,
            [Description("div.about-header-lg")]
            ExploreRemoteServices,
            [Description(".mapview")]
            VisitTheDealership,
            [Description(".review-widget")]
            ReviewsNews,
            [Description(".atv-section")]
            AutoTraderTV,
            [Description(".main-cars-for-sale")]
            Inventory,
            [Description("#contactForm")]
            LeadForm,
            [Description("div.claim-dealership")]
            DealerEdit,
            [Description("div.claim-dealership a")]
            DealerEditLink,
            [Description(".playIcon")]
            YTPlayButton,
            [Description("#videoImage")]
            YTPlayButtonHiddden,
            [Description(".hero-image")]
            MainReview,
            [Description(".cars-for-sale-big .title")]
            InventoryListing,
            [Description("h1")]
            H1Tag,
            [Description("div.communication a")]
            TermsOfUseLink,
            [Description("div.phone-location-div-lg h1")]
            NonDealerH1,
            [Description("div.marker-img-div > img")]
            NonDealerInventoryListings,
            [Description("div.inventory-div-lg")]
            NonDealerViewInventoryLink,
           [Description(".claimthispage-button")]
            FindOutHowLink




        }
    }
}
