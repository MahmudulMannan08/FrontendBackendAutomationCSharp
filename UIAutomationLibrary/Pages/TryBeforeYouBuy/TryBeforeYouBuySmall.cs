using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Pages.TryBeforeYouBuy
{
    public class TryBeforeYouBuySmall : TryBeforeYouBuyAbstract
    {
        public TryBeforeYouBuySmall(IWebDriver driver, Viewport viewport, Language language) : base(driver, viewport, language) { }

        public override bool IsNextDeliveryDetailsBtnEnabled(bool isEnabled = true)
        {
            if (!isEnabled)
            {
                return !IsElementEnabled(FindElement(By.CssSelector(TryBeforeYouBuyLocators.SmallLocators.NextDeliveryDetailsBtn.GetAttribute<DescriptionAttribute>().Description)));
            }
            return IsElementEnabled(FindElement(By.CssSelector(TryBeforeYouBuyLocators.SmallLocators.NextDeliveryDetailsBtn.GetAttribute<DescriptionAttribute>().Description)));
        }

        public override bool IsNextDepositBtnEnabled(bool isEnabled = true)
        {
            if (!isEnabled)
            {
                return !IsElementEnabled(FindElement(By.CssSelector(TryBeforeYouBuyLocators.SmallLocators.NextDepositBtn.GetAttribute<DescriptionAttribute>().Description)));
            }
            return IsElementEnabled(FindElement(By.CssSelector(TryBeforeYouBuyLocators.SmallLocators.NextDepositBtn.GetAttribute<DescriptionAttribute>().Description)));
        }

        public override bool IsPlaceDepositBtnEnabled(bool isEnabled = true)
        {
            if (!isEnabled)
            {
                return !IsElementEnabled(FindElement(By.CssSelector(TryBeforeYouBuyLocators.SmallLocators.PlaceDepositBtn.GetAttribute<DescriptionAttribute>().Description)));
            }
            return IsElementEnabled(FindElement(By.CssSelector(TryBeforeYouBuyLocators.SmallLocators.PlaceDepositBtn.GetAttribute<DescriptionAttribute>().Description)));
        }

        public override void ClickNextDeliveryDetailsBtn()
        {
            ClickElement(FindElement(By.CssSelector(TryBeforeYouBuyLocators.SmallLocators.NextDeliveryDetailsBtn.GetAttribute<DescriptionAttribute>().Description)));
        }

        public override void ClickNextDepositBtn()
        {
            ClickElement(FindElement(By.CssSelector(TryBeforeYouBuyLocators.SmallLocators.NextDepositBtn.GetAttribute<DescriptionAttribute>().Description)));
        }

        public override void ClickPlaceDepositBtn()
        {
            ClickElement(FindElement(By.CssSelector(TryBeforeYouBuyLocators.SmallLocators.PlaceDepositBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForElementNotVisible(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.LoadElement.GetAttribute<DescriptionAttribute>().Description), 60);
            WaitForPageLoad(60);
        }

        public override void ClickDepositBackBtn()
        {
            ClickElement(FindElement(By.CssSelector(TryBeforeYouBuyLocators.SmallLocators.DepositBackBtn.GetAttribute<DescriptionAttribute>().Description)));
        }

        public override DeliveryPickupDayDate SelectFirstAvailableDate()
        {
            var dates = FindElements(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.DeliveryPickupDates.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(dates.FirstOrDefault());

            IWebElement element = dates.FirstOrDefault(x => x.Enabled);
            ClickElement(element);

            var elements = element.FindElements(By.CssSelector("p"));
            return new DeliveryPickupDayDate
            {
                Day = GetElementText(elements[0]),
                Date = GetElementText(elements[1])
            };
        }

        public override DeliveryPickupDayDate SelectLastAvailableDate()
        {
            var dates = FindElements(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.DeliveryPickupDates.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(dates.FirstOrDefault());

            IWebElement element = dates.LastOrDefault(x => x.Enabled);
            ClickElement(element);

            var elements = element.FindElements(By.CssSelector("p"));
            return new DeliveryPickupDayDate
            {
                Day = GetElementText(elements[0]),
                Date = GetElementText(elements[1])
            };
        }
    }
}
