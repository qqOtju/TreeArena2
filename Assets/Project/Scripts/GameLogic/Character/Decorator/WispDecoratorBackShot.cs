using Project.Scripts.Debug;
using Project.Scripts.GameLogic.Character.Attack;
using Project.Scripts.GameLogic.Character.Component;
using Project.Scripts.Module.Factory;
using Project.Scripts.Module.Stats;
using Project.Scripts.Module.Stats.Wisp;
using UnityEngine;
using LogType = Project.Scripts.Debug.LogType;

namespace Project.Scripts.GameLogic.Character.Decorator
{
    public class WispDecoratorBackShot: WispDecorator
    {
        private readonly BulletFactory _bulletFactory;
        private readonly Transform _bulletSpawnPoint;
        
        public WispDecoratorBackShot(BulletFactory bulletFactory, Transform bulletSpawnPoint,
            WispComponent component, WispStats wispStats) : base(component)
        {
            DebugSystem.Instance.Log(LogType.WispComponent, 
                $"<color=yellow>Back shot decorator added!</color>");
            _bulletSpawnPoint = bulletSpawnPoint;
            _bulletFactory = bulletFactory;
            var args = new BulletActionsArgs(OnEnemyHit, OnWallHit, MoveForward, wispStats.Piercing);
            bulletFactory.SetActions(args);
        }

        public override void Shoot()
        {
            base.Shoot();
            DebugSystem.Instance.Log(LogType.WispComponent, 
                "Back <color=green>shot</color>!");
            var bullet = _bulletFactory.Get();
            var angle = 180f;
            bullet.transform.rotation = Quaternion.Euler(
                new Vector3(0,0, _bulletSpawnPoint.localRotation.eulerAngles.z + angle));
        }
    }
}