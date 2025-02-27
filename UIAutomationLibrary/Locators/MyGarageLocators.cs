using System.ComponentModel;


namespace MarketPlaceWeb.Locators
{
    public class MyGarageLocators
    {
        public enum CommonLocators
        {
            [Description("app-root-my-garage img[src*='loading']")]
            MyGarageLoading,
            [Description("app-vehicle-page-card .name")]
            AllSavedVehicleTitles,
            [Description("app-vehicle-page-card")]
            AllSavedVehicleCards,
            [Description("saved-vehicles-page #deleteButton")]
            SavedSubscribedVehicleTabRemoveBtn,

            #region SavedSearch
            [Description(".header-wrapper .link")]
            HomeLinks,
            [Description(".page-container .title")]
            PageTitle,
            [Description(".searches .item-title")]
            SavedSearchItemTitle,
            [Description(".searches .item")]
            SavedSearchItem,
            [Description("following-sibling::*[@class='edit']")]
            SavedSearchEdit,
            [Description("app-edit-search")]
            EditSavedSearchModal,
            [Description("app-edit-search .name-input")]
            SavedSeachName,
            [Description("app-edit-search .save-button")]
            SavedSearchBtn,
            [Description(".toastContainer .toast")]
            Toaster,
            [Description(".toast .message")]
            ToasterMsg,
            [Description("app-edit-search .delete")]
            DeleteSavedSearchBtn,
            [Description(".toast .action")]
            UndoLink,
            [Description("//div[text()=' See all saved searches' or text()=' Voir tous les recherches enregistrées']")]
            SeeAllSavedSearchLink,
            #endregion

            #region SSO Login
            [Description("#identifierId")]
            GUserName,
            [Description("div.kHn9Lb")]
            GSignIn,
            [Description("#account_name_text_field")]
            AppleUserName,
            #endregion

            #region PreferenceCentre
            [Description("#divPreferenceCentre #btnSubmit")]
            PreferenceCentreUpdateBtn,
            [Description("#preference-centre-modal")]
            UnsubscribeModal,
            [Description("#preference-centre-modal #btnUnsubscribe")]
            UnsubscribeLink,
            [Description(".toaster .alert")]
            PreferenceCentreToaster,
            [Description(".toaster .message")]
            PreferenceCentreToasterMsg,
            #endregion
        }

        public enum MyGarageTabs
        {
            [Description("button.axle-tab")]
            AllTabs,
            [Description(".my-garage-tabs")]
            MenuContainer,
            [Description("Home"), DescriptionFrench("Accueil")]
            Home,
            [Description("My vehicles"), DescriptionFrench("Mes véhicules")]
            MyVehicles,
            [Description("Saved vehicles"), DescriptionFrench("Véhicules enregistrés")]
            SavedVehicles,
            [Description("Price alerts"), DescriptionFrench("Alertes de prix")]
            PriceAlerts,
            [Description("Saved searches"), DescriptionFrench("Recherches enregistrées")]
            SavedSearches,
            [Description("Manage ads"), DescriptionFrench("Gérer les annonces")]
            ManageAds,
            [Description("Account settings"), DescriptionFrench("Paramètres")]
            AccountSettings,
            [Description("Preference centre"), DescriptionFrench("Préférences")]
            PreferenceCentre
        }

        public enum AccountSettings
        {
            [Description("p.displayName")]
            DisplayName,
            [Description("span.personal-detail__header--edit")]
            EidtPersonalDetails,
            [Description("p.firstName")]
            FirstName,
            [Description("p.lastName")]
            LastName,
            [Description("p.PostalCode")]
            PostalCode,
            [Description("p.phone")]
            PhoneNumber,
            [Description("span.email-setting__header--edit")]
            AccountSettingsChangePassword,
            [Description("p.email")]
            Email,
            [Description("span.email-setting__header--edit")]
            ChangPassword,
            [Description("span.delete-account__action")]
            DeleteMyAccount,


            [Description("app-change-password")]
            ChangePasswordModal,
            [Description("app-delete-account")]
            DeleteSsoModal,
            [Description(".delete-account__delet-action button")]
            DeleteAccountBtn,
            [Description(".successful-delete__button button")]
            DeleteAccSuccessCloseBtn
        }

        public enum MyVehicles
        {
            #region AddVehicle
            [Description("app-my-vehicle app-empty-state")]
            EmptyStateDiv,
            [Description("#addVehicleButton button")]
            AddVehicleEmptyStateBtn,
            [Description("#vehicleCarousel .add-vehicle-card")]
            AddVehicleCarrouselBtn,
            [Description("app-my-vehicle app-filled-widget")]
            FilledStateDiv,
            [Description(".my-vehicle-error")]
            ErrorStateDiv,
            [Description("#vehicleCarousel .vehicle-card")]
            VehicleTabs,

