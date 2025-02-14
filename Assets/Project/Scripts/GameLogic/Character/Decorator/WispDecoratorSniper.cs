using System.Collections.Generic;
using Project.Scripts.Debug;
using Project.Scripts.Entity;
using Project.Scripts.GameLogic.Character.Attack;
using Project.Scripts.GameLogic.Character.Component;
using Project.Scripts.Module.Factory;
using Project.Scripts.Module.Stats;
using LogType = Project.Scripts.Debug.LogType;

namespace Project.Scripts.GameLogic.Character.Decorator
{
    public class WispDecoratorSniper: WispDecorator
    {
        private readonly Dictionary<Bullet, int> _bulletsDamage = new ();

        public WispDecoratorSniper(WispComponent component, BulletFactory bulletFactory, WispStats wispStats) : base(component)
        {
            DebugSystem.Instance.Log(LogType.WispComponent, 
                $"<color=yellow>Sniper decorator added!</color>");
            var args = new BulletActionsArgs(OnEnemyHit, OnWallHit, MoveForward, wispStats.Piercing);
            bulletFactory.SetActions(args);
            bulletFactory.SetConfigBulletFunc(ConfigBullet);
        }

        public override Bullet ConfigBullet(Bullet bullet)
        {
            base.ConfigBullet(bullet);
            DebugSystem.Instance.Log(LogType.WispComponent, 
                $"<color=yellow>Sniper bullet configured!</color>");
            _bulletsDamage.TryAdd(bullet, 0);
            _bulletsDamage[bullet] = 0;
            return bullet;
        }

        public override void MoveForward(Bullet bullet)
        {
            base.MoveForward(bullet);
            _bulletsDamage[bullet] += 2;
        }

        public override void OnEnemyHit(Bullet bullet, IEnemyHealth health)
        {
            base.OnEnemyHit(bullet, health);
            if(bullet == null || health.LastHealthChangeArgs.Type == HeathChangeType.Death)
                 return;
            var bonusDamage = _bulletsDamage[bullet];
            if(bonusDamage <= 0)
                return;
            health.TakeDamage(bonusDamage);
        }
    }
}