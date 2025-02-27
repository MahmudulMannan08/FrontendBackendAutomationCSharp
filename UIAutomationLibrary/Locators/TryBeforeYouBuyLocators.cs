using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlaceWeb.Locators
{
    public class TryBeforeYouBuyLocators
    {
        public enum TbybLocators
        {
            #region HowItWorks
            [Description("tbyb-onboarding button.button-primary")]
            GetStartedBtn,
            [Description("tbyb-header .icon-block")]
            BackIcon,
            #endregion

            #region PersonalDetails
            [Description("tbyb-personal-details input[formcontrolname='firstName']")]
            FirstNameTxt,
            [Description("tbyb-personal-details input[formcontrolname='lastName']")]
            LastNameTxt,
            [Description("tbyb-personal-details input[formcontrolname='email']")]
            EmailAddressTxt,
            [Description("tbyb-personal-details input[formcontrolname='phone']")]
            PhoneNumberTxt,
            [Description("#lead-phone")]
            ReEnterPhoneTxt,
            [Description("tbyb-personal-details input[formcontrolname='agreeCheckbox']")]
            TermsConditionsCheckbox,
            [Description("tbyb-personal-details input[formcontrolname='communicationCheckbox']")]
            CommunicationCheckbox,
            [Description("tbyb-personal-details tbyb-footer button[class*='button-primary']")]
            NextDeliveryDetailsBtn,
            #endregion

            #region DeliveryDetails
            [Description("tbyb-tile-button[label='DeliveryButton'] button")]
            DeliveryTabBtn,
            [Description("tbyb-tile-button[label='PickupButton'] button")]
            PickupTabBtn,
            [Description("tbyb-delivery-details input[formcontrolname='street']")]
            DeliveryStreetTxt,
            [Description("tbyb-delivery-details input[formcontrolname='unitNumber']")]
            DeliveryUnitNoTxt,
            [Description("tbyb-delivery-details input[formcontrolname='city']")]
            DeliveryCityTxt,
            [Description("tbyb-delivery-details input[formcontrolname='postalCode']")]
            DeliveryPostalCodeTxt,
            [Description("tbyb-delivery-details .dealer-map")]
            PickupDealerMap,
            [Description("tbyb-delivery-details .dealer-address")]
            PickupAddressLbl,
            [Description("tbyb-delivery-details .dates button")]
            DeliveryPickupDates,
            [Description("tbyb-delivery-details .time button")]
            DeliveryPickupTimes,
            [Description("tbyb-delivery-details tbyb-footer button[class*='button-primary']")]
            NextDepositBtn,
            #endregion

            #region PlaceDeposit
            [Description("input[name='cardnumber']"), IframeLocator("#cardNumberElement iframe")]
            CCNumberTxt,
            [Description("tbyb-place-deposit #name")]
            CCNameTxt,
            [Description("input[name='exp-date']"), IframeLocator("#cardExpElement iframe")]
            CCExpiryTxt,
            [Description("input[name='cvc']"), IframeLocator("#cardCvcElement iframe")]
            CVVTxt,
            [Description("tbyb-place-deposit #postalCode")]
            CCPostalCodeTxt,
            [Description("tbyb-place-deposit tbyb-footer button[class*='button-primary']")]
            PlaceDepositBtn,
            [Description("tbyb-place-deposit tbyb-footer button.button-secondary")]
            DepositBackBtn,
            [Description(".vdp-double-ring-progress")]
            LoadElement,
            #endregion

            #region Congratulations
            [Description(".congratulations-address")]
            CongratulationsAddressLbl,
            [Description(".congratulations-date")]
            CongratulationsDateLbl,
            [Description("tbyb-congratulations button.button-primary")]
            BackToListingBtn,
            #endregion

            #region ProgressTbyb
            [Description("tbyb-stepper .status-indicator")]
            ProgressBars,
            #endregion
        }

        public enum SmallLocators
        {
            [Description("tbyb-personal-details tbyb-button .button-primary")]
            NextDeliveryDetailsBtn,
            [Description("tbyb-delivery-details button[class*='button-primary']")]
            NextDepositBtn,
            [Description("tbyb-place-deposit button[class*='button-primary']")]
            PlaceDepositBtn,
            [Description("tbyb-place-deposit button.button-secondary")]
            DepositBackBtn
        }

        public enum XSLocators
        {
            [Description("tbyb-personal-details tbyb-button .button-primary")]
            NextDeliveryDetailsBtn,
            [Description("tbyb-delivery-details button[class*='button-primary']")]
            NextDepositBtn,
            [Description("tbyb-place-deposit button[class*='button-primary']")]
            PlaceDepositBtn,
            [Description("tbyb-place-deposit button.button-secondary")]
            DepositBackBtn
        }
    }
}
