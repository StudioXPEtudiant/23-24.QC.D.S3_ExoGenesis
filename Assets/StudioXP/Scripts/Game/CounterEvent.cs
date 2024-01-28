using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StudioXP.Scripts.Game
{

    public class CounterEvent : MonoBehaviour
    {
        [SerializeField] private int minValue = 0;
        [SerializeField] private int maxValue = 10;
        [SerializeField] private int startValue = 0;
        [SerializeField] private bool callEventOnReset = false;
        private int counter;

       

        [SerializeField] private UnityEvent onMinReached;
        [SerializeField] private UnityEvent onMaxReached;
        [SerializeField] private UnityEvent onIncrement;
        [SerializeField] private UnityEvent onDecrement;


        public void Start()
        {
            ResetCounter();
        }


        public void ResetCounter()
        {
            counter = startValue;
            if (callEventOnReset)
            {
                if (counter == minValue)
                {
                    onMinReached.Invoke();
                }
                if (counter == maxValue)
                {
                    onMaxReached.Invoke();
                }
            }

        }

        public void IncrementValue(int value)
        {
            if (counter < maxValue)
            {
                counter+= value;
                if (counter > maxValue)
                    counter = maxValue;

                onIncrement.Invoke();
                if (counter == maxValue)
                {
                    onMaxReached.Invoke();
                }
            }
        }
        public void DecrementValue(int value)
        {
            if (counter > minValue)
            {
                counter-=value;
                if (counter < minValue)
                    counter = minValue;

                onDecrement.Invoke();
                if (counter == minValue)
                {
                    onMinReached.Invoke();
                }
            }
        }

        public void IncrementValue()
        {
            IncrementValue(1);
        }
        public void DecrementValue()
        {
            DecrementValue(1);
        }
    }
}
