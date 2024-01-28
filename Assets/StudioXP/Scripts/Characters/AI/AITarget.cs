using System;
using System.Collections.Generic;
using StudioXP.Scripts.Character;
using UnityEngine;

namespace StudioXP.Scripts.Characters.AI
{
    [RequireComponent(typeof(Health))]
    public class AITarget : MonoBehaviour
    {
        public static HashSet<AITarget> Targets { get; } = new();
        
        [SerializeField] private string category = "Animal";
        [SerializeField] private bool noSmell;
        [SerializeField] private bool invisible;

        public bool NoSmell => noSmell;
        
        public bool Invisible => invisible;
        
        public string Category { get => category; set => category = value; }
        
        public Health Health { get; private set; }

        private void Awake()
        {
            Targets.Add(this);
            Health = GetComponent<Health>();
        }

        private void OnDisable()
        {
            Targets.Remove(this);
        }
    }
}
