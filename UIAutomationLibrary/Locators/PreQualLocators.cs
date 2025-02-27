using System;
using System.ComponentModel;

namespace MarketPlaceWeb.Locators
{
    public class PreQualLocators
    {
        public enum PreQual
        {
            [Description("#lnkPre-qual")]
            PreQualNavBar,
            [Description("div.hero-banner-container")]
            PreQualHeroBannerContainer,
            [Description("div.hero-banner-container .header-panel")]
            PreQualHeroBannerContainerHeaderPanel,
            [Description("div.why-value-container")]
            PreQualWhyValueContainer,
            [Description("div.why-value-container .why-value-header")]
            PreQualWhyValueHeader,
            [Description("div.content-row.first-block")]
            PreQualWhyValueContentFirstBlock,
            [Description("div.content-row.second-block")]
            PreQualWhyValueContentSecondBlock,
            [Description("div.content-row.third-block")]
            PreQualWhyValueContentThirdBlock,
            [Description("div.content-row.fourth-block")]
            PreQualWhyValueContentFourthBlock,
            [Description("div.how-it-work-container")]
            HowDoesItWorkContainer,
            [Description("div.how-it-work-container .first-row")]
            HowDoesItWorkContainerFirstRow,
            [Description("div.how-it-work-container .how-it-work-cards-container")]
            HowDoesItWorkCardsContainer,
            [Description("app-how-it-work #start-journey")]
            FindOutYourAffordabilityBtn,
            [Description("div.faq-container")]
            PreQualFAQContainer,
            [Description("app-contact-us #contactus")]
            ContactUsContainer,
            [Description("app-contact-us #email-us")]
            EmailUsBtn,
            [Description("app-hero-section #start-journey")]
            WhatCanIAfford,
            [Description("div.introduction-content")]
            PreQualModalLanding,
            [Description("#closeButton")]
            PreQualModalLandingCloseBtn,
            [Description("#continue > button")]
            ContinueButton,
            [Description("#firstName")]
            FirstName,
            [Description("#middleName")]
            MiddleName,
            [Description("#lastName")]
            LastName,
            [Description("#dob")]
            DateOfBirth,
            [Description("#emailField input")]
            Email,
            [Description("#phone")]
            PhoneNumber,
            [Description("div.preQualResultsConsent label")]
            preQualResultsConsentCheckBox,
            [Description("div.preQualResultsConsent p")]
            preQualResultsConsentText,
            [Description("div.creditScoreConsent label")]
            creditScoreConsentCheckBox,
            [Description("div.creditScoreConsent p")]
            creditScoreConsentText,
            [Description("axle-button.closebutton")]
            CloseButton,
            [Description("axle-button[class='nextButton'] button[type='button']")]
            NextButton,
            [Description("#address")]
            Address,
            [Description("#city")]
            City,
            [Description("div.province-container")]
            ProvinceDropdown,
            [Description("ul li.dropdown-options")]
            ProvinceOptionsList,
            [Description("#postalCode")]
            PostalCode,
            [Description("#country")]
            Country,
            [Description("axle-button.backbutton")]
            BackBtnStep2,
            [Description("div.address-info-footer .nextbutton")]
            NextbtnStep2,
            [Description("//p[@class='dynamic-qa-text']/parent::div")]
            Step3Questions,
            [Description("input.prequal-radio")]
            IDVerificationRadioBtn,
            [Description("div.dynamic-qa-footer .submitButton")]
            SubmitButtonStep3,
            [Description("app-modal-header .pre-qualification")]
            PreQualResultsTitle,
            [Description(".icon-label.info-icon-label.ng-star-inserted")]
            PreQualResultsPageDisclaimer,
            [Description(".power-by.ng-star-inserted")]
            PoweredByEquifax,
            [Description("//div[contains(text(),'Interest rate') or contains(text(),\"Taux d'intérêt\")]/following-sibling::div[@class='description']")]
            InterestRate,
            [Description("//div[contains(text(),'Bi-weekly payments') or contains(text(),\"Paiements aux deux semaines\")]/following-sibling::div[@class='description']")]
            BiWeeklyPayments,
            [Description("//div[contains(text(),'Loan amount') or contains(text(),\"Montant du prêt\")]/following-sibling::div[@class='description']")]
            LoanAmount,
            [Description("//div[contains(text(),'Monthly terms') or contains(text(),\"Modalités mensuelles\")]/following-sibling::div[@class='description']")]
            MonthlyTerms,
            [Description("//div[contains(text(),'Credit score range') or contains(text(),\"Plage de cote de crédit\")]/following-sibling::div[@class='description']")]
            CreditScoreRange,
            [Description("#shopButton")]
            ShopVehiclesbutton,
            [Description("div.shop-vehicle-container .valid-until")]
            ValidUntil,
            SuperPrime,
            Prime,
            NearPrime,
            SubPrime,
            DeepSubPrime,
            Expired,
            PreQualDecline,
            IDVMaxLimitReached,
            CreditFileNotFound,
            IDVDeclined,
            [Description("div.start-shopping-container")]
            ShopAllVehiclesContainer,
            [Description("#faceted-Price")]
            SRPPriceFilter,
            [Description("#preQualModal axle-button")]
            GetStartedButton,
            [Description("//button[contains(text(),'Browse vehicles') or contains(text(),'Parcourir les véhicules')]")]
            BrowseVehiclesButton,
            [Description("p.pre-qual-error-title"), TextValue("Sorry, we are unable to fetch your results at this time."),FrenchTextValue("Désolé, nous ne sommes pas en mesure de récupérer vos résultats pour le moment.")]
            ErrorInValidIDV,
            [Description("p.pre-qual-error-title"), TextValue("Sorry, we could not find you."), FrenchTextValue("Désolé, nous n'avons pas pu vous trouver.")]
            ErrorCreditFileNotFound,
            [Description("p.pre-qual-error-content"), TextValue("Please check your information again based on your official documents to help us locate your credit file."), FrenchTextValue("Veuillez vérifier à nouveau vos informations en fonction de vos documents officiels pour nous aider à localiser votre dossier de crédit.")]
            ErrorCreditFileNotFoundContent,
            [Description("//app-error//button[contains(text(),'Try again') or contains(text(),'Essayez à nouveau')]")]
            TryAgainButton,
            [Description("p.pre-qual-error-content"),TextValue("You can try again after 72 hours."),FrenchTextValue("Vous pouvez réessayer après 72 heures.")]
            ErrorContent,
            [Description("axle-system-icon[name='warning']")]
            WarningIcon,
            [Description("axle-decorative-icon.message-icon")]
            DecorativeIcon,
            [Description("p.message-title"), TextValue("Your prequalification is expired"), FrenchTextValue("Votre préqualification a expiré.")]
            ExpiryMessageTitle,
            [Description("div.message-description")]
            ExpiryMessageDescription,
            ErrorInvalidKBA,
            [Description("div.prequal-message-container")]
            PreQualMessageContainer,
            [Description("div.result-container")]
            PreQualResultContainer,
            [Description("div.pre-qual-error")]
            PreQualErrorDiv,
            [Description("p.pre-qual-error-title"), TextValue("We could not confirm your identity."),FrenchTextValue("Nous n'avons pas pu confirmer votre identité.")]
            ErrorCouldNotConfirmIdentity,
            [Description("p.message-title"), TextValue("Your local dealer will have tailored financing options for you."), FrenchTextValue("Votre concessionnaire local aura des options de financement sur mesure pour vous.")]
            PreQualDeclineMessageTitle,
            [Description("div.message-description"), TextValue("Sorry, based on the information you provided, prequalification is not available for you at this time. Find your desired car and reach out to the dealer for personalized financing options tailored to your needs."), FrenchTextValue("Désolé, selon les informations que vous avez fournies, la préqualification n'est pas disponible pour vous actuellement. Trouvez la voiture de votre choix et contactez le concessionnaire pour des options de financement personnalisées adaptées à vos besoins.")]
            PreQualDeclineMessageContent,
            [Description("axle-button.action-button")]
            PreQualDeclineShopAllButton,
            

        }


        internal class TextValue : Attribute
        {
            public string Text { get; set; }

            public TextValue(string value)
            {
                Text = value;
            }
        }

        internal class FrenchTextValue : Attribute
        {
            public string Text { get; set; }

            public FrenchTextValue(string value)
            {
                Text = value;
            }
        }
    }
}
