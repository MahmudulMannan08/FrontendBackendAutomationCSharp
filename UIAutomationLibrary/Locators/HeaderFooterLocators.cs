using System;
using System.ComponentModel;

namespace MarketPlaceWeb.Locators
{
    public class HeaderFooterLocators
    {
        public enum Categories
        {
            [Description("#desktopMicrosites a[href='/']"), DescriptionFrench("#desktopMicrosites a[href='/']"), DescriptionMobile("#mobileMicrosites a[href='/']"), DescriptionMobileFrench("#mobileMicrosites a[href='/']")]
            CarsTrucksSuv,
            [Description("#desktopMicrosites a[href='/heavy_trucks']"), DescriptionFrench("#desktopMicrosites a[href='/camion_lourd']"), DescriptionMobile("#mobileMicrosites a[href='/heavy_trucks']"), DescriptionMobileFrench("#mobileMicrosites a[href='/camion_lourd']")]
            Commercial,
            [Description("#desktopMicrosites a[href='/trailers']"), DescriptionFrench("#desktopMicrosites a[href='/remorques']"), DescriptionMobile("#mobileMicrosites a[href='/trailers']"), DescriptionMobileFrench("#mobileMicrosites a[href='/remorques']")]
            Trailers,
            [Description("#desktopMicrosites a[href='/rv']"), DescriptionFrench("#desktopMicrosites a[href='/vr']"), DescriptionMobile("#mobileMicrosites a[href='/rv']"), DescriptionMobileFrench("#mobileMicrosites a[href='/vr']")]
            RVs,
            [Description("#desktopMicrosites a[href='/boats']"), DescriptionFrench("#desktopMicrosites a[href='/bateau']"), DescriptionMobile("#mobileMicrosites a[href='/boats']"), DescriptionMobileFrench("#mobileMicrosites a[href='/bateau']")]
            Boats,
            [Description("#desktopMicrosites a[href='/watercraft']"), DescriptionFrench("#desktopMicrosites a[href='/motomarine']"), DescriptionMobile("#mobileMicrosites a[href='/watercraft']"), DescriptionMobileFrench("#mobileMicrosites a[href='/motomarine']")]
            Watercraft,
            [Description("#desktopMicrosites a[href='/motorcycles_atvs']"), DescriptionFrench("#desktopMicrosites a[href='/moto_vtt']"), DescriptionMobile("#mobileMicrosites a[href='/motorcycles_atvs']"), DescriptionMobileFrench("#mobileMicrosites a[href='/moto_vtt']")]
            Bikes,
            [Description("#desktopMicrosites a[href='/snowmobiles']"), DescriptionFrench("#desktopMicrosites a[href='/motoneige']"), DescriptionMobile("#mobileMicrosites a[href='/snowmobiles']"), DescriptionMobileFrench("#mobileMicrosites a[href='/motoneige']")]
            Snowmobiles,
            [Description("#desktopMicrosites a[href='/heavy_equipment']"), DescriptionFrench("#desktopMicrosites a[href='/equipment_lourd']"), DescriptionMobile("#mobileMicrosites a[href='/heavy_equipment']"), DescriptionMobileFrench("#mobileMicrosites a[href='/equipment_lourd']")]
            HeavyEquipment,
            [Description("#desktopMicrosites a[href='/farm_equipment']"), DescriptionFrench("#desktopMicrosites a[href='/equipment_agricole']"), DescriptionMobile("#mobileMicrosites a[href='/farm_equipment']"), DescriptionMobileFrench("#mobileMicrosites a[href='/equipment_agricole']")]
            Farm,
            [DescriptionMobile("#mobileMicrosites a[href='#']")]
            CategoryDropDownBtn
        }

