using UnityEngine;

[CreateAssetMenu(menuName = "SO_Zombie")]
public class ZombieData : ScriptableObject
{
    [Header("Enemy")]
    public float health = 100f;
    public float damage = 10f;

    [Header("Awareness")]
    public float walkSpeed = 1.15f;
    public float runSpeed = 2.5f;
    public float awareFOV = 0.87f;
    public float viewDistance = 10f;
    public float hitDistance = 2f;
    public float hostileTimer = 10f;

    [Header("Wander")]
    public WanderType wanderType = WanderType.Waypoint;
    public float wanderRadius = 10f;
    public float idleTime = 5f;
    public float wanderTime = 10f;
}
public enum WanderType
{
    Random,
    Waypoint
}
