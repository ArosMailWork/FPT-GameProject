using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject Testpack;
    public GameObject genManager;
    //Scene Address
    [FoldoutGroup("Scene Address")]
    public string Expedition;
    [FoldoutGroup("Scene Address")]
    public string Lobby;

    [FoldoutGroup("Event")]
    public UnityEvent fadeInAnim, fadeOutAnim;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Testpack);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        // Initialize game systems here
        // For example:
        // LoadPlayerData();
        // SetupAudio();
        // InitializeUI();
    }

    public void StartNewGame()
    {
        // Logic for starting a new game
        //SceneManager.LoadScene("MainGameScene");
    }

    public void LoadGame()
    {
        // Logic for loading a saved game
    }

    public void SaveGame()
    {
        // Logic for saving the game
    }

    public void QuitGame()
    {
        // Logic for quitting the game
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
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(Expedition);
        genManager.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        fadeOutAnim.Invoke();

    }
    IEnumerator LoadLobby()
    {
        fadeInAnim.Invoke();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(Lobby);
        genManager.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        fadeOutAnim.Invoke();

    }
    public void StartExpedition()
    {
        StartCoroutine(LoadExpedition());
    }
    public void StartLobby()
    {
        StartCoroutine(LoadLobby());
    }
    #endregion


}