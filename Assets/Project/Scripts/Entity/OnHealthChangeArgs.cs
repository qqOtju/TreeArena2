using UnityEngine;

namespace Project.Scripts.Entity
{
    public struct OnHealthChangeArgs
    {
        public float Value;
        public HeathChangeType Type;
        public GameObject Object;
    }
}