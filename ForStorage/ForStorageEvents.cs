using SigmaTask9.Products;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SigmaTask9.ForStorage
{
    static class ForStorageEvents
    {
        //для події при виводі==================
        //перевірить спорчені продукти
        public static void CheckSpoiledProducts(Storage storage, string pathToLog)
        {
            storage.CheckExpirationDate(pathToLog);
        }

        //фукція для події при неправильному вводі===============
        //опрацює неправильні дані і продовжить роботу при зчитуванні інформації
        public static void CheckWhatToDo(Storage storage, string pathToLog, string message)
        {
            //копіюємо неправильно введені дані
            Product copy = storage[storage.Products.Count - 1];
            //видаляємо неправильно введені дані
            storage.Products.RemoveAt(storage.Products.Count - 1);

            Console.WriteLine("\nError : {0}", message);

            //вибираємо, що робити
            int attempts = 3;
            int action = 2;
            //поки не отримаємо вказівку, що роботи, або поки спроби не кінчаться
            while (attempts > 0)
            {
                Console.WriteLine(string.Format("Choose action:\t1 = Write to Log File\t 2 = Enter new Product"));
                Console.WriteLine(string.Format("You have {0} attempts", attempts));
                string input = Console.ReadLine();
                if (!Int32.TryParse(input, out action) || (action > 2) || (action < 1))
                {
                    attempts--;
                    continue;
                }
                //ввели правильно, значить вийти
                else
                    break;
            }
            //якщо нема спроб, то просто записати у лог файл
            if (attempts == 0)
            {
                Console.WriteLine("You have no more attempts");
                Console.WriteLine("We will write incorrect product to log file\n");
                WriteProductToLogFile(copy, pathToLog, message);
            }
            else if (action == 1)
            {
                Console.WriteLine("We will write product to log file\n");
                WriteProductToLogFile(copy, pathToLog, message);
            }
            //виправити помилку 
            else
            {
                CorrectInput(storage);
            }
        }

        //функція для правильного вводу через консоль-----------------------------
        public static void CorrectInput(Storage storage)
        {
            //буде 3 спроби
            int attempts = 3;
            while (attempts > 0)
            {
                string input;
                Console.WriteLine("\nEnter correct data, you have {0} attempts", attempts);

                //проси о ввести тип класу
                int typeOfClass;
                Console.WriteLine("Choose type: 1 = Product\t2 = Meat\t3 = Dairy");
                input = Console.ReadLine();
                if (!Int32.TryParse(input, out typeOfClass) || (typeOfClass < 1) || (typeOfClass > 2))
                {
                    Console.WriteLine("Wrong input");
                    attempts--;
                    continue;
                }
                if (typeOfClass == 1)
                {
                    try
                    {
                        //якщо успішно зчитаємо, то кінець
                        storage.Add(new Product());
                        storage[storage.Products.Count - 1].ReadFromConsole();
                        break;
                    }
                    catch (Exception ex)
                    {
                        //інакше видаляємо заготовку під об'єкт класу, зменшуємо спроби
                        storage.Products.RemoveAt(storage.Products.Count - 1);
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("Wrong input");
                        attempts--;
                        continue;
                    }
                }
                else if (typeOfClass == 2)
                {
                    try
                    {
                        //якщо успішно зчитаємо, то кінець
                        storage.Add(new Meat());
                        storage[storage.Products.Count - 1].ReadFromConsole();
                        break;
                    }
                    catch (Exception ex)
                    {
                        storage.Products.RemoveAt(storage.Products.Count - 1);
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("Wrong input");
                        attempts--;
                        continue;
                    }
                }
                else
                {
                    try
                    {
                        //якщо успішно зчитаємо, то кінець
                        storage.Add(new DairyProduct());
                        storage[storage.Products.Count - 1].ReadFromConsole();
                        break;
                    }
                    catch (Exception ex)
                    {
                        storage.Products.RemoveAt(storage.Products.Count - 1);
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("Wrong input");
                        attempts--;
                        continue;
                    }
                }
            }
            if (attempts == 0)
            {
                Console.WriteLine("You have no more attempts");
                Console.WriteLine("We will add element Soda");
                storage.Add(new Product(new DateTime(2021, 10, 4), 2.3, 5, "Soda", 30));
            }
        }

        //функця записати у лог файл-------------------------------------
        public static void WriteProductToLogFile(Product product, string pathToLogFile, string message)
        {
            using (StreamWriter writer = new StreamWriter(pathToLogFile, append: true))
            {
                writer.WriteLine("Error --> {0}", message);
                writer.WriteLine(product);
                writer.WriteLine("Time: [{0},{1}]\n", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString());
            }
        }
    }
}
