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

            public char SKU { get; set; }
            public decimal Price { get; set; }

            public Dictionary<int,int> SpecialOffers { get; set; }
        }

        static List<SkuPrice> prices = new List<SkuPrice>() {

            new SkuPrice{ SKU = 'A', Price = 50, SpecialOffers = new Dictionary<int, int> {

                { 3,130}
            } },
             new SkuPrice{ SKU = 'B', Price = 30, SpecialOffers = new Dictionary<int, int> {

                { 2,45},
            } },
             new SkuPrice{ SKU = 'C', Price = 20, SpecialOffers = new Dictionary<int, int> {} },
             new SkuPrice{ SKU = 'D', Price = 15, SpecialOffers = new Dictionary<int, int> {} },

        };




        public static int ComputePrice(string? skus)
        {

            if (String.IsNullOrEmpty(skus))
                return -1;

            Dictionary<char, int> skuQty = new Dictionary<char, int>();

            foreach (char sku in skus)
            {
                if (skuQty.ContainsKey(sku))
                    skuQty[sku]++;
                else
                {
                    skuQty.Add(sku, 1);
                }
            }

            int totalprice = 0;

            foreach (var kvp in skuQty.Keys)
            {
                decimal price = GetPriceOfSkuWithQty(kvp, skuQty[kvp]);
            }

            return totalprice;
        }

        private static decimal GetPriceOfSkuWithQty(char kvp, int quantity)
        {
            int remainingquantity = quantity;


            throw new NotImplementedException();
        }
    }
}





