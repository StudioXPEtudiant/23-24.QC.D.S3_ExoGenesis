using StudioXP.Scripts.Objects;
using UnityEngine;

namespace StudioXP.Scripts.Characters.Actions
{
    public class AttackAction : CharacterAction
    {
        [SerializeField] private int damage = 1;
        
        public override bool Execute(Interactable interactable)
        {
            if (!interactable) return false;
            
            var health = interactable.GetComponent<Health>();
            if (!health) return false;
            
            health.Decrease(damage);
            return true;
        }
    }
}
