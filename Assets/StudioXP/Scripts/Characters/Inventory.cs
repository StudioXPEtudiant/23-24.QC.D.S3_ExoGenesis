using System;
using System.Collections.Generic;
using StudioXP.Scripts.Objects;
using UnityEngine;

namespace StudioXP.Scripts.Characters
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField, Min(1)] private int size = 1;
        [SerializeField] private GameObject root;

        public GameObject Root => root;

        private readonly List<Pickable> _slots = new();
        private int _currentSlot = 0;

        private void Awake()
        {
            if (!root)
                root = gameObject;
        }

        public bool TrySetItemInEmptySlot(Pickable pickable)
        {
            if (_slots.Count >= size)
                return false;

            if (_currentSlot != _slots.Count)
                return false;

            _slots.Add(pickable);
            return true;
        }

        public Pickable RemoveCurrentSlot()
        {
            if (_currentSlot == _slots.Count) return null;
            var pickable = _slots[_currentSlot];
            _slots.RemoveAt(_currentSlot);
            _currentSlot = _slots.Count;

            return pickable;
        }

        public Pickable GetCurrentItem()
        {
            return _currentSlot >= _slots.Count ? null : _slots[_currentSlot];
        }

        public void ChangeSlot(float axis)
        {
            switch (axis)
            {
                case < 0:
                    PreviousSlot();
                    break;
                case > 0:
                    NextSlot();
                    break;
            }
        }

        public void NextSlot()
        {
            Select(_currentSlot + 1);
        }

        public void PreviousSlot()
        {
            Select(_currentSlot - 1);
        }
        
        private void Select(int slot)
        {
            if (slot > _slots.Count)
                slot = 0;
            else if (slot < 0)
                slot = _slots.Count;

            if (slot == _currentSlot) return;

            var currentInteractable = GetCurrentItem();
            if(currentInteractable)
                currentInteractable.gameObject.SetActive(false);
            
            _currentSlot = slot;
            currentInteractable = GetCurrentItem();
            if(currentInteractable)
                currentInteractable.gameObject.SetActive(true);
        }
    }
}
