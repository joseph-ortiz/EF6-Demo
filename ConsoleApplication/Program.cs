using System;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
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
            //SimpleNinjaQueries();
            //QueryAndUpdateNinja();
            //QueryAndUpdateNinjaDisconnected();
            //RetrieveDataWithFind();
            //RetrieveDataWithStoredProc();
            //DeleteNinja();
            //DeleteNinjaWithStoredProc();
            //InsertNinjaWithEquipment();
            //SimpleNinjaGraphQuery();
            ProjectQuery();
            Console.ReadKey();
        }

        private static void ProjectQuery()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninjas = context.Ninjas
                    .Select(n => new {n.Name, n.DateOfBirth, n.EquipmentOwned})
                    .ToList();
            }
        }

        private static void SimpleNinjaGraphQuery()
        {
            Ninja ninja;
            var LoadingType = "lazy";
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                switch (LoadingType)
                {
                    case "eager":
                         ninja = context.Ninjas
                            .Include(n => n.EquipmentOwned)
                            .FirstOrDefault(n => n.Name == "Joe Ortiz");
                        break;
                    case "explicit":
                         ninja = context.Ninjas.FirstOrDefault(n => n.Name.StartsWith("Joe Ortiz"));
                        Console.WriteLine("Ninja Retrieved:" + ninja.Name);
                        context.Entry(ninja).Collection(n => n.EquipmentOwned).Load();
                        break;
                    case "lazy":
                        ninja = context.Ninjas.FirstOrDefault(n => n.Name.StartsWith("Joe Ortiz"));
                        Console.WriteLine("Ninja Retrieved:" + ninja.Name);
                        Console.WriteLine("Ninja Equipment Count: " + ninja.EquipmentOwned.Count()); 
                        //mark EquipmentOwned property as virtual. Be careful with performance issues.
                        break;
                }
            }
        }

    

        private static void InsertNinjaWithEquipment()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;

                var ninja = new Ninja()
                {
                    Name = "Joe Ortiz",
                    ServedInOniwaban = false,
                    DateOfBirth = new DateTime(1990, 1, 1),
                    ClanId = 1
                };

                var muscles = new NinjaEquipment
                {
                    Name = "Muscles",
                    Type = EquipmentType.Tool
                };

                var spunk = new NinjaEquipment()
                {
                    Name = "Spunk",
                    Type = EquipmentType.Weapon
                };

                context.Ninjas.Add(ninja);
                ninja.EquipmentOwned.Add(muscles);
                ninja.EquipmentOwned.Add(spunk);
                context.SaveChanges();

            }
        }

        private static void DeleteNinjaWithStoredProc()
        {
            var keyVal = 2;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Database.ExecuteSqlCommand("exec DeleteNinjaViaId {0}", keyVal);
            }
        }

        private static void DeleteNinja()
        {
            Ninja ninja;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                ninja = context.Ninjas.FirstOrDefault();
            }

            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Entry(ninja).State = EntityState.Deleted;
                //context.Ninjas.Remove(ninja);
                //context.Ninjas.Remove(ninja);
                context.SaveChanges();
            }
        }

        private static void RetrieveDataWithStoredProc()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninjas = context.Ninjas.SqlQuery("exec GetOldNinjas");
                foreach (var ninja in ninjas)
                {
                    Console.WriteLine(ninja.Name);
                }
            }
        }

        private static void RetrieveDataWithFind()
        {
            var keyval = 2;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.Find(keyval);
                Console.WriteLine("After Find#1: " + ninja.Name);

                var someNinja = context.Ninjas.Find(keyval);
                Console.WriteLine("After Find#2: " + ninja.Name);
                ninja = null;

            }
        }

        private static void QueryAndUpdateNinjaDisconnected()
        {
            Ninja ninja;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                ninja = context.Ninjas.FirstOrDefault();
            }

            ninja.ServedInOniwaban = (!ninja.ServedInOniwaban);

            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Ninjas.Attach(ninja);
                context.Entry(ninja).State = EntityState.Modified;
                context.SaveChanges();
            }

        }

        private static void QueryAndUpdateNinja()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.FirstOrDefault();
                ninja.ServedInOniwaban = (!ninja.ServedInOniwaban);
                context.SaveChanges();
            }
        }

        private static void SimpleNinjaQueries()
        {
            using (var context = new NinjaContext())
            {
                var ninjas = context.Ninjas.Where(x => x.Name == "JoeSan");
                foreach (var ninja in ninjas)
                {
                    Console.WriteLine(ninja.Name);
                }
            }
        }

        private static void InsertNinja()
        {
            
            var ninja = new Ninja
            {
                Name = "JoeSan",
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
