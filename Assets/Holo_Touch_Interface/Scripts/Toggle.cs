using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

namespace Hecomi.HoloLensPlayground
{

public class Toggle : MonoBehaviour
{
    [SerializeField]
    GameObject toggleInner;

    [System.Serializable]
    public class ValueChangeEvent : UnityEvent<bool> {}

    public ValueChangeEvent onValueChanged = new ValueChangeEvent();

    private float value_;
    public float value
    {
        get { return value_; }
        set
        {
            bool isChanged = (value_ != value);
            value_ = value;
            bool isTrue = value_ > 0;
            toggleInner.SetActive(isTrue);
            if (isChanged) {
                onValueChanged.Invoke(isTrue);
            }
        }
    }

    void Start()
    {
        Assert.IsNotNull(toggleInner);
        value = 0;
    }
}

}