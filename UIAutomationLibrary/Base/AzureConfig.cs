using MarketPlaceWeb.Driver;
using OpenQA.Selenium;
using System;

namespace MarketPlaceWeb.Base
{
    public class AzureConfig
    {
        public Language language;
        public Uri uri;
        public string config;
        public bool isAzureEnabled = false, isHomeDeliveryToggleEnabled = false, isVinRequiredToggle = false, isDealerInfoBrandingVideoToggle = false;
        public string srpVariant, boostVariant, hpVariant;

        public AzureConfig()
        {
            isAzureEnabled = (Environment.GetEnvironmentVariable("azureconfig") == "enabled") ? true : false;
            if (isAzureEnabled)
            {
                uri = new Uri(Environment.GetEnvironmentVariable("url"));
                config = Environment.GetEnvironmentVariable("config");
                language = (uri.ToString().Contains("autotrader.ca")) ? Language.EN : (uri.ToString().Contains("autohebdo.net")) ? Language.FR : throw new Exception("Invalid URL " + uri.ToString());
                srpVariant = Environment.GetEnvironmentVariable("srp_variant");
                boostVariant = Environment.GetEnvironmentVariable("boost_variant");
                hpVariant = Environment.GetEnvironmentVariable("hp_control_variant");
                isHomeDeliveryToggleEnabled = Convert.ToBoolean(Environment.GetEnvironmentVariable("home_delivery_toggle"));
                isVinRequiredToggle = Convert.ToBoolean(Environment.GetEnvironmentVariable("vin_required_feature"));
                isDealerInfoBrandingVideoToggle = Convert.ToBoolean(Environment.GetEnvironmentVariable("dealer_info_branding_video_feature"));
            }
        }

        public Viewport GetViewport()
        {
            string configViewport = config.Split('-')[2];
            switch (configViewport.ToLower().Trim())
            {
                case "large":
                    return Viewport.Large;
                case "medium":
                    return Viewport.Medium;
                case "small":
                    return Viewport.Small;
                case "xs":
                    return Viewport.XS;
                default:
                    throw new Exception("Invalid Viewport.");
            }
        }

        public IWebDriver GetDriverFromAzure()
        {
            BrowserstackManager browserstackManager = new BrowserstackManager();
            switch (config)
            {
                case "win-chrome-large":
                    return browserstackManager.WinChromeLarge_BrowserStack();
                case "win-firefox-large":
                    return browserstackManager.WinFirefoxLarge_BrowserStack();
                case "win-edge-large":
                    return browserstackManager.WinEdgeLarge_BrowserStack();
                case "mac-safari-large":
                    return browserstackManager.MacSafariLarge_BrowserStack();
                case "mac-chrome-large":
                    return browserstackManager.MacChromeLarge_BrowserStack();
                case "ipad-safari-small":
                    return browserstackManager.IpadSafariSmallBrowserStack();
                case "ios-safari-xs":
                    return browserstackManager.IOSSafariXS();
                case "android-chrome-xs":
                    return browserstackManager.AndroidChromeXS();
                default:
                    throw new Exception("Driver is not set.");
            }
        }
    }
}