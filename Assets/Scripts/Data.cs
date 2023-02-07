using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Data", order = 1)]
public class Data : ScriptableObject
{
    [Foldout("Balls properties"), SerializeField]
    private Transform ballExample;
    [Foldout("Balls properties"), SerializeField]
    private Transform mainBall;
    [Foldout("Balls properties"), SerializeField]
    internal float ballRadius;

    private const float DEFAULT_SPHERE_RADIUS = .5f;
    void OnValidate()
    {
        mainBall.transform.localScale = Vector3.one * ballRadius / DEFAULT_SPHERE_RADIUS;
        ballExample.transform.localScale = Vector3.one * ballRadius / DEFAULT_SPHERE_RADIUS;
    }
}
