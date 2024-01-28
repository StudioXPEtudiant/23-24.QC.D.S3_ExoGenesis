using System;
using TMPro;
using UnityEngine;

namespace StudioXP.Scripts.Characters.Speech
{
    public class SpeechBubble : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TextMeshPro text;
        [SerializeField] private float fadingDuration = 1;

        private bool _hasDuration;
        private float _counter;

        private bool _isFading;
        private float _fadingCounter;
        private float _initialFadingCounter;

        private void Awake()
        {
            if (!text)
                text = GetComponent<TextMeshPro>();

            if (!spriteRenderer)
                spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (!_hasDuration) return;

            if (_isFading)
            {
                _fadingCounter -= Time.deltaTime;
                SetAlpha(_fadingCounter / _initialFadingCounter);

                if (_fadingCounter <= 0)
                    HideDialogue();

                return;
            }
            
            _counter -= Time.deltaTime;
            if (_counter > 0) return;
            
            _isFading = true;
            _fadingCounter = fadingDuration;
            _initialFadingCounter = fadingDuration;
        }

        public void HideDialogue()
        {
            gameObject.SetActive(false);
        }

        public void ShowDialogue(string dialogue)
        {
            gameObject.SetActive(true);
            text.text = dialogue;
            _hasDuration = false;
            _isFading = false;
            SetAlpha(1);
        }

        public void ShowDialogue(string dialogue, float duration)
        {
            ShowDialogue(dialogue);
            _hasDuration = true;
            _counter = duration;
        }

        private void SetAlpha(float alpha)
        {
            var spriteColor = spriteRenderer.color;
            var textColor = text.color;

            spriteColor.a = alpha;
            textColor.a = alpha;

            spriteRenderer.color = spriteColor;
            text.color = textColor;
        }
    }
}
