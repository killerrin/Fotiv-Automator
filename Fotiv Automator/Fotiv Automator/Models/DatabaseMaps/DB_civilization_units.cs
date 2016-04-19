using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Models.DatabaseMaps
{
    public class DB_civilization_units
    {
        public virtual int id { get; set; }
        public virtual int game_id { get; set; }

        public virtual int civilization_id { get; set; }
        public virtual int unit_id { get; set; }

        public virtual int? battlegroup_id { get; set; }
        public virtual int? species_id { get; set; }

        public virtual string name { get; set; }
        public virtual int current_health { get; set; }
        public virtual int experience { get; set; }
        public virtual string gmnotes { get; set; }

        public virtual DB_civilization_units Clone(bool keepID)
        {
            DB_civilization_units newShip = new DB_civilization_units();
            if (keepID) newShip.id = id;
            newShip.game_id = game_id;

            newShip.civilization_id = civilization_id;
            newShip.unit_id = unit_id;

            newShip.battlegroup_id = battlegroup_id;
            newShip.species_id = species_id;

            newShip.name = name;
            newShip.current_health = current_health;
            newShip.experience = experience;
            newShip.gmnotes = gmnotes;

            return newShip;
        }
    }

    public class MAP_civilization_units : ClassMapping<DB_civilization_units>
    {
        public MAP_civilization_units()
        {
            Table("civ_units");
            Id(x => x.id, x => x.Generator(Generators.Identity));
            Property(x => x.game_id, x => x.NotNullable(true));

            Property(x => x.civilization_id, x => x.NotNullable(true));
            Property(x => x.unit_id, x => x.NotNullable(true));

            Property(x => x.battlegroup_id, x => {
                x.Column("group_id");
                x.NotNullable(false);
                });

            Property(x => x.species_id, x => x.NotNullable(false));

            Property(x => x.name, x => x.NotNullable(true));
            Property(x => x.current_health, x => x.NotNullable(true));
            Property(x => x.experience, x => x.NotNullable(true));
            Property(x => x.gmnotes, x => x.NotNullable(false));
        }
    }
}
