using MarketPlaceWeb.Base;
using OpenQA.Selenium;
using System;
using System.ComponentModel;
using System.Linq;
using UIAutomationLibrary.Locators;

namespace UIAutomationLibrary.Pages.InstantCashOffer
{
    public class InstantCashOfferLarge : InstantCashOfferAbstract
    {
        public InstantCashOfferLarge(IWebDriver driver, Viewport viewport, Language language) : base(driver, viewport, language) { }

        public  override void  ClickNextBtnOnIcoLead(By locator, string locatorValue = null, bool scrollIntoView = false, int timeOut = 30)
        {
            WaitForElementVisible(locator, timeOut:30);
            IWebElement element = FindElement(locator);
            if (scrollIntoView) { ScrollTo(element); }
            ClickElement(element);
            WaitForIcoModalProgress(timeOut);
        }

        public override void EnterYmmtDetails(IcoLeadForm icoLeadForm)
        {
            By yearLocator = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoYearDropdown.GetAttribute<DescriptionAttribute>().Description);
            WaitForElementVisible(yearLocator);
            SelectByText(yearLocator, icoLeadForm.Year);
            WaitForIcoModalProgress();

            By makeLocator = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoMakeDropdown.GetAttribute<DescriptionAttribute>().Description);
            SelectByText(makeLocator, icoLeadForm.Make);
            WaitForIcoModalProgress();

