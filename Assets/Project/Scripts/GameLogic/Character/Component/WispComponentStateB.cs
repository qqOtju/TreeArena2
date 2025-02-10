using Project.Scripts.Debug;
using Project.Scripts.Entity;
using Project.Scripts.GameLogic.Character.Attack;
using Project.Scripts.Module.Factory;
using UnityEngine;
using LogType = Project.Scripts.Debug.LogType;

namespace Project.Scripts.GameLogic.Character.Component
{
    public class WispComponentStateB: WispComponent
    {
        private const int Damage = 50;
        private const int Piercing = 1;
        
        private readonly BulletFactory _bulletFactory;
        private readonly Transform _bulletSpawnPoint;
        
        public WispComponentStateB(BulletFactory bulletFactory, Transform bulletSpawnPoint)
        {
            _bulletFactory = bulletFactory;
            _bulletSpawnPoint = bulletSpawnPoint;
            var args = new BulletActionsArgs(OnHealthHit, OnWallHit, MoveForward, Piercing);
            _bulletFactory.SetActions(args);
        }

        public override void OnHealthHit(Bullet bullet, IHealth health)
        {
            health.TakeDamage(Damage);
            if(bullet.CurrentPiercing <= 0)
                _bulletFactory.Release(bullet);
        }

        public override void OnWallHit(Bullet bullet)
        {
            _bulletFactory.Release(bullet);
        }

        public override void MoveForward(Bullet bullet)
        {
            if (bullet.CurrenDistance <= 0)
            {
                _bulletFactory.Release(bullet);
                return;
            }
            var tr = bullet.gameObject.transform;
            var rb = bullet.Rb;
            var moveForce = tr.position + tr.right / 5;
            rb.MovePosition(moveForce);
        }

        public override void OnTargetDeath(Bullet bullet, IHealth health)
        {
            DebugSystem.Instance.Log(LogType.WispComponent, "Bullet <color=red>killed</color> target!");
        }

        public override Bullet CreateBullet()
        {
            var bullet = _bulletFactory.Create();
            bullet.transform.rotation = Quaternion.Euler(
                new Vector3(0,0, _bulletSpawnPoint.localRotation.eulerAngles.z));
            return bullet;
        }

        public override void Shoot()
        {
            DebugSystem.Instance.Log(LogType.WispComponent, "<color=green>Shoot</color>!");
            CreateBullet();
        }
    }
}