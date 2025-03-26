using System.Collections.Generic;
using Project.Scripts.Config.Item.Tree;
using Project.Scripts.Config.Item.WispShop;
using Project.Scripts.Config.Tree;
using Project.Scripts.Config.Wisp;
using Project.Scripts.GameLogic;
using Project.Scripts.GameLogic.Character;
using Project.Scripts.GameLogic.Character.Wisp;
using Project.Scripts.GameLogic.Enemy;
using Project.Scripts.GameLogic.GameCycle;
using Project.Scripts.Module.Factory;
using Project.Scripts.Module.ItemManager;
using Project.Scripts.Module.Spawner;
using Project.Scripts.Module.Stats.Enemy;
using Project.Scripts.Module.Stats.Tree;
using Project.Scripts.Module.Stats.Wisp;
using Project.Scripts.Module.System;
using UnityEngine;
using Zenject;
using Tree = Project.Scripts.GameLogic.Character.Tree;

namespace Project.Scripts.Infrastructure
{
    public class GameSceneInstaller: MonoInstaller
    {
        [SerializeField] private WispDecoratorFactory _decoratorFactory;
        [SerializeField] private TreeConfig _treeConfig;
        [SerializeField] private Transform _enemyContainer;
        [SerializeField] private Tree _tree;
        [SerializeField] private Player _player;
        [SerializeField] private Transform _bulletContainer;
        [SerializeField] private EnemySpawner _enemySpawner;
        
        private EnemyBonuses _enemyBonuses;
        private GameData _gameData;
        private List<TreeItem> _treeItems;
        private List<WispItem> _wispShopItems;

        [Inject]
        private void Construct(GameData gameData, List<TreeItem> treeItems, List<WispItem> wispShopItems)
        {
            _gameData = gameData;
            _treeItems = treeItems;
            _wispShopItems = wispShopItems;
        }
        
        public override void InstallBindings()
        {
            BindEnemySpawner();
            BindDecoratorFactory();
            BindWispStats();
            BindTreeStats();
            BindEnemyBonuses();
            BindEnemyFactory();
            BindWisp();
            BindTreeItemManager();
            BindCoinSystem();
            BindWispShopItemManager();
        }

        private void BindEnemySpawner()
        {
            Container.Bind<EnemySpawner>().FromInstance(_enemySpawner);
        }

        private void BindDecoratorFactory()
        {
            Container.Bind<WispDecoratorFactory>().FromInstance(_decoratorFactory).AsSingle();
        }

        private void BindWispStats()
        {
            var wispBonuses = new WispBonuses();
            Container.Bind<WispBonuses>().FromInstance(wispBonuses).AsSingle();
            var wispConfig = _gameData.ChosenWisp.WispConfig;
            var wispStats = new WispStats(wispConfig, wispBonuses);
            Container.Bind<WispStats>().FromInstance(wispStats).AsSingle();
        }

        private void BindTreeStats()
        {
            var treeBonuses = new TreeBonuses();
            Container.Bind<TreeBonuses>().FromInstance(treeBonuses).AsSingle();
            var treeStats = new TreeStats(_treeConfig, treeBonuses);
            Container.Bind<TreeStats>().FromInstance(treeStats).AsSingle();
        }

        private void BindEnemyBonuses()
        {
            _enemyBonuses = new EnemyBonuses();
            Container.Bind<EnemyBonuses>().FromInstance(_enemyBonuses).AsSingle();
        }

        private void BindEnemyFactory()
        {
            var enemyFactory = new EnemyFactory(_enemyContainer, _enemyBonuses, Container, _tree);
            Container.Bind<EnemyFactory>().FromInstance(enemyFactory).AsSingle();
        }

        private void BindWisp()
        {
            var wispPrefab = _gameData.ChosenWisp.WispPrefab;
            var wisp = Container.InstantiatePrefab(wispPrefab).GetComponent<WispBase>();
            wisp.Init(_player.transform, _bulletContainer);
            Container.Bind<IWisp>().FromInstance(wisp).AsSingle();
        }

        private void BindTreeItemManager()
        {
            var treeItemManager = new TreeItemManager(_treeItems);
            Container.Bind<TreeItemManager>().FromInstance(treeItemManager).AsSingle();
        }

        private void BindCoinSystem()
        {
            var coinSystem = new CoinSystem(0);
            Container.Bind<CoinSystem>().FromInstance(coinSystem).AsSingle();
        }
        
        private void BindWispShopItemManager()
        {
            var wispShopItemManager = new WispItemManager(_wispShopItems);
            Container.Bind<WispItemManager>().FromInstance(wispShopItemManager).AsSingle();
        }
    }
}