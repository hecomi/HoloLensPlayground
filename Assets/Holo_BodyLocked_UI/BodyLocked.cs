using UnityEngine;

namespace Hecomi.HoloLensPlayground
{

public class BodyLocked : MonoBehaviour 
{
    #region(Parameters)
    [Header("Basics")]
    [SerializeField]
    [Range(0f, 5f)]
    float maxDistance = 2f;

    [Range(0f, 5f)]
    [SerializeField]
    float minDistance = 0.5f;

    [SerializeField]
    float moveThresholdAngle = 10f;

    [Header("Smoothness")]
    [SerializeField]
    [Range(0f, 1f)]
    float moveSmoothness = 0.94f;

    [SerializeField]
    [Range(0f, 1f)]
    float rotateSmoothness = 0.96f;

    [Header("Collision")]
    [SerializeField]
    bool checkCollision = true;

    [SerializeField]
    float radius = 0.1f;

    [SerializeField]
    float offsetFromCollision = 0.1f;

    [SerializeField]
    LayerMask collisionLayerMask = 1 << 31;

    [SerializeField]
    int rayNum = 3;

    [SerializeField]
    float noise = 0.03f;
    #endregion

    #region(Private members)
    bool isMoving_ = false;
    float distance_ = 2f;
    Quaternion rotation_ = Quaternion.identity;
    #endregion

    void Start()
    {
        var camera = Camera.main.transform;
        rotation_ = Quaternion.LookRotation(camera.forward, Vector3.up);
        distance_ = maxDistance;

        transform.position = camera.position + (rotation_ * Vector3.forward) * distance_;
        transform.rotation = Quaternion.LookRotation(-camera.forward, Vector3.up);
    }

    void LateUpdate()
    {
        var camera = Camera.main.transform;
        var cameraForward = camera.forward;
        var currentDir = rotation_ * Vector3.forward;

        if (!isMoving_) {
            var angle = Vector3.Angle(currentDir, cameraForward);
            if (Mathf.Abs(angle) > moveThresholdAngle) {
                isMoving_ = true;
            }
        } else {
            var cameraForwardRot = Quaternion.LookRotation(cameraForward, Vector3.up);
            rotation_ = Quaternion.Slerp(rotation_, cameraForwardRot, 1f - moveSmoothness);
            currentDir = rotation_ * Vector3.forward;
            if (Vector3.Dot(currentDir, cameraForward) > 0.999f) {
                isMoving_ = false;
            }
        }

        var targetRot = Quaternion.LookRotation(-cameraForward, Vector3.up);
        var targetDistance = distance_;

        if (checkCollision) {
            float averageDistance = 0f;
            var averageNormal = Vector3.zero;
            int hitNum = 0;
            for (int i = 0; i < rayNum; ++i) {
                RaycastHit hit;
                var noiseRadius = Random.Range(0f, noise) * radius;
                if (Physics.SphereCast(camera.position, radius + noiseRadius, currentDir, out hit, maxDistance, collisionLayerMask)) {
                    averageDistance += hit.distance + (radius + noiseRadius) - offsetFromCollision;
                    averageNormal += hit.normal;
                    ++hitNum;
                }
            }

            if (hitNum > 0) {
                averageDistance /= hitNum;
                averageNormal /= hitNum;
                targetDistance = averageDistance;

                var axis = Vector3.up;
                if (Mathf.Abs(Vector3.Dot(averageNormal, axis)) > 0.9f) {
                    axis = camera.up;
                }

                var averageNormalRot = Quaternion.LookRotation(averageNormal, axis);
                if (Quaternion.Angle(targetRot, averageNormalRot) < 45f) {
                    targetRot = averageNormalRot;
                }
            }
        }

        distance_ += (targetDistance - distance_) * 0.2f;
        if (distance_ < minDistance) distance_ = minDistance;

        transform.position = camera.position + (currentDir * distance_);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 1f - rotateSmoothness);
    }
}

}