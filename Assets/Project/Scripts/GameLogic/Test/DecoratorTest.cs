using Project.Scripts.Config.Enemy;
using Project.Scripts.GameLogic.Character.Decorator;
using Project.Scripts.GameLogic.Character.Wisp;
using Project.Scripts.GameLogic.Wave;
using Project.Scripts.Module.Factory;
using Project.Scripts.Module.Spawner;
using Project.Scripts.Module.Stats.Enemy;
using Project.Scripts.Module.Stats.Tree;
using Project.Scripts.Module.Stats.Wisp;
using UnityEngine;
using Zenject;
using Tree = Project.Scripts.GameLogic.Character.Tree;

namespace Project.Scripts.GameLogic.Test
{
    public class DecoratorTest: MonoBehaviour
    {
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private Tree _treeObject;
        [SerializeField] private EnemyData _enemyData;
        
        private WispBonuses _bonuses;
        private TreeBonuses _treeBonuses;
        private EnemyFactory _enemyFactory;
        private EnemyBonuses _enemyBonuses;
        private WispBase _wispObject;

        [Inject]
        private void Construct(WispBonuses wispBonuses, TreeBonuses treeBonuses, 
            EnemyFactory enemyFactory, EnemyBonuses enemyBonuses, WispBase wispObject)
        {
            _bonuses = wispBonuses;
            _treeBonuses = treeBonuses;
            _enemyFactory = enemyFactory;
            _enemyBonuses = enemyBonuses;
            _wispObject = wispObject;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
                DoubleAttack();
            if (Input.GetKeyDown(KeyCode.S))
                BackShot();
            if (Input.GetKeyDown(KeyCode.D))
                UniqueAttack();
            if (Input.GetKeyDown(KeyCode.F))
                InterestingMovement();
            if (Input.GetKeyDown(KeyCode.G))
                BulletsOnHealthHit();
            if (Input.GetKeyDown(KeyCode.H))
                ReAim();
            if (Input.GetKeyDown(KeyCode.J))
                Melee();
            if (Input.GetKeyDown(KeyCode.K))
                Sniper();
            if (Input.GetKeyDown(KeyCode.Q))
                AddDamage();
            if (Input.GetKeyDown(KeyCode.W))
                AddAttackSpeed();
            if (Input.GetKeyDown(KeyCode.E))
                AddCriticalChance();
            if (Input.GetKeyDown(KeyCode.R))
                AddCriticalDamage();
            if(Input.GetKeyDown(KeyCode.Keypad1))
                DamageTree();
            if(Input.GetKeyDown(KeyCode.Keypad7))
                AddMaxHealth();
            if(Input.GetKeyDown(KeyCode.Keypad8))
                AddRegen();
            if(Input.GetKeyDown(KeyCode.Keypad9))
                AddArmor();
            if(Input.GetKeyDown(KeyCode.Keypad4))
                AddAbsorption();
            if(Input.GetKeyDown(KeyCode.Alpha1))
                SpawnEnemy();
            if(Input.GetKeyDown(KeyCode.Alpha2))
                AddEnemyHealth();
            if(Input.GetKeyDown(KeyCode.Alpha3))
                AddEnemyDamage();
        }

        private void DoubleAttack()
        {
            _wispObject.AddDecorator<WispDecoratorDoubleAttack>();
        }

        private void BackShot()
        {
            _wispObject.AddDecorator<WispDecoratorBackShot>();
        }

        private void UniqueAttack()
        {
            _wispObject.AddDecorator<WispDecoratorUniqueBullet>();
        }
        
        private void InterestingMovement()
        {
            _wispObject.AddDecorator<WispDecoratorInterestingMovement>();
        }
        
        private void BulletsOnHealthHit()
        {
            _wispObject.AddDecorator<WispDecoratorBulletsOnTargetDeath>();
        }

        private void ReAim()
        {
            _wispObject.AddDecorator<WispDecoratorReAim>();
        }

        private void Melee()
        {
            _wispObject.AddDecorator<WispDecoratorMelee>();
        }
        
        private void Sniper()
        {
            _wispObject.AddDecorator<WispDecoratorSniper>();
        }

        private void AddDamage()
        {
            _bonuses.Damage += 10;
        }
        
        private void AddAttackSpeed()
        {
            _bonuses.AttackSpeed += 10f;
        }
        
        private void AddCriticalChance()
        {
            _bonuses.CriticalChance += 10f;
        }

        private void AddCriticalDamage()
        {
            _bonuses.CriticalDamage += 10f;
        }
        
        private void DamageTree()
        {
            _treeObject.TakeDamage(50);
        }
        
        private void AddMaxHealth()
        {
            _treeBonuses.MaxHealth += 10;
        }
        
        private void AddRegen()
        {
            _treeBonuses.Regen += 10;
        }
        
        private void AddArmor()
        {
            _treeBonuses.Armor += 10;
        }
        
        private void AddAbsorption()
        {
            _treeBonuses.Absorption += 10;
        }

        private void SpawnEnemy()
        {
            _enemySpawner.SpawnEnemy(SpawnPoint.Left, _enemyData);
        }

        private void AddEnemyHealth()
        {
            _enemyBonuses.MaxHealth += 50;
        }

        private void AddEnemyDamage()
        {
            _enemyBonuses.Damage += 10;
        }
    }
}