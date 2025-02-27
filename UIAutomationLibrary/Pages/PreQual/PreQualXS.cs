using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using MarketPlaceWeb.Pages.MyGarage;
using MarketPlaceWeb.Pages.VDP;
using MarketPlaceWeb.Pages.HP;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.ComponentModel;

namespace UIAutomationLibrary.Pages.PreQual
{
    public class PreQualXS : PrequalAbstract
    {
        public PreQualXS(IWebDriver driver, Viewport viewport, Language language) : base(driver, viewport, language) { }


        #region PreQual

        internal override void ClickWhatCanIaffordFromLandingPage()
        {
            WaitForElementClickable(By.CssSelector(PreQualLocators.PreQual.WhatCanIAfford.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(FindElement(By.CssSelector(PreQualLocators.PreQual.WhatCanIAfford.GetAttribute<DescriptionAttribute>().Description)));
        }

        internal override string CompletePreQualStep1(PersonalInformation personalInfo, bool isExistingData = false)
        {
            if (!isExistingData)
            {
                EnterText(By.CssSelector(PreQualLocators.PreQual.FirstName.GetAttribute<DescriptionAttribute>().Description), personalInfo.FirstName);
                EnterText(By.CssSelector(PreQualLocators.PreQual.LastName.GetAttribute<DescriptionAttribute>().Description), personalInfo.LastName);
                EnterText(By.CssSelector(PreQualLocators.PreQual.DateOfBirth.GetAttribute<DescriptionAttribute>().Description), personalInfo.Dob);
                EnterText(By.CssSelector(PreQualLocators.PreQual.PhoneNumber.GetAttribute<DescriptionAttribute>().Description), personalInfo.PhoneNumber);

            }
            else
            {
                EnterText(By.CssSelector(PreQualLocators.PreQual.DateOfBirth.GetAttribute<DescriptionAttribute>().Description), personalInfo.Dob);
            }
            FocusElementJS(FindElement(By.CssSelector(PreQualLocators.PreQual.preQualResultsConsentCheckBox.GetAttribute<DescriptionAttribute>().Description)));
            WaitForElementVisible(By.CssSelector(PreQualLocators.PreQual.preQualResultsConsentCheckBox.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(FindElement(By.CssSelector(PreQualLocators.PreQual.preQualResultsConsentCheckBox.GetAttribute<DescriptionAttribute>().Description)));
            ClickElement(FindElement(By.CssSelector(PreQualLocators.PreQual.creditScoreConsentCheckBox.GetAttribute<DescriptionAttribute>().Description)));
            string email = GetElementText(FindElement(By.CssSelector(PreQualLocators.PreQual.Email.GetAttribute<DescriptionAttribute>().Description)));
            IsElementEnabled(FindElement(By.CssSelector(PreQualLocators.PreQual.NextButton.GetAttribute<DescriptionAttribute>().Description)));
            ScrollTo(FindElement(By.CssSelector(PreQualLocators.PreQual.NextButton.GetAttribute<DescriptionAttribute>().Description)));
            WaitForElementClickable(By.CssSelector(PreQualLocators.PreQual.NextButton.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(FindElement(By.CssSelector(PreQualLocators.PreQual.NextButton.GetAttribute<DescriptionAttribute>().Description)));
            return email;

        }
        #endregion
    }
}
