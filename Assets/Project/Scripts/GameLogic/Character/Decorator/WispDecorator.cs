using Project.Scripts.Entity;
using Project.Scripts.GameLogic.Character.Attack;
using Project.Scripts.GameLogic.Character.Component;
using Project.Scripts.Module.Stats;

namespace Project.Scripts.GameLogic.Character.Decorator
{
    public abstract class WispDecorator: WispComponent
    {
        protected WispComponent Component;
        
        protected WispDecorator(WispComponent component)
        {
            Component = component;
        }
        
        public override void OnEnemyHit(Bullet bullet, IEnemyHealth health)
        {
            Component.OnEnemyHit(bullet, health);
            //ToDo: be cautious with this line, may cause problems
            if(health.LastHealthChangeArgs.Type == HeathChangeType.Death)
                OnEnemyDeath(bullet, health);
        }

        public override void OnWallHit(Bullet bullet)
        {
            Component.OnWallHit(bullet);
        }

        public override void MoveForward(Bullet bullet)
        {
            Component.MoveForward(bullet);
        }

        public override void OnEnemyDeath(Bullet bullet, IEnemyHealth health)
        {
            Component.OnEnemyDeath(bullet, health);
        }

        public override Bullet ConfigBullet(Bullet bullet)
        {
            return Component.ConfigBullet(bullet);
        }

        public override void Shoot()
        {
            Component.Shoot();
        }
    }
}