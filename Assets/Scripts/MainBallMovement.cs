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
    [SerializeField] private LineRenderer mainLineRenderer;
    [SerializeField] private LineRenderer mainballReflectLineRenderer;
    [SerializeField] private LineRenderer secondaryBallReflectLineRenderer;
    private Camera camera;
    internal Vector3 velocity;
    private Vector3 ballPosition;
    private Vector3 startTouchPos;
    private Vector3 direction;
    private float distance;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        mainLineRenderer = GetComponent<LineRenderer>();
        camera = Camera.main;
        forceMinMax = data.forceMinMax;

        EnableLineRenderers(false);
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
            Release(velocity);
        }
    }
    // TODO replace raycasts with raycastCommand for performance
    private void Click()
    {
        EnableLineRenderers(true);

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

    // TODO replace with trajectory prediction 
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

            distance = distance.Remap(defaultForceMinMax.x, defaultForceMinMax.y,
                               forceMinMax.x, forceMinMax.y);

            direction.y = 0;
            velocity = distance * direction;
            ray = new Ray(ballPosition, direction);
            // secondaryBallReflectLineRenderer.enabled = false;
            if (Physics.SphereCast(ray, data.ballRadius, out hit, rayMaxDistance))
            {

                // secondaryBallReflectLineRenderer.enabled = true;

                Vector3 hitPoint = hit.point + data.ballRadius * hit.normal;
                hitPoint.y = ballPosition.y * 2;
                Vector3 secondaryBallDirection = hitPoint - hit.normal;
                secondaryBallReflectLineRenderer.SetPosition(0, hitPoint);
                secondaryBallReflectLineRenderer.SetPosition(1, secondaryBallDirection);


                mainballReflectLineRenderer.SetPosition(0, hitPoint);
                // Vector3 reflectedDirection = Vector3.Reflect(ray.direction.normalized, hit.normal);

                int normalRotation = (Vector3.Dot(hit.normal, Vector3.forward) > 0) ? 1 : -1;
                Vector3 mainReflectionRotated = Quaternion.AngleAxis(90 * normalRotation, Vector3.up) * hit.normal;
                mainballReflectLineRenderer.SetPosition(1, hitPoint + mainReflectionRotated);

                mainLineRenderer.SetPosition(0, ballPosition);
                mainLineRenderer.SetPosition(1, hitPoint);
            }
        }
    }

    internal void Release(Vector3 velocity)
    {
        rigidbody.AddForce(velocity, ForceMode.VelocityChange);

        EnableLineRenderers(false);
    }

    private void EnableLineRenderers(bool isEnabled)
    {
        mainLineRenderer.enabled = isEnabled;
        mainballReflectLineRenderer.enabled = isEnabled;
        secondaryBallReflectLineRenderer.enabled = isEnabled;
    }

}
