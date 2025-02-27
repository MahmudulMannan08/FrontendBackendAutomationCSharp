using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using MarketPlaceWeb.Pages.MyGarage;
using MarketPlaceWeb.Pages.VDP;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace UIAutomationLibrary.Pages.PreQual
{
    public class PreQualMain : Page
    {
        PrequalAbstract preQual;
        private const string _defaultValue = null;

        public PreQualMain(IWebDriver driver, Viewport viewport, Language language)
        {
            base.driver = driver;
            base.viewport = viewport;
            base.language = language;
            switch (viewport)
            {
                case Viewport.Large:
                    preQual = new PreQualLarge(driver, viewport, language);
                    break;
                case Viewport.XS:
                    preQual = new PreQualXS(driver, viewport, language);
                    break;
                case Viewport.Small:
                    preQual = new PreQualSmall(driver, viewport, language);
                    break;
            }
        }

        public void ClickWhatCanIaffordFromLandingPage() => preQual.ClickWhatCanIaffordFromLandingPage();

        public bool VerifyPrequalModalLanding() => preQual.VerifyPrequalModalLanding();

        public void ClickContinueOnPreQualModalLanding() => preQual.ClickContinueOnPreQualModalLanding();

        public virtual void CompletePreQualStep1(PersonalInformation personalInfo, bool isExistingData = false) => preQual.CompletePreQualStep1(personalInfo, isExistingData);

        public void CompletePreQualStep2(AddressInformation addressInfo, bool isExistingData = false) => preQual.CompletePreQualStep2(addressInfo, isExistingData);

        public void CompletePreQualIDVStep3() => preQual.CompletePreQualIDVStep3();

        public bool VerifyPreQualResults(PreQualLocators.PreQual resultType) => preQual.VerifyPreQualResults(resultType);

        public bool IsExpiryDateValid(PreQualLocators.PreQual resultType) => preQual.IsExpiryDateValid(resultType);

        public void ClickShopVehicleLink(PreQualLocators.PreQual resultType) => preQual.ClickShopVehicleLink(resultType);

        public string GetPreQualMaxapprovedAmount() => preQual.GetPreQualMaxapprovedAmount();

        public void ClickGetStartedButton() => preQual.ClickGetStartedButton();

        public bool IsStep1ModalVisible() => preQual.IsStep1ModalVisible();

        public void ClickBrowseVehiclesButton() => preQual.ClickBrowseVehiclesButton();


        public bool VerifyPrequalLandingPage() => preQual.VerifyPrequalLandingPage();

        public bool? IsPreQualModalLandingVisible() => preQual.IsPreQualModalLandingVisible();

        public void ClickFindOutYourAffordability() => preQual.ClickFindOutYourAffordability();

        public void ClickPreQualModalLandingCloseBtn() => preQual.ClickPreQualModalLandingCloseBtn();

        public void CompletePreQualStep3InvalidKBA() => preQual.CompletePreQualStep3InvalidKBA();

        public void ClickTryAgainButton() => preQual.ClickTryAgainButton();

        public void ClickPreQualDeclineShopAllButton(PreQualLocators.PreQual resultType) => preQual.ClickPreQualDeclineShopAllButton(resultType);

        public void WaitforPreQualModal(bool IsPreQualAlreadyDone = false) => preQual.WaitforPreQualModal(IsPreQualAlreadyDone);
       
    }
}