using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class GameManager : MonoBehaviour
{
    [CanBeNull] public GameObject Player;
    [CanBeNull] public GameObject ManagerObj;
    [CanBeNull] public GenerationManager genManager;
    [CanBeNull] public SceneLoader _sceneLoader;
    
    // Scene Address (Using buttons for easier scene path selection)
    [FoldoutGroup("Scene Address")]
    [SerializeField] public string ExpeditionPath;
    [FoldoutGroup("Scene Address")]
    [SerializeField] public string LobbyPath;

    [FoldoutGroup("Event")]
    public UnityEvent fadeInAnim, fadeOutAnim;
    public UnityEvent<Color> changeColorTransition;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (Player != null) DontDestroyOnLoad(Player);
            if (ManagerObj != null) DontDestroyOnLoad(ManagerObj);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadGame()
    {
        // Logic for loading a saved game
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    #region SceneLogic

    IEnumerator LoadExpedition()
    {
        fadeInAnim.Invoke();
        yield return new WaitForSeconds(1.5f);
        
        if (_sceneLoader != null)
            _sceneLoader.LoadScene(2);
        else
            SceneManager.LoadScene(ExpeditionPath);
        
        genManager.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        fadeOutAnim.Invoke();
    }

    IEnumerator LoadLobby()
    {
        fadeInAnim.Invoke();
        yield return new WaitForSeconds(1);
        
        if (_sceneLoader != null)
            _sceneLoader.LoadScene(1);
        else
            SceneManager.LoadScene(LobbyPath);
        
        genManager.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        fadeOutAnim.Invoke();
    }

    IEnumerator LoadMenu()
    {
        fadeInAnim.Invoke();
        yield return new WaitForSeconds(1.5f);
        
        if (_sceneLoader != null)
            _sceneLoader.LoadScene(0);
        else
            SceneManager.LoadScene("UI Main Menu");
        
        genManager.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        fadeOutAnim.Invoke();
    }

    public void StartExpedition()
    {
        PlayerController.Instance.PlayerProgressData.SaveClaimReward();
        StartCoroutine(LoadExpedition());
    }

    public void StartLobby()
    {
        StartCoroutine(LoadLobby());
    }

    public void QuitMenu()
    {
        StartCoroutine(LoadMenu());
    }

    #endregion

    // Editor-only methods to pick scene paths from file explorer
    #if UNITY_EDITOR
    [Button("Choose Expedition Scene")]
    private void ChooseExpeditionScene()
    {
        ExpeditionPath = EditorUtility.OpenFilePanel("Select Expedition Scene", "Assets/Scenes", "unity");
        if (!string.IsNullOrEmpty(ExpeditionPath))
        {
            ExpeditionPath = FileUtil.GetProjectRelativePath(ExpeditionPath);
        }
    }

    [Button("Choose Lobby Scene")]
    private void ChooseLobbyScene()
    {
        LobbyPath = EditorUtility.OpenFilePanel("Select Lobby Scene", "Assets/Scenes", "unity");
        if (!string.IsNullOrEmpty(LobbyPath))
        {
            LobbyPath = FileUtil.GetProjectRelativePath(LobbyPath);
        }
    }
    #endif
}
