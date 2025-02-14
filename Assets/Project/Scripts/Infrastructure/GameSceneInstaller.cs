﻿using Project.Scripts.Config.Wisp;
using Project.Scripts.Module.Factory;
using Project.Scripts.Module.Stats;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Infrastructure
{
    public class GameSceneInstaller: MonoInstaller
    {
        [SerializeField] private WispDecoratorFactory _decoratorFactory;
        [SerializeField] private WispData _wispData;
        
        public override void InstallBindings()
        {
            BindDecoratorFactory();
            BindWispStats();
        }

        private void BindDecoratorFactory()
        {
            Container.Bind<WispDecoratorFactory>().FromInstance(_decoratorFactory).AsSingle();
        }

        private void BindWispStats()
        {
            var wispBonuses = new WispBonuses();
            Container.Bind<WispBonuses>().FromInstance(wispBonuses).AsSingle();
            var wispStats = new WispStats(_wispData.WispConfig, wispBonuses);
            Container.Bind<WispStats>().FromInstance(wispStats).AsSingle();
        }
    }
}