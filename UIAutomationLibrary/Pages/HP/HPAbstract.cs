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
using UIAutomationLibrary.Base.ClientApi3rdParty;

namespace MarketPlaceWeb.Pages.HP
{
    public abstract class HPAbstract : Page
    {
        protected HPAbstract(IWebDriver driver, Viewport viewport, Language language)
        {
            base.driver = driver;
            base.viewport = viewport;
            base.language = language;
        }

        public enum Searchbox
        {
            Old,
            New
        }

        public void EnterLocation(string location, Searchbox searchbox)
        {
            By locationSelector = (searchbox == Searchbox.Old) ? By.CssSelector(HPLocators.SearchCriteria.Location.GetAttribute<DescriptionAttribute>().Description) : By.CssSelector(HPLocators.SearchCriteria.LocationV2.GetAttribute<DescriptionAttribute>().Description);
            IWebElement locationField = FindElement(locationSelector);
            WaitForElementClickable(locationSelector, 60);
            ScrollTo(locationField, 3);
            EnterText(locationSelector, location);
        }

        public void ClickOnSearch()
        {
            IWebElement element = FindElement(By.CssSelector(HPLocators.SearchCriteria.SearchButton.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            ClickElement(element, 30);
            WaitForPageLoad(90);
            ReloadOnErrorDisplay(By.CssSelector(CommonLocators.ErrorPage.ErrorPage500.GetAttribute<System.ComponentModel.DescriptionAttribute>().Description));
        }

        public IList<IWebElement> GetFooterLinkElements()
        {
            WaitForPageLoad(10);
            ScrollToBottom();
            IList<IWebElement> links = FindElements(By.CssSelector(HPLocators.FooterLocators.FooterLinks.GetAttribute<DescriptionAttribute>().Description), 20, 1000);
            return links;
        }

        #region SEO
        public bool IsHomePageSEOWidgetDisplaying(HPLocators.SEOWidgets widget)
        {
            return IsElementVisible(By.CssSelector(widget.GetAttribute<DescriptionAttribute>().Description));
        }
        public string GetMMNameAwardWidget(HPLocators.SEOWidgets text)
        {
            return GetElementText(driver.FindElement(By.CssSelector((text.GetAttribute<DescriptionAttribute>().Description))));
        }
        public void ClickSEOLink(HPLocators.SEOWidgets link)
        {
            ClickElement(FindElement(By.CssSelector(link.GetAttribute<DescriptionAttribute>().Description)));
        }

        public void ClickOnBodyTypeLink(int link)
        {
            IList<IWebElement> BodyTypeSRPLinks = FindElements(By.CssSelector(HPLocators.SEOWidgets.BodyTypeSRPLink.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(BodyTypeSRPLinks[link]);
            WaitForPageLoad(90);
        }

        public string GetTVWidgetTitle(HPLocators.SEOWidgets text)
        {
            return GetElementText(driver.FindElement(By.CssSelector((text.GetAttribute<DescriptionAttribute>().Description))));
        }
        public void ClickATVSEOLink(int link)
        {
            IList<IWebElement> ATTVLinks = FindElements(By.CssSelector(HPLocators.SEOWidgets.ATSeeAllLink.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(ATTVLinks[link]);
            WaitForPageLoad(90);
        }
        public void ClickEditorialLinks(int link, HPLocators.SEOWidgets editorialArticleLinks)
        {
            IList<IWebElement> editorialLinks = FindElements(By.CssSelector(editorialArticleLinks.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(editorialLinks[link]);
            WaitForPageLoad(90);
        }
        public IList<IWebElement> LatestATVVideoLinks()
        {
            WaitForPageLoad(10);
            ScrollToBottom();
            IList<IWebElement> links = FindElements(By.CssSelector(HPLocators.SEOWidgets.ATVideoLinks.GetAttribute<DescriptionAttribute>().Description));
            return links;
        }
        public IList<IWebElement> EditoralArticleLinks(HPLocators.SEOWidgets text)
        {
            WaitForPageLoad(10);
            ScrollToBottom();
            IList<IWebElement> links = FindElements(By.CssSelector(text.GetAttribute<DescriptionAttribute>().Description));
            return links;
        }
        public bool NavigateToHotOffTab(HPLocators.SEOWidgets tab)
        {
            IWebElement labelElement = driver.FindElement(By.CssSelector(tab.GetAttribute<DescriptionAttribute>().Description));
            // Execute JavaScript to get the computed style of ::after
            string script = "return window.getComputedStyle(arguments[0], ':after').getPropertyValue('border-bottom');";
            string borderBottomStyle = (string)((IJavaScriptExecutor)driver).ExecuteScript(script, labelElement);
            return borderBottomStyle.Contains("rgb(237, 28, 38)");
        }

        #endregion

        #region SSO
        public void ClickLoginBtn()
        {
            ClickElement(FindElement(By.CssSelector(HPLocators.SsoLoginModal.LoginBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForPageLoad(60);
        }

        public bool IsLoginBtnDisabledOnEmptyCredential()
        {
            IWebElement element = FindElement(By.CssSelector(HPLocators.SsoLoginModal.LoginBtn.GetAttribute<DescriptionAttribute>().Description));
            return element.Displayed && !element.Enabled;
        }

        #region PreQual

        internal virtual void ClickGlobalNavPreQual()
        {
            WaitForElementVisible(By.CssSelector(PreQualLocators.PreQual.PreQualNavBar.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(FindElement(By.CssSelector(PreQualLocators.PreQual.PreQualNavBar.GetAttribute<DescriptionAttribute>().Description)));
        }
        #endregion
        public bool IsLoginFieldsVlaidationsFiredOnBlank()
        {
            try
            {
                WaitForElementVisible(By.CssSelector(HPLocators.SsoLoginModal.ErrorNoUserName.GetAttribute<DescriptionAttribute>().Description), 5);
                WaitForElementVisible(By.CssSelector(HPLocators.SsoLoginModal.ErrorNoPassword.GetAttribute<DescriptionAttribute>().Description), 5);
                return true;
            }
            catch (Exception) { return false; }
        }

        public bool? IsFaceBookLoginAvailable()
        {
            try
            {
                WaitForElementClickable(By.CssSelector(HPLocators.SsoLoginModal.SignInWithFB.GetAttribute<DescriptionAttribute>().Description));
                return true;
            }
            catch (Exception) { return false; }
        }

        public bool? IsGoogleLoginAvailable()
        {
            try
            {
                WaitForElementClickable(By.CssSelector(HPLocators.SsoLoginModal.SignInWithGoogle.GetAttribute<DescriptionAttribute>().Description));
                return true;
            }
            catch (Exception) { return false; }
        }

        public bool? IsAppleLoginAvailable()
        {
            try
            {
                WaitForElementClickable(By.CssSelector(HPLocators.SsoLoginModal.SignInWithApple.GetAttribute<DescriptionAttribute>().Description));
                return true;
            }
            catch (Exception) { return false; }
        }

        public bool IsThePasswordCorrect()
        {
            try
            {
                WaitForElementVisible(By.CssSelector(HPLocators.SsoLoginModal.ErrorInvalidUser.GetAttribute<DescriptionAttribute>().Description), 15);
                return false;
            }
            catch (Exception) { return true; }
        }

        public bool IsMyGarageSignedOut()
        {
            try
            {
                WaitForSsoLogout();
                return true;
            }
            catch (Exception) { return false; }
        }

        public void EnterSsoLoginEmailPassword(string email, string password)
        {
            EnterText(By.CssSelector(HPLocators.SsoLoginModal.Email.GetAttribute<DescriptionAttribute>().Description), email);
            EnterText(By.CssSelector(HPLocators.SsoLoginModal.Password.GetAttribute<DescriptionAttribute>().Description), password);
        }

        public string VerifyLoginFieldsValidationsForInvalidDetails()
        {
            try
            {
                WaitForElementVisible(By.CssSelector(HPLocators.SsoLoginModal.ErrorInvalidUser.GetAttribute<DescriptionAttribute>().Description));
                return GetElementText(FindElement(By.CssSelector(HPLocators.SsoLoginModal.ErrorInvalidUser.GetAttribute<DescriptionAttribute>().Description)));
            }
            catch (Exception) { return null; }
        }

        public abstract void LaunchSsoModal();

        public abstract void WaitForSubMenuDropdownStatus(bool isExpanded = true);

        public abstract void NavigateToMyGarage();

        public abstract void LoginToSsoAccount(SsoLogin.SsoAccountType ssoAccountType, string email, string password);

        public abstract void LogoutFromSsoAccount();

        internal void GoToHomepage()
        {
            IWebElement element = FindElement(By.CssSelector(HeaderFooterLocators.Header.Logo.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            try
            {
                ClickElement(element);
            }
            catch (Exception)
            {
                RefreshPage();  //Sometimes feedback pop up appears
                element = FindElement(By.CssSelector(HeaderFooterLocators.Header.Logo.GetAttribute<DescriptionAttribute>().Description));
                ScrollTo(element);
                ClickElement(element);
            }
            WaitForPageLoad(90);
        }

        public void WaitForSsoModalStatus(bool isDisplayed = true)
        {
            if (!isDisplayed)
            {
                WaitForPageLoad(60);
                WaitForElementVisible(By.CssSelector(HeaderFooterLocators.Menus.MenuNavPanel.GetAttribute<DescriptionAttribute>().Description), 30);
            }
            else
            {
                WaitForPageLoad(60);
                WaitForElementVisible(By.CssSelector(HPLocators.SsoLoginModal.Email.GetAttribute<DescriptionAttribute>().Description));
            }
        }

        public bool IsSsoLoggedIn(bool isLoggedIn = true)
        {
            if (!isLoggedIn)
            {
                try
                {
                    WaitForElementToExistInDOM(By.CssSelector(HeaderFooterLocators.Menus.SsoLoggedInState.GetAttribute<DescriptionAttribute>().Description));  //Logged in state element is not interactable, but exists on DOM
                    return false;
                }
                catch (Exception) { return true; }
            }
            else
            {
                try
                {
                    WaitForElementToExistInDOM(By.CssSelector(HeaderFooterLocators.Menus.SsoLoggedInState.GetAttribute<DescriptionAttribute>().Description));  //Logged in state element is not interactable, but exists on DOM
                    return true;
                }
                catch (Exception) { return false; }
            }
        }

        public void WaitForSsoLogout()
        {
            WaitUntil(() => IsSsoLoggedIn(false), 30);
        }

        public bool IsSsoLoginPageDisplayed()
        {
            try
            {
                WaitForSsoModalStatus();
                return true;
            }
            catch (Exception) { return false; }
        }

        public void WaitForMyGarageDropdownStatus(bool isExpanded = true)
        {
            if (!isExpanded)
            {
                WaitUntil(() => !FindElement(By.CssSelector(HeaderFooterLocators.Menus.MyGarageToggleBtn.GetAttribute<DescriptionAttribute>().Description)).GetAttribute("class").Contains("open"));
            }
            else
            {
                WaitUntil(() => FindElement(By.CssSelector(HeaderFooterLocators.Menus.MyGarageToggleBtn.GetAttribute<DescriptionAttribute>().Description)).GetAttribute("class").Contains("open"));
            }
        }

        public void WaitForMyGarageLoad()
        {
            WaitForAllElementsNotVisible(By.CssSelector(MyGarageLocators.CommonLocators.MyGarageLoading.GetAttribute<DescriptionAttribute>().Description));
        }

        public void RegisterNewSsoAccount(SsoRegistration ssoRegistration, Mailinator mailinator, bool deleteAllInboxMessages = true)
        {
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.SsoModal.SignUpHereBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForPageLoad(30);

            WaitForElementVisible(By.CssSelector(MyGarageLocators.RegisterSsoModal.EmailTxt.GetAttribute<DescriptionAttribute>().Description));
            EnterText(By.CssSelector(MyGarageLocators.RegisterSsoModal.EmailTxt.GetAttribute<DescriptionAttribute>().Description), ssoRegistration.NewSsoAccountEmail);
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.SendVerificationCodeBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForElementVisible(By.CssSelector(MyGarageLocators.RegisterSsoModal.RegSsoModalSuccessMsg.GetAttribute<DescriptionAttribute>().Description));

            MailinatorApi mailinatorApi = new MailinatorApi(driver, mailinator.MailinatorApiToken);
            string verificationCode = mailinatorApi.GetSsoVerificationCode(mailinator.MailinatorPrivateDomainName, mailinator.MailinatorPrivateInboxName);

            EnterText(By.CssSelector(MyGarageLocators.RegisterSsoModal.VerificationCodeTxt.GetAttribute<DescriptionAttribute>().Description), !string.IsNullOrEmpty(verificationCode) ? verificationCode : throw new Exception("Could not retrieve email verification code for Sso registration"));
            if (viewport == Viewport.XS)
            {
                UnFocusElementJS(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.VerificationCodeTxt.GetAttribute<DescriptionAttribute>().Description)), 3);
            }
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.VerifyCodeBtn.GetAttribute<DescriptionAttribute>().Description)));

            WaitForElementVisible(By.CssSelector(MyGarageLocators.RegisterSsoModal.ContinueBtn.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.ContinueBtn.GetAttribute<DescriptionAttribute>().Description)), 30);
            WaitForPageLoad(30);

            WaitForElementVisible(By.CssSelector(MyGarageLocators.RegisterSsoModal.EmailReadOnlyTxt.GetAttribute<DescriptionAttribute>().Description));
            EnterText(By.CssSelector(MyGarageLocators.RegisterSsoModal.NewPasswordTxt.GetAttribute<DescriptionAttribute>().Description), ssoRegistration.NewSsoAccountPassword);
            EnterText(By.CssSelector(MyGarageLocators.RegisterSsoModal.ConfirmPasswordTxt.GetAttribute<DescriptionAttribute>().Description), ssoRegistration.NewSsoAccountPassword);
            EnterText(By.CssSelector(MyGarageLocators.RegisterSsoModal.FirstNameTxt.GetAttribute<DescriptionAttribute>().Description), ssoRegistration.NewSsoAccountFirstName);
            EnterText(By.CssSelector(MyGarageLocators.RegisterSsoModal.LastNameTxt.GetAttribute<DescriptionAttribute>().Description), ssoRegistration.NewSsoAccountLastName);
            if (viewport == Viewport.XS)
            {
                UnFocusElementJS(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.LastNameTxt.GetAttribute<DescriptionAttribute>().Description)), 3);
            }
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.ContinueBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForPageLoad(90);

            WaitForElementVisible(By.CssSelector(MyGarageLocators.RegisterSsoModal.SignUpSuccessCloseBtn.GetAttribute<DescriptionAttribute>().Description), 60);
            Wait(2); //Temporary static wait added till issue is addressed: https://trader.atlassian.net/browse/CONS-9621
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.SignUpSuccessCloseBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForSsoModalStatus(false);

            if (deleteAllInboxMessages)
            {
                _ = mailinatorApi.DeleteAllInboxMessagesAsync(mailinator.MailinatorPrivateDomainName, mailinator.MailinatorPrivateInboxName);
            }
        }

        public void RegisterSsoAccountWithResentCode(SsoRegistration ssoRegistration, Mailinator mailinator, bool deleteAllInboxMessages = true)
        {
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.SsoModal.SignUpHereBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForPageLoad(30);

            WaitForElementVisible(By.CssSelector(MyGarageLocators.RegisterSsoModal.EmailTxt.GetAttribute<DescriptionAttribute>().Description));
            EnterText(By.CssSelector(MyGarageLocators.RegisterSsoModal.EmailTxt.GetAttribute<DescriptionAttribute>().Description), ssoRegistration.NewSsoAccountEmail);
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.SendVerificationCodeBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForElementVisible(By.CssSelector(MyGarageLocators.RegisterSsoModal.RegSsoModalSuccessMsg.GetAttribute<DescriptionAttribute>().Description));

            MailinatorApi mailinatorApi = new MailinatorApi(driver, mailinator.MailinatorApiToken);
            string verificationCode = mailinatorApi.GetSsoVerificationCode(mailinator.MailinatorPrivateDomainName, mailinator.MailinatorPrivateInboxName);
            if (!string.IsNullOrEmpty(verificationCode))
            {
                Wait(5);
                Task.Run(() => mailinatorApi.DeleteAllInboxMessagesAsync(mailinator.MailinatorPrivateDomainName, mailinator.MailinatorPrivateInboxName)).GetAwaiter().GetResult();  //Call Synchronously
            }
            else { throw new Exception("Could not retrieve email verification code for Sso registration"); }

            EnterText(By.CssSelector(MyGarageLocators.RegisterSsoModal.VerificationCodeTxt.GetAttribute<DescriptionAttribute>().Description), verificationCode);
            if (viewport == Viewport.XS)
            {
                UnFocusElementJS(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.VerificationCodeTxt.GetAttribute<DescriptionAttribute>().Description)), 3);
            }
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.ResendCodeBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForElementVisible(By.CssSelector(MyGarageLocators.RegisterSsoModal.RegSsoModalSuccessMsg.GetAttribute<DescriptionAttribute>().Description));

            string resentVerificationCode = mailinatorApi.GetSsoVerificationCode(mailinator.MailinatorPrivateDomainName, mailinator.MailinatorPrivateInboxName);

            EnterText(By.CssSelector(MyGarageLocators.RegisterSsoModal.VerificationCodeTxt.GetAttribute<DescriptionAttribute>().Description), !string.IsNullOrEmpty(resentVerificationCode) ? resentVerificationCode : throw new Exception("Could not retrieve re-sent email verification code 2 for Sso registration"));
            if (viewport == Viewport.XS)
            {
                UnFocusElementJS(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.VerificationCodeTxt.GetAttribute<DescriptionAttribute>().Description)), 3);
            }
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.VerifyCodeBtn.GetAttribute<DescriptionAttribute>().Description)));

            WaitForElementVisible(By.CssSelector(MyGarageLocators.RegisterSsoModal.ContinueBtn.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.ContinueBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForPageLoad(30);

            WaitForElementVisible(By.CssSelector(MyGarageLocators.RegisterSsoModal.EmailReadOnlyTxt.GetAttribute<DescriptionAttribute>().Description));
            EnterText(By.CssSelector(MyGarageLocators.RegisterSsoModal.NewPasswordTxt.GetAttribute<DescriptionAttribute>().Description), ssoRegistration.NewSsoAccountPassword);
            EnterText(By.CssSelector(MyGarageLocators.RegisterSsoModal.ConfirmPasswordTxt.GetAttribute<DescriptionAttribute>().Description), ssoRegistration.NewSsoAccountPassword);
            EnterText(By.CssSelector(MyGarageLocators.RegisterSsoModal.FirstNameTxt.GetAttribute<DescriptionAttribute>().Description), ssoRegistration.NewSsoAccountFirstName);
            EnterText(By.CssSelector(MyGarageLocators.RegisterSsoModal.LastNameTxt.GetAttribute<DescriptionAttribute>().Description), ssoRegistration.NewSsoAccountLastName);
            if (viewport == Viewport.XS)
            {
                UnFocusElementJS(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.LastNameTxt.GetAttribute<DescriptionAttribute>().Description)), 3);
            }
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.ContinueBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForPageLoad(90);

            WaitForElementVisible(By.CssSelector(MyGarageLocators.RegisterSsoModal.SignUpSuccessCloseBtn.GetAttribute<DescriptionAttribute>().Description), 60);
            Wait(2); //Temporary static wait added till issue is addressed: https://trader.atlassian.net/browse/CONS-9621
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.SignUpSuccessCloseBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForSsoModalStatus(false);

            if (deleteAllInboxMessages)
            {
                _ = mailinatorApi.DeleteAllInboxMessagesAsync(mailinator.MailinatorPrivateDomainName, mailinator.MailinatorPrivateInboxName);
            }
        }

