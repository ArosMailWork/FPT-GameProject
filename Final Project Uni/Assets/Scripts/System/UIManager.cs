using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PA
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        [Header("Main Menu UI")] [SerializeField]
        private GameObject mainMenuPanel;

        [SerializeField] private Button newGameButton;
        [SerializeField] private Button loadGameButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button quitGameButton;

        [Header("Settings Menu UI")] [SerializeField]
        private GameObject settingsMenuPanel;

        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private Button graphicsPanelButton;
        [SerializeField] private Button soundPanelButton;
        [SerializeField] private Button backSettingsMenuButton;

        [SerializeField] private GameObject graphicsPanel;
        [SerializeField] private GameObject soundPanel;

        [Space(10)] [Header("Graphics Settings UI")] [SerializeField]
        private TMP_Dropdown fpsDropdown;

        [SerializeField] private Dropdown graphicsQualityDropdown;
        [SerializeField] private Toggle vSyncToggle;
        [SerializeField] private Toggle antiAliasingToggle;
        [SerializeField] private Toggle shadowsToggle;
        [SerializeField] private Toggle bloomToggle;
        [SerializeField] private Button backGraphicsSettingsButton;

        [Header("Sound Settings UI")] 
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private TMP_Text musicVolumeTextBG;

        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private TMP_Text musicVolumeTextSFX;

        [SerializeField] private Toggle musicToggle;
        
        [SerializeField] private Toggle sfxToggle;
        [SerializeField] private Button backSoundSettingsButton;

        private void Start()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Main Menu
            newGameButton.onClick.AddListener(OnNewGameClicked);
            loadGameButton.onClick.AddListener(OnLoadGameClicked);
            settingsButton.onClick.AddListener(OnSettingsClicked);
            quitGameButton.onClick.AddListener(OnQuitClicked);

            // Settings Menu
            backSettingsMenuButton.onClick.AddListener(OnBackSettingsClicked);
            graphicsPanelButton.onClick.AddListener(OnGraphicsPanelClicked);
            soundPanelButton.onClick.AddListener(OnSoundPanelClicked);

            // Graphics Settings
            fpsDropdown.onValueChanged.AddListener(DropdownFPSChanged);
            //graphicsQualityDropdown.onValueChanged.AddListener(OnGraphicsQualityChanged);
            vSyncToggle.onValueChanged.AddListener(OnVSyncToggled);
            antiAliasingToggle.onValueChanged.AddListener(OnAntiAliasingToggled);
//            shadowsToggle.onValueChanged.AddListener(OnShadowsToggled);
            bloomToggle.onValueChanged.AddListener(OnBloomToggled);
            backGraphicsSettingsButton.onClick.AddListener(OnBackGraphicsSettingsClicked);

            // Sound Settings
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
//            musicToggle.onValueChanged.AddListener(OnMusicToggled);
           // sfxToggle.onValueChanged.AddListener(OnSFXToggled);
            backSoundSettingsButton.onClick.AddListener(OnBackSoundSettingsClicked);


            LoadSettingsUI();
        }

        private void LoadSettingsUI()
        {
            GameSettings settings = GameSettings.Instance;
            musicVolumeSlider.value = settings.musicVolume;
            sfxVolumeSlider.value = settings.sfxVolume;
           // musicToggle.isOn = settings.isMusicEnabled;
           // sfxToggle.isOn = settings.isSFXEnabled;
            //graphicsQualityDropdown.value = settings.graphicsQuality;
            vSyncToggle.isOn = settings.isVSyncEnabled;
            antiAliasingToggle.isOn = settings.isAntiAliasingEnabled;
           // shadowsToggle.isOn = settings.isShadowsEnabled;
            bloomToggle.isOn = settings.isBloomEnabled;
            // TODO: Set FPS and Resolution dropdown values
        }

        #region Button Click Handlers

        private void OnNewGameClicked()
        {
            GameManager.Instance.StartNewGame();
        }

        private void OnLoadGameClicked()
        {
            GameManager.Instance.LoadGame();
        }

        private void OnSettingsClicked()
        {
            mainMenuPanel.SetActive(false);
            settingsMenuPanel.SetActive(true);
        }

        private void OnQuitClicked()
        {
            GameManager.Instance.QuitGame();
        }

        private void OnBackSettingsClicked()
        {
            ApplySettings();
            settingsMenuPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }

        private void OnSettingsPanelClicked()
        {
            settingsMenuPanel.SetActive(true);
        }

        private void OnGraphicsPanelClicked()
        {
            settingsPanel.SetActive(false);
            graphicsPanel.SetActive(true);
        }

        private void OnSoundPanelClicked()
        {
            settingsPanel.SetActive(false);
            soundPanel.SetActive(true);
        }

        private void OnBackGraphicsSettingsClicked()
        {
            ApplySettings();
            graphicsPanel.SetActive(false);
            settingsPanel.SetActive(true);
        }

        private void OnBackSoundSettingsClicked()
        {
            ApplySettings();
            soundPanel.SetActive(false);
            settingsPanel.SetActive(true);
        }

        #endregion

        #region Settings Change Handlers

        private void OnResolutionChanged(int value)
        {
            //GameSettings.Instance.resolution = value;
        }

        private void OnGraphicsQualityChanged(int value)
        {
            GameSettings.Instance.graphicsQuality = value;
        }

        private void OnFullscreenToggled(bool isOn)
        {
            GameSettings.Instance.isFullScreen = isOn;
        }

        private void OnVSyncToggled(bool isOn)
        {
            GameSettings.Instance.isVSyncEnabled = isOn;
        }

        private void OnAntiAliasingToggled(bool isOn)
        {
            GameSettings.Instance.isAntiAliasingEnabled = isOn;
        }

        private void OnShadowsToggled(bool isOn)
        {
            GameSettings.Instance.isShadowsEnabled = isOn;
        }

        private void OnBloomToggled(bool isOn)
        {
            GameSettings.Instance.isBloomEnabled = isOn;
        }

        private void OnMusicVolumeChanged(float value)
        {
            GameSettings.Instance.musicVolume = value;
            SoundManager.Instance.SetMusicVolume(value);
            musicVolumeTextBG.text = value.ToString();
        }

        private void OnSFXVolumeChanged(float value)
        {
            GameSettings.Instance.sfxVolume = value;
            SoundManager.Instance.SetSFXVolume(value);
            musicVolumeTextSFX.text = value.ToString();
        }

        private void OnMusicToggled(bool isOn)
        {
            GameSettings.Instance.isMusicEnabled = isOn;
            SoundManager.Instance.ToggleMusic();
        }

        private void OnSFXToggled(bool isOn)
        {
            GameSettings.Instance.isSFXEnabled = isOn;
            SoundManager.Instance.ToggleSFX();
        }

        #endregion


        private void DropdownFPSChanged(int value)
        {
            QualitySettings.SetFPS(value);
        }

        private void ApplySettings()
        {
            // GameSettings.Instance.SaveSettings();
            // QualitySettings.ApplySettings(
            //     GameSettings.Instance.graphicsQuality,
            //     GameSettings.Instance.isVSyncEnabled,
            //     GameSettings.Instance.isAntiAliasingEnabled,
            //     GameSettings.Instance.isShadowsEnabled,
            //     GameSettings.Instance.isBloomEnabled
            // );
        }
    }
}