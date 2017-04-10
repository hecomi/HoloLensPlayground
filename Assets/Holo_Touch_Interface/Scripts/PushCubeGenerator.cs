using UnityEngine;

namespace Hecomi.HoloLensPlayground
{

[RequireComponent(typeof(Push))]
public class PushCubeGenerator : MonoBehaviour 
{
    [SerializeField]
    Transform cubeBase;

    [SerializeField]
    GameObject cubePrefab;

    [SerializeField]
    float scaleSpeed = 0.005f;

    [SerializeField]
    float force = 200f;

    [SerializeField]
    float torque = 200f;

    GameObject generatedCube_;
    Vector3 scale_;

    public void OnPressed()
    {
        GenerateCube();
    }

    public void OnReleased()
    {
        ReleaseCube();
    }

    void Start()
    {
        if (!cubeBase) {
            cubeBase = transform.GetChild(0);
        }

        var push = GetComponent<Push>();
        push.onPressed.AddListener(OnPressed);
        push.onReleased.AddListener(OnReleased);
    }

    void Update()
    {
        if (generatedCube_) {
            UpdateCube();
        }
    }

	void GenerateCube() 
	{
        var meshFilter = cubeBase.GetComponent<MeshFilter>();
        if (!meshFilter) return;

        generatedCube_ = Instantiate(cubePrefab, transform);
        generatedCube_.transform.localPosition = Vector3.zero;
        generatedCube_.transform.localRotation = Quaternion.identity;

        var ex = meshFilter.sharedMesh.bounds.extents * 2;
        var s = transform.localScale;
        scale_ = new Vector3(s.x * ex.x, s.y * ex.y, s.z * ex.z);
        generatedCube_.transform.localScale = scale_;

        var rb = generatedCube_.GetComponent<Rigidbody>();
        if (rb) {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
	}

    void UpdateCube()
    {
        scale_.y += scaleSpeed * Time.deltaTime;
        generatedCube_.transform.localScale = scale_;
        generatedCube_.transform.localPosition = new Vector3(0f, scale_.y * 0.5f, 0f);
    }

    void ReleaseCube()
    {
        if (!generatedCube_) return;

        generatedCube_.transform.SetParent(null);
        Destroy(generatedCube_, 10f);

        var rb = generatedCube_.GetComponent<Rigidbody>();
        if (rb) {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce((Vector3.up + Random.insideUnitSphere * 0.1f) * force);
            rb.AddTorque(Random.insideUnitSphere * torque);
        }

        generatedCube_ = null;
    }
}

}