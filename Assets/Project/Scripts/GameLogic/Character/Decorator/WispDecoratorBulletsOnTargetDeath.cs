using Project.Scripts.Debug;
using Project.Scripts.Entity;
using Project.Scripts.GameLogic.Character.Attack;
using Project.Scripts.GameLogic.Character.Component;
using Project.Scripts.Module.Factory;
using UnityEngine;
using LogType = Project.Scripts.Debug.LogType;

namespace Project.Scripts.GameLogic.Character.Decorator
{
    public class WispDecoratorBulletsOnTargetDeath: WispDecorator
    {
        private readonly BulletFactory _bulletFactory;
        
        public WispDecoratorBulletsOnTargetDeath(WispComponent component, BulletFactory bulletFactory) : base(component)
        {
            DebugSystem.Instance.Log(LogType.WispComponent, 
                $"<color=yellow>Bullets on target death decorator added!</color>");
            _bulletFactory = bulletFactory;
            var args = new BulletActionsArgs(OnHealthHit, OnWallHit, MoveForward, 1);
            _bulletFactory.SetActions(args);
            _bulletFactory.SetConfigBulletFunc(ConfigBullet);
        }

        public override void OnTargetDeath(Bullet bullet, IHealth health)
        {
            base.OnTargetDeath(bullet, health);
            DebugSystem.Instance.Log(LogType.WispComponent, 
                "Bullets <color=green>on target death</color>!");
            var bulletLeft = _bulletFactory.Get();
            var bulletRight = _bulletFactory.Get();
            var position = health.GO.transform.position;
            bulletRight.transform.position = position;
            bulletLeft.transform.position = position;
            var rotation = bullet.transform.rotation;
            bulletLeft.transform.rotation = Quaternion.Euler(new Vector3(0,0, rotation.eulerAngles.z + 90));
            bulletRight.transform.rotation = Quaternion.Euler(new Vector3(0,0, rotation.eulerAngles.z - 90));
        }
    }
}