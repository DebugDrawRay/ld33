using UnityEngine;
using System.Collections;

public class monster : MonoBehaviour
{
    private delegate void stateContainer();
    private stateContainer currentState;

    [Header("Target Control")]
    private player _player;
    public float followSpeed;
    public float followAccel;

    [Header("Patrol Control")]
    private Vector3 waypoint;

    //components
    private Rigidbody rigid;

    private gameController _gameController;

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
    }

    void initializeComponents()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        stateMachine();
    }

    void patrol()
    {       
        followTarget(waypoint);
    }

    void followTarget(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        Vector3 targetVel = direction; //fix this, add control for velocity
        Vector3 vel = Vector3.Lerp(rigid.velocity, targetVel, followAccel);
        rigid.velocity = vel;
    }
    void patrolState()
    {
        patrol();
    }

    void chaseState()
    {
        followTarget(_player.transform.position);
    }

    void stateMachine()
    {
        currentState = patrolState;
        currentState();
    }
}
