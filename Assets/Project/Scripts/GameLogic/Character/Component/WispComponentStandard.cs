using System;
using Project.Scripts.Debug;
using Project.Scripts.Entity;
using Project.Scripts.GameLogic.Character.Attack;
using Project.Scripts.Module.Factory;
using Project.Scripts.Module.Stats;
using Project.Scripts.Module.Stats.Wisp;
using UnityEngine;
using LogType = Project.Scripts.Debug.LogType;
using Random = UnityEngine.Random;

namespace Project.Scripts.GameLogic.Character.Component
{
    public class WispComponentStandard: WispComponent
    {
        protected readonly BulletFactory BulletFactory;
        protected readonly Transform BulletSpawnPoint;
        protected readonly WispStats WispStats;
        
        public WispComponentStandard(BulletFactory bulletFactory, Transform bulletSpawnPoint, WispStats wispStats)
        {
            BulletFactory = bulletFactory;
            BulletSpawnPoint = bulletSpawnPoint;
            WispStats = wispStats;
            var args = new BulletActionsArgs(OnEnemyHit, OnWallHit, MoveForward, WispStats.Piercing);
            BulletFactory.SetActions(args);
            BulletFactory.SetConfigBulletFunc(ConfigBullet);
        }

        //ToDo: Keep in mind that you need to check if the bullet is still alive in additional decorators
        public override void OnEnemyHit(Bullet bullet, IEnemyHealth health)
        {
            var random = Random.Range(0, 100);
            if (random <= WispStats.CriticalChance)
            {
                DebugSystem.Instance.Log(LogType.WispComponent, "Bullet <color=yellow>critically</color> hit target!");
                switch (health.Type)
                {
                    case EnemyType.Basic:
                        health.TakeDamage(WispStats.DamageWithCrit);
                        break;
                    case EnemyType.Elite:
                        health.TakeDamage(WispStats.DamageWithCritAndElite);
                        break;
                    case EnemyType.Boss:
                        health.TakeDamage(WispStats.DamageWithCritAndBoss);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                DebugSystem.Instance.Log(LogType.WispComponent, "Bullet hit target!");
                switch (health.Type)
                {
                    case EnemyType.Basic:
                        health.TakeDamage(WispStats.Damage);
                        break;
                    case EnemyType.Elite:
                        health.TakeDamage(WispStats.DamageWithElite);
                        break;
                    case EnemyType.Boss:
                        health.TakeDamage(WispStats.DamageWithBoss);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            if(bullet.CurrentPiercing <= 0)
            {
                DebugSystem.Instance.Log(LogType.Bullet, "Bullet <color=red>out of piercing</color>!");
                BulletFactory.Release(bullet);
            }
        }

        public override void OnWallHit(Bullet bullet)
        {
            DebugSystem.Instance.Log(LogType.Bullet, "Bullet hit <color=blue>wall</color>!");
            BulletFactory.Release(bullet);
        }

        public override void MoveForward(Bullet bullet)
        {
            if (bullet.CurrenDistance <= 0)
            {
                DebugSystem.Instance.Log(LogType.Bullet, "Bullet <color=red>out of range</color>!");
                BulletFactory.Release(bullet);
                return;
            }
            var tr = bullet.transform;
            var rb = bullet.Rb;
            var moveForce = tr.position + tr.right / 5;
            rb.MovePosition(moveForce);
        }

        public override void OnEnemyDeath(Bullet bullet, IEnemyHealth health)
        {
            DebugSystem.Instance.Log(LogType.WispComponent, "Bullet <color=red>killed</color> target!");
        }

        public override Bullet ConfigBullet(Bullet bullet)
        {
            bullet.transform.rotation = Quaternion.Euler(
                0,0, BulletSpawnPoint.localRotation.eulerAngles.z);
            return bullet;
        }

        public override void Shoot()
        {
            DebugSystem.Instance.Log(LogType.WispComponent, "<color=green>Shoot</color>!");
            BulletFactory.Get();
        }
    }
}