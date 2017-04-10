using UnityEngine;
using UnityEngine.Assertions;

namespace Hecomi.HoloLensPlayground
{

public class ToggleEffectGenerator : MonoBehaviour 
{
    public GameObject effectPrefab;
    public MeshFilter effectMesh;

    public void OnClick(bool selected)
    {
        if (!selected) return;

        var obj = Instantiate(effectPrefab, transform.position, transform.rotation, transform);
        var effect = obj.GetComponent<ToggleEffect>();
        Assert.IsNotNull(effect);
        effect.mesh = effectMesh.sharedMesh;
    }
}

}