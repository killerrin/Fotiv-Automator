using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_characters
    {
        public virtual int ID { get; set; }
        public virtual int Species_ID { get; set; }
        public virtual int Starsystem_ID { get; set; }

        public virtual string Name { get; set; }
        public virtual string Job { get; set; }
        public virtual string Status { get; set; }
        public virtual string Website { get; set; }

        public virtual int Health { get; set; }
        public virtual int Attack { get; set; }
        public virtual int Influence { get; set; }

        public virtual int Admiral_Bonus { get; set; }
        public virtual int Science_Bonus { get; set; }
        public virtual int Colonial_Development_Bonus { get; set; }
        public virtual int Ship_Construction_Bonus { get; set; }

        public virtual string Notes { get; set; }
        public virtual string GMNotes { get; set; }
    }
}
