using UnityEngine;

namespace Hecomi.HoloLensPlayground
{

public class TouchOscUI1 : MonoBehaviour
{
    [Header("Fader")]
    [SerializeField] Fader fader1_;
    [SerializeField] Fader fader2_;
    [SerializeField] Fader fader3_;
    [SerializeField] Fader fader4_;
    [SerializeField] Fader fader5_;

    [Header("Toggle")]
    [SerializeField] Toggle toggle1_;
    [SerializeField] Toggle toggle2_;
    [SerializeField] Toggle toggle3_;
    [SerializeField] Toggle toggle4_;

    public void OnMessage(string[] address, Osc.Message msg)
    {
        var ui = address[1];
        var value = (float)msg.data[0];

        switch (ui) {
            case "fader1"  : fader1_.value  = value; break;
            case "fader2"  : fader2_.value  = value; break;
            case "fader3"  : fader3_.value  = value; break;
            case "fader4"  : fader4_.value  = value; break;
            case "fader5"  : fader5_.value  = value; break;
            case "toggle1" : toggle1_.value = value; break;
            case "toggle2" : toggle2_.value = value; break;
            case "toggle3" : toggle3_.value = value; break;
            case "toggle4" : toggle4_.value = value; break;
        }
    }
}

}