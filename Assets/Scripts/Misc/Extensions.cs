using UnityEngine;

public class Extensions : MonoBehaviour
{
    public static bool InFOV(Transform enemy, Transform player, float fov)
    {
        if (Vector3.Angle(enemy.forward, enemy.InverseTransformPoint(player.position)) < fov / 2f)
            return true;
        else
            return false;
    }
    public static bool InDistance(Transform enemy, Transform player, float viewDistance)
    {
        if (Vector3.Distance(player.position, enemy.position) < viewDistance)
            return true;
        else
            return false;
    }
}

