using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_game_species
    {
        public virtual int id { get; set; }

        public virtual int game_id { get; set; }
        public virtual int species_id { get; set; }
    }

    public class MAP_game_species : ClassMapping<DB_game_species>
    {
        public MAP_game_species()
        {
            Table("game_species");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.game_id, x => x.NotNullable(true));
            Property(x => x.species_id, x => x.NotNullable(true));
        }
    }
}
