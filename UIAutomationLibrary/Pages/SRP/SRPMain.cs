using MarketPlaceWeb.Base;
using MarketPlaceWeb.Locators;
using MarketPlaceWeb.Pages.SRP;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using static MarketPlaceWeb.Locators.SRPLocators;

namespace MarketPlaceWeb.Pages
{
    public class SRPMain : Page
    {
        SRPAbstract sRPPage;
        private const string _defaultValue = null;

        public SRPMain(IWebDriver driver, Viewport viewport, Language language)
        {
            base.driver = driver;
            base.viewport = viewport;
            base.language = language;
            switch (viewport)
            {
                case Viewport.Large:
                    sRPPage = new SRPLarge(driver, viewport, language);
                    break;
                case Viewport.XS:
                    sRPPage = new SRPXS(driver, viewport, language);
                    break;
                case Viewport.Small:
                    sRPPage = new SRPSmall(driver, viewport, language);
                    break;
            }
        }

        public string srpURL(Language language, Category category, dynamic testDataFile)
        {
            switch (category)
            {
                case Category.PV:
                    {
                        return (language.ToString() == "EN") ? GetTestData(testDataFile, "urls.srpUrlPvEn") : GetTestData(testDataFile, "urls.srpUrlPvFr");
                    }
                case Category.PVFiltered:
                    {
                        return (language.ToString() == "EN") ? GetTestData(testDataFile, "urls.srpUrlAutomationPvEn") : GetTestData(testDataFile, "urls.srpUrlAutomationPvFr");
                    }
                case Category.CommercialHeavyTrucks:
                    {
                        return (language.ToString() == "EN") ? GetTestData(testDataFile, "urls.srpUrlHeavyTrucksEn") : GetTestData(testDataFile, "urls.srpUrlHeavyTrucksFr");
                    }
                case Category.Trailers:
                    {
                        return "";
                    }
                case Category.RVs:
                    {
                        return (language.ToString() == "EN") ? GetTestData(testDataFile, "urls.srpUrlRvEn") : GetTestData(testDataFile, "urls.srpUrlRvFr");
                    }
                case Category.Boats:
                    {
                        return (language.ToString() == "EN") ? GetTestData(testDataFile, "urls.srpUrlRBoatsEn") : GetTestData(testDataFile, "urls.srpUrlRBoatsFr");
                    }
                case Category.Watercraft:
                    {
                        return (language.ToString() == "EN") ? GetTestData(testDataFile, "urls.srpUrlWatercraftEn") : GetTestData(testDataFile, "urls.srpUrlWatercraftFr");
                    }
                case Category.BikesAndATVs:
                    {
                        return (language.ToString() == "EN") ? GetTestData(testDataFile, "urls.srpUrlBikesEn") : GetTestData(testDataFile, "urls.srpUrlBikesFr");
                    }
                case Category.Snowmobiles:
                    {
                        return "";
                    }
                case Category.HeavyEquipment:
                    {
                        return "";
                    }
                case Category.Farm:
                    {
                        return "";
                    }
                default:
                    {
                        return "";
                    }
            }
        }

        public string directLinkURL(Language language, string filters, dynamic testDataFile)
        {
            switch (filters)
            {
                case "MakeModelTrimYearPrice":
                    return (language.ToString() == "EN") ? GetTestData(testDataFile, "urls.directLinkUrls.makeModelTrimYearPriceUrlEn") : GetTestData(testDataFile, "urls.directLinkUrls.makeModelTrimYearPriceUrlFr");

                case "LocDistncTrimBdTypExtClrDrvtrnFuelEngTransmisssionSeatDoors":
                    return (language.ToString() == "EN") ? GetTestData(testDataFile, "urls.directLinkUrls.locDisTrmBodyExtClrDrvtrnFuelEngTrnsSeatDoorsUrlEn") : GetTestData(testDataFile, "urls.directLinkUrls.locDisTrmBodyExtClrDrvtrnFuelEngTrnsSeatDoorsUrlFr");

                case "LengthWeightSleepsSlideOuts":
                    return (language.ToString() == "EN") ? GetTestData(testDataFile, "urls.directLinkUrls.lengthWeightSleepsSlideOutsUrlEn") : GetTestData(testDataFile, "urls.directLinkUrls.lengthWeightSleepsSlideOutsUrlFr");

                case "HoursAndSubType":
                    return (language.ToString() == "EN") ? GetTestData(testDataFile, "urls.directLinkUrls.hoursAndSubTypeUrlEn") : GetTestData(testDataFile, "urls.directLinkUrls.hoursAndSubTypeUrlFr");

                case "Horsepower":
                    return (language.ToString() == "EN") ? GetTestData(testDataFile, "urls.directLinkUrls.horsepowerUrlEn") : GetTestData(testDataFile, "urls.directLinkUrls.horsepowerUrlFr");

                case "EngineSize":
                    return (language.ToString() == "EN") ? GetTestData(testDataFile, "urls.directLinkUrls.engineSizeUrlEn") : GetTestData(testDataFile, "urls.directLinkUrls.engineSizeUrlFr");

                default:
                    return "";
            }
        }

