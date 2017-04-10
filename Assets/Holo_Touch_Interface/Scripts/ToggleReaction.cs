using UnityEngine;

namespace Hecomi.HoloLensPlayground
{

[RequireComponent(typeof(Toggle))]
public class ToggleReaction : ButtonReaction
{
	void Start() 
	{
        GetComponent<Toggle>().onValueChanged.AddListener(OnValueChanged);
	}

    void OnValueChanged(bool value)
    {
        if (value) {
            OnSelected();
        }
    }
}

}