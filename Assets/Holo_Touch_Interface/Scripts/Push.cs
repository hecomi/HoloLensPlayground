using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

namespace Hecomi.HoloLensPlayground
{

public class Push : MonoBehaviour
{
    [SerializeField]
    GameObject push;

    public UnityEvent onPressed = new UnityEvent();
    public UnityEvent onReleased = new UnityEvent();

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