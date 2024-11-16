using BeFaster.App.Solutions.CHK;
using BeFaster.Runner.Exceptions;
using System.Net.Http.Headers;

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




        internal class Product
        {
            public Product()
            {
                SpecialOffers = new List<SpecialOffer>();
            }

            public char SKU { get; set; }
            public decimal Price { get; set; }

            public List<SpecialOffer> SpecialOffers { get; set; }
        }

        internal class SpecialOffer
        {
            public int Quantity { get; set; }
            public decimal Price { get; set; }

            public int Type { get; set; } = 0;

            public List<ProductOffer> ProductOffers { get; set; }

        }

        public class ProductOffer
        {
            public char SKU { get; set; }
            public int Quantity { get; set; }
        }

        static List<Product> productPrices = new List<Product>() {

            new Product{ SKU = 'A', Price = 50, SpecialOffers = new List<SpecialOffer> {

               new  SpecialOffer { Quantity = 3, Price = 130},
                 new  SpecialOffer { Quantity = 5, Price = 200},
            } },
             new Product{ SKU = 'B', Price = 30, SpecialOffers =new List<SpecialOffer> {

                new  SpecialOffer { Quantity = 2,Price = 45},
            } },
             new Product{ SKU = 'C', Price = 20, SpecialOffers =new List<SpecialOffer>{} },
             new Product{ SKU = 'D', Price = 15, SpecialOffers =new List<SpecialOffer> { } },

             new Product{ SKU = 'E', Price = 40, SpecialOffers =new List<SpecialOffer> {

                 new SpecialOffer  { Quantity = 2, Type = 1, ProductOffers = new List<ProductOffer>{

                 new ProductOffer  { SKU = 'B', Quantity =1, }
                 } }
             } },
        };




        public static int ComputePrice(string? skus)
        {

            if (String.IsNullOrWhiteSpace(skus))
                return 0;

            Dictionary<char, int> skuQtyDict = new Dictionary<char, int>();

            foreach (char sku in skus)
            {
                if (!char.IsLetter(sku))
                    continue;

                if (!productPrices.Select(x => x.SKU).ToList().Any(x => x.Equals(sku)))
                    return -1;

                if (skuQtyDict.ContainsKey(sku))
                    skuQtyDict[sku]++;
                else
                {
                    skuQtyDict.Add(sku, 1);
                }
            }

            //TODO: deduct the qty for B if we have E or any 

            List<ProductOffer> productOffers = new List<ProductOffer>();

            var specialOffers = productPrices.ToList().Where(x => x.SpecialOffers.Count > 0 && x.SpecialOffers.Any(x => x.Type == 1)).ToList();

            foreach (var products in specialOffers)
            {
                if (skuQtyDict.ContainsKey(products.SKU) && products.SpecialOffers.Any(x => x.Quantity <= skuQtyDict[products.SKU]))

                    foreach (var offer in products.SpecialOffers)
                    {
                        foreach (var productOffer in offer.ProductOffers)
                        {

                            if (skuQtyDict.ContainsKey(productOffer.SKU) && skuQtyDict[productOffer.SKU] >= productOffer.Quantity)
                                skuQtyDict[productOffer.SKU]--;
                        }
                    }


            }



            if (skuQtyDict.Keys.Count == 0)
                return -1;

            if (!skuQtyDict.Keys.Any(x => !x.Equals(productPrices.Select(x => x.SKU))))
                return -1;

            int totalprice = 0;

            foreach (var kvp in skuQtyDict.Keys)
            {
                decimal price = GetPriceOfSkuWithQty(kvp, skuQtyDict[kvp]);
                totalprice += (int)price;
            }

            return totalprice;
        }

        private static decimal GetPriceOfSkuWithQty(char sku, int quantity)
        {
            int remainingQuantity = quantity;
            decimal totalPrice = 0;

            var skuprice = productPrices.FirstOrDefault(x => x.SKU == sku);

            skuprice.SpecialOffers = skuprice.SpecialOffers.Where(x => x.Type == 0).ToList();

            if (skuprice.SpecialOffers?.Count > 0)
            {
                while (remainingQuantity > 0 && skuprice.SpecialOffers.Any(x => x.Quantity <= remainingQuantity))
                {

                    var specialOffer = skuprice.SpecialOffers?.Where(x => x.Quantity <= remainingQuantity).OrderByDescending(x => x.Quantity).FirstOrDefault();


                    if (remainingQuantity < specialOffer.Quantity)
                        continue;

                    remainingQuantity -= specialOffer.Quantity;
                    totalPrice += specialOffer.Price;


                }
            }

            if (remainingQuantity > 0)
                totalPrice += remainingQuantity * skuprice.Price;


            return totalPrice;
        }
    }


}


