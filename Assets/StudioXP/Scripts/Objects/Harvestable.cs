using System.Collections.Generic;
using System.Linq;
using StudioXP.Scripts.Events;
using UnityEngine;
using UnityEngine.Events;

namespace StudioXP.Scripts.Objects
{
    public class Harvestable : MonoBehaviour
    {
        [SerializeField] private string category;
        [SerializeField] private int minLevel = 1;
        
        [SerializeField] private IntEvent harvested;

        public void TryHarvest(IEnumerable<string> categories, int level, int efficiency)
        {
            if(categories.Contains(category) && level >= minLevel)
                harvested.Invoke(efficiency);
        }
    }
}