        public bool VerifyPaymentCalculatorPillsOrderOnSRP() => sRPPage.VerifyPaymentCalculatorPillsOrderOnSRP();        

        #region dealerInventoryPage
        public void SubmitLeadForm(DealerInventoryLeadForm dealerInventoryLead)
        {
            sRPPage.SendEmailLeadAbs(dealerInventoryLead);
        }

        public bool IsDipLeadFeedbackMsgDisplayed()
        {
            return sRPPage.IsDipLeadFeedbackMsgDisplayed();
        }

        public void ClickOKBtnDipLead()
        {
            sRPPage.ClickOKBtnDipLead();
        }
        #endregion

        public void OpenFacetAbs(SRPLocators.Facets facets, SRPAbstract.FacetStatus facetStatus, SrpDesign srpDesign = SrpDesign.Old)
        {
            sRPPage.OpenFacetAbs(facets, facetStatus, srpDesign);
        }

        public int GetTotalNumberOfFound(SrpDesign srpDesign = SrpDesign.Old)
        {
            return sRPPage.GetTotalNumberOfFoundAbs(srpDesign);
        }

        public int GetTotalNumberOfFoundInFacetOption(SRPLocators.Facets facet, string option)
        {
            return sRPPage.GetTotalNumberOfFoundInFacetOptionAbs(facet, option);
        }

        public string GetSelectedValueOfFacet(SRPLocators.Facets facets, HeaderFooterLocators.Categories category = HeaderFooterLocators.Categories.CarsTrucksSuv, SrpDesign srpDesign = SrpDesign.Old)
        {
            return sRPPage.GetSelectedValueOfFacetAbs(facets, category, srpDesign);
        }

        public bool ContainsInFacet(SRPLocators.Facets facets, string value1 = _defaultValue, string value2 = _defaultValue, string value3 = _defaultValue, SrpDesign srpDesign = SrpDesign.Old)
        {
            return sRPPage.ContainsInFacet(facets, value1, value2, value3, srpDesign);
        }

        public string GetTitleText()
        {
            return sRPPage.GetTitleTextAbs();
        }

        public void WaitForSRPPageLoad(int timeOut = 60)
        {
            sRPPage.WaitForSRPPageLoad(timeOut);
        }

        /// <summary>
        /// This method is verifying title in all listings found on current page.
        /// Method one or two values and parameter toCompare which determines which comparison will be handled.
        /// </summary>
        /// <param name="value 1"></param>
        /// <param name="toCompare"></param>
        /// <param name="value 2"></param>
        public bool ContainsInListingTitle(string value, string toCompare = _defaultValue, string value2 = _defaultValue)
        {
            return sRPPage.ContainsInListingTitleAbs(value, toCompare, value2);
        }

        /// <summary>
        /// This method is verifying price in all listings found on current page.
        /// Method one or two values and parameter toCompare which determines which comparison will be handled.
        /// </summary>
        /// <param name="value 1"></param>
        /// <param name="toCompare"></param>
        /// <param name="value 2"></param>
        public bool ListingPrice(string value, string toCompare = _defaultValue, string value2 = _defaultValue)
        {
            return sRPPage.ListingPriceAbs(value, toCompare, value2);
        }


        /// <summary>
        /// This method is verifying mileage in all listings found on current page.
        /// Method one or two values and parameter toCompare which determines which comparison will be handled.
        /// </summary>
        /// <param name="value 1"></param>
        /// <param name="toCompare"></param>
        /// <param name="value 2"></param>
        public bool IsMileageWithinRange(string value, string toCompare = _defaultValue, string value2 = _defaultValue)
        {
            return sRPPage.IsMileageWithinRangeAbs(value, toCompare, value2);
        }

