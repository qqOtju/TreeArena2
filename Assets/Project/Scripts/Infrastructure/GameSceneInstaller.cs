using Project.Scripts.Config.Tree;
using Project.Scripts.Config.Wisp;
using Project.Scripts.GameLogic;
using Project.Scripts.GameLogic.Character;
using Project.Scripts.GameLogic.Character.Wisp;
using Project.Scripts.GameLogic.Enemy;
using Project.Scripts.Module.Factory;
using Project.Scripts.Module.Stats.Enemy;
using Project.Scripts.Module.Stats.Tree;
using Project.Scripts.Module.Stats.Wisp;
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
        [SerializeField] private WispData _wispData;
        
        private EnemyBonuses _enemyBonuses;
        private GameData _gameData;

        [Inject]
        private void Construct(GameData gameData)
        {
            _gameData = gameData;
        }
        
        public override void InstallBindings()
        {
            BindDecoratorFactory();
            BindWispStats();
            BindTreeStats();
            BindEnemyBonuses();
            BindEnemyFactory();
            BindWisp();
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
            Container.Bind<WispBase>().FromInstance(wisp).AsSingle();
        }
    }
}