using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
    internal class Program
    {
        private static SamuraiContext context = new SamuraiContext();

        private static void Main(string[] args)
        {
            //context.Database.EnsureCreated();
            //context.Database.Migrate();
            // InserMultipleSamurais();
            // InsertVariousTypes();
            // GetSamurais("Before Add:");
            // AddSamurai();
            // GetSamurais("After Add:");
            //QueryFilters();
            //RetrieveAndUpdateSamurai();
            //RetrieveAndUpdateMultipleSamurai();
            //DeleteSamurai();
            InsertBattle();
            UpdateBattleDisconnected();
            Console.Write("Press any key...");
            Console.ReadKey();
        }

        private static void InsertBattle()
        {
            var battle = new Battle() { Name = "Battle of  betılü" };
            context.Battles.Add(battle);
            context.SaveChanges();
        }

        private static void UpdateBattleDisconnected()
        {
            var battle = context.Battles.AsNoTracking().FirstOrDefault();
            battle.EndDate = new DateTime(1432, 11, 4);
            using (var newContext = new SamuraiContext())
            {
                newContext.Battles.Update(battle);
                newContext.SaveChanges();
            }
        }

        private static void DeleteSamurai()
        {
            var samurai = context.Samurais.Find(10);
            context.Samurais.Remove(samurai);
            var result = context.SaveChanges();
        }

        private async static void RetrieveAndUpdateMultipleSamurai()
        {
            var samurais = context.Samurais.Skip(0).Take(6);
            await samurais.ForEachAsync(x =>
            {
                if (x.Name.Contains("Ssan"))
                    x.Name = x.Name.Replace("Ssan", "San");
                else
                    x.Name += "San";
            });
            context.Samurais.Add(new Samurai() { Name = "ELüSan" });
            var ar = await context.SaveChangesAsync();
        }

        private static void RetrieveAndUpdateSamurai()
        {
            var samurai = context.Samurais.FirstOrDefault();
            samurai.Name += "San";
            var result = context.SaveChanges();
        }

        private static void QueryFilters()
        {
            var name = "Sampson";
            var samurai = context.Samurais.FirstOrDefault(s => s.Name == name);
            var samurai3 = context.Samurais.Find(2);
            //var samurai4 = context.Samurais.First(x=> x.Name =="camalala");
            var samurai5 = context.Samurais.FirstOrDefault(x => x.Name == "camalala");
            var filter = "J%";
            var samurai2 = context.Samurais.Where(samurai => EF.Functions.Like(samurai.Name, filter)).ToList();
            var samurai22 = context.Samurais.OrderBy(x => x.Id).LastOrDefault();

            Console.WriteLine();
        }

        private static void InsertVariousTypes()
        {
            var sam1 = new Samurai() { Name = "mi1" };
            var clan1 = new Clan() { ClanName = "clan1" };
            context.AddRange(sam1, clan1);
            context.SaveChanges();

        }

        private async static void InserMultipleSamurais()
        {
            var sam1 = new Samurai() { Name = "mi1" };
            var sam2 = new Samurai() { Name = "mi2" };
            var sam3 = new Samurai() { Name = "mi3" };
            var sam4 = new Samurai() { Name = "mi4" };
            var sam5 = new Samurai() { Name = "mi5" };
            //context.Samurais.AddRangeAsync(sam1, sam2);
            await context.Samurais.AddRangeAsync(new List<Samurai>() { sam1, sam2, sam3, sam4, sam5 });
            await context.SaveChangesAsync();
        }

        private static void AddSamurai()
        {
            var samurai = new Samurai { Name = "Sampson" };
            context.Samurais.Add(samurai);
            context.SaveChanges();
        }

        private static void GetSamurais(string text)
        {
            var samurais = context.Samurais.ToList();
            Console.WriteLine($"{text}: Samurai count is {samurais.Count}");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }
    }
}