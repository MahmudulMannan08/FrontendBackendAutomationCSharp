using System;
using System.ComponentModel;

namespace MarketPlaceWeb.Locators
{
    public class SRPLocators
    {
        public enum Facets
        {
            [FacetLocator("#faceted-parent-Location a[class='dropdown-toggle row']"), FacetRowLocator("#faceted-parent-Location"), FacetValueLocator("#faceted-Location"), SelectedFacetSpan("#faceted-Location")]
            LocationParent,   
            [FacetLocator("#faceted-parent-Location a[class='dropdown-toggle row']"), ApplyButtonSelector("#applyLocation"), FacetValueLocator("#faceted-Location"), FacetRowLocator("#faceted-parent-Location"), SelectedFacetSpan("#faceted-Location"), CloseButtonLocator("#cityOrPostalCodeLocation #closeLocation i"), BackButtonLocator("#backLocation")]
            CityPostalCodeChild,
            [DropdownLocator("#locationKilometers ul"), DropdownFacetRowLocator("#locationKilometers")]
            SearchRadiusChild,
            [FacetLocator("#faceted-parent-Location .proximity-label"), ApplyButtonSelector("#applyLocation"), FacetValueLocator("#faceted-Proximity"), FacetRowLocator("#faceted-parent-Location"), SelectedFacetSpan("#faceted-Proximity"), CloseButtonLocator("#closeLocation i"), DropdownLocator("#proximity")]
            Distance,
            [FacetLocator("#faceted-parent-Vehicle a[class='dropdown-toggle row']"), FacetRowLocator("#faceted-parent-Vehicle")]
            VehicleParent,
            [FacetLocator("#faceted-parent-Make a[class='dropdown-toggle row']"), FacetValueLocator("#faceted-Make"), ClearButtonLocator("#clearMake"), ClearButtonLocatorSXS("#clearMakeButton"), FacetList("#rfMakes li"), FacetRowLocator("#faceted-parent-Make"), SelectedFacetSpan("#faceted-Make"), SearchTextLocator("#makeSearch"), CloseButtonLocator("#closeMake i"), BackButtonLocator("#backMake")]
            MakeChild,
            [FacetLocator("#faceted-parent-Model a[class='dropdown-toggle row']"), FacetValueLocator("#faceted-Model"), ClearButtonLocator("#clearModel"), ClearButtonLocatorSXS("#clearModelButton"), FacetList("#rfModel li"), FacetRowLocator("#faceted-parent-Model"), SelectedFacetSpan("#faceted-Model"), CloseButtonLocator("#closeModel i"), BackButtonLocator("#backModel")]
            ModelChild,
            [FacetLocator("#faceted-parent-Trim a[class='dropdown-toggle row']"), ApplyButtonSelector("#applyTrim"), FacetValueLocator("#faceted-Trim"), ClearButtonLocator("#clearTrim"), ClearButtonLocatorSXS("#clearTrimButton"), FacetList("#fbTrim input"), FacetLabel("#fbTrim label"), FacetRowLocator("#faceted-parent-Trim"), SelectedFacetSpan("#faceted-parent-Trim .comma"), CloseButtonLocator("#closeTrims i"), BackButtonLocator("#backTrim")]
            TrimChild,
            [FacetLocator("#faceted-parent-BodyType a[class='dropdown-toggle row']"), ApplyButtonSelector("#applyBodyStyles"), FacetValueLocator("#faceted-BodyType"), ClearButtonLocator("#clearBodyType"), ClearButtonLocatorSXS("#clearBodyTypeButton"), FacetList("#rfBodyStyle input"), FacetLabel("#rfBodyStyle label"), FacetRowLocator("#faceted-parent-BodyType"), SelectedFacetSpan("#faceted-parent-BodyType .comma"), CloseButtonLocator("#closeBodyType i"), BackButtonLocator("#backBodyType")]
            BodyTypeChild,
            [FacetLocator("#faceted-parent-Colours a[class='dropdown-toggle row']"), ApplyButtonSelector("#applyColour"), FacetValueLocator("#faceted-Colours"), ClearButtonLocator("#clearExteriorColour"), ClearButtonLocatorSXS("#clearExteriorColourButton"), FacetList("#fbExteriorColour input"), FacetLabel("#fbExteriorColour label"), FacetRowLocator("#faceted-parent-Colours"), SelectedFacetSpan("#faceted-parent-Colours .comma"), CloseButtonLocator("#closeExteriorColour i"), BackButtonLocator("#backExteriorColor")]
            ColourChild,
            [FacetLocator("#faceted-parent-Condition a[class='dropdown-toggle row']"), ApplyButtonSelector("#applyCondition"), FacetValueLocator("#faceted-condition"), FacetRowLocator("#faceted-parent-Condition"), FacetList("#faceted-parent-Condition input"), SelectedFacetSpan("#faceted-parent-Condition .comma"), FacetLabel("#faceted-parent-Condition label"), CloseButtonLocator("#closeCondition i")]
            ConditionParent,
            [FacetLocator("#faceted-parent-Mileage a[class='dropdown-toggle row']"), ApplyButtonSelector("#applyMileage"), FacetValueLocator("#faceted-Mileage"), ClearButtonLocator("#clearMileage"), FacetRowLocator("#faceted-parent-Mileage"), SelectedFacetSpan("#faceted-Mileage"), CloseButtonLocator("#closeMileage i"), MinValueField("#rfKilometresLow"), MaxValueField("#rfKilometresHigh")]
            KilometersChild,
            [FacetLocator("#faceted-parent-Year a[class='dropdown-toggle row']"), ApplyButtonSelector("#applyYear"), FacetValueLocator("#faceted-Year"), ClearButtonLocator("#clearYear"), FacetRowLocator("#faceted-parent-Year"), SelectedFacetSpan("#faceted-Year"), CloseButtonLocator("#closeYear i"), MinDropdownOldSrpLocator("#yearLow"), MaxDropdownOldSrpLocator("#yearHigh"), MinDropdownLocator("#dropdownYearLow"), MaxDropdownLocator("#dropdownyearHigh"), MinDropdownFacetRowLocator("#faceted-parent-Year div.year-min"), MaxDropdownFacetRowLocator("#faceted-parent-Year div.year-max")]
            YearChild,
            [FacetLocator("#faceted-parent-Type a[class='dropdown-toggle row']"), ApplyButtonSelector("#applySeller"), FacetValueLocator("#faceted-type"), FacetRowLocator("#faceted-parent-Type"), SelectedFacetSpan("##faceted-parent-Type .comma"), CloseButtonLocator("#closeSellerType i"), FacetList("#faceted-parent-Type input"), SellerTypeOSP("#faceted-parent-Type .comma #imgOnlineSellerPlus")]
            SellerTypeParent,
            [FacetLocator("#faceted-parent-Csp a[class='dropdown-toggle row']"), ApplyButtonSelector("#applyCsp"), ClearButtonLocator("#clearCsp"), FacetList("#rfCsp input"), FacetRowLocator("#faceted-parent-Csp"), SelectedFacetSpan("#faceted-parent-Csp .comma"), CloseButtonLocator("#closeCsp i")]
            AtHomeServices,
            [FacetLocator("#faceted-parent-Price a[class='dropdown-toggle row']"), ApplyButtonSelector("#applyPrice"), FacetValueLocator("#faceted-Price"), ClearButtonLocator("#clearPrice"), CloseButtonLocator("#closePrice i"), MinValueField("#rfPriceLow"), MaxValueField("#rfPriceHigh"), FacetRowLocator("#faceted-parent-Price"), SelectedFacetSpan("#faceted-Price"), PriceTabRowLocator("#cashTab"), PaymentsTabRowLocator("#financeTab"), PriceTabNewSRP("#cash"), PriceTabOldSRP("a[href='#priceTab']"), PaymentsTabNewSRP("#finance"), PaymentsTabOldSRP("a[href='#paymentsTab']"), MinPaymentField("#rfPaymentLow"), MaxPaymentField("#rfPaymentHigh"), FacetList("#paymentsTab input[name='pricePaymentFreq']"), FacetLabel("#paymentsTab label[for*='pricePaymentFreq_']"), PaymentFrequencyDropdown("#paymentFrequency"), TermDropdown("#term"), DownPaymentField("#cashDown"), TradeInValueField("#tradeIn")]
            PricePaymentsParent,
            [FacetLocator("#faceted-parent-Mechanical a[class='dropdown-toggle row']"), FacetRowLocator("#faceted-parent-Mechanical")]
            MechanicalParent,
            [FacetLocator("#faceted-parent-Drivetrain a[class='dropdown-toggle row']"), ApplyButtonSelector("#applyDrivetrain"), FacetValueLocator("#faceted-Drivetrain"), ClearButtonLocator("#clearDrivetrain"), ClearButtonLocatorSXS("#clearDriveTrainButton"), FacetList("#fbDrivetrain input"), FacetLabel("#fbDrivetrain label"), FacetRowLocator("#faceted-parent-Drivetrain"), SelectedFacetSpan("#faceted-parent-Drivetrain .comma"), CloseButtonLocator("#closeDrivetrain i"), BackButtonLocator("#backDriveTrain")]
            DrivetrainChild,
            [FacetLocator("#faceted-parent-FuelTypes a[class='dropdown-toggle row']"), ApplyButtonSelector("#applyFuel"), FacetValueLocator("#faceted-FuelTypes"), ClearButtonLocator("#clearFuelType"), ClearButtonLocatorSXS("#clearFuelTypeButton"), FacetLabel("#fbFuelType label"), FacetList("#fbFuelType input"), FacetRowLocator("#faceted-parent-FuelTypes"), SelectedFacetSpan("#faceted-parent-FuelTypes .comma"), CloseButtonLocator("#closeFuelType i"), BackButtonLocator("#backFuelType")]
            FuelTypeChild,
            [FacetLocator("#faceted-parent-Engine a[class='dropdown-toggle row']"), ApplyButtonSelector("#applyEngine"), FacetValueLocator("#faceted-Engine"), ClearButtonLocator("#clearEngine"), ClearButtonLocatorSXS("#clearEngineButton"), FacetLabel("#fbEngine label"), FacetList("#fbEngine input"), FacetRowLocator("#faceted-parent-Engine"), SelectedFacetSpan("#faceted-parent-Engine .comma"), CloseButtonLocator("#closeEngine i"), BackButtonLocator("#backEngine")]
            EngineChild,
            [FacetLocator("#faceted-parent-Transmissions a[class='dropdown-toggle row']"), ApplyButtonSelector("#applyTransmission"), FacetValueLocator("#faceted-Transmissions"), ClearButtonLocator("#clearTransmission"), ClearButtonLocatorSXS("#clearTransmissionButton"), FacetLabel("#fbTransmission label"), FacetRowLocator("#faceted-parent-Transmissions"), SelectedFacetSpan("#faceted-parent-Transmissions .comma"), CloseButtonLocator("#closeTransmission i"), BackButtonLocator("#backTransmission")]
            TransmissionChild,
            [FacetLocator("#faceted-parent-Interior a[class='dropdown-toggle row']"), FacetRowLocator("#faceted-parent-Interior")]
            InteriorParent,
            [FacetLocator("#faceted-parent-SeatingCapacity a[class='dropdown-toggle row']"), ApplyButtonSelector("#applySeatingCapacity"), FacetValueLocator("#faceted-SeatingCapacity"), ClearButtonLocator("#clearSeatingCapacity"), ClearButtonLocatorSXS("#clearSeatingCapacityButton"), FacetLabel("#fbSeatingCapacity label"), FacetList("#fbSeatingCapacity input"), FacetRowLocator("#faceted-parent-SeatingCapacity"), SelectedFacetSpan("#faceted-parent-SeatingCapacity .comma"), CloseButtonLocator("#closeSeatingCapacity i"), BackButtonLocator("#backSeatingCapacity")]
            SeatingCapacityChild,
            [FacetLocator("#faceted-parent-NumberOfDoors a[class='dropdown-toggle row']"), ApplyButtonSelector("#applyNumberOfDoors"), FacetValueLocator("#faceted-NumberOfDoors"), ClearButtonLocator("#clearNumberOfDoors"), ClearButtonLocatorSXS("#clearNumberOfDoorsButton"), FacetLabel("#fbNumberOfDoors label"), FacetList("#fbNumberOfDoors input"), FacetRowLocator("#faceted-parent-NumberOfDoors"), SelectedFacetSpan("#faceted-parent-NumberOfDoors .comma"), CloseButtonLocator("#closeNumberOfDoors i"), BackButtonLocator("#backNumberOfDoors")]
            DoorsChild,
            [FacetLocator("#faceted-parent-Other a[class='dropdown-toggle row']"), ApplyButtonSelector("#applyOthers"), FacetRowLocator("#faceted-parent-Other"), FacetLabel("#faceted-parent-Other label"), FacetList("#faceted-parent-Other input"), SelectedFacetSpan("##faceted-parent-Other .comma"), CloseButtonLocator("#closeOther i")]
            OtherOptionsParent,
            [FacetLocator("#faceted-parent-VehicleType a[class='dropdown-toggle row']"), ApplyButtonSelector("#applyLength"), FacetValueLocator("#faceted-VehicleType"), ClearButtonLocator("#clearType"), FacetList("#rfVehicleType li"), FacetRowLocator("#faceted-parent-VehicleType"), SelectedFacetSpan("#faceted-parent-VehicleType .comma"), CloseButtonLocator("#closeType i")]
            Type,
            [FacetLocator("#faceted-parent-Wheelbase a[class='dropdown-toggle row']"), ApplyButtonSelector("#applyWheelbase"), FacetValueLocator("#faceted-Wheelbase"), ClearButtonLocator("#clearWheelbase"), MinValueField("#rfWheelbaseLow"), MaxValueField("#rfWheelbaseHigh"), FacetRowLocator("#faceted-parent-Wheelbase"), SelectedFacetSpan("#faceted-parent-Wheelbase .comma"), CloseButtonLocator("#closeWheelbase i")]
            Wheelbase,
            [FacetLocator("#faceted-parent-Length a[class='dropdown-toggle row']"), ApplyButtonSelector("#applyLength"), FacetValueLocator("#faceted-Length"), ClearButtonLocator("#clearLength"), MinValueField("#rfLengthMin"), MaxValueField("#rfLengthMax"), FacetRowLocator("#faceted-parent-Length"), SelectedFacetSpan("#faceted-parent-Length .comma"), CloseButtonLocator("#closeLength i")]
            Length,
            [FacetLocator("#faceted-parent-Weight a[class='dropdown-toggle row']"), ApplyButtonSelector("#applyWeight"), FacetValueLocator("#faceted-Weight"), ClearButtonLocator("#clearWeight"), MinValueField("#rfWeightMin"), MaxValueField("#rfWeightMax"), FacetRowLocator("#faceted-parent-Weight"), SelectedFacetSpan("#faceted-parent-Weight .comma"), CloseButtonLocator("#closeWeight i")]
            Weight,
            [FacetLocator("#faceted-parent-Sleeps a[class='dropdown-toggle row']"), ApplyButtonSelector("#applySleeps"), FacetValueLocator("#faceted-Sleeps"), ClearButtonLocator("#clearSleeps"), FacetList("#fbSleeps input"), FacetLabel("#fbSleeps label"), FacetRowLocator("#faceted-parent-Sleeps"), SelectedFacetSpan("#faceted-parent-Sleeps .comma"), CloseButtonLocator("#closeSleeps i")]
            Sleeps,
            [FacetLocator("#faceted-parent-SlideOuts a[class='dropdown-toggle row']"), ApplyButtonSelector("#applySlideOuts"), FacetValueLocator("#faceted-SlideOuts"), ClearButtonLocator("#clearSlideOuts"), FacetList("#fbSlideOuts input"), FacetLabel("#fbSlideOuts label"), FacetRowLocator("#faceted-parent-SlideOuts"), SelectedFacetSpan("#faceted-parent-SlideOuts .comma"), CloseButtonLocator("#closeSlideOuts i")]
            SlideOuts,
            [FacetLocator("#faceted-parent-VehicleSubType a[class='dropdown-toggle row']"), FacetValueLocator("#faceted-VehicleSubType"), ClearButtonLocator("#clearSubtype"), FacetList("#rfVehicleSubtype li"), FacetLabel("#rfVehicleSubtype li a"), FacetRowLocator("#faceted-parent-VehicleSubType"), SelectedFacetSpan("#faceted-parent-VehicleSubType .comma"), CloseButtonLocator("#closeSubtype i")]
            SubType,
            [FacetLocator("#faceted-parent-Hours a[class='dropdown-toggle row']"), ApplyButtonSelector("#applyHours"), FacetValueLocator("#faceted-Hours"), ClearButtonLocator("#clearHours"), MinValueField("#rfHoursLow"), MaxValueField("#rfHoursHigh"), FacetRowLocator("#faceted-parent-Hours"), SelectedFacetSpan("#faceted-parent-Hours .comma"), CloseButtonLocator("#closeHours i")]
            Hours,
            [FacetLocator("#faceted-parent-Horsepower a[class='dropdown-toggle row']"), ApplyButtonSelector("#applyHorsepower"), FacetValueLocator("#faceted-Horsepower"), ClearButtonLocator("#clearHorsepower"), MinValueField("#rfHorsepowerLow"), MaxValueField("#rfHorsepowerHigh"), FacetRowLocator("#faceted-parent-Horsepower"), SelectedFacetSpan("#faceted-parent-Horsepower .comma"), CloseButtonLocator("#closeHorsepower i")]
            Horsepower,
            [FacetLocator("#faceted-parent-EngineSize a[class='dropdown-toggle row']"), ApplyButtonSelector("#applyEngineSize"), FacetValueLocator("#faceted-EngineSize"), ClearButtonLocator("#clearEngineSize"), MinValueField("#rfEngineSizeLow"), MaxValueField("#rfEngineSizeHigh"), FacetRowLocator("#faceted-parent-EngineSize"), SelectedFacetSpan("#faceted-parent-EngineSize .comma"), CloseButtonLocator("#closeEngineSize i")]
            EngineSize
        }

