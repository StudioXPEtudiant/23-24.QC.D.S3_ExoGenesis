using System.Collections.Generic;
using StudioXP.Scripts.Objects;
using UnityEngine;

namespace StudioXP.Scripts.Characters.Actions
{
    public class HarvestAction : CharacterAction
    {
        [SerializeField] private string[] categories;
        [SerializeField] private int level = 0;
        [SerializeField] private int efficiency = 1;
        
        public override bool Execute(Interactable interactable)
        {
            if (!interactable) return false;
            
            var harvestable = interactable.GetComponent<Harvestable>();
            if (!harvestable) return false;
            
            harvestable.TryHarvest(categories, level, efficiency);
            return true;
        }
    }
}
