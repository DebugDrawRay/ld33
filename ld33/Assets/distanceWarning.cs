using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class distanceWarning : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        GetComponent<Image>().enabled = gameController.Instance.warning;
	}
}
