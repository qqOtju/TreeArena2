using Project.Scripts.Debug;
using Project.Scripts.Entity;
using Project.Scripts.GameLogic.Character.Attack;
using Project.Scripts.GameLogic.Character.Component;
using Project.Scripts.Module.Factory;
using LogType = Project.Scripts.Debug.LogType;

namespace Project.Scripts.GameLogic.Character.Decorator
{
    public class WispDecoratorVampireBullets: WispDecorator
    {
        private readonly IHealth _treeHealth;
        
        public WispDecoratorVampireBullets(WispComponent component, BulletFactory factory, IHealth treeHealth) : base(component)
        {
            DebugSystem.Instance.Log(LogType.WispComponent, 
                $"<color=yellow>Vampire bullets decorator added!</color>");
            _treeHealth = treeHealth;
            var args = new BulletActionsArgs(OnEnemyHit, OnWallHit, MoveForward, 1);
            factory.SetActions(args);
        }
        
        public override void OnEnemyHit(Bullet bullet, IEnemyHealth health)
        {
            base.OnEnemyHit(bullet, health);
            if(health.LastHealthChangeArgs.Type == HeathChangeType.Death)
                _treeHealth.Heal(_treeHealth.MaxHealth * 0.05f);
        }
    }
}