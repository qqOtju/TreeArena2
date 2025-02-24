using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.Config.Item.Tree
{
    [CreateAssetMenu(menuName = "Item/Tree", fileName = "Tree Item")]
    public class TreeItem: ScriptableObject
    {
        [Title("Info")]
        [SerializeField] private string _itemName;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;
        [SerializeField] private TreeItemRarity _rarity;
        [Title("Stats")]
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _regen;
        [SerializeField] private int _armor;
        [SerializeField] private float _absorption;
        
        public string ItemName => _itemName;
        public string Description => _description;
        public Sprite Icon => _icon;
        public TreeItemRarity Rarity => _rarity;
        public int MaxHealth => _maxHealth;
        public float Regen => _regen;
        public int Armor => _armor;
        public float Absorption => _absorption;
    }
    
    public enum TreeItemRarity
    {
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Unique //дають багато бонусів та багато мінусів
    }
}