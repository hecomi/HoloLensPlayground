using UnityEngine;

namespace Hecomi.HoloLensPlayground
{

[RequireComponent(typeof(Push))]
public class PushReaction : ButtonReaction
{
    void Start()
    {
        GetComponent<Push>().onPressed.AddListener(OnSelected);
    }
}

}