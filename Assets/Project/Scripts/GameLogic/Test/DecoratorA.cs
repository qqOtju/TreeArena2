using Project.Scripts.Debug;

namespace Project.Scripts.GameLogic.Test
{
    public class DecoratorA: Decorator
    {
        public DecoratorA(Component component) : base(component)
        {
        }

        public override void Operation()
        {
            base.Operation();
            DebugSystem.Instance.Log(LogType.Info, "<color=red>DecoratorA</color> 1");
        }
    }
}