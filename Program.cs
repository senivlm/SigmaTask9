using SigmaTask9.ForStorage;
using SigmaTask9.Products;
using System;
using System.IO;

namespace SigmaTask9
{
    class Program
    {
        static void Main(string[] args)
        {

            string pathRead = @"C:\Users\Acer\OneDrive\Робочий стіл\C#\SigmaTask9\StorageInfo.txt";

            Storage stor1 = new Storage();

            //визначення подій----------------------
            stor1.OnShowStorage += ForStorageEvents.CheckSpoiledProducts;
            stor1.OnIncorrectInput += ForStorageEvents.CheckWhatToDo;

            stor1.ReadFromFile(pathRead);

            stor1.ReadProductsFromConsole();
            
            Console.WriteLine(stor1);

            Console.WriteLine("\n\nWrite to end");
            Console.ReadLine();
        }
    }
}
