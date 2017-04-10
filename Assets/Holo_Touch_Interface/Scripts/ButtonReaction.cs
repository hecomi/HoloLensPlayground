using UnityEngine;

namespace Hecomi.HoloLensPlayground
{

public class ButtonReaction : MonoBehaviour 
{
    [SerializeField]
    float amp = 0.01f;

    [SerializeField]
    float freq = 3f;

    [SerializeField]
    float dampingTime = 0.5f;

    [SerializeField]
    Transform target;

    float t_ = 100f;

    enum State
    {
        Idle,
        Pushed,
    }
    State state = State.Idle;

    void Awake()
    {
        if (target == null) {
            target = transform;
        }
    }

	void Update() 
	{
        if (state == State.Idle) return;

		t_ += Time.deltaTime;
        var a = amp * Mathf.Exp(- t_ / dampingTime);
        var y = -a * Mathf.Sin(2 * Mathf.PI * freq * t_);

        var p = target.localPosition;
        p.y = y;
        target.localPosition = p;

        if (a < Mathf.Epsilon) {
            state = State.Idle;
        }
	}

    protected void OnSelected()
    {
        t_ = 0f;
        state = State.Pushed;
    }
}

}