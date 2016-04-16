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
        public virtual int? unit_battlegroup_id { get; set; }
        public virtual int unit_id { get; set; }
        public virtual int species_id { get; set; }

        public virtual string name { get; set; }
        public virtual int build_percentage { get; set; }
        public virtual int current_health { get; set; }
        public virtual int experience { get; set; }
        public virtual string gmnotes { get; set; }

        public virtual DB_civilization_units Clone(bool keepID)
        {
            DB_civilization_units newUnit = new DB_civilization_units();
            if (keepID) newUnit.id = id;
            newUnit.game_id = game_id;
            newUnit.civilization_id = civilization_id;
            newUnit.unit_battlegroup_id = unit_battlegroup_id;
            newUnit.unit_id = unit_id;
            newUnit.species_id = species_id;

            newUnit.name = name;
            newUnit.build_percentage = build_percentage;
            newUnit.current_health = current_health;
            newUnit.experience = experience;
            newUnit.gmnotes = gmnotes;

            return newUnit;
        }
    }

    public class MAP_civilization_units : ClassMapping<DB_civilization_units>
    {
        public MAP_civilization_units()
        {
            Table("civilization_units");
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.game_id, x => x.NotNullable(true));
            Property(x => x.civilization_id, x => x.NotNullable(true));

            Property(x => x.unit_battlegroup_id, x => x.NotNullable(false));
            Property(x => x.unit_id, x => x.NotNullable(true));
            Property(x => x.species_id, x => x.NotNullable(true));

            Property(x => x.name, x => x.NotNullable(false));
            Property(x => x.build_percentage, x => x.NotNullable(true));
            Property(x => x.current_health, x => x.NotNullable(true));
            Property(x => x.experience, x => x.NotNullable(true));
            Property(x => x.gmnotes, x => x.NotNullable(false));
        }
    }
}
