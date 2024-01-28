using System;
using System.Collections.Generic;
using StudioXP.Scripts.Objects;
using UnityEngine;

namespace StudioXP.Scripts.Characters.Actions
{
    public abstract class CharacterAction : MonoBehaviour
    {
        public abstract bool Execute(Interactable interactable);
    }
}
