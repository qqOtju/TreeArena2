using Project.Scripts.Debug;
using Project.Scripts.GameLogic.Character.Attack;
using Project.Scripts.GameLogic.Character.Component;
using Project.Scripts.Module.Factory;
using UnityEngine;
using LogType = Project.Scripts.Debug.LogType;

namespace Project.Scripts.GameLogic.Character.Decorator
{
    public class WispDecoratorDoubleAttack: WispDecorator
    {
        private readonly Transform _bulletSpawnPoint;
        private readonly BulletFactory _bulletFactory;

        public WispDecoratorDoubleAttack(BulletFactory bulletFactory, Transform bulletSpawnPoint,
            WispComponent component): base(component)
        {
            DebugSystem.Instance.Log(LogType.WispComponent, 
                $"<color=yellow>Double attack decorator added!</color>");
            _bulletSpawnPoint = bulletSpawnPoint;
            _bulletFactory = bulletFactory;
            var args = new BulletActionsArgs(OnHealthHit, OnWallHit, MoveForward, 2);
            _bulletFactory.SetActions(args);
            _bulletFactory.SetConfigBulletFunc(ConfigBullet);
        }

        public override void Shoot()
        {
            base.Shoot();
            DebugSystem.Instance.Log(LogType.WispComponent, 
                "Double <color=red>attack</color>!");
            var bullet1 = _bulletFactory.Get();
            var bullet2 = _bulletFactory.Get();
            const float angle = 7f;
            var localRotation = _bulletSpawnPoint.localRotation;
            bullet1.transform.rotation = Quaternion.Euler(
                new Vector3(0,0, localRotation.eulerAngles.z + angle));
            bullet2.transform.rotation = Quaternion.Euler(
                new Vector3(0,0, localRotation.eulerAngles.z - angle));
        }
    }
}