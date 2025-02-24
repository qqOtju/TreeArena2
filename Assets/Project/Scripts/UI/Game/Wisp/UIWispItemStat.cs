using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Game.Wisp
{
    public class UIWispItemStat: MonoBehaviour
    {
        [Title("UI Elements")]
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _text;
        [Title("Icons")]
        [SerializeField] private Sprite _wispDamageIcon;
        [SerializeField] private Sprite _wispAttackSpeedIcon;
        [SerializeField] private Sprite _wispCriticalChanceIcon;
        [SerializeField] private Sprite _wispCriticalDamageIcon;
        [SerializeField] private Sprite _wispPiercingIcon;
        [SerializeField] private Sprite _wispBonusEliteDamageIcon;
        [SerializeField] private Sprite _wispBonusBossDamageIcon;
        [SerializeField] private Sprite _enemyMaxHealthIcon;
        [SerializeField] private Sprite _enemyMoveSpeedIcon;
        [SerializeField] private Sprite _enemyDamageIcon;
        [SerializeField] private Sprite _enemyAttackSpeedIcon;
        [SerializeField] private Sprite _enemyAttackRangeIcon;

        public void Initialize(WispItemStatType type, string value)
        {
            _text.text = value;
            switch (type)
            {
                case WispItemStatType.Damage:
                    _icon.sprite = _wispDamageIcon;
                    break;
                case WispItemStatType.AttackSpeed:
                    _icon.sprite = _wispAttackSpeedIcon;
                    break;
                case WispItemStatType.CriticalChance:
                    _icon.sprite = _wispCriticalChanceIcon;
                    break;
                case WispItemStatType.CriticalDamage:
                    _icon.sprite = _wispCriticalDamageIcon;
                    break;
                case WispItemStatType.Piercing:
                    _icon.sprite = _wispPiercingIcon;
                    break;
                case WispItemStatType.BonusEliteDamage:
                    _icon.sprite = _wispBonusEliteDamageIcon;
                    break;
                case WispItemStatType.BonusBossDamage:
                    _icon.sprite = _wispBonusBossDamageIcon;
                    break;
                case WispItemStatType.EnemyMaxHealth:
                    _icon.sprite = _enemyMaxHealthIcon;
                    break;
                case WispItemStatType.EnemyMoveSpeed:
                    _icon.sprite = _enemyMoveSpeedIcon;
                    break;
                case WispItemStatType.EnemyDamage:
                    _icon.sprite = _enemyDamageIcon;
                    break;
                case WispItemStatType.EnemyAttackSpeed:
                    _icon.sprite = _enemyAttackSpeedIcon;
                    break;
                case WispItemStatType.EnemyAttackRange:
                    _icon.sprite = _enemyAttackRangeIcon;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
    
    [Flags]
    public enum WispItemStatType
    {
        Damage = 1 << 0,
        AttackSpeed = 1 << 1,
        CriticalChance = 1 << 2,
        CriticalDamage = 1 << 3,
        Piercing = 1 << 4,
        BonusEliteDamage = 1 << 5,
        BonusBossDamage = 1 << 6,
        EnemyMaxHealth = 1 << 7,
        EnemyMoveSpeed = 1 << 8,
        EnemyDamage = 1 << 9,
        EnemyAttackSpeed = 1 << 10,
        EnemyAttackRange = 1 << 11
    }
}