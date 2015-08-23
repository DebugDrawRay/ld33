using UnityEngine;
using System.Collections;

public class player : MonoBehaviour
{
    private delegate void stateContainer();
    private stateContainer currentState;

    [Header("Status Control")]
    public float health;
    public float maxHealth;
    public float hostileDamage;

    [Header("Movement Control")]
    public float maxSpeed;
    public float acceleration;
    public float mouseSmooth;
    public float sideStepPenalty;
    public float backMovePenalty;
    [Header("Weapon Control")]
    public GameObject projectile;

    [Header("Collisions")]
    public string[] hostileTags;
    public AudioClip damageHit;

    private bool knockback;
    private Vector3 knockbackDirection;
    public float knockbackTime;
    private float maxKnockbackTime;

    public float knockbackForce;

    
    //inputs
    private float hor;
    private float ver;
    private float mouseX;
    private float mouseY;
    private bool fire;

    //components
    private Rigidbody rigid;

    [Header("Camera Control")]
    private Quaternion camOriginalRot;

    //globals
    private float yRot;

    void Awake()
    {
        initializeInstance();
        initializeComponents();
    }

    void Start()
    {
        initializeVars();
    }

    public static player Instance
    {
        get;
        private set;
    }
    
    void initializeInstance()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    void initializeVars()
    {
        camOriginalRot = Camera.main.transform.rotation;
        maxHealth = health;
        maxKnockbackTime = knockbackTime;
    }
    void initializeComponents()
    {
        rigid = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (!gameController.Instance.paused)
        {
            inputListener();
            stateMachine();
            if (health <= 0)
            {
                deathEvent();
            }
        }
    }

    void inputListener()
    {
        hor = Input.GetAxisRaw(Inputs.horAxis);
        ver = Input.GetAxisRaw(Inputs.verAxis);
        fire = Input.GetButtonDown(Inputs.fire);
        mouseX = Input.GetAxis(Inputs.mouseX);
        mouseY = Input.GetAxis(Inputs.mouseY);
    }

    void mouseLook()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Vector3 xAxis = new Vector3(0, mouseX, 0);
        Vector3 yAxis = new Vector3(-mouseY,0,0);
        transform.Rotate(xAxis * mouseSmooth);
        transform.Rotate(yAxis * mouseSmooth);
       /* yRot += -mouseY * mouseSmooth;

        Quaternion yRotChange = Quaternion.AngleAxis(yRot, Vector3.right);

        Camera.main.transform.localRotation = camOriginalRot * yRotChange;*/
    }

    void movementControl()
    {
        Vector3 currentVel = rigid.velocity;
        hor = hor / sideStepPenalty;
        if(ver < 0)
        {
            ver = ver / backMovePenalty;
            hor = hor / backMovePenalty;
        }
        Vector3 targetVel = ((transform.forward * ver) + (transform.right * hor)) * maxSpeed;
        Vector3 forceVect = Vector3.Lerp(currentVel, targetVel, acceleration);
        rigid.velocity = forceVect;
    }

    void weaponControl()
    {
        if(fire)
        {
            Instantiate(projectile, transform.position, transform.rotation);
        }
    }

    void knockbackController()
    {
        knockbackTime -= Time.deltaTime;
        rigid.velocity = -knockbackDirection * knockbackForce;

        if(knockbackTime <= 0)
        {
            knockbackTime = maxKnockbackTime;
            knockback = false;
        }
    }

    void OnTriggerEnter(Collider hit)
    {
        foreach(string tag in hostileTags)
        {
            if(hit.gameObject.tag == tag)
            {
                knockback = true;
                knockbackDirection = hit.transform.position - transform.position;
                AudioSource.PlayClipAtPoint(damageHit, transform.position);
                health -= hostileDamage;
            }
        }
    }

    public void deathEvent()
    {
        Destroy(this.gameObject);
        Destroy(gameController.Instance.gameObject);
        Application.LoadLevel("End");
    }
    void movementState()
    {
        movementControl();
        mouseLook();
        weaponControl();
    }
    void knockbackState()
    {
        mouseLook();
        knockbackController();
    }
    void stateMachine()
    {
        if (knockback)
        {
            currentState = knockbackState;
        }
        else
        {
            currentState = movementState;
        }
        currentState();
    }
}
