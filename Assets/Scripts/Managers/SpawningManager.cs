using UnityEngine;

public class SpawningManager : MonoBehaviour, ISpawnManager
{
    [SerializeField] Transform[] spawns;
    [SerializeField] GameObject[] mobPrefabs;
    [SerializeField] int zombiesToSpawn = 1;
    [SerializeField] int bossesToSpawn = 0;
    [SerializeField] int bossRound = 5;
    [SerializeField] float nextRoundTimer = 5f;

    Transform _player;
    int _zombiesSpawned = 0;
    int _bossesSpawned = 0;

    void Awake() => _player = GameObject.FindGameObjectWithTag("Player").transform;

    public void StartSpawning()
    {
        zombiesToSpawn += 2;
        _zombiesSpawned = 0;
        _bossesSpawned = 0;

        InvokeRepeating("SpawnZombies", nextRoundTimer, 5.5f);

        if (GameManager.round % bossRound == 0)
        {
            bossesToSpawn++;
            InvokeRepeating("SpawnBosses", nextRoundTimer, 5f);
        }
    }

    public void SpawnZombies()
    {
        var randomSpawn = Random.Range(0, spawns.Length);
        var randomZombie = Random.Range(0, 2);
        if (Vector3.Distance(spawns[randomSpawn].position, _player.position) < 15f) randomSpawn = 14;
        Instantiate(mobPrefabs[randomZombie], spawns[randomSpawn].position, spawns[randomSpawn].rotation);
        _zombiesSpawned++;

        if (zombiesToSpawn == _zombiesSpawned) CancelInvoke("SpawnZombies");
    }

    public void SpawnBosses()
    {
        int randomSpawn = Random.Range(0, spawns.Length);
        if (Vector3.Distance(spawns[randomSpawn].position, _player.position) < 15f) randomSpawn = 0;
        Instantiate(mobPrefabs[2], spawns[randomSpawn].position, spawns[randomSpawn].rotation);
        _bossesSpawned++;

        if (bossesToSpawn == _bossesSpawned) CancelInvoke("SpawnBosses");
    }
}
