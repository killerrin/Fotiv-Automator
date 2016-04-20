using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Areas.GamePortal.Models.Game
{
    public class Infrastructure
    {
        public int InfrastructureID { get { return InfrastructureInfo.Infrastructure.id; } }

        public DB_civilization_infrastructure CivilizationInfo;
        public Civilization Owner { get; set; } 

        public Planet Planet;
        public InfrastructureUpgrade InfrastructureInfo;
        public DB_experience_levels ExperienceLevel;

        public Infrastructure(DB_civilization_infrastructure dbCivilizationInfrastructure, Civilization owner)
        {
            CivilizationInfo = dbCivilizationInfrastructure;
            Owner = owner;
        }

        public int CalculateMaxHealth()
        {
            int value = InfrastructureInfo.Infrastructure.base_health;
            foreach (var research in Owner.Assets.CompletedResearch)
                if (research.ResearchInfo.apply_infrastructure)
                    value += research.ResearchInfo.health_bonus;
            return value;
        }

        public int CalculateRegenerationFactor()
        {
            int value = InfrastructureInfo.Infrastructure.base_regeneration;
            foreach (var research in Owner.Assets.CompletedResearch)
                if (research.ResearchInfo.apply_infrastructure)
                    value += research.ResearchInfo.regeneration_bonus;
            return value;
        }

        public int CalculateAttack()
        {
            int value = InfrastructureInfo.Infrastructure.base_attack;
            foreach (var research in Owner.Assets.CompletedResearch)
                if (research.ResearchInfo.apply_infrastructure)
                    value += research.ResearchInfo.attack_bonus;
            return value;
        }

        public int CalculateSpecialAttack()
        {
            int value = InfrastructureInfo.Infrastructure.base_special_attack;
            foreach (var research in Owner.Assets.CompletedResearch)
                if (research.ResearchInfo.apply_infrastructure)
                    value += research.ResearchInfo.special_attack_bonus;
            return value;
        }

        public int CalculateAgility()
        {
            int value = 0;
            foreach (var research in Owner.Assets.CompletedResearch)
                if (research.ResearchInfo.apply_infrastructure)
                    value += research.ResearchInfo.agility_bonus;
            return value;
        }
    }
}
