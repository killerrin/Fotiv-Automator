using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Migrations
{
    [Migration(1)]
    public class _001_InitialSetup : Migration
    {
        public override void Up()
        {
            #region Users, Roles, Activity
            Create.Table("users")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("username").AsString(128)
                .WithColumn("email").AsCustom("VARCHAR(256)")
                .WithColumn("password_hash").AsString(128)
                .WithColumn("password_expiry").AsDateTime().Nullable();

            Create.Table("roles")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("name").AsString(128);

            Create.Table("user_roles")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("user_id").AsInt32().ForeignKey("users", "id").OnDelete(Rule.Cascade)
                .WithColumn("role_id").AsInt32().ForeignKey("roles", "id").OnDelete(Rule.Cascade);


            Create.Table("user_activity")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("user_id").AsInt32().ForeignKey("users", "id").OnDelete(Rule.Cascade)
                .WithColumn("last_active").AsDateTime();
            #endregion

            #region Game, Sectors
            Create.Table("games")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("name").AsString(128)
                .WithColumn("description").AsCustom("TEXT").Nullable()
                .WithColumn("opened_to_public").AsBoolean();

            Create.Table("sectors")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("name").AsString(128)
                .WithColumn("notes").AsCustom("TEXT").Nullable()
                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();

            Create.Table("game_sectors")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("sector_id").AsInt32().ForeignKey("sectors", "id").OnDelete(Rule.Cascade);

            Create.Table("game_users")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("user_id").AsInt32().ForeignKey("users", "id").OnDelete(Rule.Cascade)
                .WithColumn("game_id").AsInt32().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("is_gm").AsBoolean();

            #endregion

            #region Star System, Wormholes
            Create.Table("starsystems")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("sector_id").AsInt32().ForeignKey("sectors", "id").OnDelete(Rule.Cascade)
                .WithColumn("hex_x").AsInt32()
                .WithColumn("hex_y").AsInt32();

            Create.Table("wormholes")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("system_id_one").AsInt32().ForeignKey("starsystems", "id").OnDelete(Rule.Cascade)
                .WithColumn("system_id_two").AsInt32().ForeignKey("starsystems", "id").OnDelete(Rule.Cascade)
                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();
            #endregion

            #region Star, System Body, Tier
            Create.Table("stars")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("starsystem_id").AsInt32().ForeignKey("starsystems", "id").OnDelete(Rule.Cascade)
                .WithColumn("name").AsString(128)
                .WithColumn("age").AsString(128)
                .WithColumn("radiation_level").AsString(128)
                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();

            Create.Table("planet_tiers")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("name").AsString(128)
                .WithColumn("build_rate").AsInt32();

            Create.Table("planets")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("star_id").AsInt32().ForeignKey("stars", "id").OnDelete(Rule.Cascade)
                .WithColumn("orbiting_planet_id").AsInt32().Nullable().ForeignKey("planets", "id").OnDelete(Rule.SetNull)
                .WithColumn("planet_tier_id").AsInt32().Nullable().ForeignKey("planet_tiers", "id").OnDelete(Rule.SetNull)
                .WithColumn("name").AsString(128)
                .WithColumn("resources").AsInt32()
                .WithColumn("supports_colonies").AsBoolean()
                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();
            #endregion

            #region Infrastructure, Infrastructure Upgrades
            Create.Table("infrastructure")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()

                .WithColumn("name").AsString(128)
                .WithColumn("description").AsCustom("TEXT").Nullable()
                .WithColumn("rp_cost").AsInt32()

                .WithColumn("is_colony").AsBoolean()
                .WithColumn("is_military").AsBoolean()

                .WithColumn("base_health").AsInt32()
                .WithColumn("base_attack").AsInt32()
                .WithColumn("influence").AsInt32()

                .WithColumn("rp_bonus").AsInt32()
                .WithColumn("science_bonus").AsInt32()
                .WithColumn("ship_construction_bonus").AsInt32()
                .WithColumn("colonial_development_bonus").AsInt32()

                .WithColumn("research_slot").AsBoolean()
                .WithColumn("ship_construction_slot").AsBoolean()
                .WithColumn("colonial_development_slot").AsBoolean()

                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();

            Create.Table("infrastructure_upgrades")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("from_infra_id").AsInt32().ForeignKey("infrastructure", "id").OnDelete(Rule.Cascade)
                .WithColumn("to_infra_id").AsInt32().ForeignKey("infrastructure", "id").OnDelete(Rule.Cascade);
            #endregion

            #region Civilization, Civilization Player, Jump Gates Civilization Infrastructure, Star Systems Visited
            Create.Table("civilization")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("name").AsString(128)
                .WithColumn("colour").AsString(128)
                .WithColumn("rp").AsInt32()
                .WithColumn("notes").AsCustom("TEXT").Nullable()
                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();

            Create.Table("civilization_infrastructure")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("civilization_id").AsInt32().ForeignKey("civilization", "id").OnDelete(Rule.Cascade)
                .WithColumn("planet_id").AsInt32().ForeignKey("planets", "id").OnDelete(Rule.Cascade)
                .WithColumn("struct_id").AsInt32().ForeignKey("infrastructure", "id").OnDelete(Rule.Cascade)
                
                .WithColumn("name").AsString(128)
                .WithColumn("description").AsCustom("TEXT").Nullable()

                .WithColumn("build_percentage").AsInt32()
                .WithColumn("current_health").AsInt32()

                .WithColumn("can_upgrade").AsBoolean()
                .WithColumn("is_military").AsBoolean()

                .WithColumn("notes").AsCustom("TEXT").Nullable()
                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();

            Create.Table("jumpgates")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("civ_struct_id").AsInt32().ForeignKey("civilization_infrastructure", "id").OnDelete(Rule.Cascade)
                .WithColumn("from_system_id").AsInt32().ForeignKey("starsystems", "id").OnDelete(Rule.Cascade)
                .WithColumn("to_system_id").AsInt32().ForeignKey("starsystems", "id").OnDelete(Rule.Cascade)
                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();

            Create.Table("visited_starsystems")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("civilization_id").AsInt32().ForeignKey("civilization", "id").OnDelete(Rule.Cascade)
                .WithColumn("starsystem_id").AsInt32().ForeignKey("starsystems", "id").OnDelete(Rule.Cascade);

            Create.Table("user_civilizations")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("user_id").AsInt32().ForeignKey("users", "id").OnDelete(Rule.Cascade)
                .WithColumn("civilization_id").AsInt32().ForeignKey("civilization", "id").OnDelete(Rule.Cascade);

            Create.Table("game_civilizations")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("civilization_id").AsInt32().ForeignKey("civilization", "id").OnDelete(Rule.Cascade);
            #endregion

            #region Research, Civilization Research
            Create.Table("research")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("rp_cost").AsInt32()

                .WithColumn("name").AsString(128)
                .WithColumn("description").AsCustom("TEXT").Nullable()

                .WithColumn("attack_bonus").AsInt32()
                .WithColumn("health_bonus").AsInt32()

                .WithColumn("science_bonus").AsInt32()
                .WithColumn("colonial_development_bonus").AsInt32()
                .WithColumn("ship_construction_bonus").AsInt32()

                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();

            Create.Table("civilization_research")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("civilization_id").AsInt32().ForeignKey("civilization", "id").OnDelete(Rule.Cascade)
                .WithColumn("research_id").AsInt32().ForeignKey("research", "id").OnDelete(Rule.Cascade)
                .WithColumn("build_percentage").AsInt32();
           
            #endregion

            #region Character, Species, Civilization Species, Civilization Characters
            Create.Table("species")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("name").AsString(128)
                .WithColumn("notes").AsCustom("TEXT").Nullable()
                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();

            Create.Table("civilization_species")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("civilization_id").AsInt32().ForeignKey("civilization", "id").OnDelete(Rule.Cascade)
                .WithColumn("species_id").AsInt32().ForeignKey("species", "id").OnDelete(Rule.Cascade);

            Create.Table("game_species")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("species_id").AsInt32().ForeignKey("species", "id").OnDelete(Rule.Cascade);

            Create.Table("user_species")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("user_id").AsInt32().ForeignKey("users", "id").OnDelete(Rule.Cascade)
                .WithColumn("species_id").AsInt32().ForeignKey("species", "id").OnDelete(Rule.Cascade);

            Create.Table("characters")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("species_id").AsInt32().ForeignKey("species", "id").OnDelete(Rule.Cascade)
                .WithColumn("starsystem_id").AsInt32().ForeignKey("starsystems", "id").OnDelete(Rule.Cascade)

                .WithColumn("name").AsString(128)
                .WithColumn("job").AsString(128).Nullable()
                .WithColumn("status").AsString(128).Nullable()

                .WithColumn("health").AsInt32()
                .WithColumn("attack").AsInt32()
                .WithColumn("influence").AsInt32()

                .WithColumn("admiral_bonus").AsInt32()
                .WithColumn("science_bonus").AsInt32()
                .WithColumn("colonial_development_bonus").AsInt32()
                .WithColumn("ship_construction_bonus").AsInt32()

                .WithColumn("notes").AsCustom("TEXT").Nullable()
                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();

            Create.Table("civilization_characters")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("civilization_id").AsInt32().ForeignKey("civilization", "id").OnDelete(Rule.Cascade)
                .WithColumn("character_id").AsInt32().ForeignKey("characters", "id").OnDelete(Rule.Cascade);
            #endregion

            #region Ship, Ship Rate, Player Ships, Ship Battle Groups, Character Ships
            Create.Table("ship_rates")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("name").AsString(128)
                .WithColumn("build_rate").AsInt32();

            Create.Table("ship_battlegroups")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("name").AsString(128)
                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();

            Create.Table("ships")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("ship_rate_id").AsInt32().Nullable().ForeignKey("ship_rates", "id").OnDelete(Rule.SetNull)

                .WithColumn("name").AsString(128)
                .WithColumn("description").AsCustom("TEXT").Nullable()

                .WithColumn("rp_cost").AsInt32()
                .WithColumn("base_health").AsInt32()
                .WithColumn("base_attack").AsInt32()
                .WithColumn("maximum_fighters").AsInt32()
                .WithColumn("num_build").AsInt32()
                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();

            Create.Table("civilization_ships")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("ship_battlegroup_id").AsInt32().Nullable().ForeignKey("ship_battlegroups", "id").OnDelete(Rule.SetNull)
                .WithColumn("ship_id").AsInt32().ForeignKey("ships", "id").OnDelete(Rule.Cascade)
                .WithColumn("civilization_id").AsInt32().ForeignKey("civilization", "id").OnDelete(Rule.Cascade)
                .WithColumn("starsystem_id").AsInt32().ForeignKey("starsystems", "id").OnDelete(Rule.Cascade)
                .WithColumn("build_percentage").AsInt32()
                .WithColumn("current_health").AsInt32()
                .WithColumn("command_and_control").AsBoolean()
                .WithColumn("notes").AsCustom("TEXT").Nullable()
                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();

            Create.Table("civ_ship_characters")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("civ_ship_id").AsInt32().ForeignKey("civilization_ships", "id").OnDelete(Rule.Cascade)
                .WithColumn("character_id").AsInt32().ForeignKey("characters", "id").OnDelete(Rule.Cascade);
            #endregion


            #region Insert Default Data
            Insert.IntoTable("roles")
                .Row(new { name = "Admin" });

            Insert.IntoTable("users")
                .Row(new { username = "Admin", email = "example@example.com", password_hash = "$2a$13$Yc09ninVjS1GWYsbt5taF.8gmM6zz3frAFHUbD0d78wIHC/it8GqO" });
            #endregion
        }

        public override void Down()
        {
            Delete.FromTable("roles").Row(new { name = "Admin" });
            Delete.FromTable("users").Row(new { username = "Admin" });

            Delete.Table("user_activity");
            Delete.Table("user_roles");
            Delete.Table("user_species");
            Delete.Table("user_civilizations");
            Delete.Table("game_users");
            Delete.Table("game_sectors");
            Delete.Table("game_species");
            Delete.Table("game_civilizations");
            Delete.Table("civilization_research");
            Delete.Table("civilization_characters");
            Delete.Table("civilization_species");
            Delete.Table("civ_ship_characters");
            Delete.Table("visited_starsystems");
            Delete.Table("infrastructure_upgrades");

            Delete.Table("jumpgates");
            Delete.Table("wormholes");

            Delete.Table("roles");
            Delete.Table("users");
            Delete.Table("games");

            Delete.Table("research");
            Delete.Table("civilization_infrastructure");
            Delete.Table("infrastructure");

            Delete.Table("planets");
            Delete.Table("planet_tiers");
            Delete.Table("stars");

            Delete.Table("characters");
            Delete.Table("species");

            Delete.Table("civilization_ships");
            Delete.Table("ship_battlegroups");
            Delete.Table("ships");
            Delete.Table("ship_rates");

            Delete.Table("civilization");
            Delete.Table("starsystems");
            Delete.Table("sectors");
        }
    }
}
