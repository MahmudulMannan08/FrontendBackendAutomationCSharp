using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using MarketPlaceWeb.Locators;
using System.Text;
using System.Collections.ObjectModel;
using System.Net;
using System.Text.RegularExpressions;

namespace MarketPlaceWeb.Base
{
    public class Page
    {
        public IWebDriver driver;
        public Viewport viewport;
        public Uri url;
        public Language language;
        public string srpVariant;
        public string boostVariant;
        public string hpVariant;

        public IWebDriver Driver
        {
            get
            {
                return driver;
            }
            set
            {
                driver = value;
            }
        }

        public Viewport Viewport
        {
            get
            {
                return viewport;
            }
            set
            {
                viewport = value;
            }
        }

        public Language Language
        {
            get
            {
                return language;
            }
            set
            {
                language = value;
            }
        }

        public Uri URL
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
            }
        }

        public string SRPVariant
        {
            get
            {
                return srpVariant;
            }
        }

        public void Open()
        {
            try
            {
                driver.Navigate().GoToUrl(url);  //This statement has internal timeout of 60s after which returns exception. Completion of this statement does not mean page fully loaded.
                if (Viewport == Viewport.Small)  //Small view starts with Appium page
                {
                    WaitUntil(() => driver.Url.Contains(url.GetLeftPart(UriPartial.Path)), 45);
                }
                WaitForPageLoad(120);
            }
            catch (Exception)
            {
                WaitForPageLoad(120);  //After internal timeout(60s) exception still keep waiting for page to load and don't throw exception.
            }
            finally
            {
                ReloadOnErrorDisplay(By.CssSelector(CommonLocators.ErrorPage.ErrorPage500.GetAttribute<System.ComponentModel.DescriptionAttribute>().Description));
                if (url.ToString().Contains("go"))
                {
                    if (driver.Url.Contains("adId="))
                        throw new Exception("Ad is no longer Online " + url.ToString());
                }
                CloseCookieBanner();
            }
        }

        public void ReloadOnErrorDisplay(By locator, int errorPageReloadTrialCountMax = 30)
        {
            int errorPageReloadTrialCount = 0;
            while (IsElementVisible(locator) && errorPageReloadTrialCount < errorPageReloadTrialCountMax)
            {
                if (errorPageReloadTrialCount == errorPageReloadTrialCountMax)
                {
                    throw new Exception("Site is not accessible, error page displayed");
                }
                RefreshPage();
                errorPageReloadTrialCount++;
            }
        }

        public void CloseCookieBanner()
        {
            driver.FindElements(By.CssSelector(CommonLocators.CookieBanner.closeBtn.GetAttribute<System.ComponentModel.DescriptionAttribute>().Description)).FirstOrDefault(x => x.Displayed)?.Click();
        }

        public bool IsSurveyCampaignDialogDisplayed()
        {
            try
            {
                InteractOnIframe(By.CssSelector(CommonLocators.SurveyCampaignModal.SurveyIframe.GetAttribute<System.ComponentModel.DescriptionAttribute>().Description),
                () => WaitForElementVisible(By.CssSelector(CommonLocators.SurveyCampaignModal.CloseBtn.GetAttribute<System.ComponentModel.DescriptionAttribute>().Description)));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void CloseSurveyCampaignDialog()
        {
            if (IsSurveyCampaignDialogDisplayed())
            {
                InteractOnIframe(By.CssSelector(CommonLocators.SurveyCampaignModal.SurveyIframe.GetAttribute<System.ComponentModel.DescriptionAttribute>().Description),
                () => ClickElement(FindElement(By.CssSelector(CommonLocators.SurveyCampaignModal.CloseBtn.GetAttribute<System.ComponentModel.DescriptionAttribute>().Description))), 15);
                WaitForElementNotVisible(By.CssSelector(CommonLocators.SurveyCampaignModal.SurveyForm.GetAttribute<System.ComponentModel.DescriptionAttribute>().Description), 15);
            }
        }

        /// <summary>
        /// Wait for splash page to disappear if available.
        /// </summary>
        /// <param name="timeOut">Timeout in seconds for the wait.</param>
        public void WaitForSplashPage(int timeOut = 10)
        {
            if (IsElementAvailable(By.CssSelector(CommonLocators.SplashPage.splashPage.GetAttribute<System.ComponentModel.DescriptionAttribute>().Description)))
            {
                WaitForElementNotVisible(By.CssSelector(CommonLocators.SplashPage.splashPage.GetAttribute<System.ComponentModel.DescriptionAttribute>().Description), timeOut);
            }
        }

        public bool IsInCurrentUrl(string value)
        {
            return !driver.Url.Contains(value) ? WebUtility.UrlDecode(driver.Url).Contains(value.ToLower()) : driver.Url.Contains(value);
        }

        public void WaitForUrlRedirection(string url, int timeOut = 60)
        {
            WaitUntil(() => new Uri(driver.Url).GetLeftPart(UriPartial.Path).ToUpperInvariant().Contains(url.ToUpperInvariant()), timeOut);
        }

        public void ClickElement(IWebElement element, int timeOut = 10, int interval = 500)
        {
            if (!IsElementEnabled(element))
            {
                WaitUntilElementIsEnabled(element, timeOut, interval);
            }
            try
            {
               element.Click();
            }
            catch (ElementClickInterceptedException)
            {
                CloseSurveyCampaignDialog();
                element.Click();
            }
            catch (StaleElementReferenceException)
            {
                //do nothing
                //Current web driver throwing StaleElementReferenceException after completing click sometimes. Setting this empty block till this issue is fixed by Selenium org.
            }
            catch (Exception)
            {
                try
                {
                    ScrollTo(element, 2);
                    element.Click();
                }
                catch (StaleElementReferenceException)
                {
                    //do nothing
                    //Current web driver throwing StaleElementReferenceException after completing click sometimes. Setting this empty block till this issue is fixed by Selenium org.
                }
            }
        }

        public bool IsElementEnabled(IWebElement element)
        {
            try
            {
                return element.Enabled;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void WaitUntilElementIsEnabled(IWebElement element, int timeOut = 10, int interval = 500)
        {
            bool enabled = fluentWait(timeOut, interval).Until(x => (element.Enabled));
            if (!enabled)
            {
                throw new Exception("Element " + element + " is not enabled \n\n");
            }
        }


        public void ClickElementJS(string cssSelector)
        {
            string query = "document.querySelector('" + cssSelector + "').click()";
            ((IJavaScriptExecutor)driver).ExecuteScript(query);
        }

        public void ClickElementJS(IWebElement webElement, int waitTimeInSeconds = 3)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", webElement);
            Wait(waitTimeInSeconds);
        }

        public void UnFocusElementJS(IWebElement element, int waitTimeInSeconds = 1)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].blur();", element);
            Wait(waitTimeInSeconds);
        }

        public void FocusElementJS(IWebElement element, int waitTimeInSeconds = 1)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].focus();", element);
            Wait(waitTimeInSeconds);
        }

        public string GetElementText(IWebElement element)   
        {
            try
            {
                string text = element.Text;
                return !string.IsNullOrEmpty(text) ? text.Trim() : string.Empty;
            }
            catch (Exception e)
            {
                throw new Exception("Element" + element + " not found \n\n" + e.Message);
            }
        }

        public void EnterText(By locator, string text, int timeOut = 10)
        {
            IWebElement element = FindElement(locator, timeOut: timeOut);
            element.Clear();
            element.SendKeys(text);
        }

        public void EnterTextUsingJS(By locator, string text)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript($"arguments[0].value = '{text}';", driver.FindElement(locator));
        }

        public void EnterTextWithDelay(By locator, string text, int timeOut = 10)
        {
            IWebElement element = FindElement(locator, timeOut: timeOut);
            element.Clear();
            List<string> textCharList = text.Select(c => c.ToString()).ToList();
            foreach (var textChar in textCharList)
            {
                element.SendKeys(textChar);
                Thread.Sleep(100);
            }
        }

        public string GetSubstringBeforeCharacter(string text, char ch)
        {
            return !string.IsNullOrEmpty(text) == true ? text.IndexOf(ch) != -1 ? text.Substring(0, text.IndexOf(ch)) : text : string.Empty;
        }

        public string GetSubstringAfterCharacter(string text, char ch)
        {
            return !string.IsNullOrEmpty(text) == true ? text.IndexOf(ch) != -1 ? text.Substring(text.LastIndexOf(ch) + 1) : text : string.Empty;
        }

        public void ClickElementByText(String tag, String text, int timeOut = 10)
        {
            By elementByText = By.XPath($"//{tag}[text() = '{text}']");
            ClickElement(FindElement(elementByText, timeOut: timeOut));
        }

        public string GetElementAttribute(By element, string attribute, int timeOut = 10)
        {
            return FindElement(element, timeOut: timeOut).GetAttribute(attribute);
        }

        public void SlideLeft(IWebElement element)
        {
            if (DriverType() == "iOS")
            {
                Dictionary<string, string> swipeLeft = new Dictionary<string, string>();
                swipeLeft.Add("direction", "left");
                try
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("mobile:swipe", swipeLeft);
                }
                catch (Exception)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("mobile:swipe", swipeLeft);
                }
            }
            else
            {
                Actions action = (new Actions(driver));
                action.MoveToElement(element, 100 + element.Location.X + element.Size.Width / 2, element.Location.Y + element.Size.Height / 2).ClickAndHold();
                Wait(2);
                action.MoveToElement(element, -100 + element.Location.X + element.Size.Width / 2, element.Location.Y + element.Size.Height / 2).Release();
                action.Build().Perform();
                Wait(1);
            }
        }

        /// <summary>
        /// Use this method to swipe left from source element to destination element. Selenium 4.1 new feature for Actions class. IOS devices work with existing ScrollTo implementation.
        /// </summary>
        /// <param name="fromElement">Source element. For left swipe, the source element should be on the right of destination element.</param>
        /// <param name="toElement">Destination element to swipe to.</param>
        public void SwipeLeft(IWebElement fromElement, IWebElement toElement)
        {
            try
            {
                Actions action = (new Actions(driver));
                action.ClickAndHold(fromElement).Release(toElement).Build().Perform();
                Wait(2);
            }
            catch (Exception)
            {
                ScrollTo(fromElement);
            }
        }

        /// <summary>
        /// This method is used to put wait.
        /// </summary>
        /// <param name="waitTimeInSecond">Wait in seconds</param>
        public void Wait(int waitTimeInSecond)
        {
            Thread.Sleep(waitTimeInSecond * 1000);
        }

        private DefaultWait<IWebDriver> fluentWait(int timeOut, int interval)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(driver);
            wait.Timeout = TimeSpan.FromSeconds(timeOut);
            wait.PollingInterval = TimeSpan.FromMilliseconds(interval);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            return wait;
        }

        public void WaitUntilDialogDisplayed(By locator, int timeout)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                wait.Until(d =>
                {
                    return driver.FindElements(locator).Count != 0;
                });
            }
            catch (Exception e)
            {
                throw new Exception("Modal dialog not found \n" + e.Message);
            }

        }

        public void WaitUntilDialogClosed(By locator, int timeout)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                wait.Until(d =>
                {
                    return driver.FindElements(locator).Count == 0;
                });

            }
            catch (Exception e)
            {
                throw new Exception("Modal dialog is still open \n" + e.Message);
            }
        }

        /// <summary>
        /// This method is used to wait until page is ready. Use only on visible page load occurances. Won't be useful if there is no page load instance.
        /// </summary>
        /// <param name="timeOut">Timeout in seconds for the wait.</param>
        public void WaitForPageLoad(int timeOut)
        {
            string state = string.Empty;
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut));

                //Checks every 500 ms whether predicate returns true otherwise keep trying till it returns ture
                wait.Until(d =>
                {

                    try
                    {
                        state = ((IJavaScriptExecutor)driver).ExecuteScript(@"return document.readyState").ToString();
                    }
                    catch (InvalidOperationException)
                    {
                        //Ignore
                    }
                    catch (NoSuchWindowException)
                    {
                        //when popup is closed, switch to base window, if that's only window
                        if (driver.WindowHandles.Count == 1)
                        {
                            driver.SwitchTo().Window(driver.WindowHandles[0]);
                        }
                        else
                        {
                            //when popup is closed, switch to last window
                            driver.SwitchTo().Window(driver.WindowHandles.Last());
                        }
                    }
                    //In IE7 there are chances we may get state as loaded instead of complete
                    return (state.Equals("complete", StringComparison.InvariantCultureIgnoreCase) || state.Equals("loaded", StringComparison.InvariantCultureIgnoreCase));

                });
            }
            catch (TimeoutException e)
            {
                //sometimes Page remains in Interactive mode and never becomes Complete, then we can still try to access the controls
                if (!state.Equals("interactive", StringComparison.InvariantCultureIgnoreCase))
                    throw new Exception("Exception occurred while waiting for page to load: " + e.Message);
            }
            catch (NullReferenceException e)
            {
                //sometimes Page remains in Interactive mode and never becomes Complete, then we can still try to access the controls
                if (!state.Equals("interactive", StringComparison.InvariantCultureIgnoreCase))
                    throw new Exception("Exception occurred while waiting for page to load: " + e.Message);
            }
            catch (WebDriverException e)
            {
                if (driver.WindowHandles.Count == 1)
                {
                    driver.SwitchTo().Window(driver.WindowHandles[0]);
                }
                state = ((IJavaScriptExecutor)driver).ExecuteScript(@"return document.readyState").ToString();
                if (!(state.Equals("complete", StringComparison.InvariantCultureIgnoreCase) || state.Equals("loaded", StringComparison.InvariantCultureIgnoreCase)))
                    throw new Exception("Exception occurred while waiting for page to load: " + e.Message);
            }
            catch (Exception e)
            {
                throw new Exception("Exception occurred while waiting for page to load: " + e.Message);
            }
        }

        /// <summary>
        /// This method is used to fluently wait until supplied condition is met. Waits till condition returns neither null or false.
        /// </summary>
        /// <param name="timeOut">Timeout in seconds for the wait.</param>
        /// <param name="interval">Polling interval in Milliseconds for the fluent wait, verifies condition after each poll till condition returns neither null or false</param>
        protected T WaitUntil<T>(Func<T> condition, int timeOut = 10, int interval = 100)
        {
            return fluentWait(timeOut, interval).Until(x => condition());
        }

        /// <summary>
        /// This method is used to fluently wait until element is displayed or until element is displayed and clickable.
        /// </summary>
        /// <param name="timeOut">Timeout in seconds for the wait, if need to use without providing isClickable parameter value you can use like 'timeOut : 20' as parameter (targeted parameter without maintaining the serial)</param>
        /// <param name="interval">Polling interval in Milliseconds for the fluent wait, verifies condition after each poll till condition returns neither null or false</param>
        public IWebElement FindElement(By locator, int timeOut = 10, int interval = 500)
        {
            try
            {
                return fluentWait(timeOut, interval).Until(x => x.FindElement(locator));
            }
            catch (Exception e)
            {
                if (e.InnerException.Message.Contains("no such element") || e.InnerException.Message.Contains("An element could not be located"))
                {
                    return null;
                }
                else
                {
                    throw new Exception("Element" + locator + " not found \n\n" + e.Message);
                }
            }
        }

        /// <summary>
        /// Verifies element availability on DOM.
        /// </summary>
        public bool IsElementAvailable(By locator) { return driver.FindElements(locator).Count > 0; }

        /// <summary>
        /// Verifies element is available on DOM and visible (not hidden)
        /// </summary>
        public bool IsElementVisible(By locator)
        {
            if (driver.FindElements(locator).Count > 0)
            {
                try
                {
                    return driver.FindElement(locator).Displayed;
                }
                catch (Exception) { return false; }
            }
            return false;
        }

        public void WaitForElementNotVisible(By locator, int timeOut = 10, int interval = 300)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut));
                wait.PollingInterval = TimeSpan.FromMilliseconds(interval);
                wait.Timeout = TimeSpan.FromSeconds(timeOut);
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(locator));
            }
            catch (Exception) { }
        }

        public void WaitForAllElementsNotVisible(By locator, int timeOut = 60, int interval = 300)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut));
                wait.PollingInterval = TimeSpan.FromMilliseconds(interval);
                wait.Timeout = TimeSpan.FromSeconds(timeOut);
                wait.Until(InvisibilityOfAllElementsLocatedBy(locator));
            }
            catch (Exception) { }
        }

        internal Func<IWebDriver, ReadOnlyCollection<IWebElement>> InvisibilityOfAllElementsLocatedBy(By locator)
        {
            return (driver) =>
            {
                try
                {
                    var elements = driver.FindElements(locator);
                    return elements.Any(element => element.Displayed) ? null : elements;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            };
        }

        public void WaitForElementVisible(By locator, int timeOut = 30, int interval = 300)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut));
                wait.PollingInterval = TimeSpan.FromMilliseconds(interval);
                wait.Timeout = TimeSpan.FromSeconds(timeOut);
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
            }
            catch (Exception e)
            {
                throw new Exception("Exception occurred while waiting for element to be visible: " + e.Message);
            }
        }

        public void WaitForElementToExistInDOM(By locator, int timeOut = 10, int interval = 300)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut));
                wait.PollingInterval = TimeSpan.FromMilliseconds(interval);
                wait.Timeout = TimeSpan.FromSeconds(timeOut);
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(locator));
            }
            catch (Exception e)
            {
                throw new Exception("Exception occurred while waiting for element to become available in DOM: " + e.Message);
            }
        }

        public void WaitForElementClickable(By locator, int timeOut = 10, int interval = 300)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut));
                wait.PollingInterval = TimeSpan.FromMilliseconds(interval);
                wait.Timeout = TimeSpan.FromSeconds(timeOut);
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
            }
            catch (Exception e)
            {
                throw new Exception("Exception occurred while waiting for element to be displayed and enabled: " + e.Message);
            }
        }

        /// <summary>
        /// Switch to iframe by locator, and perform action methods sent as parameter. Finally it switches back to default frame. 
        /// </summary>
        /// <param name="locator">locator for the iframe</param>
        /// <param name="interaction">Action type parameter method, to interact within iframe elements</param>
        /// <param name="timeOut">Timeout in seconds for the wait until frame switch</param>
        /// <param name="interval">Polling interval in Milliseconds for the fluent wait, verifies condition after each poll till condition returns neither null or false</param>
        public void InteractOnIframe(By locator, Action interaction, int timeOut = 10, int interval = 300)
        {
            bool isFrameSwitchSuccess = new bool();
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut));
                wait.PollingInterval = TimeSpan.FromMilliseconds(interval);
                wait.Timeout = TimeSpan.FromSeconds(timeOut);
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.FrameToBeAvailableAndSwitchToIt(locator));
                isFrameSwitchSuccess = true;

                interaction();
            }
            catch (Exception e)
            {
                throw new Exception("Exception occurred while switching to iFrame: " + e.Message);
            }

            finally
            {
                if (isFrameSwitchSuccess)
                {
                    driver.SwitchTo().DefaultContent();
                }
            }
        }

        /// <summary>
        /// Find list of elements by matching locator. Fluently waits till atleast an element from list is available. For elements that is considered 'Not Displayed' use this methid with .FirstOrDefault(). For finding targetted/matching single element from list use this method with .FirstOrDefault(x => condition()), replace condition with method as required.
        /// </summary>
        /// <param name="timeOut">Timeout in seconds for the wait.</param>
        /// <param name="interval">Polling interval in Milliseconds for the fluent wait, verifies condition after each poll till condition returns neither null or false</param>
        public IList<IWebElement> FindElements(By locator, int timeOut = 10, int interval = 500)
        {
            try
            {
                return fluentWait(timeOut, interval).Until(x => x.FindElements(locator).Count > 0 ? x.FindElements(locator) : null);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Use this method to select combobox / dropbox by value. Combobox / dropbox won't be expanded for this selection.
        /// </summary>
        /// <param name="locator">locator for the combobox / dropbox element.</param>
        /// <param name="value">Value for element to be selected (not text).</param>
        public void SelectByValue(By locator, string value)
        {
            SelectElement selectElement = new SelectElement(FindElement(locator));
            WaitUntil(() => selectElement.Options.Count > 0);
            selectElement.SelectByValue(value);
        }
        public void SelectByText(By locator, string value)
        {
            SelectElement selectElement = new SelectElement(FindElement(locator));
            WaitUntil(() => selectElement.Options.Count > 0);
            selectElement.SelectByText(value, true);
        }

        public void SelectByValuePartialMatch(By locator, string valuePartialMatch)
        {
            SelectElement selectElement = new SelectElement(FindElement(locator));
            WaitUntil(() => selectElement.Options.Count > 0);
            selectElement.SelectByValue(selectElement.Options.FirstOrDefault(x => x.GetAttribute("value").ToLower().Contains(valuePartialMatch.ToLower())).GetAttribute("value"));
        }

        public void SelectFirstOption(By locator)
        {
            SelectElement selectElement = new SelectElement(FindElement(locator));
            WaitUntil(() => selectElement.Options.Count > 0);
            selectElement.SelectByText(selectElement.Options.FirstOrDefault(x => x.Text.Length > 0).Text);
        }

        /// <summary>
        /// Finds currently selected text option of a combobox / dropbox. Will return first option text if multiple selected.
        /// </summary>
        /// <param name="locator">locator for the combobox / dropbox element.</param>
        public string GetSelectedText(By locator)
        {
            SelectElement selectElement = new SelectElement(FindElement(locator));
            WaitUntil(() => selectElement.Options.Count > 0);
            return selectElement.SelectedOption.Text.Trim();
        }

        /// <summary>
        /// Returns count of elements from the list which has text. Excludes count of elements without text. Will return zero if there is none.
        /// </summary>
        /// <param name="collection">List of type IWebElement</param>
        protected int CountElements(IList<IWebElement> collection)
        {
            int count = 0;
            if (collection == null) { return 0; }
            count += collection.Where(item => item.Text.Length > 0).Count();
            return count;
        }

        /// <summary>
        /// Returns count of elements from the list which is displayed. Will return zero if there is none.
        /// </summary>
        /// <param name="collection">List of type IWebElement</param>
        protected int CountVisibleElements(IList<IWebElement> collection)
        {
            int count = 0;
            if (collection == null) { return 0; }
            count += collection.Where(item => item.Displayed).Count();
            return count;
        }

        public void RefreshPage()
        {
            try
            {
                driver.Navigate().Refresh();  //This statement has internal timeout of 60s after which returns exception. Completion of this statement does not mean page fully loaded.
                WaitForPageLoad(90);
            }
            catch (Exception)
            {
                WaitForPageLoad(90);  //After internal timeout(60s) exception still keep waiting for page to load and don't throw exception.
            }
        }
        /// <summary>
        ///navigate back from current URL in same tab 
        /// </summary>
        public void Back()
        {
            try
            {
                driver.Navigate().Back();  
                WaitForPageLoad(90);
            }
            catch (Exception)
            {
                WaitForPageLoad(90);
            }
        }

        /// <summary>
        /// Switch to different window or tab based on Func<bool> type parameter condition, which verifies condition on different tab/window to switch. 
        /// </summary>
        /// <param name="condition">Func<bool> type parameter condition which is verified on different tab/window except parent.</param>
        /// <param name="timeOut">Timeout in seconds for the wait.</param>
        public void SwitchTabOrWindow(Func<bool> condition, int timeOut = 60)
        {
            string currentWindow = null;
            try
            {
                var windowHandles = driver.WindowHandles;
                if (windowHandles.Count > 0)
                {
                    currentWindow = driver.CurrentWindowHandle;
                    if (currentWindow == null)
                    {
                        throw new Exception("No window available");
                    }

                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut));

                    wait.Until<bool>(d =>
                    {
                        foreach (var handle in windowHandles)
                        {
                            if (handle == currentWindow)
                            {
                                continue;
                            }
                            driver.SwitchTo().Window(handle);
                            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);  //Wait for page to load
                            if (condition())
                            {
                                return true;
                            }
                        }
                        return false;
                    });
                }
            }
            catch (Exception e)
            {
                throw new Exception("Exception occurred while trying to switch tab or window: " + e.Message);
            }
        }

        public void SwitchToBaseWindow()
        {
            try
            {
                driver.SwitchTo().Window(driver.WindowHandles[0]);
            }
            catch (Exception e)
            {
                throw new Exception("Exception occurred while trying to switch to base window: " + e.Message);
            }
        }

        public string DriverType()
        {
            string type = driver.GetType().ToString();
            return (type.Contains("Android")) ? "Android" : (type.Contains("iOS")) ? "iOS" : null;
        }

        public void ScrollTo(IWebElement element, int waitTimeInSecond = 1)
        {
            try
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", element);
                Wait(waitTimeInSecond);
            }
            catch (StaleElementReferenceException)
            {
                //do nothing
                //Current web driver throwing StaleElementReferenceException on element sometimes. Setting this empty block till this issue is fixed by Selenium org.
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void ScrollToBottom()
        {
            try
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0,document.body.scrollHeight)");
                Wait(1);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void ScrollToTop()
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0,0)");
            Wait(1);
        }

        public void ScrollCount(int number)
        {
            int count = 0;
            while (count < number)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0,600)");
                count += 1;
            }
        }

        public void ClearField(IWebElement element)
        {
            // This is temporary function because the Keys.Control + "A" and Keys.Delete do not work in BrowserStack.
            while (!string.IsNullOrEmpty(element.GetAttribute("value")))
            {
                element.SendKeys(Keys.Backspace);
            }
        }

        public void TakeScreenshot(string testName)
        {
            if (driver != null)
            {

                string path = Path.GetFullPath(
                                    Path.Combine(
                                        AppDomain.CurrentDomain.BaseDirectory, @"..\..")) + "/Screenshots/";
                if (System.IO.Directory.Exists(path) == false)
                {
                    if (!string.IsNullOrEmpty(path) && path.IndexOfAny(Path.GetInvalidPathChars()) == -1)
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    else
                    {
                        throw new Exception("Path has invalid characters: " + path);
                    }
                }

                string fileName = testName + DateTime.Now.ToString() + ".jpg";
                fileName = fileName.Replace("/", "_").Replace(":", "_").Replace(" ", "_");

                string screenshotPath = path + fileName;
                if (string.IsNullOrEmpty(screenshotPath) || screenshotPath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                {
                    throw new Exception("Screenshot path has invalid characters: " + screenshotPath);
                }

                try
                {
                    var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                    screenshot.SaveAsFile(screenshotPath);
                    TestContext.AddTestAttachment(screenshotPath);
                }
                catch (Exception e)
                {
                    throw new Exception("Can not take a screenshot " + e.Message);
                }
            }
        }

        public bool IsCheckboxChecked(IWebElement element)
        {
            return element.Selected;
        }

        public void WaitUntilElementDisappeared(By locator, int maxTimeOut = 10)
        {
            for (int i = 0; i < maxTimeOut; i++)
            {
                IWebElement element = FindElement(locator);
                if (element == null)
                {
                    break;
                }
                else if (maxTimeOut == 9)
                {
                    throw new Exception("The element does not disappear " + element);
                }
            }
        }

        public dynamic GetTestDataFile(string baseURL)
        {
            var dir = Path.GetDirectoryName(typeof(LocalConfig).Assembly.Location) + @"\TestData";
            var testDataFile = (baseURL.Contains("www.auto")) ? Path.Combine(dir, $"testdataproduction.json") : Path.Combine(dir, $"testdata.json");
            return JsonConvert.DeserializeObject(File.ReadAllText(testDataFile, System.Text.Encoding.UTF8));
        }

        public string GetTestData(dynamic jsonFile, string titleSchema)
        {
            JToken token = jsonFile.SelectToken(titleSchema);
            return token.ToString();
        }

        public string GetValueByJS(string query)
        {
            return ((IJavaScriptExecutor)driver).ExecuteScript(@query).ToString().Trim();
        }

        public string GetPageTitle()
        {
            return driver.Title.ToString().Trim();
        }

        public T GetJsonValue<T>(string jsonString, string propertyName)
        {
            JObject jsonObject = JObject.Parse(jsonString);
            if(jsonObject.TryGetValue(propertyName, out var propertyValue))
            {
                try
                {
                    return propertyValue.Value<T>();
                }
                catch (Exception)
                {
                    throw new Exception($"Failed to convert JSON proprty '{propertyName}' to type {typeof(T).Name}");
                }
            }
            else
            {
                throw new Exception($"JSON property '{propertyName}' not found in JSON string");
            }
        }
    }

    public static class Extensions
    {
        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
                where TAttribute : Attribute
        {
            return enumValue.GetType()
                                    .GetMember(enumValue.ToString())
                                    .First()
                                    .GetCustomAttribute<TAttribute>();
        }

        public static string GenerateRandomString(int length)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * Random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static string GenerateRandomNumber(int length)
        {
            string builder = string.Empty;
            for (int i = 0; i < length; i++)
            {
                builder += Random.Next(1, 9).ToString();
            }
            return builder;
        }

        public static string RemoveAllMatchingChar(string str, List<char> charsToRemove)
        {
            charsToRemove.ForEach(c => str = str.Replace(c.ToString(), string.Empty));
            return str;
        }

        public static string GeneratePhoneNumber(string areaCode="437")
        {
            
            if (string.IsNullOrEmpty(areaCode) || areaCode.Length != 3)
            {
                throw new ArgumentException("Area code must be a 3-digit string.");
            }

            // Get the current date-time timestamp
            DateTime now = DateTime.Now;
            // Convert the current timestamp to a long integer
            long timestamp = now.Ticks;
            // Convert the long integer timestamp to a string
            string timestampString = timestamp.ToString();
            // Take the last 7 digits of the timestamp string to form the phone number
            string lastSevenDigits = timestampString.Substring(timestampString.Length - 7);
            // Concatenate the area code and the last 7 digits
            string phoneNumber = areaCode + lastSevenDigits;
            return phoneNumber;
        }


        internal static decimal? GetNumberFromString(string inputString)
        {                       
            string pattern = @"(?:\d{1,3}(?:\s*\d{3})+|\d+)(?:\.\d+)?";            
            Match match = Regex.Match(inputString, pattern);

            // If a match is found, extract the numeric value
            if (match.Success)
            {
                string amountString = match.Value.Replace(",", "").Replace(" ",""); // Remove commas if present
                decimal amount;
                if (decimal.TryParse(amountString, out amount))
                {
                    return amount;
                }
            }

            // If no match is found or parsing fails, return null
            return null;
        }
    }
}