using UnityEngine;

namespace Hecomi.HoloLensPlayground
{

[RequireComponent(typeof(LineRenderer))]
public class XYLine : MonoBehaviour 
{
    LineRenderer line_;

    enum State
    {
        Drawing,
        Floating,
    }
    State state_ = State.Drawing;

    [SerializeField]
    Vector3 maxVelocity = new Vector3(0, 0.2f, 0f);

    [SerializeField]
    Vector3 maxAngularVelocity = new Vector3(30f, 30f, 30f);

    public Vector3 velocity { get; set; }
    public Vector3 angularVelocity { get; set; }

    Color startColor { get; set; }
    Color endColor { get; set; }

    float timer_ = 0f;

    [SerializeField]
    float duration = 10f;

    void Awake()
    {
        line_ = GetComponent<LineRenderer>();
        line_.positionCount = 0;
    }

    void Update()
    {
        switch (state_) {
            case State.Drawing:
                // wating for Add()...
                break;
            case State.Floating:
                UpdateFloating();
                break;
        }

    }

    void UpdateFloating()
    {
        transform.position += velocity * Time.deltaTime;
        transform.eulerAngles += angularVelocity * Time.deltaTime;

        timer_ += Time.deltaTime;
        {
            var color = startColor;
            color.a = 1f - timer_ / duration;;
            line_.startColor = color;
        }
        {
            var color = endColor;
            color.a = 1f - timer_ / duration;;
            line_.endColor = color;
        }
        if (timer_ >= duration) {
            Destroy(gameObject);
        }
    }

    public void Add(Vector3 pos)
    {
        var index = line_.positionCount;
        ++line_.positionCount;
        line_.SetPosition(index, pos);
    }

    public void Detach()
    {
        transform.parent = null;
        state_ = State.Floating;

        velocity = maxVelocity * Random.Range(0.1f, 1f);
        angularVelocity = maxAngularVelocity * Random.Range(-1f, 1f);

        startColor = line_.startColor;
        endColor = line_.endColor;
    }
}

}