using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public int levelToLoad = 0;

    public void NewGame()
    {
        GameManager.ChangeLevel(levelToLoad);
    }

    public void Continue()
    {
        GameManager.LoadSavedLevel();
    }

    public void Exit()
    {
        GameManager.ExitGame();
    }
}
