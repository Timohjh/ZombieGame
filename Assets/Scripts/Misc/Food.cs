using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] float healthIncrease = 30f;
    [Range(0f,0.5f)]
    [SerializeField] float fadeSpeed = 0.1f;
    [SerializeField] Material material;
    bool _doFade = false;
    bool _hasBeenEaten = false;
    Renderer _renderer;
    PlayerPVE _playerPVE;
    Color color = default;

    void Awake()
    {
        _playerPVE = GameObject.FindGameObjectWithTag("XRRig").GetComponent<PlayerPVE>();
        _renderer = GetComponent<Renderer>();
    }

    void Start() => color = _renderer.material.color;

    void Update()
    {
        if (_doFade && color.a >= 0f)
        {
            color.a -= fadeSpeed;
            _renderer.material.color = color;
        }
        else if (color.a <= 0f)
            Destroy(gameObject);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHead") && !_hasBeenEaten)
        {
            _doFade = true;
            _playerPVE.Health += healthIncrease;
            _hasBeenEaten = true;
        }
    }
}
