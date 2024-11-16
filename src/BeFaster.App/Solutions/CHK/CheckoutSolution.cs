using BeFaster.App.Solutions.CHK;
using BeFaster.Runner.Exceptions;

namespace BeFaster.App.Solutions.CHK
{
    public static class CheckoutSolution
    {

        //        Our price table and offers: 
        //+------+-------+----------------+
        //| Item | Price | Special offers |
        //+------+-------+----------------+
        //| A    | 50    | 3A for 130     |
        //| B    | 30    | 2B for 45      |
        //| C    | 20    |                |
        //| D    | 15    |                |
        //+------+-------+----------------+


        internal class SkuPrice
        {
            public SkuPrice()
            {
                SpecialOffers = new List<SpecialOffer>();
            }

            public char SKU { get; set; }
            public decimal Price { get; set; }

            public List<SpecialOffer> SpecialOffers { get; set; }
        }

        static List<SkuPrice> prices = new List<SkuPrice>() {

            new SkuPrice{ SKU = 'A', Price = 50, SpecialOffers = new List<SpecialOffer> {

               new  SpecialOffer { Quantity = 3, Price = 130},
            } },
             new SkuPrice{ SKU = 'B', Price = 30, SpecialOffers =new List<SpecialOffer> {

                new  SpecialOffer { Quantity = 2,Price = 45},
            } },
             new SkuPrice{ SKU = 'C', Price = 20, SpecialOffers =new List<SpecialOffer>{} },
             new SkuPrice{ SKU = 'D', Price = 15, SpecialOffers =new List<SpecialOffer> { } },
             new SkuPrice{ SKU = 'E', Price = 40, SpecialOffers =new List<SpecialOffer> { } },
        };




        public static int ComputePrice(string? skus)
        {

            if (String.IsNullOrWhiteSpace(skus))
                return 0;

            Dictionary<char, int> skuQty = new Dictionary<char, int>();

            foreach (char sku in skus)
            {
                if (!char.IsLetter(sku))
                    continue;

                if (!prices.Select(x => x.SKU).ToList().Any(x => x.Equals(sku)))
                    return -1;

                if (skuQty.ContainsKey(sku))
                    skuQty[sku]++;
                else
                {
                    skuQty.Add( sku, 1);
                }
            }

            if(skuQty.Keys.Count ==0)
                return -1;

            if (!skuQty.Keys.Any( x=> !x.Equals(prices.Select(x=> x.SKU))))
                return -1;

            int totalprice = 0;

            foreach (var kvp in skuQty.Keys)
            {
                decimal price = GetPriceOfSkuWithQty(kvp, skuQty[kvp]);
                totalprice += (int)price;
            }

            return totalprice;
        }

        private static decimal GetPriceOfSkuWithQty(char sku, int quantity)
        {
            int remainingQuantity = quantity;
            decimal totalPrice = 0;

            var skuprice = prices.FirstOrDefault(x => x.SKU == sku);

            if (skuprice.SpecialOffers?.Count > 0)
            {
                while (remainingQuantity > 0 && skuprice.SpecialOffers.Any(x => x.Quantity <= remainingQuantity)) { 

                    foreach (var specialOffer in skuprice.SpecialOffers?.OrderBy(x => x.Quantity))
                    {

                        if (remainingQuantity < specialOffer.Quantity)
                            break;

                        remainingQuantity -= specialOffer.Quantity;
                        totalPrice += specialOffer.Price;


                    }

                }
            }

            if (remainingQuantity != 0)
                totalPrice += remainingQuantity * skuprice.Price;


            return totalPrice;
        }
    }
}