        public enum SearchRadius
        {
            Plus_25,
            Plus_50,
            Plus_100,
            Plus_250,
            Plus_500,
            Plus_1000,
            Provincial,
            National
        }

        public enum Condition
        {
            [Description("#rfNew"), ConditionLabel("#rfNew + label")]
            New,
            [Description("#rfUsed"), ConditionLabel("#rfUsed + label")]
            Used,
            [Description("#rfPreOwned"), ConditionLabel("#rfPreOwned + label")]
            CertifiedPreOwned,
            [Description("#rfDamaged"), ConditionLabel("#rfDamaged + label")]
            Damaged
        }

        public enum SellerType
        {
            [Description("#rfDealer"), SellerTypeLabel("#rfDealer + label")]
            Dealer,
            [Description("#rfPrivate"), SellerTypeLabel("#rfPrivate + label")]
            Private
        }

        public enum BuyingOptions
        {
            [Description("#rfCspVirtualAppraisal"), CSPLabelLocator("label[for='rfCspVirtualAppraisal']")]
            TradeinOnline,
            [Description("#rfCspOnlineRes"), CSPLabelLocator("label[for='rfCspOnlineRes']")]
            ReserveOnline,
            [Description("#rfCspBuyOnline"), CSPLabelLocator("label[for='rfCspBuyOnline']")]
            BuyOnline,
            [Description("#rfCspHomeDelivery"), CSPLabelLocator("label[for='rfCspHomeDelivery']")]
            HomeDelivery,
            [Description("#rfCspMBG"), CSPLabelLocator("label[for='rfCspMBG']")]
            TenDayMoneyBack
        }

