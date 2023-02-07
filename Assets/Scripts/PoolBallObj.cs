using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBallObj : MonoBehaviour, IPoolObject
{
    [SerializeField] private Data data;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 startScale;
    private Rigidbody rigidbody;
    private float ballStopSpeedThreshold;


    public void Disable()
    {
        gameObject.SetActive(false);
    }
    public void Enable()
    {
        Reset();
        gameObject.SetActive(true);
    }
    public void Init()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        startScale = transform.localScale;
        rigidbody = GetComponent<Rigidbody>();
        ballStopSpeedThreshold = data.ballStopSpeedThreshold;
    }

    public int GetTransformHashCode() => transform.GetHashCode();

    public void Reset()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        transform.localScale = startScale;
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (rigidbody.velocity.magnitude < ballStopSpeedThreshold)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
    }
}
