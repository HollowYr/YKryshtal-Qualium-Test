using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolesTrigger : MonoBehaviour
{
    [SerializeField] private Data data;
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IPoolObject>(out IPoolObject ball))
        {
            ball.Disable();
            data.InvokeOnBallDestroyed(ball.GetTransformHashCode());
        }
    }
}
