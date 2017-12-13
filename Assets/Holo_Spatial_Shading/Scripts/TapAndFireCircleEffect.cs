using UnityEngine;


public class TapAndFireCircleEffect : MonoBehaviour
{
    void OnEnable()
    {
        UnityEngine.XR.WSA.Input.InteractionManager.InteractionSourcePressed += OnSourcePressed;
    }

    void OnDisable()
    {
        UnityEngine.XR.WSA.Input.InteractionManager.InteractionSourcePressed -= OnSourcePressed;
    }

    void OnSourcePressed(UnityEngine.XR.WSA.Input.InteractionSourcePressedEventArgs state)
    {
        var from = Camera.main.transform.position;
        var to = Camera.main.transform.forward;

        RaycastHit hit;
        if (Physics.Raycast(from, to, out hit, 10f)) {
            if (hit.collider.gameObject.layer == 31) {
                SpatialMeshModifier.Instance.Fire(hit.point);
            }
        }
    }
}
