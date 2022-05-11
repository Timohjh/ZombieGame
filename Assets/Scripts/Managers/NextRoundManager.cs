using UnityEngine;

public class NextRoundManager : MonoBehaviour, INextRound
{
    [SerializeField] UIActions uIA;
    [SerializeField] AudioClip roundSound;

    [SerializeField] GameObject[] foods;
    [SerializeField] GameObject sword;
    [SerializeField] Transform foodSpawn;

    AudioSource _audioSource;
    ISpawnManager _spawningManager;

    void Awake()
    {
        _spawningManager = GetComponent<ISpawnManager>();
        _audioSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
    }
    public void BeginNextRound()
    {
        SetupNextRound();
        uIA.SetRound();
        _spawningManager.StartSpawning();

        _audioSource.PlayOneShot(roundSound);
    }

    void SetupNextRound()
    {
        GameManager.round++;
        GameManager.roundInProgress = true;

        if (GameManager.round == 5) sword.SetActive(true);

        foreach (GameObject mob in GameManager.deadMobs) Destroy(mob);

        if (GameManager.round % 3 == 0)
        {
            var randomFood = Random.Range(0, 3);
            Instantiate(foods[randomFood], foodSpawn);
        }
    }
}
