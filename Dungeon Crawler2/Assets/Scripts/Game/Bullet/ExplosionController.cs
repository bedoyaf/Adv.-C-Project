using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// It handles the purely visual object of the smoke after and explosion, so it just waits to destroy itself
/// </summary>
public class ExplosionController : MonoBehaviour
{
    [SerializeField]
    private float timeTillDestruct = 10f;
    void Start()
    {
        Invoke("DestroySelf", timeTillDestruct);
    }

    public void SetExplosionDuration(float time)
    {
        timeTillDestruct = time;
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
