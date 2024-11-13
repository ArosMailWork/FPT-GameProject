using UnityEngine;
using UnityEngine.UI;

namespace PA
{
    public class UISettingsMenu : MonoBehaviour
    {
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private Button graphicsPanelButton;
        [SerializeField] private Button soundPanelButton;
        [SerializeField] private Button backSettingsMenuButton;

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
        }

        private void InitializeUI()
        {
            backSettingsMenuButton.onClick.AddListener(OnBackSettingsClicked);
            graphicsPanelButton.onClick.AddListener(OnGraphicsPanelClicked);
            soundPanelButton.onClick.AddListener(OnSoundPanelClicked);
        }

        private void OnBackSettingsClicked()
        {
            mainMenu.GeneralClick(mainMenu.MainMenuPanel, settingsPanel);
        }

        private void OnGraphicsPanelClicked()
        {
            mainMenu.GeneralClick(mainMenu.GraphicsMenu.GraphicsPanel, settingsPanel);
        }

        private void OnSoundPanelClicked()
        {
            mainMenu.GeneralClick(mainMenu.SoundMenu.SoundPanel, settingsPanel);
        }

        public GameObject SettingsPanel => settingsPanel;
    }
} 