        /// <summary>
        /// This method is verifying proximity in all listings found on current page.
        /// Method comparing if all values are within selected proximity.
        /// </summary>
        /// <param name="proximity"></param>
        public bool IsProximityWithinRange(int proximity)
        {
            return sRPPage.IsProximityWithinRangeAbs(proximity);
        }

        /// <summary>
        /// This method is verifying if all listing have private badge.
        /// </summary>
        public bool IsEveryListingPrivate()
        {
            return sRPPage.IsEveryListingPrivateAbs();
        }

        public bool IsEveryListingDealer()
        {
            return sRPPage.IsEveryListingDealerAbs();
        }

        /// <summary>
        /// This method is verifying if all listing have Certified Pre-Owned badge.
        /// </summary>
        public bool IsEveryListingCPO()
        {
            return sRPPage.IsEveryListingCPOAbs();
        }

        /// <summary>
        /// This method is verifying if all listing have NEW listing badge.
        /// </summary>
        public bool IsEveryListingNew()
        {
            return sRPPage.IsEveryListingNewAbs();
        }

        /// <summary>
        /// This method is verifying if all listings contain a photo.
        /// </summary>
        public bool HasEveryListingPhoto()
        {
            return sRPPage.HasEveryListingPhotoAbs();
        }

        public void SelectLocationFacet(string location)
        {
            sRPPage.SelectLocationFacetAbs(location);
        }

        public void SelectSearchRadiusFacet(SRPLocators.SearchRadius searchRadius, SrpDesign srpDesign = SrpDesign.Old)
        {
            sRPPage.SelectSearchRadiusFacetAbs(searchRadius, srpDesign);
        }

        public void SelectConditionFacetSingle(SRPLocators.Condition condition, bool toBeChecked, SrpDesign srpDesign = SrpDesign.Old)
        {
            sRPPage.SelectConditionFacetSingleAbs(condition, toBeChecked, srpDesign);
        }

        public void SelectSellerTypeFacet(SRPLocators.SellerType sellerType, bool toBeChecked, SrpDesign srpDesign = SrpDesign.Old)
        {
            sRPPage.SelectSellerTypeFacetAbs(sellerType, toBeChecked, srpDesign);
        }

        public void SelectMakeFacet(string make)
        {
            sRPPage.SelectMakeFacetAbs(make);
        }

        public void SelectModelFacet(string model)
        {
            sRPPage.SelectModelFacetAbs(model);
        }

        public bool IsBuyingOptionsChecked(SRPLocators.BuyingOptions contactlessServices, SrpDesign srpDesign = SrpDesign.Old)
        {
            return sRPPage.IsBuyingOptionsCheckedAbs(contactlessServices, srpDesign);
        }
        #region PreQual
        public bool? VerifyPriceFilterForPreQual(PreQualLocators.PreQual resultType, string preQualMaxAmount = null) => sRPPage.VerifyPriceFilterForPreQual(resultType, preQualMaxAmount);
        #endregion

        public void SelectBuyingOptionsSingle(SRPLocators.BuyingOptions contactlessServices, bool toBeChecked, SrpDesign srpDesign = SrpDesign.Old)
        {
            sRPPage.SelectBuyingOptionsSingleAbs(contactlessServices, toBeChecked, srpDesign);
        }

        public void SelectTrimFacetSingle(string trim, bool toBeChecked)
        {
            sRPPage.SelectTrimFacetSingleAbs(trim, toBeChecked);
        }

        /// <summary>
        /// This method enters Min Year or Max Year or both the years.
        /// While passing Min Year only, the MaxYear has to be "0" or blank string "". While passing MaxYear only, the MinYear has to be "0" or blank string "".
        /// </summary>
        /// <param name="year">Enum</param>
        /// <param name="minYear"></param>
        /// <param name="maxYear"></param>
        public void SelectYearFacet(SRPLocators.Year year, string minYear, string maxYear, SrpDesign srpDesign = SrpDesign.Old)
        {
            sRPPage.SelectYearFacetAbs(year, minYear, maxYear, srpDesign);
        }

