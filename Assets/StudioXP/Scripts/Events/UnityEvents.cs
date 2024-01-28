using System;
using StudioXP.Scripts.Character;
using StudioXP.Scripts.Characters;
using StudioXP.Scripts.Characters.Actions;
using StudioXP.Scripts.Objects;
using UnityEngine;
using UnityEngine.Events;

namespace StudioXP.Scripts.Events
{
    [Serializable]
    public class FloatEvent : UnityEvent<float>
    {
    }

    [Serializable]
    public class IntEvent : UnityEvent<int>
    {
    }

    [Serializable]
    public class Vector2Event : UnityEvent<Vector2>
    {
    }
    
    [Serializable]
    public class Vector3Event : UnityEvent<Vector3>
    {
    }
    
    [Serializable]
    public class QuaternionEvent : UnityEvent<Quaternion>
    {
    }

    [Serializable]
    public class RectEvent : UnityEvent<Rect>
    {
    }

    [Serializable]
    public class GameObjectEvent : UnityEvent<GameObject>
    {
    }

    [Serializable]
    public class InteractActionEvent : UnityEvent<CharacterAction>
    {
        
    }

    [Serializable]
    public class InteractableEvent : UnityEvent<Interactable>
    {
        
    }

    [Serializable]
    public class PickableEvent : UnityEvent<Pickable>
    {

    }

}
