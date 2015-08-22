using UnityEngine;
using System.Collections;

public class gameController : MonoBehaviour
{
    [Header("Game Area Control")]
    public float gameAreaRadius;


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

    public Vector3 getWaypoint()
    {
        Vector3 ranSphere = Random.insideUnitSphere;
        Vector3 ranPos = (ranSphere * gameAreaRadius) + transform.position;
        return ranPos;
    }

}
