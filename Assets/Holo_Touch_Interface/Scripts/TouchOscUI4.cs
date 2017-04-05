using UnityEngine;

namespace Hecomi.HoloLensPlayground
{

public class TouchOscUI4 : MonoBehaviour
{
    [Header("Toggle")]
    [SerializeField] Toggle toggle1;
    [SerializeField] Toggle toggle2;
    [SerializeField] Toggle toggle3;
    [SerializeField] Toggle toggle4;
    [SerializeField] MultiToggle multitoggle;

    public void OnMessage(string[] address, Osc.Message msg)
    {
        var ui = address[1];
        var value = (float)msg.data[0];

        switch (ui) {
            case "toggle1" : toggle1.value = value; break;
            case "toggle2" : toggle2.value = value; break;
            case "toggle3" : toggle3.value = value; break;
            case "toggle4" : toggle4.value = value; break;
            case "multitoggle" :
                OnMultiToggleChanged(address, value); 
                break;
        }
    }

    void OnMultiToggleChanged(string[] address, float value)
    {
        if (address.Length < 4) return;

        var y = int.Parse(address[2]) - 1;
        var x = int.Parse(address[3]) - 1;
        multitoggle[y, x] = value;
    }
}

}