using System;
using System.Collections.Generic;
using UnityEngine;


namespace StudioXP.Scripts.Objects
{
    public class Storable : MonoBehaviour
    {

        [SerializeField] private Vector3 storagePosition;
        [SerializeField] private Quaternion storageRotation;
        [SerializeField] private Vector3 storageScale=Vector3.one;

        [SerializeField] private List<String> storageTypes;

        public List<String> StorageTypes => storageTypes;

        public Vector3 StoragePosition => storagePosition;
        public Quaternion StorageRotation => storageRotation;
        public Vector3 StorageScale => storageScale;

       
    }
}
