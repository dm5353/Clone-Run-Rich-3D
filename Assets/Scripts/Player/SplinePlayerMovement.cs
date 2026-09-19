using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Splines;

public class SplinePlayerMovement : MonoBehaviour
{
    public static SplinePlayerMovement Instance { get; private set; }

    [Header("Spline Configuration")]
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private float forwardSpeed = 6.0f;

    [Header("Sideway Input Config")]
    [SerializeField] private float swipeSensitivity = 1.5f;
    [SerializeField] private float maxHorizontalDistance = 2.5f;
    [SerializeField] private float sidewaySmoothSpeed = 15.0f;

    private float currentSplineDistance = 0f;
    private float targetHorizontalOffset = 0f;
    private float currentHorizontalOffset = 0f;

    private Vector2 lastTouchPosition;
    private bool isRunning = false;
    private float splineLength = 0f;

    public bool IsRunning { get => isRunning; set => isRunning = value;  }

    private void Start()
    {
        Instance = this;

        if (splineContainer != null)
        {
            splineLength = splineContainer.CalculateLength();
        }
    }

    public void StartRunning()
    {
        IsRunning = true;
    }

    public void StopRunning()
    {
        IsRunning = false;
        GameManager.Instance.LevelOver();
    }

    private void Update()
    {
        if (!isRunning || splineContainer == null) return;

        HandleSidewayInput();
        ProcessSplineMovement();
    }

    private void HandleSidewayInput()
    {
        if (Pointer.current == null) return;

        bool isPressing = Pointer.current.press.isPressed;

        if (Pointer.current.press.wasPressedThisFrame)
        {
            lastTouchPosition = Pointer.current.position.ReadValue();
        }
        else if (isPressing)
        {
            Vector2 currentTouchPos = Pointer.current.position.ReadValue();
            float deltaX = currentTouchPos.x - lastTouchPosition.x;
            lastTouchPosition = currentTouchPos;

            targetHorizontalOffset += (deltaX / Screen.width) * swipeSensitivity * maxHorizontalDistance * 10f;
            targetHorizontalOffset = Mathf.Clamp(targetHorizontalOffset, -maxHorizontalDistance, maxHorizontalDistance);
        }
    }

    private void ProcessSplineMovement()
    {
        currentSplineDistance += forwardSpeed * Time.deltaTime;

        if (currentSplineDistance >= splineLength)
        {
            currentSplineDistance = splineLength;
            StopRunning();
        }

        float t = currentSplineDistance / splineLength;

        splineContainer.Evaluate(t, out var splinePos, out var splineForward, out var splineUp);

        Vector3 forwardDir = ((Vector3)splineForward).normalized;
        Vector3 splineRight = Vector3.Cross(Vector3.up, forwardDir).normalized;

        currentHorizontalOffset = Mathf.Lerp(currentHorizontalOffset, targetHorizontalOffset, Time.deltaTime * sidewaySmoothSpeed);

        Vector3 finalWorldPosition = (Vector3)splinePos + (splineRight * currentHorizontalOffset);
        finalWorldPosition.y = 0.5f;
        transform.position = finalWorldPosition;

        if (forwardDir != Vector3.zero)
        {
            Quaternion splineRotation = Quaternion.LookRotation(forwardDir, Vector3.up);
            Vector3 eulerAngles = splineRotation.eulerAngles;

            float swipeVelocityX = 0f;
            if (Pointer.current != null && Pointer.current.press.isPressed)
            {
                swipeVelocityX = Pointer.current.delta.x.ReadValue();
            }

            float turnSensitivity = 20f;
            float maxTurnAngle = 20f;

            float targetBonusYAngle = Mathf.Clamp(swipeVelocityX * turnSensitivity, -maxTurnAngle, maxTurnAngle);

            eulerAngles.x = 0f;
            eulerAngles.z = 0f;
            eulerAngles.y += targetBonusYAngle;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(eulerAngles), Time.deltaTime * 15f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + transform.right * maxHorizontalDistance, 0.3f);
        Gizmos.DrawWireSphere(transform.position - transform.right * maxHorizontalDistance, 0.3f);
    }
}