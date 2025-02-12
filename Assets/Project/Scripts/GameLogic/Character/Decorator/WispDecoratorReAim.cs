using System;
using Project.Scripts.Debug;
using Project.Scripts.Entity;
using Project.Scripts.GameLogic.Character.Attack;
using Project.Scripts.GameLogic.Character.Component;
using Project.Scripts.Module.Factory;
using UnityEngine;
using LogType = Project.Scripts.Debug.LogType;

namespace Project.Scripts.GameLogic.Character.Decorator
{
    public class WispDecoratorReAim: WispDecorator
    {
        private const string EnemyLayer = "Enemy";
        
        public WispDecoratorReAim(WispComponent component, BulletFactory bulletFactory) : base(component)
        {
            DebugSystem.Instance.Log(LogType.WispComponent, 
                $"<color=yellow>Re-aim decorator added!</color>");
            var args = new BulletActionsArgs(OnHealthHit, OnWallHit, MoveForward, 5);
            bulletFactory.SetActions(args);
            bulletFactory.SetConfigBulletFunc(ConfigBullet);
        }
        
        //ToDo: Optimize this method, this method creates spikes in performance
        public override void OnHealthHit(Bullet bullet, IHealth health)
        {
            base.OnHealthHit(bullet, health);
            if(bullet == null) return;
            var results = new Collider2D[20];
            var filter = new ContactFilter2D
            {
                layerMask = LayerMask.GetMask(EnemyLayer),
                useLayerMask = true,
            };
            var size = Physics2D.OverlapCircle(bullet.transform.position, 20, filter, results);
            if (size <= 0) return;
            Collider2D closestTarget = null;
            var minDistance = 3000f;
            var baseEnemy = health.GO;
            var bulletPosition = bullet.transform.position;
            foreach (var coll in results.AsSpan(0, size))
            {
                if (coll.gameObject == baseEnemy) continue;
                var distance = Vector2.SqrMagnitude(coll.transform.position - bulletPosition);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTarget = coll;
                }
            }
            if (closestTarget == null) return;
            var direction = closestTarget.transform.position - bullet.transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}