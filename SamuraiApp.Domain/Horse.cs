using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SamuraiApp.Domain
{
    public class Horse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey(nameof(Samurai))]
        public int SamuraiId { get; set; }
        public Samurai Samurai { get; set; }
    }
}
