using Project.Scripts.Debug;
using Project.Scripts.GameLogic.Character.Attack;
using Project.Scripts.GameLogic.Character.Component;
using Project.Scripts.Module.Factory;
using UnityEngine;
using LogType = Project.Scripts.Debug.LogType;

namespace Project.Scripts.GameLogic.Character.Decorator
{
    public class WispDecoratorBackShot: WispDecorator
    {
        private readonly Transform _bulletSpawnPoint;
        
        public WispDecoratorBackShot(BulletFactory bulletFactory, Transform bulletSpawnPoint,
            WispComponent component) : base(component)
        {
            DebugSystem.Instance.Log(LogType.WispComponent, 
                $"<color=yellow>Back shot decorator added!</color>");
            _bulletSpawnPoint = bulletSpawnPoint;
            var args = new BulletActionsArgs(OnHealthHit, OnWallHit, MoveForward, 2);
            bulletFactory.SetActions(args);
        }

        public override void Shoot()
        {
            base.Shoot();
            DebugSystem.Instance.Log(LogType.WispComponent, 
                "Back <color=green>shot</color>!");
            var bullet = CreateBullet();
            var angle = 180f;
            bullet.transform.rotation = Quaternion.Euler(
                new Vector3(0,0, _bulletSpawnPoint.localRotation.eulerAngles.z + angle));
        }
    }
}