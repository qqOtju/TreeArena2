using System;
using Project.Scripts.Entity;
using UnityEngine;

namespace Project.Scripts.GameLogic.Enemy
{
    [SelectionBase]
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class EnemyBase: EntityBase, IEnemyHealth
    {
        [SerializeField] private int _health;
        [SerializeField] private EnemyType _type;
        
        public EnemyType Type => _type;

        private void Awake()
        {
            OnHealthChange += CheckDeath;
        }

        private void Start()
        {
            SetInitialHealth(_health);
        }

        private void CheckDeath(OnHealthChangeArgs obj)
        {
            if(obj.Type == HeathChangeType.Death)
                Destroy(gameObject);
        }
    }
}