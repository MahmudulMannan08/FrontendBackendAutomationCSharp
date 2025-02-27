using MarketPlaceWeb.Base;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIAutomationLibrary.Pages.Editorials;

namespace UIAutomationLibrary.Pages.Dealers
{
    public class DealersSmall : DealersAbstract
    {
        public DealersSmall(IWebDriver driver, Viewport viewport, Language language) : base(driver, viewport, language) { }
    }
}
