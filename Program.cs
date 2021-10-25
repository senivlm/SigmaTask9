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
            //test=================================
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
            //=======================================

            string pathRead = @"C:\Users\Acer\OneDrive\Робочий стіл\C#\SigmaTask9\StorageInfo.txt";

            Storage stor1 = new Storage();
            stor1.OnShowStorage += CheckSpoiledProducts;
            stor1.OnIncorrectInput += CheckWhatToDo;


            stor1.ReadFromFile(pathRead);
            


            Console.WriteLine(stor1);


            Console.WriteLine("\n\nWrite to end");
            Console.ReadLine();
        }
        //для події виводу
        //перевіряємо спорчені продукти
        public static void CheckSpoiledProducts(Storage storage,string pathToLog)
        {
            storage.CheckExpirationDate(pathToLog);
        }
        //фукція для події при неправильному вводі
        public static void CheckWhatToDo(Storage storage, string pathToLog)
        {
            //копіюємо неправильно введені дані
            Product copy = storage[storage.Products.Count - 1];
            //видаляємо неправильно введені дані
            storage.Products.RemoveAt(storage.Products.Count-1);

            //вибираємо, що робити
            int attempts = 3;
            int action = 2;
            //поки не отримаємо вказівку, що роботи, або поки спроби не кінчаться
            while(attempts>0)
            {
                Console.WriteLine(string.Format("Choose action:\t1 = Write to Log File\t 2 = Enter new Product"));
                Console.WriteLine(string.Format("You have {0} attempts", attempts));
                string input = Console.ReadLine();
                if (!Int32.TryParse(input, out action) || (action > 2) || (action < 1))
                {
                    attempts--;
                    continue;
                }
                else
                    break;
            }
            if (action == 2)
            {
                CorrectInput(storage);
            }
        }
        //правильні ввід через консоль
        public static void CorrectInput(Storage storage)
        {

            //буде 3 спроби
            int attempts = 3;
            while(attempts >0)
            {
                string input;
                Console.WriteLine("Enter correct data, you have {0} attempts", attempts);

                int typeOfClass;
                Console.WriteLine("Choose type: 1 = Product\t2 = Meat");
                input = Console.ReadLine();
                if(!Int32.TryParse(input,out typeOfClass)||(typeOfClass<1)||(typeOfClass>2))
                {
                    Console.WriteLine("Wrong input");
                    attempts--;
                    continue;
                }
                if(typeOfClass ==1)
                {
                    try
                    {
                        //якщо успішно зчитаємо, то кінець
                        storage.Add(storage.ReadProduct());
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("Wrong input");
                        attempts--;
                        continue;
                    }
                }  
                else if(typeOfClass ==2)
                {
                    try
                    {
                        //якщо успішно зчитаємо, то кінець
                        storage.Add(storage.ReadMeat());
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("Wrong input");
                        attempts--;
                        continue;
                    }
                }
            }
            if(attempts == 0)
            {
                Console.WriteLine("You have no more attempts");
                Console.WriteLine("We will add element Soda");
                storage.Add(new Product(new DateTime(2021,10,4), 2.3, 5, "Soda", 30));
            }
        }
        public static void WriteProductToLogFile(Product product, string pathToLogFile)
        {
            using (StreamWriter writer = new StreamWriter(pathToLogFile,append:true))
            {
                writer.WriteLine(product);
                writer.WriteLine("Removed at Date: {0},\t Time: {1}\n", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
            }
        }
    }
}
