using Project.Scripts.GameLogic.Movement;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.GameLogic.Character
{
    [SelectionBase]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player: MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [Title("Values")]
        [ShowInInspector] private const float DefaultSpeed = 4f;

        private BasicMovement _basicMovement;
        private Rigidbody2D _rb;
        private Vector2 _moveInput;
        
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _basicMovement = new BasicMovement(_rb);
        }

        private void Update()
        {
            _moveInput.x = Input.GetAxis("Horizontal");
            _moveInput.y = Input.GetAxis("Vertical");
            AnimationCycle();
        }

        private void FixedUpdate()
        {
            _basicMovement.Move(_moveInput ,DefaultSpeed);
        }

        private void AnimationCycle()
        {
            if(_moveInput.x > 0)
                _spriteRenderer.flipX = false;
            else if(_moveInput.x < 0)
                _spriteRenderer.flipX = true;
        }
    }
}