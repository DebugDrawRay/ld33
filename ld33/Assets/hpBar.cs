using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class hpBar : MonoBehaviour
{
    private player _player;
    private Image bar;
	void Start ()
    {
        _player = player.Instance;
        bar = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        float current = _player.health;
        float max = _player.maxHealth;

        bar.fillAmount = current / max;
	}
}
