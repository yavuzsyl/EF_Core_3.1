using SamuraiApp.Domain;
using System.Collections.Generic;

namespace ConsoleApp
{
    internal class ProjectionModel
    {
        public ProjectionModel(int id ,string name ,List<Quote> quotes)
        {
            Id = id;
            Name = name;
            Quotes = quotes;
        }
        public ProjectionModel()
        {

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Quote> Quotes { get; set; }
    }
}