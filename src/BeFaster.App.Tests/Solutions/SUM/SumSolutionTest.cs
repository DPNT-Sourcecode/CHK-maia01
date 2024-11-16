using BeFaster.App.Solutions.CHK;
using BeFaster.App.Solutions.SUM;
using NUnit.Framework;

namespace BeFaster.App.Tests.Solutions.SUM
{
    [TestFixture]
    public class SumSolutionTest
    {
        [TestCase(1, 1, ExpectedResult = 2)]
        public int ComputeSum(int x, int y)
        {
            return SumSolution.Sum(x, y);
        }
    }


    [TestFixture]
    public class CheckoutSolutionTest
    {
        [TestCase("SSSZ", ExpectedResult = 65)]
        public int ComputePrice(string x)
        {
            return CheckoutSolution.ComputePrice(x);
        }

        [TestCase("X", ExpectedResult = 17)]
        public int ComputePrice2(string x)
        {
            return CheckoutSolution.ComputePrice(x);
        }

        [TestCase("Y", ExpectedResult = 20)]
        public int ComputePrice3(string x)
        {
            return CheckoutSolution.ComputePrice(x);
        }


        
    }
}
