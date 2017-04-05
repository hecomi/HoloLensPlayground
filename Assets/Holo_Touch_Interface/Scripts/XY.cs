using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

namespace Hecomi.HoloLensPlayground
{

public class XY : MonoBehaviour
{
    [SerializeField]
    Transform barX;

    [SerializeField]
    Transform barY;

    [SerializeField]
    Transform cursor;

    [SerializeField]
    float xRange = 0.1f;

    [SerializeField]
    float yRange = 0.1f;

    [System.Serializable]
    class ValueChangeEvent : UnityEvent<Vector2> {}

    [SerializeField]
    ValueChangeEvent onValueChanged = new ValueChangeEvent();

    private Vector2 value_ = Vector2.zero;
    public Vector2 value
    {
        get { return value_; }
        set
        {
            value_ = Vector2.Min(Vector2.Max(value, Vector2.zero), Vector2.one);

            var barXPos = barX.localPosition;
            barXPos.z = (value_.y - 0.5f) * xRange;
            barX.localPosition = barXPos;

            var barYPos = barY.localPosition;
            barYPos.x = (0.5f - value_.x) * yRange;
            barY.localPosition = barYPos;

            var cursorPos = cursor.localPosition;
            cursorPos.x = barYPos.x;
            cursorPos.z = barXPos.z;
            cursor.localPosition = cursorPos;

            onValueChanged.Invoke(value_);
        }
    }

    void Start()
    {
        Assert.IsNotNull(barX, "should set barX.");
        Assert.IsNotNull(barY, "should set barY.");
        Assert.IsNotNull(cursor, "should set cursor.");
        value = new Vector2(0.5f, 0.5f);
    }
}

}