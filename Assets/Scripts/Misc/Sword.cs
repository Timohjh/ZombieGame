using BNG;
using System.Collections;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float damage = 15f;
    [SerializeField] float headMultiplier = 1.15f;
    [SerializeField] float timeBetweenHits = 1f;
    [SerializeField] float swingTreshold = 1f;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hitSound;
    bool _readyToHit = true;
    bool _enoughVelocity = true;

    Rigidbody _rb;
    Transform _player;

    Renderer _renderer;
    MaterialPropertyBlock _matProp;

    void Awake()
    {
        _rb = GetComponentInParent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _renderer = GetComponentInParent<Renderer>();
        _matProp = new MaterialPropertyBlock();
        _renderer.material.EnableKeyword("_EMISSION");
    }

    void Update() => EmissionChange();

    void FixedUpdate()
    {
        if (_rb.velocity.magnitude > swingTreshold) _enoughVelocity = true;
        else _enoughVelocity = false;
    }

    public void OnVikingSwordPickup(GameObject go)
    {
        if(go.GetComponent<Grabber>().HeldGrabbable.name == "VikingSword")
            gameObject.transform.parent.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (_readyToHit && _enoughVelocity)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.volume = Random.Range(0.4f, 0.5f);

            if (other.gameObject.CompareTag("Zombie"))
                HandleDamage(damage, other);
            else if (other.gameObject.CompareTag("ZombieHead"))
                HandleDamage(damage * headMultiplier, other);
        }
    }

    void HandleDamage(float damage, Collider other)
    {
        _readyToHit = false;
        StartCoroutine(HitTimer());
        other.gameObject.GetComponentInParent<AIScript>().TakeDamage(damage);
        audioSource.PlayOneShot(hitSound);
    }

    IEnumerator HitTimer()
    {
        yield return new WaitForSeconds(timeBetweenHits);
        _readyToHit = true;
    }

    void EmissionChange()
    {
        _renderer.GetPropertyBlock(_matProp);
        //get emission color based on distance to object
        var distance = Vector3.Distance(_player.transform.position, gameObject.transform.position);
        var minDist = 5f;
        var maxDist = 60f;
        if (distance > minDist)
        {
            var lerp = Mathf.InverseLerp(minDist, maxDist, distance) * 10;
            var color = new Color(lerp, lerp, lerp);
            _matProp.SetColor("_EmissionColor", color);
        }
        else
        {
            var color = new Color(0f, 0f, 0f);
            _matProp.SetColor("_EmissionColor", color);
        }
        _renderer.SetPropertyBlock(_matProp);
    }

}
