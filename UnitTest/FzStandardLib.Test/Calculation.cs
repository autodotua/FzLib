using NUnit.Framework;
using FzLib;
using M = System.Math;

namespace FzLib.Test
{
    public class Math
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Calculation()
        {
            Calculation c = new Calculation();

            Assert.AreEqual(c.Calc("1+2").Value, 1 + 2);
            Assert.AreEqual(c.Calc("2*3").Value, 2 * 3);
            Assert.AreEqual(c.Calc("45/65").Value, 45 * 1.0 / 65);
            Assert.AreEqual(c.Calc("123-15145").Value, 123 - 15145);
            Assert.AreEqual(c.Calc("123(3+23(7-4))").Value, 123 * (3 + 23 * (7 - 4)));
            Assert.AreEqual(c.Calc("(123-45(67+89)(11-12)(3+23(7-4").Value, (123 - 45 * (67 + 89) * (11 - 12) * (3 + 23 * (7 - 4))));
            Assert.AreEqual(c.Calc("123^(3+23(7-4))").Value, M.Pow(123, (3 + 23 * (7 - 4))));
            Assert.AreEqual(c.Calc("123^(3!+23(7-4))").Value, M.Pow(123, ((3 * 2 * 1) + 23 * (7 - 4))));
            Assert.AreEqual(c.Calc("174.2sin(12.4)").Value, 174.2 * M.Sin(12.4));
            Assert.AreEqual(c.Calc("2e4cos(1.78e2)sin(2.34e3)").Value, 2e4 * M.Cos(1.78e2) * M.Sin(2.34e3));
            Assert.AreEqual(c.Calc("2e4cos(1.78!e2)sin(2.34e3)").Value, double.NaN);
            Assert.AreEqual(c.Calc("1.78e4^2.54e2").Value, M.Pow(1.78e4, 2.54e2));
            Assert.AreEqual(c.Calc("4sin(rad(45))").Value, 4 * M.Sin(45 * M.PI / 180));
            Assert.AreEqual(c.Calc("4sin(rad(12бу34'56\"))").Value, 4 * M.Sin((12 + 34.0 / 60 + 56.0 / 3600) * M.PI / 180));
            Assert.AreEqual(c.Calc("1+2").Value, 1 + 2);
            Assert.AreEqual(c.Calc("-5*-5-5*-5*-5*-5").Value, -5 * -5 - 5 * -5 * -5 * -5);
        }
    }
}