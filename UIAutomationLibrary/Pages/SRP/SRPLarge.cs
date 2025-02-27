using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MarketPlaceWeb.Pages.SRP
{
    public class SRPLarge : SRPAbstract
    {
        public SRPLarge(IWebDriver driver, Viewport viewport, Language language) : base(driver, viewport, language) { }

        private const string _defaultValue = null;

        #region dealerInventoryPage
        public override void OpenLeadTabDealerInventoryAbs()
        {
            ScrollTo(FindElement(By.CssSelector(SRPLocators.CommonLocators.DipLeadContainer.GetAttribute<DescriptionAttribute>().Description)));
        }
        #endregion

        public override int GetTotalNumberOfFoundAbs(SrpDesign srpDesign = SrpDesign.Old)
        {
            ScrollToTop();
            if (IsElementVisible(By.CssSelector(SRPLocators.CommonLocators.PageErrorWarning.GetAttribute<DescriptionAttribute>().Description)) || IsElementVisible(By.CssSelector(SRPLocators.CommonLocators.NoSearchResultWarning.GetAttribute<DescriptionAttribute>().Description))) //For multiple filters different No Search Result error message is displayed
            { return 0; }  //Counter zero result scenario
            IWebElement foundTotal = srpDesign == SrpDesign.New ? FindElement(By.CssSelector(SRPLocators.LargeLocators.FoundTotal.GetAttribute<DescriptionSRPAttribute>().DescriptionSRP)) : FindElement(By.CssSelector(SRPLocators.LargeLocators.FoundTotal.GetAttribute<DescriptionOldSRPAttribute>().DescriptionOldSRP));
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
                string cleanUnwantedDigits = GetSubstringAfterCharacter(facetOptionElement.Text, '(');  //Remove trailing characters incluing '('
                return Convert.ToInt32(GetSubstringBeforeCharacter(cleanUnwantedDigits, ')'));
            }

            return 0;
        }

        public override void SelectLocationFacetAbs(string location)
        {
            OpenFacetAbs(SRPLocators.Facets.LocationParent, FacetStatus.Open);
            FindElement(By.CssSelector(SRPLocators.CommonLocators.LocationField.GetAttribute<DescriptionAttribute>().Description)).SendKeys(location);
            ClickOnApplyOnFacet(SRPLocators.Facets.CityPostalCodeChild);
        }

        public override void SelectFirstOptionFromMakeModelTypeSubTypeAbs(SRPLocators.Facets facet)
        {
            if (facet != SRPLocators.Facets.MakeChild && facet != SRPLocators.Facets.ModelChild && facet != SRPLocators.Facets.Type && facet != SRPLocators.Facets.SubType)
            {
                throw new Exception("This method cannot work for the selected facet.");
            }
            else
            {
                OpenFacetAbs(facet, FacetStatus.Open);
                ClickElement(FindElements(By.CssSelector(facet.GetAttribute<FacetListAttribute>().FacetList)).FirstOrDefault(x => x.Text.Length > 0));
                WaitForPageLoad(90);
                WaitForSRPPageLoad();
            }
        }

        public override void SelectSearchRadiusFacetAbs(SRPLocators.SearchRadius searchRadius, SrpDesign srpDesign = SrpDesign.Old)
        {
            switch (srpDesign)
            {
                case SrpDesign.Old:
                    OpenFacetAbs(SRPLocators.Facets.Distance, FacetStatus.Open);
                    string value = (searchRadius != SRPLocators.SearchRadius.Provincial || searchRadius != SRPLocators.SearchRadius.National) ? searchRadius.ToString().Replace("Plus_", "") : (searchRadius == SRPLocators.SearchRadius.Provincial) ? "-2" : (searchRadius == SRPLocators.SearchRadius.National) ? "-1" : throw new Exception("This value is not available in the dropdown.");
                    SelectByValue(By.CssSelector(SRPLocators.Facets.Distance.GetAttribute<DropdownLocatorAttribute>().DropdownLocator), value);
                    ClickOnApplyOnFacet(SRPLocators.Facets.Distance);
                    break;
                case SrpDesign.New:
                    OpenFacetAbs(SRPLocators.Facets.SearchRadiusChild, FacetStatus.Open);
                    string valueNewSrp = (searchRadius != SRPLocators.SearchRadius.Provincial || searchRadius != SRPLocators.SearchRadius.National) ? searchRadius.ToString().Replace("Plus_", "") : (searchRadius == SRPLocators.SearchRadius.Provincial) ? "-2" : (searchRadius == SRPLocators.SearchRadius.National) ? "-1" : throw new Exception("This value is not available in the dropdown.");
                    CustomSelectByValue(FindElement(By.CssSelector(SRPLocators.Facets.SearchRadiusChild.GetAttribute<DropdownLocatorAttribute>().DropdownLocator)), FindElement(By.CssSelector(SRPLocators.Facets.SearchRadiusChild.GetAttribute<DropdownFacetRowLocatorAttribute>().DropdownFacetRowLocator)), valueNewSrp);
                    WaitForPageLoad(90);
                    WaitForSRPPageLoad();
                    break;
            }
        }

        public override void SelectMakeFacetAbs(string make)
        {
            OpenFacetAbs(SRPLocators.Facets.MakeChild, FacetStatus.Open);
            FindElement(By.CssSelector(SRPLocators.Facets.MakeChild.GetAttribute<SearchTextLocatorAttribute>().SearchTextLocator)).SendKeys(make);
            IList<IWebElement> listOfMake = FindElements(By.CssSelector(SRPLocators.Facets.MakeChild.GetAttribute<FacetListAttribute>().FacetList));
            ClickElement(listOfMake.FirstOrDefault(x => x.GetAttribute("data-dropdownvalue") == make));
            WaitForPageLoad(90);
            WaitForSRPPageLoad();
        }

        public override void SelectModelFacetAbs(string model)
        {
            OpenFacetAbs(SRPLocators.Facets.ModelChild, FacetStatus.Open);
            IList<IWebElement> listOfModel = FindElements(By.CssSelector(SRPLocators.Facets.ModelChild.GetAttribute<FacetListAttribute>().FacetList));
            ClickElement(listOfModel.FirstOrDefault(x => x.GetAttribute("data-dropdownvalue") == model));
            WaitForPageLoad(90);
            WaitForSRPPageLoad();
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
                        OpenChildFacetFlyout(facets, facetStatus);
                        break;
                }
            }
        }

        public override void CloseChildFacetFlyoutAbs(SRPLocators.Facets facet, SrpDesign srpDesign = SrpDesign.Old)
        {
            if (IsFacetOpen(facet, "open"))
            {
                IWebElement facetCloseBtn = FindElement(By.CssSelector(facet.GetAttribute<CloseButtonLocatorAttribute>().CloseButtonLocator));
                ClickElement(facetCloseBtn);
                WaitForFacetStatus(facet, FacetStatus.Close, "open");
            }
        }

        public override string GetTitleTextAbs()
        {
            IWebElement element = FindElement(By.CssSelector(SRPLocators.LargeLocators.Title.GetAttribute<DescriptionAttribute>().Description));
            ScrollTo(element);
            return GetElementText(element);
        }

        public override void ClearFacetAbs(SRPLocators.Facets facet, SrpDesign srpDesign = SrpDesign.Old)
        {
            if (srpDesign == SrpDesign.Old)
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
                        OpenFacetAbs(facet, FacetStatus.Open);
                        IWebElement clearButton = FindElement(By.CssSelector(facet.GetAttribute<ClearButtonLocatorAttribute>().ClearButtonLocator));
                        ScrollTo(clearButton);
                        ClickElement(clearButton);
                        WaitForPageLoad(90);
                        WaitForSRPPageLoad();
                        break;
                    case SRPLocators.Facets.ConditionParent:
                    case SRPLocators.Facets.SellerTypeParent:
                        ClearAllFacetsAbs();
                        break;
                    case SRPLocators.Facets.LocationParent:
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
                        OpenFacetAbs(facet, FacetStatus.Open);
                        IWebElement clearButton = FindElement(By.CssSelector(facet.GetAttribute<ClearButtonLocatorAttribute>().ClearButtonLocator));
                        ScrollTo(clearButton);
                        ClickElement(clearButton);
                        WaitForPageLoad(90);
                        WaitForSRPPageLoad();
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
                    case SRPLocators.Facets.LocationParent:
                    case SRPLocators.Facets.VehicleParent:
                    case SRPLocators.Facets.MechanicalParent:
                    case SRPLocators.Facets.InteriorParent:
                    case SRPLocators.Facets.OtherOptionsParent:
                        throw new Exception("Not a valid facet for clear facet");
                }
            }
        }

        public override string GetSelectedValueOfFacetAbs(SRPLocators.Facets facet, HeaderFooterLocators.Categories category = HeaderFooterLocators.Categories.CarsTrucksSuv, SrpDesign srpDesign = SrpDesign.Old)
        {
            if (category == HeaderFooterLocators.Categories.Bikes)
            {
                return GetValueByJS("return document.querySelector('" + SRPLocators.LargeLocators.FacetValueLocatorBikes.GetAttribute<DescriptionAttribute>().Description + "').value");
            }
            else
            {
                IWebElement element = FindElement(By.CssSelector(facet.GetAttribute<FacetValueLocatorAttribute>().FacetValueLocator));
                if (element.Displayed)
                {
                    return element.Text.Trim();
                }
                else { return string.Empty; }
            }
        }

        public override string GetFacetElementMinText(SRPLocators.Facets facet)
        {
            IWebElement facetElementText = FindElement(By.CssSelector(facet.GetAttribute<MinValueFieldAttribute>().MinValueField));
            ScrollTo(facetElementText);
            return GetValueByJS("return document.querySelector('" + facet.GetAttribute<MinValueFieldAttribute>().MinValueField + "').value");
        }

        public override string GetFacetElementMaxText(SRPLocators.Facets facet)
        {
            IWebElement facetElementText = FindElement(By.CssSelector(facet.GetAttribute<MaxValueFieldAttribute>().MaxValueField));
            ScrollTo(facetElementText);
            return GetValueByJS("return document.querySelector('" + facet.GetAttribute<MaxValueFieldAttribute>().MaxValueField + "').value");
        }

        public override string CustomGetSelectedText(SRPLocators.Facets facet)
        {
            IWebElement dropdownRowElement = FindElement(By.CssSelector(facet.GetAttribute<DropdownFacetRowLocatorAttribute>().DropdownFacetRowLocator));
            ScrollTo(dropdownRowElement);
            return GetElementText(dropdownRowElement.FindElement(By.CssSelector("button")));
        }

        public override string GetYearMinSelectedText(SrpDesign srpDesign = SrpDesign.Old)
        {
            if (srpDesign == SrpDesign.Old)
            {
                OpenFacetAbs(SRPLocators.Facets.YearChild, FacetStatus.Open);
                IWebElement dropdownRowElement = FindElement(By.CssSelector(SRPLocators.Facets.YearChild.GetAttribute<MinDropdownOldSrpLocatorAttribute>().MinDropdownOldSrpLocator));
                ScrollTo(dropdownRowElement);
                string text = GetSelectedText(By.CssSelector(SRPLocators.Facets.YearChild.GetAttribute<MinDropdownOldSrpLocatorAttribute>().MinDropdownOldSrpLocator));
                CloseFacetAbs(SRPLocators.Facets.YearChild);
                return text;
            }
            else
            {
                IWebElement dropdownRowElement = FindElement(By.CssSelector(SRPLocators.Facets.YearChild.GetAttribute<MinDropdownFacetRowLocatorAttribute>().MinDropdownFacetRowLocator));
                ScrollTo(dropdownRowElement);
                return GetElementText(dropdownRowElement.FindElement(By.CssSelector("button")));
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
                CloseFacetAbs(SRPLocators.Facets.YearChild);
                return text;
            }
            else
            {
                IWebElement dropdownRowElement = FindElement(By.CssSelector(SRPLocators.Facets.YearChild.GetAttribute<MaxDropdownFacetRowLocatorAttribute>().MaxDropdownFacetRowLocator));
                ScrollTo(dropdownRowElement);
                return GetElementText(dropdownRowElement.FindElement(By.CssSelector("button")));
            }
        }

        public override void CloseFacetAbs(SRPLocators.Facets facets, SrpDesign srpDesign = SrpDesign.Old)
        {
            OpenFacetAbs(facets, SRPAbstract.FacetStatus.Close);
        }

        public override void SelectBuyingOptionsSingleAbs(SRPLocators.BuyingOptions buyingOptions, bool toBeChecked, SrpDesign srpDesign = SrpDesign.Old)
        {
            OpenFacetAbs(SRPLocators.Facets.AtHomeServices, FacetStatus.Open);
            if (IsBuyingOptionsCheckedAbs(buyingOptions) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(buyingOptions.GetAttribute<CSPLabelLocatorAttribute>().CSPLabelLocator)));
                ClickOnApplyOnFacet(SRPLocators.Facets.AtHomeServices);
            }
        }

        public override bool IsBuyingOptionsCheckedAbs(SRPLocators.BuyingOptions buyingOptions, SrpDesign srpDesign = SrpDesign.Old)
        {
            switch (srpDesign)
            {
                case SrpDesign.Old:
                    if (!IsFacetOpen(SRPLocators.Facets.AtHomeServices, "open")) { OpenFacetAbs(SRPLocators.Facets.AtHomeServices, FacetStatus.Open); }
                    break;
                case SrpDesign.New:
                    if (!IsFacetOpen(SRPLocators.Facets.AtHomeServices, "show-list")) { OpenFacetAbs(SRPLocators.Facets.AtHomeServices, FacetStatus.Open); }
                    break;
            }
            return FindElements(By.CssSelector(buyingOptions.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault().Selected;
        }

        public override void WaitForSRPPageLoad(int timeOut = 60)
        {
            WaitUntil(() => IsElementVisible(By.CssSelector(SRPLocators.LargeLocators.SearchExpansionWarning.GetAttribute<DescriptionAttribute>().Description)) || IsElementVisible(By.CssSelector(SRPLocators.LargeLocators.ZeroListingWarning.GetAttribute<DescriptionAttribute>().Description)) || (driver.FindElements(By.CssSelector(SRPLocators.LargeLocators.FirstListing.GetAttribute<DescriptionAttribute>().Description)).Count > 0 && driver.FindElements(By.CssSelector(SRPLocators.LargeLocators.FirstListing.GetAttribute<DescriptionAttribute>().Description)).First().Displayed ), timeOut);
        }

        public override void SelectYearFacetAbs(SRPLocators.Year year, string minYear, string maxYear, SrpDesign srpDesign = SrpDesign.Old)
        {
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
                ClickOnApplyOnFacet(SRPLocators.Facets.YearChild);
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
                            WaitForPageLoad(90);
                            WaitForSRPPageLoad();
                            CustomSelectByValue(FindElement(By.CssSelector(SRPLocators.Facets.YearChild.GetAttribute<MaxDropdownLocatorAttribute>().MaxDropdownLocator)), FindElement(By.CssSelector(SRPLocators.Facets.YearChild.GetAttribute<MaxDropdownFacetRowLocatorAttribute>().MaxDropdownFacetRowLocator)), maxYear);
                        }
                        break;
                }

                WaitForPageLoad(90);
                WaitForSRPPageLoad();
            }
        }

        public override void SelectPaymentsFacetAbs(string minPayment, string maxPayment, SRPLocators.PaymentFrequency paymentFrequency, SRPLocators.Term term, string downPayment, string tradeInValue, SrpDesign srpDesign = SrpDesign.Old)
        {
            switch (srpDesign)
            {
                case SrpDesign.Old:
                    OpenFacetAbs(SRPLocators.Facets.PricePaymentsParent, FacetStatus.Open);
                    ClickElement(FindElement(By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<PaymentsTabOldSRPAttribute>().PaymentsTabOldSRP)));
                    ClearFacetAbs(SRPLocators.Facets.PricePaymentsParent);

                    By minPaymentLocator = By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<MinPaymentFieldAttribute>().MinPaymentField);
                    EnterText(minPaymentLocator, minPayment);
                    By maxPaymentLocator = By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<MaxPaymentFieldAttribute>().MaxPaymentField);
                    EnterText(maxPaymentLocator, maxPayment);

                    By paymentFrequencyDropdownLocator = By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<PaymentFrequencyDropdownAttribute>().PaymentFrequencyDropdown);
                    ScrollTo(FindElement(paymentFrequencyDropdownLocator));
                    SelectByValue(paymentFrequencyDropdownLocator, paymentFrequency.GetAttribute<DescriptionOldSRPAttribute>().DescriptionOldSRP);

                    By termsDropdownLocator = By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<TermDropdownAttribute>().TermDropdown);
                    ScrollTo(FindElement(termsDropdownLocator));
                    SelectByValue(termsDropdownLocator, term.GetAttribute<DescriptionAttribute>().Description);

                    By downPaymentLocator = By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<DownPaymentFieldAttribute>().DownPaymentField);
                    EnterText(downPaymentLocator, downPayment);

                    By tradeInValueLocator = By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<TradeInValueFieldAttribute>().TradeInValueField);
                    EnterText(tradeInValueLocator, tradeInValue);

                    ClickOnApplyOnFacet(SRPLocators.Facets.PricePaymentsParent);
                    break;
                case SrpDesign.New:
                    ClearAllFacetsAbs();  //Finance max payment field does not clear with element.clear()
                    OpenFacetAbs(SRPLocators.Facets.PricePaymentsParent, FacetStatus.Open);
                    ClickElement(FindElement(By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<PaymentsTabNewSRPAttribute>().PaymentsTabNewSRP)));
                    var tabRowLocator = FindElement(By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<PaymentsTabRowLocatorAttribute>().PaymentsTabRowLocator));
                    WaitUntil(() => tabRowLocator.GetAttribute("class").Contains("active"), 5);

                    By minPaymentLocatorNewSrp = By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<MinPaymentFieldAttribute>().MinPaymentField);
                    EnterText(minPaymentLocatorNewSrp, minPayment);
                    By maxPaymentLocatorNewSrp = By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<MaxPaymentFieldAttribute>().MaxPaymentField);
                    EnterText(maxPaymentLocatorNewSrp, maxPayment);

                    IWebElement paymentFrequencyElement = FindElement(By.CssSelector(paymentFrequency.GetAttribute<DescriptionAttribute>().Description));
                    ScrollTo(paymentFrequencyElement);
                    if (!IsCheckboxChecked(paymentFrequencyElement))
                    {
                        ClickElement(FindElement(By.CssSelector(paymentFrequency.GetAttribute<DescriptionAttribute>().Description + " + span.design")));
                    }

                    By termsDropdownLocatorNewSrp = By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<TermDropdownAttribute>().TermDropdown);
                    ScrollTo(FindElement(termsDropdownLocatorNewSrp));
                    SelectByValue(termsDropdownLocatorNewSrp, term.GetAttribute<DescriptionAttribute>().Description);

                    By downPaymentLocatorNewSrp = By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<DownPaymentFieldAttribute>().DownPaymentField);
                    EnterText(downPaymentLocatorNewSrp, downPayment);

                    By tradeInValueLocatorNewSrp = By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<TradeInValueFieldAttribute>().TradeInValueField);
                    EnterText(tradeInValueLocatorNewSrp, tradeInValue);

                    ClickOnApplyOnFacet(SRPLocators.Facets.PricePaymentsParent);
                    break;
            }
        }

        public override void SelectCheckboxMultipleAbs(SRPLocators.Facets facet, List<string> listOfOption, bool toBeChecked)
        {
            OpenFacetAbs(facet, FacetStatus.Open);
            IList<IWebElement> listOfElements = FindElements(By.CssSelector(facet.GetAttribute<FacetListAttribute>().FacetList));
            foreach (string option in listOfOption)
            {
                IWebElement element = listOfElements.FirstOrDefault(x => x.GetAttribute("data-value") == option);
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
            ClickOnApplyOnFacet(facet);
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
            OpenFacetAbs(facet, FacetStatus.Close);
            return status;
        }

        public override void SelectEngineFacetSingleAbs(SRPLocators.Engine engine, bool toBeChecked)
        {
            OpenFacetAbs(SRPLocators.Facets.EngineChild, FacetStatus.Open);
            string locator = (language.ToString() == "EN") ? engine.GetAttribute<DescriptionAttribute>().Description : engine.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench;
            IWebElement element = FindElements(By.CssSelector(locator)).FirstOrDefault();  //Element considered not Displayed
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(locator + " + label")));
            }
            ClickOnApplyOnFacet(SRPLocators.Facets.EngineChild);
        }

        public override void SelectTransmissionFacetSingleAbs(SRPLocators.Transmission transmission, bool toBeChecked)
        {
            OpenFacetAbs(SRPLocators.Facets.TransmissionChild, FacetStatus.Open);
            string locator = (language.ToString() == "EN") ? transmission.GetAttribute<DescriptionAttribute>().Description : transmission.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench;
            IWebElement element = FindElements(By.CssSelector(locator)).FirstOrDefault();
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(locator + " + label")));
            }
            ClickOnApplyOnFacet(SRPLocators.Facets.TransmissionChild);
        }

        public override void EnterMinMaxFacetAbs(SRPLocators.Facets facet, string minValue, string maxValue, SrpDesign srpDesign = SrpDesign.Old)
        {
            switch (srpDesign)
            {
                case SrpDesign.Old:
                    OpenFacetAbs(facet, FacetStatus.Open);
                    if (facet == SRPLocators.Facets.PricePaymentsParent)
                    {
                        ClickElement(FindElement(By.CssSelector(SRPLocators.Facets.PricePaymentsParent.GetAttribute<PriceTabOldSRPAttribute>().PriceTabOldSRP)));
                    }
                    if (!string.IsNullOrEmpty(minValue))
                    {
                        By locator = By.CssSelector(facet.GetAttribute<MinValueFieldAttribute>().MinValueField);
                        EnterText(locator, minValue, 20);
                    }
                    if (!string.IsNullOrEmpty(maxValue))
                    {
                        By locator = By.CssSelector(facet.GetAttribute<MaxValueFieldAttribute>().MaxValueField);
                        EnterText(locator, maxValue, 20);
                    }
                    ClickOnApplyOnFacet(facet);
                    break;
                case SrpDesign.New:
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
                        ClickElement(FindElement(By.CssSelector(facet.GetAttribute<MinValueFieldAttribute>().MinValueField)));
                        EnterText(locator, minValue, 20);
                        UnFocusElementJS(FindElement(locator));
                    }
                    if (!string.IsNullOrEmpty(maxValue))
                    {
                        By locator = By.CssSelector(facet.GetAttribute<MaxValueFieldAttribute>().MaxValueField);
                        ClickElement(FindElement(By.CssSelector(facet.GetAttribute<MaxValueFieldAttribute>().MaxValueField)));
                        EnterText(locator, maxValue, 20);
                        UnFocusElementJS(FindElement(locator));
                    }
                    ClickOnApplyOnFacet(facet);
                    break;
            }
        }

        public override void SelectConditionFacetSingleAbs(SRPLocators.Condition condition, bool toBeChecked, SrpDesign srpDesign = SrpDesign.Old)
        {
            OpenFacetAbs(SRPLocators.Facets.ConditionParent, FacetStatus.Open);
            IWebElement element = FindElements(By.CssSelector(condition.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault();  //Element considered not displayed
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(condition.GetAttribute<ConditionLabelAttribute>().ConditionLabel)));
            }
            WaitForPageLoad(90);
            WaitForSRPPageLoad();
        }

        public override void SelectSellerTypeFacetAbs(SRPLocators.SellerType sellerType, bool toBeChecked, SrpDesign srpDesign = SrpDesign.Old)
        {
            OpenFacetAbs(SRPLocators.Facets.SellerTypeParent, FacetStatus.Open);
            IWebElement element = FindElements(By.CssSelector(sellerType.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault();  //This element is not detected as displayed. Changed to FindElements
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(sellerType.GetAttribute<SellerTypeLabelAttribute>().SellerTypeLabel)));
            }
            ClickOnApplyOnFacet(SRPLocators.Facets.SellerTypeParent);
        }

        public override void ClearAllFacetsAbs()
        {
            ClickElement(FindElement(By.CssSelector(SRPLocators.CommonLocators.ClearAll.GetAttribute<DescriptionAttribute>().Description)));
            WaitForPageLoad(90);
            WaitForSRPPageLoad();
        }

        public override void SelectTrimFacetSingleAbs(string trim, bool toBeChecked)
        {
            OpenFacetAbs(SRPLocators.Facets.TrimChild, FacetStatus.Open);
            IList<IWebElement> listOfElements = FindElements(By.CssSelector(SRPLocators.Facets.TrimChild.GetAttribute<FacetListAttribute>().FacetList));
            IWebElement element = listOfElements.FirstOrDefault(x => x.GetAttribute("data-value") == trim);
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
            ClickOnApplyOnFacet(SRPLocators.Facets.TrimChild);
        }

        public override void SelectBodyTypeFacetSingleAbs(SRPLocators.BodyType bodyType, bool toBeChecked)
        {
            OpenFacetAbs(SRPLocators.Facets.BodyTypeChild, FacetStatus.Open);
            string locator = (language.ToString() == "EN") ? bodyType.GetAttribute<DescriptionAttribute>().Description : bodyType.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench;
            IWebElement element = FindElements(By.CssSelector(locator)).FirstOrDefault();
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(locator + " + label")));
            }
            ClickOnApplyOnFacet(SRPLocators.Facets.BodyTypeChild);
        }

        public override void SelectExteriorColourFacetSingleAbs(SRPLocators.ExteriorColour exteriorColour, bool toBeChecked)
        {
            OpenFacetAbs(SRPLocators.Facets.ColourChild, FacetStatus.Open);
            string locator = (language.ToString() == "EN") ? exteriorColour.GetAttribute<DescriptionAttribute>().Description : exteriorColour.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench;
            IWebElement element = FindElements(By.CssSelector(locator)).FirstOrDefault();
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(locator + " + label")));
            }
            ClickOnApplyOnFacet(SRPLocators.Facets.ColourChild);
        }

        public override void SelectDrivetrainFacetSingleAbs(SRPLocators.Drivetrain drivetrain, bool toBeChecked)
        {
            OpenFacetAbs(SRPLocators.Facets.DrivetrainChild, FacetStatus.Open);
            string locator = (language.ToString() == "EN") ? drivetrain.GetAttribute<DescriptionAttribute>().Description : drivetrain.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench;
            IWebElement element = FindElements(By.CssSelector(locator)).FirstOrDefault();
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(locator + " + label")));
            }
            ClickOnApplyOnFacet(SRPLocators.Facets.DrivetrainChild);
        }

        public override void SelectFuelTypeFacetSingleAbs(SRPLocators.FuelType fuelType, bool toBeChecked)
        {
            OpenFacetAbs(SRPLocators.Facets.FuelTypeChild, FacetStatus.Open);
            string locator = (language.ToString() == "EN") ? fuelType.GetAttribute<DescriptionAttribute>().Description : fuelType.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench;
            IWebElement element = FindElements(By.CssSelector(locator)).FirstOrDefault();
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(locator + " + label")));
            }
            ClickOnApplyOnFacet(SRPLocators.Facets.FuelTypeChild);
        }

        public override void SelectSeatingCapacityFacetSingleAbs(SRPLocators.SeatingCapacity seatingCapacity, bool toBeChecked)
        {
            OpenFacetAbs(SRPLocators.Facets.SeatingCapacityChild, FacetStatus.Open);
            string locator = (language.ToString() == "EN") ? seatingCapacity.GetAttribute<DescriptionAttribute>().Description : seatingCapacity.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench;
            IWebElement element = FindElements(By.CssSelector(locator)).FirstOrDefault();
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(locator + " + label")));
            }
            ClickOnApplyOnFacet(SRPLocators.Facets.SeatingCapacityChild);
        }

        public override void SelectDoorsFacetSingleAbs(SRPLocators.Doors doors, bool toBeChecked)
        {
            OpenFacetAbs(SRPLocators.Facets.DoorsChild, FacetStatus.Open);
            string locator = (language.ToString() == "EN") ? doors.GetAttribute<DescriptionAttribute>().Description : doors.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench;
            IWebElement element = FindElements(By.CssSelector(locator)).FirstOrDefault();
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(locator + " + label")));
            }
            ClickOnApplyOnFacet(SRPLocators.Facets.DoorsChild);
        }

        public override void SelectSleepsFacetSingleAbs(SRPLocators.Sleeps sleeps, bool toBeChecked)
        {
            OpenFacetAbs(SRPLocators.Facets.Sleeps, FacetStatus.Open);
            IWebElement element = FindElements(By.CssSelector(sleeps.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault();
            if (IsCheckboxChecked(element) != toBeChecked)
            {
                ClickElement(FindElement(By.CssSelector(sleeps.GetAttribute<DescriptionAttribute>().Description + " + label")));
            }
            ClickOnApplyOnFacet(SRPLocators.Facets.Sleeps);
        }

        public override void SelectSubTypeFacetSingleAbs(SRPLocators.SubType subType, bool toBeChecked)
        {
            OpenFacetAbs(SRPLocators.Facets.SubType, FacetStatus.Open);
            string locator = (language.ToString() == "EN") ? subType.GetAttribute<DescriptionAttribute>().Description : subType.GetAttribute<DescriptionFrenchAttribute>().DescriptionFrench;
            IWebElement element = FindElement(By.CssSelector(locator));
            ScrollTo(element);
            ClickElement(element);
            WaitForPageLoad(90);
            WaitForSRPPageLoad();
        }

        public override void SelectFirstCheckboxFromFacetAbs(SRPLocators.Facets facet)
        {
            if (facet != SRPLocators.Facets.ColourChild && facet != SRPLocators.Facets.TransmissionChild && facet != SRPLocators.Facets.SeatingCapacityChild && facet != SRPLocators.Facets.DoorsChild && facet != SRPLocators.Facets.FuelTypeChild && facet != SRPLocators.Facets.SlideOuts && facet != SRPLocators.Facets.Sleeps && facet != SRPLocators.Facets.BodyTypeChild)
            {
                throw new Exception("This method cannot work for the selected facet.");
            }
            else
            {
                OpenFacetAbs(facet, FacetStatus.Open);
                ClickElement(FindElements(By.CssSelector(facet.GetAttribute<FacetLabelAttribute>().FacetLabel)).FirstOrDefault(x => x.Text.Length > 0));  //Make sure of intended first checkbox
                ClickOnApplyOnFacet(facet);
            }
        }

        public override void SelectOtherOptionsSingleAbs(SRPLocators.OtherOptions otherOptions, bool toBeChecked, SrpDesign srpDesign = SrpDesign.Old)
        {
            OpenFacetAbs(SRPLocators.Facets.OtherOptionsParent, FacetStatus.Open);
            IList<IWebElement> otherOptionsList = FindElements(By.CssSelector(SRPLocators.Facets.OtherOptionsParent.GetAttribute<FacetListAttribute>().FacetList));
            foreach (var otherOption in otherOptionsList)
            {
                if ("#" + otherOption.GetAttribute("id") == otherOptions.GetAttribute<DescriptionAttribute>().Description)
                {
                    if (IsCheckboxChecked(otherOption) != toBeChecked)
                    {
                        ClickElement(FindElement(By.CssSelector(otherOptions.GetAttribute<DescriptionAttribute>().Description + " + label")));
                    }
                }
                else
                {
                    if (IsCheckboxChecked(otherOption) != false)
                    {
                        ClickElement(otherOption);
                    }
                }
            }
            WaitForPageLoad(90);
            WaitForSRPPageLoad();
        }

        public override bool IsOtherOptionCheckedAbs(SRPLocators.OtherOptions otherOptions)
        {
            return FindElements(By.CssSelector(otherOptions.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault().Selected;
        }

        public override bool ContainsInListingTitleAbs(string value, string toCompare = _defaultValue, string value2 = _defaultValue)
        {
            bool result = false;
            IList<IWebElement> listingTitles = FindElements(By.CssSelector(SRPLocators.Listing.Title.GetAttribute<DescriptionAttribute>().Description));

            if (string.IsNullOrEmpty(toCompare))
            {
                foreach (var item in listingTitles)
                {
                    if (item.Text.Trim().Contains(value))
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
                        numbers.Add(Convert.ToInt32(text.Trim().Substring(0, 4)));
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
            IList<IWebElement> listingPrices = FindElements(By.CssSelector(SRPLocators.Listing.Price.GetAttribute<DescriptionAttribute>().Description));

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
                    catch (FormatException) { continue; }  //Skip non-integer items
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
            IList<IWebElement> proximityLabels = FindElements(By.CssSelector(SRPLocators.Listing.Mileage.GetAttribute<DescriptionAttribute>().Description));

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

            IList<IWebElement> proximityLabels = FindElements(By.CssSelector(SRPLocators.Listing.Proximity.GetAttribute<DescriptionAttribute>().Description));
            IList<int> numbers = new List<int>();
            foreach (var item in proximityLabels)
            {
                var text = item.Text;
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
            IList<IWebElement> listingTitles = FindElements(By.CssSelector(SRPLocators.Listing.Title.GetAttribute<DescriptionAttribute>().Description));
            IList<IWebElement> privateBadges = FindElements(By.CssSelector(SRPLocators.Listing.PrivateBadge.GetAttribute<DescriptionAttribute>().Description));
           
            return CountElements(listingTitles) == CountVisibleElements(privateBadges);
        }

        public override bool IsEveryListingDealerAbs()
        {
            IList<IWebElement> privateBadges = FindElements(By.CssSelector(SRPLocators.Listing.PrivateBadge.GetAttribute<DescriptionAttribute>().Description));
            return privateBadges == null;
        }

        public override bool IsEveryListingCPOAbs()
        {
            IList<IWebElement> listingTitles = FindElements(By.CssSelector(SRPLocators.Listing.Title.GetAttribute<DescriptionAttribute>().Description));
            IList<IWebElement> cpoBadges = FindElements(By.CssSelector(SRPLocators.Listing.CpoBadge.GetAttribute<DescriptionAttribute>().Description));

            return CountElements(listingTitles) == CountVisibleElements(cpoBadges);
        }

        public override bool IsEveryListingNewAbs()
        {
            IList<IWebElement> listingTitles = FindElements(By.CssSelector(SRPLocators.Listing.Title.GetAttribute<DescriptionAttribute>().Description)); // read-only
            IList<IWebElement> newBadges = FindElements(By.CssSelector(SRPLocators.Listing.NewBadge.GetAttribute<DescriptionAttribute>().Description));

            return CountElements(listingTitles) == CountVisibleElements(newBadges);
        }

        public override bool HasEveryListingPhotoAbs()
        {
            IList<IWebElement> listingTitles = FindElements(By.CssSelector(SRPLocators.Listing.Title.GetAttribute<DescriptionAttribute>().Description)); // read-only
            IList<IWebElement> withPhoto = FindElements(By.CssSelector(SRPLocators.Listing.PhotoVisible.GetAttribute<DescriptionAttribute>().Description)); // read-only
            IList<IWebElement> withoutPhoto = FindElements(By.CssSelector(SRPLocators.Listing.PhotoNotVisible.GetAttribute<DescriptionAttribute>().Description));

            return withoutPhoto.Count <= 0 && listingTitles.Count == withPhoto.Count;
        }

        public override void ClickOnFirstOrganicListingAbs(HeaderFooterLocators.Categories category = HeaderFooterLocators.Categories.CarsTrucksSuv)
        {
            try
            {
                IWebElement element = (category == HeaderFooterLocators.Categories.CarsTrucksSuv) ? FindElements(By.CssSelector(SRPLocators.LargeLocators.OrganicListingLink.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault(x => x.Text.Length > 0) : FindElements(By.CssSelector(SRPLocators.LargeLocators.OrganicListingLink.GetAttribute<DescriptionOldSRPAttribute>().DescriptionOldSRP)).FirstOrDefault(x => x.Text.Length > 0);
                ScrollTo(element);
                ClickElement(element);
            }
            catch (Exception)
            {
                RefreshPage();
                WaitForSRPPageLoad();
                IWebElement element = (category == HeaderFooterLocators.Categories.CarsTrucksSuv) ? FindElements(By.CssSelector(SRPLocators.LargeLocators.OrganicListingLink.GetAttribute<DescriptionAttribute>().Description)).FirstOrDefault(x => x.Text.Length > 0) : FindElements(By.CssSelector(SRPLocators.LargeLocators.OrganicListingLink.GetAttribute<DescriptionOldSRPAttribute>().DescriptionOldSRP)).FirstOrDefault(x => x.Text.Length > 0);
                ScrollTo(element);
                ClickElement(element);
            }
            WaitForPageLoad(90);
        }

        public override string GetOrganicListingTitleAbs(int sequenceNo, HeaderFooterLocators.Categories category = HeaderFooterLocators.Categories.CarsTrucksSuv)
        {
            IList<IWebElement> elements = (category == HeaderFooterLocators.Categories.CarsTrucksSuv) ? FindElements(By.CssSelector(SRPLocators.LargeLocators.OrganicListingLink.GetAttribute<DescriptionAttribute>().Description)) : FindElements(By.CssSelector(SRPLocators.LargeLocators.OrganicListingLink.GetAttribute<DescriptionOldSRPAttribute>().DescriptionOldSRP));       
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
            var listTitle = listItem.FindElement(By.CssSelector(SRPLocators.CommonLocators.ListingTitle.GetAttribute<DescriptionAttribute>().Description));
            var link = listTitle.GetAttribute("href");
            var mainPhoto = listItem.FindElement(By.CssSelector(SRPLocators.CommonLocators.MainImage.GetAttribute<DescriptionAttribute>().Description));

            var mainPhotoSize = mainPhoto.Size;
            var listingImg = new ListingImage
            {
                URL = link,
                Width = mainPhotoSize.Width,
                Height = mainPhotoSize.Height
            };
            return listingImg;
        }
        public override ListingImage GetListingStripeImage(IWebElement listItem)
        {
            var listTitle = listItem.FindElement(By.CssSelector(SRPLocators.CommonLocators.ListingTitle.GetAttribute<DescriptionAttribute>().Description));
            var link = listTitle.GetAttribute("href");

            if (listItem.FindElements(By.CssSelector(SRPLocators.CommonLocators.StripeImage.GetAttribute<DescriptionAttribute>().Description)).Count() == 0)
            {
                return null;
            }            
            var stripePhoto = listItem.FindElement(By.CssSelector(SRPLocators.CommonLocators.StripeImage.GetAttribute<DescriptionAttribute>().Description));
            var stripePhotoSize = stripePhoto.Size;
            var listingImg = new ListingImage
            {
                URL = link,
                Width = stripePhotoSize.Width,
                Height = stripePhotoSize.Height
            };
            return listingImg;


        }
        public override IWebElement GetFirstMatchedListing(ListingsType listingsType)
        {
            if (listingsType == ListingsType.TS || listingsType == ListingsType.XPL || listingsType == ListingsType.PL || listingsType == ListingsType.PPL)
            {
                var listings = FindElements(By.CssSelector(SRPLocators.CommonLocators.PriorityListings.GetAttribute<DescriptionAttribute>().Description), 20, 1000);
                if (listings == null || listings.Count == 0)
                {
                    return null;
                }
                foreach (var list in listings)
                {
                    var listTitle = list.FindElement(By.CssSelector(SRPLocators.CommonLocators.ListingTitle.GetAttribute<DescriptionAttribute>().Description));
                    var link = listTitle.GetAttribute("onclick");
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
                        case ListingsType.PL:
                            if (link.Contains("ursrc=pl"))
                            {
                                return list;
                            }
                            break;
                        case ListingsType.PPL:
                            if (link.Contains("ursrc=ppl"))
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
                var listings = FindElements(By.CssSelector(SRPLocators.CommonLocators.AllOrganicListings.GetAttribute<DescriptionAttribute>().Description), 20, 1000);
                foreach (var list in listings)
                {
                    var listTitle = list.FindElement(By.CssSelector(SRPLocators.CommonLocators.ListingTitle.GetAttribute<DescriptionAttribute>().Description));
                    var link = listTitle.GetAttribute("onclick");

                    switch (listingsType)
                    {
                        case ListingsType.HL:
                            if (link.Contains("ursrc=hl"))
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
        public override string GetListingProximity(string adId)
        {
            var allListings = FindElements(By.CssSelector(SRPLocators.CommonLocators.AllOrganicListings.GetAttribute<DescriptionAttribute>().Description), 20, 1000);
            var priorityListings = FindElements(By.CssSelector(SRPLocators.CommonLocators.PriorityListings.GetAttribute<DescriptionAttribute>().Description), 20, 1000);
            var listings = allListings.Concat(priorityListings);
            foreach (var list in listings)
            {
                var linkElement = list.FindElement(By.CssSelector(SRPLocators.CommonLocators.PVListingLink.GetAttribute<DescriptionAttribute>().Description));
                var listTitleLink = linkElement.GetAttribute("href");
                if (listTitleLink.Contains(adId))
                {
                    var proximityText = new StringBuilder();
                    var proximityList = list.FindElements(By.CssSelector(SRPLocators.LargeLocators.Proximity.GetAttribute<DescriptionAttribute>().Description));
                    foreach (var proximity in proximityList)
                    {
                        if (list.Text != null)
                        {
                            proximityText.Append(proximity.Text.Trim().ToLower());
                        }
                    }
                    return proximityText.ToString();
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
                listings = FindElements(By.CssSelector(SRPLocators.CommonLocators.PriorityListingsLinksLarge.GetAttribute<DescriptionAttribute>().Description), 20, 1000);
                var priorityListingHeading = FindElement(By.CssSelector(SRPLocators.CommonLocators.PriorityListingsHeading.GetAttribute<DescriptionAttribute>().Description));
                ScrollTo(priorityListingHeading);
            }
            else
            {
                listings = FindElements(By.CssSelector(SRPLocators.CommonLocators.AllListingsLinksLarge.GetAttribute<DescriptionAttribute>().Description), 20, 1000);
                var allListingHeading = FindElement(By.CssSelector(SRPLocators.CommonLocators.AllListingsHeading.GetAttribute<DescriptionAttribute>().Description));
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
                        var adSortPoint = listItem.FindElement(By.CssSelector("span.show-ad-points")).Text;
                        var link = listItem.FindElement(By.CssSelector("a[class='inner-link']")).GetAttribute("href");
                        if (adSortPoint != "" && !link.Contains("ursrc=ts") && !link.Contains("ursrc=fl"))
                        {
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
                        var adSortPoint = listItem.FindElement(By.CssSelector("span.show-ad-points")).Text;
                        var link = listItem.FindElement(By.CssSelector("a[class='inner-link']")).GetAttribute("href");
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
            By saveSearchEmailLocator = By.CssSelector(SRPLocators.LargeLocators.SaveSearchEmail.GetAttribute<DescriptionAttribute>().Description);
            EnterText(saveSearchEmailLocator, emailAddress);
            IWebElement subscribeButton = FindElement(By.CssSelector(SRPLocators.LargeLocators.SaveSearchBtn.GetAttribute<DescriptionAttribute>().Description));
            ClickElement(subscribeButton);
            WaitForSavedSearchSuccessModalStatus();
        }
        public override bool GetSubscribeButtonStatus(bool isSubscribed)
        {
            if (isSubscribed)
            {
                try
                {
                    WaitForElementVisible(By.CssSelector(SRPLocators.LargeLocators.UnSubscribeSaveSearchLinkTop.GetAttribute<DescriptionAttribute>().Description));
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
                    WaitForElementVisible(By.CssSelector(SRPLocators.LargeLocators.SaveSearchBtn.GetAttribute<DescriptionAttribute>().Description));
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
            By unSubscribeLinkLocator = By.CssSelector(SRPLocators.LargeLocators.UnSubscribeSaveSearchLinkTop.GetAttribute<DescriptionAttribute>().Description);
            WaitForElementVisible(unSubscribeLinkLocator);
            ClickElement(FindElement(unSubscribeLinkLocator));
            if (IsElementVisible(By.CssSelector(SRPLocators.LargeLocators.SubscriptionSecondaryModal.GetAttribute<DescriptionAttribute>().Description)))
            {
                ClickElement(FindElement(By.CssSelector(SRPLocators.LargeLocators.SubscriptionSecondaryModalCloseBtn.GetAttribute<DescriptionAttribute>().Description)));
                WaitForElementNotVisible(By.CssSelector(SRPLocators.LargeLocators.SubscriptionSecondaryModal.GetAttribute<DescriptionAttribute>().Description));
            }
            WaitForElementVisible(By.CssSelector(SRPLocators.LargeLocators.SaveSearchBtn.GetAttribute<DescriptionAttribute>().Description));
        }

        #region PreQual
        internal override bool? VerifyPriceFilterForPreQual(PreQualLocators.PreQual resultType, string preQualMaxAmount = null)
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

        #region SEO
        public override void ClickShowMoreYearsLinkAbs()
        {
            ClickElement(FindElement(By.CssSelector(SRPLocators.SEOLinks.ShowMoreYearsLink.GetAttribute<DescriptionAttribute>().Description)));
        }
        public override string GetIntroDescSEOWidgetAbs()
        {
            return GetElementText(FindElement(By.CssSelector(SRPLocators.SEOLinks.IntroDesc.GetAttribute<DescriptionAttribute>().Description)));
        }
        #endregion
    }
}