        public enum Year
        {
            MinYear,
            MaxYear,
            Both
        }

        public enum PaymentFrequency
        {
            [Description("#paymentsTab #pricePaymentFreq_1"), DescriptionOldSRP("1")]
            Monthly,
            [Description("#paymentsTab #pricePaymentFreq_2"), DescriptionOldSRP("3")]
            Biweekly,
            [Description("#paymentsTab #pricePaymentFreq_3"), DescriptionOldSRP("4")]
            Weekly
        }

        public enum Term
        {
            [Description("0")]
            LongestAvailable,
            [Description("96")]
            _96,
            [Description("84")]
            _84,
            [Description("72")]
            _72,
            [Description("60")]
            _60,
            [Description("48")]
            _48,
            [Description("36")]
            _36,
            [Description("24")]
            _24,
            [Description("12")]
            _12
        }

        public enum BodyType
        {
            [Description("input[data-value='Convertible']"), DescriptionFrench("input[data-value='Décapotable ou cabriolet']")]
            Convertible,
            [Description("input[data-value='Coupe']"), DescriptionFrench("input[data-value='Coupé']")]
            Coupe,
            [Description("input[data-value='Hatchback']"), DescriptionFrench("input[data-value='Hatchback']")]
            Hatchback,
            [Description("input[data-value='Minivan']"), DescriptionFrench("input[data-value='Minifourgonnette ou fourgonnette']")]
            Minivan,
            [Description("#rfBodyStyle input[data-value=\"Other/Don't Know\"]"), DescriptionFrench("input[data-value=\"Autre / Je ne sais pas\"]")]
            OtherDontKnow,
            [Description("input[data-value='Sedan']"), DescriptionFrench("input[data-value='Berline']")]
            Sedan,
            [Description("input[data-value='SUV']"), DescriptionFrench("input[data-value='VUS']")]
            SUV,
            [Description("input[data-value='Truck']"), DescriptionFrench("input[data-value='Camion']")]
            Truck,
            [Description("input[data-value='Wagon']"), DescriptionFrench("input[data-value='Familiale']")]
            Wagon
        }

