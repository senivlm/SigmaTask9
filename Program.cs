using SigmaTask9.ForStorage;
using System;

namespace SigmaTask9
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                for (int i = 0; i < 10; i++)
                {
                    if (i == 4)
                        throw new Exception();
                    Console.WriteLine("i is {0}",i);
                }
            }
            catch (Exception ex) { }


            string pathRead = @"C:\Users\Acer\OneDrive\Робочий стіл\C#\SigmaTask9\StorageInfo.txt";
            string pathWrite = @"C:\Users\Acer\OneDrive\Робочий стіл\C#\SigmaTask9\RemovedProducts.txt";
            Storage stor1 = new Storage();
            stor1.ReadFromFile(pathRead);
            stor1.OnShowStorage += CheckSpoiledProducts;


            Console.WriteLine(stor1);


            Console.WriteLine("\n\nWrite to end");
            Console.ReadLine();
        }
        
        public static void CheckSpoiledProducts(Storage storage,string path)
        {
            storage.CheckExpirationDate(path);
        }
    }
}
