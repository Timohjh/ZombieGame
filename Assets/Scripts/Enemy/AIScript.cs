using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum State
{
    Idle,
    Wander,
    Aware,
    Damage
}

[RequireComponent(typeof(NavMeshAgent))]
public class AIScript : MonoBehaviour
{
    #region Config
    [Header("Setup")]
    public Transform player;
    NavMeshAgent _agent;
    Animator _animator;
    State _state;
    Collider[] _colliders;
    Transform _transform;
    PlayerPVE playerPVE;

    [HideInInspector] public bool isDead = false;
    [HideInInspector] public float hostileTimer;
    [HideInInspector] public bool punchDone = false;
    float _stateTimer = 0f;
    bool _isAware = false;
    bool _isDetecting = false;
    float _health = 100f;

    [SerializeField] GameObject[] waypoints;
    Vector3 _wanderPoint;
    int waypointIndex = 0;

    [Header("Events")]
    public UnityEvent OnAwareEvent;

    [ExposedScriptableObject]
    public ZombieData zombieData;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        _animator = GetComponentInChildren<Animator>();
        _colliders = GetComponentsInChildren<Collider>();
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        playerPVE = player.GetComponentInParent<PlayerPVE>();
    }
    #endregion

    #region State management
    void Start()
    {
        _transform = transform;
        _wanderPoint = RandomWayPoint();
        _state = State.Wander;
        waypointIndex = Random.Range(0, waypoints.Length - 1);
        _health = zombieData.health;
        hostileTimer = zombieData.hostileTimer;
    }

    void OnEnable() => GameManager.aliveMobs.Add(gameObject);

    void OnDisable()
    {
        if(!isDead) GameManager.deadMobs.Remove(gameObject);
        GameManager.aliveMobs.Remove(gameObject);
    }

    void Update()
    {
        switch (_state)
        {
            default:
            case State.Idle:
                _animator.SetBool("Centry", false);
                _animator.SetBool("Aware", false);
                _agent.speed = 0f;
                _stateTimer += Time.deltaTime;
                if (_stateTimer > zombieData.idleTime)
                {
                    RandomWayPoint();
                    _state = State.Wander;
                    _stateTimer = 0f;
                }
                break;

            case State.Wander:
                _animator.SetBool("Aware", false);
                _animator.SetBool("Centry", true);
                _agent.speed = zombieData.walkSpeed;
                _stateTimer += Time.deltaTime;
                if (_stateTimer > zombieData.wanderTime)
                {
                    _state = State.Idle;
                    _stateTimer = 0f;
                }
                Wander();
                break;

            case State.Aware:
                if (!isDead)
                {
                    _animator.SetBool("Aware", true);
                    _animator.SetBool("Centry", true);
                    _agent.SetDestination(player.position);
                    CheckIfHittable();
                    if (!_animator.GetBool("Hit")) _agent.speed = zombieData.runSpeed;
                    if (!_isDetecting)
                    {
                        _stateTimer += Time.deltaTime;
                        if (_stateTimer > hostileTimer)
                        {
                            OnWander();
                        }
                    }
                }
                break;

            case State.Damage:
                _agent.speed = 0f;
                break;
        }
        SearchForPlayer();
        if (_isAware && !isDead && _state != State.Damage) _state = State.Aware;
    }
    #endregion

    #region Update checks
    void SearchForPlayer()
    { 
        if(Physics.Linecast(_transform.position, player.position, out RaycastHit hit))
        {
            if (InFOV() && InDistance() && hit.transform.CompareTag("Player"))
            {
                OnAware();
                _state = State.Aware;
            }
            else
                _isDetecting = false;
        }
    }

    public bool InFOV()
    {
        Vector3 enemyToPlayer = (_transform.position - player.position).normalized;
        Vector3 lookDir = _transform.forward;
        float dot = Vector3.Dot(lookDir, enemyToPlayer);
        if (dot < -zombieData.awareFOV || _animator.GetBool("Hit") == true)
            return true;
        return false;
    }

    public bool InDistance()
    {
        if (Vector3.Distance(player.position, _transform.position) < zombieData.viewDistance)
            return true;
        return false;
    }

    void CheckIfHittable()
    {
        if (Vector3.Distance(player.position, _transform.position) < zombieData.hitDistance)
        {
            punchDone = false;
            _agent.speed = 0.19f;
            _animator.SetBool("Hit", true);
        }
        else if (punchDone)
            _animator.SetBool("Hit", false);
    }
    #endregion

    #region OnState
    public void OnWander()
    {
        _state = State.Wander;
        _stateTimer = 0f;
        _isAware = false;
        _animator.SetBool("Hit", false);
        Wander();
    }

    public void OnAware()
    {
        OnAwareEvent.Invoke();
        _isAware = true;
        _isDetecting = true;
        _stateTimer = 0f;
    }
    void OnDeath()
    {
        isDead = true;
        _animator.SetBool("Dead", true);
        _agent.enabled = false;
        foreach (Collider col in _colliders) col.enabled = false;
        GameManager.deadMobs.Add(gameObject);
        GameManager.AmILastZombie();
    }
    #endregion

    #region Functionality
    public void InProximity()
    {
        if (!playerPVE.isDead && !_isAware) OnAware();
    }

    [ContextMenu("Damage")]
    void DoDamage() => TakeDamage(50);

    public void TakeDamage(float damage)
    {
        _state = State.Damage;
        _animator.SetTrigger("Damaged");
        _health -= damage;
        _animator.SetFloat("Health", _health);

        if (_health <= 0.0001f)
        {
            GameManager.aliveMobs.Remove(gameObject);
            OnDeath();
        }
    }

    //called from AnimationEvents
    public void AfterDamage()
    {
        if (_isAware && _health >= 0.0001f) 
            _state = State.Aware;
        else
            _state = State.Wander;
    }

    void Wander()
    {
        if (zombieData.wanderType == WanderType.Random && !isDead)
        {
            if (Vector3.Distance(_transform.position, _wanderPoint) < 3f)
                 _wanderPoint = RandomWayPoint();
            else
                _agent.SetDestination(_wanderPoint);
        }
        else if (!isDead)
        {
            if (Vector3.Distance(waypoints[waypointIndex].transform.position, _transform.position) < 5f)
            {
                if(waypointIndex == waypoints.Length - 1) 
                    waypointIndex = 0;
                else
                    waypointIndex++;
            }
            else
                _agent.SetDestination(waypoints[waypointIndex].transform.position);
        }
    }
    
    public Vector3 RandomWayPoint()
    {
        Vector3 randomPnt = (Random.insideUnitSphere * zombieData.wanderRadius) + _transform.position;
        //if outside the borders, move to middle
        if (randomPnt.x > 148f || randomPnt.x < 36f || randomPnt.z > 153f || randomPnt.z < 49)
            randomPnt = waypoints[6].transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomPnt, out navHit, zombieData.wanderRadius, -1);

        return new Vector3(navHit.position.x, navHit.position.y, navHit.position.z);
    }
    #endregion
}
