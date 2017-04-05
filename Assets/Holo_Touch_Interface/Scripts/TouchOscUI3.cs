using UnityEngine;

namespace Hecomi.HoloLensPlayground
{

public class TouchOscUI3 : MonoBehaviour
{
    [Header("Toggle")]
    [SerializeField] Toggle toggle1;
    [SerializeField] Toggle toggle2;
    [SerializeField] Toggle toggle3;
    [SerializeField] Toggle toggle4;
    [SerializeField] XY xy;

    public void OnMessage(string[] address, Osc.Message msg)
    {
        var ui = address[1];
        var value = (float)msg.data[0];

        switch (ui) {
            case "toggle1" : toggle1.value = value; break;
            case "toggle2" : toggle2.value = value; break;
            case "toggle3" : toggle3.value = value; break;
            case "toggle4" : toggle4.value = value; break;
            case "xy"      : 
                xy.value = new Vector2((float)msg.data[0], (float)msg.data[1]); 
                break;
        }
    }
}

}