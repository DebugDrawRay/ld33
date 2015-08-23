using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class killCount : MonoBehaviour
{

    void Update()
    {
        GetComponent<Text>().text = gameController.Instance.killCount.ToString();
    }

}
