using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.IO;

namespace Goods
{

    [XmlInclude(typeof(Product))]
    [XmlInclude(typeof(Consignment))]
    [XmlInclude(typeof(Set))]
    [Serializable]
    public abstract class Goods
    {
        public Goods() { }

        public abstract void GetInformation();
        public abstract bool CheckShelfLife();
    }

    [Serializable]
    public class Product : Goods
    {
        public string LabelOfProduct;
        public int Price;
        public string Date;
        public int LifeCount;

        public Product() { }

        public Product(string label, int price, string date, int life)
        {
            LabelOfProduct = label;
            Price = price;
            Date = date;
            LifeCount = life;
        }

        public override void GetInformation()
        {
            Console.WriteLine("Продукт: {0}, {1}, {2}, {3}", LabelOfProduct, Price, Date, LifeCount);
        }

        public override bool CheckShelfLife()
        {
            DateTime itemDate = Convert.ToDateTime(Date);
            DateTime curDate = DateTime.Today;
            if (itemDate.AddDays(LifeCount) < curDate)
            {
                Console.Write(" - просрочено!");
                return true;
            }
            else return false;
        }
    }

    [Serializable]
    public class Consignment : Goods
    {
        public string LabelOfCons;
        public int Price;
        public int Count;
        public string Date;
        public int LifeCount;

        public Consignment() { }
        public Consignment(string label, int price, int count, string date, int lifecount)
        {
            LabelOfCons = label;
            Price = price;
            Count = count;
            Date = date;
            LifeCount = lifecount;
        }
        public override void GetInformation()
        {
            Console.WriteLine("Партия: {0}, {1}, {2}, {3}, {4}", LabelOfCons, Price, Count, Date, LifeCount);
        }

        public override bool CheckShelfLife()
        {
            DateTime itemDate = Convert.ToDateTime(Date);
            DateTime curDate = DateTime.Today;
            if (itemDate.AddDays(LifeCount) < curDate)
            {
                Console.Write(" - просрочено!");
                return true;
            }
            else return false;
        }
    }

    [Serializable]
    public class Set : Goods
    {
        public string LabelOfSet;
        public int Price;
        public string List;
        public int Count;
        public string Date;
        public int LifeCount;

        public Set() { }

        public Set(string label, int price, string list, int count, string date, int lifecount)
        {
            LabelOfSet = label;
            Price = price;
            List = list;
            Count = count;
            Date = date;
            LifeCount = lifecount;
        }

        public override bool CheckShelfLife()
        {
            DateTime itemDate = Convert.ToDateTime(Date);
            DateTime curDate = DateTime.Today;
            if (itemDate.AddDays(LifeCount) < curDate)
            {
                Console.Write(" - просрочено!");
                return true;
            }
            else return false;
        }

        public override void GetInformation()
        {
            Console.WriteLine("Набор: {0}, {1}, {2}, {3}", LabelOfSet, Price, Count, Date, LifeCount);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string path = "./input.txt";
            Goods[] array;
            try
            {
                array = ReadFile(path);
                foreach (Goods gd in array)
                {
                    gd.GetInformation();
                }
                Serialize(array);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File is not found");
            }
            catch (SerializationException)
            {
                Console.WriteLine("Some problems with serialization");
            }
        }

        public static Goods[] ReadFile(string pathIn)
        {
            using (StreamReader sr = new StreamReader(pathIn, System.Text.Encoding.Default))
            {

                int n = Convert.ToInt32(sr.ReadLine());
                int count = 0;
                Goods[] arr = new Goods[n];
                string line;
                string[] splitLine;
                while ((line = sr.ReadLine()) != null)
                {
                    splitLine = line.Split(" ");
                    switch (splitLine[0])
                    {
                        case "Product":
                            arr[count] = new Product(splitLine[1], Convert.ToInt32(splitLine[2]), splitLine[3], Convert.ToInt32(splitLine[4]));
                            count++;
                            break;
                        case "Consignment":
                            arr[count] = new Consignment(splitLine[1], Convert.ToInt32(splitLine[2]), Convert.ToInt32(splitLine[3]), splitLine[4], Convert.ToInt32(splitLine[5]));
                            count++;
                            break;
                        case "Set":
                            arr[count] = new Set(splitLine[1], Convert.ToInt32(splitLine[2]), splitLine[3], Convert.ToInt32(splitLine[4]), splitLine[5], Convert.ToInt32(splitLine[6]));
                            count++;
                            break;
                    }
                }

                return arr;
            }
        }


        public static void Serialize(Goods[] array)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Goods[]));
            using (FileStream fs = new FileStream("goods.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, array);
            }
        }
    }
}

