using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MainBallMovement : ImprovedMonoBehaviour
{
    [SerializeField] private float rayMaxDistance = 10f;
    private Rigidbody rigidbody;
    private LineRenderer lineRenderer;
    private Camera camera;
    private Vector3 ballPosition;
    private Vector3 startTouchPos;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        camera = Camera.main;
    }
    // TODO transfer input to events
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Click();
        }
        if (Input.GetMouseButton(0))
        {
            Hold();
        }
        if (Input.GetMouseButtonUp(0))
        {
            Release();
        }
    }
    // TODO replace raycasts with raycastCommand for performance
    private void Click()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, rayMaxDistance))
        {
            ballPosition = transform.position;
            // ballPosition.y = 0;

            startTouchPos = hit.point;
            // startTouchPos.y = 0;
        }
        Debug.DrawRay(ray.origin, ray.direction * 15f, Color.red, 100f);
    }

    private void Hold()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, rayMaxDistance))
        {
            Vector3 currentPosition = hit.point;
            // currentPosition.y = 0;
            Vector3 direction = ballPosition - currentPosition;
            direction = direction.normalized;
            // direction.y = 0f;
            float distance = Mathf.Abs((currentPosition - startTouchPos).magnitude);

            Debug.Log(distance);

            Debug.DrawRay(currentPosition, direction, Color.red, Time.deltaTime);
            Debug.DrawRay(ballPosition, direction * distance, Color.green, Time.deltaTime);

            direction.y = 0;
            ray = new Ray(ballPosition, direction);
            if (Physics.Raycast(ray, out hit, rayMaxDistance))
            {
                lineRenderer.SetPosition(0, ballPosition);
                Vector3 hitPoint = hit.point;
                hitPoint.y = ballPosition.y;
                lineRenderer.SetPosition(1, hitPoint);
            }
        }
    }

    private void Release()
    {
    }

    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawSphere(startPosition, 1f);
    // }
}
