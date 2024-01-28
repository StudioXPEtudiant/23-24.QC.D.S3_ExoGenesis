using UnityEngine;

namespace StudioXP.Scripts.Animation
{
    public class FlashAnimation : MonoBehaviour
    {
        [SerializeField] private Color color = Color.red;
        [SerializeField] private float duration = 0.5f;
        
        private Renderer _renderer;
        private float _counter;
        private bool _isPlaying;
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        void Update()
        {
            if (!_isPlaying) return;
            _counter -= Time.deltaTime;

            if (!(_counter <= 0)) return;
            
            _isPlaying = false;
            _renderer.material.SetColor(EmissionColor, Color.black);
        }

        public void Play()
        {
            _renderer.material.SetColor(EmissionColor, color);
            _counter = duration;
            _isPlaying = true;
        }
    }
}