        public void SelectPaymentsFacet(string minPayment, string maxPayment, SRPLocators.PaymentFrequency paymentFrequency, SRPLocators.Term term, string downPayment, string tradeInValue, SrpDesign srpDesign = SrpDesign.Old)
        {
            sRPPage.SelectPaymentsFacetAbs(minPayment, maxPayment, paymentFrequency, term, downPayment, tradeInValue, srpDesign);
        }

        public void SelectBodyTypeFacetSingle(SRPLocators.BodyType bodyType, bool toBeChecked)
        {
            sRPPage.SelectBodyTypeFacetSingleAbs(bodyType, toBeChecked);
        }

        public void SelectExteriorColourFacetSingle(SRPLocators.ExteriorColour exteriorColour, bool toBeChecked)
        {
            sRPPage.SelectExteriorColourFacetSingleAbs(exteriorColour, toBeChecked);
        }

        public void SelectDrivetrainFacetSingle(SRPLocators.Drivetrain drivetrain, bool toBeChecked)
        {
            sRPPage.SelectDrivetrainFacetSingleAbs(drivetrain, toBeChecked);
        }

        public void SelectFuelTypeFacetSingle(SRPLocators.FuelType fuelType, bool toBeChecked)
        {
            sRPPage.SelectFuelTypeFacetSingleAbs(fuelType, toBeChecked);
        }

        public void SelectEngineFacetSingle(SRPLocators.Engine engine, bool toBeChecked)
        {
            sRPPage.SelectEngineFacetSingleAbs(engine, toBeChecked);
        }

        public void SelectTransmissionFacetSingle(SRPLocators.Transmission transmission, bool toBeChecked)
        {
            sRPPage.SelectTransmissionFacetSingleAbs(transmission, toBeChecked);
        }

        public void SelectSeatingCapacityFacetSingle(SRPLocators.SeatingCapacity seatingCapacity, bool toBeChecked)
        {
            sRPPage.SelectSeatingCapacityFacetSingleAbs(seatingCapacity, toBeChecked);
        }

        public void SelectDoorsFacetSingle(SRPLocators.Doors doors, bool toBeChecked)
        {
            sRPPage.SelectDoorsFacetSingleAbs(doors, toBeChecked);
        }

        public void SelectSleepsFacetSingle(SRPLocators.Sleeps sleeps, bool toBeChecked)
        {
            sRPPage.SelectSleepsFacetSingleAbs(sleeps, toBeChecked);
        }

        public void SelectSubTypeFacetSingle(SRPLocators.SubType subType, bool toBeChecked)
        {
            sRPPage.SelectSubTypeFacetSingleAbs(subType, toBeChecked);
        }

        /// <summary>
        /// This method selects first available option from these facets only 1. Exterior Colour, 2. Transmission, 3. Seating Capacity, 4. Doors, 5. Fuel Type, 6. Slide Outs, 7. Sleeps, 8. Body Type
        /// </summary>
        /// <param name="facet"></param>
        public void SelectFirstCheckboxFromFacet(SRPLocators.Facets facet)
        {
            sRPPage.SelectFirstCheckboxFromFacetAbs(facet);
        }

        public void SelectFirstOptionFromMakeModelTypeSubType(SRPLocators.Facets facet)
        {
            sRPPage.SelectFirstOptionFromMakeModelTypeSubTypeAbs(facet);
        }

        public void SelectOtherOptionsSingle(SRPLocators.OtherOptions otherOptions, bool toBeChecked, SrpDesign srpDesign = SrpDesign.Old)
        {
            sRPPage.SelectOtherOptionsSingleAbs(otherOptions, toBeChecked, srpDesign);
        }

        public bool IsOtherOptionChecked(SRPLocators.OtherOptions otherOptions)
        {
            return sRPPage.IsOtherOptionCheckedAbs(otherOptions);
        }

        public void ClearFacet(SRPLocators.Facets facets, SrpDesign srpDesign = SrpDesign.Old)
        {
            sRPPage.ClearFacetAbs(facets, srpDesign);
        }

        public void ClearAllFacets()
        {
            sRPPage.ClearAllFacetsAbs();
        }

        public void ClickOnApplyOnFacet(SRPLocators.Facets facets)
        {
            sRPPage.ClickOnApplyOnFacet(facets);
        }

