using System.Collections.Generic;
using Project.Scripts.Config.Item.Tree;

namespace Project.Scripts.Module.ItemManager
{
    public class TreeItemManager
    {
        private const float RarityUncommon = 0.3f;
        private const float RarityRare = 0.3f;
        private const float RarityEpic = 0.2f;
        private const float RarityLegendary = 0.1f;
        private const float RarityUnique = 0.1f;
        
        private readonly Dictionary<TreeItemRarity, List<TreeItem>> _treeItems = new();

        public List<TreeItem> TreeItems { get; }

        public TreeItemManager(List<TreeItem> treeItems)
        {
            TreeItems = treeItems;
            foreach (var treeItem in treeItems)
            {
                _treeItems.TryAdd(treeItem.Rarity, new List<TreeItem>());
                _treeItems[treeItem.Rarity].Add(treeItem);
            }
        }
        
        public void RemoveItem(TreeItem item)
        {
            if (!TreeItems.Contains(item)) return;
            TreeItems.Remove(item);
            if(_treeItems.TryGetValue(item.Rarity, out var shopItem))
                shopItem.Remove(item);
        }

        public TreeItem GetRandomItem()
        {
            var random = UnityEngine.Random.value;
            if (random < RarityUncommon 
                && _treeItems[TreeItemRarity.Uncommon].Count > 0)
                return GetRandomItemByRarity(TreeItemRarity.Uncommon);
            if (random < RarityUncommon + RarityRare
                && _treeItems[TreeItemRarity.Rare].Count > 0)
                return GetRandomItemByRarity(TreeItemRarity.Rare);
            if (random < RarityUncommon + RarityRare + RarityEpic
                && _treeItems[TreeItemRarity.Epic].Count > 0)
                return GetRandomItemByRarity(TreeItemRarity.Epic);
            if (random < RarityUncommon + RarityRare + RarityEpic + RarityLegendary
                && _treeItems[TreeItemRarity.Legendary].Count > 0)
                return GetRandomItemByRarity(TreeItemRarity.Legendary);
            if(_treeItems[TreeItemRarity.Unique].Count > 0)
                return GetRandomItemByRarity(TreeItemRarity.Unique);
            return GetRandomItem();
        }

        private TreeItem GetRandomItemByRarity(TreeItemRarity uncommon)
        {
            return _treeItems[uncommon][UnityEngine.Random.Range(0, _treeItems[uncommon].Count)];
        }
    }
}