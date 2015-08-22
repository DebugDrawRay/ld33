using UnityEngine;
using System.Collections;

public class monster : MonoBehaviour
{
    private delegate void stateContainer();
    private stateContainer currentState;

    [Header("Status Control")]
    public float health;
    private float damageTaken;

    [Header("Target Control")]
    private player _player;
    public float followSpeed;
    public float followAccel;

    [Header("Patrol Control")]
    public float timeToNewWp;
    public float patrolTime;

    private float maxPatrolTime;
    private float maxTimeToNewWp;

    private Vector3 waypoint;

    [Header("Chase Control")]
    public float chaseDistance;
    public float damageThreshold;

    private bool hostileHit;
    private bool inSight;

    [Header("Collisions")]
    public string[] hostileTags;

    //components
    private Rigidbody rigid;
    private gameController _gameController;

    //state control
    private bool patrolling;
    private bool dormant;

    void Awake()
    {
        initializeComponents();
    }
    void Start()
    {
        initializeVars();
    }

    void initializeVars()
    {
        _player = player.Instance;
        _gameController = gameController.Instance;

        waypoint = _gameController.getWaypoint();

        maxTimeToNewWp = timeToNewWp;
        maxPatrolTime = patrolTime;
        patrolling = true;
    }

    void initializeComponents()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        stateMachine();
    }

    void patrolController()
    {       
        followTarget(waypoint);
    }
    
    void patrolTimer()
    {
        if(patrolTime > 0)
        {
            patrolTime -= Time.deltaTime;
        }
        else if(patrolTime <= 0)
        {
            patrolTime = maxPatrolTime;
            patrolling = false;
        }
    }

    void chaseController()
    {
        followTarget(_player.transform.position);
    }

    float targetDist(GameObject target)
    {
        return Vector3.Distance(transform.position, target.transform.position);
    }

    void followTarget(Vector3 target)
    {
        timeToNewWp -= Time.deltaTime;

        if (timeToNewWp <= 0)
        {
            waypoint = _gameController.getWaypoint();
            timeToNewWp = maxTimeToNewWp;
        }

        Vector3 direction = target - transform.position;
        Vector3 targetVel = direction; 
        targetVel = targetVel.normalized * followSpeed;
        Vector3 vel = Vector3.Lerp(rigid.velocity, targetVel, followAccel);
        rigid.velocity = vel;
    }

    //collisions
    void OnTriggerEnter(Collider hit)
    {
        foreach(string tag in hostileTags)
        {
            if(hit.gameObject.tag == tag)
            {
                health -= 10;
                damageTaken += 10;
                hostileHit = true;
            }
        }
    }

    void patrolState()
    {
        patrolController();
        patrolTimer();
    }

    void chaseState()
    {
        chaseController();
    }

    void stateMachine()
    {
        if (dormant)
        if(patrolling)
        {
            currentState = patrolState;

            if (targetDist(_player.gameObject) <= chaseDistance)
            {
                patrolling = false;
            }

            if (hostileHit)
            {
                hostileHit = false;
                patrolling = false;
            }
        }
        else
        {
            currentState = chaseState;
            if(damageTaken >= damageThreshold)
            {
                damageTaken = 0;
                patrolling = true;
            }
        }
        
        currentState();
    }
}
