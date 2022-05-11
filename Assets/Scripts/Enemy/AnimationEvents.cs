using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationEvents : MonoBehaviour
{
    #region Config
    [SerializeField] AIScript enemy;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip missSound, hitSound, stepSound, runSound, hurtSound, deathSound;
    [SerializeField] float hitDistance = 1.7f;

    Transform _playerTF;
    PlayerPVE _playerPVE;
    DamageOverlay _damageOverlay;

    void Awake()
    {
        _playerPVE = GameObject.FindGameObjectWithTag("XRRig").GetComponent<PlayerPVE>();
        _playerTF = GameObject.FindGameObjectWithTag("Player").transform;
        _damageOverlay = GameObject.Find("ScreenDamage").GetComponent<DamageOverlay>();
        audioSource = GetComponent<AudioSource>();
    }
    #endregion

    #region Mob actions
    public void Punch()
    {
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.volume = Random.Range(0.4f, 0.5f);
        if (Vector3.Distance(_playerTF.position, transform.position) < hitDistance 
            && Vector3.Dot(enemy.gameObject.transform.forward, (_playerTF.position - enemy.transform.position).normalized) > 0.15f
            && !enemy.isDead)
        {
            _playerPVE.Health -= enemy.zombieData.damage;
            _damageOverlay.EnableOverlay();
            audioSource.PlayOneShot(hitSound);
        }
        else
        {
            audioSource.volume = Random.Range(0.2f, 0.3f);
            audioSource.PlayOneShot(missSound);
        }
    }
    public void PunchDone() => enemy.punchDone = true;

    public void AfterTakingDamage()
    {
        enemy.AfterDamage();
    }
    #endregion

    #region Audio
    public void StepSound()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.volume = Random.Range(0.4f, 0.5f);
        audioSource.PlayOneShot(stepSound);
    }
    public void RunStepSound()
    {
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.volume = Random.Range(0.4f, 0.5f);
        audioSource.PlayOneShot(runSound);
    }
    public void HurtSound()
    {
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.volume = Random.Range(0.4f, 0.5f);
        audioSource.PlayOneShot(hurtSound);
    }
    public void DeathSound()
    {
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.volume = Random.Range(0.4f, 0.5f);
        audioSource.PlayOneShot(deathSound);
    }
    #endregion
}
