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

            //InsertBattle();
            //UpdateBattleDisconnected();

            //InserNewSamuraiWithAQuote();
            //InserNewSamuraiWithManyQuote();
            //AddQuoteToExistingSamuraiWhileTracked();
            //AddQuoteToExistingSamuraiWhileNotTracked(5);
            //AddQuoteToExistingSamuraiWhileNotTrackedEasy(5);

            //EagerLoadSamuraiWithQuotes();
            //ProjectSomeProperties();
            //ProjectSamuraisWithQuotes();
            //ExplicitLoadQuotes();
            //LazyLoading();

            //ModifyingRelatedDataWhenTracked();
            //ModifyingRelatedDataWhenNotTracked();

            //JoinBattleAndSamurai();
            //EnlistSamuraiIntoABattle();

            //GetSamuraiWithBattles();

            //AddNewSamuraiWithHorse();
            //AddNewHorseToSamuraiUsingSamuraiId();
            //AddNewHorseToSamuraiInMemory();
            //AddNewHorseToDisconnectedSamuraiInMemory();
            //ReplaceAHorse();

            //GetSamuraisWithHorse();
            //GetHorseWithSamurai();

            GetSamuraisWithClans();
            GetClanWithSamurais();

            Console.Write("Press any key...");
            Console.ReadKey();
        }

        private static void GetClanWithSamurais()
        {
            var clan = context.Clans.Find(1);
            var samuraisOfClan = context.Samurais.Where(s => s.Clan.Id == clan.Id).ToList();
        }

        private static void GetSamuraisWithClans()
        {
            var samurais = context.Samurais.Include(s => s.Clan).ToList();
            Console.WriteLine();
        }

        private static void GetHorseWithSamurai()
        {
            var horsesWithOutSamurai = context.Set<Horse>().ToList();
            var horseWithOutSamurai = context.Set<Horse>().Find(3);

            var samuraiWithHorse = context.Samurais.Include(x => x.Horse).FirstOrDefault(s => s.Horse != null);

            var horseWithSamurais = context.Samurais
                .Where(s => s.Horse != null)
                .Select(s => new { Horse = s.Horse, Samurai = s })
                .ToList();

            Console.WriteLine();
        }

        private static void GetSamuraisWithHorse()
        {
            var samurai = context.Samurais.Include(x => x.Horse).ToList();
        }

        private static void ReplaceAHorse()
        {
            var samurai = context.Samurais.Include(s => s.Horse).FirstOrDefault(x => x.Horse != null);
            samurai.Horse = new Horse { Name = "Replaced" };
            context.SaveChanges();
        }

        private static void AddNewHorseToDisconnectedSamuraiInMemory()
        {
            var samurai = context.Samurais.Include(s => s.Horse).FirstOrDefault(x => x.Horse == null);
            samurai.Horse = new Horse { Name = "Hoğse" };
            using (var context2 = new SamuraiContext())
            {
                context2.Samurais.Attach(samurai);
                context2.SaveChanges();
            }

        }

        private static void AddNewHorseToSamuraiInMemory()
        {
            var samurai = context.Samurais.Include(s => s.Horse).FirstOrDefault(x => x.Horse == null);
            samurai.Horse = new Horse { Name = "Hoğse" };
            context.SaveChanges();
        }

        private static void AddNewHorseToSamuraiUsingSamuraiId()
        {
            var horse = new Horse { Name = "A-horse", SamuraiId = 2 };
            context.Add(horse);
            context.SaveChanges();

        }

        private static void AddNewSamuraiWithHorse()
        {
            var samurai = new Samurai
            {
                Name = "NewSamurai",
                Horse = new Horse { Name = "NewSamuraisHorse" }
            };

            context.Samurais.Add(samurai);
            context.SaveChanges();
        }

        private static void GetSamuraiWithBattles()
        {
            var samuraiWithBattle = context.Samurais
                .Include(s => s.BattlesFought)
                .ThenInclude(sb => sb.Battle)
                .FirstOrDefault(s => s.Id == 1);

            var samuraiWithBattle2 = context.Samurais.Where(s => s.Id == 1)
                .Select(s => new
                {
                    Samurai = s,
                    Battles = s.BattlesFought.Select(bf => bf.Battle)

                }).ToList();

            //var samurai = context.Samurais.Find(1);

            //context.Entry(samurai).Collection(s => s.BattlesFought).Load();


            var samuraiWithBattle3 = context.SamuraiBattles.Where(sb => sb.SamuraiId == 1).ToList();

            Console.WriteLine();

        }

        private static void EnlistSamuraiIntoABattle()
        {
            var battle = context.Battles.FirstOrDefault();
            battle.SamuraisInBattles
                .Add(new SamuraiBattle { SamuraiId = 2 });//do not need to specify battle id because of context tracks battle
            context.SaveChanges();
        }

        private static void JoinBattleAndSamurai()
        {
            var sbjoin = new SamuraiBattle { SamuraiId = 1, BattleId = 1 };
            context.SamuraiBattles.Add(sbjoin);//if we dont have db set of join table just use context.Add(sbjoin)
            context.SaveChanges();

        }

        private static void ModifyingRelatedDataWhenNotTracked()
        {
            var samurai1 = context.Samurais.Include(s => s.Quotes).FirstOrDefault(s => s.Quotes.Count() > 3);
            samurai1.Quotes[0].Text += " did you hear that again?";
            var quote = samurai1.Quotes[0];

            using (var context2 = new SamuraiContext())
            {
                //context2.Quotes.Update(quote);
                context2.Entry(quote).State = EntityState.Modified;
                context2.SaveChanges();
            }
        }

        private static void ModifyingRelatedDataWhenTracked()
        {
            var samurai1 = context.Samurais.Include(s => s.Quotes).FirstOrDefault(s => s.Quotes.Count() > 3);
            samurai1.Quotes[0].Text += "did you hear that";
            context.SaveChanges();

            Console.WriteLine();
        }

        private static void LazyLoading()
        {
            var samurai = context.Samurais.Find(1);
            var quotes = samurai.Quotes.ToList();

            Console.WriteLine();
        }

        private static void ExplicitLoadQuotes()
        {
            var samurai = context.Samurais.Find(1);
            context.Entry(samurai).Collection(s => s.Quotes).Load();
            context.Entry(samurai).Reference(s => s.Horse).Load();
            context.Entry(samurai).Reference(s => s.Clan).Load();

            var samurai2 = context.Samurais.FirstOrDefault(s => EF.Functions.Like(s.Name, "Son"));
            context.Entry(samurai2)
                .Collection(s => s.Quotes)
                .Query()
                .Where(q => EF.Functions.Like(q.Text.ToLower(), "weck".ToLower()))
                .ToList();

        }

        private static void ProjectSomeProperties()
        {
            var samurais = context.Samurais
                .Select(s => new ProjectionModel()
                {
                    Id = s.Id,
                    Name = s.Name,
                    Quotes = s.Quotes
                }).ToList();

            var samurais2 = context.Samurais
             .Select(s => new ProjectionModel(s.Id, s.Name, s.Quotes)).ToList();
        }

        private static void ProjectSamuraisWithQuotes()
        {
            var somePropsWQuotes1 = context.Samurais.
                Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.Quotes.Count

                }).ToList();

            var somePropsWQuotes2 = context.Samurais
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    WeckQuotes = s.Quotes.Where(q => q.Text.Contains("weck2"))
                }).ToList();

            var somePropsWQuotes = context.Samurais
                .Select(s => new
                {
                    Samurai = s,
                    WeckQuotes = s.Quotes.Where(q => q.Text.Contains("weck2"))
                }).ToList();
            somePropsWQuotes[0].Samurai.Name += "changed";
            Console.WriteLine();
        }

        private static void EagerLoadSamuraiWithQuotes()
        {
            var samurais = context.Samurais.Include(s => s.Quotes);
            var samurais2 = context.Samurais.Include(s => s.Clan)
                                            .Include(s => s.BattlesFought)
                                            .Include(s => s.Quotes);
        }

        private static void AddQuoteToExistingSamuraiWhileNotTrackedEasy(int samuraiId)
        {
            var qu = new Quote
            {
                Text = "New2",
                SamuraiId = samuraiId
            };
            using (var newContext = new SamuraiContext())
            {
                newContext.Quotes.Add(qu);
                newContext.SaveChanges();
            }
        }

        private static void AddQuoteToExistingSamuraiWhileNotTracked(int samuraiId)
        {
            var samurai = context.Samurais.Find(samuraiId);
            samurai.Quotes.Add(new Quote { Text = "New2" });
            using (var newContext = new SamuraiContext())
            {
                //newContext.Samurais.Update(samurai);//starts tracking and unnecessary update instead of it use attach
                newContext.Samurais.Attach(samurai);//attach will set samurai entity state unchanged and using with like this it will only add the quote to db 
                newContext.SaveChanges();
            }
        }

        private static void AddQuoteToExistingSamuraiWhileTracked()
        {
            var samurai = context.Samurais.FirstOrDefault();
            samurai.Quotes.Add(new Quote { Text = "new" });
            context.SaveChanges();
        }

        private static void InserNewSamuraiWithAQuote()
        {
            var samurai = new Samurai()
            {
                Name = "Sam",
                Quotes = new List<Quote>()
                {
                   new Quote(){Text = "thats weck i am sam"}
                }
            };
            context.Samurais.Add(samurai);
            context.SaveChanges();
        }

        private static void InserNewSamuraiWithManyQuote()
        {
            var samurai = new Samurai()
            {
                Name = "Sam",
                Quotes = new List<Quote>()
                {
                   new Quote(){Text = "thats weck i am sam"},
                   new Quote(){Text = "thats weck1 i am sam"},
                   new Quote(){Text = "thats weck2 i am sam"},
                   new Quote(){Text = "thats weck3 i am sam"}
                }
            };
            context.Samurais.Add(samurai);
            context.SaveChanges();
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