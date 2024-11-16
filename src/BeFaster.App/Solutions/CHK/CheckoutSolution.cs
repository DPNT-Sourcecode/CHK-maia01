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


        Dictionary<string, int> priceTable = new Dictionary<string, int>();



        public static int ComputePrice(string? skus)
        {
            throw new SolutionNotImplementedException();
        }
    }
}

