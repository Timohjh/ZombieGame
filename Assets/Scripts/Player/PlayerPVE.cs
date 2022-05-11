using BNG;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPVE : MonoBehaviour
{
    public bool isDead = false;
    [SerializeField] float _health = 100f;
    [Range(50, 300)]
    [SerializeField] float MaxHealth = 150f;
    [SerializeField] GameObject restartButton;
    [SerializeField] Transform dummyPlayer;
    public PPrefsController _pPrefs;
    public float Health { 
        get => _health; 
        set => _health = Mathf.Clamp(value, 0, MaxHealth); 
    }

    SmoothLocomotion _player;
    
    void OnValidate() => Health = _health;

    void Awake() => _player = GameObject.FindGameObjectWithTag("Player").GetComponent<SmoothLocomotion>();

    void Update()
    {
        if( _health <= 0f)
            OnDeath();
    }

    public void Restart() => SceneManager.LoadScene(0);

    void OnDeath()
    {
        if (!isDead)
        {
            isDead = true;
            _pPrefs.SaveHighScore();
            restartButton.SetActive(true);
            _player.MovementForce = 0f;

            foreach(GameObject mob in GameManager.aliveMobs)
            {
                var aIScript = mob.GetComponent<AIScript>();
                aIScript.player = dummyPlayer;
                aIScript.OnWander();
            }
        }
    }
}
