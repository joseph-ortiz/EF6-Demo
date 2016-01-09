using System;
using System.Data.Entity;
using NinjaDomain.Classes;
using NinjaDomain.DataModel;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            //Database.SetInitializer(new NullDatabaseInitializer<NinjaContext>());
            //InsertNinja();
            SimpleNinjaQueries();
            Console.ReadKey();
        }

        private static void SimpleNinjaQueries()
        {
            throw new NotImplementedException();
        }

        private static void InsertNinja()
        {
            
            var ninja = new Ninja
            {
                Name = "LondonSan",
                ServedInOniwaban = false,
                DateOfBirth = new DateTime(2015, 11,15),
                ClanId = 1
            };

            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.Add(ninja);
                context.SaveChanges();
            }
        }
    }
}
