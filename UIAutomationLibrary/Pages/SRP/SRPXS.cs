using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace MarketPlaceWeb.Pages.SRP
{
    public class SRPXS : SRPAbstract
    {
        public SRPXS(IWebDriver driver, Viewport viewport, Language language) : base(driver, viewport, language) { }

        private const string _defaultValue = null;

        #region dealerInventoryPage
        public override void OpenLeadTabDealerInventoryAbs()
        {
            ClickElement(FindElement(By.CssSelector(SRPLocators.CommonLocators.DipEmailButton.GetAttribute<DescriptionAttribute>().Description)));
            WaitForElementVisible(By.CssSelector(SRPLocators.CommonLocators.DipLeadModal.GetAttribute<DescriptionAttribute>().Description));
        }
        #endregion

        public override int GetTotalNumberOfFoundAbs(SrpDesign srpDesign = SrpDesign.Old)
        {
            ScrollToTop();
            if (IsElementVisible(By.CssSelector(SRPLocators.CommonLocators.PageErrorWarning.GetAttribute<DescriptionAttribute>().Description)) || IsElementVisible(By.CssSelector(SRPLocators.CommonLocators.NoSearchResultWarning.GetAttribute<DescriptionAttribute>().Description))) //For multiple filters different No Search Result error message is displayed
            { return 0; }  //Counter zero result scenario
            IWebElement foundTotal = FindElement(By.CssSelector(SRPLocators.XSLocators.FoundTotal.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(foundTotal);
            var totalNumberofVehicles = foundTotal.Text;
            if (!string.IsNullOrEmpty(totalNumberofVehicles))
            {
                return Convert.ToInt32(totalNumberofVehicles.Trim().Replace(" ", "").Replace(",", ""));
            }
            else
            {
                throw new Exception("Total vehicle count not found on H1 row.");
            }
        }

        public override int GetTotalNumberOfFoundInFacetOptionAbs(SRPLocators.Facets facet, string option)
        {
            OpenFacetAbs(facet, FacetStatus.Open);
            if (facet == SRPLocators.Facets.SubType)
            {
                return Convert.ToInt32(Regex.Replace(FindElement(By.CssSelector(facet.GetAttribute<FacetListAttribute>().FacetList + "[data-dropdownvalue='" + option + "'] a span"))
                    .Text, @"\D", ""));
            }

            IWebElement facetOptionElement = FindElements(By.CssSelector(facet.GetAttribute<FacetLabelAttribute>().FacetLabel)).FirstOrDefault(x => x.Text.Contains(option));
            if (facetOptionElement != null)
            {
                ScrollTo(facetOptionElement);
                string cleanUnwantedDigits = GetSubstringAfterCharacter(facetOptionElement.Text, '(');  //Remove trailing characters incluing '('
                return Convert.ToInt32(GetSubstringBeforeCharacter(cleanUnwantedDigits, ')'));
            }

            return 0;
        }

        public override void SelectLocationFacetAbs(string location)
        {
            OpenFacetAbs(SRPLocators.Facets.LocationParent, FacetStatus.Open);
            FindElement(By.CssSelector(SRPLocators.CommonLocators.LocationField.GetAttribute<DescriptionAttribute>().Description)).SendKeys(location);
            var selector = SRPLocators.Facets.CityPostalCodeChild.GetAttribute<ApplyButtonSelectorAttribute>().ApplyButtonSelector;
            ClickElement(FindElement(By.CssSelector(selector)), 20);
            ClickViewResultsBtn();
            WaitForPageLoad(90);
        }

        public override void SelectFirstOptionFromMakeModelTypeSubTypeAbs(SRPLocators.Facets facet)
        {
            if (facet != SRPLocators.Facets.MakeChild && facet != SRPLocators.Facets.ModelChild && facet != SRPLocators.Facets.Type && facet != SRPLocators.Facets.SubType)
            {
                throw new Exception("This method cannot work for the selected facet.");
            }
            else
            {
                int totalNumberOfFound = GetTotalNumberOfViewResults();
                OpenFacetAbs(facet, FacetStatus.Open);
                ClickElement(FindElements(By.CssSelector(facet.GetAttribute<FacetListAttribute>().FacetList)).FirstOrDefault(x => x.Text.Length > 0));
                WaitUntilResultsChanged(totalNumberOfFound);
                ClickViewResultsBtn();
            }
        }

        public override void SelectSearchRadiusFacetAbs(SRPLocators.SearchRadius searchRadius, SrpDesign srpDesign = SrpDesign.Old)
        {
            switch (srpDesign)
            {
                case SrpDesign.Old:
                    int totalNumberOfFound = GetTotalNumberOfViewResults();
                    OpenFacetAbs(SRPLocators.Facets.Distance, FacetStatus.Open);
                    string value = (searchRadius != SRPLocators.SearchRadius.Provincial || searchRadius != SRPLocators.SearchRadius.National) ? searchRadius.ToString().Replace("Plus_", "") : (searchRadius == SRPLocators.SearchRadius.Provincial) ? "-2" : (searchRadius == SRPLocators.SearchRadius.National) ? "-1" : throw new Exception("This value is not available in the dropdown.");
                    SelectByValue(By.CssSelector(SRPLocators.Facets.Distance.GetAttribute<DropdownLocatorAttribute>().DropdownLocator), value);
                    ClickOnApplyOnFacet(SRPLocators.Facets.Distance, totalNumberOfFound);
                    ClickViewResultsBtn();
                    break;
                case SrpDesign.New:
                    int totalNumberOfFoundNewSrp = GetTotalNumberOfViewResults();
                    OpenFacetAbs(SRPLocators.Facets.SearchRadiusChild, FacetStatus.Open);
                    string valueNewSrp = (searchRadius != SRPLocators.SearchRadius.Provincial || searchRadius != SRPLocators.SearchRadius.National) ? searchRadius.ToString().Replace("Plus_", "") : (searchRadius == SRPLocators.SearchRadius.Provincial) ? "-2" : (searchRadius == SRPLocators.SearchRadius.National) ? "-1" : throw new Exception("This value is not available in the dropdown.");
                    CustomSelectByValue(FindElement(By.CssSelector(SRPLocators.Facets.SearchRadiusChild.GetAttribute<DropdownLocatorAttribute>().DropdownLocator)), FindElement(By.CssSelector(SRPLocators.Facets.SearchRadiusChild.GetAttribute<DropdownFacetRowLocatorAttribute>().DropdownFacetRowLocator)), valueNewSrp);
                    WaitUntilResultsChanged(totalNumberOfFoundNewSrp);
                    ClickViewResultsBtn();
                    break;
            }
        }

        public override void SelectMakeFacetAbs(string make)
        {
            int totalNumberOfFound = GetTotalNumberOfViewResults();
            OpenFacetAbs(SRPLocators.Facets.MakeChild, FacetStatus.Open);
            FindElement(By.CssSelector(SRPLocators.Facets.MakeChild.GetAttribute<SearchTextLocatorAttribute>().SearchTextLocator)).SendKeys(make);
            IList<IWebElement> listOfMake = FindElements(By.CssSelector(SRPLocators.Facets.MakeChild.GetAttribute<FacetListAttribute>().FacetList));
            ClickElement(listOfMake.FirstOrDefault(x => x.GetAttribute("data-dropdownvalue") == make));
            WaitUntilResultsChanged(totalNumberOfFound);
            ClickViewResultsBtn();
        }

        public override void SelectModelFacetAbs(string model)
        {
            int totalNumberOfFound = GetTotalNumberOfViewResults();
            OpenFacetAbs(SRPLocators.Facets.ModelChild, FacetStatus.Open);
            IList<IWebElement> listOfModel = FindElements(By.CssSelector(SRPLocators.Facets.ModelChild.GetAttribute<FacetListAttribute>().FacetList));
            IWebElement element = listOfModel.FirstOrDefault(x => x.GetAttribute("data-dropdownvalue") == model);
            ScrollTo(element);
            ClickElement(element);
            WaitUntilResultsChanged(totalNumberOfFound);
            ClickViewResultsBtn();
        }

        public int GetTotalNumberOfViewResults()
        {
            OpenFilters();
            IWebElement foundTotal = FindElement(By.CssSelector(SRPLocators.XSLocators.ViewResultsBtn.GetAttribute<ViewResultsSpanAttribute>().ViewResultsSpan));
            ScrollTo(foundTotal);
            var foundTotaltext = foundTotal.Text.Replace(" ", "").Replace(",", "");
            if (string.IsNullOrWhiteSpace(foundTotaltext))
            {
                return 0;
            }
            return Convert.ToInt32(foundTotaltext);
        }

        public void ClickViewResultsBtn()
        {
            ClickElement(FindElement(By.CssSelector(SRPLocators.XSLocators.ViewResultsBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitUntilFilterIsClosed();
            Wait(4); //Mitigate the wait time to start loading, at this stage document state shows complete. Alternatively implement fluent wait for total count change.
            WaitForPageLoad(90);
            WaitForSRPPageLoad();
        }

        public bool IsFilterOpen()
        {
            return IsElementAvailable(By.CssSelector(SRPLocators.XSLocators.ViewResultsBtn.GetAttribute<DescriptionAttribute>().Description));
        }

        public void WaitUntilResultsChanged(int initialFoundNumber, int maxWaitTime = 20)
        {
            try
            {
                WaitUntil(() => initialFoundNumber != GetTotalNumberOfViewResults(), maxWaitTime);
            }
            catch
            {
                //ignore timeout, as results may not be changed in some cases (catches all exceptions)
            }
        }

        private void WaitUntilFilterIsClosed(int maxWaitTime = 10)
        {
            WaitForElementNotVisible(By.CssSelector(SRPLocators.XSLocators.ViewResultsBtn.GetAttribute<DescriptionAttribute>().Description), maxWaitTime);
        }

        public override string GetTitleTextAbs()
        {
            IWebElement element = FindElement(By.CssSelector(SRPLocators.XSLocators.Title.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            return GetElementText(element);
        }

        public override void ClearFacetAbs(SRPLocators.Facets facet, SrpDesign srpDesign = SrpDesign.Old)
        {
            if (srpDesign == SrpDesign.Old)
            {
                switch (facet)
                {
                    case SRPLocators.Facets.ConditionParent:
                    case SRPLocators.Facets.SellerTypeParent:
                        ClearAllFacetsAbs();
                        break;
                    case SRPLocators.Facets.MakeChild:
                    case SRPLocators.Facets.ModelChild:
                    case SRPLocators.Facets.TrimChild:
                    case SRPLocators.Facets.BodyTypeChild:
                    case SRPLocators.Facets.ColourChild:
                    case SRPLocators.Facets.DrivetrainChild:
                    case SRPLocators.Facets.FuelTypeChild:
                    case SRPLocators.Facets.EngineChild:
                    case SRPLocators.Facets.TransmissionChild:
                    case SRPLocators.Facets.SeatingCapacityChild:
                    case SRPLocators.Facets.DoorsChild:
                    case SRPLocators.Facets.AtHomeServices:
                    case SRPLocators.Facets.PricePaymentsParent:
                    case SRPLocators.Facets.KilometersChild:
                    case SRPLocators.Facets.YearChild:
                    case SRPLocators.Facets.Type:
                    case SRPLocators.Facets.Wheelbase:
                    case SRPLocators.Facets.Length:
                    case SRPLocators.Facets.Weight:
                    case SRPLocators.Facets.Sleeps:
                    case SRPLocators.Facets.SlideOuts:
                    case SRPLocators.Facets.SubType:
                    case SRPLocators.Facets.Hours:
                    case SRPLocators.Facets.Horsepower:
                    case SRPLocators.Facets.EngineSize:
                        CloseFacetFlyoutOldSRP(facet);
                        int totalNumberOfFound1 = GetTotalNumberOfViewResults();
                        IList<IWebElement> selectedValues = FindElements(By.CssSelector(facet.GetAttribute<SelectedFacetSpanAttribute>().SelectedFacetSpan));
                        int totalSelectedValues = selectedValues.Count;
                        int totalSelectedValuesWithText = CountElements(selectedValues);
                        for (int i = 0; i < totalSelectedValues; i++)
                        {
                            if (selectedValues.Count == 1 && string.IsNullOrEmpty(selectedValues[0].Text))
                            {
                                break;
                            }
                            else
                            {
                                ClickElement(selectedValues.FirstOrDefault(e => !string.IsNullOrEmpty(e.Text)));
                                WaitUntil(() => CountElements(FindElements(By.CssSelector(facet.GetAttribute<SelectedFacetSpanAttribute>().SelectedFacetSpan))) == totalSelectedValuesWithText - i - 1);  //Check everytime a facet filter is modified against available filters
                                selectedValues = FindElements(By.CssSelector(facet.GetAttribute<SelectedFacetSpanAttribute>().SelectedFacetSpan));
                            }
                        }

                        WaitUntilResultsChanged(totalNumberOfFound1);
                        ClickViewResultsBtn();
                        break;
                    case SRPLocators.Facets.LocationParent:
                    case SRPLocators.Facets.CityPostalCodeChild:
                    case SRPLocators.Facets.SearchRadiusChild:
                    case SRPLocators.Facets.VehicleParent:
                    case SRPLocators.Facets.MechanicalParent:
                    case SRPLocators.Facets.InteriorParent:
                    case SRPLocators.Facets.OtherOptionsParent:
                        throw new Exception("Not a valid facet for clear facet");
                }
            }
            else
            {
                switch (facet)
                {
                    case SRPLocators.Facets.CityPostalCodeChild:
                    case SRPLocators.Facets.MakeChild:
                    case SRPLocators.Facets.ModelChild:
                    case SRPLocators.Facets.TrimChild:
                    case SRPLocators.Facets.BodyTypeChild:
                    case SRPLocators.Facets.ColourChild:
                    case SRPLocators.Facets.DrivetrainChild:
                    case SRPLocators.Facets.FuelTypeChild:
                    case SRPLocators.Facets.EngineChild:
                    case SRPLocators.Facets.TransmissionChild:
                    case SRPLocators.Facets.SeatingCapacityChild:
                    case SRPLocators.Facets.DoorsChild:
                        int totalNumberOfFound = GetTotalNumberOfViewResults();
                        OpenFacetAbs(facet, FacetStatus.Open);
                        ClickElement(FindElement(By.CssSelector(facet.GetAttribute<ClearButtonLocatorSXSAttribute>().ClearButtonLocatorSXS)));
                        OpenChildFacetFlyout(facet, FacetStatus.Close);
                        WaitUntilResultsChanged(totalNumberOfFound);
                        ClickViewResultsBtn();
                        break;
                    case SRPLocators.Facets.SearchRadiusChild:
                    case SRPLocators.Facets.ConditionParent:
                    case SRPLocators.Facets.KilometersChild:
                    case SRPLocators.Facets.YearChild:
                    case SRPLocators.Facets.SellerTypeParent:
                    case SRPLocators.Facets.AtHomeServices:
                    case SRPLocators.Facets.PricePaymentsParent:
                        ClearAllFacetsAbs();
                        break;
                    case SRPLocators.Facets.Type:
                    case SRPLocators.Facets.Wheelbase:
                    case SRPLocators.Facets.Length:
                    case SRPLocators.Facets.Weight:
                    case SRPLocators.Facets.Sleeps:
                    case SRPLocators.Facets.SlideOuts:
                    case SRPLocators.Facets.SubType:
                    case SRPLocators.Facets.Hours:
                    case SRPLocators.Facets.Horsepower:
                    case SRPLocators.Facets.EngineSize:
                        CloseFacetFlyoutOldSRP(facet);
                        int totalNumberOfFound1 = GetTotalNumberOfViewResults();
                        IList<IWebElement> selectedValues = FindElements(By.CssSelector(facet.GetAttribute<SelectedFacetSpanAttribute>().SelectedFacetSpan));
                        int totalSelectedValues = selectedValues.Count;
                        int totalSelectedValuesWithText = CountElements(selectedValues);
                        for (int i = 0; i < totalSelectedValues; i++)
                        {
                            if (selectedValues.Count == 1 && string.IsNullOrEmpty(selectedValues[0].Text))
                            {
                                break;
                            }
                            else
                            {
                                ClickElement(selectedValues.FirstOrDefault(e => !string.IsNullOrEmpty(e.Text)));
                                WaitUntil(() => CountElements(FindElements(By.CssSelector(facet.GetAttribute<SelectedFacetSpanAttribute>().SelectedFacetSpan))) == totalSelectedValuesWithText - i - 1);  //Check everytime a facet filter is modified against available filters
                                selectedValues = FindElements(By.CssSelector(facet.GetAttribute<SelectedFacetSpanAttribute>().SelectedFacetSpan));
                            }
                        }

                        WaitUntilResultsChanged(totalNumberOfFound1);
                        ClickViewResultsBtn();
                        break;
                    case SRPLocators.Facets.LocationParent:
                    case SRPLocators.Facets.VehicleParent:
                    case SRPLocators.Facets.MechanicalParent:
                    case SRPLocators.Facets.InteriorParent:
                    case SRPLocators.Facets.OtherOptionsParent:
                        throw new Exception("Not a valid facet for clear facet");
                }
            }
        }

        public void OpenFilters()
        {
            if (!IsFilterOpen())
            {
                By filterLocator = By.CssSelector(SRPLocators.XSLocators.Filter.GetAttribute<DescriptionAttribute>().Description);
                WaitUntilElementIsEnabled(FindElement(filterLocator));
                ClickElement(FindElement(filterLocator));
                WaitForPageLoad(90);
            }
        }

        public override void OpenParentFacetAbs(SRPLocators.Facets facet, FacetStatus facetStatus, string statusIndicator = "show-list")
        {
            switch (facet)
            {
                case SRPLocators.Facets.LocationParent:
                case SRPLocators.Facets.VehicleParent:
                case SRPLocators.Facets.ConditionParent:
                case SRPLocators.Facets.SellerTypeParent:
                case SRPLocators.Facets.AtHomeServices:
                case SRPLocators.Facets.PricePaymentsParent:
                case SRPLocators.Facets.MechanicalParent:
                case SRPLocators.Facets.InteriorParent:
                case SRPLocators.Facets.OtherOptionsParent:
                    OpenFilters();
                    IWebElement parentFacet = FindElement(By.CssSelector(facet.GetAttribute<FacetLocatorAttribute>().FacetLocator));
                    ScrollTo(parentFacet);
                    if (IsFacetOpen(facet, statusIndicator) != (facetStatus == FacetStatus.Open))
                    {
                        try
                        {
                            ClickElement(parentFacet);
                        }
                        catch (Exception)
                        {
                            ScrollTo(parentFacet);
                            ClickElement(parentFacet);
                        }
                        WaitForFacetStatus(facet, facetStatus, statusIndicator);
                    }
                    break;
                case SRPLocators.Facets.CityPostalCodeChild:
                case SRPLocators.Facets.SearchRadiusChild:
                case SRPLocators.Facets.MakeChild:
                case SRPLocators.Facets.ModelChild:
                case SRPLocators.Facets.TrimChild:
                case SRPLocators.Facets.BodyTypeChild:
                case SRPLocators.Facets.ColourChild:
                case SRPLocators.Facets.KilometersChild:
                case SRPLocators.Facets.YearChild:
                case SRPLocators.Facets.DrivetrainChild:
                case SRPLocators.Facets.FuelTypeChild:
                case SRPLocators.Facets.EngineChild:
                case SRPLocators.Facets.TransmissionChild:
                case SRPLocators.Facets.SeatingCapacityChild:
                case SRPLocators.Facets.DoorsChild:
                case SRPLocators.Facets.Type:
                case SRPLocators.Facets.Wheelbase:
                case SRPLocators.Facets.Length:
                case SRPLocators.Facets.Weight:
                case SRPLocators.Facets.Sleeps:
                case SRPLocators.Facets.SlideOuts:
                case SRPLocators.Facets.SubType:
                case SRPLocators.Facets.Hours:
                case SRPLocators.Facets.Horsepower:
                case SRPLocators.Facets.EngineSize:
                    throw new Exception("Not a parent facet!");
            }
        }

        public override void OpenFacetAbs(SRPLocators.Facets facets, FacetStatus facetStatus, SrpDesign srpDesign = SrpDesign.Old)
        {
            if (srpDesign == SrpDesign.Old)
            {
                OpenFilters();
                OpenChildFacetFlyout(facets, facetStatus);
            }
            else
            {
                switch (facets)
                {
                    case SRPLocators.Facets.LocationParent:
                    case SRPLocators.Facets.VehicleParent:
                    case SRPLocators.Facets.ConditionParent:
                    case SRPLocators.Facets.SellerTypeParent:
                    case SRPLocators.Facets.AtHomeServices:
                    case SRPLocators.Facets.PricePaymentsParent:
                    case SRPLocators.Facets.MechanicalParent:
                    case SRPLocators.Facets.InteriorParent:
                    case SRPLocators.Facets.OtherOptionsParent:
                        OpenParentFacetAbs(facets, facetStatus);
                        break;
                    case SRPLocators.Facets.CityPostalCodeChild:
                        OpenParentFacetAbs(SRPLocators.Facets.LocationParent, FacetStatus.Open);
                        OpenChildFacetFlyout(facets, facetStatus);
                        break;
                    case SRPLocators.Facets.SearchRadiusChild:
                        OpenParentFacetAbs(SRPLocators.Facets.LocationParent, FacetStatus.Open);
                        break;
                    case SRPLocators.Facets.MakeChild:
                    case SRPLocators.Facets.ModelChild:
                    case SRPLocators.Facets.TrimChild:
                    case SRPLocators.Facets.BodyTypeChild:
                    case SRPLocators.Facets.ColourChild:
                        OpenParentFacetAbs(SRPLocators.Facets.VehicleParent, FacetStatus.Open);
                        OpenChildFacetFlyout(facets, facetStatus);
                        break;
                    case SRPLocators.Facets.KilometersChild:
                    case SRPLocators.Facets.YearChild:
                        OpenParentFacetAbs(SRPLocators.Facets.ConditionParent, FacetStatus.Open);
                        break;
                    case SRPLocators.Facets.DrivetrainChild:
                    case SRPLocators.Facets.FuelTypeChild:
                    case SRPLocators.Facets.EngineChild:
                    case SRPLocators.Facets.TransmissionChild:
                        OpenParentFacetAbs(SRPLocators.Facets.MechanicalParent, FacetStatus.Open);
                        OpenChildFacetFlyout(facets, facetStatus);
                        break;
                    case SRPLocators.Facets.SeatingCapacityChild:
                    case SRPLocators.Facets.DoorsChild:
                        OpenParentFacetAbs(SRPLocators.Facets.InteriorParent, FacetStatus.Open);
                        OpenChildFacetFlyout(facets, facetStatus);
                        break;
                    case SRPLocators.Facets.Type:
                    case SRPLocators.Facets.Wheelbase:
                    case SRPLocators.Facets.Length:
                    case SRPLocators.Facets.Weight:
                    case SRPLocators.Facets.Sleeps:
                    case SRPLocators.Facets.SlideOuts:
                    case SRPLocators.Facets.SubType:
                    case SRPLocators.Facets.Hours:
                    case SRPLocators.Facets.Horsepower:
                    case SRPLocators.Facets.EngineSize:
                        OpenFilters();
                        OpenChildFacetFlyout(facets, facetStatus);
                        break;
                }
            }
        }

        public override void CloseChildFacetFlyoutAbs(SRPLocators.Facets facet, SrpDesign srpDesign = SrpDesign.Old)
        {
            switch (srpDesign)
            {
                case SrpDesign.Old:
                    if (IsFacetOpen(facet, "open"))
                    {
                        IWebElement facetCloseBtn = FindElement(By.CssSelector(facet.GetAttribute<CloseButtonLocatorAttribute>().CloseButtonLocator));
                        ClickElement(facetCloseBtn);
                        WaitForFacetStatus(facet, FacetStatus.Close, "open");
                    }
                    break;
                case SrpDesign.New:
                    if (IsFacetOpen(facet, "open"))
                    {
                        IWebElement facetCloseBtn = FindElement(By.CssSelector(facet.GetAttribute<BackButtonLocatorAttribute>().BackButtonLocator));
                        ClickElement(facetCloseBtn);
                        WaitForFacetStatus(facet, FacetStatus.Close, "open");
                    }
                    break;
            }
        }

        public void CloseFacetFlyoutOldSRP(SRPLocators.Facets facet)
        {
            OpenFilters();
            IWebElement facetElement = FindElement(By.CssSelector(facet.GetAttribute<FacetLocatorAttribute>().FacetLocator));
            ScrollTo(facetElement);
            if (IsFacetOpen(facet, "open"))
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
                WaitForFacetStatus(facet, FacetStatus.Close, "open");
            }
        }

        public override string GetSelectedValueOfFacetAbs(SRPLocators.Facets facets, HeaderFooterLocators.Categories category = HeaderFooterLocators.Categories.CarsTrucksSuv, SrpDesign srpDesign = SrpDesign.Old)
        {
            string facetValue = string.Empty;
            IWebElement element;
            switch (srpDesign)
            {
                case SrpDesign.Old:
                    CloseFacetFlyoutOldSRP(facets);
                    IList<IWebElement> listOfOptions = FindElements(By.CssSelector(facets.GetAttribute<SelectedFacetSpanAttribute>().SelectedFacetSpan));
                    int countOfOptions = CountElements(listOfOptions);
                    if (countOfOptions > 1)
                    {
                        foreach (IWebElement option in listOfOptions)
                        {
                            if (option.Text.Length > 0)
                            {
                                if (!string.IsNullOrEmpty(facetValue))
                                {
                                    facetValue = facetValue + option.Text + ", ";
                                }
                                else
                                {
                                    facetValue = option.Text + ", ";
                                }
                            }
                            else
                            {
                                if (facets == SRPLocators.Facets.SellerTypeParent && FindElements(By.CssSelector(facets.GetAttribute<SellerTypeOSPAttribute>().SellerTypeOSP), 5).Count > 0)
                                {
                                    if (!string.IsNullOrEmpty(facetValue))
                                    {
                                        facetValue = facetValue + FindElement(By.CssSelector(facets.GetAttribute<SellerTypeOSPAttribute>().SellerTypeOSP)).GetAttribute("alt") + ", ";
                                    }
                                    else
                                    {
                                        facetValue = FindElement(By.CssSelector(facets.GetAttribute<SellerTypeOSPAttribute>().SellerTypeOSP)).GetAttribute("alt") + ", ";
                                    }
                                }
                            }
                        }

                        facetValue = facetValue.Trim().TrimEnd(',');
                    }
                    else
                    {
                        if (countOfOptions != 0)
                        {
                            facetValue = FindElements(By.CssSelector(facets.GetAttribute<SelectedFacetSpanAttribute>().SelectedFacetSpan)).FirstOrDefault(x => x.Text.Length > 0).Text.Trim();
                        }
                    }
                    break;
                case SrpDesign.New:
                    OpenFacetAbs(facets, FacetStatus.Close);
                    element = FindElement(By.CssSelector(facets.GetAttribute<SelectedFacetSpanAttribute>().SelectedFacetSpan));
                    if (element.Displayed)
                    {
                        facetValue = element.Text.Trim();
                    }
                    else { return string.Empty; }
                    break;
            }

            ClickViewResultsBtn();
            return facetValue;
        }

        public override string GetFacetElementMinText(SRPLocators.Facets facet)
        {
            OpenFacetAbs(facet, FacetStatus.Open);
            IWebElement facetElementText = FindElement(By.CssSelector(facet.GetAttribute<MinValueFieldAttribute>().MinValueField));
            ScrollTo(facetElementText);
            string text = GetValueByJS("return document.querySelector('" + facet.GetAttribute<MinValueFieldAttribute>().MinValueField + "').value"); ;
            ClickViewResultsBtn();
            return text;
        }

        public override string GetFacetElementMaxText(SRPLocators.Facets facet)
        {
            OpenFacetAbs(facet, FacetStatus.Open);
            IWebElement facetElementText = FindElement(By.CssSelector(facet.GetAttribute<MaxValueFieldAttribute>().MaxValueField));
            ScrollTo(facetElementText);
            string text = GetValueByJS("return document.querySelector('" + facet.GetAttribute<MaxValueFieldAttribute>().MaxValueField + "').value");
            ClickViewResultsBtn();
            return text;
        }

        public override string CustomGetSelectedText(SRPLocators.Facets facet)
        {
            OpenFacetAbs(facet, FacetStatus.Open);
            IWebElement dropdownRowElement = FindElement(By.CssSelector(facet.GetAttribute<DropdownFacetRowLocatorAttribute>().DropdownFacetRowLocator));
            ScrollTo(dropdownRowElement);
            string text = GetElementText(dropdownRowElement.FindElement(By.CssSelector("button")));
            ClickViewResultsBtn();
            return text;
        }

        public override string GetYearMinSelectedText(SrpDesign srpDesign = SrpDesign.Old)
        {
            if (srpDesign == SrpDesign.Old)
            {
                OpenFacetAbs(SRPLocators.Facets.YearChild, FacetStatus.Open);
                IWebElement dropdownRowElement = FindElement(By.CssSelector(SRPLocators.Facets.YearChild.GetAttribute<MinDropdownOldSrpLocatorAttribute>().MinDropdownOldSrpLocator));
                ScrollTo(dropdownRowElement);
                string text = GetSelectedText(By.CssSelector(SRPLocators.Facets.YearChild.GetAttribute<MinDropdownOldSrpLocatorAttribute>().MinDropdownOldSrpLocator));
                ClickViewResultsBtn();
                return text;
            }
            else
            {
                OpenFacetAbs(SRPLocators.Facets.YearChild, FacetStatus.Open);
                IWebElement dropdownRowElement = FindElement(By.CssSelector(SRPLocators.Facets.YearChild.GetAttribute<MinDropdownFacetRowLocatorAttribute>().MinDropdownFacetRowLocator));
                ScrollTo(dropdownRowElement);
                string text = GetElementText(dropdownRowElement.FindElement(By.CssSelector("button")));
                ClickViewResultsBtn();
                return text;
            }
        }

        public override string GetYearMaxSelectedText(SrpDesign srpDesign = SrpDesign.Old)
        {
            if (srpDesign == SrpDesign.Old)
            {
                OpenFacetAbs(SRPLocators.Facets.YearChild, FacetStatus.Open);
                IWebElement dropdownRowElement = FindElement(By.CssSelector(SRPLocators.Facets.YearChild.GetAttribute<MaxDropdownOldSrpLocatorAttribute>().MaxDropdownOldSrpLocator));
                ScrollTo(dropdownRowElement);
                string text = GetSelectedText(By.CssSelector(SRPLocators.Facets.YearChild.GetAttribute<MaxDropdownOldSrpLocatorAttribute>().MaxDropdownOldSrpLocator));
                ClickViewResultsBtn();
                return text;
            }
            else
            {
                OpenFacetAbs(SRPLocators.Facets.YearChild, FacetStatus.Open);
                IWebElement dropdownRowElement = FindElement(By.CssSelector(SRPLocators.Facets.YearChild.GetAttribute<MaxDropdownOldSrpLocatorAttribute>().MaxDropdownOldSrpLocator));
                ScrollTo(dropdownRowElement);
                string text = GetElementText(dropdownRowElement.FindElement(By.CssSelector("button")));
                ClickViewResultsBtn();
                return text;
            }
        }

        public override void CloseFacetAbs(SRPLocators.Facets facets, SrpDesign srpDesign = SrpDesign.Old)
        {
            switch (srpDesign)
            {
                case SrpDesign.Old:
                    CloseFacetFlyoutOldSRP(facets);
                    break;
                case SrpDesign.New:
                    OpenFacetAbs(facets, SRPAbstract.FacetStatus.Close);
                    break;
            }
        }

        public override bool ContainsInFacet(SRPLocators.Facets facet, string value1 = _defaultValue, string value2 = _defaultValue, string value3 = _defaultValue, SrpDesign srpDesign = SrpDesign.Old)
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

        public override void SelectBuyingOptionsSingleAbs(SRPLocators.BuyingOptions contactlessServices, bool toBeChecked, SrpDesign srpDesign = SrpDesign.Old)
        {
            if (IsBuyingOptionsCheckedAbs(contactlessServices) != toBeChecked)
            {
                int totalNumberOfFound = GetTotalNumberOfViewResults();
                OpenFacetAbs(SRPLocators.Facets.AtHomeServices, FacetStatus.Open);
                IWebElement element = FindElement(By.CssSelector(contactlessServices.GetAttribute<CSPLabelLocatorAttribute>().CSPLabelLocator));
                ScrollTo(element);
                ClickElement(element);
                WaitUntilResultsChanged(totalNumberOfFound);
            }
            ClickViewResultsBtn();
        }

        public override bool IsBuyingOptionsCheckedAbs(SRPLocators.BuyingOptions contactlessServices, SrpDesign srpDesign = SrpDesign.Old)
        {
            OpenFacetAbs(SRPLocators.Facets.AtHomeServices, FacetStatus.Open);
            bool isSelected = FindElements(By.CssSelector(contactlessServices.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault().Selected;
            ClickViewResultsBtn();
            return isSelected;
        }

        public override void WaitForSRPPageLoad(int timeOut = 60)
        {
            //Need to wait until all elements are loaded otherwise location displays old stale value from filter #6542, #6540
            WaitUntil(() => IsElementVisible(By.CssSelector(SRPLocators.XSLocators.SearchExpansionWarning.GetAttribute<DescriptionAttribute>().Description)) || IsElementVisible(By.CssSelector(SRPLocators.XSLocators.ZeroListingWarning.GetAttribute<DescriptionAttribute>().Description)) || (driver.FindElements(By.CssSelector(SRPLocators.XSLocators.FirstListing.GetAttribute<DescriptionAttribute>().Description)).Count > 0 && driver.FindElements(By.CssSelector(SRPLocators.XSLocators.FirstListing.GetAttribute<DescriptionAttribute>().Description)).LastOrDefault().Displayed), timeOut);
        }

        internal void ClickOnApplyOnFacet(SRPLocators.Facets facet, int totalNumberOfViewResults)
        {
            if (IsFacetOpen(facet, "open"))
            {
                try
                {
                    IWebElement element = FindElement(By.CssSelector(facet.GetAttribute<ApplyButtonSelectorAttribute>().ApplyButtonSelector));
                    ScrollTo(element);
                    ClickElement(element, 20);
                    WaitForFacetStatus(facet, FacetStatus.Close, "open");
                }
                catch (Exception)
                {
                    //Do nothing. Old design does not always have apply button on XS
                }
                WaitUntilResultsChanged(totalNumberOfViewResults);
            }
        }

        public override void SelectYearFacetAbs(SRPLocators.Year year, string minYear, string maxYear, SrpDesign srpDesign = SrpDesign.Old)
        {
            int totalNumberOfFound = GetTotalNumberOfViewResults();
            OpenFacetAbs(SRPLocators.Facets.YearChild, FacetStatus.Open);
            if (srpDesign == SrpDesign.Old)
            {
                switch (year)
                {
                    case SRPLocators.Year.MinYear:
                        if (!string.IsNullOrEmpty(minYear))
                        {
                            SelectByValue(By.CssSelector(SRPLocators.Facets.YearChild.GetAttribute<MinDropdownOldSrpLocatorAttribute>().MinDropdownOldSrpLocator), minYear);
                        }
                        break;
                    case SRPLocators.Year.MaxYear:
                        if (!string.IsNullOrEmpty(maxYear))
                        {
                            SelectByValue(By.CssSelector(SRPLocators.Facets.YearChild.GetAttribute<MaxDropdownOldSrpLocatorAttribute>().MaxDropdownOldSrpLocator), maxYear);
                        }
                        break;
                    case SRPLocators.Year.Both:
                        if (!string.IsNullOrEmpty(minYear) && !string.IsNullOrEmpty(maxYear))
                        {
                            SelectByValue(By.CssSelector(SRPLocators.Facets.YearChild.GetAttribute<MinDropdownOldSrpLocatorAttribute>().MinDropdownOldSrpLocator), minYear);
                            SelectByValue(By.CssSelector(SRPLocators.Facets.YearChild.GetAttribute<MaxDropdownOldSrpLocatorAttribute>().MaxDropdownOldSrpLocator), maxYear);
                        }
                        break;
                }
                ClickOnApplyOnFacet(SRPLocators.Facets.YearChild, totalNumberOfFound);
                ClickViewResultsBtn();
            }
            else
            {
                switch (year)
                {
                    case SRPLocators.Year.MinYear:
                        if (!string.IsNullOrEmpty(minYear))
                        {
                            CustomSelectByValue(FindElement(By.CssSelector(SRPLocators.Facets.YearChild.GetAttribute<MinDropdownLocatorAttribute>().MinDropdownLocator)), FindElement(By.CssSelector(SRPLocators.Facets.YearChild.GetAttribute<MinDropdownFacetRowLocatorAttribute>().MinDropdownFacetRowLocator)), minYear);
                        }
                        break;
                    case SRPLocators.Year.MaxYear:
                        if (!string.IsNullOrEmpty(maxYear))
                        {
                            CustomSelectByValue(FindElement(By.CssSelector(SRPLocators.Facets.YearChild.GetAttribute<MaxDropdownLocatorAttribute>().MaxDropdownLocator)), FindElement(By.CssSelector(SRPLocators.Facets.YearChild.GetAttribute<MaxDropdownFacetRowLocatorAttribute>().MaxDropdownFacetRowLocator)), maxYear);
                        }
                        break;
                    case SRPLocators.Year.Both:
                        if (!string.IsNullOrEmpty(minYear) && !string.IsNullOrEmpty(maxYear))
                        {
                            CustomSelectByValue(FindElement(By.CssSelector(SRPLocators.Facets.YearChild.GetAttribute<MinDropdownLocatorAttribute>().MinDropdownLocator)), FindElement(By.CssSelector(SRPLocators.Facets.YearChild.GetAttribute<MinDropdownFacetRowLocatorAttribute>().MinDropdownFacetRowLocator)), minYear);
                            CustomSelectByValue(FindElement(By.CssSelector(SRPLocators.Facets.YearChild.GetAttribute<MaxDropdownLocatorAttribute>().MaxDropdownLocator)), FindElement(By.CssSelector(SRPLocators.Facets.YearChild.GetAttribute<MaxDropdownFacetRowLocatorAttribute>().MaxDropdownFacetRowLocator)), maxYear);
                        }
                        break;
                }
                ClickViewResultsBtn();
            }
        }

        public override void SelectPaymentsFacetAbs(string minPayment, string maxPayment, SRPLocators.PaymentFrequency paymentFrequency, SRPLocators.Term term, string downPayment, string tradeInValue, SrpDesign srpDesign = SrpDesign.Old)
        {
            switch (srpDesign)
            {
                case SrpDesign.Old:
                    OpenFacetAbs(SRPLocators.Facets.PricePaymentsParent, FacetStatus.Open);
                    ClickElement(FindElement(By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<PaymentsTabOldSRPAttribute>().PaymentsTabOldSRP)));
                    
                    By minPaymentLocator = By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<MinPaymentFieldAttribute>().MinPaymentField);
                    ClearField(FindElement(minPaymentLocator));
                    FindElement(minPaymentLocator).SendKeys(minPayment);
                    ScrollTo(FindElement(minPaymentLocator));
                    ClickElement(FindElement(minPaymentLocator));
                    UnFocusElementJS(FindElement(minPaymentLocator));
                    By maxPaymentLocator = By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<MaxPaymentFieldAttribute>().MaxPaymentField);
                    ClearField(FindElement(maxPaymentLocator));
                    FindElement(maxPaymentLocator).SendKeys(maxPayment);
                    ScrollTo(FindElement(maxPaymentLocator));
                    ClickElement(FindElement(maxPaymentLocator));
                    UnFocusElementJS(FindElement(maxPaymentLocator));

                    By paymentFrequencyDropdownLocator = By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<PaymentFrequencyDropdownAttribute>().PaymentFrequencyDropdown);
                    ScrollTo(FindElement(paymentFrequencyDropdownLocator));
                    SelectByValue(paymentFrequencyDropdownLocator, paymentFrequency.GetAttribute<DescriptionOldSRPAttribute>().DescriptionOldSRP);

                    By termsDropdownLocator = By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<TermDropdownAttribute>().TermDropdown);
                    ScrollTo(FindElement(termsDropdownLocator));
                    SelectByValue(termsDropdownLocator, term.GetAttribute<DescriptionAttribute>().Description);

                    IWebElement downPaymentElement = FindElement(By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<DownPaymentFieldAttribute>().DownPaymentField));
                    ClearField(downPaymentElement);
                    downPaymentElement.SendKeys(downPayment);

                    IWebElement tradeInValueElement = FindElement(By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<TradeInValueFieldAttribute>().TradeInValueField));
                    ClearField(tradeInValueElement);
                    tradeInValueElement.SendKeys(tradeInValue);

                    int totalNumberOfFound = GetTotalNumberOfViewResults();
                    ScrollTo(FindElement(maxPaymentLocator));
                    ClickElement(FindElement(maxPaymentLocator));
                    UnFocusElementJS(FindElement(maxPaymentLocator), 2);

                    ClickOnApplyOnFacet(SRPLocators.Facets.PricePaymentsParent, totalNumberOfFound);
                    ClickViewResultsBtn();
                    break;
                case SrpDesign.New:
                    ClearAllFacetsAbs();  //Finance max payment field does not clear with element.clear()
                    OpenFacetAbs(SRPLocators.Facets.PricePaymentsParent, FacetStatus.Open);
                    ClickElement(FindElement(By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<PaymentsTabNewSRPAttribute>().PaymentsTabNewSRP)));
                    var tabRowLocator = FindElement(By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<PaymentsTabRowLocatorAttribute>().PaymentsTabRowLocator));
                    WaitUntil(() => tabRowLocator.GetAttribute("class").Contains("active"), 5);

                    By minPaymentLocatorNewSrp = By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<MinPaymentFieldAttribute>().MinPaymentField);
                    EnterText(minPaymentLocatorNewSrp, minPayment);
                    ScrollTo(FindElement(minPaymentLocatorNewSrp));
                    ClickElement(FindElement(minPaymentLocatorNewSrp));
                    UnFocusElementJS(FindElement(minPaymentLocatorNewSrp));
                    By maxPaymentLocatorNewSrp = By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<MaxPaymentFieldAttribute>().MaxPaymentField);
                    EnterText(maxPaymentLocatorNewSrp, maxPayment);
                    ScrollTo(FindElement(maxPaymentLocatorNewSrp));
                    ClickElement(FindElement(maxPaymentLocatorNewSrp));
                    UnFocusElementJS(FindElement(maxPaymentLocatorNewSrp));

                    IWebElement paymentFrequencyElementNewSrp = FindElement(By.CssSelector(paymentFrequency.GetAttribute<DescriptionAttribute>().Description));
                    ScrollTo(paymentFrequencyElementNewSrp);
                    if (!IsCheckboxChecked(paymentFrequencyElementNewSrp))
                    {
                        ClickElement(FindElement(By.CssSelector(paymentFrequency.GetAttribute<DescriptionAttribute>().Description + " + span.design")));
                    }

                    By termsDropdownLocatorNewSrp = By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<TermDropdownAttribute>().TermDropdown);
                    ScrollTo(FindElement(termsDropdownLocatorNewSrp));
                    SelectByValue(termsDropdownLocatorNewSrp, term.GetAttribute<DescriptionAttribute>().Description);

                    By downPaymentLocatorNewSrp = By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<DownPaymentFieldAttribute>().DownPaymentField);
                    EnterText(downPaymentLocatorNewSrp, downPayment);
                    ScrollTo(FindElement(downPaymentLocatorNewSrp));
                    ClickElement(FindElement(downPaymentLocatorNewSrp));
                    UnFocusElementJS(FindElement(downPaymentLocatorNewSrp), 3);

                    int totalNumberOfFoundNewSrp = GetTotalNumberOfViewResults();
                    By tradeInValueLocatorNewSrp = By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<TradeInValueFieldAttribute>().TradeInValueField);
                    EnterText(tradeInValueLocatorNewSrp, tradeInValue);
                    ScrollTo(FindElement(tradeInValueLocatorNewSrp));
                    ClickElement(FindElement(tradeInValueLocatorNewSrp));
                    UnFocusElementJS(FindElement(tradeInValueLocatorNewSrp));

                    WaitUntilResultsChanged(totalNumberOfFoundNewSrp);
                    ClickViewResultsBtn();
                    break;
            }
        }

        public override void SelectCheckboxMultipleAbs(SRPLocators.Facets facet, List<string> listOfOption, bool toBeChecked)
        {
            int totalNumberOfFound = GetTotalNumberOfViewResults();
            OpenFacetAbs(facet, FacetStatus.Open);
            foreach (string option in listOfOption)
            {
                IWebElement element = FindElements(By.CssSelector(facet.GetAttribute<FacetListAttribute>().FacetList)).FirstOrDefault(x => x.GetAttribute("data-value") == option);
                element = (element != null) ? element : throw new Exception("The option to be selected in the facet " + facet + " is not available.");
                ScrollTo(element);
                if (IsCheckboxChecked(element) != toBeChecked)
                {
                    if (option == "Other/Don't Know")
                    {
                        int maxTry = 0;
                        while (true && maxTry < 5)
                        {
                            try
                            {
                                ClickElement(FindElement(By.CssSelector(facet.GetAttribute<FacetListAttribute>().FacetList + "[data-value=\"Other/Don't Know\"] + label")));
                                break;
                            }
                            catch (Exception)
                            {
                                ScrollTo(FindElement(By.CssSelector(facet.GetAttribute<FacetListAttribute>().FacetList + "[data-value=\"Other/Don't Know\"] + label")));
                                maxTry++;
                            }
                        }
                    }
                    else
                    {
                        int maxTry = 0;
                        while (true && maxTry < 5)
                        {
                            try
                            {
                                ClickElement(FindElement(By.CssSelector(facet.GetAttribute<FacetListAttribute>().FacetList + "[data-value='" + option + "'] + label")));
                                break;
                            }
                            catch (Exception)
                            {
                                ScrollTo(FindElement(By.CssSelector(facet.GetAttribute<FacetListAttribute>().FacetList + "[data-value='" + option + "'] + label")));
                                maxTry++;
                            }
                        }
                    }
                }
            }

            ClickOnApplyOnFacet(facet, totalNumberOfFound);
            ClickViewResultsBtn();
        }

        public override bool IsCheckboxStatusMultiple(SRPLocators.Facets facet, Dictionary<string, bool> listOfOptionStatus)
        {
            bool status = true;
            OpenFacetAbs(facet, FacetStatus.Open);
            IList<IWebElement> listOfElements = FindElements(By.CssSelector(facet.GetAttribute<FacetListAttribute>().FacetList));
            foreach (KeyValuePair<string, bool> optionStatus in listOfOptionStatus)
            {
                IWebElement element = listOfElements.FirstOrDefault(x => x.GetAttribute("data-value").ToLower() == optionStatus.Key.ToLower());
                element = (element != null) ? element : throw new Exception("The option to be verified in the facet " + facet + " is not available.");
                ScrollTo(element);
                if (IsCheckboxChecked(element) == optionStatus.Value)
                {
                    continue;
                }
                else
                {
                    status = false;
                    break;
                }
            }
            CloseFacetFlyoutOldSRP(facet);
            ClickViewResultsBtn();
            return status;
        }

        public override void SelectEngineFacetSingleAbs(SRPLocators.Engine engine, bool toBeChecked)
        {
            int totalNumberOfFound = GetTotalNumberOfViewResults();
            OpenFacetAbs(SRPLocators.Facets.EngineChild, FacetStatus.Open);
            string locator = (language.ToString() == "EN") ? engine.GetAttribute<DescriptionAttribute>().Description : engine.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench;
            IWebElement element = FindElements(By.CssSelector(locator)).FirstOrDefault();  //Element considered not Displayed
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(locator + " + label")));
            }
            ClickOnApplyOnFacet(SRPLocators.Facets.EngineChild, totalNumberOfFound);
            ClickViewResultsBtn();
        }

        public override void SelectTransmissionFacetSingleAbs(SRPLocators.Transmission transmission, bool toBeChecked)
        {
            int totalNumberOfFound = GetTotalNumberOfViewResults();
            OpenFacetAbs(SRPLocators.Facets.TransmissionChild, FacetStatus.Open);
            string locator = (language.ToString() == "EN") ? transmission.GetAttribute<DescriptionAttribute>().Description : transmission.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench;
            IWebElement element = FindElements(By.CssSelector(locator)).FirstOrDefault();
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(locator + " + label")));
            }
            ClickOnApplyOnFacet(SRPLocators.Facets.TransmissionChild, totalNumberOfFound);
            ClickViewResultsBtn();
        }

        public override void EnterMinMaxFacetAbs(SRPLocators.Facets facet, string minValue, string maxValue, SrpDesign srpDesign = SrpDesign.Old)
        {
            int totalNumberOfFound = 0;
            switch (srpDesign)
            {
                case SrpDesign.Old:
                    totalNumberOfFound = GetTotalNumberOfViewResults();
                    OpenFacetAbs(facet, FacetStatus.Open);
                    if (facet == SRPLocators.Facets.PricePaymentsParent)
                    {
                        ClickElement(FindElement(By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<PriceTabOldSRPAttribute>().PriceTabOldSRP)));
                    }
                    if (!string.IsNullOrEmpty(minValue))
                    {
                        By locator = By.CssSelector(facet.GetAttribute<MinValueFieldAttribute>().MinValueField);
                        EnterText(locator, minValue);
                    }

                    if (!string.IsNullOrEmpty(maxValue))
                    {
                        By locator = By.CssSelector(facet.GetAttribute<MaxValueFieldAttribute>().MaxValueField);
                        EnterText(locator, maxValue);
                    }

                    By maxValueLocator = By.CssSelector(facet.GetAttribute<MaxValueFieldAttribute>().MaxValueField);
                    ScrollTo(FindElement(maxValueLocator));
                    ClickElement(FindElement(maxValueLocator));
                    UnFocusElementJS(FindElement(maxValueLocator));
                    ClickOnApplyOnFacet(facet, totalNumberOfFound);
                    ClickViewResultsBtn();
                    break;
                case SrpDesign.New:
                    totalNumberOfFound = GetTotalNumberOfViewResults();
                    OpenFacetAbs(facet, FacetStatus.Open);
                    if (facet == SRPLocators.Facets.PricePaymentsParent)
                    {
                        ClickElement(FindElement(By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<PriceTabNewSRPAttribute>().PriceTabNewSRP)));
                        var tabRowLocator = FindElement(By.CssSelector(facet.GetAttribute<PriceTabRowLocatorAttribute>().PriceTabRowLocator));
                        WaitUntil(() => tabRowLocator.GetAttribute("class").Contains("active"), 5);
                    }
                    if (!string.IsNullOrEmpty(minValue))
                    {
                        By locator = By.CssSelector(facet.GetAttribute<MinValueFieldAttribute>().MinValueField);
                        EnterText(locator, minValue);
                        ScrollTo(FindElement(locator));
                        ClickElement(FindElement(locator));
                        UnFocusElementJS(FindElement(locator));
                    }
                    if (!string.IsNullOrEmpty(maxValue))
                    {
                        By locator = By.CssSelector(facet.GetAttribute<MaxValueFieldAttribute>().MaxValueField);
                        EnterText(locator, maxValue);
                        ScrollTo(FindElement(locator));
                        ClickElement(FindElement(locator));
                        UnFocusElementJS(FindElement(locator));
                    }
                    WaitUntilResultsChanged(totalNumberOfFound);
                    ClickViewResultsBtn();
                    break;
            }
        }

        public override void ClearAllFacetsAbs()
        {
            int totalNumberOfFound = GetTotalNumberOfViewResults();
            OpenFilters();
            IWebElement clearAllButton = FindElement(By.CssSelector(SRPLocators.CommonLocators.ClearAll.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(clearAllButton);
            WaitUntilResultsChanged(totalNumberOfFound);
            ClickViewResultsBtn();
        }

        public override void SelectConditionFacetSingleAbs(SRPLocators.Condition condition, bool toBeChecked, SrpDesign srpDesign = SrpDesign.Old)
        {
            int totalNumberOfFound = GetTotalNumberOfViewResults();
            OpenFacetAbs(SRPLocators.Facets.ConditionParent, FacetStatus.Open);
            IWebElement element = FindElements(By.CssSelector(condition.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault();
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(condition.GetAttribute<ConditionLabelAttribute>().ConditionLabel)));
            }
            WaitUntilResultsChanged(totalNumberOfFound);
            ClickViewResultsBtn();
        }

        public override bool IsOtherOptionCheckedAbs(SRPLocators.OtherOptions otherOptions)
        {
            OpenFilters();
            return FindElements(By.CssSelector(otherOptions.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault().Selected;
        }

        public override void SelectSellerTypeFacetAbs(SRPLocators.SellerType sellerType, bool toBeChecked, SrpDesign srpDesign = SrpDesign.Old)
        {
            int totalNumberOfFound = GetTotalNumberOfViewResults();
            OpenFacetAbs(SRPLocators.Facets.SellerTypeParent, FacetStatus.Open);
            IWebElement element = FindElements(By.CssSelector(sellerType.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault();  //Element considered not displayed
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(sellerType.GetAttribute<SellerTypeLabelAttribute>().SellerTypeLabel)));
            }
            WaitUntilResultsChanged(totalNumberOfFound);
            ClickViewResultsBtn();
        }

        public override void SelectTrimFacetSingleAbs(string trim, bool toBeChecked)
        {
            int totalNumberOfFound = GetTotalNumberOfViewResults();
            OpenFacetAbs(SRPLocators.Facets.TrimChild, FacetStatus.Open);
            IWebElement element = FindElements(By.CssSelector(SRPLocators.Facets.TrimChild.GetAttribute<FacetListAttribute>().FacetList)).FirstOrDefault(x => x.GetAttribute("data-value") == trim);
            element = (element != null) ? element : throw new Exception("The option to be selected in the facet Trim is not available.");
            ScrollTo(element);
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                if (trim == "Other/Don't Know")
                {
                    ClickElement(FindElement(By.CssSelector(SRPLocators.Facets.TrimChild.GetAttribute<FacetListAttribute>().FacetList + "[data-value=\"Other/Don't Know\"] + label")));
                }
                else
                {
                    ClickElement(FindElement(By.CssSelector(SRPLocators.Facets.TrimChild.GetAttribute<FacetListAttribute>().FacetList + "[data-value='" + trim + "'] + label")));
                }
            }
            ClickOnApplyOnFacet(SRPLocators.Facets.TrimChild, totalNumberOfFound);
            ClickViewResultsBtn();
        }

        public override void SelectBodyTypeFacetSingleAbs(SRPLocators.BodyType bodyType, bool toBeChecked)
        {
            int totalNumberOfFound = GetTotalNumberOfViewResults();
            OpenFacetAbs(SRPLocators.Facets.BodyTypeChild, FacetStatus.Open);
            string locator = (language.ToString() == "EN") ? bodyType.GetAttribute<DescriptionAttribute>().Description : bodyType.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench;
            IWebElement element = FindElements(By.CssSelector(locator)).FirstOrDefault();
            ScrollTo(element);

            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(locator + " + label")));
            }
            ClickOnApplyOnFacet(SRPLocators.Facets.BodyTypeChild, totalNumberOfFound);
            ClickViewResultsBtn();
        }

        public override void SelectExteriorColourFacetSingleAbs(SRPLocators.ExteriorColour exteriorColour, bool toBeChecked)
        {
            int totalNumberOfFound = GetTotalNumberOfViewResults();
            OpenFacetAbs(SRPLocators.Facets.ColourChild, FacetStatus.Open);
            string locator = (language.ToString() == "EN") ? exteriorColour.GetAttribute<DescriptionAttribute>().Description : exteriorColour.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench;
            IWebElement element = FindElements(By.CssSelector(locator)).FirstOrDefault();
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(locator + " + label")));
            }
            ClickOnApplyOnFacet(SRPLocators.Facets.ColourChild, totalNumberOfFound);
            ClickViewResultsBtn();
        }

        public override void SelectDrivetrainFacetSingleAbs(SRPLocators.Drivetrain drivetrain, bool toBeChecked)
        {
            int totalNumberOfFound = GetTotalNumberOfViewResults();
            OpenFacetAbs(SRPLocators.Facets.DrivetrainChild, FacetStatus.Open);
            string locator = (language.ToString() == "EN") ? drivetrain.GetAttribute<DescriptionAttribute>().Description : drivetrain.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench;
            IWebElement element = FindElements(By.CssSelector(locator)).FirstOrDefault();
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(locator + " + label")));
            }
            ClickOnApplyOnFacet(SRPLocators.Facets.DrivetrainChild, totalNumberOfFound);
            ClickViewResultsBtn();
        }

        public override void SelectFuelTypeFacetSingleAbs(SRPLocators.FuelType fuelType, bool toBeChecked)
        {
            int totalNumberOfFound = GetTotalNumberOfViewResults();
            OpenFacetAbs(SRPLocators.Facets.FuelTypeChild, FacetStatus.Open);
            string locator = (language.ToString() == "EN") ? fuelType.GetAttribute<DescriptionAttribute>().Description : fuelType.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench;
            IWebElement element = FindElements(By.CssSelector(locator)).FirstOrDefault();
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(locator + " + label")));
            }
            ClickOnApplyOnFacet(SRPLocators.Facets.FuelTypeChild, totalNumberOfFound);
            ClickViewResultsBtn();
        }

        public override void SelectSeatingCapacityFacetSingleAbs(SRPLocators.SeatingCapacity seatingCapacity, bool toBeChecked)
        {
            int totalNumberOfFound = GetTotalNumberOfViewResults();
            OpenFacetAbs(SRPLocators.Facets.SeatingCapacityChild, FacetStatus.Open);
            string locator = (language.ToString() == "EN") ? seatingCapacity.GetAttribute<DescriptionAttribute>().Description : seatingCapacity.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench;
            IWebElement element = FindElements(By.CssSelector(locator)).FirstOrDefault();
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(locator + " + label")));
            }
            ClickOnApplyOnFacet(SRPLocators.Facets.SeatingCapacityChild, totalNumberOfFound);
            ClickViewResultsBtn();
        }

        public override void SelectDoorsFacetSingleAbs(SRPLocators.Doors doors, bool toBeChecked)
        {
            int totalNumberOfFound = GetTotalNumberOfViewResults();
            OpenFacetAbs(SRPLocators.Facets.DoorsChild, FacetStatus.Open);
            string locator = (language.ToString() == "EN") ? doors.GetAttribute<DescriptionAttribute>().Description : doors.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench;
            IWebElement element = FindElements(By.CssSelector(locator)).FirstOrDefault();
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(locator + " + label")));
            }
            ClickOnApplyOnFacet(SRPLocators.Facets.DoorsChild, totalNumberOfFound);
            ClickViewResultsBtn();
        }

        public override void SelectSleepsFacetSingleAbs(SRPLocators.Sleeps sleeps, bool toBeChecked)
        {
            OpenFacetAbs(SRPLocators.Facets.Sleeps, FacetStatus.Open);
            IWebElement element = FindElements(By.CssSelector(sleeps.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault();
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(sleeps.GetAttribute<DescriptionAttribute>().Description + " + label")));
            }
            ClickViewResultsBtn();
        }

        public override void SelectSubTypeFacetSingleAbs(SRPLocators.SubType subType, bool toBeChecked)
        {
            int totalNumberOfFound = GetTotalNumberOfViewResults();
            OpenFacetAbs(SRPLocators.Facets.SubType, FacetStatus.Open);
            string locator = (language.ToString() == "EN") ? subType.GetAttribute<DescriptionAttribute>().Description : subType.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench;
            IWebElement element = FindElement(By.CssSelector(locator));
            ScrollTo(element);
            ClickElement(element);
            WaitUntilResultsChanged(totalNumberOfFound);
            ClickViewResultsBtn();
        }

        public override void SelectFirstCheckboxFromFacetAbs(SRPLocators.Facets facet)
        {
            if (facet != SRPLocators.Facets.ColourChild && facet != SRPLocators.Facets.TransmissionChild && facet != SRPLocators.Facets.SeatingCapacityChild && facet != SRPLocators.Facets.DoorsChild && facet != SRPLocators.Facets.FuelTypeChild && facet != SRPLocators.Facets.SlideOuts && facet != SRPLocators.Facets.Sleeps && facet != SRPLocators.Facets.BodyTypeChild)
            {
                throw new Exception("This method cannot work for the selected facet.");
            }
            else
            {
                int totalNumberOfFound = GetTotalNumberOfViewResults();
                OpenFacetAbs(facet, FacetStatus.Open);
                ClickElement(FindElements(By.CssSelector(facet.GetAttribute<FacetLabelAttribute>().FacetLabel)).FirstOrDefault(x => x.Text.Length > 0));  //Make sure of intended first checkbox
                ClickOnApplyOnFacet(facet, totalNumberOfFound);
                ClickViewResultsBtn();
            }
        }

        public override void SelectOtherOptionsSingleAbs(SRPLocators.OtherOptions otherOptions, bool toBeChecked, SrpDesign srpDesign = SrpDesign.Old)
        {
            int totalNumberOfFound = GetTotalNumberOfViewResults();
            OpenFacetAbs(SRPLocators.Facets.OtherOptionsParent, FacetStatus.Open);
            IWebElement element = FindElements(By.CssSelector(otherOptions.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault();
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(otherOptions.GetAttribute<DescriptionAttribute>().Description + " + label")));
            }
            WaitUntilResultsChanged(totalNumberOfFound);
            ClickViewResultsBtn();
        }

        public override bool ContainsInListingTitleAbs(string value, string toCompare = _defaultValue, string value2 = _defaultValue)
        {
            bool result = false;
            IList<IWebElement> listingTitles = FindElements(By.CssSelector(SRPLocators.XSLocators.OrganicListingTitle.GetAttribute<DescriptionAttribute>().Description));

            if (string.IsNullOrEmpty(toCompare))
            {
                foreach (var item in listingTitles)
                {
                    if (item.Text.Contains(value))
                    {
                        result = true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                int numValue = Convert.ToInt32(value);
                int numValue2 = Convert.ToInt32(value2);
                IList<int> numbers = new List<int>();

                foreach (var item in listingTitles)
                {
                    var text = item.Text;
                    if (!string.IsNullOrEmpty(text))
                    {
                        numbers.Add(Convert.ToInt32(text.Substring(0, 4)));
                    }
                }

                // Only MaxYear
                if (toCompare == "<")
                {
                    foreach (var item in numbers)
                    {
                        if (item <= numValue)
                        {
                            result = true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                // Only MinYear
                if (toCompare == ">")
                {
                    foreach (var item in numbers)
                    {
                        if (item >= numValue)
                        {
                            result = true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                // Both, numValue = minYear, numValue2 = maxYear
                if (toCompare == "<>")
                {
                    foreach (var item in numbers)
                    {
                        if (item >= numValue && item <= numValue2)
                        {
                            result = true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            return result;
        }

        public override bool ListingPriceAbs(string value, string toCompare = _defaultValue, string value2 = _defaultValue)
        {
            bool result = false;
            IList<IWebElement> listingPrices = FindElements(By.CssSelector(SRPLocators.XSLocators.ListPrice.GetAttribute<DescriptionAttribute>().Description));

            int numValue = Convert.ToInt32(value);
            int numValue2 = Convert.ToInt32(value2);
            IList<int> numbers = new List<int>();

            foreach (var item in listingPrices)
            {
                var text = item.Text;
                if (!string.IsNullOrEmpty(text))
                {
                    try
                    {
                        numbers.Add(Convert.ToInt32(text.Replace(",", "").Replace("$", "").Replace(" ", "").Trim()));
                    }
                    catch (FormatException) { continue; }  //Skip non-integer items, got formatexception few times on XS
                }
            }

            // Only MaxPrice
            if (toCompare == "<")
            {
                foreach (var item in numbers)
                {
                    if (item <= numValue)
                    {
                        result = true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            // Only MinPrice
            if (toCompare == ">")
            {
                foreach (var item in numbers)
                {
                    if (item >= numValue)
                    {
                        result = true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            // Both, numValue = minPrice, numValue2 = maxPrice
            if (toCompare == "<>")
            {
                foreach (var item in numbers)
                {
                    if (item >= numValue && item <= numValue2)
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

        public override bool IsMileageWithinRangeAbs(string value, string toCompare = _defaultValue, string value2 = _defaultValue)
        {
            bool result = false;
            IList<IWebElement> proximityLabels = FindElements(By.CssSelector(SRPLocators.XSLocators.Mileage.GetAttribute<DescriptionAttribute>().Description));  //Excludes featured ad

            int numValue = Convert.ToInt32(value);
            int numValue2 = Convert.ToInt32(value2);

            IList<int> numbers = new List<int>();
            foreach (var item in proximityLabels)
            {
                var text = item.Text;
                if (!string.IsNullOrEmpty(text))
                {
                    numbers.Add(Convert.ToInt32(Regex.Replace(text, @"\D", "")));
                }
            }

            // Only MaxKms
            if (toCompare == "<")
            {
                foreach (var item in numbers)
                {
                    if (item <= numValue)
                    {
                        result = true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            // Only MinKms
            if (toCompare == ">")
            {
                foreach (var item in numbers)
                {
                    if (item >= numValue)
                    {
                        result = true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            // Both, numValue = minKms, numValue2 = maxKms
            if (toCompare == "<>")
            {
                foreach (var item in numbers)
                {
                    if (item >= numValue && item <= numValue2)
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

        public override bool IsProximityWithinRangeAbs(int proximity)
        {
            bool result = false;

            IList<IWebElement> proximityLabels = FindElements(By.CssSelector(SRPLocators.XSLocators.AllOrganicProximity.GetAttribute<DescriptionAttribute>().Description));  //Excludes featured ad  //Bug https://trader.atlassian.net/browse/MCTE-620 : 'From Areas You Might Also Consider' section ads are included
            IList<int> numbers = new List<int>();
            foreach (var item in proximityLabels)
            {
                var text = item.Text;  //Store text here. Calculating text later while adding to list sometimes gives stale element error.
                if (!string.IsNullOrEmpty(text) && text.Any(c => char.IsDigit(c)))  //Not empty and not a virtual dealer
                {
                    numbers.Add(Convert.ToInt32(Regex.Replace(text, @"\D", "")));
                }
            }

            foreach (var item in numbers)
            {
                if (item <= proximity)
                {
                    result = true;
                }
                else
                {
                    return false;
                }
            }
            return result;
        }

        public override bool IsEveryListingPrivateAbs()
        {
            IList<IWebElement> listingTitles = FindElements(By.CssSelector(SRPLocators.XSLocators.ListingTitle.GetAttribute<DescriptionAttribute>().Description));
            IList<IWebElement> privateBadges = FindElements(By.CssSelector(SRPLocators.XSLocators.PrivateBadge.GetAttribute<DescriptionAttribute>().Description));

            return CountElements(listingTitles) == CountVisibleElements(privateBadges);
        }

        public override bool IsEveryListingDealerAbs()
        {
            IList<IWebElement> privateBadges = FindElements(By.CssSelector(SRPLocators.XSLocators.PrivateBadge.GetAttribute<DescriptionAttribute>().Description));
            return privateBadges == null;
        }

        public override bool IsEveryListingCPOAbs()
        {
            int correctCpoAdCount = 0;
            IList<IWebElement> listingTitles = FindElements(By.CssSelector(SRPLocators.XSLocators.OrganicListingTitle.GetAttribute<DescriptionAttribute>().Description));  //Excludes featured ad
            IList<IWebElement> vdpUrls = FindElements(By.CssSelector(SRPLocators.XSLocators.VdpUrl.GetAttribute<DescriptionAttribute>().Description));  //Excludes featured ad

            correctCpoAdCount += vdpUrls.Where(x => (x.Text.Length > 0) && x.GetAttribute("href").Contains("showcpo=CpoOnly")).Count();
            return CountElements(listingTitles) == correctCpoAdCount;
        }

        public override bool IsEveryListingNewAbs()
        {
            IList<IWebElement> priorityListings = FindElements(By.CssSelector(SRPLocators.XSLocators.PriorityListings.GetAttribute<DescriptionAttribute>().Description));
            IList<IWebElement> orgnaicListings = FindElements(By.CssSelector(SRPLocators.XSLocators.OrganicListings.GetAttribute<DescriptionAttribute>().Description));

            if (priorityListings == null || orgnaicListings == null)
            {
                throw new Exception("Could not find priorityListings or orgnaicListings");
            }

            IList<IWebElement> newCarLabelPriorityListings = language == Language.EN ? FindElements(By.CssSelector(SRPLocators.XSLocators.NewCarLabelPriorityListings.GetAttribute<DescriptionAttribute>().Description)) : FindElements(By.CssSelector(SRPLocators.XSLocators.NewCarLabelPriorityListings.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench));
            IList<IWebElement> newCarLabelOrganicListings = language == Language.EN ? FindElements(By.CssSelector(SRPLocators.XSLocators.NewCarLabelOrganicListings.GetAttribute<DescriptionAttribute>().Description)) : FindElements(By.CssSelector(SRPLocators.XSLocators.NewCarLabelOrganicListings.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench));

            if (newCarLabelPriorityListings == null || newCarLabelOrganicListings == null)
            {
                throw new Exception("Could not find new car badge on priorityListings or orgnaicListings");
            }

            return (priorityListings.Count + orgnaicListings.Count) == (newCarLabelOrganicListings.Count + newCarLabelPriorityListings.Count);
        }

        public override bool HasEveryListingPhotoAbs()
        {
            IList<IWebElement> listingTitles = FindElements(By.CssSelector(SRPLocators.XSLocators.OrganicListingTitle.GetAttribute<DescriptionAttribute>().Description)); // read-only  //Excludes featured ad
            IList<IWebElement> withPhoto = FindElements(By.CssSelector(SRPLocators.Listing.PhotoVisible.GetAttribute<DescriptionAttribute>().Description)); // read-only
            IList<IWebElement> withoutPhoto = FindElements(By.CssSelector(SRPLocators.Listing.PhotoNotVisible.GetAttribute<DescriptionAttribute>().Description));

            return withoutPhoto.Count <= 0 && listingTitles.Count == withPhoto.Count;
        }

        public override void ClickOnFirstOrganicListingAbs(HeaderFooterLocators.Categories category = HeaderFooterLocators.Categories.CarsTrucksSuv)
        {
            IWebElement element;
            try
            {
                element = FindElement(By.CssSelector(SRPLocators.XSLocators.OrganicListingLink.GetAttribute<DescriptionAttribute>().Description));
                ScrollTo(element);
            }
            catch (Exception)
            {
                RefreshPage();
                WaitForSRPPageLoad();
                element = FindElement(By.CssSelector(SRPLocators.XSLocators.OrganicListingLink.GetAttribute<DescriptionAttribute>().Description));
                ScrollTo(element);
            }
            ClickElement(element);
            WaitForPageLoad(90);
        }

        public override string GetOrganicListingTitleAbs(int sequenceNo, HeaderFooterLocators.Categories category = HeaderFooterLocators.Categories.CarsTrucksSuv)
        {
            IList<IWebElement> elements = FindElements(By.CssSelector(SRPLocators.XSLocators.OrganicListingTitle.GetAttribute<DescriptionAttribute>().Description));
            if (elements.Count < sequenceNo)
            {
                throw new Exception("Sequence Number should be less than total number of organic listing shown on SRP.");
            }
            else
            {
                ScrollTo(elements[sequenceNo]);
                return GetElementText(elements[sequenceNo]);
            }
        }

        public override ListingImage GetListingMainImage(IWebElement listItem)
        {

            var listTitle = listItem.FindElement(By.CssSelector("a"));
            var link = listTitle.GetAttribute("onclick");
            var mainPhoto = listItem.FindElement(By.CssSelector(SRPLocators.XSLocators.MainImage.GetAttribute<DescriptionAttribute>().Description));

            var listingImg = new ListingImage
            {
                URL = link,
                Width = Convert.ToInt32(mainPhoto.GetAttribute("width")),
                Height = Convert.ToInt32(mainPhoto.GetAttribute("height"))
            };
            return listingImg;
        }
        public override IWebElement GetFirstMatchedListing(ListingsType listingsType)
        {
            if (listingsType == ListingsType.TS || listingsType == ListingsType.XPL)
                {
                    var listings = FindElements(By.CssSelector(SRPLocators.XSLocators.PriorityListings.GetAttribute<DescriptionAttribute>().Description), 20, 1000);
                    if (listings == null || listings.Count == 0)
                    {
                        return null;
                    }
                    foreach (var list in listings)
                    {
                        IWebElement listTitle = list.FindElement(By.CssSelector("a"));
                        string link = listTitle.GetAttribute("onclick");
                        ScrollTo(listTitle);
                        switch (listingsType)
                        {
                            case ListingsType.TS:
                                if (link.Contains("ursrc=ts"))
                                {
                                    return list;
                                }
                                break;
                            case ListingsType.XPL:
                                if (link.Contains("ursrc=xpl"))
                                {
                                    return list;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            else
                {
                    var listings = FindElements(By.CssSelector(SRPLocators.XSLocators.OrganicAndTopAdListings.GetAttribute<DescriptionAttribute>().Description), 20, 1000);
                    foreach (var list in listings)
                    {
                        var listTitle = list.FindElement(By.CssSelector("a"));
                        var link = listTitle.GetAttribute("href");
                        switch (listingsType)
                        {
                            case ListingsType.HL:
                            case ListingsType.PL:
                                throw new NotFoundException("This testcase method should not be run for XS viewport");
                            case ListingsType.TA:
                                if (link.Contains("ursrc=ta"))
                                {
                                    return list;
                                }
                                break;
                            default:
                                return list;
                        }
                    }
                }
            return null;
        }

        public override ListingImage GetListingStripeImage(IWebElement element)
        {
            throw new NotFoundException("This testcase method should not be run for XS viewport");
        }
        public override String GetListingProximity(string adId)
        {
            var allListings = FindElements(By.CssSelector(SRPLocators.XSLocators.OrganicAndTopAdListings.GetAttribute<DescriptionAttribute>().Description), 20, 1000);
            var priorityListings = FindElements(By.CssSelector(SRPLocators.XSLocators.PriorityListings.GetAttribute<DescriptionAttribute>().Description), 20, 1000);
            var listings = allListings.Concat(priorityListings);
            foreach (var list in listings)
            {
                var listTitle = list.FindElement(By.CssSelector(SRPLocators.XSLocators.PVListingLink.GetAttribute<DescriptionAttribute>().Description));
                var listTitleLink = listTitle.GetAttribute("href");
                if (listTitleLink.Contains(adId))
                {
                    var proximity = list.FindElement(By.CssSelector(SRPLocators.XSLocators.ProximityText.GetAttribute<DescriptionAttribute>().Description)).Text.Trim().ToLower();
                    return proximity;
                }
            }
            return null;
        }
        public override IDictionary<ListingsType, List<double>> GetAdSortPoints(bool isPriorityListing)
        {

            var xplAndPlSortPoints = new List<double>();
            var orgSortPoints = new List<double>();
            IList<IWebElement> listings;
            if (isPriorityListing)
            {
                listings = FindElements(By.CssSelector(SRPLocators.XSLocators.PriorityListingsLinks.GetAttribute<DescriptionAttribute>().Description), 20, 1000);
                var listingHeaders = FindElements(By.CssSelector(SRPLocators.XSLocators.ListingsHeaders.GetAttribute<DescriptionAttribute>().Description));
                var priorityListingHeading = listingHeaders[0];
                ScrollTo(priorityListingHeading);
            }
            else
            {
                listings = FindElements(By.CssSelector(SRPLocators.XSLocators.AllListingsLinks.GetAttribute<DescriptionAttribute>().Description), 20, 1000);
                var listingHeaders = FindElements(By.CssSelector(SRPLocators.XSLocators.ListingsHeaders.GetAttribute<DescriptionAttribute>().Description));
                var allListingHeading = listingHeaders[1];
                ScrollTo(allListingHeading);
            }
            var sortPoints = new Dictionary<ListingsType, List<double>>();
            if (listings != null)
            {
                int n = 1;
                if (isPriorityListing)
                {
                    NUnit.Framework.TestContext.WriteLine("................Priority Listing................");
                    foreach (var listItem in listings)
                    {
                        var adSortPoint = listItem.GetAttribute("data-ad-sort-point");
                        var link = listItem.GetAttribute("href");
                        if (!link.Contains("ursrc=ts") && !link.Contains("ursrc=fl"))
                        {
                            if (adSortPoint == "")
                            {
                                adSortPoint = "0";
                            }
                            if (language.ToString() == "FR")
                            {
                                adSortPoint = adSortPoint.Replace(",", ".");
                            }
                            if (link.Contains("ursrc=xpl") || link.Contains("ursrc=pl"))
                            {
                                xplAndPlSortPoints.Add(Convert.ToDouble(adSortPoint));
                            }
                        }
                        NUnit.Framework.TestContext.WriteLine($" link {n} href: " + link + " sort point: " + adSortPoint);
                        n++;

                    }
                    sortPoints.Add(ListingsType.PL, xplAndPlSortPoints);
                }
                else
                {
                    NUnit.Framework.TestContext.WriteLine("................All Listing................");
                    foreach (var listItem in listings)
                    {
                        var adSortPoint = listItem.GetAttribute("data-ad-sort-point");
                        var link = listItem.GetAttribute("href");
                        if (adSortPoint != "")
                        {
                            if (language.ToString() == "FR")
                            {
                                adSortPoint = adSortPoint.Replace(",", ".");
                            }
                            orgSortPoints.Add(Convert.ToDouble(adSortPoint));
                        }
                        NUnit.Framework.TestContext.WriteLine($" link {n} href: " + link + " sort point " + adSortPoint);
                        n++;
                    }
                    sortPoints.Add(ListingsType.Org, orgSortPoints);
                }

            }
            return sortPoints;
        }

        public override void SubscribeSaveSearch(string emailAddress)
        {
            By saveSearchLocator = By.CssSelector(SRPLocators.XSLocators.SaveSearchBtn.GetAttribute<DescriptionAttribute>().Description);
            ClickElement(FindElement(saveSearchLocator));
            WaitForElementVisible(By.CssSelector(SRPLocators.XSLocators.SubscribeSaveSearchModal.GetAttribute<DescriptionAttribute>().Description));
            By saveSearchEmailLocator = By.CssSelector(SRPLocators.XSLocators.SaveSearchText.GetAttribute<DescriptionAttribute>().Description);
            EnterText(saveSearchEmailLocator, emailAddress);
            UnFocusElementJS(FindElement(saveSearchEmailLocator), 3);
            IWebElement subscribeButton = FindElement(By.CssSelector(SRPLocators.XSLocators.SubscribeBtn.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(subscribeButton);
            WaitForSavedSearchSuccessModalStatus();
        }
        public override bool GetSubscribeButtonStatus(bool isSubscribed)
        {
            if (isSubscribed)
            {
                try
                {
                    WaitUntil(() => FindElement(By.CssSelector(SRPLocators.XSLocators.SaveSearchToggle.GetAttribute<DescriptionAttribute>().Description)).GetAttribute("class").Contains("status-subscribed"));
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    WaitUntil(() => !FindElement(By.CssSelector(SRPLocators.XSLocators.SaveSearchToggle.GetAttribute<DescriptionAttribute>().Description)).GetAttribute("class").Contains("status-subscribed"));
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        public override void UnSubscribeSaveSearch()
        {
            ClickElement(FindElement(By.CssSelector(SRPLocators.XSLocators.SaveSearchBtn.GetAttribute<DescriptionAttribute>().Description)));
            WaitForElementNotVisible(By.CssSelector(SRPLocators.XSLocators.SubsribedSavedSearchIcon.GetAttribute<DescriptionAttribute>().Description), 20);
        }

        public override string GetHomeDeliveryInfoIconTooltipMessage()
        {
            var homeDeliveryToggleInfoIcon = FindElements(By.CssSelector(SRPLocators.XSLocators.HomeDeliveryToggleInfoIcon.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault();
            homeDeliveryToggleInfoIcon.Click();
            By homeDeliveryInfoTooltipLocator = By.CssSelector(SRPLocators.XSLocators.HomeDeliveryToggleInfoTooltip.GetAttribute<DescriptionAttribute>().Description);
            WaitForElementVisible(homeDeliveryInfoTooltipLocator);
            var tooltipText = FindElement(homeDeliveryInfoTooltipLocator).Text;
            return tooltipText;
        }
        public override bool IsAllListingWithoutTraditionDealer(Proximity proximity)
        {
            List<IWebElement> proximityDivs = FindElements(By.CssSelector(SRPLocators.XSLocators.AllOrganicProximity.GetAttribute<DescriptionAttribute>().Description)).ToList();
            bool allProximityValid = proximityDivs.All(div =>
            {
                string proximityText = div.Text.ToLower();
                return proximityText == proximity.virtualProximityText || proximityText.EndsWith(proximity.hybridProximityText);
            });
            return allProximityValid;
        }
        #region SEO
        public override void ClickShowMoreYearsLinkAbs()
        {
            ClickElement(FindElement(By.CssSelector(SRPLocators.SEOLinks.ShowMoreYearsXSLink.GetAttribute<DescriptionAttribute>().Description)));
        }
        public override string GetIntroDescSEOWidgetAbs()
        {
            string intro1= GetElementText(FindElement(By.CssSelector(SRPLocators.SEOLinks.IntroDesc.GetAttribute<DescriptionAttribute>().Description)));
            string intro = GetElementText(FindElement(By.CssSelector(SRPLocators.SEOLinks.IntroTitleXS.GetAttribute<DescriptionAttribute>().Description)));
            return intro + " " + intro1;
        }
        #endregion

        internal override bool VerifyPaymentCalculatorPillsOrderOnSRP()
        {
            // Find all elements matching the given CSS selector within a specified timeout
            IList<IWebElement> listingPills = FindElements(By.CssSelector(SRPLocators.XSLocators.ListingPills.GetAttribute<DescriptionAttribute>().Description), 20, 1000);
  
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

            return isFirstPayCalcPill;
        }

        #region PreQual
        internal override bool? VerifyPriceFilterForPreQual(PreQualLocators.PreQual resultType, string preQualMaxAmount = null)
        {
            WaitForPageLoad(10);
            WaitForElementVisible(By.CssSelector(SRPLocators.SmallLocators.Filter.GetAttribute<DescriptionAttribute>().Description));
            string url = Driver.Url;
            OpenFilters();
            WaitForElementVisible(By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<FacetRowLocatorAttribute>().FacetRowLocator));
            string priceFilterValue = GetElementText(FindElement(By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<FacetRowLocatorAttribute>().FacetRowLocator)));
            switch (resultType)
            {
                case PreQualLocators.PreQual.SuperPrime:
                case PreQualLocators.PreQual.Prime:
                case PreQualLocators.PreQual.ErrorInValidIDV:
                case PreQualLocators.PreQual.ErrorInvalidKBA:
                case PreQualLocators.PreQual.IDVMaxLimitReached:
                case PreQualLocators.PreQual.PreQualDecline:
                    {
                        return (url.EndsWith("cars") || url.EndsWith("autos"))
                            && (priceFilterValue.ToLower().Trim().Equals("price/payments new") || priceFilterValue.ToLower().Trim().Equals("prix/paiements nouveau"));
                    }
                case PreQualLocators.PreQual.NearPrime:
                case PreQualLocators.PreQual.SubPrime:
                case PreQualLocators.PreQual.DeepSubPrime:
                    {
                        return url.Contains("pRng=%2C" + preQualMaxAmount) && priceFilterValue.Replace(",", "").Replace(" ", "").Contains(preQualMaxAmount);
                    }
            }
            ClickViewResultsBtn();
            return false;
            
        }
        #endregion
    }
}