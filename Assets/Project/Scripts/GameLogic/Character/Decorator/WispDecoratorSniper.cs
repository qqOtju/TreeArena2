using System.Collections.Generic;
using Project.Scripts.Debug;
using Project.Scripts.Entity;
using Project.Scripts.GameLogic.Character.Attack;
using Project.Scripts.GameLogic.Character.Component;
using Project.Scripts.Module.Factory;
using UnityEngine;
using LogType = Project.Scripts.Debug.LogType;

namespace Project.Scripts.GameLogic.Character.Decorator
{
    public class WispDecoratorSniper: WispDecorator
    {
        private const int BonusDamageMultiplier = 5;
        
        private readonly Dictionary<Bullet, int> _bulletsDamage = new ();
        private readonly BulletFactory _bulletFactory;
        
        public WispDecoratorSniper(WispComponent component, BulletFactory bulletFactory) : base(component)
        {
            DebugSystem.Instance.Log(LogType.WispComponent, 
                $"<color=yellow>Sniper decorator added!</color>");
            _bulletFactory = bulletFactory;
            var args = new BulletActionsArgs(OnHealthHit, OnWallHit, MoveForward, 1);
            _bulletFactory.SetActions(args);
            _bulletFactory.SetConfigBulletFunc(ConfigBullet);
        }

        public override Bullet ConfigBullet(Bullet bullet)
        {
            base.ConfigBullet(bullet);
            _bulletsDamage.TryAdd(bullet, 0);
            _bulletsDamage[bullet] = 0;
            return bullet;
        }

        public override void MoveForward(Bullet bullet)
        {
            base.MoveForward(bullet);
            _bulletsDamage[bullet]++;
        }

        public override void OnHealthHit(Bullet bullet, IHealth health)
        {
            base.OnHealthHit(bullet, health);
            if(health.LastHealthChangeArgs.Type == HeathChangeType.Death)
                 return;
            var bonusDamage = _bulletsDamage[bullet] * BonusDamageMultiplier;
            health.TakeDamage(bonusDamage);
            if(bullet.CurrentPiercing <= 0)
                _bulletFactory.Release(bullet);
        }
    }
}