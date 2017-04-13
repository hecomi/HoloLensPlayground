using UnityEngine;
using UnityEngine.Assertions;

namespace Hecomi.HoloLensPlayground
{

[RequireComponent(typeof(MeshFilter))]
public class MultiToggleCube : MonoBehaviour 
{
    [SerializeField]
    GameObject cubePrefab;

    [SerializeField]
    float height = 0.1f;

    GameObject cube_;
    float currentHeight = 0f;

    float scaleY
    {
        set
        {
            currentHeight = value;

            var scale = cube_.transform.localScale;
            scale.y = value;
            cube_.transform.localScale = scale;
        }
    }

    void Start()
    {
        Generate();
    }

    void Update()
    {
        scaleY = currentHeight + (height - currentHeight) * 0.2f;
        cube_.transform.localPosition = new Vector3(0f, currentHeight * 0.5f, 0f);
    }

    void OnEnable()
    {
        if (!cube_) return;
        scaleY = 0f;
    }

    void OnDisable()
    {
        if (!cube_) return;
        scaleY = 0f;
    }

    void Generate()
    {
        Assert.IsNotNull(cubePrefab);
        cube_ = Instantiate(cubePrefab, transform.position, transform.rotation, transform);

        var ex = GetComponent<MeshFilter>().sharedMesh.bounds.extents * 2;
        var s = transform.localScale;
        cube_.transform.localScale = new Vector3(s.x * ex.x, height, s.z * ex.z);
        cube_.transform.localPosition = new Vector3(0f, height * 0.5f, 0f);
    }
}

}