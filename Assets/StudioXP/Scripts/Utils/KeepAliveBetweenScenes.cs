using System;
using UnityEngine;

namespace StudioXP.Scripts.Utils
{
    public class KeepAliveBetweenScenes : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