        public enum ExteriorColour
        {
            [Description("input[data-value='Beige']")]
            Beige,
            [Description("input[data-value='Black']"), DescriptionFrench("input[data-value='Noir']")]
            Black,
            [Description("input[data-value='Blue']")]
            Blue,
            [Description("input[data-value='Bronze']")]
            Bronze,
            [Description("input[data-value='Brown']")]
            Brown,
            [Description("input[data-value='Burgundy']")]
            Burgundy,
            [Description("input[data-value='Charcoal']")]
            Charcoal,
            [Description("input[data-value='cognac & charcoal']")]
            CognacAndCharcol,
            [Description("input[data-value='Copper']")]
            Copper,
            [Description("input[data-value='Cream']")]
            Cream,
            [Description("input[data-value='Dark Blue']")]
            DarkBlue,
            [Description("input[data-value='Dark Green']")]
            DarkGreen,
            [Description("input[data-value='Dark Grey']")]
            DarkGrey,
            [Description("input[data-value='Gold']")]
            Gold,
            [Description("input[data-value='Green']")]
            Green,
            [Description("input[data-value='Grey']")]
            Grey,
            [Description("input[data-value='Light Blue']")]
            LightBlue,
            [Description("input[data-value='Light Green']")]
            LightGreen,
            [Description("input[data-value='Light Grey']")]
            LightGrey,
            [Description("input[data-value='Maroon']")]
            Maroon,
            [Description("input[data-value='Not Specified']")]
            NotSpecified,
            [Description("input[data-value='Orange']")]
            Orange,
            [Description("input[data-value='Pewter']")]
            Pewter,
            [Description("input[data-value='Pink']")]
            Pink,
            [Description("input[data-value='Purple']")]
            Purple,
            [Description("input[data-value='Red']")]
            Red,
            [Description("input[data-value='Silver']")]
            Silver,
            [Description("input[data-value='Stone']")]
            Stone,
            [Description("input[data-value='Tan']")]
            Tan,
            [Description("input[data-value='Taupe']")]
            Taupe,
            [Description("input[data-value='Teal']")]
            Teal,
            [Description("input[data-value='Unspecified']")]
            Unspecified,
            [Description("input[data-value='White']")]
            White,
            [Description("input[data-value='Yellow']")]
            Yellow
        }

        public enum Drivetrain
        {
            [Description("input[data-value='All Wheel Drive']"), DescriptionFrench("input[data-value='Trans. intégrale']")]
            AllWheelDrive,
            [Description("input[data-value='Four Wheel Drive']"), DescriptionFrench("input[data-value='4 roues motrices']")]
            FourWheelDrive,
            [Description("input[data-value='Front Wheel Drive']"), DescriptionFrench("input[data-value='Traction avant']")]
            FrontWheelDrive,
            [Description("input[data-value='Rear Wheel Drive']"), DescriptionFrench("input[data-value='Propulsion arrière']")]
            RearWheelDrive,
            [Description("input[data-value='Unknown']"), DescriptionFrench("input[data-value='Inconnu']")]
            Unknown
        }

        public enum FuelType
        {
            [Description("input[data-value='Diesel']")]
            Diesel,
            [Description("input[data-value='Electric']")]
            Electric,
            [Description("input[data-value='Flex Fuel']")]
            FlexFuel,
            [Description("input[data-value='Gas/Electric Hybrid']")]
            GasElectricHybrid,
            [Description("input[data-value='Gasoline']"), DescriptionFrench("input[data-value='Essence']")]
            Gasoline,
            [Description("input[data-value='Natural Gas']")]
            NaturalGas,
            [Description("input[data-value='Unknown']")]
            Unknown
        }

        public enum Engine
        {
            [Description("#fbEngine input[data-value='10+ Cylinder']")]
            _10Cylinder,
            [Description("#fbEngine input[data-value='3 Cylinder']")]
            _3Cylinder,
            [Description("#fbEngine input[data-value='4 Cylinder']"), DescriptionFrench("#fbEngine input[data-value='4 cylindres']")]
            _4Cylinder,
            [Description("#fbEngine input[data-value='6 Cylinder']")]
            _6Cylinder,
            [Description("#fbEngine input[data-value='8 Cylinder']")]
            _8Cylinder,
            [Description("#fbEngine input[data-value='Electric Motor']")]
            ElectricMotor,
            [Description("#fbEngine input[data-value='RH1']")]
            RH1,
            [Description("#fbEngine input[data-value='Rotary']")]
            Rotary,
            [Description("#fbEngine input[data-value='Unknown']")]
            Unknown
        }

        public enum Transmission
        {
            [Description("#fbTransmission input[data-value='Automatic']"), DescriptionFrench("#fbTransmission input[data-value='Automatique']")]
            Automatic,
            [Description("#fbTransmission input[data-value='Manual']"), DescriptionFrench("#fbTransmission input[data-value='Manuelle']")]
            Manual,
            [Description("#fbTransmission input[data-value=\"Other/Don't Know\"]")]
            OtherDontKnow
        }

        public enum SeatingCapacity
        {
            [Description("#fbSeatingCapacity input[data-value='2 seats']")]
            _2Seats,
            [Description("#fbSeatingCapacity input[data-value='3 seats']")]
            _3Seats,
            [Description("#fbSeatingCapacity input[data-value='4 seats']")]
            _4Seats,
            [Description("#fbSeatingCapacity input[data-value='5 seats']"), DescriptionFrench("#fbSeatingCapacity input[data-value='5 places']")]
            _5Seats,
            [Description("#fbSeatingCapacity input[data-value='6 seats']")]
            _6Seats,
            [Description("#fbSeatingCapacity input[data-value='7 seats']")]
            _7Seats,
            [Description("#fbSeatingCapacity input[data-value='8 seats']")]
            _8Seats,
            [Description("#fbSeatingCapacity input[data-value='9+ seats']")]
            _9PlusSeats,
            [Description("#fbSeatingCapacity input[data-value='Unknown']")]
            Unknown,
            [Description("#fbSeatingCapacity input[data-value='WBE']")]
            WBE
        }

