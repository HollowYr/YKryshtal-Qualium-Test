using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MainBallMovement : ImprovedMonoBehaviour
{
    [SerializeField] private float rayMaxDistance = 10f;
    [SerializeField] private Data data;
    [SerializeField] private Transform ballPredictionCircle;
    [SerializeField] private LineRenderer launchDirectionLineRenderer;
    [SerializeField] private LineRenderer mainballReflectLineRenderer;
    [SerializeField] private LineRenderer secondaryBallReflectLineRenderer;
    private Vector2 forceMinMax;
    // approximately half of the board length
    private Vector2 defaultForceMinMax = new Vector2(0, 10);
    private Rigidbody rigidbody;
    private Camera camera;
    internal Vector3 launchVelocity;
    private Vector3 ballPosition;
    private Vector3 startTouchPos;
    private Vector3 launchDirection;
    private float distanceBetweenTouches;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        launchDirectionLineRenderer = GetComponent<LineRenderer>();
        camera = Camera.main;
        forceMinMax = data.forceMinMax;
        defaultForceMinMax.y = data.tableSize.x / 2;
        EnableTrajectoryPredictionVisuals(false);
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
            Release(launchVelocity);
        }
    }
    // TODO replace raycasts with raycastCommand for performance
    private void Click()
    {
        EnableTrajectoryPredictionVisuals(true);

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, rayMaxDistance))
        {
            ballPosition = transform.position;
            startTouchPos = hit.point;
        }
    }

    private void Hold()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, rayMaxDistance))
        {
            launchVelocity = GetLaunchVelocity(hit.point);
            data.InvokeOnPowerChanged(distanceBetweenTouches);

            ray = new Ray(ballPosition, launchDirection);
            if (Physics.SphereCast(ray, data.ballRadius, out hit, rayMaxDistance))
            {
                Vector3 hitPoint = hit.point + data.ballRadius * hit.normal;
                hitPoint.y = ballPosition.y * 2;

                ballPredictionCircle.position = hitPoint;

                SetSecondaryReflectionLine(hitPoint, hit);

                SetMainReflectionLine(hitPoint, hit);

                launchDirectionLineRenderer.SetPosition(0, ballPosition);
                launchDirectionLineRenderer.SetPosition(1, hitPoint);
            }
        }

        Vector3 GetLaunchVelocity(Vector3 hitPoint)
        {
            Vector3 currentPosition = hitPoint;
            launchDirection = ballPosition - currentPosition;
            launchDirection = launchDirection.normalized;
            distanceBetweenTouches = Mathf.Abs((currentPosition - startTouchPos).magnitude);
            distanceBetweenTouches = distanceBetweenTouches.Remap(defaultForceMinMax.x, defaultForceMinMax.y,
                                                                forceMinMax.x, forceMinMax.y);

            launchDirection.y = 0;
            return launchDirection * distanceBetweenTouches;
        }
        void SetSecondaryReflectionLine(Vector3 hitPoint, RaycastHit hit)
        {
            Vector3 secondaryBallDirection = hitPoint - hit.normal;
            secondaryBallReflectLineRenderer.SetPosition(0, hitPoint);
            secondaryBallReflectLineRenderer.SetPosition(1, secondaryBallDirection);
        }
        void SetMainReflectionLine(Vector3 hitPoint, RaycastHit hit)
        {
            hit.transform.TryGetComponent<IPoolObject>(out IPoolObject hittedBall);

            mainballReflectLineRenderer.SetPosition(0, hitPoint);

            Vector3 mainReflection = Vector3.zero;
            if (hittedBall == null)
            {
                Vector3 reflectedDirection = Vector3.Reflect(launchDirection.normalized, hit.normal);
                mainReflection = reflectedDirection;
                secondaryBallReflectLineRenderer.enabled = false;
            }
            else
            {
                secondaryBallReflectLineRenderer.enabled = true;
                int normalRotation = (Vector3.Dot(hit.normal, Vector3.forward) > 0) ? 1 : -1;
                Vector3 mainReflectionRotated = Quaternion.AngleAxis(90 * normalRotation, Vector3.up) * hit.normal;
                mainReflection = mainReflectionRotated;
            }
            mainballReflectLineRenderer.SetPosition(1, hitPoint + mainReflection);
        }
    }


    internal void Release(Vector3 velocity)
    {
        rigidbody.AddForce(velocity, ForceMode.VelocityChange);
        data.InvokeOnPowerChanged(0);
        EnableTrajectoryPredictionVisuals(false);
    }

    private void EnableTrajectoryPredictionVisuals(bool isEnabled)
    {
        launchDirectionLineRenderer.enabled = isEnabled;
        mainballReflectLineRenderer.enabled = isEnabled;
        secondaryBallReflectLineRenderer.enabled = isEnabled;
        ballPredictionCircle.gameObject.SetActive(isEnabled);
    }
}
