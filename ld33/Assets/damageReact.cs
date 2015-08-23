using UnityEngine;
using System.Collections;

public class damageReact : MonoBehaviour
{
    public float reactionLength;
    private float maxReactionLength;

    public float reactionMag;

    private player _player;
    private Vector2 origin;
    private float currentHealth;
    void Start()
    {
        _player = player.Instance;
        origin = transform.position;
        currentHealth = _player.health;

        maxReactionLength = reactionLength;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth > _player.health)
        {
            reaction();
        }
    }

    void reaction()
    {
        reactionLength -= Time.deltaTime;
        Vector2 ranCir = origin + (Random.insideUnitCircle * reactionMag);
        transform.position = ranCir;

        if(reactionLength <= 0)
        {
            transform.position = origin;
            reactionLength = maxReactionLength;
            currentHealth = _player.health;
        }
    }
}
