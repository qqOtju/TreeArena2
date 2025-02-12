using System;
using Project.Scripts.Entity;
using UnityEngine;

namespace Project.Scripts.GameLogic.Enemy
{
    [SelectionBase]
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class EnemyBase: EntityBase
    {
        [SerializeField] private int _health;

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