        public enum Menus
        {
            [Description("#navbarPrimary.menu-open")]
            MenuOpen,
            [Description("#mainNav a[href='/buy-car-online']"), DescriptionFrench("#mainNav a[href='/acheter-en-ligne']")]
            BuyOnline,
            [Description("#mainNav a[href='/home-delivery/']"), DescriptionFrench("#mainNav a[href='/livraison-à-domicile/']")]
            HomeDelivery,
            [Description("#mainNav a[href='/ico/sellmycar']")]
            SellMyCar,
            [Description("#mainNav li.shop-new-cars a.dropdown-toggle")]
            ShopNewCars,
            [Description("#mainNav a[href*='electri']")]
            ShopElectric,
            [Description("#mainNav li.newsReviews a.dropdown-toggle")]
            ReviewsAndAdvice,
            [Description("#mainNav a[href='/ico/valuations/']")]
            WhatsMyCarWorth,
            [Description("#signin-dropwdown a.dropdown-toggle"), DescriptionSubMenuSignIn("a#lnkSignIn")]
            SignIn,
            [Description("#languageSwitch")]
            LanguageToggle,
            [Description("a[href='/my-garage/']")]
            HomeSubMenuBtn,
            [Description("#navbarLogout")]
            SignOutSubMenuBtn,

            [Description(".my-account .dropdown-toggle"), DescriptionSubMenuSignIn("#lnkSignIn")]
            MyAccountToggleBtn,
            [Description(".my-account")]
            MyAccountToggleState,
            [Description("#myGarageDropdown")]
            MyGarageToggleBtn,
            [Description("#navbarPrimary")]
            MenuNavPanel,
            [Description(".my-account .dropdown-menu.logged-in")]
            SsoLoggedInState
        }

       

        public enum Header
        {
            [Description("#logo")]
            Logo
        }

        public enum Footer
        {
            [Description("#aboutUsLinkContainer")]
            AboutUsContainer,
            [Description("#dealerServicesLinkContainer")]
            DealerServicesContainer,
            [Description("#resourcesLinkContainer")]
            ResoursesContainer,
            [Description("#partnersLinkContainer")]
            PartnersContainer,
            [Description("#customerSupportLinkContainer")]
            CustomerSupportContainer,
            [Description(".footer-logo")]
            LogoFooter,
            [Description("#hlnkSocialFacebook")]
            FacebookBadge,
            [Description("#hlnkSocialTwitter")]
            TwitterBadge,
            [Description("#hlnkSocialYouTube")]
            YoutubeBadge,
            [Description(".app-store-badges .app-badge.android-badge")]
            AndroidAppBadge,
            [Description(".app-store-badges .app-badge.apple-badge")]
            AppleAppBadge,
            [Description("#lblCopyright")]
            CopyRightLbl
        }

        public enum XSLocators
        {
            [Description("#mobileMicrosites i.far.fa-chevron-down-header")]
            CategoryDropdown,
            [Description("a#lnkSignIn")]
            SignIn,                        
            [Description("#myGarageDropdown .dropdown-toggle")]
            MyGarageToggleBtn,
            [Description("a[href='/Account/Signout']")]
            SignOut,            
            [Description("#mobileMicrosites div.btn-group")]
            CategoryDropdownStatusLocator,

            [Description("#mainNavToggleContainer"), DescriptionSubMenuSignIn("#lnkSignIn")]
            MyAccountToggleBtn,
            [Description("#navbarPrimary")]
            MyAccountToggleState
        }

        public enum SmallLocators
        {
            [Description("#mobileMicrosites i.far.fa-chevron-down-header")]
            CategoryDropdown,
            [Description("a#lnkSignIn")]
            SignIn,
            [Description("#myGarageDropdown a")]
            MyGarageToggleBtn,
            [Description("a[href='/Account/Signout']")]
            SignOut,            
            [Description("#mobileMicrosites div.btn-group")]
            CategoryDropdownStatusLocator,

            [Description("#mainNavToggleContainer"), DescriptionSubMenuSignIn("#lnkSignIn")]
            MyAccountToggleBtn,
            [Description("#navbarPrimary")]
            MyAccountToggleState,
            [Description("#navbarLogout a")]
            SignOutSubMenuBtn
        }
    }

    internal class DescriptionMobileAttribute : Attribute
    {
        public string DescriptionMobile { get; set; }

        public DescriptionMobileAttribute(string value)
        {
            DescriptionMobile = value;
        }
    }

    internal class DescriptionMobileFrenchAttribute : Attribute
    {
        public string DescriptionMobileFrench { get; set; }

        public DescriptionMobileFrenchAttribute(string value)
        {
            DescriptionMobileFrench = value;
        }
    }

    internal class DescriptionSubMenuSignInAttribute : Attribute
    {
        public string DescriptionSubMenuSignIn { get; set; }
        public DescriptionSubMenuSignInAttribute(string value)
        {
            DescriptionSubMenuSignIn = value;
        }
    }
}