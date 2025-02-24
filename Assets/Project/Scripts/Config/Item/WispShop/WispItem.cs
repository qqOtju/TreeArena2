using System;
using System.Text.RegularExpressions;
using Project.Scripts.GameLogic.Character.Decorator;
using Project.Scripts.Utils;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Project.Scripts.Config.Item.WispShop
{
    [CreateAssetMenu(menuName = "Item/Wisp Shop", fileName = "Wisp Shop Item")]
    public class WispItem: ScriptableObject
    {
        [Title("Info")]
        [SerializeField] private string _itemName;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;
        [SerializeField] private WispShopItemRarity _rarity;
        [SerializeField] private int _price;
        [Title("Wisp Stats")]
        [SerializeField] private float _damage;
        [SerializeField] private float _attackSpeed;
        [SerializeField] private float _criticalChance;
        [SerializeField] private float _criticalDamage;
        [SerializeField] private int _piercing;
        [SerializeField] private float _bonusEliteDamage;
        [SerializeField] private float _bonusBossDamage;
        [Title("Enemy Stats")]
        [SerializeField] private float _enemyMaxHealth;
        [SerializeField] private float _enemyMoveSpeed;
        [SerializeField] private float _enemyDamage;
        [SerializeField] private float _enemyAttackSpeed;
        [SerializeField] private float _enemyAttackRange;
        [Title("Wisp Decorator")]
        [ValueDropdown("GetWispDecoratorTypes")]
        [SerializeField] private string _wispDecoratorTypeName;
        
        public string ItemName => _itemName;
        public string Description => _description;
        public Sprite Icon => _icon;
        public WispShopItemRarity Rarity => _rarity;
        public int Price => _price;
        public float Damage => _damage;
        public float AttackSpeed => _attackSpeed;
        public float CriticalChance => _criticalChance;
        public float CriticalDamage => _criticalDamage;
        public int Piercing => _piercing;
        public float BonusEliteDamage => _bonusEliteDamage;
        public float BonusBossDamage => _bonusBossDamage;
        public float EnemyMaxHealth => _enemyMaxHealth;
        public float EnemyMoveSpeed => _enemyMoveSpeed;
        public float EnemyDamage => _enemyDamage;
        public float EnemyAttackSpeed => _enemyAttackSpeed;
        public float EnemyAttackRange => _enemyAttackRange;
        public Type WispDecoratorType => string.IsNullOrEmpty(_wispDecoratorTypeName) ? null : Type.GetType(_wispDecoratorTypeName);
        
        private static ValueDropdownList<string> GetWispDecoratorTypes()
        {
            var list = new ValueDropdownList<string>
            {
                { "None", "" } // Додаємо можливість вибрати null
            };
            foreach (var type in TypeCache.GetTypesDerivedFrom<WispDecorator>())
                if (!type.IsAbstract)
                {
                    var typeName = type.Name;
                    var cleanedName = typeName.Replace("WispDecorator", "");
                    var formattedName = Regex.Replace(cleanedName, @"(?<!^)([A-Z])", " $1");
                    list.Add(formattedName, type.AssemblyQualifiedName);
                }
            return list;
        }
    }
    
    public enum WispShopItemRarity
    {
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Unique
    }
}