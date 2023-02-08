using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Data", order = 1)]
public class Data : ScriptableObject
{
    private const float DEFAULT_SPHERE_RADIUS = .5f;

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
    [Foldout("Balls properties"), SerializeField]
    internal Vector2 tableSize;
    
    public event Action<float> OnPowerChanged;
    public event Action<int> OnBallDestroyed;
    public event Action OnBallsReset;

    public void InvokeOnPowerChanged(float power) => OnPowerChanged?.Invoke(power);
    public void InvokeOnBallDestroyed(int hashCode) => OnBallDestroyed?.Invoke(hashCode);
    public void InvokeOnBallsReset() => OnBallsReset?.Invoke();

    void OnValidate()
    {
        mainBall.transform.localScale = Vector3.one * ballRadius / DEFAULT_SPHERE_RADIUS;
        ballExample.transform.localScale = Vector3.one * ballRadius / DEFAULT_SPHERE_RADIUS;
    }
}