        public enum Doors
        {
            [Description("#fbNumberOfDoors input[data-value='2 Door']"), DescriptionFrench("#fbNumberOfDoors input[data-value='2 portes']")]
            _2Door,
            [Description("#fbNumberOfDoors input[data-value='3 Door']"), DescriptionFrench("#fbNumberOfDoors input[data-value='3 portes']")]
            _3Door,
            [Description("#fbNumberOfDoors input[data-value='4+ Door']"), DescriptionFrench("#fbNumberOfDoors input[data-value='4 portes et +']")]
            _4PlusDoor,
            [Description("#fbNumberOfDoors input[data-value='Unknown']"), DescriptionFrench("#fbNumberOfDoors input[data-value='Inconnu']")]
            Unknown,
        }

        public enum Sleeps
        {
            [Description("#fbSleeps input[data-value='1']")]
            _1,
            [Description("#fbSleeps input[data-value='2']")]
            _2,
            [Description("#fbSleeps input[data-value='3']")]
            _3,
            [Description("#fbSleeps input[data-value='4']")]
            _4,
            [Description("#fbSleeps input[data-value='5']")]
            _5,
            [Description("#fbSleeps input[data-value='6']")]
            _6,
            [Description("#fbSleeps input[data-value='7']")]
            _7,
            [Description("#fbSleeps input[data-value='8']")]
            _8,
            [Description("#fbSleeps input[data-value='9']")]
            _9
        }

        public enum SubType
        {
            [Description("#rfVehicleSubtype li[data-dropdownvalue='Bow Riders']")]
            BowRiders,
            [Description("#rfVehicleSubtype li[data-dropdownvalue='Cruiser']"), DescriptionFrench("#rfVehicleSubtype li[data-dropdownvalue='Croiseur']")]
            Cruiser,
            [Description("#rfVehicleSubtype li[data-dropdownvalue='Fishing Boat']"), DescriptionFrench("#rfVehicleSubtype li[data-dropdownvalue='Bateau de pêche']")]
            FishingBoat
        }

        public enum OtherOptions
        {
            [Description("#rfPhoto")]
            With_Photos,
            [Description("#rfPrice")]
            With_Price,
            [Description("#rfCarFax")]
            With_Free_CARFAX_Report
        }

        public enum Sort
        {
            [Description("CreatedDateAsc")]
            Posted_Date_Old_To_New,
            [Description("CreatedDateDesc")]
            Posted_Date_New_To_Old,
            [Description("PriceDesc")]
            Price_High_To_Low,
            [Description("PriceAsc")]
            Price_Low_To_High,
            [Description("OdometerDesc")]
            Kilometres_High_To_Low,
            [Description("OdometerAsc")]
            Kilometres_Low_To_High,
            [Description("YearDesc")]
            Year_New_To_Old,
            [Description("YearAsc")]
            Year_Old_To_New,
            [Description("DistanceAsc")]
            Location_Nearest
        }

        public enum Display
        {
            [Description("15")]
            _15,
            [Description("25")]
            _25,
            [Description("50")]
            _50,
            [Description("100")]
            _100
        }

        public enum Listing
        {
            [Description(".title-with-trim")]
            Title,
            [Description("span.price-amount")]
            Price,
            [Description(".odometer-proximity")]
            Mileage,
            [Description("div[class*='organic-qa'] span[class='proximity-text']")]  //Organic ads only
            Proximity,
            [Description(".svg_privateBadge")]
            PrivateBadge,
            [Description(".cpo")]
            CpoBadge,
            [Description("div[id*='newCarBadge']")]
            NewBadge,
            [Description(".photo-area img[src*='tdr']")]
            PhotoVisible,
            [Description(".photo-area img[src*='blank']")]
            PhotoNotVisible,
            [Description(".badges-div .at-badge.finance-badge")]
            EasyFinanceApprovalBadge,
            [Description(".badges-div")]
            ListingsPill,
            [Description("span.at-badge")]
            SrpListingPills
        }

        public enum CommonLocators
        {
            [Description("#sortBy")]
            SortBy,
            [Description("#pageErrorWarning")]
            PageErrorWarning,
            [Description("#searchExpansionWarning")]
            NoSearchResultWarning,
            [Description("#locationAddress")]
            LocationField,
            [Description("#newSearch")]
            ClearAll,
            [Description("#filterByHomeDelivery")]
            HomeDeliveryToggle,
            [Description("#pageSize")]
            Display,
            [Description("#mastheadStickyAchorPriority")]
            PriorityListingsHeading,
            [Description("#mastheadStickyAchorOrganic")]
            AllListingsHeading,
            [Description("div.priority-qa")]
            PriorityListingsLinksLarge,
            [Description("div.organic-qa")]
            AllListingsLinksLarge,
            [Description("div.listing-details:not([class$='listing-details organic']) a[data-ad-sort-point]")]
            PriorityListingsLinks,
            [Description("div[class$='listing-details organic'] a[data-ad-sort-point]")]
            AllListingsLinks,
            [Description("div[class*='badges-'] > span[class*='finance-']")]
            AllEasyFinancialBadges,
            [Description("[class*='priority-qa']")]
            AllPriorityListingAds,
            [Description("[class*='organic-qa']")]
            AllOrganicListingAds,
            [Description(".priority-qa .result-item-inner")]  //Issue: Currently no differentiation for class names between priority and extensions ads. Created dev task: https://trader.atlassian.net/browse/CONS-2955
            PriorityListings,
            [Description(".re-layout-wrapper > a.inner-link")] 
            PVListingLink,
            [Description("[class='inner-link']")]
            ListingTitle,
            [Description(".organic-qa .result-item-inner")]
            AllOrganicListings,
            [Description(".photo-area .photo-image")]
            MainImage,
            [Description(".thumbnail-list-dt-ts>:first-child img")]
            StripeImage,

            #region DealerInventoryPage
            [Description(".message-button button")]
            DipEmailButton,
            [Description("#contactForm")]
            DipLeadContainer,
            [Description("#messagePopup")]
            DipLeadModal,
            [Description("#contactForm #contact_name")]
            DipLeadNameField,
            [Description("#contactForm #contact_email")]
            DipLeadEmailField,
            [Description("#contactForm #contact_phone")]
            DipLeadPhoneField,
            [Description("#contactForm #contact_message")]
            DipLeadMessageField,
            [Description("#contactForm button")]
            DipLeadSendMessageBtn,
            [Description(".successMessage")]
            DipLeadFeedbackMsg,
            [Description(".successMessage button")]
            DipLeadFeedbackOKBtn,
            #endregion

