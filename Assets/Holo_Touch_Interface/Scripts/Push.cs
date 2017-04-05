using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

namespace Hecomi.HoloLensPlayground
{

public class Push : MonoBehaviour
{
    [SerializeField]
    GameObject push;

    [SerializeField]
    UnityEvent onPressed = new UnityEvent();

    [SerializeField]
    UnityEvent onReleased = new UnityEvent();

    private float value_;
    public float value
    {
        get { return value_; }
        set
        {
            value_ = value;
            bool isTrue = value_ > Mathf.Epsilon;
            push.SetActive(isTrue);

            if (isTrue) {
                onPressed.Invoke();
            } else {
                onReleased.Invoke();
            }
        }
    }

    void Start()
    {
        Assert.IsNotNull(push);
        value = 0;
    }
}

}