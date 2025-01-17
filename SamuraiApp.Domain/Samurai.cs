﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SamuraiApp.Domain
{
    public class Samurai
    {
        public Samurai()
        {
            Quotes = new List<Quote>();

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Quote> Quotes { get; set; }
        public virtual Clan Clan { get; set; }
        public virtual Horse Horse { get; set; }
        public virtual List<SamuraiBattle> BattlesFought { get; set; }

    }
}
