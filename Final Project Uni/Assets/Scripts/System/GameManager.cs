using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject Testpack;
    public static GameObject p { get; private set; }
    public string Expedition;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            p = Testpack;
            DontDestroyOnLoad(p);
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

    public void StartExpedition()
    {
        SceneManager.LoadScene(Expedition);
    }
    #endregion 

}