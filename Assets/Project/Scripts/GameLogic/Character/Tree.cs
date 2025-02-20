using System;
using Project.Scripts.Debug;
using Project.Scripts.Entity;
using Project.Scripts.Module.Stats.Tree;
using UnityEngine;
using Zenject;
using LogType = Project.Scripts.Debug.LogType;
using Random = UnityEngine.Random;

namespace Project.Scripts.GameLogic.Character
{
    public class Tree: EntityBase
    {
        private TreeBonuses _treeBonuses;
        private TreeStats _treeStats;
        private float _currentArmor;

        private int _healthDebugTick;
        private int _armorDebugTick;
        
        public float CurrentArmor
        {
            get => _currentArmor;
            set
            {
                _currentArmor = value;
                _currentArmor = Mathf.Clamp(value, 0, _treeStats.Armor);
                OnArmorChange?.Invoke(_currentArmor);
            }
        }
        
        public event Action<float> OnArmorChange; 

        [Inject]
        private void Construct(TreeStats treeStats, TreeBonuses treeBonuses)
        {
            _treeStats = treeStats;
            _treeBonuses = treeBonuses;
        }
        
        private void Awake()
        {
            _treeBonuses.OnMaxHealthChanged += IncreaseMaxHealth;
            OnHealthChange += DebugHealthChange;
            OnArmorChange += DebugArmorChange;
        }

        private void Start()
        {
            SetInitialHealth(_treeStats.MaxHealth);
            CurrentArmor = _treeStats.Armor;
        }

        private void Update()
        {
            if(CurrentHealth < MaxHealth)
            {
                Heal(_treeStats.Regen * Time.deltaTime);
            }
        }

        private void OnDestroy()
        {
            _treeBonuses.OnMaxHealthChanged -= IncreaseMaxHealth;
            OnHealthChange -= DebugHealthChange;
            OnArmorChange -= DebugArmorChange;
        }
        
        private void OnWaveEnd()
        {
            _treeBonuses.Regen += 500;
        }
        
        private void OnWaveStart()
        {
            _treeBonuses.Regen -= 500;
            CurrentArmor = _treeStats.Armor;
        }

        public override void TakeDamage(float dmg)
        {
            var chance = Random.Range(0, 100);
            if (chance < _treeStats.Absorption)
            {
                DebugSystem.Instance.Log(LogType.Tree, "<color=green>Absorption proc!</color>");
                dmg = 1;
            }
            var armorDmg = dmg * 0.7f;
            var healthDmg = dmg * 0.3f;
            if (CurrentArmor > 0)
            {
                var baseArmorDmg = armorDmg;
                armorDmg -= CurrentArmor;
                CurrentArmor -= baseArmorDmg;
                if(armorDmg > 0)
                    healthDmg += armorDmg;
            }
            else
                healthDmg += armorDmg;
            CurrentHealth -= healthDmg;
            DebugSystem.Instance.Log(LogType.Tree, $"Tree takes <color=red>{dmg}</color> damage \n HP {CurrentHealth}/{MaxHealth} \n Armor {CurrentArmor}/{_treeStats.Armor}");
        }
        
        private void DebugHealthChange(OnHealthChangeArgs args)
        {
            _healthDebugTick++;
            if(_healthDebugTick == 50)
            {
                DebugSystem.Instance.Log(LogType.Tree, $"Health: {CurrentHealth}/{MaxHealth}");
                _healthDebugTick = 0;
            }
        }

        private void DebugArmorChange(float obj)
        {
            _armorDebugTick++;
            if(_armorDebugTick == 50)
            {
                DebugSystem.Instance.Log(LogType.Tree, $"Armor: {CurrentArmor}/{_treeStats.Armor}");
                _armorDebugTick = 0;
            }
        }
    }
}