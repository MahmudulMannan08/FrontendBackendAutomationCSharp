using MarketPlaceWeb.Base;
using OpenQA.Selenium;

namespace UIAutomationLibrary.Pages.InstantCashOffer
{
    public class InstantCashOfferMain : Page
    {
        InstantCashOfferAbstract icoAbstract;

        public InstantCashOfferMain(IWebDriver driver, Viewport viewport, Language language)
        {
            base.driver = driver;
            base.viewport = viewport;
            base.language = language;
            switch (viewport)
            {
                case Viewport.Large:
                    icoAbstract = new InstantCashOfferLarge(driver, viewport, language);
                    break;
                case Viewport.XS:
                    icoAbstract = new InstantCashOfferXS(driver, viewport, language);
                    break;
                case Viewport.Small:
                    icoAbstract = new InstantCashOfferSmall(driver, viewport, language);
                    break;
            }
        }
        
        public bool IsIcoWidgetAvailable()
        {
            return icoAbstract.IsIcoWidgetAvailable();
        }

        public bool IsDWWIcoOfferAvailable()
        {
            return icoAbstract.IsDWWIcoOfferAvailable();
        }

        public void ClickIcoButton(IcoLeadForm icoLeadForm)
        {
            icoAbstract.ClickInstantCashOfferButton(icoLeadForm);
        }

        public void ClickNextBtnOnIcoLead(By locator, string locatorValue = null, bool scrollIntoView = false, int timeOut = 30)
        {
            icoAbstract.ClickNextBtnOnIcoLead(locator, locatorValue , scrollIntoView, timeOut);
        }

        public void EnterYmmtDetails(IcoLeadForm icoLeadForm)
        {
            icoAbstract.EnterYmmtDetails(icoLeadForm);
        }

        public void EnterVinDetails(IcoLeadForm icoLeadForm)
        {
            icoAbstract.EnterVinDetails(icoLeadForm);
        }        

        public void EnterIncorrectVinDetails()
        {
            icoAbstract.EnterIncorrectVinDetails();
        }
        public void ClickEnterYourVehicleMakeAndModelBtn()
        {
            icoAbstract.ClickEnterYourVehicleMakeAndModelBtn();
        }

        public void EnterVehicleDetails(IcoLeadForm icoLeadForm)
        {
            icoAbstract.EnterVehicleDetails(icoLeadForm);
        }
        public bool IsHighMileageOldVehiclePageDisplayed()
        {
            return icoAbstract.IsHighMileageOldVehiclePageDisplayed();
        }

        public bool VerifyElementsOnHighMileageOldVehiclePage()
        {
            return icoAbstract.VerifyElementsOnHighMileageOldVehiclePage();
        }

        public void EnterVehicleConditions(IcoLeadForm icoLeadForm)
        {
            icoAbstract.EnterVehicleConditions(icoLeadForm);
        }

        public void EnterCustomerDetails(IcoLeadForm icoLeadForm)
        {
            icoAbstract.EnterCustomerDetails(icoLeadForm);
        }
        public void EnterCustomerDetailsDWW(IcoLeadForm icoLeadForm)
        {
            icoAbstract.EnterCustomerDetailsDWW(icoLeadForm);
        }

        public void EnterVinDetailsDWW(IcoLeadForm icoLeadForm)
        {
            icoAbstract.EnterVinDetailsDWW(icoLeadForm);
        }

        public void EnterVehicleDetailsDWW(IcoLeadForm icoLeadForm)
        {
            icoAbstract.EnterVehicleDetailsDWW(icoLeadForm);
        }

        public void EnterVehicleConditionsDWW(IcoLeadForm icoLeadForm)
        {
            icoAbstract.EnterVehicleConditionsDWW(icoLeadForm);        }


        public void SubmitPhoneCodeVerificationByText(IcoLeadForm icoLeadForm)
        {
            icoAbstract.SubmitPhoneCodeVerificationByText(icoLeadForm);
        }

        public bool IsIcoSuccessPageDisplayed()
        {
            return icoAbstract.IsIcoSuccessPageDisplayed();
        }

        public int GetParticipatingDealerCount()
        {
            return icoAbstract.GetParticipatingDealerCount();
        }

        public void CloseIcoSuccessModal()
        {
            icoAbstract.CloseIcoSuccessModal();
        }

        public bool IsIcoModalClosed()
        {
            return icoAbstract.IsIcoModalClosed();
        }

        public bool IsIcoOfferDisplayedOnVdp()
        {
            return icoAbstract.IsIcoOfferDisplayedOnVdp();
        }

        public bool IsIcoWidgetButtonDisplayed(bool isDisplayed, string buttonText = null)
        {
            return icoAbstract.IsIcoWidgetButtonDisplayed(isDisplayed, buttonText);
        }       

    }
}
