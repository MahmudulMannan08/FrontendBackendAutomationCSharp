using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using static MarketPlaceWeb.Locators.SRPLocators;

namespace MarketPlaceWeb.Pages.SRP
{
    public abstract class SRPAbstract : Page
    {
        protected SRPAbstract(IWebDriver driver, Viewport viewport, Language language)
        {
            base.driver = driver;
            base.viewport = viewport;
            base.language = language;
        }

        private const string _defaultValue = null;

        #region dealerInventoryPage
        public abstract void OpenLeadTabDealerInventoryAbs();

        public void EnterTextOnDipLeadForm(By locator, string text)
        {
            ScrollTo(FindElement(locator));
            EnterText(locator, text);
        }

        public void ClickSendMessageBtnDipLead()
        {
            IWebElement element = FindElement(By.CssSelector(SRPLocators.CommonLocators.DipLeadSendMessageBtn.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            ClickElement(element);
        }

        public void SendEmailLeadAbs(DealerInventoryLeadForm dealerInventoryLead)
        {
            OpenLeadTabDealerInventoryAbs();

            EnterTextOnDipLeadForm(By.CssSelector(SRPLocators.CommonLocators.DipLeadNameField.GetAttribute<DescriptionAttribute>().Description), dealerInventoryLead.Name);
            EnterTextOnDipLeadForm(By.CssSelector(SRPLocators.CommonLocators.DipLeadEmailField.GetAttribute<DescriptionAttribute>().Description), dealerInventoryLead.Email);
            EnterTextOnDipLeadForm(By.CssSelector(SRPLocators.CommonLocators.DipLeadPhoneField.GetAttribute<DescriptionAttribute>().Description), dealerInventoryLead.PhoneNumber);
            EnterTextOnDipLeadForm(By.CssSelector(SRPLocators.CommonLocators.DipLeadMessageField.GetAttribute<DescriptionAttribute>().Description), dealerInventoryLead.Message);
            ClickSendMessageBtnDipLead();
            //Need new implementation for DIP lead wait.
        }

        public bool IsDipLeadFeedbackMsgDisplayed()
        {
            try
            {
                WaitForElementVisible(By.CssSelector(SRPLocators.CommonLocators.DipLeadFeedbackMsg.GetAttribute<DescriptionAttribute>().Description), 20);
                return true;
            }
            catch (Exception) { return false; }
        }

        public void ClickOKBtnDipLead()
        {
            ClickElement(FindElement(By.CssSelector(SRPLocators.CommonLocators.DipLeadFeedbackOKBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForElementNotVisible(By.CssSelector(SRPLocators.CommonLocators.DipLeadFeedbackMsg.GetAttribute<DescriptionAttribute>().Description));
        }
        #endregion

        public abstract int GetTotalNumberOfFoundAbs(SrpDesign srpDesign = SrpDesign.Old);

        public abstract int GetTotalNumberOfFoundInFacetOptionAbs(SRPLocators.Facets facet, string option);

        public abstract void SelectLocationFacetAbs(string location);

        public abstract void SelectFirstOptionFromMakeModelTypeSubTypeAbs(SRPLocators.Facets facet);

        public abstract void SelectSearchRadiusFacetAbs(SRPLocators.SearchRadius searchRadius, SrpDesign srpDesign = SrpDesign.Old);

        public abstract void SelectMakeFacetAbs(string make);

        public abstract void SelectModelFacetAbs(string model);

        public bool IsFacetOpen(SRPLocators.Facets facet, string statusIndicator)
        {
            try
            {
                return FindElement(By.CssSelector(facet.GetAttribute<FacetRowLocatorAttribute>().FacetRowLocator)).GetAttribute("class").Contains(statusIndicator);
            }
            catch (Exception)
            {
                throw new Exception("Unable to detect facet status.");
            }
        }

        public void WaitForFacetStatus(SRPLocators.Facets facet, FacetStatus facetStatus, string statusIndicator)
        {
            var facetRowLocator = FindElement(By.CssSelector(facet.GetAttribute<FacetRowLocatorAttribute>().FacetRowLocator));
            if (facetStatus == FacetStatus.Open)
            {
                WaitUntil(() => facetRowLocator.GetAttribute("class").Contains(statusIndicator), 15);
            }
            else
            {
                WaitUntil(() => !facetRowLocator.GetAttribute("class").Contains(statusIndicator));
            }
        }

        public void OpenChildFacetFlyout(SRPLocators.Facets facet, FacetStatus facetStatus, SrpDesign srpDesign = SrpDesign.Old)
        {
            switch (srpDesign)
            {
                case SrpDesign.Old:
                    if (facetStatus == FacetStatus.Open)
                    {
                        WaitForElementVisible(By.CssSelector(facet.GetAttribute<FacetLocatorAttribute>().FacetLocator));
                        IWebElement facetElement = FindElement(By.CssSelector(facet.GetAttribute<FacetLocatorAttribute>().FacetLocator));
                        ScrollTo(facetElement);
                        if (IsFacetOpen(facet, "open") != true)
                        {
                            try
                            {
                                ClickElement(facetElement);
                            }
                            catch (Exception)
                            {
                                ScrollTo(facetElement);
                                ClickElement(facetElement);
                            }
                            WaitForFacetStatus(facet, FacetStatus.Open, "open");
                        }
                    }
                    else
                    {
                        CloseChildFacetFlyoutAbs(facet);
                    }
                    break;
                case SrpDesign.New:
                    if (facetStatus == FacetStatus.Open)
                    {
                        IWebElement facetElement = FindElement(By.CssSelector(facet.GetAttribute<FacetLocatorAttribute>().FacetLocator));
                        ScrollTo(facetElement);
                        if (facet == SRPLocators.Facets.ModelChild || facet == SRPLocators.Facets.TrimChild)
                        {
                            try
                            {
                                WaitForFacetStatus(facet, FacetStatus.Enable, "disable-filter");
                            }
                            catch (Exception)
                            {
                                throw new Exception("Model/Trim facet is not enabled.");
                            }
                        }
                        if (IsFacetOpen(facet, "open") != true)
                        {
                            try
                            {
                                ClickElement(facetElement);
                            }
                            catch (Exception)
                            {
                                ScrollTo(facetElement);
                                ClickElement(facetElement);
                            }
                            WaitForFacetStatus(facet, FacetStatus.Open, "open");
                        }
                    }
                    else
                    {
                        CloseChildFacetFlyoutAbs(facet);
                    }
                    break;
            }
        }

        internal virtual bool VerifyPaymentCalculatorPillsOrderOnSRP()
        {            
            IList<IWebElement> listingPills = FindElements(By.CssSelector(SRPLocators.Listing.ListingsPill.GetAttribute<DescriptionAttribute>().Description), 20, 1000);
           
            bool isFirstPayCalcPill = false;

            foreach (IWebElement listing in listingPills)
            {
                // Find all badge-icon elements within the current listing
                IList<IWebElement> webElements = listing.FindElements(By.CssSelector(SRPLocators.Listing.SrpListingPills.GetAttribute<DescriptionAttribute>().Description));
                                                              
                if (webElements.Count == 1 && (GetElementText(webElements[0]).Contains("Payment Calculator") || GetElementText(webElements[0]).Contains("Calculatrice de paiement")))
                { 
                    isFirstPayCalcPill = true; 
                }
                else if (webElements.Count == 2 && ((GetElementText(webElements[0]).Contains("Award Winner") || GetElementText(webElements[0]).Contains("Award Winner French"))))
                {
                    if (GetElementText(webElements[1]).Contains("Payment Calculator") || GetElementText(webElements[1]).Contains("Calculatrice de paiement"))
                        isFirstPayCalcPill = true;
                }
                else if (webElements.Count == 2 && (GetElementText(webElements[0]).Contains("Payment Calculator") || GetElementText(webElements[0]).Contains("Calculatrice de paiement")))
                { 
                    isFirstPayCalcPill = true; 
                }
                else
                {                   
                    isFirstPayCalcPill = false; 
                }



            }
            
            return  isFirstPayCalcPill;
        }

       

        public void CustomSelectByValue(IWebElement dropdownElement, IWebElement dropdownRowElement, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception("Value can not be null: " + value);
            }
            ScrollTo(dropdownRowElement);
            ClickElement(dropdownRowElement.FindElement(By.CssSelector("button")));
            WaitUntil(() => dropdownRowElement.GetAttribute("class").Contains("open"), 15);

            var option = dropdownElement.FindElements(By.XPath(".//li[@value='" + value + "']")).FirstOrDefault();
            if (option == null)
            {
                throw new Exception("Dropdown does not contain value: " + value);
            }
            try
            {
                option.Click();
            }
            catch (Exception)
            {
                ScrollTo(option);
                option.Click();
            }
        }

        public abstract void OpenFacetAbs(SRPLocators.Facets facet, FacetStatus facetStatus, SrpDesign srpDesign = SrpDesign.Old);

        public abstract void OpenParentFacetAbs(SRPLocators.Facets facet, FacetStatus facetStatus, string statusIndicator = "show-list");

        public abstract void CloseChildFacetFlyoutAbs(SRPLocators.Facets facet, SrpDesign srpDesign = SrpDesign.Old);

        public abstract string GetSelectedValueOfFacetAbs(SRPLocators.Facets facet, HeaderFooterLocators.Categories category = HeaderFooterLocators.Categories.CarsTrucksSuv, SrpDesign srpDesign = SrpDesign.Old);

        public abstract string GetFacetElementMinText(SRPLocators.Facets facet);

        public abstract string GetFacetElementMaxText(SRPLocators.Facets facet);

        public abstract string CustomGetSelectedText(SRPLocators.Facets facet);

        public abstract string GetYearMinSelectedText(SrpDesign srpDesign = SrpDesign.Old);

        public abstract string GetYearMaxSelectedText(SrpDesign srpDesign = SrpDesign.Old);

        public abstract void CloseFacetAbs(SRPLocators.Facets facets, SrpDesign srpDesign = SrpDesign.Old);

        public virtual bool ContainsInFacet(SRPLocators.Facets facet, string value1 = _defaultValue, string value2 = _defaultValue, string value3 = _defaultValue, SrpDesign srpDesign = SrpDesign.Old)
        {
            bool result = false;
            IList<string> listOfValues = new List<string>() { value1, value2, value3 };
            string selectedValueOfFacet = GetSelectedValueOfFacetAbs(facet, srpDesign: srpDesign);
            foreach (var element in listOfValues)
            {
                if (element != null)
                {
                    if (selectedValueOfFacet.Contains(element))
                    {
                        result = true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return result;
        }

        #region PreQual
        internal virtual bool? VerifyPriceFilterForPreQual(PreQualLocators.PreQual resultType, string preQualMaxAmount = null)
        {
            WaitForPageLoad(10);
            WaitForElementVisible(By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<FacetValueLocatorAttribute>().FacetValueLocator));
            string url = Driver.Url;

            switch (resultType)
            {
                case PreQualLocators.PreQual.SuperPrime:
                case PreQualLocators.PreQual.Prime:
                case PreQualLocators.PreQual.ErrorInValidIDV:
                case PreQualLocators.PreQual.ErrorInvalidKBA:
                case PreQualLocators.PreQual.IDVMaxLimitReached:
                case PreQualLocators.PreQual.PreQualDecline:
                    {
                        string priceFilterValue = GetElementText(FindElement(By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<FacetValueLocatorAttribute>().FacetValueLocator)));
                        return (url.EndsWith("cars") || url.EndsWith("autos")) 
                            && (priceFilterValue.ToLower().Trim().Contains("any") || priceFilterValue.ToLower().Trim().Contains("tout"));
                    }                
                case PreQualLocators.PreQual.NearPrime:                  
                case PreQualLocators.PreQual.SubPrime:                 
                case PreQualLocators.PreQual.DeepSubPrime:
                    {
                        string priceFilterValue = GetElementText(FindElement(By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<FacetValueLocatorAttribute>().FacetValueLocator)));
                        return url.Contains("pRng=%2C" + preQualMaxAmount) && priceFilterValue.Replace(",", "").Replace(" ", "").Contains(preQualMaxAmount);
                    }
            }
            
            return false;
        }
        #endregion

        public abstract string GetTitleTextAbs();

        public abstract void ClearFacetAbs(SRPLocators.Facets facets, SrpDesign srpDesign = SrpDesign.Old);

        public abstract void SelectBuyingOptionsSingleAbs(SRPLocators.BuyingOptions buyingOptions, bool toBeChecked, SrpDesign srpDesign = SrpDesign.Old);

        public abstract bool IsBuyingOptionsCheckedAbs(SRPLocators.BuyingOptions contactlessServices, SrpDesign srpDesign = SrpDesign.Old);

        public abstract void WaitForSRPPageLoad(int timeOut = 60);

        public virtual void ClickOnApplyOnFacet(SRPLocators.Facets facets)
        {
            try
            {
                ClickElement(FindElement(By.CssSelector(facets.GetAttribute<ApplyButtonSelectorAttribute>().ApplyButtonSelector)), 30);
            }
            catch (Exception)
            {
                //Not all facets have apply button
            }
            finally
            {
                WaitForPageLoad(90);
                WaitForSRPPageLoad();
            }
        }

        public bool IsHomeDeliveryOn()
        {
            return IsCheckboxChecked(FindElements(By.CssSelector(SRPLocators.CommonLocators.HomeDeliveryToggle.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault());
        }

        public abstract void SelectYearFacetAbs(SRPLocators.Year year, string minYear, string maxYear, SrpDesign srpDesign = SrpDesign.Old);

        public abstract void SelectPaymentsFacetAbs(string minPayment, string maxPayment, SRPLocators.PaymentFrequency paymentFrequency, SRPLocators.Term term, string downPayment, string tradeInValue, SrpDesign srpDesign = SrpDesign.Old);

        public abstract void SelectCheckboxMultipleAbs(SRPLocators.Facets facet, List<string> listOfOptions, bool toBeChecked);

        public abstract bool IsCheckboxStatusMultiple(SRPLocators.Facets facet, Dictionary<string, bool> listOfOptionStatus);

        public abstract void SelectEngineFacetSingleAbs(SRPLocators.Engine engine, bool toBeChecked);

        public abstract void SelectTransmissionFacetSingleAbs(SRPLocators.Transmission transmission, bool toBeChecked);

        public abstract void EnterMinMaxFacetAbs(SRPLocators.Facets facet, string minValue, string maxValue, SrpDesign srpDesign = SrpDesign.Old);

        public abstract void ClearAllFacetsAbs();

        public abstract void SelectConditionFacetSingleAbs(SRPLocators.Condition condition, bool toBeChecked, SrpDesign srpDesign = SrpDesign.Old);

        public abstract void SelectSellerTypeFacetAbs(SRPLocators.SellerType sellerType, bool toBeChecked, SrpDesign srpDesign = SrpDesign.Old);

        public abstract void SelectTrimFacetSingleAbs(string trim, bool toBeChecked);

        public abstract void SelectBodyTypeFacetSingleAbs(SRPLocators.BodyType bodyType, bool toBeChecked);

        public abstract void SelectExteriorColourFacetSingleAbs(SRPLocators.ExteriorColour exteriorColour, bool toBeChecked);

        public abstract void SelectDrivetrainFacetSingleAbs(SRPLocators.Drivetrain drivetrain, bool toBeChecked);

        public abstract void SelectFuelTypeFacetSingleAbs(SRPLocators.FuelType fuelType, bool toBeChecked);

        public abstract void SelectSeatingCapacityFacetSingleAbs(SRPLocators.SeatingCapacity seatingCapacity, bool toBeChecked);

        public abstract void SelectDoorsFacetSingleAbs(SRPLocators.Doors doors, bool toBeChecked);

        public abstract void SelectSleepsFacetSingleAbs(SRPLocators.Sleeps sleeps, bool toBeChecked);

        public abstract void SelectSubTypeFacetSingleAbs(SRPLocators.SubType subType, bool toBeChecked);

        public abstract void SelectFirstCheckboxFromFacetAbs(SRPLocators.Facets facet);

        public abstract void SelectOtherOptionsSingleAbs(SRPLocators.OtherOptions otherOptions, bool toBeChecked, SrpDesign srpDesign = SrpDesign.Old);

        public abstract bool IsOtherOptionCheckedAbs(SRPLocators.OtherOptions otherOptions);

        public string GetDisplay()
        {
            if (viewport == Viewport.XS)
            {
                throw new Exception("XS view can't run Display tests");
            }
            return GetSelectedText(By.CssSelector(SRPLocators.CommonLocators.Display.GetAttribute<DescriptionAttribute>().Description));  //Changed implementation to SelectElement class
        }

        public void SelectDisplay(SRPLocators.Display display)
        {
            if (viewport != Viewport.XS)
            {
                SelectByValue(By.CssSelector(SRPLocators.CommonLocators.Display.GetAttribute<DescriptionAttribute>().Description), display.GetAttribute<DescriptionAttribute>().Description);  //Changed implementation to SelectElement class
                WaitForPageLoad(90);
                WaitForSRPPageLoad();
            }
        }

        public void SelectSort(SRPLocators.Sort sort)
        {
            SelectByValue(By.CssSelector(SRPLocators.CommonLocators.SortBy.GetAttribute<DescriptionAttribute>().Description), sort.GetAttribute<DescriptionAttribute>().Description);  //Changed implementation to SelectElement class
            WaitForPageLoad(90);
            WaitForSRPPageLoad();
        }

        public void CloseFeedbackPopUp()
        {
            IWebElement popUp = FindElement(By.CssSelector(SRPLocators.SmallLocators.FeedbackPopUp.GetAttribute<DescriptionAttribute>().Description), 20, 1000);
            if (popUp != null)
            {
                ClickElementJS(SRPLocators.SmallLocators.CloseFeedbackPopUp.GetAttribute<DescriptionAttribute>().Description);
            }
        }

        public enum FacetStatus
        {
            Open,
            Close,
            Enable
        }

        public abstract bool ContainsInListingTitleAbs(string value, string toCompare = _defaultValue, string value2 = _defaultValue);

        public abstract bool ListingPriceAbs(string value, string toCompare = _defaultValue, string value2 = _defaultValue);

        public abstract bool IsMileageWithinRangeAbs(string value, string toCompare = _defaultValue, string value2 = _defaultValue);

        public abstract bool IsProximityWithinRangeAbs(int proximity);

        public abstract bool IsEveryListingPrivateAbs();

        public abstract bool IsEveryListingDealerAbs();

        public abstract bool IsEveryListingCPOAbs();

        public abstract bool IsEveryListingNewAbs();

        public abstract bool HasEveryListingPhotoAbs();

        public abstract void ClickOnFirstOrganicListingAbs(HeaderFooterLocators.Categories category = HeaderFooterLocators.Categories.CarsTrucksSuv);

        public abstract string GetOrganicListingTitleAbs(int sequenceNo, HeaderFooterLocators.Categories category);

        public abstract IWebElement GetFirstMatchedListing(ListingsType listingsType);

        public abstract ListingImage GetListingMainImage(IWebElement element);

        public abstract ListingImage GetListingStripeImage(IWebElement element);

        public bool IsEasyFinanceBadgeDisplayed()
        {
            IList<IWebElement> easyFinancialBadge = FindElements(By.CssSelector(SRPLocators.CommonLocators.AllEasyFinancialBadges.GetAttribute<DescriptionAttribute>().Description));
            return easyFinancialBadge.Count > 0;
        }

        public bool IsEasyFinanceBadgeDisplayedForAllListings()
        {
            IList<IWebElement> priorityListingAds = FindElements(By.CssSelector(SRPLocators.CommonLocators.AllPriorityListingAds.GetAttribute<DescriptionAttribute>().Description));

            IList<IWebElement> allListingAds = FindElements(By.CssSelector(SRPLocators.CommonLocators.AllOrganicListingAds.GetAttribute<DescriptionAttribute>().Description));

            bool isBadgeDisplayed = false;

            foreach (var element in priorityListingAds)
            {
                var baseLocator = SRPLocators.Listing.EasyFinanceApprovalBadge.GetAttribute<DescriptionAttribute>().Description;
                var adId = element.GetAttribute("id");
                var formattedLocator = baseLocator.Replace("''", $"'{adId}'");

                isBadgeDisplayed = IsElementVisible(By.CssSelector(formattedLocator));
            }

            foreach (var element in allListingAds)
            {
                var baseLocator = SRPLocators.Listing.EasyFinanceApprovalBadge.GetAttribute<DescriptionAttribute>().Description;
                var adId = element.GetAttribute("id");
                var formattedLocator = baseLocator.Replace("''", $"'{adId}'");

                isBadgeDisplayed = IsElementVisible(By.CssSelector(formattedLocator));
            }

            return isBadgeDisplayed;
        }
        public abstract string GetListingProximity(string adId);
        public void OpenEasyFinanceModal()
        {
            IList<IWebElement> easyFinancialBadge = FindElements(By.CssSelector(SRPLocators.CommonLocators.AllEasyFinancialBadges.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(easyFinancialBadge.First());
        }

        #region Boost
        /// <summary>
        /// This method fetch AdSortPoints from SRP page for Priority listings / All listings 
        /// </summary>
        /// <param name="isPriorityListing">Pass true to fetch only Priority listing AdSortPoints</param>
        /// <returns>Adsort points with listing types PL,PPL,XPL,Org </returns>
        public abstract IDictionary<ListingsType, List<double>> GetAdSortPoints(bool isPriorityListing);
        public enum ListingsType
        {
            XPL,
            PPL,
            PL,
            Org,
            TS,
            HL,
            TA
        }

        /// <summary>
        /// This method return default sort type of listings
        /// </summary>
        /// <returns>Sort type</returns>
        /// 
        public string GetSortType()
        {
            var sortType = new SelectElement(FindElement(By.CssSelector(SRPLocators.CommonLocators.SortBy.GetAttribute<DescriptionAttribute>().Description))).SelectedOption.Text;
            return sortType;
        }

        internal int GetOrganicListingsDisplayCount()
        {
            IList<IWebElement> listings = FindElements(By.CssSelector(SRPLocators.CommonLocators.AllOrganicListingAds.GetAttribute<DescriptionAttribute>().Description), 20, 1000);
            return listings.Count;
        }
        #endregion

        public bool IsSpinIconDisplayedOnSrp(IWebElement listItem)
        {
            try
            {
                var spinIcon = listItem.FindElement(By.CssSelector(SRPLocators.LargeLocators.Spin360Icon.GetAttribute<DescriptionAttribute>().Description));
                return spinIcon.Text != string.Empty;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void ClickOnListing(IWebElement listItem)
        {
            IWebElement element = listItem.FindElement(By.CssSelector(SRPLocators.CommonLocators.ListingTitle.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(element);
            WaitForPageLoad(90);
        }

        public class ListingImage
        {
            public string URL { get; set; }
            public int Height { get; set; }
            public int Width { get; set; }
        }

        public bool IsSurveyCampaignDialogDisplay()
        {
            try
            {
                InteractOnIframe(By.CssSelector(SRPLocators.SurveyCampaignModal.SurveyIframe.GetAttribute<DescriptionAttribute>().Description),
                () => WaitForElementVisible(By.CssSelector(SRPLocators.SurveyCampaignModal.CloseBtn.GetAttribute<DescriptionAttribute>().Description)), 10);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public virtual bool IsAllListingWithoutTraditionDealer(Proximity proximity)
        {
            List<IWebElement> proximityDivs = FindElements(By.CssSelector(SRPLocators.CommonLocators.AllOrganicListingAds.GetAttribute<DescriptionAttribute>().Description)).ToList();
            bool allProximityValid = proximityDivs.All(div =>
            {
                string proximityText = div.FindElement(By.CssSelector(SRPLocators.CommonLocators.AllListingProximity.GetAttribute<DescriptionAttribute>().Description)).Text.ToLower();
                return proximityText == proximity.virtualProximityText || proximityText.EndsWith(proximity.hybridProximityText);
            });
            return allProximityValid;
        }

        public void ClickOnHomeDelivery()
        {
            IWebElement homeDeliveryToggle = FindElements(By.CssSelector(SRPLocators.CommonLocators.HomeDeliveryToggle.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault();
            homeDeliveryToggle.Click();
        }
        public string GetHomeDeliveryTooltipMessage()
        {
            var homeDeliveryToggle = FindElements(By.CssSelector(SRPLocators.CommonLocators.HomeDeliveryToggle.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault();
            Actions actions = new Actions(driver);
            actions.MoveToElement(homeDeliveryToggle).Perform();
            By homeDeliveryTooltipLocator = By.CssSelector(SRPLocators.CommonLocators.HomeDeliveryToggleTooltip.GetAttribute<DescriptionAttribute>().Description);
            WaitForElementVisible(homeDeliveryTooltipLocator);
            var tooltipText = FindElement(homeDeliveryTooltipLocator).Text;
            return tooltipText;
        }
        public virtual string GetHomeDeliveryInfoIconTooltipMessage()
        {
            var homeDeliveryToggleInfoIcon = FindElements(By.CssSelector(SRPLocators.CommonLocators.HomeDeliveryToggleInfoIcon.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault();
            Actions actions = new Actions(driver);
            actions.MoveToElement(homeDeliveryToggleInfoIcon).Perform();
            By homeDeliveryInfoTooltipLocator = By.CssSelector(SRPLocators.CommonLocators.HomeDeliveryToggleInfoTooltip.GetAttribute<DescriptionAttribute>().Description);
            WaitForElementVisible(homeDeliveryInfoTooltipLocator);
            var tooltipText = FindElement(homeDeliveryInfoTooltipLocator).Text;
            return tooltipText;
        }

        #region SaveSearch
        public abstract void SubscribeSaveSearch(string emailAddress);
        public abstract bool GetSubscribeButtonStatus(bool isSubscribed = true);
        public abstract void UnSubscribeSaveSearch();
        public bool IsSavedSearchSuccessDisplayed()
        {
            try
            {
                WaitForElementVisible(By.CssSelector(SRPLocators.CommonLocators.SuccessSaveSearchModal.GetAttribute<DescriptionAttribute>().Description));
                return true;
            }
            catch (Exception) { return false; }
        }
        public void CloseSavedSearchSuccessModal()
        {
            ClickElement(FindElement(By.CssSelector(SRPLocators.CommonLocators.SuccessSaveSearchModalCloseBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForSavedSearchSuccessModalStatus(false);
        }
        public void WaitForSavedSearchSuccessModalStatus(bool isDisplayed = true)
        {
            if (!isDisplayed)
            {
                WaitForElementNotVisible(By.CssSelector(SRPLocators.CommonLocators.SuccessSaveSearchModal.GetAttribute<DescriptionAttribute>().Description));
            }
            else
            {
                WaitForElementVisible(By.CssSelector(SRPLocators.CommonLocators.SuccessSaveSearchModal.GetAttribute<DescriptionAttribute>().Description));
            }
        }
        public string GetToasterMessage()
        {
            IWebElement toasterMessage = FindElement(By.CssSelector(SRPLocators.CommonLocators.ToasterMsg.GetAttribute<DescriptionAttribute>().Description));
            return GetElementText(toasterMessage);
        }
        public bool IsRedirectedToCorrectSRPFromMyGarage(string srpSubURL, string searchName)
        {
            string srpTitleText = GetTitleTextAbs();
            string urlWithoutQuery = new Uri(driver.Url).GetLeftPart(UriPartial.Path);
            return srpTitleText.ToLower().Contains(searchName.ToLower()) && urlWithoutQuery.ToUpperInvariant().Contains(srpSubURL.ToUpperInvariant());
        }
        #endregion

        #region SEO
        public string GetLocalSRPLink()
        {
            return GetElementAttribute(By.CssSelector(SRPLocators.SEOLinks.LocalSRPLink.GetAttribute<DescriptionAttribute>().Description), "href");

        }
        public bool IsSEOWidgetDisplayingOnSRPPage(SRPLocators.SEOLinks widget)
        {
            return IsElementVisible(By.CssSelector(widget.GetAttribute<DescriptionAttribute>().Description));
        }
        public void ClickSEOWidgetLink(SRPLocators.SEOLinks link)
        {
            ClickElement(FindElement(By.CssSelector(link.GetAttribute<DescriptionAttribute>().Description)));
        
        }
        public string GetElementFromListSEO(int num,SEOLinks element)
        {
            WaitForElementVisible(By.CssSelector(element.GetAttribute<DescriptionAttribute>().Description));
            
            IList<IWebElement> elements = FindElements(By.CssSelector(element.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(elements[num]);
            return elements[num].Text;

        }
        public abstract void ClickShowMoreYearsLinkAbs();
        public abstract string GetIntroDescSEOWidgetAbs();
        
        public string GetSEOElementsText(SEOLinks element)
        {
            return GetElementText(FindElement(By.CssSelector(element.GetAttribute<DescriptionAttribute>().Description)));
        }

        public string[] GetDiscoverModelsSRPSEOWidgets(SEOLinks element)
        {
            WaitForElementVisible(By.CssSelector(element.GetAttribute<DescriptionAttribute>().Description));
            IList<IWebElement> elements = FindElements(By.CssSelector(element.GetAttribute<DescriptionAttribute>().Description));
            string[] arr = new string[elements.Count];

            for (var i = 0; i < arr.Length; i++)
            {
                arr[i] = elements[i].Text;
            }
            return arr;
        }

        public string GetPopularWidgetTitle()
        {
            return GetElementText(FindElement(By.CssSelector(SRPLocators.PopularCarsWidget.PopularCarWidgetTitle.GetAttribute<DescriptionAttribute>().Description)));
        }

        public bool IsPoluparWidgetModelDisplayed(string model)
        {
            IList<IWebElement> widgetModels = FindElements(By.CssSelector(SRPLocators.PopularCarsWidget.PopularCarWidgetModelLinks.GetAttribute<DescriptionAttribute>().Description));

            foreach (var widgetModel in widgetModels)
            {
                widgetModel.GetAttribute("href");
                if (widgetModel.Text.Trim().Equals(model))
                {
                    return true;
                }
            }
            return false;
        }

        public string GetPopularWidgetModelLink(string model)
        {
            IList<IWebElement> widgetModels = FindElements(By.CssSelector(SRPLocators.PopularCarsWidget.PopularCarWidgetModelLinks.GetAttribute<DescriptionAttribute>().Description));

            foreach (var widgetModel in widgetModels)
            {
                if (widgetModel.Text.Trim().Equals(model))
                {
                    string s = widgetModel.GetAttribute("href");
                    return widgetModel.GetAttribute("href"); ;
                }
            }
            return null;
        }

        public int GetTotalWidgetModelCount()
        {
            IList<IWebElement> widgetModels = FindElements(By.CssSelector(SRPLocators.PopularCarsWidget.PopularCarWidgetModelLinks.GetAttribute<DescriptionAttribute>().Description));
            return widgetModels.Count;
        }

         public bool IsPopularWidgetTitle()
        {
            
            return IsElementVisible(By.CssSelector(SRPLocators.PopularCarsWidget.PopularCarWidgetTitle.GetAttribute<DescriptionAttribute>().Description));
        }
        #endregion

    }

    public enum SrpDesign
    {
        Old,
        New
    }

    public class DealerInventoryLeadForm : Page
    {
        public DealerInventoryLeads LeadType { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public enum DealerInventoryLeads
        {
            GeneralInquiry,
            ServiceInquiry,
            UsedVehicleInquiry,
            NewVehicleInquiry
        }
    }

    public class Proximity
    {
        public string virtualProximityText { get; set; }
        public string hybridProximityText { get; set; }
    }
}