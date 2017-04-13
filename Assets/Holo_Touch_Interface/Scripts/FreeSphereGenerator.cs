using UnityEngine;
using UnityEngine.Assertions;

namespace Hecomi.HoloLensPlayground
{

public class FreeSphereGenerator : MonoBehaviour 
{
    [SerializeField]
    GameObject spherePrefab;

    [SerializeField]
    Transform spawnLocator;

    [SerializeField]
    float interval = 5f;

    [SerializeField]
    float sphereLifeTime = 15f;

    float timer_ = 0;

    void Start()
    {
        Assert.IsNotNull(spherePrefab);
    }

	void Update() 
	{
        timer_ += Time.deltaTime;
        if (timer_ >= interval) {
            timer_ -= interval;
            Generate();
        }
	}

    void Generate()
    {
        var sphere = Instantiate(spherePrefab, spawnLocator.position, spawnLocator.rotation, null);
        Destroy(sphere, sphereLifeTime);

        var rb = sphere.GetComponent<Rigidbody>();
        if (rb) {
            rb.AddTorque(Random.insideUnitSphere * 10f);
            rb.AddForce(Random.insideUnitSphere * 1f);
        }
    }
}

}