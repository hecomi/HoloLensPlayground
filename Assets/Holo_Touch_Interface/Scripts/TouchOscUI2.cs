using UnityEngine;

namespace Hecomi.HoloLensPlayground
{

public class TouchOscUI2 : MonoBehaviour
{
    [Header("Push")]
    [SerializeField] Push push1;
    [SerializeField] Push push2;
    [SerializeField] Push push3;
    [SerializeField] Push push4;
    [SerializeField] Push push5;
    [SerializeField] Push push6;
    [SerializeField] Push push7;
    [SerializeField] Push push8;
    [SerializeField] Push push9;
    [SerializeField] Push push10;
    [SerializeField] Push push11;
    [SerializeField] Push push12;
    [SerializeField] Push push13;
    [SerializeField] Push push14;
    [SerializeField] Push push15;
    [SerializeField] Push push16;

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
            case "push1"  : push1.value  = value; break;
            case "push2"  : push2.value  = value; break;
            case "push3"  : push3.value  = value; break;
            case "push4"  : push4.value  = value; break;
            case "push5"  : push5.value  = value; break;
            case "push6"  : push6.value  = value; break;
            case "push7"  : push7.value  = value; break;
            case "push8"  : push8.value  = value; break;
            case "push9"  : push9.value  = value; break;
            case "push10"  : push10.value  = value; break;
            case "push11"  : push11.value  = value; break;
            case "push12"  : push12.value  = value; break;
            case "push13"  : push13.value  = value; break;
            case "push14"  : push14.value  = value; break;
            case "push15"  : push15.value  = value; break;
            case "push16"  : push16.value  = value; break;
            case "toggle1" : toggle1.value = value; break;
            case "toggle2" : toggle2.value = value; break;
            case "toggle3" : toggle3.value = value; break;
            case "toggle4" : toggle4.value = value; break;
        }
    }

    public Push GetPush(int n)
    {
        switch (n)
        {
            case 1: return push1;
            case 2: return push2;
            case 3: return push3;
            case 4: return push4;
            case 5: return push5;
            case 6: return push6;
            case 7: return push7;
            case 8: return push8;
            case 9: return push9;
            case 10: return push10;
            case 11: return push11;
            case 12: return push12;
            case 13: return push13;
            case 14: return push14;
            case 15: return push15;
            case 16: return push16;
        }

        return null;
    }
}

}