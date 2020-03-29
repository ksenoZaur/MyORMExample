using System;
using System.Collections.Generic;

namespace MyORM
{
    class Program
    {
        static void Main(string[] args)
        {
            HubORM qb = new HubORM(@"ORMConfig.xml", 
                @"Data Source=XENON-PC\SQLEXPRESS; Initial Catalog=exampleDB2; Integrated Security=true;");
            var result = qb.GetAll<SomeBook>();
            foreach (var res in result)
            {
                Console.WriteLine(res.ToString());
            }
        }
    }
}