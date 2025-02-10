using Project.Scripts.Module.Factory;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Infrastructure
{
    public class GameSceneInstaller: MonoInstaller
    {
        [SerializeField] private WispDecoratorFactory _decoratorFactory;
        
        public override void InstallBindings()
        {
            BindDecoratorFactory();
        }

        private void BindDecoratorFactory()
        {
            Container.Bind<WispDecoratorFactory>().FromInstance(_decoratorFactory).AsSingle();
        }
    }
}