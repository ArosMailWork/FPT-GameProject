using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PA
{
    public class UIMainMenu : MonoBehaviour
    {
        public static UIMainMenu Instance { get; private set; }

        [Header("Main Menu UI")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button optionButton;
        [SerializeField] private Button quitGameButton;

        [Header("References")]
        public MakeTransitionUI makeTransitionUI;
        [SerializeField] private UISettingsMenu settingsMenu;
        [SerializeField] private UIGraphicsMenu graphicsMenu;
        [SerializeField] private UISoundMenu soundMenu;

        public SceneLoader _sceneLoader;

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

        public void GeneralClick(GameObject newChild, GameObject oldChild)
        {
            newChild.SetActive(true);
            oldChild.SetActive(true);
            makeTransitionUI.ConfigTwoGameObjectOfTransition(newChild, oldChild);
            makeTransitionUI.MakeTransition();

            DelayedDeactivationAsync().Forget();

            async UniTaskVoid DelayedDeactivationAsync()
            {
                await UniTask.Delay(TimeSpan.FromSeconds(makeTransitionUI.duration));
                oldChild.SetActive(false);
            }
        }
        
        public async void OnContinueClicked()
        {           
            makeTransitionUI.MakeFadeTransition();            
            await UniTask.Delay(TimeSpan.FromSeconds(makeTransitionUI.duration));
            
            if(_sceneLoader != null)
                _sceneLoader.LoadScene(1);
            else
                SceneManager.LoadScene("TestGenMap");
            
            await UniTask.Delay(TimeSpan.FromSeconds(makeTransitionUI.duration));
            makeTransitionUI.fadeBlackImage.GetComponent<CanvasGroup>().alpha = 1;
            gameObject.SetActive(false);
        }

        public async void OnNewGameClicked()
        {           
            makeTransitionUI.MakeFadeTransition();            
            await UniTask.Delay(TimeSpan.FromSeconds(makeTransitionUI.duration));
            
            if(_sceneLoader != null)
                _sceneLoader.LoadScene(1);
            else
                SceneManager.LoadScene("Lobby");
            
            await UniTask.Delay(TimeSpan.FromSeconds(makeTransitionUI.duration));
            makeTransitionUI.fadeBlackImage.GetComponent<CanvasGroup>().alpha = 1;
            gameObject.SetActive(false);
        }

        public void OnSettingsClicked()
        {
            GeneralClick(settingsMenu.SettingsPanel, mainMenuPanel);
        }

        public void OnQuitClicked()
        {
            GameManager.Instance.QuitGame();
        }

        // Properties for accessing panels
        public GameObject MainMenuPanel => mainMenuPanel;
        public UISettingsMenu SettingsMenu => settingsMenu;
        public UIGraphicsMenu GraphicsMenu => graphicsMenu;
        public UISoundMenu SoundMenu => soundMenu;
    }
}