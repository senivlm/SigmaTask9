using SigmaTask9.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SigmaTask9.ForStorage
{
    static class StorageFunctions
    {
        //знайти подібні продукти у обох сховищах------------------------------------
        public static List<Product> GetSimilarProducts(Storage st1, Storage st2)
        {
            if (st1 == null || st2 == null)
                throw new ArgumentNullException("Storage is null");
            //знаходимо однакові імена через Where, де прописуємо лямба функцію
            //продукти мають бути однакові, тому можна використати функцію Contains
            //під кнець перетвоюємо масив у список
            return st1.Products.Where((prodFromSt1) => st2.Products.Contains(prodFromSt1)).ToList();
        }
        //всі різні продукти з двох сховищ--------------------
        public static List<Product> GetAllUniqueProducts(Storage st1, Storage st2)
        {
            if (st1 == null || st2 == null)
                throw new ArgumentNullException("Storage is null");

            //отримуємо всі унікальні тільи з першого 
            List<Product> allUnique = GetUniqueProductsInFirstStore(st1,st2);
            //додаємо всі унікальні тільки з другого
            allUnique.AddRange(GetUniqueProductsInSecondStore(st1,st2));
            return allUnique;
        }
        //Знайти всі товари, які є в I складі,яких немає в II складі-----------------
        public static List<Product> GetUniqueProductsInFirstStore(Storage st1, Storage st2)
        {
            if (st1 == null || st2 == null)
                throw new ArgumentNullException("Storage is null");

            //те саме, але при оберненій дії
            return st1.Products.Where((prodFromSt1) => !(st2.Products.Contains(prodFromSt1))).ToList();
        }
        //Знайти всі товари, які є в II складі,яких немає в I складі-----------------
        public static List<Product> GetUniqueProductsInSecondStore(Storage st1, Storage st2)
        {
            if (st1 == null || st2 == null)
                throw new ArgumentNullException("Storage is null");

            //st1 i st2 поміяли місцями
            return st2.Products.Where((prodFromSt2) => !(st1.Products.Contains(prodFromSt2))).ToList();
        }

    }
}