            [Description(".toaster .message")]
            ToasterMsg,
            [Description("#priceDropModal .price-modal-body")]
            SuccessSaveSearchModal,
            [Description("#priceDropModal .close-button")]
            SuccessSaveSearchModalCloseBtn,
            [Description(".home-delivery-info-tooltip-parent")]
            HomeDeliveryToggleInfoIcon,
            [Description(".home-delivery-toggle-tooltip")]
            HomeDeliveryToggleTooltip,
            [Description(".home-delivery-info-tooltip")]
            HomeDeliveryToggleInfoTooltip,
            [Description(".proximity>span:last-child")]
            AllListingProximity
        }

        public enum LargeLocators
        {
            [DescriptionOldSRP("#sbCount"), DescriptionSRP("#titleCount")]
            FoundTotal,
            [Description("#titleText")]
            Title,
            [Description("#SearchListings div.result-item-inner")]
            FirstListing,
            [DescriptionOldSRP("div[class*='organic-qa'] a.result-title"), Description("div[class*='organic-qa'] span.title-with-trim")]
            OrganicListingLink,
            [Description("#locationAddress")]
            FacetValueLocatorBikes,
            [Description(".image-captions .spin.click")]
            Spin360Icon,
            [Description("#listingsWrapperTop #txtSaveSearchEmail_top")]
            SaveSearchEmail,
            [Description("#listingsWrapperTop #btnSaveSearchDesktop_top")]
            SaveSearchBtn,
            [Description("#listingsWrapperTop #aSaveSearchBtn")]
            SaveSerachTurnOffBtn,
            [Description("[class*='proximity-text']")]
            Proximity,
            [Description("#dvSaveSearchContainer_top #saveSearchBtn")]
            UnSubscribeSaveSearchLinkTop,
            [Description("#dvSaveSearchContainer_top #saveSearchBtn")]
            UnSubscribeSaveSearchLinkBottom,
            [Description("#dvSearchInputModal")]
            SubscriptionSecondaryModal,
            [Description("#dvSearchInputModal .modal-close-icon")]
            SubscriptionSecondaryModalCloseBtn,
            [Description("#zeroResultsWarning")]
            ZeroListingWarning,
            [Description("#searchExpansionWarning")]
            SearchExpansionWarning,

        }

        public enum XSLocators
        {
            [Description("#rfTitle .result-count")]
            FoundTotal,
            [Description("#rfTitle")]
            Title,
            [Description("#aFilterBtn")]
            Filter,
            [Description("#aFilterBtn.searchMode"), ViewResultsSpan("#filterCount")]
            ViewResultsBtn,
            [Description(".organic-qa:not([class*='top-listing']) .list-title")]
            OrganicListingTitle,
            [Description(".list-price")]
            ListPrice,
            [Description("div[class*='organic-qa'] .odometer")]  //Changing CSS to include only organic listed mileage
            Mileage,
            [Description(".organic-qa:not([class*='top-listing']) .proximity")]
            AllOrganicProximity,
            [Description("div[class*='organic-qa'] > a")]  //Changing CSS to include only organic listed url
            VdpUrl,
            [Description(".new-car-label")]
            NewCarLabel,
            [Description("#SearchListings a.row.no-gutter.list-item")]
            FirstListing,
            [Description("span[class*='svg_PriceAnalysisBadges price-badge-svg private']")]
            PrivateBadge,
            [Description(".organic-qa:not([class*='top-listing']) a.list-item")]
            OrganicListingLink,
            [Description("#SearchListings h3.list-title")]
            ListingTitle,
            [Description("div[class*='list-container  priority-qa']")]
            PriorityListings,
            [Description(".organic-qa:not([class*='top-listing'])")]
            OrganicListings,
            [Description("div[class*='list-container  priority-qa'] span[class*='price-badge-svg new']"), DescriptionFrench("div[class*='list-container  priority-qa'] span[class*='price-badge-svg new']")]
            NewCarLabelPriorityListings,
            [Description("div[class*='list-container  organic-qa'] span[class*='price-badge-svg new']"), DescriptionFrench("div[class*='list-container  organic-qa'] span[class*='price-badge-svg new']")]
            NewCarLabelOrganicListings,
            [Description(".enhanced-listing.list-container")]
            EnhancedListings,
            [Description(".list-image-wrapper >.photo-image")]
            MainImage,
            [Description(".organic-qa")]
            OrganicAndTopAdListings,
            [Description(".list-container .list-item")]
            PVListingLink,
            [Description(".proximity")]
            ProximityText,
            [Description("#SearchListings .section-header-container")]
            ListingsHeaders,
            [Description("div[class*='list-container  priority-qa'] > a")]
            PriorityListingsLinks,
            [Description("div[class*='list-container  organic-qa'] > a")]
            AllListingsLinks,
            [Description("#dvSearchBtnContainer #iconSaveSearch")]
            SaveSearchUnsubscribeIcon,
            [Description("#dvSearchBtnContainer .icon-save-search-unsubscribed")]
            SaveSearchSubscribeIcon,
            [Description(".mobile-header-container #zeroResultsWarning")]
            ZeroListingWarning,
            [Description(".mobile-header-container #searchExpansionWarning")]
            SearchExpansionWarning,


            [Description("#dvSearchBtnContainer #saveSearchBtn")]
            SaveSearchBtn,
            [Description("#dvSearchBtnContainer #aSaveSearchBtn")]
            SaveSearchToggle, 
            [Description(".save-search-toggle.status-subscribed")]
            SubsribedSavedSearchIcon,

            [Description("#dvSearchInputModal")]
            SubscribeSaveSearchModal,
            [Description("#dvSearchInputModal #txtSaveSearchEmail")]
            SaveSearchText,
            [Description("#dvSearchInputModal #btnSubscribe")]
            SubscribeBtn,


            [Description(".home-delivery-toggle-tooltip-mobile")]
            HomeDeliveryToggleTooltip,
            [Description(".home-delivery-info-tooltip-parent-mobile")]
            HomeDeliveryToggleInfoIcon,
            [Description(".home-delivery-info-tooltip-mobile")]
            HomeDeliveryToggleInfoTooltip,
            [Description("div.text-right.badges-block")]
            ListingPills

        }

