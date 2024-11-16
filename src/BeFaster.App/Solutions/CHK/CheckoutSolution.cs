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

        };




        public static int ComputePrice(string? skus)
        {
            foreach (char sku in skus)
            {

                if (sku < 65 || sku > 68)
                    return -1;



            }
        }
    }
}


