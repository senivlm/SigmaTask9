using SigmaTask9.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace SigmaTask9.ForStorage
{
    static class FindProducts
    {
        public static Product FindProductByName(Storage storage,string name)
        {
            Product result = new Product();
            result.NameOfProduct = "Not Found";
            bool found = false; 
            for(int i =0; i<storage.Products.Count; i++ )
            {
                if(storage[i].NameOfProduct.ToLower().Equals(name))
                {
                    result = storage[i];
                    found = true;
                    break;
                }
            }
            if(!found)
            {
                Console.WriteLine("Product not found");
            }
            return result;
        }
        public static Product FindProductByPrice(Storage storage, double price)
        {
            Product result = new Product();
            result.NameOfProduct = "Not Found";
            bool found = false;
            for (int i = 0; i < storage.Products.Count; i++)
            {
                if (storage[i].PriceOfProduct.Equals(price))
                {
                    result = storage[i];
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                Console.WriteLine("Product not found");
            }
            return result;
        }
        public static Product FindProductByWeight(Storage storage, double weight)
        {
            Product result = new Product();
            result.NameOfProduct = "Not Found";
            bool found = false;
            for (int i = 0; i < storage.Products.Count; i++)
            {
                if (storage[i].WeightOfProduct.Equals(weight))
                {
                    result = storage[i];
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                Console.WriteLine("Product not found");
            }
            return result;
        }
        public static Product FindProductByCreationDate(Storage storage, DateTime creationDay)
        {
            Product result = new Product();
            result.NameOfProduct = "Not Found";
            bool found = false;
            for (int i = 0; i < storage.Products.Count; i++)
            {
                if (storage[i].CreationDay.Equals(creationDay))
                {
                    result = storage[i];
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                Console.WriteLine("Product not found");
            }
            return result;
        }
    }
}
