using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance { get; private set; }

    [Header("Target Tracking")]
    [SerializeField] private Transform target;

    [Header("Position Config")]
    [SerializeField] private Vector3 startOffset = new Vector3(0, 1.75f, -3.25f);
    [SerializeField] private Vector3 runOffset = new Vector3(0, 3f, -3f);
    [SerializeField] private float positionSmoothTime = 0.2f;

    [Header("Rotation Config")]
    [SerializeField] private float rotationSmoothSpeed = 5.0f;

    private Vector3 currentOffset;
    private Vector3 currentVelocity;

    public Vector3 CurrentOffset { get => currentOffset; set => currentOffset = value; }

    private void Start()
    {
        Instance = this;
        ChangeOffset();

        if (target != null)
        {
            transform.position = target.position + currentOffset;
            transform.rotation = Quaternion.LookRotation(target.position - transform.position + Vector3.up * 1.5f);
        }
    }

    private void LateUpdate()
    {
        if (target == null && !SplinePlayerMovement.Instance.IsRunning) return;

        Vector3 targetOffset = target.transform.TransformDirection(currentOffset);
        Vector3 targetPosition = target.position + targetOffset;

        Vector3 localTargetPos = target.transform.InverseTransformPoint(targetPosition);
        localTargetPos.x = 0;
        targetPosition = target.transform.TransformPoint(localTargetPos);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, positionSmoothTime);

        Vector3 lookAtTarget = target.position + target.transform.forward * 2.0f + Vector3.up * 1.2f;
        Quaternion targetRotation = Quaternion.LookRotation(lookAtTarget - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothSpeed * Time.deltaTime);


    }

    public void ChangeOffset(bool isRun = false) => CurrentOffset = isRun ? runOffset : startOffset;
}
