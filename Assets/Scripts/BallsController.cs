using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallsController : MonoBehaviour
{
    [SerializeField] private Data data;
    [SerializeField] private Transform mainBall;
    [SerializeField] private PoolSystem poolSystem;
    [SerializeField] private Transform[] balls;
    private int mainBallHashCode;
    private int ballsCount;
    void Start()
    {
        mainBallHashCode = mainBall.GetHashCode();
        data.OnBallDestroyed += OnBallDestroyed;
        Application.quitting += Unsubscribe;
        data.OnBallsReset += ResetBallCount;
    }

    private void ResetBallCount()
    {
        ballsCount = balls.Count();
    }

    private void Unsubscribe()
    {
        data.OnBallDestroyed -= OnBallDestroyed;
        data.OnBallsReset -= ResetBallCount;
    }

    private void OnBallDestroyed(int hashCode)
    {
        ballsCount--;
        if (mainBallHashCode == hashCode || ballsCount <= 0)
        {
            poolSystem.ResetAllObjects();
            return;
        }
    }

    void OnValidate()
    {
        balls = GetComponentsInChildren<Transform>().Where(t => t.transform != this.transform).ToArray();
        ResetBallCount();
        float xOffset = 0;
        float ballRadius = data.ballRadius;
        int count = 0;
        for (int row = 1; row <= 5; row++)
        {
            for (int i = 0; i < row; i++, count++)
            {
                float rowLength = 2 * ballRadius * (row - 1);
                float zOffset = (i * 2 * ballRadius) - (rowLength / 2);

                balls[count].localPosition = new Vector3(xOffset, ballRadius, zOffset);
            }
            xOffset += 2 * ballRadius;
        }
    }
}