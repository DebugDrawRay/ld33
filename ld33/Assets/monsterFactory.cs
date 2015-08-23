using UnityEngine;
using System.Collections;

public class monsterFactory : MonoBehaviour
{
    [Header("Monsters")]
    public GameObject monster;
    private GameObject monsterSpawned;

    [Header("Controls")]
    public int count;
    public float minRange;
    private float maxRange;

    void Start()
    {
        maxRange = gameController.Instance.gameAreaRadius;
        spawnMonster();
    }
    public void spawnMonster()
    {
        Vector3 ranSphere = Random.insideUnitSphere;
        float ranRange = Random.Range(minRange, maxRange);
        Vector3 ranPos = (ranSphere * ranRange) + transform.position;
        monsterSpawned = Instantiate(monster, ranPos, Quaternion.identity) as GameObject;
        monsterSpawned.GetComponent<monster>().factory = this;
    }
}
