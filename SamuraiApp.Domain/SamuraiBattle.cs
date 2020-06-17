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
        public virtual Battle Battle { get; set; }
        public virtual Samurai Samurai { get; set; }
    }
}