        public void CloseFacet(SRPLocators.Facets facets, SrpDesign srpDesign = SrpDesign.Old)
        {
            sRPPage.CloseFacetAbs(facets, srpDesign);
        }

        public bool IsHomeDeliveryOn()
        {
            return sRPPage.IsHomeDeliveryOn();
        }

        public void SelectSort(SRPLocators.Sort sort)
        {
            sRPPage.SelectSort(sort);
        }

        public void SelectDisplay(SRPLocators.Display display)
        {
            sRPPage.SelectDisplay(display);
        }

        public void SelectCheckboxMultiple(SRPLocators.Facets facet, List<string> listOfOption, bool toBeChecked)
        {
            sRPPage.SelectCheckboxMultipleAbs(facet, listOfOption, toBeChecked);
        }

        public bool IsCheckboxStatusMultiple(SRPLocators.Facets facet, Dictionary<string, bool> listOfOptionStatus)
        {
            return sRPPage.IsCheckboxStatusMultiple(facet, listOfOptionStatus);
        }

        public string GetFacetElementMinText(SRPLocators.Facets facet)
        {
            return sRPPage.GetFacetElementMinText(facet);
        }

        public string GetFacetElementMaxText(SRPLocators.Facets facet)
        {
            return sRPPage.GetFacetElementMaxText(facet);
        }

        public string CustomGetSelectedText(SRPLocators.Facets facet)
        {
            return sRPPage.CustomGetSelectedText(facet);
        }

        public string GetYearMinSelectedText(SrpDesign srpDesign = SrpDesign.Old)
        {
            return sRPPage.GetYearMinSelectedText(srpDesign);
        }

        public string GetYearMaxSelectedText(SrpDesign srpDesign = SrpDesign.Old)
        {
            return sRPPage.GetYearMaxSelectedText(srpDesign);
        }

        public void EnterMinMaxFacet(SRPLocators.Facets facet, string minValue, string maxValue, SrpDesign srpDesign = SrpDesign.Old)
        {
            sRPPage.EnterMinMaxFacetAbs(facet, minValue, maxValue, srpDesign);
        }

        public string GetDisplay()
        {
            return sRPPage.GetDisplay();
        }

        public void ClickOnFirstOrganicListing(HeaderFooterLocators.Categories category = HeaderFooterLocators.Categories.CarsTrucksSuv)
        {
            sRPPage.ClickOnFirstOrganicListingAbs(category);
        }

        /// <summary>
        /// It returns a specific listing title
        /// </summary>
        /// <param name="sequenceNo"></param>
        /// <returns></returns>
        public string GetOrganicListingTitle(int sequenceNo, HeaderFooterLocators.Categories category)
        {
            return sRPPage.GetOrganicListingTitleAbs(sequenceNo, category);
        }

        public void CloseFeedbackPopUp()
        {
            sRPPage.CloseFeedbackPopUp();
        }

        public bool IsEasyFinanceBadgeDisplayed()
        {
            return sRPPage.IsEasyFinanceBadgeDisplayed();
        }

        public bool IsEasyFinanceBadgeDisplayedForAllListings()
        {
            return sRPPage.IsEasyFinanceBadgeDisplayedForAllListings();
        }

        public void OpenEasyFinanceModal()
        {
            sRPPage.OpenEasyFinanceModal();
        }

        public enum Category
        {
            PV,
            CommercialHeavyTrucks,
            Trailers,
            RVs,
            Boats,
            Watercraft,
            BikesAndATVs,
            Snowmobiles,
            HeavyEquipment,
            Farm,
            PVFiltered
        }

        public IDictionary<SRPAbstract.ListingsType, List<double>> GetAdSortPoints(bool isPriorityListing = false)
        {
            return sRPPage.GetAdSortPoints(isPriorityListing);
        }
        public string GetSortType()
        {
            return sRPPage.GetSortType();
        }
        public IWebElement GetFirstMatchedListing(SRPAbstract.ListingsType listingsType)
        {
            return sRPPage.GetFirstMatchedListing(listingsType);
        }
        public SRPAbstract.ListingImage GetListingMainImage(IWebElement element)
        {
            return sRPPage.GetListingMainImage(element);
        }
        public SRPAbstract.ListingImage GetListingStripeImage(IWebElement element)
        {
            return sRPPage.GetListingStripeImage(element);
        }

