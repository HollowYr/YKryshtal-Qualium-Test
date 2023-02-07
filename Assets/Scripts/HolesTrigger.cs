using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolesTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IPoolObject>(out IPoolObject ball))
        {
            ball.Disable();
        }
    }
}
