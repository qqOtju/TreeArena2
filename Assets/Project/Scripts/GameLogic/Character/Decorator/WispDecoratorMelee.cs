using System.Collections.Generic;
using Project.Scripts.Debug;
using Project.Scripts.Entity;
using Project.Scripts.GameLogic.Character.Attack;
using Project.Scripts.GameLogic.Character.Component;
using Project.Scripts.Module.Factory;
using LogType = Project.Scripts.Debug.LogType;

namespace Project.Scripts.GameLogic.Character.Decorator
{
    public class WispDecoratorMelee: WispDecorator
    {
        private const int BaseDamage = 100;
        
        private readonly Dictionary<Bullet, int> _bulletsDamage = new ();
        private readonly BulletFactory _bulletFactory;

        public WispDecoratorMelee(WispComponent component, BulletFactory bulletFactory) : base(component)
        {
            DebugSystem.Instance.Log(LogType.WispComponent, 
                $"<color=yellow>Melee decorator added!</color>");
            _bulletFactory = bulletFactory;
            var args = new BulletActionsArgs(OnHealthHit, OnWallHit, MoveForward, 1);
            _bulletFactory.SetActions(args);
            _bulletFactory.SetConfigBulletFunc(ConfigBullet);
        }

        public override Bullet ConfigBullet(Bullet bullet)
        {
            base.ConfigBullet(bullet);
            DebugSystem.Instance.Log(LogType.WispComponent, 
                $"<color=yellow>Melee bullet created!</color>");
            _bulletsDamage.TryAdd(bullet, BaseDamage);
            _bulletsDamage[bullet] = BaseDamage;
            return bullet;
        }

        public override void MoveForward(Bullet bullet)
        {
            base.MoveForward(bullet);
            _bulletsDamage[bullet] -= 2;
        }

        public override void OnHealthHit(Bullet bullet, IHealth health)
        {
            base.OnHealthHit(bullet, health);
            if(bullet == null || health.LastHealthChangeArgs.Type == HeathChangeType.Death)
                return;
            var bonusDamage = _bulletsDamage[bullet];
            if(bonusDamage <= 0)
                return;
            health.TakeDamage(bonusDamage);
        }
    }
}