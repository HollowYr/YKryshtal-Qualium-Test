using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MainBallMovement : ImprovedMonoBehaviour
{
    [SerializeField] private float rayMaxDistance = 10f;
    [SerializeField] private Data data;
    private Vector2 forceMinMax;
    // approximately half of the board length
    private Vector2 defaultForceMinMax = new Vector2(0, 10);
    private Rigidbody rigidbody;
    private LineRenderer lineRenderer;
    private Camera camera;
    private Vector3 ballPosition;
    private Vector3 startTouchPos;
    private Vector3 direction;
    private float distance;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        camera = Camera.main;
        forceMinMax = data.forceMinMax;
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
        lineRenderer.enabled = true;

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
            direction = ballPosition - currentPosition;
            direction = direction.normalized;
            // direction.y = 0f;
            distance = Mathf.Abs((currentPosition - startTouchPos).magnitude);

            Debug.Log(distance.ToString().Color("green"));
            distance = distance.Remap(defaultForceMinMax.x, defaultForceMinMax.y,
                               forceMinMax.x, forceMinMax.y);
            Debug.Log(distance.ToString().Color("red"));
            Debug.DrawRay(currentPosition, direction, Color.red, Time.deltaTime);

            direction.y = 0;
            ray = new Ray(ballPosition, direction);
            if (Physics.Raycast(ray, out hit, rayMaxDistance))
            {

                lineRenderer.SetPosition(0, ballPosition);
                Vector3 hitPoint = hit.point;
                hitPoint.y = ballPosition.y * 2;
                lineRenderer.SetPosition(1, hitPoint);
            }
        }
    }

    private void Release()
    {
        rigidbody.AddForce(direction * distance, ForceMode.VelocityChange);

        lineRenderer.enabled = false;
    }

    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawSphere(startPosition, 1f);
    // }
}
