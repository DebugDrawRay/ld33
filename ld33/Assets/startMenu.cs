using UnityEngine;
using System.Collections;

public class startMenu : MonoBehaviour
{
    public string gameStartScene;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void startGame()
    {
        Application.LoadLevel(gameStartScene);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
