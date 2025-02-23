using System;
using Project.Scripts.Config.Item.Tree;
using Project.Scripts.Debug;

namespace Project.Scripts.Module.Stats.Tree
{
    public class TreeBonuses
    {
        private int _maxHealth;
        private float _regen;
        private int _armor;
        private float _absorption;
        
        public int MaxHealth
        {
            get => _maxHealth;
            set
            {
                if(value == _maxHealth) return;
                _maxHealth = value;
                DebugSystem.Instance.Log(LogType.TreeStats, $"Bonus max health: {_maxHealth}");
                OnMaxHealthChanged?.Invoke(_maxHealth);
            }
        }
        public float Regen
        {
            get => _regen;
            set
            {
                if(value == _regen) return;
                _regen = value;
                DebugSystem.Instance.Log(LogType.TreeStats, $"Bonus regen: {_regen}");
                OnRegenChanged?.Invoke(_regen);
            }
        }
        public int Armor
        {
            get => _armor;
            set
            {
                if(value == _armor) return;
                _armor = value;
                DebugSystem.Instance.Log(LogType.TreeStats, $"Bonus armor: {_armor}");
                OnArmorChanged?.Invoke(_armor);
            }
        }
        public float Absorption
        {
            get => _absorption;
            set
            {
                if(value == _absorption) return;
                _absorption = value;
                DebugSystem.Instance.Log(LogType.TreeStats, $"Bonus absorption: {_absorption}");
                OnResistChanged?.Invoke(_absorption);
            }
        }
        
        public event Action<int> OnMaxHealthChanged;
        public event Action<float> OnRegenChanged;
        public event Action<int> OnArmorChanged;
        public event Action<float> OnResistChanged;
        
        public void ApplyItemBonuses(TreeItem item)
        {
            MaxHealth += item.MaxHealth;
            Regen += item.Regen;
            Armor += item.Armor;
            Absorption += item.Absorption;
        }
    }
}