            By modelLocator = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoModelDropdown.GetAttribute<DescriptionAttribute>().Description);
            SelectFirstOption(modelLocator);
            WaitForIcoModalProgress();

            By trimlLocator = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoYmmtTrimDropdown.GetAttribute<DescriptionAttribute>().Description);
            SelectFirstOption(trimlLocator);
            WaitForIcoModalProgress();

            By nextBasicDetailBtnLocator = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoYmmtNextBasicDetailBtn.GetAttribute<DescriptionAttribute>().Description);
            ClickNextBtnOnIcoLead(nextBasicDetailBtnLocator, scrollIntoView:true);
        }

        public override void EnterVinDetails(IcoLeadForm icoLeadForm)
        {
            By vinLocator = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoVinTextbox.GetAttribute<DescriptionAttribute>().Description);
            WaitForElementVisible(vinLocator);
            EnterText(vinLocator, icoLeadForm.Vin);
            UnFocusElementJS(FindElement(vinLocator));
            WaitForIcoModalProgress();

            if (!IsVinTrimEnabled())
            {
                //Try again
                VinGenerator vinGenerator = new VinGenerator();
                icoLeadForm.Vin = vinGenerator.GetRandomVin().VinNumber;
                EnterText(vinLocator, icoLeadForm.Vin);
                UnFocusElementJS(FindElement(vinLocator));
                WaitForIcoModalProgress();

                if (!IsVinTrimEnabled())
                {
                    throw new Exception("Unable to generate random valid VIN");
                }
            }

            By trimlLocator = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoVinTrimDropdown.GetAttribute<DescriptionAttribute>().Description);
            SelectFirstOption(trimlLocator);
            WaitForIcoModalProgress();

            if (IsVinServerSideErrorDisplayed())
            {
                throw new Exception("ICO not available for VIN: " + icoLeadForm.Vin);
            }

            By nextBasicDetailBtnLocator = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoVinNextBasicDetailBtn.GetAttribute<DescriptionAttribute>().Description);
            ClickNextBtnOnIcoLead(nextBasicDetailBtnLocator);
        }

        public override void EnterIncorrectVinDetails()
        {
            for (int i = 0; i < 3; i++)
            {
                By vinLocator = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoVinTextbox.GetAttribute<DescriptionAttribute>().Description);
                WaitForElementVisible(vinLocator);
                EnterText(vinLocator, "1234");
                UnFocusElementJS(FindElement(vinLocator));
                WaitForIcoModalProgress();

                By nextBasicDetailBtnLocator = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoVinNextBasicDetailBtn.GetAttribute<DescriptionAttribute>().Description);
                ClickNextBtnOnIcoLead(nextBasicDetailBtnLocator);
            }
        }
        public override void ClickEnterYourVehicleMakeAndModelBtn()
        {
            By BtnEnterYourVehicleMakeandModel = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoOrEnterYourVehicleMakeAndModelBtn.GetAttribute<DescriptionAttribute>().Description);
            WaitForElementVisible(BtnEnterYourVehicleMakeandModel);
            ClickElement(FindElement(BtnEnterYourVehicleMakeandModel));
        }
        public override void EnterVinDetailsDWW(IcoLeadForm icoLeadForm)
        {
            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame("icoFrame");
            By vinLocator = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoVinTextbox.GetAttribute<DescriptionAttribute>().Description);
            By trimlLocator = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoVinTrimDropdown.GetAttribute<DescriptionAttribute>().Description);
            By nextBasicDetailBtnLocator = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoNextBtn.GetAttribute<DescriptionAttribute>().Description);

            WaitForElementVisible(vinLocator);
            EnterText(vinLocator, icoLeadForm.Vin);
            UnFocusElementJS(FindElement(vinLocator));
            WaitForIcoModalProgress();

            int counter = 0;

            while (IsValidVin(icoLeadForm) == false)
            {
                //Try again
                VinGenerator vinGenerator = new VinGenerator();
                icoLeadForm.Vin = vinGenerator.GetRandomVin().VinNumber;
                EnterText(vinLocator, icoLeadForm.Vin);
                UnFocusElementJS(FindElement(vinLocator));
                WaitForIcoModalProgress();
                counter++;

                if (counter > 3)
                {
                    throw new Exception("Unable to generate random valid VIN");
                }
            }

            UnFocusElementJS(FindElement(vinLocator));
            WaitForIcoModalProgress();

            SelectFirstOption(trimlLocator);
            WaitForIcoModalProgress();

            if (IsVinServerSideErrorDisplayed())
            {
                throw new Exception("ICO not available for VIN: " + icoLeadForm.Vin);
            }

            ClickNextBtnOnIcoLead(nextBasicDetailBtnLocator, InstantCashOfferLocators.DWWLocators.IcoNextBtn.GetAttribute<DescriptionAttribute>().Description , scrollIntoView:true , timeOut:30);
        }

        public override void EnterVehicleDetails(IcoLeadForm icoLeadForm)
        {
            IWebElement colorElement = FindElements(By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoVehicleColors.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault(x => x.GetAttribute("title").Contains(icoLeadForm.Color));
            ClickElement(colorElement);

            By kilometerLocator = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoVehicleKilometer.GetAttribute<DescriptionAttribute>().Description);
            EnterTextWithLoadOnIcoLead(kilometerLocator, icoLeadForm.Kilometers);

            By postalCodeLocator = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoVehiclePostalCode.GetAttribute<DescriptionAttribute>().Description);
            EnterTextWithLoadOnIcoLead(postalCodeLocator, icoLeadForm.PostalCode);
            UnFocusElementJS(FindElement(postalCodeLocator));
            WaitForIcoModalProgress();

            By ownershipAnsLocators = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoVehicleOwnershipAns.GetAttribute<DescriptionAttribute>().Description);
            SelectIcoAnswer(ownershipAnsLocators, icoLeadForm.IcoQuestionnaireAnswers[IcoLeadForm.IcoQuestionnaire.IsOriginalOwner], icoLeadForm: icoLeadForm);

            By makePaymentsAnsLocators = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoVehiclePaymentsAns.GetAttribute<DescriptionAttribute>().Description);
            SelectIcoAnswer(makePaymentsAnsLocators, icoLeadForm.IcoQuestionnaireAnswers[IcoLeadForm.IcoQuestionnaire.IsMakingPayment], icoLeadForm: icoLeadForm, scrollIntoView: true);

            By icoVehicleReplacementAnsLocators = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoVehicleReplacementAns.GetAttribute<DescriptionAttribute>().Description);
            if (IsElementAvailable(icoVehicleReplacementAnsLocators))
            {
                SelectIcoAnswer(icoVehicleReplacementAnsLocators, icoLeadForm.IcoQuestionnaireAnswers[IcoLeadForm.IcoQuestionnaire.IsVehicleReplacement], icoLeadForm: icoLeadForm);
            }

            By nextConditionBtnLocator = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoVehicleDetailsNextCondBtn.GetAttribute<DescriptionAttribute>().Description);
            ClickNextBtnOnIcoLead(nextConditionBtnLocator, scrollIntoView: true);
        }

        public override void EnterVehicleDetailsDWW(IcoLeadForm icoLeadForm)
        {
            IWebElement colorElement = FindElement(By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoVehicleColors.GetAttribute<DescriptionAttribute>().Description));
            By kilometerLocator = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoVehicleKilometer.GetAttribute<DescriptionAttribute>().Description);
            By postalCodeLocator = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoVehiclePostalCode.GetAttribute<DescriptionAttribute>().Description);
            By ownershipAnsLocators = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoVehicleOwnershipAns.GetAttribute<DescriptionAttribute>().Description);
            By makePaymentsAnsLocators = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoVehiclePaymentsAns.GetAttribute<DescriptionAttribute>().Description);
            By icoVehicleReplacementAnsLocators = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoVehicleReplacementAns.GetAttribute<DescriptionAttribute>().Description);
            By nextConditionBtnLocator = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoNextBtn.GetAttribute<DescriptionAttribute>().Description);
            ScrollTo(colorElement);

            ClickElementJS(InstantCashOfferLocators.DWWLocators.IcoVehicleColors.GetAttribute<DescriptionAttribute>().Description); 
           
            EnterTextWithLoadOnIcoLead(kilometerLocator, icoLeadForm.Kilometers);

            EnterTextWithLoadOnIcoLead(postalCodeLocator, icoLeadForm.PostalCode);
            UnFocusElementJS(FindElement(postalCodeLocator));
            WaitForIcoModalProgress();

            SelectIcoAnswer(ownershipAnsLocators,
                icoLeadForm.IcoQuestionnaireAnswers[IcoLeadForm.IcoQuestionnaire.IsOriginalOwner],
                locatorValue: InstantCashOfferLocators.DWWLocators.IcoVehicleOwnershipAns.GetAttribute<DescriptionAttribute>().Description,
                 icoLeadForm :icoLeadForm, scrollIntoView: true);

            SelectIcoAnswer(makePaymentsAnsLocators,
                icoLeadForm.IcoQuestionnaireAnswers[IcoLeadForm.IcoQuestionnaire.IsMakingPayment],
                locatorValue: InstantCashOfferLocators.DWWLocators.IcoVehiclePaymentsAns.GetAttribute<DescriptionAttribute>().Description,
                 icoLeadForm :icoLeadForm, scrollIntoView: true);

            if (IsElementAvailable(icoVehicleReplacementAnsLocators))
            {
                SelectIcoAnswer(icoVehicleReplacementAnsLocators,
                    icoLeadForm.IcoQuestionnaireAnswers[IcoLeadForm.IcoQuestionnaire.IsVehicleReplacement],
                    locatorValue: InstantCashOfferLocators.DWWLocators.IcoVehicleReplacementAns.GetAttribute<DescriptionAttribute>().Description,
                     icoLeadForm :icoLeadForm, scrollIntoView: true);
            }

            ClickNextBtnOnIcoLead(nextConditionBtnLocator, locatorValue: InstantCashOfferLocators.DWWLocators.IcoNextBtn.GetAttribute<DescriptionAttribute>().Description, scrollIntoView: true);

        }


        public override void EnterVehicleConditions(IcoLeadForm icoLeadForm)
        {
            By wasInAccidentAnsLocators = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoVehicleAccidentAns.GetAttribute<DescriptionAttribute>().Description);
            SelectIcoAnswer(wasInAccidentAnsLocators, icoLeadForm.IcoQuestionnaireAnswers[IcoLeadForm.IcoQuestionnaire.WasInAccident], icoLeadForm: icoLeadForm);

            By hasDamageAnsLocators = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoVehicleDamageAns.GetAttribute<DescriptionAttribute>().Description);
            SelectIcoAnswer(hasDamageAnsLocators, icoLeadForm.IcoQuestionnaireAnswers[IcoLeadForm.IcoQuestionnaire.HasDamage], icoLeadForm: icoLeadForm);

            By frontTireDropdown = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoFrontTireDropdown.GetAttribute<DescriptionAttribute>().Description);
            SelectByValuePartialMatch(frontTireDropdown, icoLeadForm.FrontTireCondition);

            By rearTireDropdown = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoRearTireDropdown.GetAttribute<DescriptionAttribute>().Description);
            SelectByValuePartialMatch(rearTireDropdown, icoLeadForm.RearTireCondition);

            By hasIssueAnsLocators = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoVehicleIssuesAns.GetAttribute<DescriptionAttribute>().Description);
            SelectIcoAnswer(hasIssueAnsLocators, icoLeadForm.IcoQuestionnaireAnswers[IcoLeadForm.IcoQuestionnaire.HasMechanicalIssue], icoLeadForm: icoLeadForm);

            By hasLightAnsLocators = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoVehicleLightsAns.GetAttribute<DescriptionAttribute>().Description);
            SelectIcoAnswer(hasLightAnsLocators, icoLeadForm.IcoQuestionnaireAnswers[IcoLeadForm.IcoQuestionnaire.HasWarningLight], icoLeadForm: icoLeadForm);

            By hasModAnsLocators = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoVehicleModAns.GetAttribute<DescriptionAttribute>().Description);
            SelectIcoAnswer(hasModAnsLocators, icoLeadForm.IcoQuestionnaireAnswers[IcoLeadForm.IcoQuestionnaire.HasModification], icoLeadForm: icoLeadForm);

            By hasOdorAnsLocators = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoVehicleOdorAns.GetAttribute<DescriptionAttribute>().Description);
            SelectIcoAnswer(hasOdorAnsLocators, icoLeadForm.IcoQuestionnaireAnswers[IcoLeadForm.IcoQuestionnaire.HasOdor], icoLeadForm: icoLeadForm);

            By hasOtherIssueAnsLocators = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoVehicleOtherIssuesAns.GetAttribute<DescriptionAttribute>().Description);
            SelectIcoAnswer(hasOtherIssueAnsLocators, icoLeadForm.IcoQuestionnaireAnswers[IcoLeadForm.IcoQuestionnaire.HasOtherIssue], icoLeadForm: icoLeadForm);

            By nextContactBtnLocator = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoVehicleCondNextContactBtn.GetAttribute<DescriptionAttribute>().Description);
            ClickNextBtnOnIcoLead(nextContactBtnLocator,scrollIntoView: true);
        }

        public override void EnterVehicleConditionsDWW(IcoLeadForm icoLeadForm)
        {
            
            By wasInAccidentAnsLocators = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoVehicleAccidentAns.GetAttribute<DescriptionAttribute>().Description);
            By hasHistoryAns = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoVehicleHistoryAns.GetAttribute<DescriptionAttribute>().Description);
            By hasDamageAnsLocators = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoVehicleDamageAns.GetAttribute<DescriptionAttribute>().Description);
            By tireCondition = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoTireCondition.GetAttribute<DescriptionAttribute>().Description);
            By hasIssueAnsLocators = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoVehicleIssuesAns.GetAttribute<DescriptionAttribute>().Description);
            By hasLightAnsLocators = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoVehicleLightsAns.GetAttribute<DescriptionAttribute>().Description);
            By hasModAnsLocators = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoVehicleModAns.GetAttribute<DescriptionAttribute>().Description);
            By hasOdorAnsLocators = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoVehicleOdorAns.GetAttribute<DescriptionAttribute>().Description);
            By hasOtherIssueAnsLocators = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoVehicleOtherIssuesAns.GetAttribute<DescriptionAttribute>().Description);
            By nextContactBtnLocator = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoNextBtn.GetAttribute<DescriptionAttribute>().Description);


            WaitForElementVisible(nextContactBtnLocator);
            ScrollTo(FindElement(tireCondition));
            SelectIcoAnswer(tireCondition,
                icoLeadForm.IcoQuestionnaireAnswers[IcoLeadForm.IcoQuestionnaire.TireCondition],
                locatorValue: InstantCashOfferLocators.DWWLocators.IcoTireCondition.GetAttribute<DescriptionAttribute>().Description,
                 icoLeadForm :icoLeadForm, scrollIntoView: true);
            SelectIcoAnswer(hasHistoryAns,
                icoLeadForm.IcoQuestionnaireAnswers[IcoLeadForm.IcoQuestionnaire.HasCleanHistory],
                locatorValue: InstantCashOfferLocators.DWWLocators.IcoVehicleHistoryAns.GetAttribute<DescriptionAttribute>().Description,
                 icoLeadForm :icoLeadForm, scrollIntoView: true);
                   

            SelectIcoAnswer(wasInAccidentAnsLocators,
                icoLeadForm.IcoQuestionnaireAnswers[IcoLeadForm.IcoQuestionnaire.WasInAccident],
                 locatorValue: InstantCashOfferLocators.DWWLocators.IcoVehicleAccidentAns.GetAttribute<DescriptionAttribute>().Description,
                  icoLeadForm :icoLeadForm, scrollIntoView: true);

            SelectIcoAnswer(hasHistoryAns, icoLeadForm.IcoQuestionnaireAnswers[IcoLeadForm.IcoQuestionnaire.HasDamage],
                locatorValue: InstantCashOfferLocators.DWWLocators.IcoVehicleHistoryAns.GetAttribute<DescriptionAttribute>().Description,
                  icoLeadForm :icoLeadForm, scrollIntoView: true);

            SelectIcoAnswer(hasDamageAnsLocators,
                icoLeadForm.IcoQuestionnaireAnswers[IcoLeadForm.IcoQuestionnaire.HasDamage],
                 locatorValue: InstantCashOfferLocators.DWWLocators.IcoVehicleDamageAns.GetAttribute<DescriptionAttribute>().Description,
                  icoLeadForm :icoLeadForm, scrollIntoView: true);


            SelectIcoAnswer(hasIssueAnsLocators, icoLeadForm.IcoQuestionnaireAnswers[IcoLeadForm.IcoQuestionnaire.HasMechanicalIssue],
                 locatorValue: InstantCashOfferLocators.DWWLocators.IcoVehicleIssuesAns.GetAttribute<DescriptionAttribute>().Description,
                  icoLeadForm :icoLeadForm, scrollIntoView: true);

            SelectIcoAnswer(hasLightAnsLocators, icoLeadForm.IcoQuestionnaireAnswers[IcoLeadForm.IcoQuestionnaire.HasWarningLight],
                 locatorValue: InstantCashOfferLocators.DWWLocators.IcoVehicleLightsAns.GetAttribute<DescriptionAttribute>().Description,
                  icoLeadForm :icoLeadForm, scrollIntoView: true);

            SelectIcoAnswer(hasModAnsLocators, icoLeadForm.IcoQuestionnaireAnswers[IcoLeadForm.IcoQuestionnaire.HasModification],
                 locatorValue: InstantCashOfferLocators.DWWLocators.IcoVehicleModAns.GetAttribute<DescriptionAttribute>().Description,
                  icoLeadForm :icoLeadForm, scrollIntoView: true);

            SelectIcoAnswer(hasOdorAnsLocators, icoLeadForm.IcoQuestionnaireAnswers[IcoLeadForm.IcoQuestionnaire.HasOdor],
                 locatorValue: InstantCashOfferLocators.DWWLocators.IcoVehicleOdorAns.GetAttribute<DescriptionAttribute>().Description,
                  icoLeadForm :icoLeadForm, scrollIntoView: true);

            SelectIcoAnswer(hasOtherIssueAnsLocators, icoLeadForm.IcoQuestionnaireAnswers[IcoLeadForm.IcoQuestionnaire.HasOtherIssue],
                 locatorValue: InstantCashOfferLocators.DWWLocators.IcoVehicleOtherIssuesAns.GetAttribute<DescriptionAttribute>().Description,
                  icoLeadForm :icoLeadForm, scrollIntoView: true);
            ClickNextBtnOnIcoLead(nextContactBtnLocator, locatorValue: InstantCashOfferLocators.DWWLocators.IcoNextBtn.GetAttribute<DescriptionAttribute>().Description, scrollIntoView: true);

        }

        public override void EnterCustomerDetails(IcoLeadForm icoLeadForm)
        {
            By icoFirstNameLocator = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoFirstName.GetAttribute<DescriptionAttribute>().Description);
            EnterTextWithLoadOnIcoLead(icoFirstNameLocator, icoLeadForm.FirstName);

            By icoLastNameLocator = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoLastName.GetAttribute<DescriptionAttribute>().Description);
            EnterTextWithLoadOnIcoLead(icoLastNameLocator, icoLeadForm.LastName);

            By icoMobileLocator = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoMobileNumber.GetAttribute<DescriptionAttribute>().Description);
            EnterTextWithLoadOnIcoLead(icoMobileLocator, icoLeadForm.PhoneNumber);
            if (IsElementVisible(By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoCustomerDetailError.GetAttribute<DescriptionAttribute>().Description)))
            {
                EnterTextWithLoadOnIcoLead(icoMobileLocator, "6044" + Extensions.GenerateRandomNumber(6));
            }

            By icoEmailLocator = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoEmail.GetAttribute<DescriptionAttribute>().Description);
            EnterTextWithLoadOnIcoLead(icoEmailLocator, icoLeadForm.Email);

            By icoContactTimeLocator = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoContactTimeDropdown.GetAttribute<DescriptionAttribute>().Description);
            SelectFirstOption(icoContactTimeLocator);

            By icoSellWhenLocator = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoSellWhenDropdown.GetAttribute<DescriptionAttribute>().Description);
            if (IsElementAvailable(icoSellWhenLocator))
            {
                SelectFirstOption(icoSellWhenLocator);
            }

            CheckIcoTerms(icoLeadForm, locatorValue: InstantCashOfferLocators.CommonLocators.IcoTermsChkbox.GetAttribute<DescriptionAttribute>().Description, scrollIntoView: true);

            By nextIcoBtnLocator = By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoNextIcoBtn.GetAttribute<DescriptionAttribute>().Description);
            ClickNextBtnOnIcoLead(nextIcoBtnLocator, scrollIntoView: true);
        }

        public override void EnterCustomerDetailsDWW(IcoLeadForm icoLeadForm)
        {
            icoLeadForm.PhoneNumber = "8675555309";
            icoLeadForm.PhoneCode = "1234";
            By icoFirstNameLocator = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoFirstName.GetAttribute<DescriptionAttribute>().Description);
            By icoLastNameLocator = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoLastName.GetAttribute<DescriptionAttribute>().Description);
            By icoMobileLocator = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoMobileNumber.GetAttribute<DescriptionAttribute>().Description);
            By icoEmailLocator = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoEmail.GetAttribute<DescriptionAttribute>().Description);
            By icoContactTimeLocator = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoContactTimeDropdown.GetAttribute<DescriptionAttribute>().Description);
            By icoSellWhenLocator = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoSellWhenDropdown.GetAttribute<DescriptionAttribute>().Description);
            By nextIcoBtnLocator = By.CssSelector(InstantCashOfferLocators.DWWLocators.IcoNextBtn.GetAttribute<DescriptionAttribute>().Description);

            WaitForElementVisible(icoFirstNameLocator);
            EnterTextWithLoadOnIcoLead(icoFirstNameLocator, icoLeadForm.FirstName);

            EnterTextWithLoadOnIcoLead(icoLastNameLocator, icoLeadForm.LastName);

            EnterTextWithLoadOnIcoLead(icoMobileLocator, icoLeadForm.PhoneNumber);
            if (IsElementVisible(By.CssSelector(InstantCashOfferLocators.CommonLocators.IcoCustomerDetailError.GetAttribute<DescriptionAttribute>().Description)))
            {
                EnterTextWithLoadOnIcoLead(icoMobileLocator, "6044" + Extensions.GenerateRandomNumber(6));
            }

            EnterTextWithLoadOnIcoLead(icoEmailLocator, icoLeadForm.Email);

            SelectFirstOption(icoContactTimeLocator);

            if (IsElementAvailable(icoSellWhenLocator))
            {
                SelectFirstOption(icoSellWhenLocator);
            }

            CheckIcoTerms(icoLeadForm, locatorValue: InstantCashOfferLocators.DWWLocators.IcoTermsChkbox.GetAttribute<DescriptionAttribute>().Description, scrollIntoView: true);

            ClickNextBtnOnIcoLead(nextIcoBtnLocator, locatorValue: InstantCashOfferLocators.DWWLocators.IcoNextBtn.GetAttribute<DescriptionAttribute>().Description, scrollIntoView: true);
        }
    }
}
