using UnityEngine;

namespace Hecomi.HoloLensPlayground
{

public class ToggleEffect : MonoBehaviour 
{
    public Mesh mesh
    {
        get { return GetComponent<ParticleSystemRenderer>().mesh; }
        set { GetComponent<ParticleSystemRenderer>().mesh = value; }
    }

	void Start() 
	{
		var particle = GetComponent<ParticleSystem>();
		var lifeTime = particle.main.startLifetime.constant + particle.main.duration;
		Destroy(gameObject, lifeTime);
	}
}

}