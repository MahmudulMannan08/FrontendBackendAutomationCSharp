using Stripe;
using System.Linq;
using System.Threading;

namespace UIAutomationLibrary.Base.ClientApi3rdParty
{
    public class Stripe
    {
        public string stripe_AccountId;

        public Stripe(string stripe_SecretKey, string stripe_AccountId)
        {
            this.stripe_AccountId = stripe_AccountId;
            StripeConfiguration.ApiKey = stripe_SecretKey;
        }

        private RequestOptions requestOptions = new RequestOptions { ApiKey = StripeConfiguration.ApiKey };
        private ChargeListOptions chargeListOptions = new ChargeListOptions { Limit = 50 };

        /// <summary>
        /// Retrieve Stripe charge from charge list by matching Ad ID.
        /// </summary>
        /// <param name="adId">Unique identifier for ad</param>
        public Charge RetrieveChargeByAdId(string adId)
        {
            ChargeService chargeService = new ChargeService();
            return chargeService.List(chargeListOptions, requestOptions).FirstOrDefault(x => x.Metadata.ContainsValue($"5_{adId}"));
        }

        /// <summary>
        /// Creates a Refund service that cancels or refunds(if captured) a charge in Stripe by matching Ad ID, if charge data is available or charge is not already refunded.
        /// </summary>
        /// <param name="adId">Unique identifier for ad</param>
        public void CancelOrRefundChargeByAdId(string adId)
        {
            var charge = RetrieveChargeByAdId(adId);
            if (charge == null || charge.Refunded) { return; }

            var options = new RefundCreateOptions
            {
                Charge = charge.Id  //Retrieve and assign charge id: ch_id from charge data
            };
            RefundService refundService = new RefundService();
            refundService.Create(options, requestOptions);
            Thread.Sleep(5000);
        }

        /// <summary>
        /// Creates a Capture service that captures a charge in Stripe by matching Ad ID, if charge data is available or charge is not already captured.
        /// </summary>
        /// <param name="adId">Unique identifier for ad</param>
        public void CaptureChargeByAdId(string adId)
        {
            ChargeService chargeService = new ChargeService();
            var charge = RetrieveChargeByAdId(adId);
            if (charge == null || charge.Refunded) { return; }
            if (!charge.Captured)
            {
                chargeService.Capture(charge.Id, requestOptions: requestOptions);
                Thread.Sleep(5000);
            }
        }
    }
}
