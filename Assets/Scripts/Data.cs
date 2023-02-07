using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Data", order = 1)]
public class Data : ScriptableObject
{
    [Foldout("Balls properties"), SerializeField]
    private Transform ballExample;
    [Foldout("Balls properties"), SerializeField]
    internal MainBallMovement mainBall;
    [Foldout("Balls properties"), SerializeField]
    internal float ballRadius;
    [Foldout("Balls properties"), SerializeField]
    internal float ballStopSpeedThreshold = .1f;
    [Foldout("Balls properties"), SerializeField]
    internal Vector2 forceMinMax;

    public event Action<float> OnPowerChanged;
    public event Action<int> OnBallDestroyed;

    public void InvokeOnPowerChanged(float power)
    {
        OnPowerChanged?.Invoke(power);
    }

    public void InvokeOnBallDestroyed(int hashCode)
    {
        OnBallDestroyed?.Invoke(hashCode);
    }

    private const float DEFAULT_SPHERE_RADIUS = .5f;
    void OnValidate()
    {
        mainBall.transform.localScale = Vector3.one * ballRadius / DEFAULT_SPHERE_RADIUS;
        ballExample.transform.localScale = Vector3.one * ballRadius / DEFAULT_SPHERE_RADIUS;
    }
}
