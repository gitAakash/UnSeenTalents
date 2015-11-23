using System;
using Stripe;

namespace UnseentalentsApp.Models.Repository
{
    public static class StripPayment
    {
        public static string GetTokenId(paymentrequestfortoken model)
        {
            string token = "";
            var myToken = new StripeTokenCreateOptions
            {
                Card = new StripeCreditCardOptions
                {
                    // set these properties if passing full card details (do not
                    // set these properties if you set TokenId)
                    Number = model.cardnumber, // "4242424242424242",
                    ExpirationYear = model.expiryear, // "2022",
                    ExpirationMonth = model.expirymonth, // "10",
                    //AddressCountry = "US",                // optional
                    //AddressLine1 = "24 Beef Flank St",    // optional
                    //AddressLine2 = "Apt 24",              // optional
                    //AddressCity = "Biggie Smalls",        // optional
                    //AddressState = "NC",                  // optional
                    //AddressZip = "27617",                 // optional
                    Name = model.Name, // optional
                    // Cvc = "1223"                          // optional
                }
            };
            // if you need this...
            // set this property if using a customer (stripe connect only)
            //myToken.CustomerId = *customerId*;


            var tokenService = new StripeTokenService();
            StripeToken stripeToken = tokenService.Create(myToken);

            token = stripeToken.Id;
            return token;
        }

        public static string ChargeCustomer(paymentrequestforcharge objchargereq)
        {
            var myCharge = new StripeChargeCreateOptions();
            // always set these properties
            myCharge.Amount = ConvertToCents(objchargereq.amount);
            myCharge.Currency = "usd";

            // set this if you want to
            myCharge.Description = objchargereq.description;

            // setting up the card
            // setting up the card
            myCharge.Source = new StripeSourceOptions
            {
                TokenId = objchargereq.tokenid
            };

            var chargeService = new StripeChargeService();
            StripeCharge stripeCharge = chargeService.Create(myCharge);

            return stripeCharge.Id;
        }

        public static int ConvertToCents(string dollars)
        {
            return Convert.ToInt32(Convert.ToInt64(dollars)*100);
        }
    }
}