using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services
{
    public class Formulas
    {
        public static double StandardPriceFormula(double supplierPrice, double exchangeRate)
        {
            return Math.Ceiling(((supplierPrice / 120 * 112 * 1.1) + (supplierPrice / 120 * 112 * 1.1 * 0.5)) * exchangeRate);
        }

        public static double StandardPriceLimitFormula(double supplierPrice, double exchangeRate)
        {
            return Math.Ceiling(((supplierPrice / 120 * 112 * 1.1) + (supplierPrice / 120 * 112 * 1.1 * 0.25)) * exchangeRate);
        }
    }
}
