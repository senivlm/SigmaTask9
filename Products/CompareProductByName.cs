using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SigmaTask9.Products
{
    class CompareProductByName : IComparer<Object>
    {
        public int Compare([AllowNull] Object x, [AllowNull] Object y)
        {
            Product p1 = x as Product;
            Product p2 = y as Product;
            //не враховуємо чи велика чи мала буква
            return p1.NameOfProduct.ToLower().CompareTo(p2.NameOfProduct.ToLower());
        }
    }
}
