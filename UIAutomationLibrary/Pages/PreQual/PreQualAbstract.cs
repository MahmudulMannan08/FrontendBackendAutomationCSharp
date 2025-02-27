using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using MarketPlaceWeb.Pages.MyGarage;
using MarketPlaceWeb.Pages.VDP;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UIAutomationLibrary.Base;

namespace UIAutomationLibrary.Pages.PreQual
{
    public abstract class PrequalAbstract : Page
    {
        protected PrequalAbstract(IWebDriver driver, Viewport viewport, Language language)
        {
            base.driver = driver;
            base.viewport = viewport;
            base.language = language;
        }

        internal virtual void ClickWhatCanIaffordFromLandingPage()
        {
            ClickElement(FindElement(By.CssSelector(PreQualLocators.PreQual.WhatCanIAfford.GetAttribute<DescriptionAttribute>().Description)));            
        }

        internal void WaitforPreQualModal(bool IsPreQualAlreadyDone=false)
        {
            if(IsPreQualAlreadyDone)
            {
                WaitForElementVisible(By.CssSelector(PreQualLocators.PreQual.PreQualResultsTitle.GetAttribute<DescriptionAttribute>().Description));
            }
            else 
            {
                WaitForElementVisible(By.CssSelector(PreQualLocators.PreQual.ContinueButton.GetAttribute<DescriptionAttribute>().Description));
            }

        }
        internal bool VerifyPrequalModalLanding()
        {

            return IsElementVisible(By.CssSelector(PreQualLocators.PreQual.PreQualModalLanding.GetAttribute<DescriptionAttribute>().Description));
        }
        internal void ClickShopVehicleLink(PreQualLocators.PreQual resultType)
        {
            ClickElement(FindElement(By.CssSelector(PreQualLocators.PreQual.ShopVehiclesbutton.GetAttribute<DescriptionAttribute>().Description)));

        }
        internal bool IsStep1ModalVisible()
        {
            return IsElementVisible(By.CssSelector(PreQualLocators.PreQual.FirstName.GetAttribute<DescriptionAttribute>().Description))
                   && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.MiddleName.GetAttribute<DescriptionAttribute>().Description))
                   && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.LastName.GetAttribute<DescriptionAttribute>().Description))
                   && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.Email.GetAttribute<DescriptionAttribute>().Description))
                   && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.PhoneNumber.GetAttribute<DescriptionAttribute>().Description))
                   && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.creditScoreConsentCheckBox.GetAttribute<DescriptionAttribute>().Description));
        }
        internal void ClickPreQualModalLandingCloseBtn()
        {
            ClickElement(FindElement(By.CssSelector(PreQualLocators.PreQual.PreQualModalLandingCloseBtn.GetAttribute<DescriptionAttribute>().Description)));
        }
        internal bool? IsPreQualModalLandingVisible()
        {
            return IsElementVisible(By.CssSelector(PreQualLocators.PreQual.PreQualModalLanding.GetAttribute<DescriptionAttribute>().Description));
        }
        internal void ClickFindOutYourAffordability()
        {
            ClickElement(FindElement(By.CssSelector(PreQualLocators.PreQual.FindOutYourAffordabilityBtn.GetAttribute<DescriptionAttribute>().Description)));
        }
        internal bool VerifyPrequalLandingPage()
        {
            return IsElementVisible(By.CssSelector(PreQualLocators.PreQual.PreQualHeroBannerContainer.GetAttribute<DescriptionAttribute>().Description))
                && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.PreQualHeroBannerContainerHeaderPanel.GetAttribute<DescriptionAttribute>().Description))
                && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.PreQualWhyValueContainer.GetAttribute<DescriptionAttribute>().Description))
                && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.PreQualWhyValueHeader.GetAttribute<DescriptionAttribute>().Description))
                && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.HowDoesItWorkContainer.GetAttribute<DescriptionAttribute>().Description))
                && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.HowDoesItWorkCardsContainer.GetAttribute<DescriptionAttribute>().Description))
                && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.PreQualFAQContainer.GetAttribute<DescriptionAttribute>().Description))
                && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.ContactUsContainer.GetAttribute<DescriptionAttribute>().Description))
                && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.EmailUsBtn.GetAttribute<DescriptionAttribute>().Description));
        }
        internal void ClickPreQualDeclineShopAllButton(PreQualLocators.PreQual resultType)
        {
            ClickElement(FindElement(By.CssSelector(PreQualLocators.PreQual.PreQualDeclineShopAllButton.GetAttribute<DescriptionAttribute>().Description)));
        }
        internal void ClickTryAgainButton()
        {
            ClickElement(FindElement(By.XPath(PreQualLocators.PreQual.TryAgainButton.GetAttribute<DescriptionAttribute>().Description)));
        }
        internal void ClickBrowseVehiclesButton()
        {
            ClickElement(FindElement(By.XPath(PreQualLocators.PreQual.BrowseVehiclesButton.GetAttribute<DescriptionAttribute>().Description)));
        }
        internal void ClickGetStartedButton()
        {
            ClickElement(FindElement(By.CssSelector(PreQualLocators.PreQual.GetStartedButton.GetAttribute<DescriptionAttribute>().Description)));
        }
        internal string GetPreQualMaxapprovedAmount()
        {
            string preQualAmount = GetElementText(FindElement(By.XPath(PreQualLocators.PreQual.LoanAmount.GetAttribute<DescriptionAttribute>().Description)));
            return Extensions.GetNumberFromString(preQualAmount).ToString();
        }
        internal void ClickContinueOnPreQualModalLanding()
        {
            ClickElement(FindElement(By.CssSelector(PreQualLocators.PreQual.ContinueButton.GetAttribute<DescriptionAttribute>().Description)));
        }
        internal virtual string CompletePreQualStep1(PersonalInformation personalInfo, bool isExistingData = false)
        {
            if (!isExistingData)
            {
                EnterText(By.CssSelector(PreQualLocators.PreQual.FirstName.GetAttribute<DescriptionAttribute>().Description), personalInfo.FirstName);
                EnterText(By.CssSelector(PreQualLocators.PreQual.LastName.GetAttribute<DescriptionAttribute>().Description), personalInfo.LastName);
                EnterText(By.CssSelector(PreQualLocators.PreQual.DateOfBirth.GetAttribute<DescriptionAttribute>().Description), personalInfo.Dob);
                EnterText(By.CssSelector(PreQualLocators.PreQual.PhoneNumber.GetAttribute<DescriptionAttribute>().Description), personalInfo.PhoneNumber);
                ClickElement(FindElement(By.CssSelector(PreQualLocators.PreQual.preQualResultsConsentCheckBox.GetAttribute<DescriptionAttribute>().Description)));
                ClickElement(FindElement(By.CssSelector(PreQualLocators.PreQual.creditScoreConsentCheckBox.GetAttribute<DescriptionAttribute>().Description)));
            }
            else
            {
                EnterText(By.CssSelector(PreQualLocators.PreQual.DateOfBirth.GetAttribute<DescriptionAttribute>().Description), personalInfo.Dob);
                ClickElement(FindElement(By.CssSelector(PreQualLocators.PreQual.preQualResultsConsentCheckBox.GetAttribute<DescriptionAttribute>().Description)));
                ClickElement(FindElement(By.CssSelector(PreQualLocators.PreQual.creditScoreConsentCheckBox.GetAttribute<DescriptionAttribute>().Description)));
            }
            string email = GetElementText(FindElement(By.CssSelector(PreQualLocators.PreQual.Email.GetAttribute<DescriptionAttribute>().Description)));
            ScrollTo(FindElement(By.CssSelector(PreQualLocators.PreQual.NextButton.GetAttribute<DescriptionAttribute>().Description)));
            WaitForElementClickable(By.CssSelector(PreQualLocators.PreQual.NextButton.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(FindElement(By.CssSelector(PreQualLocators.PreQual.NextButton.GetAttribute<DescriptionAttribute>().Description)));
            return email;

        }
        internal void CompletePreQualStep2(AddressInformation addressInfo, bool isExistingData = false)
        {
            if (!isExistingData)
            {
                EnterText(By.CssSelector(PreQualLocators.PreQual.Address.GetAttribute<DescriptionAttribute>().Description), addressInfo.AddressLine1);
                EnterText(By.CssSelector(PreQualLocators.PreQual.City.GetAttribute<DescriptionAttribute>().Description), addressInfo.City);
                ClickElement(FindElement(By.CssSelector(PreQualLocators.PreQual.ProvinceDropdown.GetAttribute<DescriptionAttribute>().Description)));
                WaitForElementClickable(By.CssSelector(PreQualLocators.PreQual.ProvinceOptionsList.GetAttribute<DescriptionAttribute>().Description));
                SelectListItemByValue(addressInfo.Province);
                EnterText(By.CssSelector(PreQualLocators.PreQual.PostalCode.GetAttribute<DescriptionAttribute>().Description), addressInfo.PostalCode);
            }
            WaitForElementClickable(By.CssSelector(PreQualLocators.PreQual.NextbtnStep2.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(FindElement(By.CssSelector(PreQualLocators.PreQual.NextbtnStep2.GetAttribute<DescriptionAttribute>().Description)));
            Wait(2);//Adding static wait to handle to delay 
            ClickElement(FindElement(By.CssSelector(PreQualLocators.PreQual.NextbtnStep2.GetAttribute<DescriptionAttribute>().Description)));
            Wait(3);//Adding static wait to handle to delay 
            if (!IsElementVisible(By.CssSelector(PreQualLocators.PreQual.IDVerificationRadioBtn.GetAttribute<DescriptionAttribute>().Description))
                && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.NextbtnStep2.GetAttribute<DescriptionAttribute>().Description)))
            {

                ClickElement(FindElement(By.CssSelector(PreQualLocators.PreQual.NextbtnStep2.GetAttribute<DescriptionAttribute>().Description)));

            }

        }
        private void SelectListItemByValue(string value)
        {
            // Locate the dropdown list containing the items
            var dropdownItems = FindElements(By.CssSelector(PreQualLocators.PreQual.ProvinceOptionsList.GetAttribute<DescriptionAttribute>().Description));

            // Use a lambda expression to find the item with the matching value
            var item = dropdownItems.FirstOrDefault(li => li.FindElement(By.TagName("span")).Text.Equals(value, StringComparison.OrdinalIgnoreCase));

            // Click the item if found, otherwise throw an exception
            if (item != null)
            {
                item.Click();
            }
            else
            {
                throw new NoSuchElementException($"No list item with value '{value}' found.");
            }
        }
        internal void CompletePreQualIDVStep3()
        {
            WaitForElementVisible(By.CssSelector(PreQualLocators.PreQual.IDVerificationRadioBtn.GetAttribute<DescriptionAttribute>().Description));
            IList<IWebElement> allOptions = FindElements(By.CssSelector(PreQualLocators.PreQual.IDVerificationRadioBtn.GetAttribute<DescriptionAttribute>().Description));
            foreach (WebElement opt in allOptions)
            {
                if (opt.FindElement(By.XPath("parent::div[contains(@class,'choice')]")).GetAttribute("class").Contains("true"))
                {
                    IWebElement optionToClick = opt.FindElement(By.XPath("parent::div[contains(@class,'choice') and contains(@class,'true')]/input"));
                    ScrollTo(optionToClick);
                    opt.FindElement(By.XPath("parent::div[contains(@class,'choice') and contains(@class,'true')]/input")).Click();
                }

            }

            ClickElement(FindElement(By.CssSelector(PreQualLocators.PreQual.SubmitButtonStep3.GetAttribute<DescriptionAttribute>().Description)));
        }
        internal void CompletePreQualStep3InvalidKBA()
        {
            IList<IWebElement> allQuestions = FindElements(By.XPath(PreQualLocators.PreQual.Step3Questions.GetAttribute<DescriptionAttribute>().Description));
            foreach (IWebElement question in allQuestions)
            {
                IList<IWebElement> allOptions = question.FindElements(By.CssSelector(PreQualLocators.PreQual.IDVerificationRadioBtn.GetAttribute<DescriptionAttribute>().Description));
                bool correctAnswerFound = false;

                foreach (IWebElement option in allOptions)
                {
                    if (option.FindElement(By.XPath("parent::div[contains(@class,'choice')]")).GetAttribute("class").Contains("false"))
                    {
                        ScrollTo(option);
                        option.Click();
                        correctAnswerFound = true;
                        break;
                    }
                }

                if (!correctAnswerFound && allOptions.Count > 0)
                {
                    ScrollTo(allOptions[0]);
                    allOptions[0].Click();
                }
            }

            ClickElement(FindElement(By.CssSelector(PreQualLocators.PreQual.SubmitButtonStep3.GetAttribute<DescriptionAttribute>().Description)));


        }
        internal bool VerifyPreQualResults(PreQualLocators.PreQual resultType)
        {
            WaitForPageLoad(10);
            switch (resultType)
            {
                case PreQualLocators.PreQual.SuperPrime:
                    {
                        WaitForElementVisible(By.CssSelector(PreQualLocators.PreQual.PreQualResultContainer.GetAttribute<DescriptionAttribute>().Description), 50);
                        bool reportDetailsExists = (IsElementVisible(By.XPath(PreQualLocators.PreQual.InterestRate.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.XPath(PreQualLocators.PreQual.BiWeeklyPayments.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.XPath(PreQualLocators.PreQual.LoanAmount.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.XPath(PreQualLocators.PreQual.MonthlyTerms.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.XPath(PreQualLocators.PreQual.CreditScoreRange.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.ShopVehiclesbutton.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.ValidUntil.GetAttribute<DescriptionAttribute>().Description)));
                        string creditScore = GetElementText(FindElement(By.XPath(PreQualLocators.PreQual.CreditScoreRange.GetAttribute<DescriptionAttribute>().Description)));
                        bool IsCreditScoreRangeValid = creditScore.Contains("Excellent (850-900)");
                        return reportDetailsExists && IsCreditScoreRangeValid;
                    }

                case PreQualLocators.PreQual.Prime:
                    {
                        WaitForElementVisible(By.CssSelector(PreQualLocators.PreQual.PreQualResultContainer.GetAttribute<DescriptionAttribute>().Description), 50);
                        bool reportDetailsExists = (IsElementVisible(By.XPath(PreQualLocators.PreQual.InterestRate.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.XPath(PreQualLocators.PreQual.BiWeeklyPayments.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.XPath(PreQualLocators.PreQual.LoanAmount.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.XPath(PreQualLocators.PreQual.MonthlyTerms.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.XPath(PreQualLocators.PreQual.CreditScoreRange.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.ShopVehiclesbutton.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.ValidUntil.GetAttribute<DescriptionAttribute>().Description)));
                        string creditScore = GetElementText(FindElement(By.XPath(PreQualLocators.PreQual.CreditScoreRange.GetAttribute<DescriptionAttribute>().Description)));
                        bool IsCreditScoreRangeValid = creditScore.Contains("Very Good (724-849)") || creditScore.Contains("Très bien (724-849)");
                        return reportDetailsExists && IsCreditScoreRangeValid;
                    }

                case PreQualLocators.PreQual.NearPrime:
                    {
                        WaitForElementVisible(By.CssSelector(PreQualLocators.PreQual.PreQualResultContainer.GetAttribute<DescriptionAttribute>().Description), 50);
                        bool reportDetailsExists = (IsElementVisible(By.XPath(PreQualLocators.PreQual.InterestRate.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.XPath(PreQualLocators.PreQual.BiWeeklyPayments.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.XPath(PreQualLocators.PreQual.LoanAmount.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.XPath(PreQualLocators.PreQual.MonthlyTerms.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.XPath(PreQualLocators.PreQual.CreditScoreRange.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.ValidUntil.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.ShopAllVehiclesContainer.GetAttribute<DescriptionAttribute>().Description)));

                        string creditScore = GetElementText(FindElement(By.XPath(PreQualLocators.PreQual.CreditScoreRange.GetAttribute<DescriptionAttribute>().Description)));
                        bool IsCreditScoreRangeValid = creditScore.Contains("Good (660-723)") || creditScore.Contains("Bien (660-723)");
                        return reportDetailsExists && IsCreditScoreRangeValid;
                    }

                case PreQualLocators.PreQual.SubPrime:
                    {
                        WaitForElementVisible(By.CssSelector(PreQualLocators.PreQual.PreQualResultContainer.GetAttribute<DescriptionAttribute>().Description), 50);
                        bool reportDetailsExists = (!IsElementVisible(By.XPath(PreQualLocators.PreQual.InterestRate.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.XPath(PreQualLocators.PreQual.BiWeeklyPayments.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.XPath(PreQualLocators.PreQual.LoanAmount.GetAttribute<DescriptionAttribute>().Description))
                            && !IsElementVisible(By.XPath(PreQualLocators.PreQual.MonthlyTerms.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.XPath(PreQualLocators.PreQual.CreditScoreRange.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.ValidUntil.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.ShopAllVehiclesContainer.GetAttribute<DescriptionAttribute>().Description)));

                        string creditScore = GetElementText(FindElement(By.XPath(PreQualLocators.PreQual.CreditScoreRange.GetAttribute<DescriptionAttribute>().Description)));
                        bool IsCreditScoreRangeValid = creditScore.Contains("Fair (560-659)") || creditScore.Contains("Passable (560-659)");
                        return reportDetailsExists && IsCreditScoreRangeValid;
                    }
                case PreQualLocators.PreQual.DeepSubPrime:
                    {
                        WaitForElementVisible(By.CssSelector(PreQualLocators.PreQual.PreQualResultContainer.GetAttribute<DescriptionAttribute>().Description), 50);
                        bool reportDetailsExists = (!IsElementVisible(By.XPath(PreQualLocators.PreQual.InterestRate.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.XPath(PreQualLocators.PreQual.BiWeeklyPayments.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.XPath(PreQualLocators.PreQual.LoanAmount.GetAttribute<DescriptionAttribute>().Description))
                            && !IsElementVisible(By.XPath(PreQualLocators.PreQual.MonthlyTerms.GetAttribute<DescriptionAttribute>().Description))
                            && !IsElementVisible(By.XPath(PreQualLocators.PreQual.CreditScoreRange.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.ValidUntil.GetAttribute<DescriptionAttribute>().Description))
                            && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.ShopAllVehiclesContainer.GetAttribute<DescriptionAttribute>().Description)));
                        return reportDetailsExists;
                    }

                case PreQualLocators.PreQual.ErrorCreditFileNotFound:
                    {
                        WaitForElementVisible(By.CssSelector(PreQualLocators.PreQual.PreQualErrorDiv.GetAttribute<DescriptionAttribute>().Description), 50);
                        bool reportDetailsExists = (IsElementVisible(By.CssSelector(PreQualLocators.PreQual.WarningIcon.GetAttribute<DescriptionAttribute>().Description))
                           && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.ErrorCreditFileNotFound.GetAttribute<DescriptionAttribute>().Description))
                           && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.ErrorCreditFileNotFoundContent.GetAttribute<DescriptionAttribute>().Description)));
                        string ErrorTitle = GetElementText(FindElement(By.CssSelector(PreQualLocators.PreQual.ErrorInValidIDV.GetAttribute<DescriptionAttribute>().Description)));
                        string ErrorContent = GetElementText(FindElement(By.CssSelector(PreQualLocators.PreQual.ErrorCreditFileNotFoundContent.GetAttribute<DescriptionAttribute>().Description)));
                        bool IsErrorTitleValid = (ErrorTitle.ToLower().Contains(PreQualLocators.PreQual.ErrorCreditFileNotFound.GetAttribute<PreQualLocators.TextValue>().Text.ToLower())
                            || ErrorTitle.ToLower().Contains(PreQualLocators.PreQual.ErrorCreditFileNotFound.GetAttribute<PreQualLocators.FrenchTextValue>().Text.ToLower()));
                        bool IsErrorContentValid = (ErrorContent.ToLower().Contains(PreQualLocators.PreQual.ErrorCreditFileNotFoundContent.GetAttribute<PreQualLocators.TextValue>().Text.ToLower())
                            || ErrorContent.ToLower().Contains(PreQualLocators.PreQual.ErrorCreditFileNotFoundContent.GetAttribute<PreQualLocators.FrenchTextValue>().Text.ToLower()));
                        return reportDetailsExists && IsErrorTitleValid && IsErrorContentValid;
                    }
                case PreQualLocators.PreQual.ErrorInValidIDV:
                case PreQualLocators.PreQual.ErrorInvalidKBA:
                    {
                        WaitForElementVisible(By.CssSelector(PreQualLocators.PreQual.PreQualErrorDiv.GetAttribute<DescriptionAttribute>().Description), 50);
                        bool reportDetailsExists = (IsElementVisible(By.CssSelector(PreQualLocators.PreQual.WarningIcon.GetAttribute<DescriptionAttribute>().Description))
                           && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.ErrorInValidIDV.GetAttribute<DescriptionAttribute>().Description))
                           && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.ErrorContent.GetAttribute<DescriptionAttribute>().Description)));
                        string ErrorTitle = GetElementText(FindElement(By.CssSelector(PreQualLocators.PreQual.ErrorInValidIDV.GetAttribute<DescriptionAttribute>().Description)));
                        string ErrorContent = GetElementText(FindElement(By.CssSelector(PreQualLocators.PreQual.ErrorContent.GetAttribute<DescriptionAttribute>().Description)));
                        bool IsErrorTitleValid = (ErrorTitle.ToLower().Contains(PreQualLocators.PreQual.ErrorInValidIDV.GetAttribute<PreQualLocators.TextValue>().Text.ToLower())
                            || ErrorTitle.ToLower().Contains(PreQualLocators.PreQual.ErrorInValidIDV.GetAttribute<PreQualLocators.FrenchTextValue>().Text.ToLower()));
                        bool IsErrorContentValid = (ErrorContent.ToLower().Contains(PreQualLocators.PreQual.ErrorContent.GetAttribute<PreQualLocators.TextValue>().Text.ToLower())
                            || ErrorContent.ToLower().Contains(PreQualLocators.PreQual.ErrorContent.GetAttribute<PreQualLocators.FrenchTextValue>().Text.ToLower()));
                        return reportDetailsExists && IsErrorTitleValid && IsErrorContentValid;
                    }
                case PreQualLocators.PreQual.Expired:
                    {
                        WaitForElementVisible(By.CssSelector(PreQualLocators.PreQual.PreQualMessageContainer.GetAttribute<DescriptionAttribute>().Description), 50);
                        bool reportDetailsExists = (IsElementVisible(By.CssSelector(PreQualLocators.PreQual.DecorativeIcon.GetAttribute<DescriptionAttribute>().Description))
                        && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.ExpiryMessageTitle.GetAttribute<DescriptionAttribute>().Description))
                        && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.ExpiryMessageDescription.GetAttribute<DescriptionAttribute>().Description)));
                        string ExpiryTitle = GetElementText(FindElement(By.CssSelector(PreQualLocators.PreQual.ExpiryMessageTitle.GetAttribute<DescriptionAttribute>().Description)));
                        bool IsExpiryTitleValid = (ExpiryTitle.ToLower().Contains(PreQualLocators.PreQual.ExpiryMessageTitle.GetAttribute<PreQualLocators.TextValue>().Text.ToLower())
                            || ExpiryTitle.ToLower().Contains(PreQualLocators.PreQual.ExpiryMessageTitle.GetAttribute<PreQualLocators.FrenchTextValue>().Text.ToLower()));
                        return reportDetailsExists && IsExpiryTitleValid;
                    }

                case PreQualLocators.PreQual.IDVMaxLimitReached:
                    {
                        WaitForElementVisible(By.CssSelector(PreQualLocators.PreQual.PreQualErrorDiv.GetAttribute<DescriptionAttribute>().Description), 50);
                        bool reportDetailsExists = (IsElementVisible(By.CssSelector(PreQualLocators.PreQual.WarningIcon.GetAttribute<DescriptionAttribute>().Description))
                           && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.ErrorCouldNotConfirmIdentity.GetAttribute<DescriptionAttribute>().Description))
                           && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.ErrorContent.GetAttribute<DescriptionAttribute>().Description)));
                        string ErrorTitle = GetElementText(FindElement(By.CssSelector(PreQualLocators.PreQual.ErrorCouldNotConfirmIdentity.GetAttribute<DescriptionAttribute>().Description)));
                        string ErrorContent = GetElementText(FindElement(By.CssSelector(PreQualLocators.PreQual.ErrorContent.GetAttribute<DescriptionAttribute>().Description)));
                        bool IsErrorTitleValid = (ErrorTitle.ToLower().Contains(PreQualLocators.PreQual.ErrorCouldNotConfirmIdentity.GetAttribute<PreQualLocators.TextValue>().Text.ToLower())
                            || ErrorTitle.ToLower().Contains(PreQualLocators.PreQual.ErrorCouldNotConfirmIdentity.GetAttribute<PreQualLocators.FrenchTextValue>().Text.ToLower()));
                        bool IsErrorContentValid = (ErrorContent.ToLower().Contains(PreQualLocators.PreQual.ErrorContent.GetAttribute<PreQualLocators.TextValue>().Text.ToLower())
                            || ErrorContent.ToLower().Contains(PreQualLocators.PreQual.ErrorContent.GetAttribute<PreQualLocators.FrenchTextValue>().Text.ToLower()));
                        return reportDetailsExists && IsErrorTitleValid && IsErrorContentValid;
                    }
                case PreQualLocators.PreQual.PreQualDecline:
                    {
                        WaitForElementVisible(By.CssSelector(PreQualLocators.PreQual.PreQualMessageContainer.GetAttribute<DescriptionAttribute>().Description), 50);
                        bool reportDetailsExists = (IsElementVisible(By.CssSelector(PreQualLocators.PreQual.DecorativeIcon.GetAttribute<DescriptionAttribute>().Description))
                           && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.PreQualDeclineMessageTitle.GetAttribute<DescriptionAttribute>().Description))
                           && IsElementVisible(By.CssSelector(PreQualLocators.PreQual.PreQualDeclineMessageContent.GetAttribute<DescriptionAttribute>().Description)));
                        string ErrorTitle = GetElementText(FindElement(By.CssSelector(PreQualLocators.PreQual.PreQualDeclineMessageTitle.GetAttribute<DescriptionAttribute>().Description)));
                        string ErrorContent = GetElementText(FindElement(By.CssSelector(PreQualLocators.PreQual.PreQualDeclineMessageContent.GetAttribute<DescriptionAttribute>().Description)));
                        bool IsErrorTitleValid = (ErrorTitle.ToLower().Contains(PreQualLocators.PreQual.PreQualDeclineMessageTitle.GetAttribute<PreQualLocators.TextValue>().Text.ToLower())
                            || ErrorTitle.ToLower().Contains(PreQualLocators.PreQual.PreQualDeclineMessageTitle.GetAttribute<PreQualLocators.FrenchTextValue>().Text.ToLower()));
                        bool IsErrorContentValid = (ErrorContent.ToLower().Contains(PreQualLocators.PreQual.PreQualDeclineMessageContent.GetAttribute<PreQualLocators.TextValue>().Text.ToLower())
                            || ErrorContent.ToLower().Contains(PreQualLocators.PreQual.PreQualDeclineMessageContent.GetAttribute<PreQualLocators.FrenchTextValue>().Text.ToLower()));
                        return reportDetailsExists && IsErrorTitleValid && IsErrorContentValid;
                    }
            }

            return false;
        }

        internal bool IsExpiryDateValid(PreQualLocators.PreQual resultType)
    {
        string expiryDateString = GetElementText(FindElement(By.CssSelector(PreQualLocators.PreQual.ValidUntil.GetAttribute<DescriptionAttribute>().Description)));
        Console.WriteLine($"Original expiry date string: '{expiryDateString}'");

        string pattern = @"\b(\d{1,2} [a-zA-Zéèê]+ \d{4}|[a-zA-Z]+ \d{1,2},? \d{4})\b";
        Match match = Regex.Match(expiryDateString, pattern);
        Console.WriteLine($"Regex match success: {match.Success}");
        DateTime expiryDate = DateTime.MinValue;

        if (match.Success)
        {
            string datePart = match.Value.Trim();
            Console.WriteLine($"Matched date part: '{datePart}'");

            string[] dateFormats = { "dd MMMM yyyy", "MMMM dd yyyy", "MMMM dd, yyyy" };
            CultureInfo[] cultures = { new CultureInfo("fr-FR"), new CultureInfo("en-US"), CultureInfo.InvariantCulture };

            bool parsed = false;

            foreach (var culture in cultures)
            {
                if (DateTime.TryParseExact(datePart, dateFormats, culture, DateTimeStyles.None, out expiryDate))
                {
                    parsed = true;
                    break;
                }
            }

            if (parsed)
            {
                DateTime expectedDate = DateTime.Today.AddDays(1);
                Console.WriteLine($"Expiry Date: {expiryDate.Date}, Expected Date: {expectedDate.Date}");
                return expiryDate.Date == expectedDate.Date;
            }
        }

        return false;
    }






    public class PreQualResultsPage
        {
            public string InterestRate { get; set; }
            public string BiWeeklyPayments { get; set; }
            public string LoanAmount { get; set; }
            public string MonthlyTerms { get; set; }
            public string CreditScoreRange { get; set; }
            public string ValidUntil { get; set; }
        }
    }
}
