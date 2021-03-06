﻿using FluentMigrator;
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
                .WithColumn("turn_number").AsInt32()
                .WithColumn("opened_to_public").AsBoolean();

            Create.Table("sectors")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().ForeignKey("games", "id").OnDelete(Rule.Cascade)

                .WithColumn("name").AsString(128)
                .WithColumn("description").AsCustom("TEXT").Nullable()
                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();

            Create.Table("game_users")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("user_id").AsInt32().ForeignKey("users", "id").OnDelete(Rule.Cascade)
                .WithColumn("game_id").AsInt32().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("is_gm").AsBoolean();

            #endregion

            #region Star System, Wormholes
            Create.Table("starsystems")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("sector_id").AsInt32().ForeignKey("sectors", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("hex_x").AsInt32()
                .WithColumn("hex_y").AsInt32()
                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();

            Create.Table("wormholes")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("system_id_one").AsInt32().ForeignKey("starsystems", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("system_id_two").AsInt32().ForeignKey("starsystems", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();
            #endregion

            #region Starmap
            Create.Table("star_types")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().Nullable().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("name").AsString(128);

            Create.Table("star_ages")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().Nullable().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("name").AsString(128);

            Create.Table("radiation_levels")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().Nullable().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("name").AsString(128);

            Create.Table("stars")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("starsystem_id").AsInt32().ForeignKey("starsystems", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("star_type_id").AsInt32().Nullable().ForeignKey("star_types", "id")//.OnDelete(Rule.SetNull)
                .WithColumn("star_age_id").AsInt32().Nullable().ForeignKey("star_ages", "id")//.OnDelete(Rule.SetNull)
                .WithColumn("radiation_level_id").AsInt32().Nullable().ForeignKey("radiation_levels", "id")//.OnDelete(Rule.SetNull)
                .WithColumn("name").AsString(128)
                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();

            Create.Table("planet_tiers")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().Nullable().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("name").AsString(128)
                .WithColumn("build_rate").AsInt32();

            Create.Table("planet_types")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().Nullable().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("name").AsString(128);

            Create.Table("stage_of_life")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().Nullable().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("name").AsString(128);

            Create.Table("planets")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("star_id").AsInt32().ForeignKey("stars", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("orbiting_planet_id").AsInt32().Nullable().ForeignKey("planets", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("planet_tier_id").AsInt32().Nullable().ForeignKey("planet_tiers", "id")//.OnDelete(Rule.SetNull)
                .WithColumn("planet_type_id").AsInt32().Nullable().ForeignKey("planet_types", "id")//.OnDelete(Rule.SetNull)
                .WithColumn("stage_of_life_id").AsInt32().Nullable().ForeignKey("stage_of_life", "id")//.OnDelete(Rule.SetNull)
                .WithColumn("name").AsString(128)
                .WithColumn("resources").AsInt32()
                .WithColumn("supports_colonies").AsBoolean()
                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();
            #endregion

            #region Infrastructure, Infrastructure Upgrades
            Create.Table("infrastructure")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().Nullable().ForeignKey("games", "id").OnDelete(Rule.Cascade)

                .WithColumn("name").AsString(128)
                .WithColumn("description").AsCustom("TEXT").Nullable()
                .WithColumn("rp_cost").AsInt32()

                .WithColumn("is_colony").AsBoolean()
                .WithColumn("is_military").AsBoolean()

                .WithColumn("base_health").AsInt32()
                .WithColumn("base_regeneration").AsInt32()
                .WithColumn("base_attack").AsInt32()
                .WithColumn("base_special_attack").AsInt32()
                .WithColumn("base_agility").AsInt32()

                .WithColumn("influence_bonus").AsInt32()

                .WithColumn("rp_bonus").AsInt32()
                .WithColumn("science_bonus").AsInt32()
                .WithColumn("ship_construction_bonus").AsInt32()
                .WithColumn("colonial_development_bonus").AsInt32()
                .WithColumn("unit_training_bonus").AsInt32()

                .WithColumn("research_slots").AsInt32()
                .WithColumn("ship_construction_slots").AsInt32()
                .WithColumn("colonial_development_slots").AsInt32()
                .WithColumn("unit_training_slots").AsInt32()

                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();

            Create.Table("infrastructure_upgrades")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().Nullable().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("from_infra_id").AsInt32().ForeignKey("infrastructure", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("to_infra_id").AsInt32().ForeignKey("infrastructure", "id");//.OnDelete(Rule.Cascade);
            #endregion

            #region Civilization, Civilization Traits, Civilization Player, Jump Gates, Civilization Infrastructure, Star Systems Visited
            Create.Table("civilization_traits")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().Nullable().ForeignKey("games", "id").OnDelete(Rule.Cascade)

                .WithColumn("name").AsString(128)
                .WithColumn("description").AsCustom("TEXT").Nullable()

                .WithColumn("influence_bonus").AsInt32()
                .WithColumn("trade_bonus").AsInt32()

                .WithColumn("apply_military").AsBoolean()
                .WithColumn("apply_units").AsBoolean()
                .WithColumn("apply_ships").AsBoolean()
                .WithColumn("apply_infrastructure").AsBoolean()

                .WithColumn("science_bonus").AsInt32()
                .WithColumn("colonial_development_bonus").AsInt32()
                .WithColumn("ship_construction_bonus").AsInt32()
                .WithColumn("unit_training_bonus").AsInt32();

            Create.Table("tech_levels")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().Nullable().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("name").AsString(128)
                .WithColumn("attack_detriment").AsInt32();

            Create.Table("civilization")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().ForeignKey("games", "id").OnDelete(Rule.Cascade)

                .WithColumn("civilization_traits_1_id").AsInt32().Nullable().ForeignKey("civilization_traits", "id")//.OnDelete(Rule.SetNull)
                .WithColumn("civilization_traits_2_id").AsInt32().Nullable().ForeignKey("civilization_traits", "id")//.OnDelete(Rule.SetNull)
                .WithColumn("civilization_traits_3_id").AsInt32().Nullable().ForeignKey("civilization_traits", "id")//.OnDelete(Rule.SetNull)
                .WithColumn("tech_level_id").AsInt32().Nullable().ForeignKey("tech_levels", "id")//.OnDelete(Rule.SetNull)

                .WithColumn("name").AsString(128)
                .WithColumn("colour").AsString(128)
                .WithColumn("rp").AsInt32()
                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();

            Create.Table("civilization_met")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().ForeignKey("games", "id").OnDelete(Rule.Cascade)

                .WithColumn("civilization_id1").AsInt32().Nullable().ForeignKey("civilization_traits", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("civilization_id2").AsInt32().Nullable().ForeignKey("civilization_traits", "id");//.OnDelete(Rule.Cascade);

            Create.Table("civilization_infrastructure")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("civilization_id").AsInt32().ForeignKey("civilization", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("planet_id").AsInt32().ForeignKey("planets", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("struct_id").AsInt32().ForeignKey("infrastructure", "id")//.OnDelete(Rule.Cascade)
                
                .WithColumn("name").AsString(128)
                .WithColumn("current_health").AsInt32()
                .WithColumn("experience").AsInt32()
                .WithColumn("can_upgrade").AsBoolean()
                .WithColumn("is_military").AsBoolean()

                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();

            Create.Table("jumpgates")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("civ_struct_id").AsInt32().ForeignKey("civilization_infrastructure", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("from_system_id").AsInt32().ForeignKey("starsystems", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("to_system_id").AsInt32().ForeignKey("starsystems", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();

            Create.Table("visited_starsystems")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("civilization_id").AsInt32().ForeignKey("civilization", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("starsystem_id").AsInt32().ForeignKey("starsystems", "id");//.OnDelete(Rule.Cascade);

            Create.Table("user_civilizations")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("game_user_id").AsInt32().ForeignKey("game_users", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("user_id").AsInt32().ForeignKey("users", "id").OnDelete(Rule.Cascade)
                .WithColumn("civilization_id").AsInt32().ForeignKey("civilization", "id");//.OnDelete(Rule.Cascade);
            #endregion

            #region Research, Civilization Research
            Create.Table("research")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().Nullable().ForeignKey("games", "id").OnDelete(Rule.Cascade)

                .WithColumn("name").AsString(128)
                .WithColumn("description").AsCustom("TEXT").Nullable()
                .WithColumn("rp_cost").AsInt32()

                .WithColumn("apply_military").AsBoolean()
                .WithColumn("apply_units").AsBoolean()
                .WithColumn("apply_ships").AsBoolean()
                .WithColumn("apply_infrastructure").AsBoolean()

                .WithColumn("domestic_influence_bonus").AsInt32()
                .WithColumn("foreign_influence_bonus").AsInt32()

                .WithColumn("attack_bonus").AsInt32()
                .WithColumn("special_attack_bonus").AsInt32()
                .WithColumn("health_bonus").AsInt32()
                .WithColumn("regeneration_bonus").AsInt32()
                .WithColumn("agility_bonus").AsInt32()

                .WithColumn("science_bonus").AsInt32()
                .WithColumn("colonial_development_bonus").AsInt32()
                .WithColumn("ship_construction_bonus").AsInt32()
                .WithColumn("unit_training_bonus").AsInt32()

                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();

            Create.Table("civilization_research")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("civilization_id").AsInt32().ForeignKey("civilization", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("research_id").AsInt32().ForeignKey("research", "id");//.OnDelete(Rule.Cascade);
            #endregion

            #region Species, Civilization Species
            Create.Table("species")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                
                .WithColumn("game_id").AsInt32().Nullable().ForeignKey("games", "id").OnDelete(Rule.Cascade)

                .WithColumn("name").AsString(128)
                .WithColumn("description").AsCustom("TEXT").Nullable()
                
                .WithColumn("base_attack").AsInt32()
                .WithColumn("base_special_attack").AsInt32()
                .WithColumn("base_health").AsInt32()
                .WithColumn("base_regeneration").AsInt32()
                .WithColumn("base_agility").AsInt32()

                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();

            Create.Table("civilization_species")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("civilization_id").AsInt32().ForeignKey("civilization", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("species_id").AsInt32().ForeignKey("species", "id");//.OnDelete(Rule.Cascade);
            #endregion

            #region Unit, Unit Category, Civilization Units, Battlegroups
            Create.Table("experience_levels")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("name").AsString(128)
                .WithColumn("threshold").AsInt32()
                .WithColumn("attack_bonus").AsInt32()
                .WithColumn("special_attack_bonus").AsInt32()
                .WithColumn("health_bonus").AsInt32()
                .WithColumn("regeneration_bonus").AsInt32()
                .WithColumn("agility_bonus").AsInt32();

            Create.Table("unit_categories")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().Nullable().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("name").AsString(128)
                .WithColumn("build_rate").AsInt32()
                .WithColumn("is_military").AsBoolean();

            Create.Table("civilization_battlegroups")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                .WithColumn("civilization_id").AsInt32().ForeignKey("civilization", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("starsystem_id").AsInt32().ForeignKey("starsystems", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("name").AsString(128)
                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();

            Create.Table("units")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().Nullable().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                
                .WithColumn("unit_category_id").AsInt32().Nullable().ForeignKey("unit_categories", "id")//.OnDelete(Rule.SetNull)

                .WithColumn("name").AsString(128)
                .WithColumn("unit_type").AsString(128)
                .WithColumn("description").AsCustom("TEXT").Nullable()
                .WithColumn("rp_cost").AsInt32()
                .WithColumn("number_to_build").AsInt32()

                .WithColumn("can_embark").AsBoolean()
                .WithColumn("can_attack_ground_units").AsBoolean()
                .WithColumn("can_attack_boats").AsBoolean()
                .WithColumn("can_attack_planes").AsBoolean()
                .WithColumn("can_attack_spaceships").AsBoolean()

                .WithColumn("embarking_slots").AsInt32()
                .WithColumn("negate_damage").AsInt32()

                .WithColumn("base_attack").AsInt32()
                .WithColumn("base_special_attack").AsInt32()
                .WithColumn("base_health").AsInt32()
                .WithColumn("base_regeneration").AsInt32()
                .WithColumn("base_agility").AsInt32()

                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();

            Create.Table("civ_units")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().ForeignKey("games", "id").OnDelete(Rule.Cascade)
                
                .WithColumn("civilization_id").AsInt32().ForeignKey("civilization", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("unit_id").AsInt32().ForeignKey("units", "id")//.OnDelete(Rule.Cascade)
                
                .WithColumn("group_id").AsInt32().Nullable().ForeignKey("civilization_battlegroups", "id")//.OnDelete(Rule.SetNull)
                .WithColumn("species_id").AsInt32().Nullable().ForeignKey("species", "id")//.OnDelete(Rule.Cascade)

                .WithColumn("name").AsString(128)
                .WithColumn("current_health").AsInt32()
                .WithColumn("experience").AsInt32()
                .WithColumn("gmnotes").AsCustom("TEXT").Nullable();
            #endregion

            #region Research And Development
            Create.Table("civ_rnd_research")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().ForeignKey("games", "id").OnDelete(Rule.Cascade)

                .WithColumn("civ_id").AsInt32().ForeignKey("civilization", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("cstruct_id").AsInt32().ForeignKey("civilization_infrastructure", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("research_id").AsInt32().ForeignKey("research", "id")//.OnDelete(Rule.Cascade)

                .WithColumn("build_percentage").AsInt32();

            Create.Table("civ_rnd_units")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().ForeignKey("games", "id").OnDelete(Rule.Cascade)

                .WithColumn("civ_id").AsInt32().ForeignKey("civilization", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("cstruct_id").AsInt32().ForeignKey("civilization_infrastructure", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("unit_id").AsInt32().ForeignKey("units", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("species_id").AsInt32().Nullable().ForeignKey("species", "id")//.OnDelete(Rule.Cascade)

                .WithColumn("name").AsString(128)
                .WithColumn("build_percentage").AsInt32();

            Create.Table("civ_rnd_structure")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("game_id").AsInt32().ForeignKey("games", "id").OnDelete(Rule.Cascade)

                .WithColumn("civ_id").AsInt32().ForeignKey("civilization", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("cstruct_id").AsInt32().ForeignKey("civilization_infrastructure", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("struct_id").AsInt32().ForeignKey("infrastructure", "id")//.OnDelete(Rule.Cascade)
                .WithColumn("planet_id").AsInt32().ForeignKey("planets", "id")//.OnDelete(Rule.Cascade)

                .WithColumn("name").AsString(128)
                .WithColumn("build_percentage").AsInt32();
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
            
            Delete.Table("experience_levels");
            Delete.Table("user_activity");
            Delete.Table("user_roles");
            Delete.Table("user_civilizations");
            Delete.Table("game_users");
            Delete.Table("civ_rnd_research");
            Delete.Table("civ_rnd_units");
            Delete.Table("civ_rnd_structure");
            Delete.Table("civilization_met");
            Delete.Table("civilization_research");
            Delete.Table("civilization_species");
            Delete.Table("visited_starsystems");
            Delete.Table("infrastructure_upgrades");
            
            Delete.Table("jumpgates");
            Delete.Table("wormholes");
            
            Delete.Table("research");
            Delete.Table("civilization_infrastructure");
            Delete.Table("infrastructure");
            
            Delete.Table("planets");
            Delete.Table("planet_tiers");
            Delete.Table("planet_types");
            Delete.Table("stage_of_life");
            Delete.Table("stars");
            Delete.Table("star_types");
            Delete.Table("star_ages");
            Delete.Table("radiation_levels");
                        
            Delete.Table("civ_units");
            Delete.Table("civilization_battlegroups");
            Delete.Table("units");
            Delete.Table("unit_categories");
            
            Delete.Table("species");
            
            Delete.Table("civilization");
            Delete.Table("civilization_traits");
            Delete.Table("tech_levels");
            Delete.Table("starsystems");
            Delete.Table("sectors");
            
            Delete.Table("roles");
            Delete.Table("users");
            Delete.Table("games");
        }
    }
}
