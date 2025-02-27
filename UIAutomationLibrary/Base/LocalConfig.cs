using MarketPlaceWeb.Driver;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;

namespace MarketPlaceWeb.Base
{
    public class LocalConfig
    {
        private readonly object EnvironmentSettings;
        public Uri uri { get; }
        public string config { get; }
        public Viewport viewport;
        public Language language;

        public LocalConfig()
        {
            var dir = Path.GetDirectoryName(typeof(LocalConfig).Assembly.Location) + @"\Configs";
            var envSettingsFile = Path.Combine(dir, $"testconfig.json");
            if (!File.Exists(envSettingsFile)) throw new Exception($"Unable to locate {envSettingsFile}");
            EnvironmentSettings = JsonConvert.DeserializeObject<ExpandoObject>(File.ReadAllText(envSettingsFile));
            config = GetSetting<string>("Configuration");
            if (string.IsNullOrEmpty(config)) config = "chrome-large";
            uri = new Uri(GetSetting<string>("TraderUrl"));
            language = (uri.ToString().Contains("autotrader.ca")) ? Language.EN : (uri.ToString().Contains("autohebdo.net")) ? Language.FR : throw new Exception("Invalid URL " + uri.ToString());
        }

        private T GetSetting<T>(string key)
        {
            object value = EnvironmentSettings;
            var path = key.Split('.');
            foreach (var propName in path) value = ((IDictionary<string, object>)value)[propName];
            return (T)Convert.ChangeType(value, typeof(T));
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

        public IWebDriver GetDriverFromLocal()
        {
            BrowserstackManager browserstackManager = new BrowserstackManager();
            LocalDriverManager chromeDriverManager = new LocalDriverManager();
            switch (config.ToLower())
            {
                case "local-chrome-large":
                    return chromeDriverManager.ChromeLarge();
                case "local-chrome-xs":
                    return chromeDriverManager.ChromeLarge();
                case "local-chrome-small":
                    return chromeDriverManager.ChromeLarge();
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