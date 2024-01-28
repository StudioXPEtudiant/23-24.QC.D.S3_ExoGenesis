using System;
using UnityEngine;
using UnityEngine.UI;

namespace StudioXP.Scripts.UI
{
    [Serializable]
    public class EffectUI
    {
        [SerializeField] private string identifier;
        [SerializeField] private Image image;
        [SerializeField] private float defaultDuration = 1;

        public string Identifier => identifier;
        public Image Image => image;
        public float DefaultDuration => defaultDuration;
    }
}
