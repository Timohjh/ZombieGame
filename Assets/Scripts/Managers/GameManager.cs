using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Config
    public static int round = 0;
    public static List<GameObject> deadMobs;
    public static List<GameObject> aliveMobs;
    public static bool roundInProgress;

    bool _gameStarted = false;
    INextRound _nextRound;

    void Awake() => _nextRound = GetComponent<INextRound>();

    void Start()
    {
        deadMobs = new List<GameObject>();
        aliveMobs = new List<GameObject>();
        round = 0;
        roundInProgress = false;
    }
    #endregion

    void Update()
    {
        if (_gameStarted && aliveMobs.Count == 0 && !roundInProgress)
            _nextRound.BeginNextRound();
    }

    [ContextMenu("start")]
    public void StartGame() => _gameStarted = true;

    public static void AmILastZombie()
    {
        if (aliveMobs.Count == 0) roundInProgress = false;
    }
}
