using System;
using UnityEngine;
using UnityEngine.UI;

namespace StudioXP.Scripts.UI
{
    public class RadialValueUI : MonoBehaviour
    {
        private Material _material;

        private int _removedSegmentsProperty;
        private float _minSegment;

        private int _segmentCountProperty;
        private float _maxSegment;

        private float _maxValue;
        public float MaxValue
        {
            get => _maxValue;
            set
            {
                _maxValue = value;
                RefreshVisual();
            }
        }

        private float _value;
        public float Value
        {
            get => _value;
            set
            {
                _value = value;
                RefreshVisual();
            }
        }

        private float _scale;

        protected virtual void Awake()
        {
            _removedSegmentsProperty = Shader.PropertyToID("_RemovedSegments");
            _segmentCountProperty = Shader.PropertyToID("_SegmentCount");

            _material = GetComponent<Image>().material;
            _minSegment = _material.GetFloat(_removedSegmentsProperty);
            _maxSegment = _material.GetFloat(_segmentCountProperty);
            _scale = _maxSegment - _minSegment;
        }

        private void RefreshVisual()
        {
            _material.SetFloat(_removedSegmentsProperty, ConvertValueToSegment(Value));
        }

        private float ConvertValueToSegment(float value)
        {
            return _maxSegment - ((_value / _maxValue) * _scale + _minSegment);
        }
    }
}
