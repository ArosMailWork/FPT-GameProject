using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PA
{
    public class UIGraphicsMenu : MonoBehaviour
    {
        [SerializeField] private GameObject graphicsPanel;
        [SerializeField] private TMP_Dropdown fpsDropdown;
        [SerializeField] private Toggle vSyncToggle;
        [SerializeField] private Toggle antiAliasingToggle;
        [SerializeField] private Toggle bloomToggle;
        [SerializeField] private Button backGraphicsSettingsButton;

        private UIMainMenu mainMenu;
        private MakeTransitionUI makeTransitionUI;

        private void Awake()
        {
            mainMenu = GetComponentInParent<UIMainMenu>();
            makeTransitionUI = mainMenu.makeTransitionUI;
        }

        private void Start()
        {
            InitializeUI();
            LoadSettings();
        }

        private void InitializeUI()
        {
            fpsDropdown.onValueChanged.AddListener(DropdownFPSChanged);
            vSyncToggle.onValueChanged.AddListener(OnVSyncToggled);
            antiAliasingToggle.onValueChanged.AddListener(OnAntiAliasingToggled);
            bloomToggle.onValueChanged.AddListener(OnBloomToggled);
            backGraphicsSettingsButton.onClick.AddListener(OnBackGraphicsSettingsClicked);
        }

        private void LoadSettings()
        {
            GameSettings settings = GameSettings.Instance;
            vSyncToggle.isOn = settings.isVSyncEnabled;
            antiAliasingToggle.isOn = settings.isAntiAliasingEnabled;
            bloomToggle.isOn = settings.isBloomEnabled;
        }

        private void OnBackGraphicsSettingsClicked()
        {
            mainMenu.GeneralClick(mainMenu.SettingsMenu.SettingsPanel, graphicsPanel);
        }

        private void OnVSyncToggled(bool isOn)
        {
            GameSettings.Instance.isVSyncEnabled = isOn;
        }

        private void OnAntiAliasingToggled(bool isOn)
        {
            GameSettings.Instance.isAntiAliasingEnabled = isOn;
        }

        private void OnBloomToggled(bool isOn)
        {
            GameSettings.Instance.isBloomEnabled = isOn;
        }

        private void DropdownFPSChanged(int value)
        {
            QualitySettings.SetFPS(value);
        }

        public GameObject GraphicsPanel => graphicsPanel;
    }
} 