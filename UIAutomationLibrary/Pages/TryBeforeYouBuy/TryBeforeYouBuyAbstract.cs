using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Pages.TryBeforeYouBuy
{
    public abstract class TryBeforeYouBuyAbstract : Page
    {
        protected TryBeforeYouBuyAbstract(IWebDriver driver, Viewport viewport, Language language)
        {
            base.driver = driver;
            base.viewport = viewport;
            base.language = language;
        }

        public virtual bool IsRedirectedToTbybFunnel(bool isRedirected = true)
        {
            if (!isRedirected)
            {
                return !IsElementVisible(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.GetStartedBtn.GetAttribute<DescriptionAttribute>().Description));
            }
            return IsElementVisible(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.GetStartedBtn.GetAttribute<DescriptionAttribute>().Description));
        }

        public virtual void EnterPersonalDetails(TbybPersonalDetails tbybPersonalDetails)
        {
            ClickElement(FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.GetStartedBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForPageLoad(30);

            EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.FirstNameTxt.GetAttribute<DescriptionAttribute>().Description), tbybPersonalDetails.FirstName);
            EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.LastNameTxt.GetAttribute<DescriptionAttribute>().Description), tbybPersonalDetails.LastName);
            EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.EmailAddressTxt.GetAttribute<DescriptionAttribute>().Description), tbybPersonalDetails.EmailAddress);
            EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.PhoneNumberTxt.GetAttribute<DescriptionAttribute>().Description), tbybPersonalDetails.PhoneNumber);
            EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.ReEnterPhoneTxt.GetAttribute<DescriptionAttribute>().Description), tbybPersonalDetails.PhoneNumber);
        }

        public void CheckTermsConditions(bool toBeChecked = true)
        {
            IWebElement element = FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.TermsConditionsCheckbox.GetAttribute<DescriptionAttribute>().Description));
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.TermsConditionsCheckbox.GetAttribute<DescriptionAttribute>().Description + " + span")));
            }
        }

        public void CheckCommunications(bool toBeChecked = true)
        {
            IWebElement element = FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.CommunicationCheckbox.GetAttribute<DescriptionAttribute>().Description));
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.CommunicationCheckbox.GetAttribute<DescriptionAttribute>().Description + " + span")));
            }
        }

        public bool IsRedirectedToDeliveryDetails(bool isRedirected = true)
        {
            if (!isRedirected)
            {
                return !IsElementVisible(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.DeliveryTabBtn.GetAttribute<DescriptionAttribute>().Description));
            }
            try
            {
                WaitForElementVisible(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.DeliveryTabBtn.GetAttribute<DescriptionAttribute>().Description), 30);
                return true;
            }
            catch (Exception) { return false; }
        }

        public bool IsRedirectedToDeposit(bool isRedirected = true)
        {
            if (!isRedirected)
            {
                return !IsElementVisible(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.CCNameTxt.GetAttribute<DescriptionAttribute>().Description));
            }
            try
            {
                WaitForElementVisible(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.CCNameTxt.GetAttribute<DescriptionAttribute>().Description), 60);
                return true;
            }
            catch (Exception) { return false; }
        }

        public bool IsRedirectedToCongratulations(bool isRedirected = true)
        {
            if (!isRedirected)
            {
                return !IsElementVisible(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.BackToListingBtn.GetAttribute<DescriptionAttribute>().Description));
            }
            try
            {
                WaitForElementVisible(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.BackToListingBtn.GetAttribute<DescriptionAttribute>().Description), 60);
                return true;
            }
            catch (Exception) { return false; }
        }

        public void ClickBackToListingBtn()
        {
            IWebElement element = FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.BackToListingBtn.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            ClickElement(element);
            WaitForPageLoad(60);
        }

        public void ClickBackIconTbybFunnel()
        {
            ClickElement(FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.BackIcon.GetAttribute<DescriptionAttribute>().Description)));
            WaitForPageLoad(90);
        }

        public bool IsProgressComplete(ProgressBarTbyb progressBarTbyb)
        {
            var progressBarElements = FindElements(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.ProgressBars.GetAttribute<DescriptionAttribute>().Description));
            switch (progressBarTbyb)
            {
                case ProgressBarTbyb.PersonalDetailsProgress:
                    return progressBarElements[0].GetAttribute("class").Contains("complete");
                case ProgressBarTbyb.DeliveryDetailsProgress:
                    return progressBarElements[0].GetAttribute("class").Contains("complete") && progressBarElements[1].GetAttribute("class").Contains("complete");
                default:
                    return false;
            }
        }

        public void SelectTbybFlowType(TbybDeliveryDetails.TbybFlowType type)
        {
            switch (type)
            {
                case TbybDeliveryDetails.TbybFlowType.Delivery:
                    ClickElement(FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.DeliveryTabBtn.GetAttribute<DescriptionAttribute>().Description)));
                    WaitForElementVisible(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.DeliveryStreetTxt.GetAttribute<DescriptionAttribute>().Description));
                    break;
                case TbybDeliveryDetails.TbybFlowType.Pickup:
                    ClickElement(FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.PickupTabBtn.GetAttribute<DescriptionAttribute>().Description)));
                    WaitForElementVisible(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.PickupDealerMap.GetAttribute<DescriptionAttribute>().Description));
                    break;
            }
        }

        public bool IsTbybFlowTypeSelected(TbybDeliveryDetails.TbybFlowType type)
        {
            switch (type)
            {
                case TbybDeliveryDetails.TbybFlowType.Delivery:
                    return FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.DeliveryTabBtn.GetAttribute<DescriptionAttribute>().Description)).GetAttribute("class").Contains("selected");
                case TbybDeliveryDetails.TbybFlowType.Pickup:
                    return FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.PickupTabBtn.GetAttribute<DescriptionAttribute>().Description)).GetAttribute("class").Contains("selected");
                default:
                    return false;
            }
        }

        public virtual void EnterDeliveryAddress(TbybDeliveryDetails tbybDeliveryDetails)
        {
            EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.DeliveryStreetTxt.GetAttribute<DescriptionAttribute>().Description), tbybDeliveryDetails.StreetAddress);
            EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.DeliveryUnitNoTxt.GetAttribute<DescriptionAttribute>().Description), tbybDeliveryDetails.UnitNumber);
            EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.DeliveryCityTxt.GetAttribute<DescriptionAttribute>().Description), tbybDeliveryDetails.City);
            EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.DeliveryPostalCodeTxt.GetAttribute<DescriptionAttribute>().Description), tbybDeliveryDetails.PostalCode);
        }

        public string SelectFirstAvailableTime()
        {
            var times = FindElements(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.DeliveryPickupTimes.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(times.FirstOrDefault());
            IWebElement element = times.FirstOrDefault(x => x.Enabled);
            ClickElement(element);
            return GetElementText(element);
        }

        public string SelectLastAvailableTime()
        {
            var times = FindElements(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.DeliveryPickupTimes.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(times.LastOrDefault());
            IWebElement element = times.LastOrDefault(x => x.Enabled);
            ClickElement(element);
            return GetElementText(element);
        }

        public virtual void EnterDepositDetails(TbybDeposit tbybDeposit)
        {
            InteractOnIframe(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.CCNumberTxt.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                            () => EnterTextWithDelay(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.CCNumberTxt.GetAttribute<DescriptionAttribute>().Description), tbybDeposit.CCNumber));
            EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.CCNameTxt.GetAttribute<DescriptionAttribute>().Description), tbybDeposit.CCName);
            InteractOnIframe(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.CCExpiryTxt.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                            () => EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.CCExpiryTxt.GetAttribute<DescriptionAttribute>().Description), tbybDeposit.CCExpiry));
            InteractOnIframe(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.CVVTxt.GetAttribute<IframeLocatorAttribute>().IframeLocator),
                            () => EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.CVVTxt.GetAttribute<DescriptionAttribute>().Description), tbybDeposit.CVV));
            EnterText(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.CCPostalCodeTxt.GetAttribute<DescriptionAttribute>().Description), tbybDeposit.PostalCode);
        }

        public string GetAddressOnCongratulations()
        {
            return GetElementText(FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.CongratulationsAddressLbl.GetAttribute<DescriptionAttribute>().Description)));
        }

        public string GetAddressOnPickup()
        {
            IWebElement element = FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.PickupAddressLbl.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            return GetSubstringBeforeCharacter(GetElementText(element), ',');
        }

        public string GetDateTimeOnCongratulations()
        {
            return GetElementText(FindElement(By.CssSelector(TryBeforeYouBuyLocators.TbybLocators.CongratulationsDateLbl.GetAttribute<DescriptionAttribute>().Description)));
        }

        public DeliveryPickupMonthYear GetDeliveryPickupMonthYear(string day, string date)
        {
            string dayCalulated = (language.ToString() == "EN") ? DateTime.Parse(DateTime.UtcNow.Month.ToString() + "/" + date + "/" + DateTime.UtcNow.Year.ToString(), CultureInfo.InvariantCulture).ToString("ddd", new CultureInfo("en-US")) : DateTime.Parse(DateTime.UtcNow.Month.ToString() + "/" + date + "/" + DateTime.UtcNow.Year.ToString(), CultureInfo.InvariantCulture).ToString("ddd", new CultureInfo("fr-FR"));
            if (dayCalulated.ToLower().Contains(day.ToLower()))
            {
                return new DeliveryPickupMonthYear
                {
                    Month = DateTime.UtcNow.Month.ToString(),
                    Year = DateTime.UtcNow.Year.ToString()
                };
            }
            return new DeliveryPickupMonthYear
            {
                Month = DateTime.UtcNow.AddMonths(1).Month.ToString(),
                Year = DateTime.UtcNow.AddMonths(1).Month.ToString() == "1" ? DateTime.UtcNow.AddYears(1).Year.ToString() : DateTime.UtcNow.Year.ToString()  //Add a year if Month is January (new year)
            };
        }

        public abstract bool IsNextDeliveryDetailsBtnEnabled(bool isEnabled = true);
        public abstract bool IsNextDepositBtnEnabled(bool isEnabled = true);
        public abstract bool IsPlaceDepositBtnEnabled(bool isEnabled = true);
        public abstract void ClickNextDeliveryDetailsBtn();
        public abstract void ClickNextDepositBtn();
        public abstract void ClickPlaceDepositBtn();
        public abstract void ClickDepositBackBtn();
        public abstract DeliveryPickupDayDate SelectFirstAvailableDate();
        public abstract DeliveryPickupDayDate SelectLastAvailableDate();
    }

    public class TbybPersonalDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class TbybDeliveryDetails
    {
        public string StreetAddress { get; set; }
        public string UnitNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public enum TbybFlowType
        {
            Delivery,
            Pickup
        }
    }

    public class DeliveryPickupDayDate
    {
        public string Day { get; set; }
        public string Date { get; set; }
    }

    public class DeliveryPickupMonthYear
    {
        public string Month { get; set; }
        public string Year { get; set; }
    }

    public class TbybDeposit
    {
        public string CCNumber { get; set; }
        public string CCName { get; set; }
        public string CCExpiry { get; set; }
        public string CVV { get; set; }
        public string PostalCode { get; set; }
    }

    public enum ProgressBarTbyb
    {
        PersonalDetailsProgress,
        DeliveryDetailsProgress
    }
}
