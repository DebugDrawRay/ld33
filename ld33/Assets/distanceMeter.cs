using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class distanceMeter : MonoBehaviour
{
    private Text meter;

    void Start()
    {
        meter = GetComponent<Text>();
    }
	void Update ()
    {
        meter.text = gameController.Instance.playerDist.ToString() + "KM";	
	}
}
