using UnityEngine;
using System;

public class Target : MonoBehaviour
{
    public Action<IHittable> OnTargetHit;

    private void OnTriggerEnter(Collider other)
    {
        var hittable = other.gameObject.GetComponent<IHittable>();
        if (hittable != null)
            OnTargetHit.Invoke(hittable);
    }
}
