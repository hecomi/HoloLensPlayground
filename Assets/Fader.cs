using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

namespace Hecomi.HoloLensPlayground
{

public class Fader : MonoBehaviour
{
    [SerializeField]
    Transform bar;

    [System.Serializable]
    class ValueChangeEvent : UnityEvent<float> {}

    [SerializeField]
    ValueChangeEvent onValueChanged = new ValueChangeEvent();

    private float value_ = 0f;
    public float value
    {
        get { return value_; }
        set
        {
            value_ = Mathf.Clamp(value, 0f, 1f);

            var s = bar.localScale;
            s.x = value_;
            bar.localScale = s;

            onValueChanged.Invoke(value_);
        }
    }

    void Start()
    {
        Assert.IsNotNull(bar, "should set bar.");
        value = 0f;
    }
}

}