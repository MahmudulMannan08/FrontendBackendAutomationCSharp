using MarketPlaceWeb.Base;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Pages.TryBeforeYouBuy
{
    public class TryBeforeYouBuyMain : Page
    {
        TryBeforeYouBuyAbstract tbybAbstract;
        public TryBeforeYouBuyMain(IWebDriver driver, Viewport viewport, Language language)
        {
            base.driver = driver;
            base.viewport = viewport;
            base.language = language;
            switch (viewport)
            {
                case Viewport.Large:
                    tbybAbstract = new TryBeforeYouBuyLarge(driver, viewport, language);
                    break;
                case Viewport.XS:
                    tbybAbstract = new TryBeforeYouBuyXS(driver, viewport, language);
                    break;
                case Viewport.Small:
                    tbybAbstract = new TryBeforeYouBuySmall(driver, viewport, language);
                    break;
            }
        }

        public bool IsRedirectedToTbybFunnel(bool isRedirected = true)
        {
            return tbybAbstract.IsRedirectedToTbybFunnel(isRedirected);
        }

        public void EnterPersonalDetails(TbybPersonalDetails tbybPersonalDetails)
        {
            tbybAbstract.EnterPersonalDetails(tbybPersonalDetails);
        }

        public void CheckTermsConditions(bool toBeChecked = true)
        {
            tbybAbstract.CheckTermsConditions(toBeChecked);
        }

        public void CheckCommunications(bool toBeChecked = true)
        {
            tbybAbstract.CheckCommunications(toBeChecked);
        }

        public bool IsNextDeliveryDetailsBtnEnabled(bool isEnabled = true)
        {
            return tbybAbstract.IsNextDeliveryDetailsBtnEnabled(isEnabled);
        }

        public bool IsNextDepositBtnEnabled(bool isEnabled = true)
        {
            return tbybAbstract.IsNextDepositBtnEnabled(isEnabled);
        }

        public bool IsPlaceDepositBtnEnabled(bool isEnabled = true)
        {
            return tbybAbstract.IsPlaceDepositBtnEnabled(isEnabled);
        }

        public void ClickBackIconTbybFunnel()
        {
            tbybAbstract.ClickBackIconTbybFunnel();
        }

        public void ClickNextDeliveryDetailsBtn()
        {
            tbybAbstract.ClickNextDeliveryDetailsBtn();
        }

        public void ClickNextDepositBtn()
        {
            tbybAbstract.ClickNextDepositBtn();
        }

        public void ClickPlaceDepositBtn()
        {
            tbybAbstract.ClickPlaceDepositBtn();
        }

        public void ClickDepositBackBtn()
        {
            tbybAbstract.ClickDepositBackBtn();
        }

        public void ClickBackToListingBtn()
        {
            tbybAbstract.ClickBackToListingBtn();
        }

        public bool IsRedirectedToDeliveryDetails(bool isRedirected = true)
        {
            return tbybAbstract.IsRedirectedToDeliveryDetails(isRedirected);
        }

        public bool IsRedirectedToDeposit(bool isRedirected = true)
        {
            return tbybAbstract.IsRedirectedToDeposit(isRedirected);
        }

        public bool IsRedirectedToCongratulations(bool isRedirected = true)
        {
            return tbybAbstract.IsRedirectedToCongratulations(isRedirected);
        }

        public bool IsProgressComplete(ProgressBarTbyb progressBarTbyb)
        {
            return tbybAbstract.IsProgressComplete(progressBarTbyb);
        }

        public void SelectTbybFlowType(TbybDeliveryDetails.TbybFlowType type)
        {
            tbybAbstract.SelectTbybFlowType(type);
        }

        public bool IsTbybFlowTypeSelected(TbybDeliveryDetails.TbybFlowType type)
        {
            return tbybAbstract.IsTbybFlowTypeSelected(type);
        }

        public void EnterDeliveryAddress(TbybDeliveryDetails tbybDeliveryDetails)
        {
            tbybAbstract.EnterDeliveryAddress(tbybDeliveryDetails);
        }

        public DeliveryPickupDayDate SelectFirstAvailableDate()
        {
            return tbybAbstract.SelectFirstAvailableDate();
        }

        public DeliveryPickupDayDate SelectLastAvailableDate()
        {
            return tbybAbstract.SelectLastAvailableDate();
        }

        public string SelectFirstAvailableTime()
        {
            return tbybAbstract.SelectFirstAvailableTime();
        }

        public string SelectLastAvailableTime()
        {
            return tbybAbstract.SelectLastAvailableTime();
        }

        public void EnterDepositDetails(TbybDeposit tbybDeposit)
        {
            tbybAbstract.EnterDepositDetails(tbybDeposit);
        }

        public string GetAddressOnCongratulations()
        {
            return tbybAbstract.GetAddressOnCongratulations();
        }

        public string GetAddressOnPickup()
        {
            return tbybAbstract.GetAddressOnPickup();
        }

        public string GetDateTimeOnCongratulations()
        {
            return tbybAbstract.GetDateTimeOnCongratulations();
        }

        public DeliveryPickupMonthYear GetDeliveryPickupMonthYear(string day, string date)
        {
            return tbybAbstract.GetDeliveryPickupMonthYear(day, date);
        }
    }
}