        public void RegisterSsoAccountWithChangedEmail(SsoRegistration ssoRegistration, Mailinator mailinator, bool deleteAllInboxMessages = true)
        {
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.SsoModal.SignUpHereBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForPageLoad(30);

            WaitForElementVisible(By.CssSelector(MyGarageLocators.RegisterSsoModal.EmailTxt.GetAttribute<DescriptionAttribute>().Description));
            EnterText(By.CssSelector(MyGarageLocators.RegisterSsoModal.EmailTxt.GetAttribute<DescriptionAttribute>().Description), ssoRegistration.NewSsoAccountEmail);
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.SendVerificationCodeBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForElementVisible(By.CssSelector(MyGarageLocators.RegisterSsoModal.RegSsoModalSuccessMsg.GetAttribute<DescriptionAttribute>().Description));

            MailinatorApi mailinatorApi = new MailinatorApi(driver, mailinator.MailinatorApiToken);
            string verificationCode = mailinatorApi.GetSsoVerificationCode(mailinator.MailinatorPrivateDomainName, mailinator.MailinatorPrivateInboxName);
            if (!string.IsNullOrEmpty(verificationCode))
            {
                Wait(5);
                Task.Run(() => mailinatorApi.DeleteAllInboxMessagesAsync(mailinator.MailinatorPrivateDomainName, mailinator.MailinatorPrivateInboxName)).GetAwaiter().GetResult();  //Call Synchronously
            }
            else { throw new Exception("Could not retrieve email verification code for Sso registration"); }

            EnterText(By.CssSelector(MyGarageLocators.RegisterSsoModal.VerificationCodeTxt.GetAttribute<DescriptionAttribute>().Description), verificationCode);
            if (viewport == Viewport.XS)
            {
                UnFocusElementJS(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.VerificationCodeTxt.GetAttribute<DescriptionAttribute>().Description)), 3);
            }
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.VerifyCodeBtn.GetAttribute<DescriptionAttribute>().Description)));

            WaitForElementVisible(By.CssSelector(MyGarageLocators.RegisterSsoModal.ContinueBtn.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.ChangeEmailBtn.GetAttribute<DescriptionAttribute>().Description)));

            WaitForElementVisible(By.CssSelector(MyGarageLocators.RegisterSsoModal.EmailTxt.GetAttribute<DescriptionAttribute>().Description));
            EnterText(By.CssSelector(MyGarageLocators.RegisterSsoModal.EmailTxt.GetAttribute<DescriptionAttribute>().Description), ssoRegistration.NewSsoAccountEmail_2);
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.SendVerificationCodeBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForElementVisible(By.CssSelector(MyGarageLocators.RegisterSsoModal.RegSsoModalSuccessMsg.GetAttribute<DescriptionAttribute>().Description));

            string verificationCode_2 = mailinatorApi.GetSsoVerificationCode(mailinator.MailinatorPrivateDomainName, mailinator.MailinatorPrivateInboxName_2);

            EnterText(By.CssSelector(MyGarageLocators.RegisterSsoModal.VerificationCodeTxt.GetAttribute<DescriptionAttribute>().Description), !string.IsNullOrEmpty(verificationCode_2) ? verificationCode_2 : throw new Exception("Could not retrieve email verification code 2 for Sso registration"));
            if (viewport == Viewport.XS)
            {
                UnFocusElementJS(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.VerificationCodeTxt.GetAttribute<DescriptionAttribute>().Description)), 3);
            }
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.VerifyCodeBtn.GetAttribute<DescriptionAttribute>().Description)));

            WaitForElementVisible(By.CssSelector(MyGarageLocators.RegisterSsoModal.ContinueBtn.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.ContinueBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForPageLoad(30);

            WaitForElementVisible(By.CssSelector(MyGarageLocators.RegisterSsoModal.EmailReadOnlyTxt.GetAttribute<DescriptionAttribute>().Description), 20);
            EnterText(By.CssSelector(MyGarageLocators.RegisterSsoModal.NewPasswordTxt.GetAttribute<DescriptionAttribute>().Description), ssoRegistration.NewSsoAccountPassword);
            EnterText(By.CssSelector(MyGarageLocators.RegisterSsoModal.ConfirmPasswordTxt.GetAttribute<DescriptionAttribute>().Description), ssoRegistration.NewSsoAccountPassword);
            EnterText(By.CssSelector(MyGarageLocators.RegisterSsoModal.FirstNameTxt.GetAttribute<DescriptionAttribute>().Description), ssoRegistration.NewSsoAccountFirstName);
            EnterText(By.CssSelector(MyGarageLocators.RegisterSsoModal.LastNameTxt.GetAttribute<DescriptionAttribute>().Description), ssoRegistration.NewSsoAccountLastName);
            if (viewport == Viewport.XS)
            {
                UnFocusElementJS(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.LastNameTxt.GetAttribute<DescriptionAttribute>().Description)), 3);
            }
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.ContinueBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForPageLoad(90);

            WaitForElementVisible(By.CssSelector(MyGarageLocators.RegisterSsoModal.SignUpSuccessCloseBtn.GetAttribute<DescriptionAttribute>().Description), 60);
            Wait(2); //Temporary static wait added till issue is addressed: https://trader.atlassian.net/browse/CONS-9621
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.SignUpSuccessCloseBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForSsoModalStatus(false);

            if (deleteAllInboxMessages)
            {
                _ = mailinatorApi.DeleteAllInboxMessagesAsync(mailinator.MailinatorPrivateDomainName, mailinator.MailinatorPrivateInboxName);
            }
        }

        public void ResetPasswordWithForgotPassword(SsoLogin ssoLogin, Mailinator mailinator)
        {
            MailinatorApi mailinatorApi = new MailinatorApi(driver, mailinator.MailinatorApiToken);
            Task.Run(() => mailinatorApi.DeleteAllInboxMessagesAsync(mailinator.MailinatorPrivateDomainName, mailinator.MailinatorPrivateInboxName)).GetAwaiter().GetResult();  //Call Synchronously

            ClickElement(FindElement(By.CssSelector(HPLocators.SsoLoginModal.ForgotYourPassword.GetAttribute<DescriptionAttribute>().Description)));
            WaitForPageLoad(30);
            WaitForElementVisible(By.CssSelector(MyGarageLocators.RegisterSsoModal.EmailTxt.GetAttribute<DescriptionAttribute>().Description));

            EnterText(By.CssSelector(MyGarageLocators.RegisterSsoModal.EmailTxt.GetAttribute<DescriptionAttribute>().Description), ssoLogin.LocalAccountEmail);
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.UpdatePassword.SendVerificationCodeBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForElementVisible(By.CssSelector(MyGarageLocators.UpdatePassword.SentCodeSucessMessage.GetAttribute<DescriptionAttribute>().Description));

            var dict = mailinatorApi.GetSsoVerificationCodeAndMsgID(mailinator.MailinatorPrivateDomainName, mailinator.MailinatorPrivateInboxName);

            EnterText(By.CssSelector(MyGarageLocators.RegisterSsoModal.VerificationCodeTxt.GetAttribute<DescriptionAttribute>().Description), !string.IsNullOrEmpty(dict["Code"]) ? dict["Code"] : throw new Exception("Could not retrieve email verification code for Sso registration"));
            if (viewport == Viewport.XS)
            {
                UnFocusElementJS(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.VerificationCodeTxt.GetAttribute<DescriptionAttribute>().Description)), 3);
            }
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.UpdatePassword.VerifyCodeBtn.GetAttribute<DescriptionAttribute>().Description)));

            WaitForElementVisible(By.CssSelector(MyGarageLocators.RegisterSsoModal.ContinueBtn.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.ContinueBtn.GetAttribute<DescriptionAttribute>().Description)));

            WaitForElementVisible(By.CssSelector(MyGarageLocators.RegisterSsoModal.NewPasswordTxt.GetAttribute<DescriptionAttribute>().Description), 30);
            EnterText(By.CssSelector(MyGarageLocators.RegisterSsoModal.NewPasswordTxt.GetAttribute<DescriptionAttribute>().Description), ssoLogin.UpdatedPassword);
            EnterText(By.CssSelector(MyGarageLocators.RegisterSsoModal.ConfirmPasswordTxt.GetAttribute<DescriptionAttribute>().Description), ssoLogin.UpdatedPassword);
            if (viewport == Viewport.XS)
            {
                UnFocusElementJS(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.ConfirmPasswordTxt.GetAttribute<DescriptionAttribute>().Description)), 3);
            }
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.ContinueBtn.GetAttribute<DescriptionAttribute>().Description)));

            WaitForElementVisible(By.CssSelector(MyGarageLocators.RegisterSsoModal.PasswordVerified.GetAttribute<DescriptionAttribute>().Description), 60);
            ClickElement(FindElement(By.CssSelector(MyGarageLocators.RegisterSsoModal.Login.GetAttribute<DescriptionAttribute>().Description)));
            WaitForSsoModalStatus();
        }

        #endregion
    }
}

