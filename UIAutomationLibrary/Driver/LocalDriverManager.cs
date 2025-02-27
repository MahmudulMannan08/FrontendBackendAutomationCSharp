using MarketPlaceWeb.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace MarketPlaceWeb.Driver
{
    public class LocalDriverManager
    {
        private IWebDriver CreateChromeDriver()
        {
            LocalConfig localConfig = new LocalConfig();
            ChromeOptions options = new ChromeOptions();
            Viewport viewport = localConfig.GetViewport();
            switch (viewport)
            {
                case (Viewport.XS):
                    {
                        options.EnableMobileEmulation("Galaxy S5");
                        break;
                    }
                case (Viewport.Small):
                    {
                        options.EnableMobileEmulation("iPad");
                        break;
                    }
                case (Viewport.Large):
                    {
                        options.AddArgument("--start-maximized");
                        break;
                    }

            }
            ChromeDriver driver = new ChromeDriver(options);
            return driver;
        }
        public IWebDriver ChromeLarge()
        {
            IWebDriver driver = CreateChromeDriver();
            return driver;
        }
    }
}