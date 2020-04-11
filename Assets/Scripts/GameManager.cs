using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int currentLevel;
    public static int savedLevel;
    private static GameManager gameManager;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (gameManager == null)
            gameManager = this;
        else
            Destroy(gameObject);

        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    private void Start()
    {
        if(PlayerPrefs.HasKey("SavedLevel"))
        {
            int temp = PlayerPrefs.GetInt("SavedLevel");
            if (savedLevel != temp)
                savedLevel = temp;
        }
        else
        {
            PlayerPrefs.SetInt("SavedLevel", currentLevel);
        }
    }

    private void Update()
    {
        PlayerPrefs.Save();
    }

    public static void ChangeLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
        PlayerPrefs.SetInt("SavedLevel", levelIndex);
    }

    public static void LoadSavedLevel()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("SavedLevel"));
        }
        else
            Debug.LogWarning("No saved level index");
    }

    public static void ExitGame()
    {
        Application.Quit();
        if(UnityEditor.EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.ExitPlaymode();
        }
    }
}
