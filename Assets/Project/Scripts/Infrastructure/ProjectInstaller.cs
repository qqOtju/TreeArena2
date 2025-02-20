using Project.Scripts.Config.Wisp;
using Project.Scripts.GameLogic;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Infrastructure
{
    public class ProjectInstaller: MonoInstaller
    {
        [SerializeField] private WispData _baseWispData;
        
        public override void InstallBindings()
        {
            BindDiContainer();
            BindGameData();
        }

        private void BindDiContainer()
        {
            Container.Bind<DiContainer>().FromInstance(Container).AsSingle();
        }

        private void BindGameData()
        {
            var gameData = new GameData(_baseWispData);
            Container.Bind<GameData>().FromInstance(gameData).AsSingle();
        }
    }
}