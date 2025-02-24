using System.Collections.Generic;
using Project.Scripts.Config.Item.WispShop;

namespace Project.Scripts.Module.ItemManager
{
    public class WispItemManager
    {
        private const float RarityUncommon = 0.325f;
        private const float RarityRare = 0.325f;
        private const float RarityEpic = 0.175f;
        private const float RarityLegendary = 0.1f;
        private const float RarityUnique = 0.075f;
        
        private readonly Dictionary<WispShopItemRarity, List<WispItem>> _wispShopItems = new();
        
        public List<WispItem> WispShopItems { get; }
        
        public WispItemManager(List<WispItem> wispShopItems)
        {
            WispShopItems = wispShopItems;
            foreach (var wispShopItem in wispShopItems)
            {
                _wispShopItems.TryAdd(wispShopItem.Rarity, new List<WispItem>());
                _wispShopItems[wispShopItem.Rarity].Add(wispShopItem);
            }
        }
        
        public void RemoveItem(WispItem item)
        {
            if (!WispShopItems.Contains(item)) return;
            WispShopItems.Remove(item);
            if(_wispShopItems.TryGetValue(item.Rarity, out var shopItem))
                shopItem.Remove(item);
        }
        
        public WispItem GetRandomItem()
        {
            var random = UnityEngine.Random.value;
            if (random < RarityUncommon 
                && _wispShopItems[WispShopItemRarity.Uncommon].Count > 0)
                return GetRandomItemByRarity(WispShopItemRarity.Uncommon);
            if (random < RarityUncommon + RarityRare 
                && _wispShopItems[WispShopItemRarity.Rare].Count > 0)
                return GetRandomItemByRarity(WispShopItemRarity.Rare);
            if (random < RarityUncommon + RarityRare + RarityEpic 
                && _wispShopItems[WispShopItemRarity.Epic].Count > 0)
                return GetRandomItemByRarity(WispShopItemRarity.Epic);
            if (random < RarityUncommon + RarityRare + RarityEpic + RarityLegendary 
                && _wispShopItems[WispShopItemRarity.Legendary].Count > 0)
                return GetRandomItemByRarity(WispShopItemRarity.Legendary);
            if(_wispShopItems[WispShopItemRarity.Unique].Count > 0)
                return GetRandomItemByRarity(WispShopItemRarity.Unique);
            return GetRandomItem();
        }

        private WispItem GetRandomItemByRarity(WispShopItemRarity uncommon)
        {
            return _wispShopItems[uncommon][UnityEngine.Random.Range(0, _wispShopItems[uncommon].Count)];
        }
    }
}