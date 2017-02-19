using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class TapAndFireCircleEffect : MonoBehaviour
{
    void OnEnable()
    {
        InteractionManager.SourcePressed += OnSourcePressed;
    }

    void OnDisable()
    {
        InteractionManager.SourcePressed -= OnSourcePressed;
    }

    void OnSourcePressed(InteractionSourceState state)
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
