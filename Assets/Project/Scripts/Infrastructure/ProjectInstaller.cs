using UnityEngine;
using Zenject;

namespace Project.Scripts.Infrastructure
{
    public class ProjectInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            BindDiContainer();
        }

        private void BindDiContainer()
        {
            Container.Bind<DiContainer>().FromInstance(Container).AsSingle();
        }
    }
}