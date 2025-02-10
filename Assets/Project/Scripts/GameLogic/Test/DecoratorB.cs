using Project.Scripts.Debug;

namespace Project.Scripts.GameLogic.Test
{
    public class DecoratorB: Decorator
    {
        public DecoratorB(Component component): base(component)
        {
        }

        public override void Operation()
        {
            base.Operation();
            DebugSystem.Instance.Log(LogType.Info, "<color=red>DecoratorB</color> 1");
        }
    }
}