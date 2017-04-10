using UnityEngine;
using UnityEngine.Assertions;

namespace Hecomi.HoloLensPlayground
{

[RequireComponent(typeof(Fader))]
public class FaderCube : MonoBehaviour 
{
    public enum Direction { X, Z }

    [SerializeField]
    Direction direction = Direction.X;

    [SerializeField]
    Transform bar;

    [SerializeField]
    GameObject cubePrefab;

    [SerializeField]
    float maxHeight = 0.1f;

    GameObject cube_;
    Vector3 extents_;

    void Start()
    {
        var fader = GetComponent<Fader>();
        Assert.IsNotNull(fader);
        fader.onValueChanged.AddListener(OnValueChanged);

        GenerateCube();
    }

    void GenerateCube()
    {
        var meshFilter = bar.GetChild(0).GetComponent<MeshFilter>();
        if (!meshFilter) return;

        cube_ = Instantiate(cubePrefab, bar.position, bar.rotation, transform);
        extents_ = meshFilter.sharedMesh.bounds.extents * 2;
        OnValueChanged(bar.localScale.x);
    }

    public void OnValueChanged(float value)
    {
        if (!cube_) return;

        var s = bar.localScale;
        var w = s.x * extents_.x;
        var h = value * maxHeight;
        var d = s.z * extents_.z;
        cube_.transform.localScale = new Vector3(w, h, d);

        if (direction == Direction.X) {
            cube_.transform.localPosition = new Vector3(-w / 2, h / 2, 0);
        } else {
            cube_.transform.localPosition = new Vector3(0, h / 2, -w / 2);
        }
    }
}

}