using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private void Start()
    {
        if(PlayerManager.ActivePlayer())
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
}
