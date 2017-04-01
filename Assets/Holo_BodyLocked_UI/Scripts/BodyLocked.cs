using UnityEngine;

namespace Hecomi.HoloLensPlayground
{

public class BodyLocked : MonoBehaviour 
{
    #region(Parameters)
    [Header("Basics")]
    [SerializeField]
    float maxDistance = 2f;

    [SerializeField]
    float minDistance = 0.5f;

    [SerializeField]
    float minScaleRatio = 0.5f;

    [SerializeField]
    float moveThresholdAngle = 15f;

    [Header("Smoothness")]
    [SerializeField]
    [Range(0f, 1f)]
    float directionSmoothness = 0.94f;

    [SerializeField]
    [Range(0f, 1f)]
    float distanceSmoothness = 0.8f;

    [SerializeField]
    [Range(0f, 1f)]
    float rotationSmoothness = 0.96f;

    [Header("Collision")]
    [SerializeField]
    bool checkCollision = true;

    [SerializeField]
    float floorCeilingAngle = 80f;

    [SerializeField]
    float maxPasteAngle = 40f;

    [SerializeField]
    float offsetFromCollision = 0.1f;

    [SerializeField]
    LayerMask collisionLayerMask = 1 << 31;

    [SerializeField]
    float rayRadius = 0.1f;

    [SerializeField]
    int rayNum = 3;

    [SerializeField]
    float rayNoise = 0.03f;
    #endregion

    #region(Private members)
    bool isMoving_ = false;
    Vector3 initScale_ = Vector3.one;
    Vector3 direction_ = Vector3.forward;
    float distance_ = 2f;
    Quaternion targetRotation_ = Quaternion.identity;
    float targetDistance_ = 2f;
    #endregion

    void Start()
    {
        var camera = Camera.main.transform;

        initScale_ = transform.localScale;
        direction_ = camera.forward;
        distance_ = targetDistance_ = maxDistance;
        targetRotation_ = Quaternion.LookRotation(camera.forward, Vector3.up);

        transform.position = camera.position + (direction_ * distance_);
        transform.rotation = targetRotation_;
    }

    void UpdateDirection()
    {
        var camera = Camera.main.transform;
        var angle = Vector3.Angle(direction_, camera.forward);

        if (!isMoving_) {
            if (Mathf.Abs(angle) > moveThresholdAngle) {
                isMoving_ = true;
            }
        } else {
            if (angle < 1f) {
                isMoving_ = false;
            }
            var cameraForwardRot = Quaternion.LookRotation(camera.forward, Vector3.up);
            var directionRot =  Quaternion.LookRotation(direction_, Vector3.up);
            direction_ = Quaternion.Slerp(directionRot, cameraForwardRot, 1f - directionSmoothness) * Vector3.forward;
        }

        targetDistance_ = maxDistance;
        targetRotation_ = Quaternion.LookRotation(camera.forward, Vector3.up);
    }

    void UpdateCollision()
    {
        if (!checkCollision) return;

        var camera = Camera.main.transform;
        float averageDistance = 0f;
        var averageNormal = Vector3.zero;
        int hitNum = 0;

        for (int i = 0; i < rayNum; ++i) {
            RaycastHit hit;
            var noiseRadius = Random.Range(0f, rayNoise) * rayRadius;
            if (Physics.SphereCast(camera.position, rayRadius + noiseRadius, direction_, out hit, maxDistance, collisionLayerMask)) {
                averageDistance += hit.distance + (rayRadius + noiseRadius) - offsetFromCollision;
                averageNormal += hit.normal;
                ++hitNum;
            }
        }

        if (hitNum > 0) {
            averageDistance /= hitNum;
            averageNormal /= hitNum;

            var axis = Vector3.up;
            if (Mathf.Abs(Vector3.Dot(averageNormal, axis)) > Mathf.Cos(floorCeilingAngle * Mathf.Deg2Rad)) {
                axis = camera.up;
            }

            var forward = camera.forward;
            if (Vector3.Angle(direction_, -averageNormal) < maxPasteAngle) {
                forward = -averageNormal;
            }

            targetDistance_ = averageDistance; 
            targetRotation_ = Quaternion.LookRotation(forward, axis);
        }
    }

    void LateUpdate()
    {
        UpdateDirection();
        UpdateCollision();

        distance_ = Mathf.Max(distance_ + (targetDistance_ - distance_) * distanceSmoothness, minDistance);
        var scaleFactor = (distance_ - minDistance) / (maxDistance - minDistance);

        transform.position = Camera.main.transform.position + (direction_ * distance_);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation_, 1f - rotationSmoothness);
        transform.localScale = initScale_ * (1f - minScaleRatio * (1f - Mathf.Clamp(scaleFactor, 0f, 1f)));
    }
}

}