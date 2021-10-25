using System;

namespace SigmaTask9
{
    class Program
    {
        static void Main(string[] args)
        {
            string pathRead = @"C:\Users\Acer\OneDrive\Робочий стіл\C#\SigmaTask9\StorageInfo.txt";
            string pathWrite = @"C:\Users\Acer\OneDrive\Робочий стіл\C#\SigmaTask9\RemovedProducts.txt";
            Storage stor1 = new Storage();
            stor1.ReadFromFile(pathRead);

            stor1.CheckExpirationDate(pathWrite);


            Console.WriteLine("\n\nWrite to end");
            Console.ReadLine();
        }
    }
}
