using UnityEngine;
using System.Collections;

public class gameController : MonoBehaviour
{
    [Header("Game Area Control")]
    public float gameAreaRadius;
    public float firstWarnDist;
    public float lastWarnDist;

    public bool warning;

    public int killCount;

    public int playerDist;

    public bool paused;
    public GameObject pauseScreen;

    void Awake()
    {
        initializeInstance();
        pauseGame(false);
    }

    public static gameController Instance
    {
        get;
        private set;
    }

    void initializeInstance()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        enforceGameArea();
        if(Input.GetButtonDown(Inputs.pause))
        {
            if (paused)
            {
                pauseGame(false);
            }
            else
            {
                pauseGame(true);
            }
        }
    }
    public Vector3 getWaypoint()
    {
        Vector3 ranSphere = Random.insideUnitSphere;
        Vector3 ranPos = (ranSphere * gameAreaRadius) + transform.position;
        return ranPos;
    }

    void enforceGameArea()
    {
        playerDist = (int)Mathf.Round(Vector3.Distance(player.Instance.transform.position, transform.position));
        if (playerDist >= gameAreaRadius)
        {
            player.Instance.deathEvent();
        }
        else if (playerDist >= (gameAreaRadius - lastWarnDist))
        {

        }
        else if (playerDist >= (gameAreaRadius - firstWarnDist))
        {
            warning = true;
        }
        else
        {
            warning = false;
        }
      
    }

    public void pauseGame(bool pause)
    {
        if(pause)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            paused = true;
            pauseScreen.SetActive(paused);
            Time.timeScale = 0;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            paused = false;
            pauseScreen.SetActive(paused);
            Time.timeScale = 1;
        }
    }

    public void quitGame()
    {
        Application.Quit();
    }
    
}
