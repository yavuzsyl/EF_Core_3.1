using System;
using System.Collections.Generic;
using System.Text;

namespace SamuraiApp.Domain
{
    public class SamuraiBattle
    {
        //foreign keys we can use them as composite key which represents primary key
        public int BattleId { get; set; }
        public int SamuraiId { get; set; }

        //optional
        public Battle Battle { get; set; }
        public Samurai Samurai { get; set; }
    }
}
