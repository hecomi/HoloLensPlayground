using UnityEngine;
using UnityEngine.Assertions;

namespace Hecomi.HoloLensPlayground
{

public class TimingNote : MonoBehaviour 
{
    const float IGNORE_RATIO = 0.15f;
    const float SUCCESS_RATIO = 0.05f;

    [SerializeField]
    GameObject touchEffectPrefab;

    [SerializeField]
    GameObject touchEffectPrefab2;

    public Vector3 from { get; set; }
    public Vector3 to { get; set; }
    public Color failColor { get; set; }
    public Color successColor { get; set; }
    public float speed { get; set; }
    public Vector3 dir { get { return (to - from).normalized; } }
    public float distance { get { return (to - transform.localPosition).magnitude; } }
    public Push push { get; set; }

    Material material_;
    float initDistance_;
    float stateTimer_ = 0f;

    public float ratio
    {
        get { return distance / initDistance_; }
    }

    int tintColor_;

    public Color color
    {
        get { return material_.GetColor(tintColor_); }
        set { material_.SetColor(tintColor_, value); }
    }

    public float alpha
    {
        get { return color.a; }
        set { color = new Color(color.r, color.g, color.b, value); }
    }

    enum State
    {
        Move,
        Wait,
        Fail,
        Success,
    }
    State state_ = State.Move;

    void Awake()
    {
        material_ = GetComponent<Renderer>().material; // clone
        tintColor_ = Shader.PropertyToID("_TintColor");
    }

    void Start()
    {
        Assert.IsNotNull(push);

        initDistance_ = (to - from).magnitude;

        push.onPressed.AddListener(OnPressed);
        push.onReleased.AddListener(OnReleased);
    }

    void OnDestroy()
    {
        push.onPressed.RemoveListener(OnPressed);
        push.onReleased.RemoveListener(OnReleased);
    }

    void Update()
    {
        UpdateState();
    }

    void UpdateState()
    {
        switch (state_) {
            case State.Move: {
                UpdateMove();
                break;
            }
            case State.Wait: {
                UpdateWait();
                break;
            }
            case State.Fail: {
                UpdateFail();
                break;
            }
            case State.Success: {
                UpdateSuccess();
                break;
            }
        }
        stateTimer_ += Time.deltaTime;
    }

    void UpdateMove()
    {
        var thresh = 0.2f;
        var baseA = 0.5f;
        if (ratio < thresh) {
            alpha = baseA + 0.8f * Mathf.Pow((thresh - ratio) / thresh, 2f);
        } else {
            alpha = baseA;
        }

        transform.localPosition += dir * speed * Time.deltaTime;
        if (transform.localPosition.y <= 0.0f) {
            Wait();
        }
    }

    void UpdateWait()
    {
        if (stateTimer_ > 0.2f) {
            Fail();
        }
    }

    void UpdateFail()
    {
        var a = Mathf.Min(1f - stateTimer_ / 0.5f, 1f);
        if (a <= 0f) Destroy(gameObject);
        alpha = a;
    }

    void UpdateSuccess()
    {
        var a = Mathf.Min(1f - stateTimer_ / 0.5f, 1f);
        if (a <= 0f) Destroy(gameObject);
        alpha = a;
    }

    void OnPressed()
    {
        if (ratio > IGNORE_RATIO) return;

        if (ratio < SUCCESS_RATIO) {
            Success();
        } else {
            Fail();
        }
    }

    void OnReleased()
    {
        // nothing to do...
    }

    bool IsMoveOrWait()
    {
        return state_ == State.Move || state_ == State.Wait;
    }

    void Wait()
    {
        if (state_ != State.Move) return;

        state_ = State.Wait;
        stateTimer_ = 0f;
    }

    void Fail()
    {
        if (!IsMoveOrWait()) return;

        state_ = State.Fail;
        stateTimer_ = 0f;
        color = failColor;

        //GenerateTouchEffect(touchEffectPrefab, failColor);
    }

    void Success()
    {
        if (!IsMoveOrWait()) return;

        state_ = State.Success;
        stateTimer_ = 0f;
        color = successColor;

        GenerateTouchEffect(touchEffectPrefab, successColor);
        //GenerateTouchEffect(touchEffectPrefab2, successColor);
    }

    void GenerateTouchEffect(GameObject prefab, Color color)
    {
        var obj = Instantiate(prefab, push.transform);
        obj.transform.localPosition = new Vector3(0f, 0.005f, 0f);
        obj.transform.localRotation = Quaternion.identity;

        var ps = obj.GetComponent<ParticleSystem>();
        Assert.IsNotNull(ps);
        var main = ps.main;
        main.startColor = color;

        var collision = ps.collision;
        collision.SetPlane(0, push.transform);

        Destroy(obj, ps.main.startLifetime.constantMax + ps.main.duration);
    }
}

}