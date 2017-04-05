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
    class ValueChangeEvent : UnityEvent<bool> {}

    [SerializeField]
    ValueChangeEvent onValueChanged = new ValueChangeEvent();

    private float value_;
    public float value
    {
        get { return value_; }
        set
        {
            value_ = value;
            bool isTrue = value_ > 0;
            toggleInner.SetActive(isTrue);
            onValueChanged.Invoke(isTrue);
        }
    }

    void Start()
    {
        Assert.IsNotNull(toggleInner);
        value = 0;
    }
}

}