using System;
using Project.Scripts.Debug;
using UnityEngine;
using LogType = Project.Scripts.Debug.LogType;

namespace Project.Scripts.Entity
{
    public abstract class EntityBase: MonoBehaviour, IHealth
    {
        private int _maxHealth;
        private float _currentHealth;

        public GameObject GO => gameObject;
        public OnHealthChangeArgs LastHealthChangeArgs { get; private set; }
        public int MaxHealth
        {
            get => _maxHealth;
            private set => _maxHealth = Mathf.Max(1, value); 
        }
        public float CurrentHealth
        {
            get => _currentHealth;
            protected set
            {
                var baseHealth = _currentHealth;
                _currentHealth = Mathf.Clamp(value, 0, MaxHealth); 
                OnHealthChange?.Invoke(CreateHealthChangeArgs(baseHealth,CurrentHealth,transform.position));
            }
        }

        public event Action<OnHealthChangeArgs> OnHealthChange;

        private OnHealthChangeArgs CreateHealthChangeArgs(float previousHealth, float currentHealth, Vector3 position)
        {
            var type = HeathChangeType.Damage;
            if (currentHealth > previousHealth)
                type = HeathChangeType.Heal;
            else if (currentHealth <= 0)
                type = HeathChangeType.Death;
            var args = new OnHealthChangeArgs
            {
                Object = gameObject,
                Value = currentHealth,
                Type = type
            };
            LastHealthChangeArgs = args;
            return args;
        }

        public virtual void TakeDamage(float dmg)
        {
            var args = CreateHealthChangeArgs(CurrentHealth, CurrentHealth - dmg, transform.position);
            CurrentHealth -= dmg;
            DebugSystem.Instance.Log(LogType.Entity, $"{gameObject.name} takes <color=red>{dmg}</color> damage | {CurrentHealth}/{MaxHealth}");
        }
        
        public virtual void Heal(float heal)
        {
            var args = CreateHealthChangeArgs(CurrentHealth, CurrentHealth + heal, transform.position);
            CurrentHealth += heal;
            //DebugSystem.Instance.Log(LogType.Entity, $"{gameObject.name} heals <color=green>{heal}</color> | {CurrentHealth}/{MaxHealth}");
        }
        
        public virtual void IncreaseMaxHealth(int value)
        {
            var relative = CurrentHealth / MaxHealth;
            var health = MaxHealth + value;
            if (health < 0)
                MaxHealth = 10;
            else
                MaxHealth += value;
            CurrentHealth = MaxHealth * relative;
            DebugSystem.Instance.Log(LogType.Entity, $"{gameObject.name} max health increased by <color=blue>{value}</color> | {CurrentHealth}/{MaxHealth}");
        }
        
        protected void SetInitialHealth(int maxHealth)
        {
            if (maxHealth <= 0) throw new ArgumentException("MaxHealth must be positive.");
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }
    }
}