            [Description("#myGarageModal")]
            AddVehicleModal,
            [Description("#myVehicleVinInput")]
            VinInputTxt,
            [Description("#yearDropdown .axle-dropdown")]
            YearDropdown,
            [Description("#yearDropdown .axle-dropdown .dropdown-icon")]
            YearDropdownIcon,
            [Description("#yearDropdown .dropdown-content")]
            YearDropdownContent,
            [Description("#makeDropdown .axle-dropdown")]
            MakeDropdown,
            [Description("#makeDropdown .axle-dropdown .dropdown-icon")]
            MakeDropdownIcon,
            [Description("#makeDropdown .dropdown-content")]
            MakeDropdownContent,
            [Description("#modelDropdown .axle-dropdown")]
            ModelDropdown,
            [Description("#modelDropdown .axle-dropdown .dropdown-icon")]
            ModelDropdownIcon,
            [Description("#modelDropdown .dropdown-content")]
            ModelDropdownContent,
            [Description("app-add-vin-ymm #nextButton button")]
            VinYmmNextBtn,
            
            [Description("app-add-trim-mileage")]
            AddTrimMileageModal,
            [Description("#trimDropdown .axle-dropdown")]
            TrimDropdown,
            [Description("#trimDropdown .axle-dropdown .dropdown-icon")]
            TrimDropdownIcon,
            [Description("#trimDropdown .dropdown-content")]
            TrimDropdownContent,
            [Description("#myVehicleVinInput")]
            KilometresInputTxt,
            [Description("app-add-trim-mileage #nextButton button")]
            TrimMileageNextBtn,

            [Description("app-add-color")]
            AddColorModal,
            [Description(".my-vehicle-colors-container")]
            ColorsContainerDiv,
            [Description(".color-square")]
            MyVehicleColors,
            [Description("#saveVehicleButton button")]
            SaveVehicleBtn,

            [Description("app-vehicle-saved")]
            MyVehicleSuccessModal,
            [Description("app-vehicle-saved #closeButton")]
            MyVehicleSuccessCloseBtn,
            #endregion

            #region EditVehicle
            [Description(".vehicle-content .edit-vehicle-link .axle-text-link")]
            EditVehicleLink,
            [Description(".vehicle-content .axle-text-link.large.normal.red")]
            DeleteVehicleLink,
            [Description(".edit-vehicle-modal #myGarageModal")]
            EditVehicleModal,
            [Description("#myGarageModal .delete-vehicle-section")]
            DeleteVehicleModal,

            [Description("#saveChangesButton button")]
            SaveVehicleChangesBtn,
            [Description(".delete-vehicle-section #deleteButton .axle-button")]
            DeleteVehicleBtn,
            [Description(".my-vehicle-colors-title")]
            MyVehicleColorTitle,
            #endregion

            #region VehicleDetail
            [Description("#vehicleDetails .vehicle-title")]
            VehicleTitle,
            [Description("#vehicleDetails .vehicle-trim .vehicle-details-value")]
            VehicleTrim,
            [Description("#vehicleDetails .vehicle-exterior-color .vehicle-details-value")]
            VehicleColor,
            [Description("#vehicleDetails .vehicle-mileage .vehicle-details-value")]
            VehicleMileage,
            [Description("#addVin .axle-text-link")]
            AddVinBtn,
            [Description("#vehicleDetails .vehicle-vin .vin-value")]
            VehicleVin,
            [Description("app-add-vin #addVinButton button")]
            AddVinNextBtn,
            #endregion
        }

        public enum EditAccountDetails
        {
            [Description("input[formcontrolname='firstName']")]
            FirstName,
            [Description("input[formcontrolname='lastName']")]
            LastName,
            [Description("input[formcontrolname='postalCode']")]
            PostCode,
            [Description("input[formcontrolname='phone']")]
            PhoneNumber,
            [Description("div.personal-detail__details--update button")]
            SaveChanges,
            [Description("#oldPassword")]
            Oldpassword,
            [Description("#newPassword")]
            NewPassword,
            [Description("#reenterPassword")]
            ConfirmNewPassword,
            [Description("#continue")]
            UpdatePassword,
            [Description("div.successful-update__button button")]
            CloseUpdateSuccessful,
            [Description("iframe[src='/Account/ChangePassword']")]
            iframe,
            [Description(".password-change-header")]
            PasswordUpdatedHeaderTxt,
            [Description(".change-password-successful-container #continue")]
            ClosePasswordUpdateSuccessful
        }

        public enum Home
        {
            [Description("div.title")]
            Title,
            [Description("//div[contains(text(),' See all price alerts') or contains(text(),'Voir toutes les alertes de prix')]")]
            SeeAllPriceAlerts,

