using Project.Scripts.Debug;
using Project.Scripts.GameLogic.Character.Attack;
using Project.Scripts.GameLogic.Character.Component;
using Project.Scripts.Module.Factory;
using Project.Scripts.Module.Stats;
using UnityEngine;
using LogType = Project.Scripts.Debug.LogType;

namespace Project.Scripts.GameLogic.Character.Decorator
{
    public class WispDecoratorInterestingMovement: WispDecorator
    {
        private readonly BulletFactory _bulletFactory;
        
        public WispDecoratorInterestingMovement(WispComponent component, BulletFactory bulletFactory, WispStats wispStats): base(component)
        {
            DebugSystem.Instance.Log(LogType.WispComponent, $"<color=yellow>Interesting movement decorator added!</color>");
            _bulletFactory = bulletFactory;
            var args = new BulletActionsArgs(OnEnemyHit, OnWallHit, MoveForward, wispStats.Piercing);
            _bulletFactory.SetActions(args);
            _bulletFactory.SetConfigBulletFunc(ConfigBullet);
        }

        //ToDo: be cautious with this method, because it doesn't call the base method
        public override void MoveForward(Bullet bullet)
        {
            if (bullet.CurrenDistance <= 0)
            {
                _bulletFactory.Release(bullet);
                return;
            }
            var tr = bullet.gameObject.transform;
            var rb = bullet.Rb;
            var forwardDirection = tr.right * 0.2f; // Швидкість руху вперед
            const float waveAmplitude = 0.1f; // Висота хвилі
            const float waveFrequency = 0.5f; // Частота хвилі
            var waveOffset = Mathf.Cos(bullet.CurrenDistance * waveFrequency) * waveAmplitude;
            var perpendicularDirection = tr.up * waveOffset;
            var moveForce = tr.position + forwardDirection + perpendicularDirection;
            rb.MovePosition(moveForce);
        }
    }
}