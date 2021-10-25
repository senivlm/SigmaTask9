using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaTask9.Products
{
    class DairyProduct : Product
    {
        //з днями придатності

        public DairyProduct()
        : this(new DateTime(2020, 1, 1), 0, 0, "N/A", 0) { }

        public DairyProduct(DateTime time, double price = 0, double weight = 0,
            string name = "N/A", int exDay = 0) : base(time, price, weight, name, exDay = 0)
        {
        }
        //Equal
        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType()) return false;

            var other = obj as DairyProduct;
            return ((this.NameOfProduct == other.NameOfProduct) && (this.ExpirationDay == other.ExpirationDay));
        }
        //ToString
        public override string ToString()
        {
            return base.ToString();
        }
        //змінити ціну
        public override void ChangePrice(int interest)
        {
            if (interest < 0) { interest = 0; }

            double d_interest = ((double)interest) / 100 + ((double)ExpirationDay) / 100;


            this.PriceOfProduct = this.PriceOfProduct +
                this.PriceOfProduct * d_interest;
        }

        //зчитати з консолі------------------------
        public virtual void ReadFromConsole()
        {
            string input;
            //загальні поля
            double price;
            double weight;
            string name;
            int exDay;

            //ціну
            Console.WriteLine("Write price(>0)");
            input = Console.ReadLine();
            if ((!Double.TryParse(input, out price)) || (price < 0))
            {
                throw new ArgumentException(string.Format("Wrong price: {0}", price));
            }
            this.PriceOfProduct = price;
            //вага
            Console.WriteLine("Write weight(>0)");
            input = Console.ReadLine();
            if ((!Double.TryParse(input, out weight)) || (weight < 0))
            {
                throw new ArgumentException(string.Format("Wrong weight: {0}", weight));
            }
            this.WeightOfProduct = weight;
            //дні придатності
            Console.WriteLine("Write Expitation day(>0)");
            input = Console.ReadLine();
            if ((!Int32.TryParse(input, out exDay)) || (exDay < 1))
            {
                throw new ArgumentException(string.Format("Wrong expiration day: {0}", exDay));
            }
            this.ExpirationDay = exDay;
            //дата створення
            Console.WriteLine("Write creation date(12:08:2001)");
            input = Console.ReadLine();
            string[] str_date = input.Split(':');
            int day, month, year;

            if (!Int32.TryParse(str_date[1], out month) || (month < 1) || (month > 12))
            {
                throw new ArgumentException(string.Format("Wrong Month: {0}", month));
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
                throw new ArgumentException(string.Format("Wrong Day: {0}", day));
            }
            //пройшли всі перевірки, свторюємо дату
            this.CreationDay = new DateTime(year, month, day);
        }
    }
}