            #region SavedSubscribedVehiclesWidget
            [Description("app-saved-vehicles-widget .name")]
            AllSavedVehicleTitles,
            [Description("app-saved-vehicles-widget .vehicle-card")]
            AllSavedVehicleCards,
            [Description("app-saved-vehicles-widget #deleteButton")]
            SavedSubscribedVehicleWidgetRemoveBtn,
            #endregion
        }

        public enum HomeXS
        {
            [Description("div.title")]
            Title,
            [Description("//div[text()='Price Alerts' or text()='Alertes de prix']/parent::div//div[text()=' See all' or text()=' Voir tout']")]
            SeeAllPriceAlerts
        }

        public enum HomeSmall
        {
            [Description("app-saved-vehicles-widget #deleteButton button")]
            SavedSubscribedVehicleWidgetRemoveBtn
        }

        public enum Login
        {
            [Description("#email")]
            FbUserName,
            [Description("#pass")]
            FbPassword,
            [Description("#loginbutton")]
            FbLogin
        }

        public enum LoginXS
        {
            [Description("#m_login_email")]
            FbUserName,
            [Description("#m_login_password")]
            FbPassword,
            [Description("#login_password_step_element > button")]
            FbLogin,
            [Description("button[value='Continue']")]
            FbContinue,


        }

        public enum LoginSM
        {
            [Description("#m_login_email"), DescriptionIOS("#email")]
            FbUserName,
            [DescriptionIOS("#pass"), Description("#m_login_password")]
            FbPassword,
            [Description("#login_password_step_element")]
            FbLogin
        }

        public enum SsoModal
        {
            [Description("#createAccount")]
            SignUpHereBtn
        }

        public enum RegisterSsoModal
        {
            [Description("#ssoModal #back-signin")]
            BackToLoginBtn,
            [Description("input#email")]
            EmailTxt,
            [Description("#emailVerificationControl_but_send_code")]
            SendVerificationCodeBtn,
            [Description("#emailVerificationControl_but_send_new_code")]
            ResendCodeBtn,
            [Description("#emailVerificationControl_success_message")]
            RegSsoModalSuccessMsg,
            [Description("#verificationCode")]
            VerificationCodeTxt,
            [Description("#emailVerificationControl_but_verify_code")]
            VerifyCodeBtn,
            [Description("#emailVerificationControl_but_change_claims")]
            ChangeEmailBtn,
            [Description("#continue")]
            ContinueBtn,
            [Description(".sign-up-successful-container #continue")]
            SignUpSuccessCloseBtn,
            [Description("#readonlyEmail")]
            EmailReadOnlyTxt,
            [Description("#newPassword")]
            NewPasswordTxt,
            [Description("#reenterPassword")]
            ConfirmPasswordTxt,
            [Description("#givenName")]
            FirstNameTxt,
            [Description("#surname")]
            LastNameTxt,
            [Description("#welcome-message")]
            WelcomeMsg,
            [Description("img[alt=\"update password verified\"]")]
            PasswordVerified,
            [Description("#login")]
            Login
        }

        public enum UpdatePassword
        {
            [Description("#emailVerificationControl-InvalidAccountDisallowed_but_send_code")]
            SendVerificationCodeBtn,
            [Description("#emailVerificationControl-InvalidAccountDisallowed_success_message")]
            SentCodeSucessMessage,
            [Description("#emailVerificationControl-InvalidAccountDisallowed_but_verify_code")]
            VerifyCodeBtn
        }

        public enum XSLocators
        {
            [Description("#best-deals-carousel-xs-exp img[alt='great deal']")]
            GreatPriceBadge,
            [Description("#best-deals-carousel-xs-exp img[alt='good deal']")]
            GoodPriceBadge,

            #region SavedSearch
            [Description("//div[text()='Saved Searches' or text()=' Voir tout']")]  //Won't work if price alert is also available on My Garage home
            SeeAllSavedSearchLabel,
            [Description("span .link")]
            SeeAllSavedSearchLink
            #endregion
        }

        public enum SmallLocators
        {
            [Description("saved-vehicles-page #deleteButton button")]
            SavedSubscribedVehicleTabRemoveBtn
        }

        public enum PreferenceCentreSettings
        {
            [Description("#divPreferenceCentre #ChkNewListings")]
            NewListings,
            [Description("#divPreferenceCentre #ChkPriceDrop")]
            PriceDrop,
            [Description("#divPreferenceCentre #ChkSimilarListings")]
            SimilarListings,
            [Description("#divPreferenceCentre #ChkNewsLetter")]
            Newsletter,
            [Description("#divPreferenceCentre #ChkNewProducts")]
            NewProducts,
            [Description("#divPreferenceCentre #ChkOffers")]
            OffersAndDiscounts,
            [Description("#divPreferenceCentre #ChkSurveys")]
            Surverys
        }
    }


}
