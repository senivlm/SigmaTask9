using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SigmaTask9.Products
{
    class CompareProductByWeight: IComparer<Object>
    {
        public int Compare([AllowNull] Object x, [AllowNull] Object y)
        {
            Product p1 = x as Product;
            Product p2 = y as Product;
            return p1.WeightOfProduct.CompareTo(p2.WeightOfProduct);
        }
    }
}
