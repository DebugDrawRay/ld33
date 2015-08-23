using UnityEngine;
using System.Collections;

public class gameController : MonoBehaviour
{
    [Header("Game Area Control")]
    public float gameAreaRadius;
    public float firstWarnDist;
    public float lastWarnDist;

    public int playerDist;
    void Awake()
    {
        initializeInstance();
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
         
        }
        else
        {
        }
      
    }
}
