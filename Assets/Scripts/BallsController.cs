using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallsController : MonoBehaviour
{
    [SerializeField] private Transform[] balls;
    [SerializeField] private Data data;
    void OnValidate()
    {
        balls = GetComponentsInChildren<Transform>().Where(t => t.transform != this.transform).ToArray();
        int ballsCount = balls.Count();
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