        public enum SmallLocators
        {
            [Description("#viewResultsBtn"), ViewResultsSpan("#filterCount")]
            ViewResultsBtn,
            [Description("#titleCount")]
            FoundTotal,
            [Description("#sbCount")]
            FoundTotalFilter,
            [Description("#titleText")]
            Title,
            [Description("#filterBtn")]
            Filter,
            [Description("#facetedBoxWrapper.showOnSmall")]
            FilterDiv,
            [Description(".srp-filter-heading .close-cross")]
            CloseFilter,
            [Description("#SearchListings div.result-item-inner")]
            FirstListing,
            [DescriptionOldSRP("div[class*='organic-qa'] a.result-title"), Description("div[class*='organic-qa'] span.title-with-trim")]
            OrganicListingLink,
            [Description(".listing-details .result-title")]
            ListingTitle,
            [Description("#close")]
            CloseFeedbackPopUp,
            [Description("div.container")]
            FeedbackPopUp,
            [Description("#locationAddress")]
            FacetValueLocatorBikes,
            [Description("[class*='proximity-text']")]
            Proximity,
            [Description(".save-search-option .save-search-btn")]
            SaveSearchBtn,
            [Description(".save-search-option .search-saved-btn")]
            SavedSearchBtn,
            [Description(".search-saved.save-search-toggle")]
            SaveSearchToggle,
            [Description("#dvSearchInputModal")]
            SubscribeSaveSearchModal,
            [Description("#dvSearchInputModal #txtSaveSearchEmailModal")]
            SaveSearchText,
            [Description("#dvSearchInputModal #btnSubscribeModal")]
            SubscribeBtn,
            [Description("#zeroResultsWarning")]
            ZeroListingWarning,
            [Description("#searchExpansionWarning")]
            SearchExpansionWarning,
        }

        public enum SEOLinks
        {
            [Description("#localLinks a")]
            LocalSRPLink,
            [Description("#srpSeoContainer")]
            SEOWidget,
            [Description("#seoIntroExpandAll")]
            ExpandAllLink,
            [Description("#seoHideButtonTop")]
            SEOHideButtonTop,
            [Description("#seoIntroDesc a")]
            ResearchLink,
            [Description(".article-content a.article-title")]
            ArticleTitles,
            [Description("#seoIntroDesc")]
            IntroDesc,
            [Description(".seo-intro-text-title")]
            IntroTitleXS,
            [Description("#srpSeoYearsList a")]
            SeoYearsLinks,
            [Description("#seoMoreYearsLg")]
            ShowMoreYearsLink,
            [Description("#seoMoreYearsXs")]
            ShowMoreYearsXSLink,
            [Description("#seoYouMayLike .make-model")]
            MakeModelnames,
            [Description("#seoYouMayLike .view-inventory-title")]
            ViewInventoryLinks,
            [Description("#seoYouMayLike .youMayLike-compare")]
            CompareLinks,
            [Description("#seoDiscoverModels .tabs label")]
            DiscoverModelTabs,
            [Description("#seo-tab-Coupe  a.view-inventory-title")]
            CoupeViewInventoryLinks,
            [Description("#seo-tab-Coupe .make-model")]
            CoupeModels,
            [Description("#seo-tab-Sedan  a.view-inventory-title")]
            SedanViewInventoryLinks,
            [Description("#seo-tab-Sedan .make-model")]
            SedanModels,
            [Description("#seo-tab-Hatchback a.discoverModels-learnMore")]
            HatchbackLearnMoreLinks,
            [Description("#seo-tab-Hatchback .make-model")]
            HatchbackModels,
            [Description("#seo-tab-SUV a.discoverModels-learnMore")]
            SUVLearnMoreLinks,
            [Description("#seo-tab-SUV .make-model")]
            SUVModels,
            [Description("#seo-tab-SUV .seo-make-show-more")]
            ShowMoreSUVsLink,

            [Description("#seoDiscoverPopular .seo-discover-popular-mm")]
            GeneralSRPMMName,
            [Description("#seoDiscoverPopular .seo-view-inventory-title")]
            GeneralSRPViewInventoryLink,
            [Description("#seoDiscoverPopular .seo-discover-popular-learn-more")]
            GeneralSRPLearnMoreLink,
            [Description(".seo-editorial-view-all a")]
            EditorialSeeAllLink,
            [Description("#seoDiscoverPopular .seo-discover-popular-mm .make-model")]
            DiscoverModelNames,
            [Description("#seoCompareModelBodytype a")]
            ComparePopularCars,
        }
        public enum SurveyCampaignModal
        {
            [Description("iframe[title='Usabilla Feedback Form']")]
            SurveyIframe,
            [Description("#close")]
            CloseBtn,
            [Description(".container #poll")]
            SurveyForm
        }

        public enum PopularCarsWidget
        {
            [Description("h5.mvp-title")]
            PopularCarWidgetTitle,
            [Description("#mvp-links a")]
            PopularCarWidgetModelLinks,
        }
    }

    internal class DescriptionFrenchAttribute : Attribute
    {
        public string DescriptionFrench { get; set; }

        public DescriptionFrenchAttribute(string value)
        {
            DescriptionFrench = value;
        }
    }

    internal class DescriptionOldSRPAttribute : Attribute
    {
        public string DescriptionOldSRP { get; set; }

        public DescriptionOldSRPAttribute(string value)
        {
            DescriptionOldSRP = value;
        }
    }

    internal class DescriptionSRPAttribute : Attribute
    {
        public string DescriptionSRP { get; set; }

        public DescriptionSRPAttribute(string value)
        {
            DescriptionSRP = value;
        }
    }

    internal class FacetLocatorAttribute : Attribute
    {
        public string FacetLocator { get; set; }

        public FacetLocatorAttribute(string value)
        {
            FacetLocator = value;
        }
    }

    internal class ApplyButtonSelectorAttribute : Attribute
    {
        public string ApplyButtonSelector { get; set; }

        public ApplyButtonSelectorAttribute(string value)
        {
            ApplyButtonSelector = value;
        }
    }

    internal class FacetValueLocatorAttribute : Attribute
    {
        public string FacetValueLocator { get; set; }

        public FacetValueLocatorAttribute(string value)
        {
            FacetValueLocator = value;
        }
    }

    internal class FacetLabelAttribute : Attribute
    {
        public string FacetLabel { get; set; }

        public FacetLabelAttribute(string value)
        {
            FacetLabel = value;
        }
    }

    internal class ClearButtonLocatorAttribute : Attribute
    {
        public string ClearButtonLocator { get; set; }

        public ClearButtonLocatorAttribute(string value)
        {
            ClearButtonLocator = value;
        }
    }

    internal class ClearButtonLocatorSXSAttribute : Attribute
    {
        public string ClearButtonLocatorSXS { get; set; }

        public ClearButtonLocatorSXSAttribute(string value)
        {
            ClearButtonLocatorSXS = value;
        }
    }

    internal class CSPLabelLocatorAttribute : Attribute
    {
        public string CSPLabelLocator { get; set; }

        public CSPLabelLocatorAttribute(string value)
        {
            CSPLabelLocator = value;
        }
    }

    internal class FacetListAttribute : Attribute
    {
        public string FacetList { get; set; }

        public FacetListAttribute(string value)
        {
            FacetList = value;
        }
    }

    internal class DropdownLocatorAttribute : Attribute
    {
        public string DropdownLocator { get; set; }

        public DropdownLocatorAttribute(string value)
        {
            DropdownLocator = value;
        }
    }

