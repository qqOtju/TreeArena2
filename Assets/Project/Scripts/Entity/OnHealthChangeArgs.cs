using UnityEngine;

namespace Project.Scripts.Entity
{
    public struct OnHealthChangeArgs
    {
        public float PreviousHealth;
        public float CurrentHealth;
        public HeathChangeType Type;
        public GameObject Object;
    }
}