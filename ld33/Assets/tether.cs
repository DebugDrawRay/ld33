using UnityEngine;
using System.Collections;

public class tether : MonoBehaviour
{
    private LineRenderer connection;
	// Use this for initialization
	void Start ()
    {
        connection = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        connection.SetPosition(0, transform.position);
        connection.SetPosition(1, player.Instance.transform.position);
	}
}
