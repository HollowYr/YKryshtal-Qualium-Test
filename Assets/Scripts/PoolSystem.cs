using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolSystem : MonoBehaviour
{
    [SerializeField] private IPoolObject[] balls;
    [SerializeField] private Transform ballsParent;
    int ballsCount;
    void Start()
    {
        balls = ballsParent.GetComponentsInChildren<IPoolObject>();
        ballsCount = balls.Count();
    }

    public void ResetAllObjects()
    {
        for (int i = 0; i < ballsCount; i++)
        {
            balls[i].Enable();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ResetAllObjects();
        }
    }
}
