using Project.Scripts.Debug;

namespace Project.Scripts.GameLogic.Test
{
    public class ConcreteComponent: Component
    {
        public override void Operation()
        {
            DebugSystem.Instance.Log(LogType.Info, "<color=red>ConcreteComponent</color> 1");
        }
    }
}