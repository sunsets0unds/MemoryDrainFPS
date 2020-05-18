using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

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
}
