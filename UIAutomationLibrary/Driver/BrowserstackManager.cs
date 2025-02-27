using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;

namespace MarketPlaceWeb.Driver
{
    public class BrowserstackManager
    {
        private readonly string testNameIOS = "IOS_" + TestContext.CurrentContext.Test.Name;
        private readonly string testNameIPAD = "IPAD_" + TestContext.CurrentContext.Test.Name;
        private readonly string testNameAndroid = "AN_" + TestContext.CurrentContext.Test.Name;
        private readonly string testNameWinChrome = "WIN_CHROME_" + TestContext.CurrentContext.Test.Name;
        private readonly string testNameWinEdge = "WIN_EDGE_" + TestContext.CurrentContext.Test.Name;
        private readonly string testNameWinFireFox = "WIN_FIREFOX_" + TestContext.CurrentContext.Test.Name;
        private readonly string testNameMacChrome = "MAC_CHROME_" + TestContext.CurrentContext.Test.Name;
        private readonly string testNameMacSafari = "MAC_SAFARI_" + TestContext.CurrentContext.Test.Name;        
        private readonly string testClassName = TestContext.CurrentContext.Test.ClassName;
        private readonly string browserstackUser = "{BrowserstackUserHere}"; //ConfigurationManager.AppSettings["browserstackUser"];
        private readonly string browserstackAccessKey = "{BrowserstackKeyHere}";//ConfigurationManager.AppSettings["browserstackPassword"];

        // MOBILE 
        public IWebDriver IOSSafariXS()
        {
            SafariOptions options = new SafariOptions();
            Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();
            browserstackOptions.Add("deviceName", "iPhone 13");
            browserstackOptions.Add("realMobile", "true");
            browserstackOptions.Add("projectName", testClassName);
            browserstackOptions.Add("sessionName", testNameIOS);
            browserstackOptions.Add("local", "false");
            browserstackOptions.Add("debug", "true");
            browserstackOptions.Add("userName", browserstackUser);
            browserstackOptions.Add("accessKey", browserstackAccessKey);
            browserstackOptions.Add("idleTimeout", 300);
            options.AddAdditionalOption("bstack:options", browserstackOptions);
            IWebDriver driver = new RemoteWebDriver(new Uri("http://hub-cloud.browserstack.com/wd/hub/"), options);
            BrowserStackExtensions.GetBrowserStackSessionDetails(driver);
            return driver;
        }

        public IWebDriver AndroidChromeXS()
        {
            ChromeOptions options = new ChromeOptions();
            Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();
            browserstackOptions.Add("osVersion", "12.0");
            browserstackOptions.Add("deviceName", "Samsung Galaxy S21");
            browserstackOptions.Add("realMobile", "true");
            browserstackOptions.Add("projectName", testClassName);
            browserstackOptions.Add("sessionName", testNameAndroid);
            browserstackOptions.Add("local", "false");
            browserstackOptions.Add("debug", "true");
            browserstackOptions.Add("consoleLogs", "verbose");
            browserstackOptions.Add("userName", browserstackUser);
            browserstackOptions.Add("accessKey", browserstackAccessKey);
            options.AddAdditionalOption("bstack:options", browserstackOptions);
            IWebDriver driver = new RemoteWebDriver(new Uri("http://hub-cloud.browserstack.com/wd/hub/"), options);
            BrowserStackExtensions.GetBrowserStackSessionDetails(driver);
            return driver;
        }

        // TABLET
        public IWebDriver IpadSafariSmallBrowserStack()
        {
            SafariOptions options = new SafariOptions();
            Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();
            browserstackOptions.Add("deviceName", "iPad Pro 11 2024");
            browserstackOptions.Add("realMobile", "true");
            browserstackOptions.Add("projectName", testClassName);
            browserstackOptions.Add("sessionName", testNameIPAD);
            browserstackOptions.Add("local", "false");
            browserstackOptions.Add("debug", "true");
            browserstackOptions.Add("userName", browserstackUser);
            browserstackOptions.Add("accessKey", browserstackAccessKey);
            browserstackOptions.Add("idleTimeout", 300);
            options.AddAdditionalOption("bstack:options", browserstackOptions);
            IWebDriver driver = new RemoteWebDriver(new Uri("http://hub-cloud.browserstack.com/wd/hub/"), options);
            BrowserStackExtensions.GetBrowserStackSessionDetails(driver);
            return driver;
        }

