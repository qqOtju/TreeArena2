using Project.Scripts.GameLogic.Character.Component;
using Project.Scripts.Module.Factory;
using UnityEngine;

namespace Project.Scripts.GameLogic.Character.Decorator
{
    public class WispDecoratorStandard: WispDecorator
    {
        public WispDecoratorStandard(WispComponent component, BulletFactory factory, Transform spawnPoint) : base(component)
        {
            
        }
    }
}