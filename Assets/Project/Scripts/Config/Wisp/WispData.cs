using Project.Scripts.GameLogic.Character.Attack;
using Project.Scripts.GameLogic.Character.Wisp;
using UnityEngine;

namespace Project.Scripts.Config.Wisp
{
    [CreateAssetMenu(menuName = "Data/Wisp Data", fileName = "Wisp Data")]
    public class WispData: ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private WispBase _wispsPrefab;
        [SerializeField] private WispConfig _wispConfig;
        [SerializeField] private Bullet _bulletPrefab;
        
        public Sprite Icon => _icon;
        public string Name => _name;
        public string Description => _description;
        public WispBase WispsPrefab => _wispsPrefab;
        public WispConfig WispConfig => _wispConfig;
        public Bullet BulletPrefab => _bulletPrefab;
    }
}