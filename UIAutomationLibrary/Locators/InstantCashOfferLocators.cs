using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIAutomationLibrary.Locators
{
    public class InstantCashOfferLocators
    {
        public enum CommonLocators
        {
            [Description("vdp-ico-widget")]
            IcoWidget,
            [DescriptionTradeIn("vdp-ico-widget div.card-cta a"), DescriptionBuyingCenter("#ReadyToGetStartedTitle a.getOfferNow")]
            IcoButton,
            [Description("vdp-ico-widget h3.ico-title")]
            IcoOfferTitle,
            [Description("#icoModal")]
            IcoModal,
            [Description(".vdp-double-ring-progress")]
            IcoModalProgress,
            [Description("#ymmtPanel #years")]
            IcoYearDropdown,
            [Description("#CarTooOld")]
            IcoHighMileageOldVehicle,
            [Description("#CarTooOldButtonPlaceHolder")]
            PostFreeAd,
            [Description("#sell-tips-image")]
            SellTipVideo,
            [Description("img[src*='/Images/Pages/ICO/SellMyCar/Articles/en-CA/ArticleImg-1.png']")]
            CertifyYourCarImage,
            [Description("img[src*='/Images/Pages/ICO/SellMyCar/Articles/en-CA/ArticleImg-2.png']")]
            GetCarPhotosImage,
            [Description("img[src*='/Images/Pages/ICO/SellMyCar/Articles/en-CA/ArticleImg-3.png']")]
            HowToCleanCarImage,
            [Description("img[src*='/Images/Pages/ICO/SellMyCar/Articles/en-CA/ArticleImg-4.png']")]
            VehicleResaleImage,
            [Description("#ymmtPanel #makes")]
            IcoMakeDropdown,
            [Description("#ymmtPanel #models")]
            IcoModelDropdown,
            [Description("#icoLicenseVin #vin")]
            IcoVinTextbox,
            [Description("#ymmtPanel #trims2")]
            IcoYmmtTrimDropdown,
            [Description("#icoLicenseVin #trims")]
            IcoVinTrimDropdown,
            [Description("#vinTrimPanel div.error.server-side")]
            IcoVinErrorServerSide,
            [Description("#ymmtPanel button.btn-primary")]
            IcoYmmtNextBasicDetailBtn,
            [Description("#icoLicenseVin button.btn-primary")]
            IcoVinNextBasicDetailBtn,
            [Description("#colours div[class='square']")]
            IcoVehicleColors,
            [Description("#mileageInput")]
            IcoVehicleKilometer,
            [Description("#mileage-postal input[name='postalCode']")]
            IcoVehiclePostalCode,
            [Description("#ownership input[name='isOriginalOwner'] + span")]
            IcoVehicleOwnershipAns,
            [Description("#ownership input[name='stillMakingPayments'] + span")]
            IcoVehiclePaymentsAns,
            [Description("#ownership input[name='isInterestedNewVehicle'] + span")]
            IcoVehicleReplacementAns,
            [Description("#icoVehicleDetails button.btn-primary")]
            IcoVehicleDetailsNextCondBtn,
            [Description("div.white-box-panel.text-center a")]
            IcoOrEnterYourVehicleMakeAndModelBtn,

            [Description("#wasInAnAccident input[name='wasInAnAccident'] + span")]
            IcoVehicleAccidentAns,
            [Description("#hasDamage input[name='hasDamage'] + span")]
            IcoVehicleDamageAns,
            [Description("#frontTiresSituation")]
            IcoFrontTireDropdown,
            [Description("#rearTiresSituation")]
            IcoRearTireDropdown,
            [Description("#hasMechanicalIssues input[name='hasMechanicalIssues'] + span")]
            IcoVehicleIssuesAns,
            [Description("#hasWarningLights input[name='hasWarningLights'] + span")]
            IcoVehicleLightsAns,
            [Description("#hasModifications input[name='hasModifications'] + span")]
            IcoVehicleModAns,
            [Description("#hasOdors input[name='hasOdors'] + span")]
            IcoVehicleOdorAns,
            [Description("#hasOtherIssues input[name='hasOtherIssues'] + span")]
            IcoVehicleOtherIssuesAns,
            [Description("#icoVehicleCondition button.btn-primary")]
            IcoVehicleCondNextContactBtn,

            [Description("#icoCustomerDetails input[name='firstName']")]
            IcoFirstName,
            [Description("#icoCustomerDetails input[name='lastName']")]
            IcoLastName,
            [Description("#icoCustomerDetails input[name='phone']")]
            IcoMobileNumber,
            [Description("#icoCustomerDetails span.error")]
            IcoCustomerDetailError,
            [Description("#icoCustomerDetails input[name='email']")]
            IcoEmail,
            [Description("#bestTimeToContact select[name='bestTimeToContact']")]
            IcoContactTimeDropdown,
            [Description("#userInterestQuestion select[name='userInterest']")]
            IcoSellWhenDropdown,
            [Description("#termsConditionsV2")]
            IcoTermsChkbox,
            [Description("#icoCustomerDetails button.btn-cd")]
            IcoNextIcoBtn,

            [Description("#icoVerifyPhoneByCodeSelectMethod button.btn-secondary")]
            IcoCodeMethodBtns,
            [Description("#verificationCode0")]
            IcoPhoneVerificationCode0,
            [Description("#verificationCode1")]
            IcoPhoneVerificationCode1,
            [Description("#verificationCode2")]
            IcoPhoneVerificationCode2,
            [Description("#verificationCode3")]
            IcoPhoneVerificationCode3,
            [Description("#checkCode")]
            IcoSubmitCodeBtn,

            [Description("#GaugeCashOffer")]
            IcoOfferSection,
            [Description("#ParticipatingDealers ico-certificate-dealer")]
            IcoOfferParticipatingDealers,
            [Description("#icoModal .modal-footer button.btn-primary")]
            IcoSuccessCloseBtn
        }

        public enum DWWLocators
        {
            //Vin Selection
            [Description("#icoFrame")]
            IcoFrame,
            [Description("#vehicle_vin")]
            IcoVinTextbox,
            [Description("#vehicle_style")]
            IcoVinTrimDropdown,
            [Description("button[name='submit']")]
            IcoNextBtn,

            //Vehicle Details
            [Description("span[style='background-color: rgb(227, 47, 67);']")]
            IcoVehicleColors,
            [Description("input[name='vehicle_color']")]
            SmallIcoVehicleColors,
            [Description("input[type='file']")]
            file,
            [Description("#vehicle_mileage")]
            IcoVehicleKilometer,
            [Description("#postal_code")]
            IcoVehiclePostalCode,
            [Description("input[name='is_original_owner'][value='false']")]
            IcoVehicleOwnershipAns,
            [Description("input[name='is_liened'][value='false']")]
            IcoVehiclePaymentsAns,
            [Description("input[name='is_interested_new_vehicle'][value='false']")]
            IcoVehicleReplacementAns,
            

            //Vehicle Condition 
            [Description("input[name='has_accident'][value='false']")]
            IcoVehicleAccidentAns,
            [Description("input[name='carfax_has_bad_vhr'][value='false']")]
            IcoVehicleHistoryAns,
            [Description("input[name='has_external_damage'][value='false']")]
            IcoVehicleDamageAns,
            [Description("#front_tire_age")]
            IcoTireDropdown,
            [Description("input[name='front_tire_age'][data-identifier='good']")]
            IcoTireCondition,
            [Description("input[name='has_mechanical_issues'][value='false']")]
            IcoVehicleIssuesAns,
            [Description("input[name='has_warning_lights'][value='false']")]
            IcoVehicleLightsAns,
            [Description("input[name='has_modifications'][value='false']")]
            IcoVehicleModAns,
            [Description("input[name='has_odor'][value='false']")]
            IcoVehicleOdorAns,
            [Description("input[name='has_other_issues'][value='false']")]
            IcoVehicleOtherIssuesAns,
           
            //Customer Details
            [Description("#firstName")]
            IcoFirstName,
            [Description("#lastName")]
            IcoLastName,
            [Description("#cellPhone")]
            IcoMobileNumber,
            [Description("#icoCustomerDetails span.error")]
            IcoCustomerDetailError,
            [Description("#email")]
            IcoEmail,
            [Description("#best_time_to_contact")]
            IcoContactTimeDropdown,
            [Description("#expect_transact_months")]
            IcoSellWhenDropdown,
            [Description("#prAgreement+span")]
            IcoTermsChkbox,
            

            [Description("//button[contains(text(),'ext')]")]
            IcoCodeMethodBtns,
            [Description("#codeOne")]
            IcoPhoneVerificationCode0,
            [Description("#codeTwo")]
            IcoPhoneVerificationCode1,
            [Description("#codeThree")]
            IcoPhoneVerificationCode2,
            [Description("#codeFour")]
            IcoPhoneVerificationCode3,
            

            [Description(".pr-offer-summary")]
            IcoOfferSection,
            [Description(".pr-offer-cert-contact-dealers")]
            IcoOfferParticipatingDealers,
            [Description("//button[.='Print']")]
            IcoPrintBtn
        }

    }
   

    internal class DescriptionTradeInAttribute : Attribute
    {
        public string DescriptionTradeIn { get; set; }

        public DescriptionTradeInAttribute(string value)
        {
            DescriptionTradeIn = value;
        }
    }

    internal class DescriptionBuyingCenterAttribute : Attribute
    {
        public string DescriptionBuyingCenter { get; set; }

        public DescriptionBuyingCenterAttribute(string value)
        {
            DescriptionBuyingCenter = value;
        }
    }

    internal class DescriptionDWWAttribute : Attribute
    {
        public string DescriptionDWW { get; set; }

        public DescriptionDWWAttribute(string value)
        {
            DescriptionDWW = value;
        }
    }
}
