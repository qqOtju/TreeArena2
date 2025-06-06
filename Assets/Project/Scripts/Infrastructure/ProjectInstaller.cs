using System.Collections.Generic;
using Project.Scripts.Config.Item.Tree;
using Project.Scripts.Config.Item.WispShop;
using Project.Scripts.Config.Wisp;
using Project.Scripts.GameLogic.GameCycle;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Infrastructure
{
    public class ProjectInstaller: MonoInstaller
    {
        [SerializeField] private WispData _baseWispData;
        [SerializeField] private List<TreeItem> _treeItems;
        [SerializeField] private List<WispItem> _wispShopItems;

        public override void InstallBindings()
        {
            BindDiContainer();
            BindGameData();
            BindTreeItems();
            BindWispShopItems();
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

        private void BindTreeItems()
        {
            //TODO: Check, make something better
            Container.Bind<List<TreeItem>>().FromInstance(_treeItems).AsSingle();
        }
        
        private void BindWispShopItems()
        {
            Container.Bind<List<WispItem>>().FromInstance(_wispShopItems).AsSingle();
        }
    }
}