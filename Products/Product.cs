using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaTask9.Products
{
    class Product
    {
        double priceOfProduct;
        double weightOfProduct;
        int expirationDate;
        public string NameOfProduct { get; set; }
        public DateTime CreationDay { get; set; }
        //властивість------
        public int ExpirationDay
        {
            get { return this.expirationDate; }
            set
            {
                if (value >= 0)
                {
                    this.expirationDate = value;
                }
                else
                    throw new ArgumentException("Wrong Day");
            }
        }
        //властивість-------------
        public double PriceOfProduct
        {
            get { return this.priceOfProduct; }
            set
            {
                if (value >= 0)
                    this.priceOfProduct = value;
                else
                    throw new ArgumentException("Wrong number");
            }
        }
        //властивість-----------------
        public double WeightOfProduct
        {
            get { return this.weightOfProduct; }
            set
            {
                if (value >= 0)
                    this.weightOfProduct = value;
                else
                    throw new ArgumentException("Wrong number");
            }
        }


        //Конструктори------------------
        public Product()
        : this(new DateTime(2020, 1, 1), 0, 0, "N/A", 0) { }

        public Product(DateTime time, double price = 0, double weight = 0,
            string name = "N/A", int exDay = 0)
        {
            try
            {
                NameOfProduct = name;
                PriceOfProduct = price;
                WeightOfProduct = weight;
                ExpirationDay = exDay;
                CreationDay = time;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


        //Parse-------------------
        //приклад коректних вхідних даних
        //s = "10:02:2001 32,32 5,5 Milk 30"
        public virtual void Parse(string str_object)
        {

            string[] lineSplit = str_object.Split();
            if (lineSplit.Length != 5)
            {
                throw new FormatException("Wrong input string");
            }

            //дата
            string[] date = lineSplit[0].Split(':');
            if (date.Length != 3)
            {
                throw new FormatException("Wrong date info");
            }
            int day, month, year;

            if (!Int32.TryParse(date[1], out month) || (month < 1) || (month > 12))
            {
                throw new ArgumentException("Wrong Month");
            }
            if (!Int32.TryParse(date[2], out year) || (year < 1900))
            {
                throw new ArgumentException("Wrong Year");
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
                throw new ArgumentException("Wrong Day");
            }
            //пройшли всі перевірки, свторюємо дату
            this.CreationDay = new DateTime(year, month, day);
            //ціна
            if (!Double.TryParse(lineSplit[1], out this.priceOfProduct) || (PriceOfProduct < 0))
            {
                throw new ArgumentException("Wrong Price");
            }
            //вага
            if (!Double.TryParse(lineSplit[2], out this.weightOfProduct) || (WeightOfProduct < 0))
            {
                throw new ArgumentException("Wrong Weight");
            }
            //назва
            NameOfProduct = lineSplit[3];
            //день придатності
            if (!Int32.TryParse(lineSplit[4], out this.expirationDate) || (ExpirationDay <= 0))
            {
                throw new ArgumentException("Wrong Expiration day");
            }
        }

        //фунція рівності-----------
        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType()) return false;

            var other = obj as Product;
            return (this.NameOfProduct == other.NameOfProduct);
        }
        //ToString------------
        public override string ToString()
        {
            string str_date = CreationDay.ToShortDateString();
            string res = string.Format("Name: {0}\tPrice: {1:f2}$\tWeight: {2:f2}kg\nDate of creation: {3}\tExpiration day: {4}",
                NameOfProduct, PriceOfProduct, WeightOfProduct, str_date, ExpirationDay);
            return res;
        }
        //змінити ціну------------------------
        public virtual void ChangePrice(int interest)
        {
            if (interest < 0) { interest = 0; }

            double d_interest = ((double)interest) / 100;
            this.PriceOfProduct = this.PriceOfProduct +
                this.PriceOfProduct * d_interest;
        }
    }
}
