using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using MarketPlaceWeb.Pages.MyGarage;
using MarketPlaceWeb.Pages.VDP;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using static UIAutomationLibrary.Locators.HubPagesLocators;

namespace MarketPlaceWeb.Pages.HP
{
    public class HPMain : Page
    {
        HPAbstract hPPage;
        private const string _defaultValue = null;

        public HPMain(IWebDriver driver, Viewport viewport, Language language)
        {
            base.driver = driver;
            base.viewport = viewport;
            base.language = language;
            switch (viewport)
            {
                case Viewport.Large:
                    hPPage = new HPLarge(driver, viewport, language);
                    break;
                case Viewport.XS:
                    hPPage = new HPXS(driver, viewport, language);
                    break;
                case Viewport.Small:
                    hPPage = new HPSmall(driver, viewport, language);
                    break;
            }
        }

        public void EnterLocation(string location, HPAbstract.Searchbox searchbox = HPAbstract.Searchbox.Old)
        {
            hPPage.EnterLocation(location, searchbox);
        }

        public void ClickOnSearch()
        {
            hPPage.ClickOnSearch();
        }

        public IList<IWebElement> GetFooterLinkElements()
        {
            return hPPage.GetFooterLinkElements();
        }

        #region SEO
        public void ClickOnBodyTypeLink(int link)
        {
            hPPage.ClickOnBodyTypeLink(link);
        }

        public bool IsHomePageSEOWidgetDisplaying(HPLocators.SEOWidgets widget)
        {
            return hPPage.IsHomePageSEOWidgetDisplaying(widget);
        }

        public void ClickSEOLink(HPLocators.SEOWidgets link)
        {
            hPPage.ClickSEOLink(link);
        }
        public string GetMMNameAwardWidget(HPLocators.SEOWidgets text)
        {
            return hPPage.GetMMNameAwardWidget(text);
        }

        public string GetTVWidgetTitle(HPLocators.SEOWidgets text)
        {
            return hPPage.GetTVWidgetTitle(text);
        }
        public void ClickATVSEOLink(int element)
        {
            hPPage.ClickATVSEOLink(element);
        }
        public void ClickEditorialLinks(int element, HPLocators.SEOWidgets editorialArticleLinks)
        {
            hPPage.ClickEditorialLinks(element, editorialArticleLinks);
        }
        
        public IList<IWebElement> LatestATVVideoLinks()
        {
            return hPPage.LatestATVVideoLinks();
        }
        public bool NavigateToHotOffTab(HPLocators.SEOWidgets tab)
        {
            return hPPage.NavigateToHotOffTab(tab);
        }
        public IList<IWebElement> EditoralArticleLinks(HPLocators.SEOWidgets text)
        {
            return hPPage.EditoralArticleLinks(text);
        }
        #endregion

        #region SSO
        public void ClickLogin() => hPPage.ClickLoginBtn();

        public bool? IsLoginFieldsVlaidationsFiredOnBlank() => hPPage.IsLoginFieldsVlaidationsFiredOnBlank();

        public bool? IsLoginBtnDisabledOnEmptyCredential() => hPPage.IsLoginBtnDisabledOnEmptyCredential();

        public string VerifyLoginFieldsValidationsForInvalidDetails() => hPPage.VerifyLoginFieldsValidationsForInvalidDetails();

        public void EnterSsoLoginEmailPassword(string email, string password) => hPPage.EnterSsoLoginEmailPassword(email, password);

        public bool? IsFaceBookLoginAvailable() => hPPage.IsFaceBookLoginAvailable();

        public bool? IsGoogleLoginAvailable() => hPPage.IsGoogleLoginAvailable();

        public bool? IsAppleLoginAvailable() => hPPage.IsAppleLoginAvailable();

        public bool? IsMyGarageSignedOut() => hPPage.IsMyGarageSignedOut();

        public bool IsThePasswordCorrect() => hPPage.IsThePasswordCorrect();

        public void LaunchSsoModal() => hPPage.LaunchSsoModal();

        public void LoginToSsoAccount(SsoLogin.SsoAccountType ssoAccountType, string email, string password) => hPPage.LoginToSsoAccount(ssoAccountType, email, password);

        public bool IsSsoLoginPageDisplayed() => hPPage.IsSsoLoginPageDisplayed();

        public void LogoutFromSsoAccount() => hPPage.LogoutFromSsoAccount();

        public bool IsSsoLoggedIn(bool isLoggedIn = true) => hPPage.IsSsoLoggedIn(isLoggedIn);

        public void NavigateToMyGarage() => hPPage.NavigateToMyGarage();

        public void RegisterNewSsoAccount(SsoRegistration ssoRegistration, Mailinator mailinator, bool deleteAllInboxMessages = true) => hPPage.RegisterNewSsoAccount(ssoRegistration, mailinator, deleteAllInboxMessages);

        public void RegisterSsoAccountWithResentCode(SsoRegistration ssoRegistration, Mailinator mailinator, bool deleteAllInboxMessages = true) => hPPage.RegisterSsoAccountWithResentCode(ssoRegistration, mailinator, deleteAllInboxMessages);

        public void RegisterSsoAccountWithChangedEmail(SsoRegistration ssoRegistration, Mailinator mailinator, bool deleteAllInboxMessages = true) => hPPage.RegisterSsoAccountWithChangedEmail(ssoRegistration, mailinator, deleteAllInboxMessages);

        public void ResetPasswordWithForgotPassword(SsoLogin ssoLogin, Mailinator mailinator) => hPPage.ResetPasswordWithForgotPassword(ssoLogin, mailinator);
        #endregion

        #region PreQual
        public void ClickGlobalNavPreQual() => hPPage.ClickGlobalNavPreQual();
        #endregion
    }
}
