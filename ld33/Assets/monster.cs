using UnityEngine;
using System.Collections;

public class monster : MonoBehaviour
{
    private delegate void stateContainer();
    private stateContainer currentState;

    [Header("Status Control")]
    public float health;
    private float damageTaken;

    private player _player;

    [Header("Patrol Control")]
    public float timeToNewWp;
    public float patrolTime;

    private float maxPatrolTime;
    private float maxTimeToNewWp;

    private Vector3 waypoint;

    [Header("Chase Control")]
    public float chaseDistance;
    public float damageThreshold;

    public float followSpeed;
    public float followAccel;

    public float findingTime;
    private float maxFindingTime;

    private bool hostileHit;
    private bool inSight;

    [Header("Dormancy Control")]
    public float dormantTime;
    public int dormantCycles;

    private float maxDormantTime;
    private int maxDormantCycles;

    [Header("Collisions")]
    public string[] hostileTags;
    public float hostileDamage;
    public AudioClip damageHit;

    //components
    private Rigidbody rigid;
    private gameController _gameController;
    public monsterFactory factory;
    //state control
    private bool patrolling;
    private bool dormant;
    private bool finding;

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
        maxDormantTime = dormantTime;
        maxFindingTime = findingTime;
        patrolling = true;
    }

    void initializeComponents()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!gameController.Instance.paused)
        {
            if (_player)
            {
                stateMachine();
            }
            if (health <= 0)
            {
                deathEvent();
            }
        }
    }

    void patrolController()
    {
        timeToNewWp -= Time.deltaTime;

        if (timeToNewWp <= 0)
        {
            waypoint = _gameController.getWaypoint();
            timeToNewWp = maxTimeToNewWp;
        }

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
            dormant = true;
        }
    }

    void chaseController()
    {
        followTarget(_player.transform.position);
        lookAtTarget(_player.transform.position);
    }

    float targetDist(GameObject target)
    {
        return Vector3.Distance(transform.position, target.transform.position);
    }

    void followTarget(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        Vector3 targetVel = direction; 
        targetVel = targetVel.normalized * followSpeed;
        Vector3 vel = Vector3.Lerp(rigid.velocity, targetVel, followAccel);
        rigid.velocity = vel;
    }

    void dormancyController()
    {
        dormantTime -= Time.deltaTime;
        patrolling = false;

        if(rigid.velocity.magnitude > 0)
        {
            rigid.velocity = Vector3.zero;
            dormantCycles--;
        }
        if (dormantTime <= 0)
        {
            if (dormantCycles <= 0)
            {
                dormantCycles = maxDormantCycles;
                finding = true;
                dormant = false;
                patrolling = true;
            }
            else
            {
                float ran = Random.value;
                if (ran > .5)
                {
                    patrolling = true;
                    dormantTime = maxDormantTime;
                    dormant = false;
                }
                else
                {
                    dormantTime = maxDormantTime;
                }
            }
        }
    }

    void findingController()
    {
        findingTime -= Time.deltaTime;
        patrolling = true;
        dormant = false;
        followTarget(_player.transform.position);
        lookAtTarget(_player.transform.position);

        if (findingTime <= 0)
        {
            findingTime = maxFindingTime;
            finding = false;
        }

    }

    void lookAtTarget(Vector3 target)
    {
        Vector3 relPos = target - transform.position;
        transform.rotation = Quaternion.LookRotation(relPos);
    }

    //collisions
    void OnTriggerEnter(Collider hit)
    {
        foreach(string tag in hostileTags)
        {
            if(hit.gameObject.tag == tag)
            {
                AudioSource.PlayClipAtPoint(damageHit, transform.position);
                health -= hostileDamage;
                damageTaken += hostileDamage;
                hostileHit = true;
            }
        }
    }

    //death event
    void deathEvent()
    {
        factory.spawnMonster();
        _gameController.killCount++;
        Destroy(this.gameObject);
    }
    void patrolState()
    {
        Debug.Log("Patrolling");
        patrolController();
        patrolTimer();
    }

    void chaseState()
    {
        Debug.Log("Chasing");
        chaseController();
    }

    void dormantState()
    {
        Debug.Log("Dormant");
        dormancyController();
    }
    void findingState()
    {
        Debug.Log("Finding Player");
        findingController();
    }

    void stateMachine()
    {
        if (hostileHit)
        {
            finding = true;
            patrolling = false;
            dormant = false;

            currentState = findingState;

            hostileHit = false;
        }

        if (finding)
        {
            currentState = findingState;
        }
        else
        {
            if (targetDist(_player.gameObject) <= chaseDistance)
            {
                patrolling = false;
                dormant = false;
            }
            else if (targetDist(_player.gameObject) >= chaseDistance)
            {
                patrolling = true;
            }

            if (dormant)
            {
                currentState = dormantState;
            }
            else
            {
                if (patrolling)
                {
                    currentState = patrolState;
                }
                else
                {
                    currentState = chaseState;
                    if (damageTaken >= damageThreshold)
                    {
                        damageTaken = 0;
                        patrolling = true;
                    }
                }
            }
        }
        
        currentState();
    }
}
