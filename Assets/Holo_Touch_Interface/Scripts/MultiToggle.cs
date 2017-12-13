using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

namespace Hecomi.HoloLensPlayground
{

public class MultiToggle : MonoBehaviour
{
    float[,] values_ = new float[8, 8];
    GameObject[,] objects_ = new GameObject[8, 8];

    [System.Serializable]
    class ValueChangeEvent : UnityEvent<int, int, bool> {}

    [SerializeField]
    ValueChangeEvent onValueChanged = new ValueChangeEvent();

    public float this[int y, int x]
    {
        get
        {
            return values_[y, x];
        }
        set
        {
            values_[y, x] = value;
            bool isTrue = value > 0;
            objects_[y, x].SetActive(isTrue);
            onValueChanged.Invoke(x, y, isTrue);
        }
    }

    void Start()
    {
        for (int y = 0; y < 8; ++y) {
            for (int x = 0; x < 8; ++x) {
                var name = string.Format("pToggle{0}_{1}", y + 1, x + 1);
                var obj = transform.Find(name);
                Assert.IsNotNull(obj);
                objects_[y, x] = obj.gameObject;
                this[y, x] = 0f;
            }
        }
    }
}

}