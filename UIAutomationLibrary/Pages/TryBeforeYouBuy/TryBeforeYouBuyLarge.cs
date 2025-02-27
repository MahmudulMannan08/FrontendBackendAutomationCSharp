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
    public class TryBeforeYouBuyLarge : TryBeforeYouBuyAbstract
    {
        public TryBeforeYouBuyLarge(IWebDriver driver, Viewport viewport, Language language) : base(driver, viewport, language) { }

        public override bool IsRedirectedToTbybFunnel(bool isRedirected = true)
        {
            if (!isRedirected)
            {
                return !IsElementVisible(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.FirstNameTxt.GetAttribute<DescriptionAttribute>().Description));
            }
            return IsElementVisible(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.FirstNameTxt.GetAttribute<DescriptionAttribute>().Description));
        }

        public override void EnterPersonalDetails(TbybPersonalDetails tbybPersonalDetails)
        {
            EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.FirstNameTxt.GetAttribute<DescriptionAttribute>().Description), tbybPersonalDetails.FirstName);
            EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.LastNameTxt.GetAttribute<DescriptionAttribute>().Description), tbybPersonalDetails.LastName);
            EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.EmailAddressTxt.GetAttribute<DescriptionAttribute>().Description), tbybPersonalDetails.EmailAddress);
            EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.PhoneNumberTxt.GetAttribute<DescriptionAttribute>().Description), tbybPersonalDetails.PhoneNumber);
            EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.ReEnterPhoneTxt.GetAttribute<DescriptionAttribute>().Description), tbybPersonalDetails.PhoneNumber);
        }

        public override bool IsNextDeliveryDetailsBtnEnabled(bool isEnabled = true)
        {
            if (!isEnabled)
            {
                return !IsElementEnabled(FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.NextDeliveryDetailsBtn.GetAttribute<DescriptionAttribute>().Description)));
            }
            return IsElementEnabled(FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.NextDeliveryDetailsBtn.GetAttribute<DescriptionAttribute>().Description)));
        }

        public override bool IsNextDepositBtnEnabled(bool isEnabled = true)
        {
            if (!isEnabled)
            {
                return !IsElementEnabled(FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.NextDepositBtn.GetAttribute<DescriptionAttribute>().Description)));
            }
            return IsElementEnabled(FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.NextDepositBtn.GetAttribute<DescriptionAttribute>().Description)));
        }

        public override bool IsPlaceDepositBtnEnabled(bool isEnabled = true)
        {
            if (!isEnabled)
            {
                return !IsElementEnabled(FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.PlaceDepositBtn.GetAttribute<DescriptionAttribute>().Description)));
            }
            return IsElementEnabled(FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.PlaceDepositBtn.GetAttribute<DescriptionAttribute>().Description)));
        }

        public override void ClickNextDeliveryDetailsBtn()
        {
            ClickElement(FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.NextDeliveryDetailsBtn.GetAttribute<DescriptionAttribute>().Description)));
        }

        public override void ClickNextDepositBtn()
        {
            ClickElement(FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.NextDepositBtn.GetAttribute<DescriptionAttribute>().Description)));
        }

        public override void ClickPlaceDepositBtn()
        {
            ClickElement(FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.PlaceDepositBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForElementNotVisible(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.LoadElement.GetAttribute<DescriptionAttribute>().Description), 30);
            WaitForPageLoad(60);
        }

        public override void ClickDepositBackBtn()
        {
            ClickElement(FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.DepositBackBtn.GetAttribute<DescriptionAttribute>().Description)));
        }

        public override DeliveryPickupDayDate SelectFirstAvailableDate()
        {
            IWebElement element = FindElements(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.DeliveryPickupDates.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault(x => x.Enabled);
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
            IWebElement element = FindElements(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.DeliveryPickupDates.GetAttribute<DescriptionAttribute>().Description)).LastOrDefault(x => x.Enabled);
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
