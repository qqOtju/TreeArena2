using System;
using Project.Scripts.GameLogic.Character.Decorator;
using Project.Scripts.GameLogic.Character.Wisp;
using UnityEngine;

namespace Project.Scripts.GameLogic.Test
{
    public class DecoratorTest: MonoBehaviour
    {
        [SerializeField] private GameObject _wispObject;
        
        private IWisp _wisp;

        private void Start()
        {
            _wisp = _wispObject.GetComponent<IWisp>();
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
        }

        private void DoubleAttack()
        {
            _wisp.AddDecorator<WispDecoratorDoubleAttack>();
        }

        private void BackShot()
        {
            _wisp.AddDecorator<WispDecoratorBackShot>();
        }

        private void UniqueAttack()
        {
            _wisp.AddDecorator<WispDecoratorUniqueBullet>();
        }
        
        private void InterestingMovement()
        {
            _wisp.AddDecorator<WispDecoratorInterestingMovement>();
        }
        
        private void BulletsOnHealthHit()
        {
            _wisp.AddDecorator<WispDecoratorBulletsOnTargetDeath>();
        }

        private void ReAim()
        {
            _wisp.AddDecorator<WispDecoratorReAim>();
        }

        private void Melee()
        {
            _wisp.AddDecorator<WispDecoratorMelee>();
        }
    }
}