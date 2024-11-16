using BeFaster.App.Solutions.CHK;
using BeFaster.Runner.Exceptions;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

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


             new Product{ SKU = 'F', Price = 10, SpecialOffers =new List<SpecialOffer> {

                 new SpecialOffer  { Quantity = 2, Type = 1, ProductOffers = new List<ProductOffer>{

                 new ProductOffer  { SKU = 'F', Quantity =1, }
                 } }
             } },
        };




        public static int ComputePrice(string? skus)
        {

            String.Concat(skus.OrderBy(c => c));


            if (String.IsNullOrWhiteSpace(skus))
                return 0;

            Dictionary<char, int> skuQtyDict = new Dictionary<char, int>();
            Dictionary<char, int> discountDict = new Dictionary<char, int>();

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

            foreach (var product in specialOffers)
            {
                if (skuQtyDict.ContainsKey(product.SKU) && product.SpecialOffers.Any(x => x.Quantity <= skuQtyDict[product.SKU]))

                    foreach (var offer in product.SpecialOffers)
                    {
                        foreach (var productOffer in offer.ProductOffers)
                        {
                            int toDeduct = 0;


                            toDeduct = skuQtyDict[product.SKU] / offer.Quantity;


                            if (productOffer.SKU == product.SKU)
                            {
                                toDeduct = RecursiveDiscount(skuQtyDict[product.SKU], 0);

                                if (skuQtyDict.ContainsKey(productOffer.SKU) && skuQtyDict[productOffer.SKU] >= productOffer.Quantity)
                                {

                                    if (!discountDict.ContainsKey(productOffer.SKU))
                                    {
                                        discountDict.Add(productOffer.SKU, productOffer.Quantity * toDeduct);
                                    }

                                }

                            }

                            else
                            {
                                if (skuQtyDict.ContainsKey(productOffer.SKU))
                                {

                                    skuQtyDict[productOffer.SKU] = skuQtyDict[productOffer.SKU] - productOffer.Quantity * toDeduct;
                                }


                            }
                        }


                    }
            }



            if (skuQtyDict.Keys.Count == 0)
                return -1;

            if (!skuQtyDict.Keys.Any(x => !x.Equals(productPrices.Select(x => x.SKU))))
                return -1;

            int totalprice = 0;
            int totaldiscount = 0;

            foreach (var kvp in skuQtyDict.Keys)
            {
                decimal price = GetPriceOfSkuWithQty(kvp, skuQtyDict[kvp]);
                totalprice += (int)price;
            }

            foreach (var kvp in discountDict.Keys)
            {
                decimal price = GetPriceOfSkuWithQty(kvp, discountDict[kvp]);
                totaldiscount += (int)price;
            }

            return totalprice - totaldiscount;
        }

        private static decimal GetPriceOfSkuWithQty(char sku, int quantity)
        {
            int remainingQuantity = quantity;
            decimal totalPrice = 0;

            var skuprice = productPrices.FirstOrDefault(x => x.SKU == sku);

            var specialOffers = skuprice.SpecialOffers.Where(x => x.Type == 0).ToList();

            if (specialOffers.Count > 0)
            {
                while (remainingQuantity > 0 && specialOffers.Any(x => x.Quantity <= remainingQuantity))
                {

                    var specialOffer = specialOffers?.Where(x => x.Quantity <= remainingQuantity).OrderByDescending(x => x.Quantity).FirstOrDefault();


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


        private static decimal GetDiscount(char sku, int quantity)
        {
            return GetPriceOfSkuWithQty(sku, quantity);
        }


        private static int RecursiveDiscount(int total, int totalDiscountSoFar)
        {


            if (total <= 2)
                return totalDiscountSoFar;

            return RecursiveDiscount(total - 3, ++totalDiscountSoFar); // 2, 0
                                                                       // 3 => 1, 1
                                                                       // 4 => 2, 1
                                                                       // 5 => 3(1) , 1 
                                                                       // 6 => 4 (1), 1 + 1
        }
    }


}
