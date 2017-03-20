using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.SpatialMapping;

[RequireComponent(typeof(SpatialMappingManager))]
public class SpatialMeshModifier : Singleton<SpatialMeshModifier>
{
    Material material_;

    int centerKey_;
    int radiusKey_;
    int widthKey_;

    public float width = 0.1f;
    public float speed = 1f;

    Vector3 center
    {
        get;
        set;
    }

    float time 
    { 
        get; 
        set;
    }

    float radius
    {
        get { return speed * time; }
    }

    void Start()
    {
        material_ = GetComponent<SpatialMappingManager>().SurfaceMaterial;

        centerKey_ = Shader.PropertyToID("_Center");
        radiusKey_ = Shader.PropertyToID("_Radius");
        widthKey_ = Shader.PropertyToID("_Width");

        center = -9999f * Vector3.one;
    }

    void Update()
    {
        material_.SetVector(centerKey_, center);
        material_.SetFloat(radiusKey_, radius);
        material_.SetFloat(widthKey_, width);

        time += Time.deltaTime;
    }

    public void Fire(Vector3 pos)
    {
        center = pos;
        time = 0f;
    }

    [ContextMenu("Test")]
    void Test()
    {
        Fire(Camera.main.transform.position);
    }
}
