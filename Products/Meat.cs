using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaTask9.Products
{
    //перечислення
    enum MeatCategories { High_sort = 1, I_sort, II_sort };
    enum MeatTypes { Lamb = 1, Veal, Pork, Chicken };

    class Meat : Product
    {
        public MeatCategories Category { get; set; }
        private MeatTypes type;
        //тип м'яса одночасно є і його ім'ям
        //властивість
        public MeatTypes Type
        {
            get { return this.type; }
            set
            {
                this.type = value;
                NameOfProduct = Enum.GetName(typeof(MeatTypes), type);
            }
        }



        //Конструктор має ціну, вагу, категорію і тип м'яса
        public Meat()
        : this(new DateTime(2020, 1, 1), 0, 0, "N/A", 0, 1, 1) { }
        public Meat(DateTime time, double price = 0, double weight = 0,
            string name = "N/A", int exDay = 0,
            int index_category = 1, int index_type = 1) : base(time, price, weight, name, exDay)
        {
            try
            {
                if ((index_type < 1) || (index_category > 4))
                {
                    throw new ArgumentException("Wrong index");
                }
                else
                {
                    this.Category = (MeatCategories)Enum.GetValues(typeof(MeatCategories)).GetValue(index_category - 1);
                    this.Type = (MeatTypes)Enum.GetValues(typeof(MeatTypes)).GetValue(index_type - 1);

                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        
        //Equal
        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType()) return false;

            var other = obj as Meat;
            return (this.Type == other.Type);
        }
        //ToString
        public override string ToString()
        {
            string s_type = NameOfProduct;
            string s_category = Enum.GetName(typeof(MeatCategories), Category);
            string str_date = CreationDay.ToShortDateString();
            string res = string.Format("Type: {0}\tCategory: {1}\nPrice: {2:f2}$\tWeight: {3:f2}kg\nDate of creation: {4}\tExpiration day: {5}",
                 s_type, s_category, PriceOfProduct, WeightOfProduct, str_date, ExpirationDay);
            return res;
        }
        //змінити ціну 
        public override void ChangePrice(int interest)
        {
            if (interest < 0) { interest = 0; }

            double d_interest = ((double)interest) / 100;
            //обраховуємо особливість сорту
            switch (Category)
            {
                case MeatCategories.High_sort:
                    d_interest += 0.5;
                    break;
                case MeatCategories.I_sort:
                    d_interest += 0.3;
                    break;
                default:
                    d_interest += 0.1;
                    break;
            }
            this.PriceOfProduct = this.PriceOfProduct +
                this.PriceOfProduct * d_interest;
        }

        //зчитати з файлу клас Meat-------------
        //приклад коректних вхідних даних
        //m = "10:02:2001 32,32 5,5 30 1 1"
        public override void Parse(string str_object)
        {
            string[] lineSplit = str_object.Split();
            if (lineSplit.Length != 6)
            {
                throw new FormatException(string.Format("Wrong input string: {0}", str_object));
            }

            //дата
            string[] date = lineSplit[0].Split(':');
            if (date.Length != 3)
            {
                throw new FormatException(string.Format("Wrong date info: {0}", lineSplit[0]));
            }
            int day, month, year;

            if (!Int32.TryParse(date[1], out month) || (month < 1) || (month > 12))
            {
                throw new ArgumentException(string.Format("Wrong Month: {0}", month));
            }
            if (!Int32.TryParse(date[2], out year) || (year < 1900))
            {
                throw new ArgumentException(string.Format("Wrong Year: {0}", year));
            }
            //перевіряємо чи рік високосний
            //і чи правильна кількість днів у місяці
            if (Int32.TryParse(date[0], out day) && (day > 0))
            {
                switch (month)
                {
                    //31 день у місяці
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 8:
                    case 10:
                    case 12:
                        if (day > 31)
                        {
                            throw new ArgumentException("Day cannnot be >31");
                        }
                        break;
                    //28 днів або 29 високосному році
                    case 2:
                        bool isLeapYear = false;
                        //преевірка на високосність
                        if (year % 4 == 0)
                        {
                            if ((year % 100 != 0) || (year % 100 == 0 && year % 400 == 0))
                            {
                                isLeapYear = true;
                            }
                        }
                        if (isLeapYear)
                        {
                            if (day > 29)
                            {
                                throw new ArgumentException("Day cannnot be >29 in February, leap year");
                            }
                        }
                        else
                        {
                            if (day > 28)
                            {
                                throw new ArgumentException("Day cannnot be >28 in February, not leap year");
                            }
                        }
                        break;
                    //30 днів у місяці
                    default:
                        if (day > 30)
                        {
                            throw new ArgumentException("Day cannot be >30");
                        }
                        break;
                }
            }
            else
            {
                throw new ArgumentException(string.Format("Wrong Day: {0}", day));
            }
            //пройшли всі перевірки, свторюємо дату
            this.CreationDay = new DateTime(year, month, day);
            //ціна---------
            if (!Double.TryParse(lineSplit[1], out this.priceOfProduct) || (PriceOfProduct < 0))
            {
                throw new ArgumentException(string.Format("Wrong Price: {0}", PriceOfProduct));
            }
            //вага-----------
            if (!Double.TryParse(lineSplit[2], out this.weightOfProduct) || (WeightOfProduct < 0))
            {
                throw new ArgumentException(string.Format("Wrong Weight: {0}", WeightOfProduct));
            }
            //день придатності
            if (!Int32.TryParse(lineSplit[3], out this.expirationDate) || (ExpirationDay <= 0))
            {
                throw new ArgumentException(string.Format("Wrong Expiration day: {0}", expirationDate));
            }
            //категорія
            int category;
            if (!Int32.TryParse(lineSplit[4], out category) || (category < 1) || (category > 3))
            {
                throw new ArgumentException(string.Format("Wrong category: {0}",category));
            }
            //індекс масиву System.Array від 0 до 2
            this.Category = (MeatCategories)Enum.GetValues(typeof(MeatCategories)).GetValue(category - 1);
            //тип
            int type;
            if (!Int32.TryParse(lineSplit[5], out type) || (type < 1) || (type > 4))
            {
                throw new ArgumentException(string.Format("Wrong type: {0}", type));
            }
            //індекс масиву System.Array від 0 до 3
            Type = (MeatTypes)Enum.GetValues(typeof(MeatTypes)).GetValue(type - 1);
        }
        //зчитати з консолі---------------------
        public override void ReadFromConsole()
        {
            string input;
            //загальні поля
            double price;
            double weight;
            string name;
            int exDay;
            //уточняємо інформацію по виду продукту
            //м'ясо
            int category;
            int type;
            //тип м'яса (одночасно це і його ім'я)
            Console.WriteLine("Choose type: 1->Lamb\t2->Veal\t3->Pork\t4->Chicken");
            input = Console.ReadLine();
            if ((!Int32.TryParse(input, out type)) || (type < 1) || (type > 4))
            {
                throw new ArgumentException(string.Format("Wrong type of meat: {0}",type));
            }
            this.Type = (MeatTypes)Enum.GetValues(typeof(MeatTypes)).GetValue(type - 1);
            //категорія
            Console.WriteLine("Choose category: 1->High_sort\t 2->I_sort\t3->II_sort");
            input = Console.ReadLine();
            if ((!Int32.TryParse(input, out category)) || (category < 1) || (category > 3))
            {
                throw new ArgumentException(string.Format("Wrong category of meat: {0}", category));
            }
            this.Category = (MeatCategories)Enum.GetValues(typeof(MeatCategories)).GetValue(category - 1);
            //ціну
            Console.WriteLine("Write price(>0)");
            input = Console.ReadLine();
            if ((!Double.TryParse(input, out price)) || (price <= 0))
            {
                throw new ArgumentException(string.Format("Wrong price: {0}",price));
            }
            this.PriceOfProduct = price;
            //вага
            Console.WriteLine("Write weight(>0)");
            input = Console.ReadLine();
            if ((!Double.TryParse(input, out weight)) || (weight <= 0 ))
            {
                throw new ArgumentException(string.Format("Wrong weight: {0}", weight));
            }
            this.WeightOfProduct = weight;
            //дні придатності
            Console.WriteLine("Write Expitation day(>0)");
            input = Console.ReadLine();
            if ((!Int32.TryParse(input, out exDay)) || (exDay < 1))
            {
                throw new ArgumentException(string.Format("Wrong expiration day: {0}",exDay));
            }
            this.ExpirationDay = exDay;
            //дата створення
            Console.WriteLine("Write creation date(12:08:2001)");
            input = Console.ReadLine();
            string[] str_date = input.Split(':');
            int day, month, year;

            if (!Int32.TryParse(str_date[1], out month) || (month < 1) || (month > 12))
            {
                throw new ArgumentException(string.Format("Wrong Month: {0}",month));
            }
            if (!Int32.TryParse(str_date[2], out year) || (year < 1900))
            {
                throw new ArgumentException(string.Format("Wrong Year: {0}", year));
            }
            //перевіряємо чи рік високосний
            //і чи правильна кількість днів у місяці
            if (Int32.TryParse(str_date[0], out day) && (day > 0))
            {
                switch (month)
                {
                    //31 день у місяці
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 8:
                    case 10:
                    case 12:
                        if (day > 31)
                        {
                            throw new ArgumentException("Day cannnot be >31");
                        }
                        break;
                    //28 днів або 29 високосному році
                    case 2:
                        bool isLeapYear = false;
                        //преевірка на високосність
                        if (year % 4 == 0)
                        {
                            if ((year % 100 != 0) || (year % 100 == 0 && year % 400 == 0))
                            {
                                isLeapYear = true;
                            }
                        }
                        if (isLeapYear)
                        {
                            if (day > 29)
                            {
                                throw new ArgumentException("Day cannnot be >29 in February, leap year");
                            }
                        }
                        else
                        {
                            if (day > 28)
                            {
                                throw new ArgumentException("Day cannnot be >28 in February, not leap year");
                            }
                        }
                        break;
                    //30 днів у місяці
                    default:
                        if (day > 30)
                        {
                            throw new ArgumentException("Day cannot be >30");
                        }
                        break;
                }
            }
            else
            {
                throw new ArgumentException(string.Format("Wrong Day: {0}",day));
            }
            //пройшли всі перевірки, свторюємо дату
            this.CreationDay = new DateTime(year, month, day);
        }
    }
}
