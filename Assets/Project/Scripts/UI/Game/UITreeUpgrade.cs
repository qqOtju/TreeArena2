using System;
using Project.Scripts.GameLogic.Wave;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Game
{
    [RequireComponent(typeof(Canvas))]
    public class UITreeUpgrade: MonoBehaviour
    {
        [SerializeField] private WaveController _waveController;
        [SerializeField] private Button _closeButton;
        
        private Canvas _canvas;

        public event Action OnClose; 

        private void Awake()
        {
            _waveController.OnWaveEnd += OnWaveEndHandler;
            _closeButton.onClick.AddListener(OnCloseButtonClick);
        }

        private void Start()
        {
            _canvas = GetComponent<Canvas>();
        }

        private void OnCloseButtonClick()
        {
            _canvas.enabled = false;
            OnClose?.Invoke();
        }

        private void OnWaveEndHandler(int obj)
        {
            _canvas.enabled = true;
        }
    }
}