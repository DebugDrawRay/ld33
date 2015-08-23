using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class minimapPlayer : MonoBehaviour
{
    private GameObject _monster;
    private player _player;

    private Image icon;
    private RectTransform rectTrans;


    public float scaling;
    public float trackingDistance;
    void Start()
    {
        _monster = GameObject.FindGameObjectWithTag("Enemy");
        _player = player.Instance;
        icon = GetComponent<Image>();
        rectTrans = GetComponent<RectTransform>();
        icon.enabled = false;
    }

    void Update()
    {
        if (_monster)
        {
            if (Vector3.Distance(_player.transform.position, _monster.transform.position) <= trackingDistance)
            {
                icon.enabled = true;
                Vector2 monPos = new Vector2(_monster.transform.position.x, _monster.transform.position.y);
                Vector2 playerPos = new Vector2(_player.transform.position.x, _player.transform.position.y);
                Vector2 relPos = playerPos - monPos;
                rectTrans.anchoredPosition = relPos / scaling;
            }
            else
            {
                icon.enabled = false;
            }
        }
    }
}
