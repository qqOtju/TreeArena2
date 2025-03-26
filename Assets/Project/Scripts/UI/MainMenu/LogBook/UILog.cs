using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.MainMenu.LogBook
{
    public class UILog: MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private Button _button;

        public Button ChooseButton => _button;

        public void Initialize(Sprite icon, string logName, string description)
        {
            _icon.sprite = icon;
            _name.text = logName;
            _description.text = description;
        }
    }
}