using UnityEngine;
using System.Collections;

public class asteroid : MonoBehaviour
{
    [Header("Forces")]
    public float initialForce;

    void Start()
    {
        float x = Random.Range(0f, 1f);
        float y = Random.Range(0f, 1f);
        float z = Random.Range(0f, 1f);
        Vector3 forceVect = new Vector3(x, y, z);
        Debug.Log(forceVect * initialForce);
        GetComponent<Rigidbody>().velocity = forceVect * initialForce;
    }
}
