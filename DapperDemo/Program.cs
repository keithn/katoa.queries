using System;

namespace DapperDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new DemoDb();
            db.Open();
            db.AllData().ForEach(Console.WriteLine);
            Console.WriteLine("\r\nFiltered:");
            db.DataLike("an").ForEach(Console.WriteLine);
        }
    }
}