using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
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

        [Header("Main Menu UI")]
        [SerializeField]
        private GameObject mainMenuPanel;

        [SerializeField]
        private Button newGameButton;

        [SerializeField]
        private Button loadGameButton;

        [SerializeField]
        private Button settingsButton;

        [SerializeField]
        private Button quitGameButton;

        [Header("Settings Menu UI")]
        [SerializeField]
        private GameObject settingsMenuPanel;

        [SerializeField]
        private GameObject settingsPanel;

        [SerializeField]
        private Button graphicsPanelButton;

        [SerializeField]
        private Button soundPanelButton;

        [SerializeField]
        private Button backSettingsMenuButton;

        [SerializeField]
        private GameObject graphicsPanel;

        [SerializeField]
        private GameObject soundPanel;

        [Space(10)]
        [Header("Graphics Settings UI")]
        [SerializeField]
        private TMP_Dropdown fpsDropdown;

        [SerializeField]
        private Dropdown graphicsQualityDropdown;

        [SerializeField]
        private Toggle vSyncToggle;

        [SerializeField]
        private Toggle antiAliasingToggle;

        [SerializeField]
        private Toggle shadowsToggle;

        [SerializeField]
        private Toggle bloomToggle;

        [SerializeField]
        private Button backGraphicsSettingsButton;

        [Header("Sound Settings UI")]
        [SerializeField]
        private Slider musicVolumeSlider;

        [SerializeField]
        private TMP_Text musicVolumeTextBG;

        [SerializeField]
        private Slider sfxVolumeSlider;

        [SerializeField]
        private TMP_Text musicVolumeTextSFX;

        [SerializeField]
        private Toggle musicToggle;

        [SerializeField]
        private Toggle sfxToggle;

        [SerializeField]
        private Button backSoundSettingsButton;

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

        [Header("Transition")]
        #region Change Child

        public GameObject maskA;
        public GameObject maskB;

        void ConfigTwoGameObjectOfTransition(GameObject newChild, GameObject oldChild)
        {
            var tempA = maskA.transform.GetChild(0);
            var tempB = maskB.transform.GetChild(0);
            //clear parent
            tempA.SetParent(null);
            tempB.SetParent(null);
            // Ensure both new and old child have FixPosition component
            if (!newChild.GetComponent<FixPosition>())
            {
                newChild.AddComponent<FixPosition>();
            }

            if (!oldChild.GetComponent<FixPosition>())
            {
                oldChild.AddComponent<FixPosition>();
            }

            //false mean left to right
            //true mean right to left
            if (!toggle)
            {
                newChild.transform.SetParent(maskA.transform);
                oldChild.transform.SetParent(maskB.transform);
            }
            else
            {
                newChild.transform.SetParent(maskB.transform);
                oldChild.transform.SetParent(maskA.transform);
            }
        }

        #endregion

        #region Playe Anim

        public GameObject WipeSlider;
        public Animator transitionAnimator;
        public float delayActive = 0.5f;

        //false mean left to right
        //true mean right to left
        public bool toggle = false;

        public void MakeTransition()
        {
            if (!toggle)
            {
                transitionAnimator.SetTrigger("Wipe A to B");
                toggle = true;
                return;
            }

            if (toggle)
            {
                transitionAnimator.SetTrigger("Wipe B to A");
                toggle = false;
                return;
            }
        }

        #endregion

        #region Button Click Handlers

        void GeneralClick(GameObject newChild, GameObject oldChild)
        {
            newChild.SetActive(true);
            oldChild.SetActive(true);
            ConfigTwoGameObjectOfTransition(newChild, oldChild);
            MakeTransition();

            DelayedDeactivationAsync().Forget();

            async UniTaskVoid DelayedDeactivationAsync()
            {
                await UniTask.Delay(TimeSpan.FromSeconds(delayActive));
                oldChild.SetActive(false);
            }
        }

        private async void OnNewGameClicked()
        {
            MakeTransition();
            await UniTask.Delay(TimeSpan.FromSeconds(delayActive));
            GameManager.Instance.StartNewGame();
        }

        private void OnLoadGameClicked()
        {
            GameManager.Instance.LoadGame();
        }

        private void OnSettingsClicked()
        {
            GeneralClick(settingsPanel, mainMenuPanel);
        }

        private void OnQuitClicked()
        {
            GameManager.Instance.QuitGame();
        }

        private void OnBackSettingsClicked()
        {
            ApplySettings();
            GeneralClick(mainMenuPanel, settingsMenuPanel);
        }

        private void OnSettingsPanelClicked()
        {
            GeneralClick(settingsMenuPanel, mainMenuPanel);
        }

        private void OnGraphicsPanelClicked()
        {
            GeneralClick(graphicsPanel, settingsPanel);
        }

        private void OnSoundPanelClicked()
        {
            GeneralClick(soundPanel, settingsPanel);
        }

        private void OnBackGraphicsSettingsClicked()
        {
            ApplySettings();
            GeneralClick(settingsPanel, graphicsPanel);
        }

        private void OnBackSoundSettingsClicked()
        {
            ApplySettings();
            GeneralClick(settingsPanel, soundPanel);
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
