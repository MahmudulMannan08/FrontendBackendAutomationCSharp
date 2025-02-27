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
    public class TryBeforeYouBuyXS : TryBeforeYouBuyAbstract
    {
        public TryBeforeYouBuyXS(IWebDriver driver, Viewport viewport, Language language) : base(driver, viewport, language) { }

        public override bool IsNextDeliveryDetailsBtnEnabled(bool isEnabled = true)
        {
            if (!isEnabled)
            {
                return !IsElementEnabled(FindElement(By.CssSelector(TryBeforeYouBuyLocators.XSLocators.NextDeliveryDetailsBtn.GetAttribute<DescriptionAttribute>().Description)));
            }
            return IsElementEnabled(FindElement(By.CssSelector(TryBeforeYouBuyLocators.XSLocators.NextDeliveryDetailsBtn.GetAttribute<DescriptionAttribute>().Description)));
        }

        public override bool IsNextDepositBtnEnabled(bool isEnabled = true)
        {
            if (!isEnabled)
            {
                return !IsElementEnabled(FindElement(By.CssSelector(TryBeforeYouBuyLocators.XSLocators.NextDepositBtn.GetAttribute<DescriptionAttribute>().Description)));
            }
            return IsElementEnabled(FindElement(By.CssSelector(TryBeforeYouBuyLocators.XSLocators.NextDepositBtn.GetAttribute<DescriptionAttribute>().Description)));
        }

        public override bool IsPlaceDepositBtnEnabled(bool isEnabled = true)
        {
            if (!isEnabled)
            {
                return !IsElementEnabled(FindElement(By.CssSelector(TryBeforeYouBuyLocators.XSLocators.PlaceDepositBtn.GetAttribute<DescriptionAttribute>().Description)));
            }
            return IsElementEnabled(FindElement(By.CssSelector(TryBeforeYouBuyLocators.XSLocators.PlaceDepositBtn.GetAttribute<DescriptionAttribute>().Description)));
        }

        public override void ClickNextDeliveryDetailsBtn()
        {
            ClickElement(FindElement(By.CssSelector(TryBeforeYouBuyLocators.XSLocators.NextDeliveryDetailsBtn.GetAttribute<DescriptionAttribute>().Description)));
        }

        public override void ClickNextDepositBtn()
        {
            ClickElement(FindElement(By.CssSelector(TryBeforeYouBuyLocators.XSLocators.NextDepositBtn.GetAttribute<DescriptionAttribute>().Description)));
        }

        public override void ClickPlaceDepositBtn()
        {
            ClickElement(FindElement(By.CssSelector(TryBeforeYouBuyLocators.XSLocators.PlaceDepositBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForElementNotVisible(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.LoadElement.GetAttribute<DescriptionAttribute>().Description), 60);
            WaitForPageLoad(60);
        }

        public override void ClickDepositBackBtn()
        {
            ClickElement(FindElement(By.CssSelector(TryBeforeYouBuyLocators.XSLocators.DepositBackBtn.GetAttribute<DescriptionAttribute>().Description)));
        }

        public override DeliveryPickupDayDate SelectFirstAvailableDate()
        {
            var dates = FindElements(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.DeliveryPickupDates.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(dates.FirstOrDefault());
            for (int i = 0; i < dates.Count; i++)
            {
                try
                {
                    if (!dates[i].Enabled) { continue; }
                    ClickElement(dates[i]);
                    var elements = dates[i].FindElements(By.CssSelector("p"));
                    return new DeliveryPickupDayDate
                    {
                        Day = GetElementText(elements[0]),
                        Date = GetElementText(elements[1])
                    };
                }
                catch (Exception)
                {
                    SwipeLeft(dates[i], dates[i - 1]);
                    if (!dates[i].Enabled) { continue; }
                    ClickElement(dates[i]);
                    var elements = dates[i].FindElements(By.CssSelector("p"));
                    return new DeliveryPickupDayDate
                    {
                        Day = GetElementText(elements[0]),
                        Date = GetElementText(elements[1])
                    };
                }
            }
            return null;
        }

        public override DeliveryPickupDayDate SelectLastAvailableDate()
        {
            var dates = FindElements(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.DeliveryPickupDates.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(dates.FirstOrDefault());
            for (int i = dates.Count - 1; i >= 0; i--)
            {
                try
                {
                    if (!dates[i].Enabled) { continue; }
                    ClickElement(dates[i]);
                    var elements = dates[i].FindElements(By.CssSelector("p"));
                    return new DeliveryPickupDayDate
                    {
                        Day = GetElementText(elements[0]),
                        Date = GetElementText(elements[1])
                    };
                }
                catch (Exception)
                {
                    SwipeLeft(dates[i], i + 1 <= dates.Count - 1 ? dates[i + 1] : dates[i]);  //Swiping right
                    if (!dates[i].Enabled) { continue; }
                    ClickElement(dates[i]);
                    var elements = dates[i].FindElements(By.CssSelector("p"));
                    return new DeliveryPickupDayDate
                    {
                        Day = GetElementText(elements[0]),
                        Date = GetElementText(elements[1])
                    };
                }
            }
            return null;
        }

        public override void EnterDeliveryAddress(TbybDeliveryDetails tbybDeliveryDetails)
        {
            ScrollTo(FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.DeliveryStreetTxt.GetAttribute<DescriptionAttribute>().Description)));
            EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.DeliveryStreetTxt.GetAttribute<DescriptionAttribute>().Description), tbybDeliveryDetails.StreetAddress);

            ScrollTo(FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.DeliveryUnitNoTxt.GetAttribute<DescriptionAttribute>().Description)));
            EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.DeliveryUnitNoTxt.GetAttribute<DescriptionAttribute>().Description), tbybDeliveryDetails.UnitNumber);

            ScrollTo(FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.DeliveryCityTxt.GetAttribute<DescriptionAttribute>().Description)));
            EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.DeliveryCityTxt.GetAttribute<DescriptionAttribute>().Description), tbybDeliveryDetails.City);
            
            EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.DeliveryPostalCodeTxt.GetAttribute<DescriptionAttribute>().Description), tbybDeliveryDetails.PostalCode);
        }

        public override void EnterDepositDetails(TbybDeposit tbybDeposit)
        {
            InteractOnIframe(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.CCNumberTxt.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                            () => EnterTextWithDelay(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.CCNumberTxt.GetAttribute<DescriptionAttribute>().Description), tbybDeposit.CCNumber));
            
            EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.CCNameTxt.GetAttribute<DescriptionAttribute>().Description), tbybDeposit.CCName);

            InteractOnIframe(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.CCExpiryTxt.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                            () => ScrollTo(FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.CCExpiryTxt.GetAttribute<DescriptionAttribute>().Description))));
            InteractOnIframe(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.CCExpiryTxt.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                            () => EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.CCExpiryTxt.GetAttribute<DescriptionAttribute>().Description), tbybDeposit.CCExpiry));

            InteractOnIframe(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.CVVTxt.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                            () => EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.CVVTxt.GetAttribute<DescriptionAttribute>().Description), tbybDeposit.CVV));

            ScrollTo(FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.CCPostalCodeTxt.GetAttribute<DescriptionAttribute>().Description)));
            EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.CCPostalCodeTxt.GetAttribute<DescriptionAttribute>().Description), tbybDeposit.PostalCode);
        }
    }
}