    internal class DropdownFacetRowLocatorAttribute : Attribute
    {
        public string DropdownFacetRowLocator { get; set; }

        public DropdownFacetRowLocatorAttribute(string value)
        {
            DropdownFacetRowLocator = value;
        }
    }

    internal class FacetRowLocatorAttribute : Attribute
    {
        public string FacetRowLocator { get; set; }

        public FacetRowLocatorAttribute(string value)
        {
            FacetRowLocator = value;
        }
    }

    internal class SelectedFacetSpanAttribute : Attribute
    {
        public string SelectedFacetSpan { get; set; }

        public SelectedFacetSpanAttribute(string value)
        {
            SelectedFacetSpan = value;
        }
    }

    internal class CloseButtonLocatorAttribute : Attribute
    {
        public string CloseButtonLocator { get; set; }

        public CloseButtonLocatorAttribute(string value)
        {
            CloseButtonLocator = value;
        }
    }

    internal class BackButtonLocatorAttribute : Attribute
    {
        public string BackButtonLocator { get; set; }

        public BackButtonLocatorAttribute(string value)
        {
            BackButtonLocator = value;
        }
    }

    internal class SellerTypeOSPAttribute : Attribute
    {
        public string SellerTypeOSP { get; set; }

        public SellerTypeOSPAttribute(string value)
        {
            SellerTypeOSP = value;
        }
    }

    internal class ViewResultsSpanAttribute : Attribute
    {
        public string ViewResultsSpan { get; set; }

        public ViewResultsSpanAttribute(string value)
        {
            ViewResultsSpan = value;
        }
    }

    internal class SearchTextLocatorAttribute : Attribute
    {
        public string SearchTextLocator { get; set; }

        public SearchTextLocatorAttribute(string value)
        {
            SearchTextLocator = value;
        }
    }

    internal class MinValueFieldAttribute : Attribute
    {
        public string MinValueField { get; set; }

        public MinValueFieldAttribute(string value)
        {
            MinValueField = value;
        }
    }

    internal class MaxValueFieldAttribute : Attribute
    {
        public string MaxValueField { get; set; }

        public MaxValueFieldAttribute(string value)
        {
            MaxValueField = value;
        }
    }

    internal class MinDropdownLocatorAttribute : Attribute
    {
        public string MinDropdownLocator { get; set; }

        public MinDropdownLocatorAttribute(string value)
        {
            MinDropdownLocator = value;
        }
    }

    internal class MaxDropdownLocatorAttribute : Attribute
    {
        public string MaxDropdownLocator { get; set; }

        public MaxDropdownLocatorAttribute(string value)
        {
            MaxDropdownLocator = value;
        }
    }

    internal class MinDropdownOldSrpLocatorAttribute : Attribute
    {
        public string MinDropdownOldSrpLocator { get; set; }

        public MinDropdownOldSrpLocatorAttribute(string value)
        {
            MinDropdownOldSrpLocator = value;
        }
    }

    internal class MaxDropdownOldSrpLocatorAttribute : Attribute
    {
        public string MaxDropdownOldSrpLocator { get; set; }

        public MaxDropdownOldSrpLocatorAttribute(string value)
        {
            MaxDropdownOldSrpLocator = value;
        }
    }

    internal class MinDropdownFacetRowLocatorAttribute : Attribute
    {
        public string MinDropdownFacetRowLocator { get; set; }

        public MinDropdownFacetRowLocatorAttribute(string value)
        {
            MinDropdownFacetRowLocator = value;
        }
    }

    internal class MaxDropdownFacetRowLocatorAttribute : Attribute
    {
        public string MaxDropdownFacetRowLocator { get; set; }

        public MaxDropdownFacetRowLocatorAttribute(string value)
        {
            MaxDropdownFacetRowLocator = value;
        }
    }

    internal class PriceTabNewSRPAttribute : Attribute
    {
        public string PriceTabNewSRP { get; set; }

        public PriceTabNewSRPAttribute(string value)
        {
            PriceTabNewSRP = value;
        }
    }

    internal class PriceTabOldSRPAttribute : Attribute
    {
        public string PriceTabOldSRP { get; set; }

        public PriceTabOldSRPAttribute(string value)
        {
            PriceTabOldSRP = value;
        }
    }

    internal class PaymentsTabNewSRPAttribute : Attribute
    {
        public string PaymentsTabNewSRP { get; set; }

        public PaymentsTabNewSRPAttribute(string value)
        {
            PaymentsTabNewSRP = value;
        }
    }

    internal class PaymentsTabOldSRPAttribute : Attribute
    {
        public string PaymentsTabOldSRP { get; set; }

        public PaymentsTabOldSRPAttribute(string value)
        {
            PaymentsTabOldSRP = value;
        }
    }

    internal class PriceTabRowLocatorAttribute : Attribute
    {
        public string PriceTabRowLocator { get; set; }

        public PriceTabRowLocatorAttribute(string value)
        {
            PriceTabRowLocator = value;
        }
    }

    internal class PaymentsTabRowLocatorAttribute : Attribute
    {
        public string PaymentsTabRowLocator { get; set; }

        public PaymentsTabRowLocatorAttribute(string value)
        {
            PaymentsTabRowLocator = value;
        }
    }

    internal class MinPaymentFieldAttribute : Attribute
    {
        public string MinPaymentField { get; set; }

        public MinPaymentFieldAttribute(string value)
        {
            MinPaymentField = value;
        }
    }

    internal class MaxPaymentFieldAttribute : Attribute
    {
        public string MaxPaymentField { get; set; }

        public MaxPaymentFieldAttribute(string value)
        {
            MaxPaymentField = value;
        }
    }

    internal class PaymentFrequencyDropdownAttribute : Attribute
    {
        public string PaymentFrequencyDropdown { get; set; }

        public PaymentFrequencyDropdownAttribute(string value)
        {
            PaymentFrequencyDropdown = value;
        }
    }

    internal class TermDropdownAttribute : Attribute
    {
        public string TermDropdown { get; set; }

        public TermDropdownAttribute(string value)
        {
            TermDropdown = value;
        }
    }

    internal class DownPaymentFieldAttribute : Attribute
    {
        public string DownPaymentField { get; set; }

        public DownPaymentFieldAttribute(string value)
        {
            DownPaymentField = value;
        }
    }

    internal class TradeInValueFieldAttribute : Attribute
    {
        public string TradeInValueField { get; set; }

        public TradeInValueFieldAttribute(string value)
        {
            TradeInValueField = value;
        }
    }

    internal class ConditionLabelAttribute : Attribute
    {
        public string ConditionLabel { get; set; }

        public ConditionLabelAttribute(string value)
        {
            ConditionLabel = value;
        }
    }

    internal class SellerTypeLabelAttribute : Attribute
    {
        public string SellerTypeLabel { get; set; }

        public SellerTypeLabelAttribute(string value)
        {
            SellerTypeLabel = value;
        }
    }
}