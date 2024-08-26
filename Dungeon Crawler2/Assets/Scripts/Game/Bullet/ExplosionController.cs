using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
