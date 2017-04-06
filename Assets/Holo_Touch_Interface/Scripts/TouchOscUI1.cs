using UnityEngine;

namespace Hecomi.HoloLensPlayground
{

public class TouchOscUI1 : MonoBehaviour
{
    [Header("Fader")]
    [SerializeField] Fader fader1;
    [SerializeField] Fader fader2;
    [SerializeField] Fader fader3;
    [SerializeField] Fader fader4;
    [SerializeField] Fader fader5;

    [Header("Toggle")]
    [SerializeField] Toggle toggle1;
    [SerializeField] Toggle toggle2;
    [SerializeField] Toggle toggle3;
    [SerializeField] Toggle toggle4;

    public void OnMessage(string[] address, Osc.Message msg)
    {
        var ui = address[1];
        var value = (float)msg.data[0];

        switch (ui) {
            case "fader1"  : fader1.value  = value; break;
            case "fader2"  : fader2.value  = value; break;
            case "fader3"  : fader3.value  = value; break;
            case "fader4"  : fader4.value  = value; break;
            case "fader5"  : fader5.value  = value; break;
            case "toggle1" : toggle1.value = value; break;
            case "toggle2" : toggle2.value = value; break;
            case "toggle3" : toggle3.value = value; break;
            case "toggle4" : toggle4.value = value; break;
        }
    }
}

}