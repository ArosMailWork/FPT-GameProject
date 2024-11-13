using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PA
{
    public class UISoundMenu : MonoBehaviour
    {
        [SerializeField] private GameObject soundPanel;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private TMP_Text musicVolumeTextBG;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private TMP_Text musicVolumeTextSFX;
        [SerializeField] private Button backSoundSettingsButton;

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
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            backSoundSettingsButton.onClick.AddListener(OnBackSoundSettingsClicked);
        }

        private void LoadSettings()
        {
            GameSettings settings = GameSettings.Instance;
            musicVolumeSlider.value = settings.musicVolume;
            sfxVolumeSlider.value = settings.sfxVolume;
        }

        private void OnBackSoundSettingsClicked()
        {
            mainMenu.GeneralClick(mainMenu.SettingsMenu.SettingsPanel, soundPanel);
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

        public GameObject SoundPanel => soundPanel;
    }
} 
 