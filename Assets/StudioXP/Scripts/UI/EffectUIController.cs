using System;
using System.Collections.Generic;
using UnityEngine;

namespace StudioXP.Scripts.UI
{
    public class EffectUIController : MonoBehaviour
    {
        public static EffectUIController Instance { get; private set; }

        [SerializeField] private EffectUI[] effects;

        private readonly Dictionary<string, EffectUI> _effectsByIdentifier = new();

        private EffectUI _playingEffect = null;
        private float _counter;
        private float _currentDuration;
                
        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            foreach (var effect in effects)
            {
                _effectsByIdentifier.Add(effect.Identifier, effect);
                effect.Image.gameObject.SetActive(false);
            }
                
        }

        public void Play(string identifier)
        {
            if (!_effectsByIdentifier.ContainsKey(identifier)) return;
            
            var effect = _effectsByIdentifier[identifier];
            Play(effect, effect.DefaultDuration);
        }

        public void Play(string identifier, float duration)
        {
            if (!_effectsByIdentifier.ContainsKey(identifier)) return;
            
            var effect = _effectsByIdentifier[identifier];
            Play(effect, duration);
        }

        private void Play(EffectUI effectUI, float duration)
        {
            _playingEffect?.Image.gameObject.SetActive(false);

            _playingEffect = effectUI;
            _counter = duration;
            _currentDuration = duration;
            
            _playingEffect.Image.gameObject.SetActive(true);
            UpdateColor();
        }

        private void UpdateColor()
        {
            var color = _playingEffect.Image.color;
            color.a = _counter / _currentDuration;
            _playingEffect.Image.color = color;
        }

        private void Update()
        {
            if (_playingEffect == null) return;

            if (_counter <= 0)
            {
                _playingEffect.Image.gameObject.SetActive(false);
                _playingEffect = null;
                return;
            }

            _counter -= Time.deltaTime;
            UpdateColor();
        }
    }
}
