using Fotiv_Automator.Models.DatabaseMaps;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.Models.Game
{
    public class CivilizationShip
    {
        public DB_civilization_ships Info;

        public Ship Ship;

        public DB_ship_battlegroups BattlegroupInfo;
        public List<DB_characters> CharactersInfo = new List<DB_characters>();

        public CivilizationShip(DB_civilization_ships dbCivilizationShip)
        {
            Info = dbCivilizationShip;
            Ship = new Ship();

            QueryBattlegroup();
            QueryShipCharacters();
        }

        public void QueryBattlegroup()
        {
            if (Info.ship_battlegroup_id == null)
                return;

            Debug.WriteLine(string.Format("Civilization Ship: {0}, Getting Battlegroups", Info.id));
            BattlegroupInfo = Database.Session.Query<DB_ship_battlegroups>().Where(x => x.id == Info.ship_battlegroup_id).First();
        }

        public void QueryShipCharacters()
        {
            CharactersInfo = new List<DB_characters>();

            Debug.WriteLine(string.Format("Civilization Ship: {0}, Getting Ship Characters", Info.id));
            var dbCharacters = Database.Session.Query<DB_characters>().ToList();
            var dbShipCharacters = Database.Session.Query<DB_civ_ship_characters>()
                .Where(x => x.civ_ship_id == Info.id)
                .ToList();

            foreach (var dbShipCharacter in dbShipCharacters)
            {
                CharactersInfo.Add(dbCharacters.Where(x => x.id == dbShipCharacter.character_id).First());
            }
        }
    }
}
