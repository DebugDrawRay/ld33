using UnityEngine;
using System.Collections;

public class projectile : MonoBehaviour
{
    [Header("Movement Control")]
    public float speed;

    [Header("Time Control")]
    public float lifeTime;

    //components
    private Rigidbody rigid;

    void Awake()
    {
        initializeComponents();
    }

    void Start()
    {
        setSpeed();
    }

    void Update()
    {
        lifeTimer();
    }

    void initializeComponents()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void setSpeed()
    {
        rigid.velocity = transform.forward * speed;
    }

    void OnCollisionEnter(Collision hit)
    {
        Destroy(this.gameObject);
    }

    void lifeTimer()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
