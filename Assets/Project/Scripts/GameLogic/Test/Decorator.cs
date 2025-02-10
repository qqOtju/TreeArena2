namespace Project.Scripts.GameLogic.Test
{
    public class Decorator: Component
    {
        protected Component _component;

        public Decorator(Component component)
        {
            _component = component;
        }

        public void SetComponent(Component component)
        {
            _component = component;
        }

        // The Decorator delegates all work to the wrapped component.
        public override void Operation()
        {
            if (_component != null)
                _component.Operation();
        }
    }
}