        // DEKSTOP LARGE
        public IWebDriver WinChromeLarge_BrowserStack()
        {
            ChromeOptions options = new ChromeOptions();
            options.BrowserVersion = "latest";
            options.AddArguments("start-maximized");
            Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();
            browserstackOptions.Add("os", "Windows");
            browserstackOptions.Add("osVersion", "10");
            browserstackOptions.Add("projectName", testClassName);
            browserstackOptions.Add("sessionName", testNameWinChrome);
            browserstackOptions.Add("local", "false");
            browserstackOptions.Add("debug", "true");
            browserstackOptions.Add("consoleLogs", "verbose");
            browserstackOptions.Add("telemetryLogs", "true");
            browserstackOptions.Add("userName", browserstackUser);
            browserstackOptions.Add("accessKey", browserstackAccessKey);
            browserstackOptions.Add("idleTimeout", 300);
            options.AddAdditionalOption("bstack:options", browserstackOptions);
            IWebDriver driver = new RemoteWebDriver(new Uri("http://hub-cloud.browserstack.com/wd/hub/"), options);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10)); // Exlicit wait for 10 
            BrowserStackExtensions.GetBrowserStackSessionDetails(driver);
            return driver;
        }

        public IWebDriver WinEdgeLarge_BrowserStack()
        {
            EdgeOptions options = new EdgeOptions();
            options.BrowserVersion = "latest";
            options.AddArguments("start-maximized");
            Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();
            browserstackOptions.Add("os", "Windows");
            browserstackOptions.Add("osVersion", "10");
            browserstackOptions.Add("projectName", testClassName);
            browserstackOptions.Add("sessionName", testNameWinEdge);
            browserstackOptions.Add("local", "false");
            browserstackOptions.Add("debug", "true");
            browserstackOptions.Add("seleniumVersion", "4.1.0");
            browserstackOptions.Add("telemetryLogs", "true");
            browserstackOptions.Add("userName", browserstackUser);
            browserstackOptions.Add("accessKey", browserstackAccessKey);
            browserstackOptions.Add("idleTimeout", 300);
            options.AddAdditionalOption("bstack:options", browserstackOptions);
            IWebDriver driver = new RemoteWebDriver(new Uri("http://hub-cloud.browserstack.com/wd/hub/"), options);
            BrowserStackExtensions.GetBrowserStackSessionDetails(driver);
            return driver;
        }

        public IWebDriver WinFirefoxLarge_BrowserStack()
        {
            FirefoxOptions options = new FirefoxOptions();
            options.BrowserVersion = "latest";
            Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();
            browserstackOptions.Add("os", "Windows");
            browserstackOptions.Add("osVersion", "10");
            browserstackOptions.Add("resolution", "1280x1024");
            browserstackOptions.Add("projectName", testClassName);
            browserstackOptions.Add("sessionName", testNameWinFireFox);
            browserstackOptions.Add("local", "false");
            browserstackOptions.Add("debug", "true");
            browserstackOptions.Add("telemetryLogs", "true");
            browserstackOptions.Add("userName", browserstackUser);
            browserstackOptions.Add("accessKey", browserstackAccessKey);
            browserstackOptions.Add("idleTimeout", 300);
            options.AddAdditionalOption("bstack:options", browserstackOptions);
            IWebDriver driver = new RemoteWebDriver(new Uri("http://hub-cloud.browserstack.com/wd/hub/"), options);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
            BrowserStackExtensions.GetBrowserStackSessionDetails(driver);
            return driver;
        }

        public IWebDriver MacSafariLarge_BrowserStack()
        {
            SafariOptions options = new SafariOptions();
            options.BrowserVersion = "latest";
            options.BrowserVersion = "13.1";
            Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();
            browserstackOptions.Add("os", "OS X");
            browserstackOptions.Add("osVersion", "Catalina");
            browserstackOptions.Add("resolution", "1280x1024");
            browserstackOptions.Add("projectName", testClassName);
            browserstackOptions.Add("sessionName", testNameMacSafari);
            browserstackOptions.Add("local", "false");
            browserstackOptions.Add("debug", "true");
            browserstackOptions.Add("telemetryLogs", "true");
            browserstackOptions.Add("userName", browserstackUser);
            browserstackOptions.Add("accessKey", browserstackAccessKey);
            browserstackOptions.Add("idleTimeout", 300);
            options.AddAdditionalOption("bstack:options", browserstackOptions);
            IWebDriver driver = new RemoteWebDriver(new Uri("http://hub-cloud.browserstack.com/wd/hub/"), options);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
            BrowserStackExtensions.GetBrowserStackSessionDetails(driver);
            return driver;
        }

        public IWebDriver MacChromeLarge_BrowserStack()
        {
            ChromeOptions options = new ChromeOptions();
            options.BrowserVersion = "latest";
            options.AddArguments("start-maximized");
            Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();
            browserstackOptions.Add("os", "OS X");
            browserstackOptions.Add("osVersion", "Catalina");
            browserstackOptions.Add("projectName", testClassName);
            browserstackOptions.Add("sessionName", testNameMacChrome);
            browserstackOptions.Add("local", "false");
            browserstackOptions.Add("debug", "true");
            browserstackOptions.Add("consoleLogs", "verbose");
            browserstackOptions.Add("telemetryLogs", "true");
            browserstackOptions.Add("userName", browserstackUser);
            browserstackOptions.Add("accessKey", browserstackAccessKey);
            options.AddAdditionalOption("bstack:options", browserstackOptions);
            IWebDriver driver = new RemoteWebDriver(new Uri("http://hub-cloud.browserstack.com/wd/hub/"), options);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
            BrowserStackExtensions.GetBrowserStackSessionDetails(driver);
            return driver;
        }
    }

    public static class BrowserStackExtensions
    {
        /// <summary>
        /// This Method will mark failed status for Browser Stack if the test fails
        /// </summary>
        /// <param name="driver"></param>
        public static void MarkBSFailedStatus(IWebDriver driver)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \" Test Failed \"}}");
        }

        /// <summary>
        /// This method will print the browser stack active session link for the test
        /// </summary>
        /// <returns></returns>
        public static string GetBrowserStackSessionDetails(IWebDriver driver)
        {            
            var url = "https://api.browserstack.com/automate/builds.json?status=running";
            string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                                           .GetBytes("{BrowserstackUserHere}" + ":" + "{BrowserstackKeyHere}"));
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Headers.Add("Authorization", "Basic " + encoded);

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var runningBuildID = result.ToString().Split(new string[] { "\"hashed_id\":" }, StringSplitOptions.None)[1]
                    .Split(new string[] { ",\"build_tag\"" }, StringSplitOptions.None)[0].Replace("\"", "");

                RemoteWebDriver remoteDriver = (RemoteWebDriver)driver;
                string sessionId = remoteDriver.SessionId.ToString();
                string bsURL = "https://www.browserstack.com/automate/builds/" + runningBuildID + "/sessions/" + sessionId;
                TestContext.WriteLine("BrowserStack BuildID " + runningBuildID);
                TestContext.WriteLine("SessionID " + sessionId);
                if (!string.IsNullOrEmpty(sessionId))
                {
                    TestContext.WriteLine("BrowserStack Session URL " + bsURL);
                }
                else
                {
                    TestContext.WriteLine("Browserstack session was not found!!!");
                }
                return bsURL;
            }

        }
    }
}
