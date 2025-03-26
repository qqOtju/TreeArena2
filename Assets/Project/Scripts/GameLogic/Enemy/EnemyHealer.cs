using System;
using Project.Scripts.Entity;
using UnityEngine;

namespace Project.Scripts.GameLogic.Enemy
{
    public class EnemyHealer: EnemyBase
    {
        private const string EnemyTagLayer = "Enemy";
        
        private readonly Collider2D[] _nearbyAllies = new Collider2D[20];
        private readonly IHealth[] _allies = new IHealth[20];
        
        private ContactFilter2D _contactFilter;

        protected override void Start()
        {
            base.Start();
            _contactFilter = new ContactFilter2D
            {
                useLayerMask = true,
                layerMask = LayerMask.GetMask(EnemyTagLayer)
            };
        }

        protected override void AttackCycle()
        {
            if(!IsNearbyTree()) return;
            AttackTimer += Time.deltaTime;
            if (AttackTimer >= EnemyValue.AttackSpeed)
            {
                var allies = FindClosestAllies();
                foreach (var ally in allies)
                {
                    if (ally != null)
                        Attack(ally);
                }
                AttackTimer = 0;
            }
        }

        protected override void Attack(IHealth health)
        {
            health.Heal(EnemyValue.Damage);
        }

        private IHealth[] FindClosestAllies()
        {
            Physics2D.OverlapCircle(transform.position, EnemyValue.AttackRange,
                _contactFilter, _nearbyAllies);
            Array.Clear(_allies, 0, _allies.Length);
            for (var index = 0; index < _nearbyAllies.Length; index++)
            {
                var ally = _nearbyAllies[index];
                if (ally != null && ally.transform.CompareTag(EnemyTagLayer))
                {
                    var health = ally.GetComponent<IHealth>();
                    if (health != null)
                        _allies[index] = health;
                }
            }
            return _allies;
        }

        protected override void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, EnemyValue.AttackRange);
        }
    }
}