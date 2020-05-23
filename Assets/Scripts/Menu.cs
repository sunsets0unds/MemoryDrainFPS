using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadBar;

    private void Start()
    {
        if (PlayerManager.ActivePlayer())
        {
            Destroy(PlayerManager.ActivePlayer().gameObject);
        }

        if (Cursor.lockState == CursorLockMode.Locked)
            Cursor.lockState = CursorLockMode.None;
    }

    public void LoadLevel(int levelIndex)
    {
        GameManager.ChangeLevel(levelIndex);
    }

    public void Continue()
    {
        GameManager.LoadSavedLevel();
    }

    public void Exit()
    {
        GameManager.ExitGame();
    }

    public void LoadLevelAsync(int levelIndex)
    {
        StartCoroutine(LoadAsync(levelIndex));
    }

    IEnumerator LoadAsync(int levelIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);

        if (loadingScreen)
            loadingScreen.SetActive(true);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            if (loadBar)
                loadBar.value = progress;

            Debug.Log(progress);

            yield return null;
        }
    }
}