        public int GetOrganicListingsDisplayCount()
        {
            return sRPPage.GetOrganicListingsDisplayCount();
        }
        #region SEO
        public string GetLocalSRPLink()
        {
            return sRPPage.GetLocalSRPLink();

        }
        public bool IsSEOWidgetDisplayingOnSRPPage(SEOLinks widget)
        {
            return sRPPage.IsSEOWidgetDisplayingOnSRPPage(widget);
        }
        public void ClickSEOWidgetLink(SRPLocators.SEOLinks link)
        {
            sRPPage.ClickSEOWidgetLink(link);
        }
        public void ClickOnLinkFromList(int num, SEOLinks element)
        {
            IList<IWebElement> elements = FindElements(By.CssSelector(element.GetAttribute<DescriptionAttribute>().Description));
            if (elements.Count < num)
            {
                throw new Exception("Year Number should be less than total number of Years.");
            }
            else
            {
                ScrollTo(elements[num]);

                ClickElement(elements[num]);
            }
        }
        public string GetSEOElementsText(SEOLinks element)
        { return sRPPage.GetSEOElementsText(element); }

        public string GetElementFromListSEO(int num, SEOLinks element)
        {

            return sRPPage.GetElementFromListSEO(num, element);
        }

        public void ClickShowMoreYearsLink()
        {sRPPage.ClickShowMoreYearsLinkAbs(); }

        public string GetIntroDescSEOWidget()
        { return sRPPage.GetIntroDescSEOWidgetAbs(); }
        public string[] GetDiscoverModelsSRPSEOWidgets(SEOLinks element)
        {
            return sRPPage.GetDiscoverModelsSRPSEOWidgets(element);
        }
        public string GetPopularWidgetTitle()
        {
            return sRPPage.GetPopularWidgetTitle();
        }

        public bool IsPoluparWidgetModelDisplayed(string model)
        {
            return sRPPage.IsPoluparWidgetModelDisplayed(model);
        }

        public string GetPopularWidgetModelLink(string model)
        {
            return sRPPage.GetPopularWidgetModelLink(model);
        }

        public int GetTotalWidgetModelCount()
        {
            return sRPPage.GetTotalWidgetModelCount();
        }

        public bool IsPopularWidgetTitle()
        {

            return sRPPage.IsPopularWidgetTitle();
        }

        #endregion
        public bool IsSpinIconDisplayedOnSrp(IWebElement element)
        {
            return sRPPage.IsSpinIconDisplayedOnSrp(element);
        }
        public void ClickOnListing(IWebElement element)
        {
            sRPPage.ClickOnListing(element);
        }
        public string GetListingProximity(string adId)
        {
            return sRPPage.GetListingProximity(adId);
        }
        public void SubscribeSaveSearch(string emailAddress)
        {
            sRPPage.SubscribeSaveSearch(emailAddress);
        }
        public bool IsSavedSearchSuccessDisplayed()
        {
            return sRPPage.IsSavedSearchSuccessDisplayed();
        }
        public void CloseSavedSearchSuccessModal()
        {
            sRPPage.CloseSavedSearchSuccessModal();
        }
        public bool GetSubscribeButtonStatus(bool isSubscribed = true)
        {
            return sRPPage.GetSubscribeButtonStatus(isSubscribed);
        }
        public void UnSubscribeSaveSearch()
        {
            sRPPage.UnSubscribeSaveSearch();
        }
        public string GetToasterMessage()
        {
            return sRPPage.GetToasterMessage();
        }
        public bool IsRedirectedToCorrectSRPFromMyGarage(string srpSubURL, string searchName)
        {
            return sRPPage.IsRedirectedToCorrectSRPFromMyGarage(srpSubURL, searchName);
        }
        public bool IsAllListingWithoutTraditionDealer(Proximity proximity)
        {
            return sRPPage.IsAllListingWithoutTraditionDealer(proximity);
        }
        public void ClickOnHomeDelivery()
        {
            sRPPage.ClickOnHomeDelivery();
        }
        public string GetHomeDeliveryTooltipMessage()
        {
            return sRPPage.GetHomeDeliveryTooltipMessage();
        }
        public string GetHomeDeliveryInfoIconTooltipMessage()
        {
            return sRPPage.GetHomeDeliveryInfoIconTooltipMessage();
        }
    } 


    }