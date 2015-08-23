using UnityEngine;
using System.Collections;

public class asteroidFactory : MonoBehaviour
{
    [Header("Objects")]
    public GameObject asteroid;
    [Header("Controls")]
    public int minCount;
    public int maxCount;
    public float minRange;
    private float maxRange;

    void Start()
    {
        maxRange = gameController.Instance.gameAreaRadius;
        int count = Random.Range(minCount, maxCount);
        for (int i = 0; i <= count; i++)
        {
            Vector3 ranSphere = Random.insideUnitSphere;
            float ranRange = Random.Range(minRange, maxRange);
            Vector3 ranPos = (ranSphere * ranRange) + transform.position;
            Instantiate(asteroid, ranPos, Quaternion.identity);
        }
    }
}
