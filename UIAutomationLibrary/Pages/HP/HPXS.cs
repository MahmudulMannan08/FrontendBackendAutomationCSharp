using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using MarketPlaceWeb.Pages.MyGarage;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.ComponentModel;

namespace MarketPlaceWeb.Pages.HP
{
    public class HPXS : HPAbstract
    {
        public HPXS(IWebDriver driver, Viewport viewport, Language language) : base(driver, viewport, language) { }

        #region SSO
        public override void LaunchSsoModal()
        {
            try
            {
                ClickElement(FindElement(By.CssSelector(HeaderFooterLocators.XSLocators.MyAccountToggleBtn.GetAttribute<DescriptionAttribute>().Description)));
            }
            catch (Exception)
            {
                GoToHomepage();
                ClickElement(FindElement(By.CssSelector(HeaderFooterLocators.XSLocators.MyAccountToggleBtn.GetAttribute<DescriptionAttribute>().Description)));
            }
            WaitForSubMenuDropdownStatus();
            ClickElement(FindElement(By.CssSelector(HeaderFooterLocators.XSLocators.MyAccountToggleBtn.GetAttribute<DescriptionSubMenuSignInAttribute>().DescriptionSubMenuSignIn)));
            WaitForSsoModalStatus();
        }

        public override void WaitForSubMenuDropdownStatus(bool isExpanded = true)
        {
            if (!isExpanded)
            {
                WaitUntil(() => !FindElement(By.CssSelector(HeaderFooterLocators.XSLocators.MyAccountToggleState.GetAttribute<DescriptionAttribute>().Description)).GetAttribute("class").Contains("menu-open"));
            }
            else
            {
                WaitUntil(() => FindElement(By.CssSelector(HeaderFooterLocators.XSLocators.MyAccountToggleState.GetAttribute<DescriptionAttribute>().Description)).GetAttribute("class").Contains("menu-open"));
            }
        }

        public override void NavigateToMyGarage()
        {
            try
            {
                ClickElement(FindElement(By.CssSelector(HeaderFooterLocators.XSLocators.MyAccountToggleBtn.GetAttribute<DescriptionAttribute>().Description)));
            }
            catch (Exception)
            {
              GoToHomepage();
              ClickElement(FindElement(By.CssSelector(HeaderFooterLocators.XSLocators.MyAccountToggleBtn.GetAttribute<DescriptionAttribute>().Description)));
            }

            WaitForSubMenuDropdownStatus();
            ClickElement(FindElement(By.CssSelector(HeaderFooterLocators.Menus.HomeSubMenuBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForPageLoad(60);
            WaitForMyGarageLoad();
        }

        public override void LoginToSsoAccount(SsoLogin.SsoAccountType ssoAccountType, string email, string password)
        {
            switch (ssoAccountType)
            {
                case SsoLogin.SsoAccountType.LocalAccount:
                    EnterText(By.CssSelector(HPLocators.SsoLoginModal.Email.GetAttribute<DescriptionAttribute>().Description), email);
                    EnterText(By.CssSelector(HPLocators.SsoLoginModal.Password.GetAttribute<DescriptionAttribute>().Description), password);
                    ClickElement(FindElement(By.CssSelector(HPLocators.SsoLoginModal.LoginBtn.GetAttribute<DescriptionAttribute>().Description)));

                    WaitForSsoModalStatus(false);
                    break;
                case SsoLogin.SsoAccountType.SocialAccountFB:
                    ScrollTo(FindElement(By.CssSelector(HPLocators.SsoLoginModal.SignInWithFB.GetAttribute<DescriptionAttribute>().Description)));
                    ClickElement(FindElement(By.CssSelector(HPLocators.SsoLoginModal.SignInWithFB.GetAttribute<DescriptionAttribute>().Description)));
                    WaitForUrlRedirection("facebook.com");
                    WaitForPageLoad(60);

                    EnterText(By.CssSelector(MyGarageLocators.LoginXS.FbUserName.GetAttribute<DescriptionAttribute>().Description), email);
                    EnterText(By.CssSelector(MyGarageLocators.LoginXS.FbPassword.GetAttribute<DescriptionAttribute>().Description), password);
                    ClickElement(FindElement(By.CssSelector(MyGarageLocators.LoginXS.FbLogin.GetAttribute<DescriptionAttribute>().Description)));

                    try
                    {
                        By fbContinueBtnLocator = By.CssSelector(MyGarageLocators.LoginXS.FbContinue.GetAttribute<DescriptionAttribute>().Description);
                        WaitForElementVisible(fbContinueBtnLocator);
                        ClickElement(FindElement(fbContinueBtnLocator));
                    }
                    catch (Exception)
                    {
                        //Do nothing if Continue step is not displayed on mobile
                    }

                    WaitForUrlRedirection(language.ToString() == "EN" ? "autotrader.ca" : "autohebdo.net");

                    WaitForSsoModalStatus(false);
                    break;
                case SsoLogin.SsoAccountType.SocialAccountGoogle:
                    //Out of scope
                    break;
                case SsoLogin.SsoAccountType.SocialAccountApple:
                    //Out of scope
                    break;
            }
        }

        public override void LogoutFromSsoAccount()
        {
            try
            {
                ClickElement(FindElement(By.CssSelector(HeaderFooterLocators.SmallLocators.MyAccountToggleBtn.GetAttribute<DescriptionAttribute>().Description)));
            }
            catch (Exception)
            {
                GoToHomepage();
                ClickElement(FindElement(By.CssSelector(HeaderFooterLocators.SmallLocators.MyAccountToggleBtn.GetAttribute<DescriptionAttribute>().Description)));
            }
            WaitForSubMenuDropdownStatus();
            IWebElement element = FindElement(By.CssSelector(HeaderFooterLocators.Menus.SignOutSubMenuBtn.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            ClickElement(element);
            
            WaitForSsoModalStatus(false);
        }
        #endregion

        #region PreQual
        internal override void ClickGlobalNavPreQual()
        {

            ClickElement(FindElement(By.CssSelector(HeaderFooterLocators.XSLocators.MyAccountToggleBtn.GetAttribute<DescriptionAttribute>().Description)));
            ClickElement(FindElement(By.CssSelector(PreQualLocators.PreQual.PreQualNavBar.GetAttribute<DescriptionAttribute>().Description)));
        }
        #endregion
    }
}
