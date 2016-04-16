using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Fotiv_Automator.Models.DatabaseMaps;

namespace Fotiv_Automator
{
    public static class Database
    {
        private const string SessionKey = "Fotiv_Automator.Database.SessionKey";
        private static ISessionFactory _sessionFactory;

        public static ISession Session
        {
            get { return (ISession)HttpContext.Current.Items[SessionKey]; }
        }

        public static void Configure()
        {
            var config = new Configuration();

            // Configure the Connection String
            config.Configure();

            // Add our Mappings
            var mapper = new ModelMapper();
            #region Migration 001
            mapper.AddMapping<MAP_civilization>();
            mapper.AddMapping<MAP_civilization_infrastructure>();
            mapper.AddMapping<MAP_civilization_research>();
            mapper.AddMapping<MAP_civilization_met>();
            mapper.AddMapping<MAP_civilization_ships>();
            mapper.AddMapping<MAP_civilization_species>();
            mapper.AddMapping<MAP_civilization_traits>();
            mapper.AddMapping<MAP_civilization_units>();
            mapper.AddMapping<MAP_experience_levels>();
            mapper.AddMapping<MAP_game_users>();
            mapper.AddMapping<MAP_games>();
            mapper.AddMapping<MAP_infrastructure>();
            mapper.AddMapping<MAP_infrastructure_upgrades>();
            mapper.AddMapping<MAP_jumpgates>();
            mapper.AddMapping<MAP_planet_tiers>();
            mapper.AddMapping<MAP_planet_types>();
            mapper.AddMapping<MAP_planets>();
            mapper.AddMapping<MAP_radiation_levels>();
            mapper.AddMapping<MAP_research>();
            mapper.AddMapping<MAP_roles>();
            mapper.AddMapping<MAP_sectors>();
            mapper.AddMapping<MAP_ship_battlegroups>();
            mapper.AddMapping<MAP_ship_rates>();
            mapper.AddMapping<MAP_ships>();
            mapper.AddMapping<MAP_species>();
            mapper.AddMapping<MAP_stage_of_life>();
            mapper.AddMapping<MAP_star_ages>();
            mapper.AddMapping<MAP_star_types>();
            mapper.AddMapping<MAP_stars>();
            mapper.AddMapping<MAP_starsystems>();
            mapper.AddMapping<MAP_tech_levels>();
            mapper.AddMapping<MAP_unit_battlegroups>();
            mapper.AddMapping<MAP_units>();
            mapper.AddMapping<MAP_user_activity>();
            mapper.AddMapping<MAP_user_civilizations>();
            mapper.AddMapping<MAP_user_roles>();
            mapper.AddMapping<MAP_users>();
            mapper.AddMapping<MAP_visited_starsystems>();
            mapper.AddMapping<MAP_wormholes>();
            #endregion

            config.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

            // Create our session factory
            _sessionFactory = config.BuildSessionFactory();
        }

        public static void OpenSession()
        {
            HttpContext.Current.Items[SessionKey] = _sessionFactory.OpenSession();
        }

        public static void CloseSession()
        {
            var session = HttpContext.Current.Items[SessionKey] as ISession;
            if (session != null)
                session.Close();

            HttpContext.Current.Items.Remove(SessionKey);
        }
    }
}
