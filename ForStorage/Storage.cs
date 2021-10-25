using SigmaTask9.Products;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaTask9.ForStorage
{

    class Storage
    {
        //делегат для спорчених продуктів
        public delegate void SpoiledProductsHandler(Storage storage, string pathToLog);
        //делегат для некореткних даних
        public delegate void InCorrectDataHandler(Storage storage, string pathToLog,string message);


        //подія при виводі перевіряти спорені продукти
        public event SpoiledProductsHandler OnShowStorage;
        //подія при неправильних даних
        public event InCorrectDataHandler OnIncorrectInput;
        //шлях до лог файлу
        public string PathToLogFile { get; private set; }

        List<Product> products;
        //властивість
        public List<Product> Products => products;

        //конструктор--------------------------
        public Storage()
        {
            this.products = new List<Product>();
            PathToLogFile = @"C:\Users\Acer\OneDrive\Робочий стіл\C#\SigmaTask9\LogFile.txt";
        }

        //ініціалізація через список-------------------------
        public void InitByArray(Product[] prod_arr)
        {
            this.products = new List<Product>(prod_arr);
        }

        //швидка ініціалізація------------
        public void QuickInit()
        {
            products.Add(new Product(new DateTime(2021, 3, 12), 3.21, 3.3, "Waterlemon", 30));
            products.Add(new Product(new DateTime(2021, 9, 28), 5, 1.3, "Banana", 30));
            products.Add(new Product(new DateTime(2020, 5, 2), 10, 0.5, "Nuts", 30));
            products.Add(new Meat(new DateTime(2021, 10, 20), 10, 0.5, "NA", 30, 1, 2));
            products.Add(new DairyProduct(new DateTime(2021, 10, 2), 5, 0.5, "Milk", 30));
        }

        //додати елемент-------------------------
        public void Add(Product item)
        {
            //якщо нема елемента, додати
            if(!Products.Contains(item))
            {
                this.products.Add(item);
            }
            //інаше нічого
            else
            {
                Console.WriteLine("Product {0} is already added",item.NameOfProduct);
            }
        }
        //видалити елемент через ім'я-----------
        public bool RemoveByName(string name)
        {
            //результат видалення
            //true, якщо змогло видалити
            bool result = false;
            for(int i =0; i< Products.Count;i++)
            {
                Product curProd = (Product)this.products[i];
                if(curProd.NameOfProduct.ToLower().Equals(name.ToLower()))
                {
                    this.products.RemoveAt(i);
                    result = true;
                    break;
                }
            }
            return result;
        }

        //зчитати з консолі------------------------------------
        public void ReadProductsFromConsole()
        {
            string input;
            int variety;
            int sizeOfArr;
            //розмір масиву
            Console.WriteLine("How many products do you want(>=0)?");
            input = Console.ReadLine();

            //поки не отримуємо потрібне число
            if ((!Int32.TryParse(input, out sizeOfArr)) || (sizeOfArr < 0))
            {
                Console.WriteLine("Wrong input, will be 2 instead");
                sizeOfArr = 2;
            }
            //оновлюємо розмір масиву
            products.Clear();
            for (int counter = 0; counter < sizeOfArr; counter++)
            {
                //вибираємо варіант класу product
                Console.WriteLine(string.Format("Choose variety: 1->Meat\t2->Dairy Product\t3->Product"));
                input = Console.ReadLine();
                if((!Int32.TryParse(input, out variety)) || (variety < 1) || (variety > 3))
                {
                    Console.WriteLine("Wrong input, will be 3 instead");
                    variety = 3;
                }
                //м'ясний
                if (variety == 1)
                {
                    //створюємо об'єкт з отриманих даних і додаємо в масив
                    try
                    {
                        products.Add(new Meat());
                        products[products.Count - 1].ReadFromConsole();
                    }
                    catch (Exception ex)
                    {
                        OnIncorrectInput?.Invoke(this,PathToLogFile, ex.Message);
                    }
                }
                //молочні
                else if (variety == 2)
                {
                    try
                    {
                        products.Add(new DairyProduct());
                        products[products.Count - 1].ReadFromConsole();
                    }
                    catch (Exception ex)
                    {
                        OnIncorrectInput?.Invoke(this,PathToLogFile, ex.Message);
                    }
                }
                //звичайний
                else
                {
                    try
                    {
                        products.Add(new Product());
                        products[products.Count - 1].ReadFromConsole();
                    }
                    catch (Exception ex)
                    {
                        OnIncorrectInput?.Invoke(this, PathToLogFile, ex.Message);
                    }
                }
            }
        }

        //отримати інформацію з файлу--------------------
        public void ReadFromFile(string path)
        {
            if (File.Exists(path))
            {
                using (StreamReader r = new StreamReader(path))
                {
                    string line;
                    //оновлюємо список
                    products.Clear();

                    //зчитуємо поки не буде кінця файлу
                    while ((line = r.ReadLine())!=null)
                    {
                        //визначити який тип продукту нам записали
                        int elements = line.Split().Length;

                        //якщо дано 5 елементів, то це клас Product
                        //Product і DairyProduct однакові, у обох є термін придатності
                        if (elements == 5)
                        {
                            //попередньо створюємо об'єкт касу в списку
                            //міняємо цей об'єкт, у нас є функція приведення стрічки у об'єкти класу
                            try
                            {
                                products.Add(new Product());
                                products[products.Count - 1].Parse(line);
                            }
                            catch (Exception ex)
                            {
                                OnIncorrectInput?.Invoke(this, PathToLogFile,ex.Message);
                            }
                        }
                        //якщо 6 елем, то це класс Meat
                        else if (elements == 6)
                        {
                            //при виникнені помилки, ми опрацюємо неправильні дані
                            //і зчитування продовжиться далі
                            try
                            {
                                products.Add(new Meat());
                                products[products.Count - 1].Parse(line);
                            }
                            catch (Exception ex)
                            {
                                OnIncorrectInput?.Invoke(this, PathToLogFile, ex.Message);
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Wrong number of elements in text");
                        }
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("File not found");
            }
        }

        //перевіряє дати пригодністі всіх елементів у storage-------------
        public void CheckExpirationDate(string pathToWrite)
        {
            using (StreamWriter writer = new StreamWriter(pathToWrite,append:true))
            {
                for (int i = 0; i < this.products.Count; i++)
                {
                    //Substract порінює дві дати і вертає різницю між ними у TimeSpan
                    //Порівнюємо з теперішньою датою
                    TimeSpan difference = DateTime.Now.Subtract(this.products[i].CreationDay);
                    //отримує скільки днів минуло
                    int daysPassed = (int)difference.TotalDays;
                    //якщо термін пригодністі сплив
                    //зписуємо у файл і вилучаємо
                    if (daysPassed > this.products[i].ExpirationDay)
                    {
                        writer.WriteLine("Info --> Expired");
                        writer.WriteLine(this.products[i]);
                        writer.WriteLine("Removed at Time: [{0},{1}]\n",DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString());
                        RemoveByName(this.products[i].NameOfProduct);
                    }
                }
            }
        }

        //вивестю всю інформацію-----------------
        public override string ToString()
        {
            //викликаємо подію запису спорчених продуктів у файл
            OnShowStorage?.Invoke(this,PathToLogFile);


            string res = "";
            for (int i = 0; i < products.Count; i++)
            {
                res += String.Format("№{0}->{1}\n", i + 1, products[i].ToString());
            }
            return res;
        }

        //знайти м'ясні масиви-----------------
        public Meat[] FindMeatProducts()
        {
            return (Meat[])products.Where(x => x is Meat).ToArray();
        }

        //індексатор-----------------
        public Product this[int index]
        {

            get
            {
                try
                {
                    return products[index];
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Wrong index");
                    return new Product();
                }
            }
            set
            {
                try
                {
                    products[index] = value;
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Wrong index");
                }

            }

        }
        //змінити ціни у всіх------------------------
        public void ChangeAllPrice(int interest)
        {
            foreach (var item in products)
            {
                item.ChangePrice(interest);
            }
        }
        //енумератор-------------------
        public IEnumerator<Product> GetEnumerator()
        {
            return new StorageEnumerator(products);
        }

    }
    //Клас Енумератор для Storage--------
    class StorageEnumerator : IEnumerator<Product>
    {
        List<Product> products;
        int position = -1;

        public StorageEnumerator(List<Product> products)
        {
            this.products = products;
        }
        public Product Current
        {
            get
            {
                if (position == -1 || position >= products.Count)
                    throw new InvalidOperationException("Wrong position");
                return products[position];
            }
        }

        object IEnumerator.Current => throw new NotImplementedException();

        public void Dispose() { }

        public bool MoveNext()
        {
            if (position < products.Count - 1)
            {
                position++;
                return true;
            }
            else
                return false;
        }

        public void Reset()
        {
            position = -1;
        }
    }
}
