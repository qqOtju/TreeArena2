using System;
using System.Collections.Generic;
using System.Text;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Debug
{
    //ToDo: зробити #if (DEVELOPMENT_BUILD || UNITY_EDITOR) всюди де використовую DebugSystem.Instance.Log    
    public class DebugSystem: MonoBehaviour
    {
        [Title("Settings")]
        [SerializeField] private bool _logToConsole;
        [SerializeField] private bool _scrollToBottom;
        [Title("Window")]
        [SerializeField] private TMP_Text _logText;
        [SerializeField] private RectTransform _windowRect;
        [SerializeField] private ScrollRect _scrollRect;
        [Title("Type")]
        [SerializeField] private Button _typeButtonPrefab;
        [SerializeField] private Transform _typeButtonParent;
        [Title("Buttons")]
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _logToFileButton;
        [SerializeField] private Button _toggleWindowSizeButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _openButton;
        [SerializeField] private Button _limitLogEntriesButton;
        
        private const string ScreenshotPath = "Screenshot/";
        private const int MaxLogEntries = 30;
        private const string LogFileName = "log.txt";
        
        private Vector2 _originalAnchoredPosition;
        private StringBuilder _logBuilder = new StringBuilder();
        private Vector2 _originalSizeDelta;
        private Vector2 _originalMinAnchor;
        private Vector2 _originalMaxAnchor;
        private bool _isMaximized = false;
        private Dictionary<LogType, Log> _logs = new ();
        private bool _isPaused;
        private LogType _currentLogType;
        private int _currentLines;
        private bool _limitLogEntries = false;
        private int _screenshotIndex = 0;

        public static DebugSystem Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            CreateLogTypes();
            OnLogButtonClicked(LogType.All);
            _pauseButton.onClick.AddListener(OnPauseButtonClicked);
            _logToFileButton.onClick.AddListener(LogToFile);
            _toggleWindowSizeButton.onClick.AddListener(ToggleWindowSize);
            _closeButton.onClick.AddListener(Close);
            _openButton.onClick.AddListener(Open);
            _limitLogEntriesButton.onClick.AddListener(SetLimitLogEntries);
            _originalAnchoredPosition = _windowRect.anchoredPosition;
            _originalSizeDelta = _windowRect.sizeDelta;
            _originalMinAnchor = _windowRect.anchorMin;
            _originalMaxAnchor = _windowRect.anchorMax;
            SetLimitLogEntries();
        }

        private void Start()
        {
            _screenshotIndex = PlayerPrefs.GetInt("ScreenshotIndex", 0);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                Screenshot();
            }
        }

        private void OnDestroy()
        {
            if(Instance == this)
                Instance = null;
            _pauseButton.onClick.RemoveAllListeners();
            _logToFileButton.onClick.RemoveAllListeners();
            _toggleWindowSizeButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
            _openButton.onClick.RemoveAllListeners();
            _limitLogEntriesButton.onClick.RemoveAllListeners();
        }

        private void Screenshot()
        {
            var path = ScreenshotPath + $"screenshot_{_screenshotIndex}.png";
            ScreenCapture.CaptureScreenshot(path);
            _screenshotIndex++;
            PlayerPrefs.SetInt("ScreenshotIndex", _screenshotIndex);
        }

        private void SetLimitLogEntries()
        {
            _limitLogEntries = !_limitLogEntries;
            var image = _limitLogEntriesButton.GetComponentInChildren<Image>();
            image.color = _limitLogEntries ? Color.green : Color.red;
        }

        private void Close()
        {
            _openButton.gameObject.SetActive(true);
            _windowRect.gameObject.SetActive(false);
        }

        private void Open()
        {
            _openButton.gameObject.SetActive(false);
            _windowRect.gameObject.SetActive(true);
        }

        private void ToggleWindowSize()
        {
            if (_isMaximized)
                MinimizeWindow();
            else
                MaximizeWindow();
            _isMaximized = !_isMaximized;
        }

        private void MinimizeWindow()
        {
            _windowRect.anchorMin = _originalMinAnchor;
            _windowRect.anchorMax = _originalMaxAnchor;
            _windowRect.anchoredPosition = _originalAnchoredPosition;
            _windowRect.sizeDelta = _originalSizeDelta;
        }

        private void MaximizeWindow()
        {
            _windowRect.anchorMin = new Vector2(0, 0);
            _windowRect.anchorMax = new Vector2(1, 1);
            _windowRect.anchoredPosition = Vector2.zero;
            _windowRect.sizeDelta = Vector2.zero;
        }

        private void OnPauseButtonClicked()
        {
            _isPaused = !_isPaused;
            Time.timeScale = _isPaused ? 0 : 1;
        }

        private void CreateLogTypes()
        {
            foreach (var logType in Enum.GetValues(typeof(LogType)))
            {
                _logs.Add((LogType)logType, new Log());
                var button = Instantiate(_typeButtonPrefab, _typeButtonParent);
                button.GetComponentInChildren<TMP_Text>().text = logType.ToString();
                button.onClick.AddListener(() => OnLogButtonClicked((LogType)logType));
            }
        }

        [Title("Actions")] [Button(ButtonSizes.Medium)]
        private void LogToFile()
        {
            var lines = new List<string>();
            foreach (var message in _logs[LogType.All].Messages)
            {
                var text = $"[{message.LogType}] [{message.Time:HH:mm:ss}] {message.Message}";
                lines.Add(text);
            }
            System.IO.File.WriteAllLines(LogFileName, lines);
        }

        private void OnLogButtonClicked(LogType logType)
        {
            _currentLogType = logType;
            _logBuilder.Clear();
            _logText.SetText("");
            _currentLines = 0;
            foreach (var message in _logs[logType].Messages)
            {
                var logMessage = message;
                CreateLogText(logMessage);
            }
            if(_scrollToBottom)
                ScrollToBottom();
        }
        
        private void CreateLogText(LogMessage message)
        {
            var newLine = $"<color=grey>[{message.Time:HH:mm:ss}]</color> {message.Message}";
            _logBuilder.AppendLine(newLine);
            _currentLines++;
            if (_currentLines > MaxLogEntries && _limitLogEntries)
            {
                var index = _logBuilder.ToString().IndexOf('\n') + 1;
                _logBuilder.Remove(0, index);
                _currentLines--;
            }
            _logText.SetText(_logBuilder);
        }
        
        private void ScrollToBottom()
        {
            if(!_windowRect.gameObject.activeSelf) return;
            Canvas.ForceUpdateCanvases();
            _scrollRect.verticalNormalizedPosition = 0f;
        }
        
        [Button(ButtonStyle.CompactBox, Expanded = true)]//
        public void Log(LogType logType, string message)
        {
            if(_logToConsole)
                UnityEngine.Debug.Log(message);
            _logs[logType].AddMessage(logType, message);
            if(logType != LogType.All)
                _logs[LogType.All].AddMessage(logType, message);
            if(logType == _currentLogType || _currentLogType == LogType.All)
            {
                var logMessage = _logs[logType].Messages[^1];
                CreateLogText(logMessage);
            }
            if(_scrollToBottom)
                ScrollToBottom();
        }
    }
}