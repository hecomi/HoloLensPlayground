using UnityEngine;


public class TapAndAddWindow : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;

    [SerializeField]
    float offset = 0f;

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
                var pos = hit.point + hit.normal * offset;
                var rot = Quaternion.LookRotation(hit.normal, Vector3.up);
                Instantiate(prefab, pos, rot, null);
            }
        }
    }
}
