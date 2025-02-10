using System;
using UnityEngine;

namespace Project.Scripts.Entity
{
    public interface IHealth
    {
        public int MaxHealth { get; }
        public float CurrentHealth { get; }
        public GameObject GO { get; }
        
        public event Action<OnHealthChangeArgs> OnHealthChange; 

        public void TakeDamage(float dmg);
        public void Heal(float heal);
        public void IncreaseMaxHealth(int value);
    }
}