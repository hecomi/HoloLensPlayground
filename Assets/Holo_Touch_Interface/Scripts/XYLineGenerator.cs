using UnityEngine;
using UnityEngine.Assertions;

namespace Hecomi.HoloLensPlayground
{

[RequireComponent(typeof(XY))]
public class XYLineGenerator : MonoBehaviour 
{
    [SerializeField]
    GameObject linePrefab;

    [SerializeField]
    Transform cursor;

    [SerializeField]
    float timeout = 0.5f;

    XYLine line_;
    bool touched_ = false;
    float touchTimer_ = 0f;

    void Start()
    {
        GetComponent<XY>().onValueChanged.AddListener(OnValueChanged);
    }

    void Update()
    {
        DetectTimeout();
    }

    void DetectTimeout()
    {
        if (!touched_) return;

        touchTimer_ += Time.deltaTime;
        if (touchTimer_ > timeout) {
            touched_ = false;
            OnTouchEnd();
        }
    }

    public void OnValueChanged(Vector2 value)
    {
        touchTimer_ = 0f;

        if (!touched_) {
            touched_ = true;
            OnTouchStart();
        } else {
            OnTouchMove();
        }
    }

    void OnTouchStart()
    {
        var obj = Instantiate(linePrefab, transform.position, transform.rotation, transform);
        line_ = obj.GetComponent<XYLine>();
        Assert.IsNotNull(line_);
        line_.Add(cursor.localPosition);
    }

    void OnTouchMove()
    {
        if (!line_) return;
        line_.Add(cursor.localPosition);
    }

    void OnTouchEnd()
    {
        if (!line_) return;
        line_.Detach();
    }
}

}