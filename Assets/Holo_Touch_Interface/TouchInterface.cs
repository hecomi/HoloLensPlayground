using UnityEngine;
using UnityEngine.Assertions;
using System;

namespace Hecomi.HoloLensPlayground
{

public class TouchInterface : MonoBehaviour
{
    [Header("ui")]
    [SerializeField] GameObject ui1;
    [SerializeField] GameObject ui2;
    [SerializeField] GameObject ui3;
    [SerializeField] GameObject ui4;

    [Header("menu")]
    [SerializeField] Toggle menu1;
    [SerializeField] Toggle menu2;
    [SerializeField] Toggle menu3;
    [SerializeField] Toggle menu4;

    TouchOscUI1 uiView1_;
    TouchOscUI2 uiView2_;

    void Start()
    {
        uiView1_ = ui1.GetComponent<TouchOscUI1>();
        Assert.IsNotNull(uiView1_, "TouchOscUI1 was not found.");

        uiView2_ = ui2.GetComponent<TouchOscUI2>();
        Assert.IsNotNull(uiView2_, "TouchOscUI2 was not found.");

        menu1.value = 0f;
        menu2.value = 0f;
        menu3.value = 0f;
        menu4.value = 0f;
    }

    public void OnMessage(Osc.Message msg)
    {
        var address = msg.path.Substring(1).Split(new char[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
        if (address.Length == 0) return;

        var menu = address[0];
        bool isMenu1 = menu == "1";
        bool isMenu2 = menu == "2";
        bool isMenu3 = menu == "3";
        bool isMenu4 = menu == "4";

        ui1.SetActive(isMenu1);
        ui2.SetActive(isMenu2);
        ui3.SetActive(isMenu3);
        ui4.SetActive(isMenu4);

        menu1.value = isMenu1 ? 1f : 0f;
        menu2.value = isMenu2 ? 1f : 0f;
        menu3.value = isMenu3 ? 1f : 0f;
        menu4.value = isMenu4 ? 1f : 0f;

        if (address.Length == 1) return;

        Assert.IsTrue(msg.data.Length > 0);

        var ui = address[1];
        var value = (float)msg.data[0];

        if      (menu == "1") uiView1_.OnMessage(ui, value);
        else if (menu == "2") uiView2_.OnMessage(ui, value);
    }
}

}