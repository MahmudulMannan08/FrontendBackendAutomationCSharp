using System.ComponentModel;

namespace MarketPlaceWeb.Locators
{
    public class CommonLocators
    {
        public enum CookieBanner
        {
            [Description("#cookie-banner .close-button")]
            closeBtn
        }

        public enum SplashPage
        {
            [Description("#splash")]
            splashPage
        }

        public enum ErrorPage
        {
            [Description("div.image500")]
            ErrorPage500
        }

        public enum SurveyCampaignModal
        {
            [Description("iframe[title='Usabilla Feedback Form']")]
            SurveyIframe,
            [Description("#close")]
            CloseBtn,
            [Description(".container #poll")]
            SurveyForm
        }
    }
}
