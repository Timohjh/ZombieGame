using BNG;
using UnityEngine;

public class AwarenessProximity : MonoBehaviour
{
    [SerializeField] SmoothLocomotion playerController;
    [SerializeField] SphereCollider sphereCollider;
    [SerializeField] float walkHearRadius = 1f;
    [SerializeField] float runHearRadius = 1.5f;

    void Update()
    {
        if (playerController.CheckSprint())
            sphereCollider.radius = runHearRadius;
        else
            sphereCollider.radius = walkHearRadius;
    }

    void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag != "RemoteGrabber" && other.gameObject.CompareTag("Zombie"))
            other.GetComponentInParent<AIScript>().InProximity();
    }
}
